using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace BroforceModSoftware.Threading {
    public static class ThreadHandling {
        public delegate void Threading();
        public static event Threading Finished;

        static Queue<Action> tasks = new Queue<Action>();

        public static void QueueTask(Action action){
            tasks.Enqueue(action);
        }

        public static void ExecuteTasks(){
            if (tasks.Count > 0){
                RunNextTask();
            }
        }

        // Runs the next task queued in [tasks]
        async static void RunNextTask(int timeout = 10){
            // Timeout in ms
            timeout = timeout * 1000;

            // Task creation
            var task = Task.Run(tasks.Dequeue());
            await task.ContinueWith(t => System.Console.WriteLine("TASK DONE"));

            // Timeout
            if (await Task.WhenAny(task, Task.Delay(timeout)) == task) {
                // Task completed without timing out
                if (tasks.Count > 0){
                    RunNextTask();
                } else {
                    Finished?.Invoke(); 
                }
            }
        }
    }
}