using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Threading.Tasks
{
    public static class TaskExtensions
    {
        /// <summary>
        /// https://johnthiriet.com/removing-async-void/#
        /// </summary>
        public static async void WaitAsync(this Task task, Action<Exception> handleError = null)
        {
            try
            {
                await task;
            }
            catch (Exception error)
            {
                if (handleError == null) throw;
                else
                    handleError.Invoke(error);
            }
        }
    }
}
