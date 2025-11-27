using System.Net.Mail;

namespace TicketManagementSystem.API.Services
{
    public interface IEmailValidationService
    {
        bool IsValidEmail(string email);
        bool IsValidEmail(string email, IEnumerable<string> allowedDomains);
    }

    public class EmailValidationService : BaseService, IEmailValidationService
    {
        public EmailValidationService(ILogger<EmailValidationService> logger) : base(logger)
        {
        }

        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var mailAddress = new MailAddress(email);
                return mailAddress.Address == email;
            }
            catch
            {
                LogWarning("Email rechazado por formato inválido: {Email}", email);
                return false;
            }
        }

        public bool IsValidEmail(string email, IEnumerable<string> allowedDomains)
        {
            if (!IsValidEmail(email))
                return false;

            var domain = email.Split('@')[1].ToLowerInvariant();
            var isAllowed = allowedDomains.Any(d => d.ToLowerInvariant() == domain);

            if (!isAllowed)
            {
                LogWarning("Email rechazado por dominio no permitido: {Email}, Dominios permitidos: {AllowedDomains}", email, string.Join(", ", allowedDomains));
            }

            return isAllowed;
        }
    }
}