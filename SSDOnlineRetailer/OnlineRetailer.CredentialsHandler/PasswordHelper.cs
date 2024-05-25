using System.Text;
using System.Security.Cryptography;
using OnlineRetailer.Infrastructure;
using OnlineRetailer.Core;
namespace OnlineRetailer.CredentialsHandler
{
    public class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public static bool ValidateCustomer(int id, string password, IRepository<Customer> repository)
        {
            var customer = repository.Get(id);
            if (customer == null)
            {
                return false;
            }

            string newHashedPassword = HashPassword(password);
            return customer.hashedPassword == newHashedPassword;
        }
    }
}
