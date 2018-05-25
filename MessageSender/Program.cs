using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using MessageSender.Model;
using System.Net;
using System.IO;
using System.Timers;
using System.Diagnostics;

namespace MessageSender
{
    class Program
    {
        private static string adress;
        private static string path;
        private static int counter = 0;
        private static IConfiguration Configuration { get; set; }

        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("config.json", optional: false);
            Configuration = builder.Build();
            adress = $"{Configuration["configuration:adress"]}";
            path = $"{Configuration["configuration:path"]}";

            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            Timer t = new Timer(3000);
            t.Elapsed += OnTimeEvent;
            t.Start();
            
            Console.ReadKey();
        }

        private static void OnTimeEvent(object sender, EventArgs e) { HealthChecker(); } 

        public static void HealthChecker()
        {
            var health = HealthCheck.GetHealthStatus(adress);
            StringWriter stringWriter = new StringWriter();
            if (health == null)
            {
                stringWriter.WriteLine("Connection failed");
            }
            else
            {
                stringWriter.WriteLine("Response status: " + health.ServerResponseStatus);
                stringWriter.WriteLine("Version: " + health.Version);
                stringWriter.WriteLine("Is Db Connected: " + health.DbStatus);
                stringWriter.WriteLine("Response Time: " + health.ResponseTime);
                foreach (var worker in health.WorkerList)
                {
                    stringWriter.WriteLine(worker.Name);
                    stringWriter.WriteLine(worker.Status);
                }
            }

            bool append = !(counter >= 4); 
            using (StreamWriter stream = new StreamWriter(path, append))
            {
                stream.Write(stringWriter.ToString());
                stream.Close();
            }
            if(!append){ counter = 0; }
            counter++;
            Console.WriteLine(stringWriter);
        }
    }
}
