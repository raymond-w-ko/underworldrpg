using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace UnderworldEngine.GameState
{
    interface IScreen
    {
        bool IsFocused
        {
            get;
            set;
        }

        void Unload();
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime);
    }
}
