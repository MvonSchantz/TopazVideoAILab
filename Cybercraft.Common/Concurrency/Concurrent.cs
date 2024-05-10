using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cybercraft.Common.Concurrency
{
    public static class Concurrent
    {
        public static void ForEachThreaded<T>(IEnumerable<T> source, int maxConcurrency, Action<T> body)
        {
            if (maxConcurrency <= 1)
            {
                foreach (var element in source)
                {
                    body(element);
                }
                return;
            }

            T[] sourceArray = source.ToArray();
            int chunkSize = sourceArray.Length / maxConcurrency;
            var threads = new Thread[maxConcurrency];

            for (int t = 0; t < maxConcurrency; t++)
            {
                threads[t] = new Thread(p =>
                {
                    int threadId = (int)p;
                    int firstItem = threadId * chunkSize;
                    int lastItem;
                    if (threadId == maxConcurrency - 1)
                    {
                        lastItem = sourceArray.Length - 1;
                    }
                    else
                    {
                        lastItem = (threadId + 1) * chunkSize - 1;
                    }

                    for (int i = firstItem; i <= lastItem; i++)
                    {
                        body(sourceArray[i]);
                    }
                });
                threads[t].Start(t);
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }
        }

        public static void ForEach<T>(IEnumerable<T> source, int maxConcurrency, Action<T> body)
        {
            if (maxConcurrency <= 1)
            {
                foreach (var element in source)
                {
                    body(element);
                }
                return;
            }

            using (var concurrencySemaphore = new SemaphoreSlim(maxConcurrency))
            {
                Parallel.ForEach(source, element =>
                {
                    try
                    {
                        concurrencySemaphore.Wait();
                        body(element);
                    }
                    finally
                    {
                        concurrencySemaphore.Release();
                    }
                });

                /*var tasks = new List<Task>();
                var sourceArray = source.ToArray();
                for (var i = 0; i < sourceArray.Length; i++)
                {
                    var element = sourceArray[i];
                    concurrencySemaphore.Wait();

                    var task = Task.Run(() =>
                    {
                        try
                        {
                            Debug.WriteLine("Entered");
                            body(element);
                            GC.Collect();
                            GC.WaitForPendingFinalizers();
                            GC.Collect();
                        }
                        finally
                        {
                            concurrencySemaphore.Release();
                            Debug.WriteLine("Exited");
                        }
                    });
                    tasks.Add(task);
                }

                Task.WaitAll(tasks.ToArray());*/

                /*var threads = new List<Thread>();
                foreach (var element in source)
                {
                    concurrencySemaphore.Wait();

                    var thread = new Thread(e =>
                    {
                        try
                        {
                            body((T)e);
                        }
                        finally
                        {
                            concurrencySemaphore.Release();
                        }
                    });
                    thread.Start(element);
                    threads.Add(thread);
                }

                foreach (var thread in threads)
                {
                    thread.Join();
                }*/
            }
        }

        public static void ParallelThreaded(params Action[] body)
        {
            var threads = new Thread[body.Length];
            for (int t = 0; t < body.Length; t++)
            {
                threads[t] = new Thread(p =>
                {
                    int threadId = (int)p;
                    body[threadId]();
                });
                threads[t].Start(t);
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }
        }
    }
}
