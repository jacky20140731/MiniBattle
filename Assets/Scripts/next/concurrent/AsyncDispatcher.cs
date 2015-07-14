using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System;

namespace th.nx
{
    public class AsyncDispatcher
    {
        public AsyncDispatcher(sbyte maxConcurrent)
        {
            Errno err = (maxConcurrent > 0 ? Errno.OK : Errno.InvalidArg);

            if (err == Errno.OK)
            {
                _maxConcurrent = maxConcurrent;
                this.idleTimeout = 10;
                this.killIdleNumOnce = 1;
                this.leastWorkerNum = 1;
                this.postNumOnce = 1;

                _taskQueue = new Deque<AsyncTask>();
                _responseQueue = new Deque<AsyncTask>();

                _workerPool = new List<AsyncWorker>();
                _idleWorkers = new Deque<AsyncWorker>();
            }
            
            Debug.Assert(err == Errno.OK);
        }

        public Errno ensureLaunchedWorkers(sbyte n)
        {
            Errno err = ((n >= 0 && n <= _maxConcurrent) ? Errno.OK : Errno.InvalidArg);

            if (err == Errno.OK && n > _workerPool.Count)
            {
                AsyncWorker worker = null;
                int num = n - _workerPool.Count;

                while (num-- > 0)
                {
                    worker = new AsyncWorker(this);
                    worker.launch();
                    _workerPool.Add(worker);
                }
            }

            Debug.Assert(err == Errno.OK);
            return err;
        }

        public float idleTimeout
        {
            get { return _idleTimeout; }
            set
            {
                if (value >= 0)
                    _idleTimeout = value;
                else
                    Debug.Assert(false);
            }
        }

        public int killIdleNumOnce
        {
            get { return _killIdleNumOnce; }
            set
            {
                if (value >= 0)
                    _killIdleNumOnce = value;
                else
                    Debug.Assert(false);
            }
        }

        public sbyte leastWorkerNum
        {
            get { return _leastWorkerNum; }
            set
            {
                if (value >= 0 && value <= _maxConcurrent)
                    _leastWorkerNum = value;
                else
                    Debug.Assert(false);
            }
        }


        public int postNumOnce
        {
            get { return _postNumOnce; }
            set
            {
                if (value > 0)
                    _postNumOnce = value;
                else
                    Debug.Assert(false);
            }
        }

        public Errno dispatch(AsyncTask task)
        {
            return dispatch(task, 0, 0);
        }

        public Errno dispatch(AsyncTask task, sbyte prior)
        {
            return dispatch(task, prior, 0);
        }

        // AsyncDispatchBehaviour
        /// <summary>
        /// 
        /// </summary>
        /// <param name="task"></param>
        /// <param name="prior"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        public Errno dispatch(AsyncTask task, sbyte prior, sbyte channel)
        {
            Errno err = (task != null && task.onHandleTask != null) ? Errno.OK : Errno.InvalidArg;

            if (err == Errno.OK)
            {
                task.setPrior(prior);
                task.setChannel(channel);

                int i = 0;

                Monitor.Enter(_taskQueue);  //++

                for (i = _taskQueue.Count - 1; i >= 0; --i)
                {
                    if (prior >= _taskQueue[i].getPrior())
                        break;
                }

                if (i < _taskQueue.Count - 1)
                    _taskQueue.Insert(i + 1, task);
                else
                    _taskQueue.Add(task);

                Monitor.Exit(_taskQueue);  //--

                wakeupWorker();
            }

            Debug.Assert(err == Errno.OK);
            return err;
        }

        public int cancelTask(AsyncTask task)
        {
            int cancelNum = 0;

            if (task != null)
            {
                AsyncTask existTask = null;

                // search task queue
                if (_taskQueue.Count > 0)
                {
                    Monitor.Enter(_taskQueue);  //++

                    for (int i = _taskQueue.Count - 1; i >= 0; --i)
                    {
                        existTask = _taskQueue[i];

                        if (existTask == task)
                        {
                            _taskQueue.RemoveAt(i);
                            ++cancelNum;
                            break;
                        }
                    }  // for

                    Monitor.Exit(_taskQueue);  //--
                }


                if (cancelNum <= 0)
                {
                    // search current task
                    AsyncWorker worker = null;
                    for (int i = 0; i < _workerPool.Count; ++i)
                    {
                        worker = _workerPool[i];

                        Monitor.Enter(worker);  //++

                        existTask = worker.getCurrentTask();
                        if (existTask != null && existTask == task)
                        {
                            worker.cancelCurrentTask();
                            Monitor.Exit(worker);  //--

                            ++cancelNum;
                            break;
                        }

                        Monitor.Exit(worker);  //--
                    }
                }


                if (cancelNum <= 0)
                {
                    // search response queue
                    Monitor.Enter(_responseQueue);  //++

                    for (int i = _responseQueue.Count - 1; i >= 0; --i)
                    {
                        existTask = _responseQueue[i];

                        if (existTask == task)
                        {
                            _responseQueue.RemoveAt(i);
                            ++cancelNum;
                            break;
                        }
                    }  // for

                    Monitor.Exit(_responseQueue);  //--
                }
            }

            return cancelNum;
        }

        public int cancelTask(int taskID)
        {
            int cancelNum = 0;
            AsyncTask existTask = null;

            // search task queue
            if (_taskQueue.Count > 0)
            {
                Monitor.Enter(_taskQueue);  //++

                for (int i = _taskQueue.Count - 1; i >= 0; --i)
                {
                    existTask = _taskQueue[i];

                    if (existTask.taskID == taskID)
                    {
                        _taskQueue.RemoveAt(i);
                        ++cancelNum;
                        break;
                    }
                }  // for

                Monitor.Exit(_taskQueue);  //--
            }


            if (cancelNum <= 0)
            {
                // search current task
                AsyncWorker worker = null;
                for (int i = 0; i < _workerPool.Count; ++i)
                {
                    worker = _workerPool[i];

                    Monitor.Enter(worker);  //++
                    
                    existTask = worker.getCurrentTask();
                    if (existTask != null && existTask.taskID == taskID)
                    {
                        worker.cancelCurrentTask();
                        Monitor.Exit(worker);  //--

                        ++cancelNum;
                        break;
                    }

                    Monitor.Exit(worker);  //--
                }
            }


            if (cancelNum <= 0)
            {
                // search response queue
                Monitor.Enter(_responseQueue);  //++

                for (int i = _responseQueue.Count - 1; i >= 0; --i)
                {
                    existTask = _responseQueue[i];

                    if (existTask.taskID == taskID)
                    {
                        _responseQueue.RemoveAt(i);
                        ++cancelNum;
                        break;
                    }
                }  // for

                Monitor.Exit(_responseQueue);  //--
            }

            return cancelNum;
        }

        public int cancelTasksWithID(int taskID)
        {
            int cancelNum = 0;
            AsyncTask existTask = null;

            // search task queue
            if (_taskQueue.Count > 0)
            {
                Monitor.Enter(_taskQueue);  //++

                for (int i = _taskQueue.Count - 1; i >= 0; --i)
                {
                    existTask = _taskQueue[i];

                    if (existTask.taskID == taskID)
                    {
                        _taskQueue.RemoveAt(i);
                        ++cancelNum;
                        //break;
                    }
                }  // for

                Monitor.Exit(_taskQueue);  //--
            }


            //if (cancelNum <= 0)
            {
                // search current task
                AsyncWorker worker = null;
                for (int i = 0; i < _workerPool.Count; ++i)
                {
                    worker = _workerPool[i];

                    Monitor.Enter(worker);  //++

                    existTask = worker.getCurrentTask();
                    if (existTask != null && existTask.taskID == taskID)
                    {
                        worker.cancelCurrentTask();
                        //Monitor.Exit(worker);  //--

                        ++cancelNum;
                        //break;
                    }

                    Monitor.Exit(worker);  //--
                }
            }


            //if (cancelNum <= 0)
            {
                // search response queue
                Monitor.Enter(_responseQueue);  //++

                for (int i = _responseQueue.Count - 1; i >= 0; --i)
                {
                    existTask = _responseQueue[i];

                    if (existTask.taskID == taskID)
                    {
                        _responseQueue.RemoveAt(i);
                        ++cancelNum;
                        //break;
                    }
                }  // for

                Monitor.Exit(_responseQueue);  //--
            }

            return cancelNum;
        }


        public int cancelTasksWithChannel(sbyte channel)
        {
            int cancelNum = 0;
            AsyncTask existTask = null;

            // search task queue
            if (_taskQueue.Count > 0)
            {
                Monitor.Enter(_taskQueue);  //++

                for (int i = _taskQueue.Count - 1; i >= 0; --i)
                {
                    existTask = _taskQueue[i];

                    if (existTask.getChannel() == channel)
                    {
                        _taskQueue.RemoveAt(i);
                        ++cancelNum;
                        //break;
                    }
                }  // for

                Monitor.Exit(_taskQueue);  //--
            }


            //if (cancelNum <= 0)
            {
                // search current task
                AsyncWorker worker = null;
                for (int i = 0; i < _workerPool.Count; ++i)
                {
                    worker = _workerPool[i];

                    Monitor.Enter(worker);  //++

                    existTask = worker.getCurrentTask();
                    if (existTask != null && existTask.getChannel() == channel)
                    {
                        worker.cancelCurrentTask();
                        //Monitor.Exit(worker);  //--

                        ++cancelNum;
                        //break;
                    }

                    Monitor.Exit(worker);  //--
                }
            }


            //if (cancelNum <= 0)
            {
                // search response queue
                Monitor.Enter(_responseQueue);  //++

                for (int i = _responseQueue.Count - 1; i >= 0; --i)
                {
                    existTask = _responseQueue[i];

                    if (existTask.getChannel() == channel)
                    {
                        _responseQueue.RemoveAt(i);
                        ++cancelNum;
                        //break;
                    }
                }  // for

                Monitor.Exit(_responseQueue);  //--
            }

            return cancelNum;
        }

        public int cancelAllTasks()
        {
            int cancelNum = 0;

            // search task queue
            if (_taskQueue.Count > 0)
            {
                Monitor.Enter(_taskQueue);  //++
                _taskQueue.Clear();
                Monitor.Exit(_taskQueue);  //--
            }


            //if (cancelNum <= 0)
            {
                // search current task
                AsyncWorker worker = null;
                for (int i = 0; i < _workerPool.Count; ++i)
                {
                    worker = _workerPool[i];

                    Monitor.Enter(worker);  //++
                    worker.cancelCurrentTask();
                    Monitor.Exit(worker);  //--
                }
            }


            //if (cancelNum <= 0)
            {
                // search response queue
                Monitor.Enter(_responseQueue);  //++
                _responseQueue.Clear();
                Monitor.Exit(_responseQueue);  //--
            }

            return cancelNum;
        }

        public void reset()
        {
            cancelAllTasks();

            AsyncWorker worker = null;
            for (int i = 0; i < _workerPool.Count; ++i)
            {
                worker = _workerPool[i];
                worker.signalForTerminate();
            }

            _workerPool.Clear();
        }

        internal void postResponses()
        {
            sbyte recvNum = 0;
            IList<AsyncTask> respList = null;

            if (_responseQueue.Count > 0 && _postNumOnce > 0)
            {
                respList = new Deque<AsyncTask>();

                Monitor.Enter(_responseQueue);  //++

                while (_responseQueue.Count > 0 && recvNum < _postNumOnce)
                {
                    respList.Add(_responseQueue[0]);
                    _responseQueue.RemoveAt(0);

                    ++recvNum;
                }

                Monitor.Exit(_responseQueue);  //--
            }


            if (respList != null)
            {
                AsyncTask task = null;
                for (int i = 0; i < respList.Count; ++i)
                {
                    task = respList[i];
                    Debug.Assert(task != null && task.onCompleteTask != null);

                    task.onCompleteTask(task, task.errorCode, task.resultDict);
                    task.resultDict = null;
                }
            }
        }

        internal int terminateIdleWorkers(DateTime nowUTC)
        {
            int terminateNum = 0;

            if (_idleWorkers.Count > 0 && _workerPool.Count > _leastWorkerNum && terminateNum < _killIdleNumOnce)
            {
                AsyncWorker worker = null;
                TimeSpan delta;
                int i;

                Monitor.Enter(_idleWorkers);  // ++

                while (_idleWorkers.Count > 0 
                        && _workerPool.Count > _leastWorkerNum 
                        && terminateNum < _killIdleNumOnce)
                {
                    worker = _idleWorkers[0];
                    delta = nowUTC - worker.getIdleTimestamp();

                    if (delta.TotalSeconds < 0 || delta.TotalSeconds > _idleTimeout)
                    {
                        //if (worker.status == AsyncWorker.Status.WaitingTask)
                        {
                            worker.signalForTerminate();
                            //_idleWorkers.RemoveAt(0);

                            for (i = 0; i < _workerPool.Count; ++i)
                            {
                                if (worker == _workerPool[i])
                                {
                                    _workerPool.RemoveAt(i);
                                    break;
                                }
                            }

                            ++terminateNum;
                        }
                    }
                    else
                    {
                        break;
                    }
                }  // while

                Monitor.Exit(_idleWorkers);  // --
            }

            return terminateNum;
        }

        internal IList<AsyncWorker> getIdleWorkerList()
        {
            return _idleWorkers;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal AsyncTask popTask()
        {
            AsyncTask task = null;

            //if (_taskQueue.Count > 0)
            {
                Monitor.Enter(_taskQueue);  //++
                if (_taskQueue.Count > 0)
                {
                    task = _taskQueue[0];
                    _taskQueue.RemoveAt(0);
                }
                Monitor.Exit(_taskQueue);  //--
            }

            return task;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal IList<AsyncTask> getResponseQueue()
        {
            return _responseQueue;
        }

        /// <summary>
        /// 
        /// </summary>
        private void wakeupWorker()
        {
            AsyncWorker worker = null;

            if (_idleWorkers.Count > 0)
            {
                Monitor.Enter(_idleWorkers);  //++
                worker = _idleWorkers[0];
                _idleWorkers.RemoveAt(0);
                Monitor.Exit(_idleWorkers);  //--

                Monitor.Enter(worker);  //++
                worker.status = AsyncWorker.Status.Working;
                Monitor.Pulse(worker);  // Pulse
                Monitor.Exit(worker);  //--
            }
            else
            {
                /*
                for (int i = 0; i < _workerPool.Count; ++i)
                {
                    worker = _workerPool[i];
                    
                    if (worker.status == AsyncWorker.Status.Ready)
                        break;
                    else
                        worker = null;
                }  // for

                if (worker != null)
                {
                    worker.launch();
                }
                else
                //*/
                    if (_workerPool.Count < _maxConcurrent)
                {
                    //Log.debug("launch new worker!!!!!!!!!!!!!!!!!!!");
                    worker = new AsyncWorker(this);
                    worker.launch();
                    _workerPool.Add(worker);
                }
            }
        }

        private sbyte _maxConcurrent;
        private float _idleTimeout;
        private int _killIdleNumOnce;
        private sbyte _leastWorkerNum;
        private int _postNumOnce;

        private IList<AsyncTask> _taskQueue;  // 从开头取
        private IList<AsyncTask> _responseQueue;  // 从开头取
        private IList<AsyncWorker> _workerPool;
        private IList<AsyncWorker> _idleWorkers;
    }
}