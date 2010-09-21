using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using UnderworldEngine.Game;

namespace UnderworldEngine.Graphics
{
    class Picker
    {
        private Grid _grid;
        public Picker(Grid grid)
        {
            _grid = grid;
        }

        public void Update(GameTime gameTime)
        {
            _mouseStatePrev = _mouseStateCurrent;
            _mouseStateCurrent = Mouse.GetState();

            Vector3 nearSource = new Vector3(_mouseStateCurrent.X, _mouseStateCurrent.Y, 0);
            Vector3 farSource = nearSource;
            farSource.Z = Game1.Camera.NearPlaneDistance;

            Vector3 nearPoint = Game1.DefaultGraphicsDevice.Viewport.Unproject(nearSource,
                Game1.Camera.ProjectionMatrix,
                Game1.Camera.ViewMatrix,
                Matrix.Identity);
            Vector3 farPoint = Game1.DefaultGraphicsDevice.Viewport.Unproject(farSource,
                Game1.Camera.ProjectionMatrix,
                Game1.Camera.ViewMatrix,
                Matrix.Identity);

            Vector3 direction = farPoint - nearPoint;
            direction.Normalize();

            Ray ray = new Ray(nearPoint, direction);

            if (_mouseStatePrev.LeftButton == ButtonState.Pressed &&
                _mouseStateCurrent.LeftButton == ButtonState.Released) {
                Click(ray);
            }
        }

        private MouseState _mouseStateCurrent;
        private MouseState _mouseStatePrev;

        private void Click(Ray ray)
        {
            Vector2 grid = _grid.FindIntersection(ray);
            Game1.console.Log(grid.ToString());
            if ((float.IsNaN(grid.X) || float.IsNaN(grid.Y))) {
                return;
            }

            _grid.Select(grid);
        }
    }
}
