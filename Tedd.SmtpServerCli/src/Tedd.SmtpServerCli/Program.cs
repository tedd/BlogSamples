using System;
using System.Net;
using System.Threading.Tasks;
using Tedd.SmtpServerCli.Daemon;

namespace Tedd.SmtpServerCli
{
    class Program
    {
        private static Task<Task> _daemonTask;

        static void Main(string[] args)
        {
            var smtpDaemon = new SmtpDaemon("localhost", IPAddress.Parse("127.0.0.1"), 25);
            _daemonTask = Task.Factory.StartNew(smtpDaemon.Start);
            
            
            Console.WriteLine("Press enter to stop.");
            Console.ReadLine();
            smtpDaemon.Stop();
        }
    }
}
