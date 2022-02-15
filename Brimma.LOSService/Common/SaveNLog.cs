using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Brimma.LOSService.Common
{
    public class SaveNLog
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public void SaveLogFile(string className, string methodName, string errStackRace, string errMsg)
        {
            string errMessage = System.Environment.NewLine + "Error Occured at: " + DateTime.Now + System.Environment.NewLine;
            errMessage = errMessage + "Class: " + className + System.Environment.NewLine;
            errMessage = errMessage + "Method: " + methodName + System.Environment.NewLine;
            errMessage = errMessage + "Error Message: " + errMsg + System.Environment.NewLine;
            errMessage = errMessage + "Error StackTrace: " + errStackRace + System.Environment.NewLine;
            logger.Error(errMessage);
        }
    }
}
