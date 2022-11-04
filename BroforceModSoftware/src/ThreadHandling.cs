using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using System.Threading;

/// <summary>
/// Handles threading for the GUI
/// </summary>

namespace BroforceModSoftware.Threading {
    public static class ThreadHandling {
        public delegate void Threading();
        public static event Threading Finished;

        static Queue<Action> tasks = new Queue<Action>();

        public static void QueueTask(Action action){
            tasks.Enqueue(action);

            RunNextTask();
        }

        // Runs the next task queued in [tasks]
        static void RunNextTask(){
            new Thread(() => {
                Thread.CurrentThread.IsBackground = true; 

                // Task creation
                Task.Run(tasks.Dequeue());

                // Task completed without timing out
                if (tasks.Count == 0){
                    if (Finished != null) Finished.Invoke(); 
                } else {
                    RunNextTask();
                }
            }).Start();
        }
    }
}