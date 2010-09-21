using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UnderworldEngine.Graphics
{
    class FpsCounter
    {
        private int _numOfFrames = 0;
        private double _fps = 0;

        private SpriteBatch _spriteBatch;
        private SpriteFont _spriteFont;

        public bool IsVisible;

        public FpsCounter() : this(Game1.DefaultContent.Load<SpriteFont>("Consolas")) { }

        public FpsCounter(SpriteFont spriteFont)
        {
            _spriteFont = spriteFont;
            _spriteBatch = new SpriteBatch(Game1.DefaultGraphicsDevice);

            IsVisible = true;
        }

        public void Update(GameTime gameTime)
        {
            if (!IsVisible) {
                return;
            }

            // Calculate FPS
            if (gameTime.TotalGameTime.Milliseconds == 0) {
                _fps = _numOfFrames;
                _numOfFrames = 0;
            }
        }

        public void DrawAt(Vector2 position, Color color)
        {
            if (!IsVisible) {
                return;
            }

            _numOfFrames++;
            _spriteBatch.DrawString(_spriteFont, _fps.ToString(), position, color);
        }

        public void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            DrawAt(new Vector2(0, 0), Color.Yellow);
            _spriteBatch.End();
            _spriteBatch.ResetFor3d();
        }
    }
}
