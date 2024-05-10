using System;
using System.IO;
using System.Threading;

namespace Cybercraft.Common
{
    public abstract class SafeBase
    {
        protected static void SafeExecute(Action action)
        {
            SafeExecute(() =>
            {
                action();
                return true;
            });
        }

        protected static T SafeExecute<T>(Func<T> action)
        {
            const int retries = 8;
            int sleep = 250;

            for (int retry = 0; retry < retries; retry++)
            {
                try
                {
                    return action();
                }
                catch (IOException e)
                {
                    switch (((uint)e.HResult))
                    {
                        case 0x80070035: // The network path was not found.
                            break;
                        default:
                            throw;
                    }

                    Thread.Sleep(sleep);
                    sleep *= 2;
                }
            }
            return action();
        }
    }
}
