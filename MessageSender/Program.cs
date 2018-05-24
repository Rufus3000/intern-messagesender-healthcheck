using System;
using System.Net;
using MessageSender.Model;

namespace MessageSender
{
    class Program
    {
        private static string adress = "https://10.0.1.221:9000/";

        static void Main(string[] args)
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            HealthChecker();
            Console.ReadKey();
        }
        
        public static void HealthChecker()
        {
            var health = HealthCheck.GetHealthStatus(adress);

            if (health == null)
            {
                Console.WriteLine("Connection failed");
            }
            else
            {
                Console.WriteLine("Response status: " + health.ServerResponseStatus);
                Console.WriteLine("Version: " + health.Version);
                Console.WriteLine("Is Db Connected: " + health.DbStatus);
                foreach (var worker in health.WorkerList)
                {
                    Console.WriteLine(worker.Name);
                    Console.WriteLine(worker.Status);
                }
            }
        }
    }
}
