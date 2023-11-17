using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Collections.Concurrent;

namespace TCPConnection
{
    abstract public class ServerWokersPool
    {
        protected ConcurrentQueue<IServerWorker> _workers;
        public int ClientsCount;
        public ServerWokersPool(int count) {
            ClientsCount = count;
        }
        public void Run(TcpClient client) {
            if (_workers.Count != 0)
            {
                Task.Run(
                    () =>
                    {
                        if (_workers.TryDequeue(out IServerWorker worker)) {
                            worker.StartWork(client);
                            _workers.Enqueue(worker);
                        }
                    }
                    );
            }
        }
    }
}
