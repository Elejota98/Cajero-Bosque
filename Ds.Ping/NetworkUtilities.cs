﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Net;

namespace Ds.Ping
{
    /// <summary>
    /// Common network functions.
    /// </summary>
    public class NetworkUtilities
    {
        #region Interop Definitions

        [DllImport("WININET", CharSet = CharSet.Auto)]
        private static extern bool InternetGetConnectedState(
            ref InternetConnectionStatesType lpdwFlags,
            int dwReserved);

        #endregion

        #region Connection State Functions

        public static InternetConnectionStatesType CurrentState
        {
            get
            {
                InternetConnectionStatesType state = 0;

                InternetGetConnectedState(ref state, 0);

                return state;
            }
        }

        public static bool IsOnline()
        {
            InternetConnectionStatesType connectionStatus = CurrentState;

            return (!Validation.IsFlagged((int)InternetConnectionStatesType.Offline, (int)connectionStatus));
        }

        #endregion

        #region Other Network Functions

        public static IPHostEntry ResolveHost(string hostname)
        {
            try
            {
                return Dns.GetHostByName(hostname);
            }
            catch
            {
            }

            return null;
        }

        #endregion
    }
}
