using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace UnderworldEngine.Game
{
    class Level : UnderworldEngine.GameState.IScreen
    {
        private Map _map;
        private LinkedList<Entity> _entityList;

        public Level(string mapName)
        {
            this._map = new Map(mapName);
            _entityList = new LinkedList<Entity>();
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

            foreach (Entity entity in _entityList) {
                entity.Unload();
            }
        }

        public void Update(GameTime gameTime)
        {
            _map.Update(gameTime);

            foreach (Entity entity in _entityList) {
                entity.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime)
        {
            _map.Draw(gameTime);

            foreach (Entity entity in _entityList) {
                entity.Draw(gameTime);
            }
        }
    }
}
