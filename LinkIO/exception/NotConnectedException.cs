﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkIOcsharp.exception
{
    public class NotConnectedException : Exception
    {
        public NotConnectedException(String message)
           : base(message) { }

    }
}
