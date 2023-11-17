using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace TCPConnection
{
    public interface IServerWorker
    {
        void StartWork(TcpClient _tcpClient);
    }
}
