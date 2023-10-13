using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ds.Ping
{
    public class Validation
    {
        private Validation() { }

        public static bool IsFlagged(int flaggedEnum, int flaggedValue)
        {
            if ((flaggedEnum & flaggedValue) != 0)
                return true;
            else
                return false;
        }
    }
}
