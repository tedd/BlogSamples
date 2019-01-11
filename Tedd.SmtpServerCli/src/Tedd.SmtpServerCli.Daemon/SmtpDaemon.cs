using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Tedd.SmtpServerCli.Daemon
{
    public class SmtpDaemon : IDisposable
    {
        public readonly string Hostname;
        public readonly IPAddress ListenAddress;
        public readonly int ListenPort;

        private readonly object _listenSocketLock = new object();
        private TcpListener _tcpListener;
        private CancellationTokenSource _cancellationTokenSource;
        private Task _listenTask;

        public SmtpDaemon(string hostname, IPAddress listenAddress, int listenPort)
        {
            Hostname = hostname;
            ListenAddress = listenAddress;
            ListenPort = listenPort;
        }

        public SmtpDaemon(string hostname, int listenPort) : this(hostname, IPAddress.Any, listenPort)
        {
        }


        public async Task Start()
        {
            lock (_listenSocketLock)
            {
                if (_tcpListener != null)
                    throw new Exception($"Attempting to set up listening on {ListenAddress.ToString()} port {ListenPort}: Already listening.");

                _cancellationTokenSource = new CancellationTokenSource();

                _tcpListener = new TcpListener(ListenAddress, ListenPort);
                _tcpListener.Start();

                _listenTask = Task.Factory.StartNew(ListenLoop);
            }
        }

        private async Task ListenLoop()
        {
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                var client = await _tcpListener.AcceptSocketAsync().ConfigureAwait(false);
                var smtpServerClient = new SmtpServerClient(Hostname, client);
                // We do not await this on purpose
                Task.Factory.StartNew(smtpServerClient.Process);
            }

        }

        public void Stop()
        {
            lock (_listenSocketLock)
            {
                _cancellationTokenSource.Cancel();
                _tcpListener?.Stop();
                _tcpListener = null;
            }
        }

        #region IDisposable

        private void ReleaseUnmanagedResources()
        {
            _tcpListener.Stop();
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        ~SmtpDaemon()
        {
            ReleaseUnmanagedResources();
        }

        #endregion
    }
}
