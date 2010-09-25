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

        public static ZeroArg PrimaryDispatch;
        public static ZeroArg AltDispatch;
        public static ZeroArg CancelDispatch;
        public static ZeroArg MenuDispatch;

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
                // this would the be A key on an XBOX360 or the Cross on PS3
                case "primary":
                    PrimaryDispatch();
                    break;
                // this would be the X key on an XBOX360 or the Square on PS3
                case "alt":
                    AltDispatch();
                    break;
                // this would be the B key on an XBOX360 or the Circle on PS3
                case "cancel":
                    CancelDispatch();
                    break;
                // this would be the Y key on an XBOX360 or the Triangle on PS3
                case "menu":
                    MenuDispatch();
                    break;
            }
        }
    }
}
