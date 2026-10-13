namespace MemberShip.Application.IEmailsender
{
    public interface IEmailsender
    {
        void SendEmail(string email, string subject, string message);
    }
}
