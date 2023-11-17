using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;

namespace TCPConnection
{
    public class ClientConnection
    {
        TcpClient _client;
        public ClientConnection(string IP, int port)
        {
            _client = new TcpClient(IP, port);
        }
        public void SendMessages(List<string> messages) {
            foreach (string str in messages)
                SendMessage(str);
        }
        public void SendMessage(string messsage)
        {
            StreamWriter writer = new StreamWriter(_client.GetStream());
            writer.WriteLine(messsage);
            writer.Flush();
        }
        public string RecieveMessage()
        {
            StreamReader reader = new StreamReader(_client.GetStream());
            return reader.ReadLine();
        }
        public bool IsConnected()
        {
            return _client.Connected;
        }
    }
}
