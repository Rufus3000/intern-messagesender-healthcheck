using System.Net;
using System.Collections.Generic;
using System.Text;

namespace MessageSender.Model
{
    class HealthCheck
    {
        public HealthStatus GetHealthStatus(string adress)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(adress);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            int statusCode = (int)response.StatusCode;

            if(statusCode != 200)
            {
                return new HealthStatus() { ServerResponseStatus = statusCode};
            }
        }
    }   
}