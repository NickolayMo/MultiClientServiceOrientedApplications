namespace CarRental.Web.Models
{
    public class AccountLoginModel
    {
        public string ReturnUrl { get; set; }
        public string LoginEmail { get; set; }
        public bool RememberMe { get; set; }
        public string Password { get; set; }
    }
}