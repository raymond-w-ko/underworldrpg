using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UnderworldEngine.Game
{
    public class BattleCursor
    {
        private Grid _grid;

        private int _xLimit;
        private int _zLimit;

        private Vector2 _gridPosition;
        private Vector2 _futureGridPosition;

        private float _movementSpeed;
        /// <summary>
        /// Battle cursor movement speed is in terms of Direct X units per second.
        /// </summary>
        public float MovementSpeed
        {
            get
            {
                return _movementSpeed;
            }
            set
            {
                _movementSpeed = (value / 60.0f);
            }
        }

        private BillboardQuadTexture _cursor;
        private float _bobFactor;
        public float BobFactor
        {
            get
            {
                return _bobFactor;
            }
            set
            {
                if (value < 0 || value > _xLimit / 2.0f ||
                    value > _zLimit / 2.0f
                    ) {
                    throw new ApplicationException("Invalid Bob Factor specified");
                }
                _bobFactor = value;
            }
        }

        private QuadTexture _overlay;

        float _timer;
        float _minAlpha;
        float _maxAlpha;
        float _direction;

        

        public BattleCursor(Grid grid,
            string cursorTextureName, string gridOverlayTextureName,
            Vector2 gridPosition
            )
        {
            _grid = grid;
            Vector2 dimensions = _grid.Dimensions;
            _xLimit = (int)dimensions.X;
            _zLimit = (int)dimensions.Y;
            _futureGridPosition = _gridPosition = gridPosition;

            // We want camera to lag behind a little bit, similar to Disgaea
            MovementSpeed = Game1.Camera.MoveSpeed * 1.10f;

            // Create Cursor/Pointer/Floating Hand
            _cursor = new BillboardQuadTexture(
                new Vector3(0f, 0f, 0f),
                1.0f,
                "Textures/cursor");
            _cursor.Scale = .75f ;
            _cursor.EnableAlphaBlending(BlendFunction.Add, Blend.SourceAlpha, Blend.InverseSourceAlpha);
            _bobFactor = .5f;

            _overlay = new QuadTexture(
                new Vector3(0f, 0f, 0f),
                Vector3.Up,
                Vector3.Forward,
                1.0f, 1.0f,
                gridOverlayTextureName
                );
            _overlay.EnableAlphaBlending(
                BlendFunction.Add,
                Blend.SourceAlpha,
                Blend.One
                );
            _overlay.Alpha = 1.0f;

            _timer = 0;
            _minAlpha = .2f;
            _maxAlpha = .6f;
            _direction = +1f;

            registerWithScripter();

            updatePosition();
        }

        #region Scripting support
        private void registerWithScripter()
        {
            // register binds
            UnderworldEngine.Scripting.BattleCursor.UpDispatch += this.up;
            UnderworldEngine.Scripting.BattleCursor.DownDispatch += this.down;
            UnderworldEngine.Scripting.BattleCursor.LeftDispatch += this.left;
            UnderworldEngine.Scripting.BattleCursor.RightDispatch += this.right;
        }

        private void up()
        {
            if (!Game1.Camera.IsAcceptingCommands) {
                return;
            }

            Vector3 offset = Game1.Camera.GetRelativeDirectionOffset(
                UnderworldEngine.Graphics.Camera.Direction.Up
                );
            _futureGridPosition.X += offset.X;
            _futureGridPosition.Y += offset.Z;

            Game1.Camera.LookUp();
        }

        private void down()
        {
            if (!Game1.Camera.IsAcceptingCommands) {
                return;
            }

            Vector3 offset = Game1.Camera.GetRelativeDirectionOffset(
                UnderworldEngine.Graphics.Camera.Direction.Down
                );
            _futureGridPosition.X += offset.X;
            _futureGridPosition.Y += offset.Z;

            Game1.Camera.LookDown();
        }

        private void left()
        {
            if (!Game1.Camera.IsAcceptingCommands) {
                return;
            }

            Vector3 offset = Game1.Camera.GetRelativeDirectionOffset(
                UnderworldEngine.Graphics.Camera.Direction.Left
                );
            _futureGridPosition.X += offset.X;
            _futureGridPosition.Y += offset.Z;

            Game1.Camera.LookLeft();
        }

        private void right()
        {
            if (!Game1.Camera.IsAcceptingCommands) {
                return;
            }

            Vector3 offset = Game1.Camera.GetRelativeDirectionOffset(
                UnderworldEngine.Graphics.Camera.Direction.Right
                );
            _futureGridPosition.X += offset.X;
            _futureGridPosition.Y += offset.Z;

            Game1.Camera.LookRight();
        }
        #endregion

        private void tick()
        {
            if (_direction > 0) {
                if (_timer > 37.5f) {
                    _direction = -1f;
                }
                else {
                    _timer++;
                }
            }
            else if (_direction < 0) {
                if (_timer < 0) {
                    _direction = 1f;
                }
                else {
                    _timer--;
                }
            }
        }

        private void updatePosition()
        {
            GridSquare gs = _grid.GetGridSquare(
                (uint)Math.Round(_gridPosition.X), (uint)Math.Round(_gridPosition.Y)
                );

            _overlay.CalculateVertices(
                new Vector3(_gridPosition.X + .5f, gs.Height + .0001f, _gridPosition.Y + .5f),
                Vector3.Up,
                Vector3.Forward,
                1.0f, 1.0f
                );
            _overlay.FillVertices();

            _cursor.Height = gs.Height + 1.0f;
            _cursor.GridPosition.X = _gridPosition.X;
            _cursor.GridPosition.Y = _gridPosition.Y;
            _cursor.CalculateVertices();
        }

        public void Update(GameTime gameTime)
        {
            tick();

            if (_futureGridPosition != _gridPosition && Game1.Camera.IsAcceptingCommands) {
                _gridPosition = _futureGridPosition;
                updatePosition();
            }

            _overlay.Alpha = ((_maxAlpha - _minAlpha) * (float)(_timer / 37.5f)) + _minAlpha;

            _cursor.Offset.Y = _bobFactor * (float)((60.0f - _timer) / 37.5f);
            _cursor.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            _overlay.Draw(gameTime);
            _cursor.Draw(gameTime);
        }

        public void Unload()
        {
            UnderworldEngine.Scripting.BattleCursor.UpDispatch -= this.up;
            UnderworldEngine.Scripting.BattleCursor.DownDispatch -= this.down;
            UnderworldEngine.Scripting.BattleCursor.LeftDispatch -= this.left;
            UnderworldEngine.Scripting.BattleCursor.RightDispatch -= this.right;
        }
    }
}
