﻿namespace ApiModels.Requests
{
    public class TaskAgentRequest
    {
        public string Command { get; set; }
        public string[] Arguments { get; set; }
        public byte[] File {  get; set; }


    }
}
