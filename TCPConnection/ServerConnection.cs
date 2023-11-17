using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;

namespace TCPConnection
{
   public class ServerConnection
    {
        TcpListener _tcpListener;
        List<TcpClient> _tcpClients;
        ServerWokersPool _workers;
        bool _toAllClients = false;
        public ServerConnection(string ip, int port, ServerWokersPool workers = null) {
            _tcpListener = new TcpListener(IPAddress.Parse(ip), port);
            _tcpListener.Start();
            _workers = workers;
            _tcpClients = new List<TcpClient>();
        }
        public void AcceptClients(int clientsCount) {
            if (_workers != null && _workers.ClientsCount < clientsCount)
                clientsCount = _workers.ClientsCount;
            while (true) {
                if (_tcpClients.Count < clientsCount)
                {
                    TcpClient client = _tcpListener.AcceptTcpClient();
                    _tcpClients.Add(client);
                    if (_workers != null)
                        _workers.Run(client);
                }
                else
                    Thread.Sleep(5000);
                RemoveClients();
            }
        }
        /// <summary>
        /// Send message to current client
        /// </summary>
        /// <param name="client"></param>
        /// <param name="message"></param>
        public void SendMessage(string message, TcpClient client) {
            StreamWriter writer = new StreamWriter(client.GetStream());
            writer.WriteLine(message);
            try
            {
                writer.Flush();
            }
            catch {
                client.Close();
                client.Dispose();
                if (!_toAllClients)
                    _tcpClients.Remove(client);
            }
        }
        /// <summary>
        /// Send message to all clients
        /// </summary>
        /// <param name="message"></param>
        public void SendMessage(string message)
        {
            _toAllClients = true;
            foreach (var client in _tcpClients) {
                SendMessage(message, client);
            }
            RemoveClients();
            _toAllClients = false;
        }
        public string RecieveMessage(TcpClient client) {
            StreamReader reader = new StreamReader(client.GetStream());
            return reader.ReadLine();
        }
        void RemoveClients() {
            for (int i = 0; i < _tcpClients.Count;) {
                if (_tcpClients[i] == null)
                    _tcpClients.Remove(_tcpClients[i]);
                else
                    i++;
            }
        }
    }
}
