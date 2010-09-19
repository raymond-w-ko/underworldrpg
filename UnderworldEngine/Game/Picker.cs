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
            if ((grid.X == float.NaN || grid.Y == float.NaN)) {
                return;
            }

            _grid.Select(grid);
        }

        /*
        private void stuff()
        {
            Vector3 pos = ray.Position;
            Vector3 dir = ray.Direction;

            float x = (-pos.Y) / dir.Y;
            Vector3 final = pos + (x * dir);
        }
        */

        /*
        private Ray UpdatePickRay()
        {
            MouseState mouseState = Mouse.GetState();
            Ray pickRay;

            Vector3 v;
            v.X = (((2.0f * mouseState.X) / Game1.DefaultGraphicsDevice.Viewport.Width) - 1);
            v.Y = -(((2.0f * mouseState.Y) / Game1.DefaultGraphicsDevice.Viewport.Height) - 1);
            v.Z = 0.0f;

            pickRay.Position.X = Game1.Camera.ViewInverseMatrix.M41;
            pickRay.Position.Y = Game1.Camera.ViewInverseMatrix.M42;
            pickRay.Position.Z = Game1.Camera.ViewInverseMatrix.M43;
            pickRay.Direction = Vector3.Normalize(
                Vector3.Transform(v, Game1.Camera.ViewProjectionInverseMatrix) - pickRay.Position
                );

            return pickRay;
        }
        */
    }
}
