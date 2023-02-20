using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AutoUpdaterByReleaseLatest
{
    public class Internet
    {
        public bool OK()
        {
			try
			{
                Dns.GetHostEntry("");
                return true;
			}
			catch
			{
                return false;
			}
        }
    }
}
