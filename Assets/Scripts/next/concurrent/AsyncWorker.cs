using System;
using System.Collections.Generic;
using System.Threading;
//using System;
//using System.ComponentModel;
//using System.Diagnostics;
//using System.Threading;
using UnityEngine;

namespace th.nx
{
    internal class AsyncWorker
    {
        public enum Status
        {
            Ready = 1,
            Working,
            WaitingTask,
            Dying,
        };

        public AsyncWorker(AsyncDispatcher dispatcher)
        {
            Errno err = (dispatcher != null ? Errno.OK : Errno.InvalidArg);

            if (err == Errno.OK)
            {
                _dispatcher = dispatcher;
                this.status = Status.Ready;
            }

            Debug.Assert(err == Errno.OK);
        }

        public Status status
        {
            get { return _status; }
            set
            {
                Status st = this.status;

                if (value != st)
                {
                    _status = value;
                    switch (value)
                    {
                        case Status.WaitingTask:
                            IList<AsyncWorker> idleWorkerList = _dispatcher.getIdleWorkerList();
                            Monitor.Enter(idleWorkerList);  //++
                            idleWorkerList.Add(this);
                            Monitor.Exit(idleWorkerList);  //--
                            //_status = value;
                            break;
                        case Status.Ready:
                        case Status.Working:
                        case Status.Dying:
                            //_status = value;
                            break;
                        default:
                            Debug.Assert(false, "Invalid status with " + value);
                            _status = st;
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Errno launch()
        {
            Errno err = (this.status == Status.Ready ? Errno.OK : Errno.Already);

            if (err == 0)
            {
                _worker = new Thread(new ThreadStart(workerMain)
                        //, 128000
                        );

                Monitor.Enter(_worker);  //++
                _worker.Start();
                this.status = Status.Working;
                Monitor.Wait(_worker);  // wait
                Monitor.Exit(_worker);  //--
            }

            return err;
        }

        public void signalForTerminate()
        {
            Status st = this.status;

            if (st != Status.Ready && st != Status.Dying)
            {
                Monitor.Enter(this);  //++

                cancelCurrentTask();

                st = this.status;
                this.status = Status.Dying;

                switch (st)
                {
                    case Status.WaitingTask:
                        IList<AsyncWorker> idleWorkerList = _dispatcher.getIdleWorkerList();
                        Monitor.Enter(idleWorkerList);  //++
                        for (int i = 0; i < idleWorkerList.Count; ++i)
                        {
                            if (idleWorkerList[i] == this)
                            {
                                idleWorkerList.RemoveAt(i);
                                break;
                            }
                        }
                        Monitor.Exit(idleWorkerList);  //--

                        Monitor.Pulse(this);  // Pulse
                        break;
                    default:
                        break;
                }

                Monitor.Exit(this);  //--
            }
        }


        internal void cancelCurrentTask()
        {
            _curTask = null;  // g.s.c
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal DateTime getIdleTimestamp()
        {
            return _idleTimestamp;
        }

        //------------------------------------------------------------------------------
        // 
        //------------------------------------------------------------------------------
        internal AsyncTask getCurrentTask()
        {
            return _curTask;
        }


        private void workerMain()
        {
            Monitor.Enter(_worker);  //++
            Monitor.Pulse(_worker);  // Pulse
            Monitor.Exit(_worker);  //--

            processTasks();
        }

        private void processTasks()
        {
            bool isExit = false;
            AsyncTask task = null;
            IDictionary<int, object> resultDict = null;

            IList<AsyncTask> responseQueue = _dispatcher.getResponseQueue();

            while (true)
            {
                Monitor.Enter(this);  // ++

                switch (this.status)
                {
                    case Status.Dying:
                        Monitor.Exit(this);  // --
                        isExit = true;
                        break;
                    default:
                        break;
                }

                if (isExit)
                    break;


                task = _dispatcher.popTask();

                if (task != null)
                {
                    _curTask = task;
                    Monitor.Exit(this);  // --

                    resultDict = new Dictionary<int, object>();
                    task.errorCode = task.onHandleTask(task, resultDict);

                    Monitor.Enter(this);  // ++
                    if (_curTask != null && task.onCompleteTask != null)
                    {
                        task.resultDict = resultDict;

                        Monitor.Enter(responseQueue);  // ++1

                        /*
                        if (responseQueue.Count > 0)
                            responseQueue.Insert(0, task);
                        else
                        //*/
                            responseQueue.Add(task);

                        Monitor.Exit(responseQueue);  // --1
                    }

                    _curTask = null;
                    Monitor.Exit(this);  // --

                    task = null;
                    resultDict = null;
                }
                else
                {
                    this.status = Status.WaitingTask;
                    _idleTimestamp = DateTime.UtcNow;

                    Monitor.Wait(this);  //wait

                    switch (this.status)
                    {
                        case Status.Dying:
                            isExit = true;
                            break;
                        default:
                            break;
                    }

                    Monitor.Exit(this);  // --
                }

                if (isExit)
                    break;

            }  // while

            return;
        }


        private Thread _worker;

        private AsyncDispatcher _dispatcher;
        private volatile Status _status;

        private volatile AsyncTask _curTask;

        private DateTime _idleTimestamp;
    }
}
