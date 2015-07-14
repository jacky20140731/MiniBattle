using System.Collections.Generic;
using UnityEngine;

namespace th.nx
{
    public delegate int OnHandleTask(AsyncTask task, IDictionary<int, object> oResult);
    public delegate void OnCompleteTask(AsyncTask task, int errCode, IDictionary<int, object> result);

    public class AsyncTask
    {
        public AsyncTask()
                : this(0)
        {

        }

        public AsyncTask(int taskID)
        {
            this.taskID = taskID;
            _paramDict = new Dictionary<int, object>();
        }

        public int taskID
        {
            get{ return _taskID; }
            set{ _taskID = value; }
        }

        public sbyte getPrior()
        {
            return _prior;
        }

        public sbyte getChannel()
        {
            return _channel;
        }

        public IDictionary<int, object> getParamDict()
        {
            return _paramDict;
        }

        public OnHandleTask onHandleTask
        {
            get { return _onHandleTask; }
            set { _onHandleTask = value; }
        }

        public OnCompleteTask onCompleteTask
        {
            get { return _onCompleteTask; }
            set { _onCompleteTask = value; }
        }


        internal void setPrior(sbyte prior)
        {
            _prior = prior;
        }

        internal void setChannel(sbyte channel)
        {
            _channel = channel;
        }

        internal IDictionary<int, object> resultDict
        {
            get { return _resultDict; }
            set { _resultDict = value;  }
        }


        internal int errorCode
        {
            get { return _errorCode; }
            set { _errorCode = value; }
        }


        private int _taskID;
        private IDictionary<int, object> _paramDict;
        private IDictionary<int, object> _resultDict;
        private OnHandleTask _onHandleTask;
        private OnCompleteTask _onCompleteTask;

        private sbyte _channel;
        private sbyte _prior;

        private volatile int _errorCode;
    }
}
