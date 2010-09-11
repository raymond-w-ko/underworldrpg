using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnderworldEngine.Scripting
{
    interface IInterpretable
    {
        public void run(string function);
    }
}
