﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IO_API_Project
{
    public class Pokemon
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int CaptureRate { get; set; }
    }
}
