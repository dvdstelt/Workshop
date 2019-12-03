﻿using System;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace Divergent.Customers
{
    static class Program
    {
        public static async Task Main(string[] args)
        {
            var host = new Host();

            // pass this command line option to run as a windows service
            if (args.Contains("--run-as-service"))
            {
                using (var windowsService = new WindowsService(host))
                {
                    ServiceBase.Run(windowsService);
                    return;
                }
            }

            Console.Title = Host.EndpointName;

            var tcs = new TaskCompletionSource<object>();
            Console.CancelKeyPress += (sender, e) => { tcs.SetResult(null); };

            await host.Start();
            await Console.Out.WriteLineAsync("Press Ctrl+C to exit...");

            await tcs.Task;
            await host.Stop();
        }
    }
}
