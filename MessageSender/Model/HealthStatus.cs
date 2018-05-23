using System;
using System.Collections.Generic;
using System.Text;

namespace MessageSender.Model
{
    class HealthStatus
    {
        public bool DbStatus = true;
        public string Version { get; set; }
        public int ServerResponseStatus { get; set; }
        public List<WorkerStatus> WorkerList { get; set; }
    }
}
