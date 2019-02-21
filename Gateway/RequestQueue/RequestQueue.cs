using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading; 
using System.Threading.Tasks;
using Gateway.Services; 

namespace Gateway.RequestQueue
{
    public class RetryQueue
    {
        private static ConcurrentQueue<Func<Task<Result>>> tasks = new ConcurrentQueue<Func<Task<Result>>>();
        private static Timer timer = new Timer(TimerCallback, null, 0, 2000);

        public static void UntilSuccess(Func<Task<Result>> func) =>
            tasks.Enqueue(func);

        private static void TimerCallback(object o)
        {
            Func<Task<Result>> task = null;
            while (tasks.Count > 0)
            {
                if (tasks.TryDequeue(out task))
                {
                    var result = task.Invoke().Result;
                    if (result.Code != 200)
                        tasks.Enqueue(task);
                }
            }
        }
    }
}
