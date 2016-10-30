using CarRental.Client.Contracts.ServiceContracts;
using CarRental.Web.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Core.Common.Contracts;
using CarRental.Client.Entities;
using CarRental.Web.Models;

namespace CarRental.Web.Controllers.API
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [RoutePrefix("api/reservation")]
    [Authorize]
    public class ReservationApiController : ApiControllerBase
    {
        private IInventoryService _InventoryService;
        private IRentalService _RentalService;

        [ImportingConstructor]
        public ReservationApiController(IInventoryService iventoryservice, IRentalService rentalService)
        {
            _InventoryService = iventoryservice;
            _RentalService = rentalService;
        }
        protected override void RegisterServices(List<IServiceContract> disposableServices)
        {
            disposableServices.Add(_InventoryService);
        }

        [HttpGet]
        [Route("availablecars/{pickupDate}/{returnDate}")]
        public HttpResponseMessage GetAvailableCars(HttpRequestMessage request, DateTime pickupDate, DateTime returnDate)
        {
            return GetHttpResponse(request, ()=> {
                HttpResponseMessage response = null;
                Car[] cars = _InventoryService.GetAllAvailableCars(pickupDate, returnDate);
                response = request.CreateResponse<Car[]>(HttpStatusCode.OK, cars);
                return response;
            });
           
        }
        [HttpPost]
        [Route("reservecar")]
        public HttpResponseMessage ReserveCar(HttpRequestMessage request, [FromBody] ReservationModel model)
        {
            return GetHttpResponse(request, () => {
                HttpResponseMessage response = null;
                string user = User.Identity.Name;
                Reservation reservation =  _RentalService.MakeReservation(user, model.CarId, model.PickupDate, model.ReturnDate);
                response = request.CreateResponse<Reservation>(HttpStatusCode.OK, reservation);
                return response;
            });
        }

    }
}
