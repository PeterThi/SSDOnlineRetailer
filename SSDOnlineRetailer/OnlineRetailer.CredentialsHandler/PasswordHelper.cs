using System.Text;
using System.Collections;
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

    public class LoginThrottler
    {
        private static Dictionary<string, TrackingStruct> trackingDict = new Dictionary<string, TrackingStruct>();

        private int maxAttempts;
        private TimeSpan lockoutPeriod;

        public LoginThrottler(int maxAttempts, TimeSpan lockoutPeriod)
        {
            this.maxAttempts = maxAttempts;
            this.lockoutPeriod = lockoutPeriod;
            Console.WriteLine("New throttler made in cons");
        }

        public bool IsBlocked(string ip)
        {
            Console.WriteLine("ip is attempting" + ip);
            if (trackingDict.TryGetValue(ip, out TrackingStruct trackStruct)) //if the ip is blocked, return true
            {
                Console.WriteLine("found ip");
                if (trackStruct.attempts >= maxAttempts && DateTime.Now - trackStruct.lastAttempt < lockoutPeriod)
                {
                    Console.WriteLine("true, should be blocked", trackStruct.attempts, trackStruct.attempts >= maxAttempts, DateTime.Now - trackStruct.lastAttempt < lockoutPeriod);
                    return true;
                }
                if (DateTime.Now -  trackStruct.lastAttempt >= lockoutPeriod)
                {
                    Console.WriteLine("False, updating attempts", trackStruct.attempts, trackStruct.attempts >= maxAttempts, DateTime.Now - trackStruct.lastAttempt < lockoutPeriod);
                    trackingDict[ip] = new TrackingStruct { attempts = trackStruct.attempts, lastAttempt = DateTime.Now};
                }
                
            }
            return false;

        }

        public void RegisterAttempt(string ip, bool isSuccesfull)
        {
            if (isSuccesfull)
            {
                trackingDict.Remove(ip);
                Console.WriteLine("Successful, removed");
            }
            else
            {
                foreach (var tracking in trackingDict.Keys)
                {
                    Console.WriteLine("inside1");
                    Console.WriteLine(tracking.ToString() + "BEFORE ADDING");
                }
                Console.WriteLine(trackingDict.TryGetValue(ip, out TrackingStruct trackingStructd));
                if (trackingDict.TryGetValue(ip, out TrackingStruct trackingStruct))
                {
                    trackingStruct.attempts++;
                    trackingDict[ip] = trackingStruct;
                }
                else
                {
                    trackingDict[ip] = new TrackingStruct { attempts = 1, lastAttempt = DateTime.Now };
                    Console.WriteLine("Added new");
                    foreach (var tracking in trackingDict.Keys)
                    {
                        Console.WriteLine("inside");
                        Console.WriteLine (tracking.ToString() + "AFTER ADDING");
                    }
                }
                
            }
        }
    }

    public class TrackingStruct
    {
        public int attempts;
        public DateTime lastAttempt;
    }
}
