using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace Tedd.SmtpServerCli.Daemon
{
    class SmtpServerClient : IDisposable
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();
        private readonly Socket _socket;
        private readonly NetworkStream _networkStream;
        private readonly StreamReader _streamReader;
        private readonly StreamWriter _streamWriter;
        private readonly string _hostname;
        private readonly string _clientIp;

        public SmtpServerClient(string hostname, Socket socket)
        {
            _socket = socket;
            _networkStream = new NetworkStream(socket, true);
            _streamReader = new StreamReader(_networkStream);
            _streamWriter = new StreamWriter(_networkStream);
            _streamWriter.AutoFlush = true;
            var remote = _socket.RemoteEndPoint as IPEndPoint;
            _clientIp = remote.Address.ToString() + ":" + remote.Port;
        }

        private void LogTrace(string message) => _log.Trace($"{_clientIp} {message}");
        private void LogDebug(string message) => _log.Debug($"{_clientIp} {message}");
        private void LogInfo(string message) => _log.Info($"{_clientIp} {message}");

        public async Task Process()
        {
            // We implement essential parts of RFC 821 SMTP: https://tools.ietf.org/html/rfc821
            // Protocol is fairly simple, and strict on spacing and stuff.

            // Connection opening
            await SendLine(220, $"{_hostname} Simple Mail Transfer Service Ready");
            string line, lineUC;
            bool inData = false, hasHelo = false;
            string from = null, to = null;

            var dataSB = new StringBuilder();
            while (true)
            {
                line = await ReadLine();
                lineUC = line.ToUpperInvariant(); // Protocol is case insensitive

                if (!inData)
                {
                    // Initial handshake, no action required
                    if (lineUC.StartsWith("HELO"))
                    {
                        hasHelo = true;
                        await SendLine(250, $"{_hostname}");
                    }
                    // Who is sending e-mail, can be ignored if we want to
                    else if (lineUC.StartsWith("MAIL FROM:"))
                    {
                        if (!hasHelo)
                        {
                            await SendLine(503, $"Send HELO first");
                            continue;
                        }

                        from = line.Substring(11);
                        LogInfo($"From: {from}");

                        await SendLine(250, $"ok");
                    }
                    // Who is receiving e-mail. This is the actual target. Any TO: within data body is ignored.
                    else if (lineUC.StartsWith("RCPT TO:"))
                    {
                        if (string.IsNullOrWhiteSpace(from))
                        {
                            await SendLine(503, $"Send MAIL FROM: first");
                            continue;
                        }

                        to = line.Substring(9);
                        LogInfo($"To: {to}");

                        await SendLine(250, $"ok");
                    }
                    // The content of the e-mail will follow
                    else if (lineUC == "DATA")
                    {
                        if (string.IsNullOrWhiteSpace(to))
                        {
                            await SendLine(503, $"Bad sequence of commands");
                            continue;
                        }
                        inData = true;
                        await SendLine(354, $"Start mail input; end with <CRLF>.<CRLF>");
                    }
                    else if (lineUC.StartsWith("QUIT"))
                    {
                        await SendLine(221, $"{_hostname} Service closing transmission channel");
                        _socket.Close(1);
                        LogInfo("Closing");
                        return;
                    }
                    else if (lineUC.StartsWith("NOOP"))
                    {
                        await SendLine(250, $"ok");
                    }
                    else
                        await SendLine(502, $"Unrecognized command");

                }

                if (inData)
                {
                    if (line == ".")
                    {
                        inData = false;
                        await SendLine(250, $"ok");

                        // TODO: Mail delivery is done!
                        LogDebug("--- Message start ---");
                        LogDebug(dataSB.ToString());
                        LogDebug("--- Message end ---");

                        // Clear up, ready for new receive
                        dataSB.Clear();
                        from = null;
                        to = null;

                        continue;
                    }

                    dataSB.AppendLine(line);
                }

            }
        }

        private async Task SendLine(int code, string text)
        {
            var str = $"{code} {text}";
            LogTrace("> " + str);
            await _streamWriter.WriteAsync(str + "\r\n");
        }

        private async Task<string> ReadLine()
        {
            var str = await _streamReader.ReadLineAsync();
            LogTrace("<" + str);

            return str;
        }



        #region IDisposable

        private void ReleaseUnmanagedResources()
        {
            _socket?.Dispose();
            _networkStream?.Dispose();
            _streamWriter?.Dispose();
        }

        private void Dispose(bool disposing)
        {
            ReleaseUnmanagedResources();
            if (disposing)
            {
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~SmtpServerClient()
        {
            Dispose(false);
        }

        #endregion
    }
}
