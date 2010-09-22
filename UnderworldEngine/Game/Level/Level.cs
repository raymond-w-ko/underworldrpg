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
        private BattleCursor _battleCursor;

        public Level(string mapName)
        {
            this._map = new Map(mapName);
            _entityList = new LinkedList<Entity>();
            _battleCursor = new BattleCursor(_map.Grid, "Textures/grid_overlay", "Textures/grid_overlay");
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

            _battleCursor.Unload();
        }

        public void Update(GameTime gameTime)
        {
            _map.Update(gameTime);

            foreach (Entity entity in _entityList) {
                entity.Update(gameTime);
            }

            _battleCursor.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            _map.Draw(gameTime);

            foreach (Entity entity in _entityList) {
                entity.Draw(gameTime);
            }

            _battleCursor.Draw(gameTime);
        }
    }
}
