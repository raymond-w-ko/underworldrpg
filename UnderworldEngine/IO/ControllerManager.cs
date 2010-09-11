using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace UnderworldEngine.IO
{
    class ControllerManager
    {
        #region buttonCommands
        public string acommand;
        public string bcommand;
        public string backcommand;
        public string ddowncommand;
        public string dleftcommand;
        public string drightcommand;
        public string dupcommand;
        public string lscommand;
        public string lshcommand;
        public string ltcommand;
        public string rscommand;
        public string rshcommand;
        public string rtcommand;
        public string startcommand;
        public string ycommand;
        public string xcommand;
        #endregion

        GamePadState current;
        GamePadState prev;
        PlayerIndex index;
        public PlayerIndex PlayerIndex
        {
            get{ return index; }
        }

        public ControllerManager(PlayerIndex index)
        {
            this.index = index;
            prev = GamePad.GetState(index);
        }

        public void UpdateInput()
        {
            current = GamePad.GetState(index);
            if (current.IsConnected)
            {
                if (current.Buttons.A == ButtonState.Pressed &&
                    prev.Buttons.A == ButtonState.Released)
                {
                    Game1.interpreter.run(this.acommand);
                }

                if (current.Buttons.B == ButtonState.Pressed &&
                    prev.Buttons.B == ButtonState.Released)
                {
                    Game1.interpreter.run(this.bcommand);
                }

                prev = current;
            }
        }
    }
}
