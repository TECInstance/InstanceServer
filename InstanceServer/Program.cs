using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace InstanceServer {
    internal class Program {
        private static void Main() {
            var _srv = new TcpListener(IPAddress.Parse(GetLocalIpAddress()), 1337);
            _srv.Start();
            Console.WriteLine("Started listening on [{0}:1337]", GetLocalIpAddress());
            while (true) {
                if (_srv.Pending()) {
                    var _client = _srv.AcceptTcpClient();
                    Console.WriteLine("Connected!"); 
                    var _buffer = new byte[1];
                    Console.WriteLine("Waiting for client activity...");
                    while (true) {
                        if (!_client.Connected) {
                            break;
                        }
                        var _streamclient = _client.GetStream();
                        _streamclient.Read(_buffer, 0, _buffer.Length);
                        var _datarecieved = Encoding.ASCII.GetString(_buffer);
                        Console.Write(_datarecieved);
                        //streamclient.Close();
                    }
                }
            }
        }

        public static string GetLocalIpAddress() {
            var _host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var _ip in _host.AddressList.Where(ip => ip.AddressFamily == AddressFamily.InterNetwork)) {
                return _ip.ToString();
            }
            throw new Exception("Local IP Address Not Found!");
        }
    }
}