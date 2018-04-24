using System;

namespace Siteimprove.Integration.Sitefinity.Mvc.ViewModels
{
    /// <summary>
    /// ViewModel for displaying Exception and other errors onto a View
    /// </summary>
    public class ErrorViewModel
    {
        public ErrorViewModel(Exception exception)
        {
            this.Message = exception.Message;
            this.StackTrace = exception.StackTrace;
        }

        public string Message { get; set; }

        public string StackTrace { get; set; }
    }
}
