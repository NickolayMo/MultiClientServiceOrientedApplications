using CarRental.Business.Managers.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM = System.ServiceModel;
using System.Timers;
using CarRental.Business.Entities;
using System.Transactions;
using System.Security.Principal;
using CarRental.Common;
using System.Threading;
using Core.Common.Core;
using CarRental.Business.Bootstrapper;

namespace CarRental.ServiceHost.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            GenericPrincipal principal = new GenericPrincipal(
                new GenericIdentity("Kolya"), new string[] {Security.CAR_RENTAL_ADMIN}
                );
            Thread.CurrentPrincipal = principal;

            ObjectBase.Container = MEFLoader.Init();

            System.Console.WriteLine("Starting up service");
            System.Console.WriteLine("");
           
            SM.ServiceHost inventoryManagerHost = new SM.ServiceHost(typeof(InventoryManager));
            StartService(inventoryManagerHost, "InventoryManager");

            SM.ServiceHost accountManagerHost = new SM.ServiceHost(typeof(AccountManager));
            StartService(accountManagerHost, "AccountManager");

            SM.ServiceHost rentalManagerHost = new SM.ServiceHost(typeof(RentalManager));
            StartService(rentalManagerHost, "RentalManager");
            System.Timers.Timer timer = new System.Timers.Timer(100000);
            timer.Elapsed += OnTimerElapsed;
            timer.Start();
            
            System.Console.WriteLine("");
            System.Console.WriteLine("Press [Enter] to exit...");
            System.Console.ReadLine();
            timer.Stop();

            System.Console.WriteLine("Reservation monitor started");

            StopService(inventoryManagerHost, "InventoryManager");
            StopService(accountManagerHost, "AccountManager");
            StopService(rentalManagerHost, "RentalManager");
        }

        private static void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            System.Console.WriteLine("Looking for dead reservation started at " + DateTime.Now.ToString());
            RentalManager rentalManager = new RentalManager();
            Reservation[] reservations = rentalManager.GetDeadReservations();
            if (reservations != null)
            {
                foreach (var reservation in reservations)
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        try
                        {
                            rentalManager.CancelReservation(reservation.ReservationId);
                            System.Console.WriteLine("Canceling reservation  " + reservation.ReservationId);
                            scope.Complete();
                        }
                        catch(Exception ex)
                        {
                            System.Console.WriteLine($"There was an exception {ex.Message} when attempting cancel reservation");
                        }
                    }
                }
            }
        }

        private static void StartService(SM.ServiceHost host, string serviceDescription)
        {
            host.Open();
            System.Console.WriteLine($"service {serviceDescription} started");
            foreach (var endpoints in host.Description.Endpoints)
            {
                System.Console.WriteLine("Listening of endpoint:");
                System.Console.WriteLine($" address: {endpoints.Address.Uri.ToString()}");
                System.Console.WriteLine($" binding: {endpoints.Binding}");
                System.Console.WriteLine($" contract: {endpoints.Contract.ConfigurationName}");
            }
            System.Console.WriteLine();
        }
        private static void StopService(SM.ServiceHost host, string serviceDescription)
        {
            host.Close();

            System.Console.WriteLine($"service {serviceDescription} stoped");
        }
    }
}
