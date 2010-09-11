using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace UnderworldEngine.Scripting
{
    class Run : IInterpretable
    {
        #region IInterpretable Members

        public void run(string function)
        {
            string[] command = function.Split(new Char[] { ' ' });

            StreamReader reader = new StreamReader(".\\Content\\Scripts\\" + command[1]);

            while (!reader.EndOfStream)
            {
                Game1.interpreter.run(reader.ReadLine().TrimStart().TrimEnd());
            }
        }

        #endregion
    }
}
