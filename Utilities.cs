using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncVoid
{
    public static class Utilities
    {
        public static async void FireAndForget(this Task task, Action<Exception> onErrors)
        {
            try
            {
                await task;
            }
            catch (Exception ex)
            {
                onErrors(ex);
            }
        }
    }
}
