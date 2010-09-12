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

        #region dpadCommands
        public string ddowncommand;
        public string dleftcommand;
        public string drightcommand;
        public string dupcommand;
        public string ddownrightcommand;
        public string ddownleftcommand;
        public string duprightcommand;
        public string dupleftcommand;
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
                #region Buttons
                if (current.Buttons.A == ButtonState.Pressed &&
                    prev.Buttons.A == ButtonState.Released)
                {
                    Game1.interpreter.run(this.acommand);
                }
                else if (current.Buttons.B == ButtonState.Pressed &&
                    prev.Buttons.B == ButtonState.Released)
                {
                    Game1.interpreter.run(this.bcommand);
                }
                else if (current.Buttons.X == ButtonState.Pressed &&
                    prev.Buttons.X == ButtonState.Released)
                {
                    Game1.interpreter.run(this.xcommand);
                }
                else if (current.Buttons.Y == ButtonState.Pressed &&
                    prev.Buttons.Y == ButtonState.Released)
                {
                    Game1.interpreter.run(this.ycommand);
                }
                else if (current.Buttons.Back == ButtonState.Pressed &&
                    prev.Buttons.Back == ButtonState.Released)
                {
                    Game1.interpreter.run(this.backcommand);
                }
                else if (current.Buttons.Start == ButtonState.Pressed &&
                    prev.Buttons.Start == ButtonState.Released)
                {
                    Game1.interpreter.run(this.startcommand);
                }
                else if (current.Buttons.LeftShoulder == ButtonState.Pressed &&
                    prev.Buttons.LeftShoulder == ButtonState.Released)
                {
                    Game1.interpreter.run(this.lshcommand);
                }
                else if (current.Buttons.LeftStick == ButtonState.Pressed &&
                    prev.Buttons.LeftStick == ButtonState.Released)
                {
                    Game1.interpreter.run(this.lscommand);
                }
                else if (current.IsButtonDown(Buttons.LeftTrigger) &&
                    prev.IsButtonUp(Buttons.LeftTrigger))
                {
                    Game1.interpreter.run(this.ltcommand);
                }
                else if (current.Buttons.RightShoulder == ButtonState.Pressed &&
                    prev.Buttons.RightShoulder == ButtonState.Released)
                {
                    Game1.interpreter.run(this.rshcommand);
                }
                else if (current.Buttons.RightStick == ButtonState.Pressed &&
                    prev.Buttons.RightStick == ButtonState.Released)
                {
                    Game1.interpreter.run(this.rtcommand);
                }
                else if (current.IsButtonDown(Buttons.RightTrigger) &&
                    prev.IsButtonUp(Buttons.RightTrigger))
                {
                    Game1.interpreter.run(this.rtcommand);
                }
                #endregion

                #region DPad
                if (current.IsButtonDown(Buttons.DPadUp) && current.IsButtonDown(Buttons.DPadLeft) &&
                    prev.IsButtonUp(Buttons.DPadUp) && prev.IsButtonUp(Buttons.DPadLeft))
                {
                    Game1.interpreter.run(this.dupleftcommand);
                }
                else if (current.IsButtonDown(Buttons.DPadUp) && current.IsButtonDown(Buttons.DPadRight) &&
                    prev.IsButtonUp(Buttons.DPadUp) && prev.IsButtonUp(Buttons.DPadRight))
                {
                    Game1.interpreter.run(this.duprightcommand);
                }
                else if (current.IsButtonDown(Buttons.DPadDown) && current.IsButtonDown(Buttons.DPadRight) &&
                    prev.IsButtonUp(Buttons.DPadDown) && prev.IsButtonUp(Buttons.DPadRight))
                {
                    Game1.interpreter.run(this.ddownrightcommand);
                }
                else if (current.IsButtonDown(Buttons.DPadDown) && current.IsButtonDown(Buttons.DPadLeft) &&
                    prev.IsButtonUp(Buttons.DPadUp) && prev.IsButtonUp(Buttons.DPadLeft))
                {
                    Game1.interpreter.run(this.ddownleftcommand);
                }
                else if (current.IsButtonDown(Buttons.DPadUp) &&
                    prev.IsButtonUp(Buttons.DPadUp))
                {
                    Game1.interpreter.run(this.dupcommand);
                }
                else if (current.IsButtonUp(Buttons.DPadDown) &&
                    prev.IsButtonUp(Buttons.DPadDown))
                {
                    Game1.interpreter.run(this.ddowncommand);
                }
                else if (current.IsButtonDown(Buttons.DPadLeft) &&
                    prev.IsButtonUp(Buttons.DPadLeft))
                {
                    Game1.interpreter.run(this.dleftcommand);
                }
                else if (current.IsButtonDown(Buttons.DPadRight) &&
                    prev.IsButtonUp(Buttons.DPadRight))
                {
                    Game1.interpreter.run(this.drightcommand);
                }
                #endregion

                prev = current;
            }
        }
    }
}
