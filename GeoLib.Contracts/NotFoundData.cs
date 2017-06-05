﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GeoLib.Contracts
{
    [DataContract]
    public class NotFoundData
    {
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public string When { get; set; }
        [DataMember]
        public string User { get; set; }
    }
}
