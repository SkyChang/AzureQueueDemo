using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using System.Threading.Tasks;
using AspNetMvcWebAndWorkRole.Web;
using AspNetMvcWebAndWorkRole.Web.Models;

namespace DBRepoWorkerRole
{
    public class WorkerRole : RoleEntryPoint
    {
        private UnitOfWork _uow;
        CancellationTokenSource tokenSource = new CancellationTokenSource();

        public override void Run()
        {
            Task task = RunAsync(tokenSource.Token);
            try
            {
                task.Wait();
            }
            catch (Exception ex)
            {
                Trace.TraceInformation("Unhandled exception in FixIt worker role.", "Information");
            }
        }

        private async Task RunAsync(CancellationToken token)
        {
            QueueHelper queueHelpter = new QueueHelper(_uow);
            while (!token.IsCancellationRequested)
            {
                try
                {
                    await queueHelpter.ProcessMessagesAsync();
                }
                catch (Exception ex)
                {
                    Trace.TraceInformation("Exception in worker role Run loop.", "Information");
                }
                await Task.Delay(1000);
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;
            _uow = new UnitOfWork();
            return base.OnStart();
        }

        public override void OnStop()
        {
            tokenSource.Cancel();
            tokenSource.Token.WaitHandle.WaitOne();
            base.OnStop();
        }
    }
}
