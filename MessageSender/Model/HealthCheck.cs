using System.Net;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;

namespace MessageSender.Model
{
    class HealthCheck
    {

        public HealthStatus GetHealthStatus(string adress)
        {
            string data = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(adress);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            int statusCode = (int)response.StatusCode;

            if (statusCode != 200)
            {
                return new HealthStatus() { ServerResponseStatus = statusCode };
            };

            Stream stream = response.GetResponseStream();
            StreamReader streamReader = new StreamReader(stream);
            data = streamReader.ReadToEnd();
            XmlReader reader = XmlReader.Create(new StringReader(data));
            
            
            
            

        }
    }   
}