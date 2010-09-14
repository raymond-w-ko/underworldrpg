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

        private SpriteFont _spriteFont;

        public bool IsVisible;

        public FpsCounter()
        {
            _spriteFont = Game1.DefaultContent.Load<SpriteFont>("Consolas");
            IsVisible = true;
        }

        public FpsCounter(SpriteFont spriteFont)
        {
            _spriteFont = spriteFont;
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

        public void Draw(Vector2 position, Color color)
        {
            if (!IsVisible) {
                return;
            }

            _numOfFrames++;
            Game1.DefaultSpriteBatch.DrawString(_spriteFont, _fps.ToString(), position, color);
        }

        public void Draw()
        {
            Draw(new Vector2(0, 0), Color.Yellow);
        }
    }
}
