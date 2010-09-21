using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

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

        public void SwitchOn(string name)
        {
            nameToScreen[name].IsFocused = true;
        }

        public void SwitchOff(string name)
        {
            nameToScreen[name].IsFocused = false;
        }

        public void Remove(string name)
        {
            if (name == "level" && nameToScreen[name] == null) {
                return;
            }
            nameToScreen[name].Unload();
            nameToScreen.Remove(name);
        }

        ~ScreenManager()
        {
            foreach (IScreen screen in nameToScreen.Values) {
                screen.Unload();
            }
        }

        /// <summary>
        /// Update the IScreen that is in focus
        /// </summary>
        public void Update(GameTime gameTime)
        {
            foreach(IScreen sc in nameToScreen.Values)
            {
                if (sc.IsFocused)
                {
                    sc.Update(gameTime);
                }
            }
        }

        /// <summary>
        /// Draw the IScreen that is in focus
        /// </summary>
        public void Draw(GameTime gameTime)
        {
            foreach (IScreen sc in nameToScreen.Values)
            {
                if (sc.IsFocused)
                {
                    sc.Draw(gameTime);
                }
            }
        }
    }
}
