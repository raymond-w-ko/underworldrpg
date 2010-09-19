using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnderworldEngine.Game.Level
{
    class Level : UnderworldEngine.GameState.IScreen
    {
        private Map _map;

        public Level(string mapName)
        {
            this._map = new Map(mapName);
        }

        public bool _isFocused;
        public bool IsFocused
        {
            get
            {
                return _isFocused;
            }
            set
            {
                _isFocused = value;
            }
        }

        public void Unload()
        {
            _map.Unload();
        }

        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            _map.Update(gameTime);
        }

        public void Draw()
        {
            _map.Draw();
        }
    }
}
