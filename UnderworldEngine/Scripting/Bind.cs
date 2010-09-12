﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnderworldEngine.Scripting
{
    class Bind : IInterpretable
    {
        #region IInterpretable Members

        public void run(string function)
        {
            string[] command = function.Split(new Char[] { ' ' });
            string keyToBind = command[1];
            StringBuilder bindingTemp = new StringBuilder();
            string binding;
            for (int i = 2; i < command.Length; i++)
            {
                bindingTemp.Append(command[i]);
                bindingTemp.Append(" ");
            }

            keyToBind = keyToBind.ToLower();
            binding = bindingTemp.ToString();
            switch (keyToBind)
            {
                #region Buttons
                case "a":
                    Game1.controller1.acommand = binding; 
                    break;
                case "b":
                    Game1.controller1.bcommand = binding;
                    break;
                case "x":
                    Game1.controller1.xcommand = binding;
                    break;
                case "y":
                    Game1.controller1.ycommand = binding;
                    break;
                case "back":
                    Game1.controller1.backcommand = binding;
                    break;
                case "start":
                    Game1.controller1.startcommand = binding;
                    break;
                case "leftshoulder":
                    Game1.controller1.lshcommand = binding;
                    break;
                case "leftstick":
                    Game1.controller1.lscommand = binding;
                    break;
                case "lefttrigger":
                    Game1.controller1.ltcommand = binding;
                    break;
                case "rightshoulder":
                    Game1.controller1.rshcommand = binding;
                    break;
                case "rightstick":
                    Game1.controller1.rscommand = binding;
                    break;
                case "righttrigger":
                    Game1.controller1.rtcommand = binding;
                    break;
                #endregion

                #region DPad
                case "dpadup":
                    Game1.controller1.dupcommand = binding;
                    break;
                case "dpaddown":
                    Game1.controller1.ddowncommand = binding;
                    break;
                case "dpadleft":
                    Game1.controller1.dleftcommand = binding;
                    break;
                case "dpadright":
                    Game1.controller1.drightcommand = binding;
                    break;
                case "dpadupleft":
                    Game1.controller1.dupleftcommand = binding;
                    break;
                case "dpadupright":
                    Game1.controller1.duprightcommand = binding;
                    break;
                case "dpaddownright":
                    Game1.controller1.ddownrightcommand = binding;
                    break;
                case "dpaddownleft":
                    Game1.controller1.ddownleftcommand = binding;
                    break;
                #endregion
            }
        }

        #endregion
    }
}
