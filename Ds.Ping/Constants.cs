﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ds.Ping
{
    public class Constants
    {
        private Constants() { }

        public const int InvalidInt = int.MinValue;
        public const long InvalidLong = long.MinValue;
        public const string InvalidString = null;
        public const Decimal InvalidDecimal = Decimal.MinValue;
        public const float InvalidFloat = float.MinValue;
        public const double InvalidDouble = double.MinValue;

        public static readonly DateTime InvalidDateTime = DateTime.MinValue;
        public static readonly Guid InvalidGuid = Guid.Empty;
        public static readonly TimeSpan InvalidTimeSpan = TimeSpan.MinValue;
    }
}
