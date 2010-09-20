using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            _map.Update(gameTime);

            foreach (Entity entity in _entityList) {
                entity.Update(gameTime);
            }
        }

        public void Draw()
        {
            _map.Draw();

            foreach (Entity entity in _entityList) {
                entity.Draw();
            }
        }
    }
}
