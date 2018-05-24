using System;
using System.Collections.Generic;
using System.Text;

namespace MessageSender.Model
{
    class HealthStatus
    {
        public string DbStatus { get; set; }
        public string Version { get; set; }
        public int ServerResponseStatus { get; set; }
        public List<WorkerStatus> WorkerList { get; set; }
    }
}
