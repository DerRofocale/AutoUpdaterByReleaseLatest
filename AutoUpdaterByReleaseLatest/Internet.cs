using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AutoUpdaterByReleaseLatest
{
    public class Internet
    {
        public static bool OK()
        {
			try
			{
                using (HttpClient http = new HttpClient())
                {
                    http.GetAsync("https://github.com/");
                }
                return true;
			}
			catch
			{
                return false;
			}
        }
    }
}
