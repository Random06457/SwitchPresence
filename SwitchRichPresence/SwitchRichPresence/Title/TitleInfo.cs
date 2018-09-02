using Random0666_s_ToolBox.Modules.NintendoArchive;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SwitchRichPresence.TitleInfo;

namespace SwitchRichPresence
{
    public class ApplicationRecord
    {
        public ulong TitleID { get; private set; }
        public ApplicationRecord(BinaryReader br)
        {
            TitleID = br.ReadUInt64();
            br.ReadByte();

            br.BaseStream.Position += 0xF;
        }
    }

    public class TitleInfo
    {
        public const int NACP_SIZE = 0x4000;
        public const int ICON_SIZE = 0x20000;
        public const int TITLE_INFO_SIZE = NACP_SIZE + ICON_SIZE;

        private Socket client;

        public ulong TitleID { get; private set; }
        public Bitmap Icon { get; private set; }
        public NACP Metadata { get; private set; }

        public TitleInfo(Socket socket, ulong tid)
        {
            client = socket;
            TitleID = tid;

            GetControlData();
        }
        public bool IsCached()
        {
            return (File.Exists(GetPath()));
        }

        private string GetPath()
        {
            //return string.Format(@"Controls\{0}.bin", TitleID.ToString("X16"));
            return MainForm.TEMP_PATH + TitleID.ToString("X16") + ".bin";
        }
        private void GetControlData()
        {
            byte[] buff = new byte[0];
            if (!IsCached())
            {
                TcpCommand.SendCommand(client, TcpCommand.SendCommandType.GetControlData);
                TcpCommand.ReceiveConfirm(client);
                TcpCommand.SendBuffer(client, TitleID);
                buff = TcpCommand.ReceiveBuffer(client, TITLE_INFO_SIZE);
            }
            else
            {
                buff = File.ReadAllBytes(GetPath());
            }

            using (MemoryStream msBuff = new MemoryStream(buff))
            {
                BinaryReader br = new BinaryReader(msBuff);
                byte[] nacpData = br.ReadBytes(NACP_SIZE);
                byte[] iconData = br.ReadBytes(ICON_SIZE);

                using (MemoryStream ms = new MemoryStream(nacpData))
                    Metadata = new NACP(ms);

                using (MemoryStream ms = new MemoryStream(iconData))
                    Icon = (Bitmap)Image.FromStream(ms);

                if(!IsCached())
                {
                    File.WriteAllBytes(GetPath(), buff);
                }
            }
        }
    }

}
