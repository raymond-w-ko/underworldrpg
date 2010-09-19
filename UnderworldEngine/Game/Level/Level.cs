using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnderworldEngine.Game.Level
{
    class Level : UnderworldEngine.GameState.IScreen
    {
        private Map _map;
        public Level()
        {
            ;
        }

        #region IScreen Members

        public bool IsFocused
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void Unload()
        {
            throw new NotImplementedException();
        }

        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public void Draw()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
