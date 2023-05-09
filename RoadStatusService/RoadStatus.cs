using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RoadStatusService
{ 


    [DataContract]
    public class RoadStatus
    {
        [DataMember (Name= "displayName")]  
        public string DisplayName { get; set; }

        [DataMember (Name ="statusSeverity")]
        public string StatusSeverity { get; set; }

        [DataMember (Name ="statusSeverityDescription")]
        public string StatusSeverityDescription { get; set; }
    }
}
