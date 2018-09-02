using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SwitchRichPresence
{
    [Serializable]
    public class TcpCommandException : Exception
    {
        public TcpCommandException() { }
        public TcpCommandException(string message) : base(message) { }
        public TcpCommandException(string message, Exception inner) : base(message, inner) { }
        protected TcpCommandException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    public class TcpCommand
    {
        private const uint SRV_MAGIC = 0x11223300;// 53 52 56 00 | "SRV" + 1byte info
        private const uint CLT_MAGIC = 0x33221100;// 43 4C 54 00 | "CLT" + 1byte info

        public enum SendCommandType : byte
        {
            SendBuffer      = 0,
            Confirm         = 1,
            GetControlData  = 2,
            ListApps        = 3,
            GetActiveUser   = 4,
            GetCurrentApp   = 5,
            GetVersion      = 6,
            Disconnect      = 0xFF,
        }

        private static byte[] ReceiveRaw(Socket client, int size)
        {
            byte[] buffer = new byte[size];

            int total = 0;
            while (total < buffer.Length)
            {
                int count = client.Receive(buffer, total, buffer.Length - total, SocketFlags.None);
                if (count < 0)
                    throw new Exception("Error while receiving data !");
                total += count;
            }

            return buffer;
        }
        private static void SendRaw(Socket client, byte[] data)
        {
            int total = 0;
            while (total < data.Length)
            {
                int count = client.Send(data, total, data.Length - total, SocketFlags.None);
                if (count < 0)
                    throw new Exception("Error while receiving data !");
                total += count;
            }
        }

        //send cmd id
        public static void SendCommand(Socket client, SendCommandType type)
        {
            byte[] buffer = BitConverter.GetBytes(CLT_MAGIC | (byte)type);
            SendRaw(client, buffer);
        }
        public static void ReceiveConfirm(Socket client)
        {
            byte[] buffer = ReceiveRaw(client, 4);

            uint magic = BitConverter.ToUInt32(buffer, 0);

            if ((magic & 0xFFFFFF00) != SRV_MAGIC)
                throw new TcpCommandException(string.Format("Invalid Response magic : 0x{0} instead of 0x{1}", (magic & 0x00FFFFFF).ToString("X"), SRV_MAGIC.ToString("X")));
        }
        
        //header + data
        public static byte[] ReceiveBuffer(Socket client, int size)
        {
            byte[] header = ReceiveRaw(client, 4);

            //validate command
            uint magic = BitConverter.ToUInt32(header, 0);
            if ((magic & 0xFFFFFF00) != SRV_MAGIC)
                throw new TcpCommandException(string.Format("Invalid Response magic : 0x{0} instead of 0x{1}", ((magic & 0xFFFFFF00)>>8).ToString("X"), (SRV_MAGIC>>8).ToString("X")));

            byte[] data = ReceiveRaw(client, size);

            return data;
        }
        public static void SendBuffer(Socket client, byte[] data)
        {
            byte[] header = BitConverter.GetBytes(CLT_MAGIC | (byte)SendCommandType.SendBuffer);
            SendRaw(client, header);
            SendRaw(client, data);
        }

        //wrappers
        public static void SendBuffer(Socket client, ulong nb)
        {
            SendBuffer(client, BitConverter.GetBytes(nb));
        }
        public static int ReceiveInt32(Socket client)
        {
            return BitConverter.ToInt32(ReceiveBuffer(client, 4), 0);
        }
        public static ulong ReceiveUInt64(Socket client)
        {
            return BitConverter.ToUInt64(ReceiveBuffer(client, 8), 0);
        }
        public static bool ReceiveBool(Socket client)
        {
            byte[] buff = ReceiveBuffer(client, 1);
            return BitConverter.ToBoolean(buff, 0);
        }
    }
}
