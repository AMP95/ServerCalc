using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCPConnection;

namespace ServerCalcServer
{
    class Program
    {
        static void Main(string[] args)
        {
            int count = 10;
            CalcPool calc = new CalcPool(count);
            ServerConnection connection = new ServerConnection("127.0.0.1", 888, calc);
            connection.AcceptClients(count);
        }
    }
}
