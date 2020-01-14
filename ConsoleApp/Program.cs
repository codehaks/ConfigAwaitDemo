using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            SynchronizationContext.SetSynchronizationContext(new SingleThreadSynchronizationContext());
            var s1 = new Stopwatch();
            s1.Start();

            for (int i = 0; i < 10; i++)
            {


                var t1 = Thread.CurrentThread.ManagedThreadId;
                var task1 = DownloadAsync();
                var task2 = DownloadAsync();
                var r = await Task.WhenAll(task1, task2);
                var t2 = Thread.CurrentThread.ManagedThreadId;

                var result = $"{t1} ---> [" + r[0] + " --> " + r[1] + $"] ---> {t2}";

                Console.WriteLine(result);
            }

            s1.Stop();

            Console.WriteLine("Elasped ms: "+s1.ElapsedMilliseconds);

        }

        static private async Task<string> DownloadAsync()
        {
            //SynchronizationContext.SetSynchronizationContext(new OneAtATimeSyncContext());

            var t1 = Thread.CurrentThread.ManagedThreadId;
            await Task.Delay(1000);
            var t2 = Thread.CurrentThread.ManagedThreadId;

            return $" {t1} -> {t2}";
        }

        class MySync : SynchronizationContext
        {
            public override void Post(SendOrPostCallback d, object state)
            {
                base.Post(d, state);
            }

            public override void Send(SendOrPostCallback d, object state)
            {
                base.Send(d, state);
            }
        }

        /// <summary>Provides a SynchronizationContext that's single-threaded.</summary>
        private sealed class SingleThreadSynchronizationContext : SynchronizationContext
        {
            /// <summary>The queue of work items.</summary>
            private readonly BlockingCollection<KeyValuePair<SendOrPostCallback, object>> m_queue =
                new BlockingCollection<KeyValuePair<SendOrPostCallback, object>>();
            /// <summary>The processing thread.</summary>
            private readonly Thread m_thread = Thread.CurrentThread;

            /// <summary>Dispatches an asynchronous message to the synchronization context.</summary>
            /// <param name="d">The System.Threading.SendOrPostCallback delegate to call.</param>
            /// <param name="state">The object passed to the delegate.</param>
            public override void Post(SendOrPostCallback d, object state)
            {
                if (d == null) throw new ArgumentNullException("d");
                m_queue.Add(new KeyValuePair<SendOrPostCallback, object>(d, state));
            }

            /// <summary>Not supported.</summary>
            public override void Send(SendOrPostCallback d, object state)
            {
                throw new NotSupportedException("Synchronously sending is not supported.");
            }

            /// <summary>Runs an loop to process all queued work items.</summary>
            public void RunOnCurrentThread()
            {
                foreach (var workItem in m_queue.GetConsumingEnumerable())
                    workItem.Key(workItem.Value);
            }

            /// <summary>Notifies the context that no more work will arrive.</summary>
            public void Complete() { m_queue.CompleteAdding(); }
        }

        private class OneAtATimeSyncContext : SynchronizationContext
        {
            private Task _task = Task.CompletedTask;
            private object lockObj = new object();

            public override void Post(SendOrPostCallback d, object state)
            {
                lock (lockObj)
                {
                    _task = _task.ContinueWith(_ =>
                    {
                        d(state);
                    });
                }
            }
        }

        //static private async Task DownloadAsync()
        //{

        //    var clinet = new HttpClient();
        //    await clinet.GetAsync("https://codehaks.com");

        //}
    }
}
