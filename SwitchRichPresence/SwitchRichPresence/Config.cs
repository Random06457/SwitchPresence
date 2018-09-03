using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwitchRichPresence
{
    public class Config
    {
        private const string CONFIG_PATH = "config.txt";

        public string ClientID { get; set; } = "464720851976060940";
        public string Ip { get; set; } = "192.168.0.XX";
        public bool ShowUser { get; set; } = true;
        public bool ShowTimer { get; set; } = true;
        public string Icon { get; set; } = "icon";

        public Config()
        {
            if (File.Exists(CONFIG_PATH))
            {
                string[] lines = File.ReadAllLines(CONFIG_PATH);

                foreach (var line in lines)
                {
                    string[] parts = line.Replace(" ", "").Replace("\t", "").Split('=');

                    if (parts.Length == 2)
                    {
                        try
                        {
                            switch (parts[0].ToLower())
                            {
                                case "client_id":
                                    ClientID = parts[1];
                                    break;
                                case "ip":
                                    Ip = parts[1];
                                    break;
                                case "show_user":
                                    ShowUser = bool.Parse(parts[1]);
                                    break;
                                case "show_timer":
                                    ShowTimer = bool.Parse(parts[1]);
                                    break;
                                case "icon":
                                    Icon = parts[1];
                                    break;
                            }
                        }
                        catch { }
                    }
                }
            }
        }
        public void Save()
        {
            List<string> lines = new List<string>()
            {
                "client_id=" + ClientID,
                "ip=" + Ip,
                "show_user=" + (ShowUser ? "true" : "false"),
                "show_timer=" + (ShowTimer ? "true" : "false"),
                "icon=" +  Icon,
            };

            File.WriteAllLines(CONFIG_PATH, lines);
        }
    }
}
