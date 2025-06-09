using Company.DAL.Models;

namespace Company_MVC.Helpers
{
    public interface IMailingService
    {

        public void SendEmail(Email email);

    }
}
