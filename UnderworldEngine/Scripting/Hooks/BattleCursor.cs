using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnderworldEngine.Scripting
{
    class BattleCursor : IInterpretable
    {
        public delegate void ZeroArg();
        public static ZeroArg UpDispatch;
        public static ZeroArg DownDispatch;
        public static ZeroArg LeftDispatch;
        public static ZeroArg RightDispatch;

        public void run(string function)
        {
            string[] command = function.Split(Game1.interpreter.Mask);
            switch (command[1]) {
                case "up":
                    UpDispatch();
                    break;
                case "down":
                    DownDispatch();
                    break;
                case "left":
                    LeftDispatch();
                    break;
                case "right":
                    RightDispatch();
                    break;
            }
        }
    }
}
