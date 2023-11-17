using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TCPConnection;
using CalcCommand;
using System.IO;

namespace ServerCalcServer
{
    public class Calculator : IServerWorker
    {
        StreamReader _reader;
        StreamWriter _writer;
        string message;
        public void StartWork(TcpClient _tcpClient)
        {
            _reader = new StreamReader(_tcpClient.GetStream());
            _writer = new StreamWriter(_tcpClient.GetStream());
            while (_tcpClient.Connected) {
                try
                {
                    if (int.TryParse(_reader.ReadLine(), out int result))
                    {
                        Tuple<double, double> operands = GetOperands();
                        switch ((CalculationCommand)result)
                        {
                            case CalculationCommand.Addition:
                                message = (operands.Item1 + operands.Item2).ToString(); break;
                            case CalculationCommand.Division:
                                message = Division(operands.Item1, operands.Item2).ToString(); break;
                            case CalculationCommand.Exponentiation:
                                message = Math.Pow(operands.Item1, operands.Item2).ToString(); break;
                            case CalculationCommand.Multiplication:
                                message = (operands.Item1 * operands.Item2).ToString(); break;
                            case CalculationCommand.Substraction:
                                message = (operands.Item1 - operands.Item2).ToString(); break;
                            default:
                                throw new InvalidOperationException("Недопустимая команда");
                        }
                    }
                    else
                        throw new InvalidOperationException("Недопустимая команда");
                }
                catch (Exception ex) {
                    message = ex.Message;
                }
                _writer.WriteLine(message);
                try
                {
                    _writer.Flush();
                }
                catch {
                    _tcpClient.Close();
                    _tcpClient.Dispose();
                    break;
                }
            }
        }
        Tuple<double, double> GetOperands()
        {
            if (double.TryParse(_reader.ReadLine(), out double left))
            {
                if (double.TryParse(_reader.ReadLine(), out double right))
                {
                    return Tuple.Create(left, right);
                }
            }
            throw new ArgumentException("Недопустимые аргументы");
        }
        double Division(double left, double right) {
            if(right == 0)
                throw new InvalidOperationException("Деление на ноль!");
            return left / right;
        }
    }
    public class CalcPool : ServerWokersPool {
        public CalcPool(int count):base(count) {
            _workers = new System.Collections.Concurrent.ConcurrentQueue<IServerWorker>();
            for (int i = 0; i < ClientsCount; i++) {
                _workers.Enqueue(new Calculator());
            }
        }
    }
         
}
