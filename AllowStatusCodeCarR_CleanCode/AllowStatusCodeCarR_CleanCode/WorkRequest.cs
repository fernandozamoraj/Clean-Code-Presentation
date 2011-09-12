using System;

namespace AllowStatusCodeCarR_CleanCode
{
    public class WorkRequest
    {
        private string _currentStatus;

        public bool LastStatusWasCompleted()
        {
            return false;    
        }

        public WorkRequestType WorkRequestType { get; set; }

        public string CanBeClosed{
            get
            {
                return DetermineIfCanBeClosedMessage();
            }
        }

        private string DetermineIfCanBeClosedMessage()
        {
            if(_currentStatus == "X" || _currentStatus == "Y" || _currentStatus == "Z")
                return string.Empty;

            return "Cannot be closed because it is in status " + _currentStatus;            
        }

        public string CurrentStatus
        {
            get { return _currentStatus; }
        }

        internal void SetCurrentStatus(string statusCode)
        {
            _currentStatus = statusCode;
        }
    }
}
