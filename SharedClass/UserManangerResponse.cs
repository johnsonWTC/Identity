﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class UserManangerResponse
    {
        public string Message  { get; set; }
        public bool isSuccess { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
