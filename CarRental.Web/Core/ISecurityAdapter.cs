namespace CarRental.Web.Services
{
    public interface ISecurityAdapter
    {
        bool ChangePassword(string loginEmail, string oldPassword, string newPassword);
        void Initialize();
        bool Login(string loginEmail, string password, bool rememberMe);
        void Register(string loginEmail, string password, object propertyValues);
        bool UserExists(string loginEmail);
    }
}