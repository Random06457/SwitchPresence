using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SwitchRichPresence
{
    public class Updater
    {
        public const string APP_VERSION = "1.0.0";
        private const string LATEST_URL = "https://raw.githubusercontent.com/Random0666/Useless-stuff/master/SwitchRichPresence/latest.txt";

        public static void GetUpdate()
        {
            try
            {
                WebClient client = new WebClient();
                byte[] data = client.DownloadData(LATEST_URL);
                StringReader sr = new StringReader(Encoding.UTF8.GetString(data));

                string latestVer = sr.ReadLine();
                if (latestVer != APP_VERSION)
                {
                    UpdateForm form = new UpdateForm(latestVer, sr.ReadToEnd());
                    form.ShowDialog();
                }
            }
            //probably no correction
            catch { }
        }
    }
}
