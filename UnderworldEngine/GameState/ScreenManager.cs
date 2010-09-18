﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnderworldEngine.GameState
{
    class ScreenManager
    {
        Dictionary<string, IScreen> nameToScreen;

        public ScreenManager()
        {
            nameToScreen = new Dictionary<string, IScreen>();
        }

        public void AddScreen(string name, IScreen screen)
        {
            nameToScreen[name] = screen;
        }

        /// <summary>
        /// Switches focus to a screen
        /// </summary>
        /// <param name="name">the screen we want to focus on</param>
        public void SwitchTo(string name)
        {
            foreach (IScreen sc in nameToScreen.Values)
            {
                sc.IsFocused = false;
            }

            nameToScreen[name].IsFocused = true;
        }

        /// <summary>
        /// Update the IScreen that is in focus
        /// </summary>
        public void Update()
        {
            foreach(IScreen sc in nameToScreen.Values)
            {
                if (sc.IsFocused)
                {
                    sc.Update();
                    return;
                }
            }
        }

        /// <summary>
        /// Draw the IScreen that is in focus
        /// </summary>
        public void Draw()
        {
            foreach (IScreen sc in nameToScreen.Values)
            {
                if (sc.IsFocused)
                {
                    sc.Draw();
                    return;
                }
            }
        }
    }
}
