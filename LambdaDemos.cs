using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncVoid
{
    class LambdaDemos
    {

        public void Method()
        {
            Action foo = async () => await Task.Yield();
            Func<Task> bar = async () => await Task.Yield();

            APIMethod(async () => await Task.Yield()); // resolves to?
        }

        public void APIMethod(Action act)
        {
            act();
        }

        public async Task APIMethod(Func<Task> act)
        {
            await act();
        }
    }
}
