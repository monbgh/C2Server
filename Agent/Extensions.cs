﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Security.Cryptography.X509Certificates;


namespace Agent
{
    public static  class Extensions
    {
        public static byte[] Serialise<T>(this T data)
        {
            var serialiser = new DataContractJsonSerializer(typeof(T));

            using (var ms = new MemoryStream()) {
                serialiser.WriteObject(ms, data);
                return ms.ToArray();
            } 
        }
        public static T Deserialize<T>(this byte[] data)
        {
            var serialiser = new DataContractJsonSerializer(typeof(T));
            using (var ms = new MemoryStream()) {

                return (T)serialiser.ReadObject(ms);

            }
        }

    }
}
