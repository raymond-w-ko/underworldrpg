using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace UnderworldEngine.IO
{
    class KeyboardManager
    {
        public Dictionary<string, string> KeyToAction;
        KeyboardState current;
        HashSet<Keys> prev;

        public KeyboardManager()
        {
            KeyToAction = new Dictionary<string, string>();
            foreach (string k in Enum.GetNames(typeof(Keys)))
            {
                KeyToAction[k.ToLower()] = "None";
            }

            prev = new HashSet<Keys>();
        }

        public void UpdateInput()
        {
            //do nothing if console is open
            if (Game1.console.IsOpen)
                return;

            //get state
            current = Keyboard.GetState();
            Keys[] pressed = current.GetPressedKeys();
            //check to see if any keys were unpressed before
            for (int i = 0; i < pressed.Length; i++)
            {
                if (!prev.Contains(pressed[i]))
                {
                    try
                    {
                        string key = Enum.GetName(typeof(Keys), pressed[i]).ToLower();
                        Game1.interpreter.run(KeyToAction[key]);
                    }
                    catch (Exception e)
                    {
                        Game1.console.Log(e.Message);
                    }
                }
            }

            prev = new HashSet<Keys>();
            for (int i = 0; i < pressed.Length; i++)
            {
                prev.Add(pressed[i]);
            }
        }
    }
}
