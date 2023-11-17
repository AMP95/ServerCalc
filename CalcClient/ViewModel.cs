using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using TCPConnection;
using CalcCommand;

namespace CalcClient
{
    class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        void Notify(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        ClientConnection connection;
        string _result;
        string _left;
        string _right;
        CalculationCommand _command;
        string _symCommand;
        public ViewModel() {
            connection = new ClientConnection("127.0.0.1", 888);
        }
        public string Command
        {
            get { return _symCommand; }
            set {
                switch (value) {
                    case "-": _command = CalculationCommand.Substraction; break;
                    case "*": _command = CalculationCommand.Multiplication; break;
                    case "/": _command = CalculationCommand.Division; break;
                    case "^": _command = CalculationCommand.Exponentiation; break;
                    default : _command = CalculationCommand.Addition;break;
                }
                _symCommand = value;
                Notify("Command");
            }
        }
        public string Result
        {
            get { return _result; }
            set {
                _result = value;
                Notify("Result");
            }
        }
        public string Left {
            get { return _left; }
            set {
                if (isValuable(value))
                {
                    _left = value;
                    Notify("Left");
                }
            }
        }
        public string Right
        {
            get { return _right; }
            set
            {
                if (isValuable(value))
                {
                    _right = value;
                    Notify("Right");
                }
            }
        }
        bool isValuable(string value) {
            return value == "" || 
                   value != null  &&
                    (Char.IsDigit(value.Last()) ||
                    value.Last() == '.' ||
                    value.Last() == ',');
        }
        public ButtonCommand CalcCommandClick
        {
            get { return new ButtonCommand(
                (obj) => {
                    Command = obj.ToString();
                }
                ); }
        }
        public ButtonCommand ResultCommand
        {
            get {
                return new ButtonCommand(
                    (obj) => {
                        connection.SendMessages(
                            new List<string>() { 
                                ((int)_command).ToString(),
                                Left,
                                Right
                            }
                            );
                        Left = "";
                        Right = "";
                        Result = connection.RecieveMessage();
                    },
                    () => {
                        return Left != null && Left != "" &&
                        Right != null && Right != "";
                    }
                    );
            }
        }
    }
}
