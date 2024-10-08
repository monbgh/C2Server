﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agent.Models
{
    [DataContract]
    public class AgentTask
    {
        [DataMember(Name ="id")]
        public string Id { get; set; }

        [DataMember(Name = "command")]
        public string Command { get; set; }

        [DataMember(Name = "argument")]
        public string[] Arguments { get; set; }

        [DataMember(Name = "file")]
        public string File { get; set; }
        public byte[] FileBytes
        {
            get
            {

                if (string.IsNullOrEmpty(File)) return new byte[0];
                return Convert.FromBase64String(File);
            }
        }
                
    }
}
