using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace UnderworldEngine.GameState
{
    class BlankScreen : IScreen
    {
        #region IScreen Members

        bool isFocused;
        public bool IsFocused
        {
            get
            {
                return isFocused;
            }
            set
            {
                isFocused = value;
            }
        }

        public void Unload()
        {
            //nothing to unload
        }

        public void Update(GameTime gameTime)
        {
            //nothing to update
        }

        public void Draw(GameTime gameTime)
        {
            Game1.DefaultGraphicsDevice.Clear(Color.Black);
        }

        #endregion
    }
}
