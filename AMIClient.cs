using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace AMIClient
{
    public class AmiClient
    {
        private TcpClient socket;
        internal static readonly byte[] TerminatorBytes = { 0x0d, 0x0a };
        internal static readonly char[] TerminatorChars = { '\x0d', '\x0a' };

        public AmiClient()
        {
        }

        public delegate void AMIEventHandler(object? sender, AMIEventArgs e);

        public event AMIEventHandler DataReceived;

        private void OnDataReceived(string data)
        {
            DataReceived?.Invoke(this, new AMIEventArgs(data));
        }

        public async void Run(string server, int port, string user, string password)
        {
            socket = new TcpClient(server, port);
            using (var stream = socket.GetStream())
            {
                Login(user, password, stream);
                var bytes = new byte[4096];
                using (var ms = new MemoryStream())
                {
                    int nrBytes;
                    while ((nrBytes = stream.Read(bytes, 0, bytes.Length)) > 0)
                    {
                        ms.SetLength(0);
                        ms.Write(bytes, 0, bytes.Length);
                        var str = Encoding.UTF8.GetString(ms.ToArray(), 0, (int)ms.Length);
                        OnDataReceived(str);
                    }
                }
            }
        }

        private void Login(string username, string password, NetworkStream stream)
        {
            var keyValues = new List<KeyValuePair<string, string>>();
            keyValues.Add(new KeyValuePair<string, string>("Action", "Login"));
            keyValues.Add(new KeyValuePair<string, string>("Username", username));
            keyValues.Add(new KeyValuePair<string, string>("Secret", password));
            var buffer = ToBytes(keyValues);
            lock (stream)
            {
                stream.WriteAsync(buffer, 0, buffer.Length);
            }
        }

        public byte[] ToBytes(List<KeyValuePair<string, string>> keyValues)
        {
            var stream = new MemoryStream();

            using (var writer = new StreamWriter(stream, new UTF8Encoding(false)))
            {
                foreach (var field in keyValues)
                {
                    writer.Write(field.Key);
                    writer.Write(": ");
                    writer.Write(field.Value);
                    writer.Write(TerminatorChars);
                }

                writer.Write(TerminatorChars);
            }

            return stream.ToArray();
        }

        public class AMIEventArgs
        {
            public string Data { get; set; }

            public AMIEventArgs(string data)
            {
                Data = data;
            }
        }
    }
}
