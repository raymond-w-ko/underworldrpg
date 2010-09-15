﻿using System;
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

            RageScript rs = Game1.DefaultContent.Load<RageScript>("./Scripts/" + command[1]);
            StreamReader reader = new StreamReader(rs.stream);

            while (!reader.EndOfStream)
            {
                Game1.interpreter.run(reader.ReadLine().TrimStart().TrimEnd());
            }

            rs.Unload();
        }

        #endregion
    }
}
