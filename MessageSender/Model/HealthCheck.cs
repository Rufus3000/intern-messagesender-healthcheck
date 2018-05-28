using System.Net;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System;
using System.Xml.Linq;
using System.Diagnostics;

namespace MessageSender.Model
{
    class HealthCheck
    {

        public static HealthStatus GetHealthStatus(string adress)
        {
            List<WorkerStatus> workers = new List<WorkerStatus>();
            Stopwatch sw = new Stopwatch();
            int responseTime;

            try {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(adress);
                request.AllowAutoRedirect = false;
                request.Accept = "application/xml";
                sw.Start();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                sw.Stop();

                responseTime = (int)sw.ElapsedMilliseconds;
                int statusCode = (int)response.StatusCode;

                if (statusCode != 200)
                {
                    return new HealthStatus() { ServerResponseStatus = statusCode };
                };
                Stream stream = response.GetResponseStream();
                
                XDocument doc;
                doc = XDocument.Load(stream);
                string xmlns = doc.Root.Attribute("xmlns").Value;

                
                foreach (var worker in doc.Root.Element("{" + xmlns + "}Workers").Elements())
                {

                    workers.Add(new WorkerStatus() { Name = worker.Element("{" + xmlns + "}Name").Value, Status = worker.Element("{" + xmlns + "}Status").Value });
                }

                return new HealthStatus()
                {
                    ServerResponseStatus = statusCode,
                    Version = doc.Root.Element("{" + xmlns + "}Version").Value,
                    DbStatus = doc.Root.Element("{" + xmlns + "}IsDbConnected").Value,
                    ResponseTime = responseTime,
                    WorkerList = workers

                };
            }
            catch
            {
                return null;
            }
        }
    }   
}