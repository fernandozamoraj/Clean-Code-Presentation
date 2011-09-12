using System;

namespace AllowStatusCodeCarR_CleanCode
{
    public class WorkRequestHistory
    {
        private WorkRequest _workRequest;
        private string _availableFundsAreExceededMessage;

        public void ChangeStatusCodeToR()
        {
            if(AllowsStatusCdeR("R") == string.Empty)
            {
                DoSomethingToHonorRequest();
            }
            else
            {
                RejectRequest();
            }
        }

        private void RejectRequest()
        {
            //..
        }

        private void DoSomethingToHonorRequest()
        {
            //..
        }

        private string AllowsStatusCdeR(string statusCode)
        {
            string errMessage = string.Empty;

            if (!_workRequest.LastStatusWasCompleted())
            {
                if (!string.IsNullOrEmpty(_availableFundsAreExceededMessage))
                {
                    errMessage = _availableFundsAreExceededMessage;
                }
                else if (_workRequest.WorkRequestType == WorkRequestType.Internal)
                {
                    if (_workRequest.CanBeClosed != string.Empty)
                    {
                        //checks may only be set if Work Order is "closable" when an ORG type rule
                        errMessage = _workRequest.CanBeClosed;
                    }
                    else
                    {
                        errMessage = CheckForSomeOtherRandomCondition(statusCode);
                    }
                }
                else if (!_workRequest.LastStatusWasCompleted())
                {
                    errMessage = GetMustBePrecedByXMessage(statusCode);
                }
            }

            return errMessage;
        }

        private string GetMustBePrecedByXMessage(string statusCode)
        {
            const string statusCodesRequiredForR = "S, C, T, C, W, X";
            const string statusCodesRequiredForU = "R";

            switch (statusCode)
            {
                case "R":
                    if (statusCodesRequiredForR.IndexOf(statusCode) > -1)
                        return string.Empty;
                    
                    return "Must be preceded by one of the following " + statusCodesRequiredForR;
                    
                case "U":
                    if (statusCodesRequiredForU.IndexOf(statusCode) > -1)
                        return string.Empty;

                    return "Must be preceded by one of the following " + statusCodesRequiredForR;

            }

            return "Is not preceded by the proper status.";
        }

        private string CheckForSomeOtherRandomCondition(object o)
        {
            return string.Empty;
        }
    }
}