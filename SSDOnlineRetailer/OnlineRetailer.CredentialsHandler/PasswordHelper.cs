using System.Text;
using System.Collections;
using System.Security.Cryptography;
using OnlineRetailer.Infrastructure;
using OnlineRetailer.Core;
using Monitoring;
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
        }

        public bool IsBlocked(string ip)
        {
            if (trackingDict.TryGetValue(ip, out TrackingStruct trackStruct)) //if the ip is blocked, return true
            {
                if (trackStruct.attempts >= maxAttempts && DateTime.Now - trackStruct.lastAttempt < lockoutPeriod)
                {
                    MonitoringService.Log.Warning("Following banned IP trying to log in:" + ip);
                    Console.WriteLine("true, should be blocked", trackStruct.attempts, trackStruct.attempts >= maxAttempts, DateTime.Now - trackStruct.lastAttempt < lockoutPeriod);
                    return true;
                }
                if (DateTime.Now -  trackStruct.lastAttempt >= lockoutPeriod)
                {
                    
                    trackingDict[ip] = new TrackingStruct { attempts = trackStruct.attempts, lastAttempt = DateTime.Now};
                    if (trackStruct.attempts >= maxAttempts)
                    {
                        MonitoringService.Log.Warning("Following ip was just banned for too many attempts" + ip);
                    }
                }
                
            }
            return false;

        }

        public void RegisterAttempt(string ip, bool isSuccesfull)
        {
            if (isSuccesfull)
            {
                trackingDict.Remove(ip);
                MonitoringService.Log.Verbose("following ip succesfully logged in:" + ip);
            }
            else
            {

                Console.WriteLine(trackingDict.TryGetValue(ip, out TrackingStruct trackingStructd));
                if (trackingDict.TryGetValue(ip, out TrackingStruct trackingStruct))
                {
                    trackingStruct.attempts++;
                    trackingDict[ip] = trackingStruct;
                }
                else
                {
                    MonitoringService.Log.Verbose("New ip registered trying to log in" + ip);
                    trackingDict[ip] = new TrackingStruct { attempts = 1, lastAttempt = DateTime.Now };
                    
                }
                
            }
        }

        public void unbanIp(string ip)
        {
            if (trackingDict.ContainsKey(ip))
            {
                trackingDict.Remove(ip);
            }
            
        }
    }

    public class TrackingStruct
    {
        public int attempts;
        public DateTime lastAttempt;
    }
}
