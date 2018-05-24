using System.Net;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System;
using System.Xml.Linq;

namespace MessageSender.Model
{
    class HealthCheck
    {

        public static HealthStatus GetHealthStatus(string adress)
        {
            List<WorkerStatus> workers = new List<WorkerStatus>();

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(adress);
            request.AllowAutoRedirect = false;
            request.Accept = "application/xml";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
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
                WorkerList = workers

                };      
        }
    }   
}