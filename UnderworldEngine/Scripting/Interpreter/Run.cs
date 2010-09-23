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
            string[] command = function.Split(Game1.interpreter.Mask);

            RageScript rs = Game1.DefaultContent.Load<RageScript>("./Scripts/" + command[1]);
            StreamReader reader = new StreamReader(rs.stream);

            LinkedList<string> file = new LinkedList<string>();

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine().TrimStart().TrimEnd();
                file.AddFirst(line);
            }

            foreach (string line in file) {
                Game1.interpreter.runFirst(line);
            }

            rs.Unload();
        }

        #endregion
    }
}
