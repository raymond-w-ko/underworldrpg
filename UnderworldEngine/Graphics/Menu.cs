using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using UnderworldEngine.Scripting;

namespace UnderworldEngine.Graphics
{
    class Menu
    {
        public enum MenuEntryType
        {
            Label,
            Editable,
            Separator
        }

        public struct MenuEntry
        {
            MenuEntryType menuEntryType;
            string text;
            public string Text
            {
                get
                {
                    return text;
                }
            }

            public MenuEntry(MenuEntryType met, string s)
            {
                menuEntryType = met;
                text = s;
            }
        }

        public bool IsVisible = true;

        protected Vector2 _position;
        private float _xSize;
        private float _ySize;

        // graphics
        private SpriteBatch _spriteBatch;
        private SpriteFont _font;
        private BasicEffect _basicEffect;
        private VertexDeclaration _vertexDeclaration;

        // display
        private string _fontName;
        private float _fontHeight;
        private float _verticalSpacing = 5.0f;
        private Color _defaultTextColor;

        private Color _backgroundColor;
        private float _backgroundAlpha;
        private Color _borderColor;
        
        // content
        public List<MenuEntry> MenuEntries;
        private Vector2 _averageCharSize;
        private int _menuEntryToDisplay;
        private string _menuRemainingText;

        public Menu(string fontName, Color defaultTextColor,
            Color backgroundColor, float backgroundAlpha,
            Color borderColor,
            float xSize, float ySize,
            float xPosition, float yPosition)
        {
            _position = new Vector2(xPosition, yPosition);
            _xSize = xSize;
            _ySize = ySize;

            _fontName = fontName;
            _defaultTextColor = defaultTextColor;
            _backgroundColor = backgroundColor;
            _backgroundAlpha = backgroundAlpha;
            _borderColor = borderColor;

            _spriteBatch = new SpriteBatch(Game1.DefaultGraphicsDevice);
            _font = Game1.DefaultContent.Load<SpriteFont>(_fontName);

            _basicEffect = new BasicEffect(Game1.DefaultGraphicsDevice, null);
            _basicEffect.VertexColorEnabled = false;
            _basicEffect.LightingEnabled = false;
            _basicEffect.TextureEnabled = false;
            _basicEffect.View = Matrix.Identity;
            _basicEffect.World = Matrix.Identity;

            // measure the font height
            _fontHeight = 0f;

            List<char> chars = ConsoleKeyMap.GetRegisteredCharacters();
            foreach (char c in chars) {
                Vector2 size = _font.MeasureString(c.ToString());

                if (size.Y > _fontHeight)
                    _fontHeight = size.Y;
            }

            _vertexDeclaration = new VertexDeclaration(Game1.DefaultGraphics.GraphicsDevice,
                    new VertexElement[] {
                        new VertexElement(0, 0, VertexElementFormat.Vector3,
                            VertexElementMethod.Default, VertexElementUsage.Position, 0)
                    }
                );

            MenuEntries = new List<MenuEntry>();
            _averageCharSize = calculateAverageCharLength();
            _menuEntryToDisplay = 0;
            _menuRemainingText = "";
        }

        private Vector2 calculateAverageCharLength()
        {
            string alphabet = "abcdefghijklmnopqrstuvwxyz" +
                              ",.?\"\"()";
            Vector2 measurement = _font.MeasureString(alphabet);

            measurement.X /= (float)alphabet.Length;

            return measurement;
        }

        public void MoveTo(Vector2 pos)
        {
            _position = pos;
        }

        public virtual void Draw()
        {
            if (!IsVisible) {
                return;
            }

            // measure the height of a line in the current font
            float fontHeight = (_fontHeight + _verticalSpacing);

            // create vertices for the background quad
            Vector3[] vertices = new Vector3[]
            {
                new Vector3(0, 0, 0),
                new Vector3(_xSize, 0, 0),
                new Vector3(_xSize, _ySize, 0),
                new Vector3(0, _ySize, 0),
            };

            // create indices for the background quad
            short[] indices = new short[] { 0, 1, 2, 2, 3, 0 };

            // create an orthographic projection to draw the quad as a sprite
            _basicEffect.Projection = Matrix.CreateOrthographicOffCenter(0,
                Game1.DefaultGraphics.GraphicsDevice.Viewport.Width,
                Game1.DefaultGraphics.GraphicsDevice.Viewport.Height, 0,
                0, 1);

            _basicEffect.DiffuseColor = _backgroundColor.ToVector3();
            _basicEffect.Alpha = _backgroundAlpha;

            // set the vertex declaration
            Game1.DefaultGraphics.GraphicsDevice.VertexDeclaration = _vertexDeclaration;

            // save current blending mode
            bool blend = Game1.DefaultGraphics.GraphicsDevice.RenderState.AlphaBlendEnable;

            // enable alpha blending
            Game1.DefaultGraphics.GraphicsDevice.RenderState.AlphaBlendEnable = true;
            Game1.DefaultGraphics.GraphicsDevice.RenderState.AlphaBlendOperation = BlendFunction.Add;
            Game1.DefaultGraphics.GraphicsDevice.RenderState.SourceBlend = Blend.SourceAlpha;
            Game1.DefaultGraphics.GraphicsDevice.RenderState.DestinationBlend = Blend.InverseSourceAlpha;
            Game1.DefaultGraphics.GraphicsDevice.RenderState.SeparateAlphaBlendEnabled = false;

            // move to correct position
            _basicEffect.World = Matrix.CreateTranslation(_position.X, _position.Y, 0);

            _basicEffect.Begin();

            // draw the quad
            foreach (EffectPass pass in _basicEffect.CurrentTechnique.Passes) {
                pass.Begin();

                Game1.DefaultGraphics.GraphicsDevice.DrawUserIndexedPrimitives(
                    PrimitiveType.TriangleList,
                    vertices, 0, 4, indices, 0, 2);

                pass.End();
            }

            _basicEffect.End();

            _spriteBatch.Begin();
            drawFancyBorders();
            
            // restore previous alpha blend
            Game1.DefaultGraphics.GraphicsDevice.RenderState.AlphaBlendEnable = blend;

            // Draw Menu Entries
            Vector2 textStart = new Vector2(_position.X + 5, _position.Y + 5);
            Vector2 textEnd = new Vector2(_position.X + _xSize - 5, _position.Y + _ySize - 5);
            float maxTextWidth = _xSize - 5;
            for (int ii = _menuEntryToDisplay; ii < MenuEntries.Count(); ii++) {
                bool result = displayEntry((MenuEntries[ii]), ref textStart, ref textEnd, maxTextWidth);
                if (result) {
                    continue;
                }
                else {
                    break;
                }
            }
            _spriteBatch.End();
            _spriteBatch.ResetFor3d();
        }

        public bool displayEntry(MenuEntry entry, ref Vector2 textStart, ref Vector2 textEnd, float maxTextWidth)
        {
            // line of text to display
            String textToDisplay;
            if (_menuRemainingText.Length > 0) {
                textToDisplay = _menuRemainingText;
            }
            else {
                textToDisplay = entry.Text;
            }

            while (textToDisplay.Length > 0) {
                Vector2 measurement = _font.MeasureString(textToDisplay);

                // Find estimated number of characters can fit width;
                int estimatedChars = (int)(maxTextWidth / _averageCharSize.X);
                int numChars = estimatedChars > textToDisplay.Length ?
                    textToDisplay.Length : estimatedChars;
                String candidate = new String(textToDisplay.ToCharArray(), 0, numChars);
                // Increment until string fits
                while ((_font.MeasureString(candidate).X < maxTextWidth) &&
                       (textToDisplay.Length > candidate.Length)
                      ) {
                    candidate = new String(textToDisplay.ToCharArray(), 0, candidate.Length + 1);
                }
                // Decrement until string fits
                while (_font.MeasureString(candidate).X > maxTextWidth) {
                    candidate = new String(candidate.ToCharArray(), 0, candidate.Length - 1);
                }

                // Adaptive averaging
                _averageCharSize =  (_font.MeasureString(candidate));
                _averageCharSize.X *= (1.0f / (float)candidate.Length);

                // Write string to screen
                _spriteBatch.DrawString(_font, candidate, textStart, _defaultTextColor);

                // Cut out written string
                numChars = textToDisplay.Length - candidate.Length > 0 ?
                    textToDisplay.Length - candidate.Length : textToDisplay.Length;
                if (numChars != textToDisplay.Length) {
                    textToDisplay =
                        new String(textToDisplay.ToCharArray(),
                        candidate.Length, numChars
                        );
                }
                else {
                    textToDisplay = "";
                }

                // move "cursor" to next line
                textStart.Y += _fontHeight + _verticalSpacing;

                // check to see if there is even a next line
                if (textStart.Y >= textEnd.Y) {
                    _menuRemainingText = textToDisplay;
                    return false;
                }
            }
            return true;
        }

        public void AddEntry(MenuEntryType menuEntryType, string s)
        {
            MenuEntry me = new MenuEntry(menuEntryType, s);
            MenuEntries.Add(me);
        }

        public void Advance()
        {
            ;
        }

        private void drawBorders()
        {
            _spriteBatch.DrawLine(5, _borderColor,
                new Vector2(_position.X, _position.Y),
                new Vector2(_position.X + _xSize, _position.Y)
                );
            _spriteBatch.DrawLine(5, _borderColor,
                new Vector2(_position.X + _xSize, _position.Y),
                new Vector2(_position.X + _xSize, _position.Y + _ySize)
                );
            _spriteBatch.DrawLine(5, _borderColor,
                new Vector2(_position.X + _xSize, _position.Y + _ySize),
                new Vector2(_position.X, _position.Y + _ySize)
                );
            _spriteBatch.DrawLine(5, _borderColor,
                new Vector2(_position.X, _position.Y),
                new Vector2(_position.X, _position.Y + _ySize)
                );
        }

        private void drawFancyBorders()
        {
            Vector3 startColor = _backgroundColor.ToVector3();
            Vector3 endColor = _borderColor.ToVector3();

            Color[] colors = new Color[6];

            Vector3 diff = endColor - startColor;
            for (int ii = 1; ii <= 6; ii++) {
                float step = ((float)((float)ii/6.0f));
                colors[ii-1] = new Color(startColor + (step * diff));
            }

            for (int ii = 0; ii < 6; ii++) {
                // Top Border
                _spriteBatch.DrawLine(1, colors[ii],
                    new Vector2(_position.X - ii, _position.Y - ii),
                    new Vector2(_position.X + _xSize + ii, _position.Y - ii)
                    );
                // Right Border
                _spriteBatch.DrawLine(1, colors[ii],
                    new Vector2(_position.X + _xSize + ii, _position.Y - ii),
                    new Vector2(_position.X + _xSize + ii, _position.Y + _ySize + ii)
                    );
                // Bottom Border
                _spriteBatch.DrawLine(1, colors[ii],
                    new Vector2(_position.X - ii - 1, _position.Y + _ySize + ii),
                    new Vector2(_position.X + _xSize + ii, _position.Y + _ySize + ii)
                    );
                // Left Border
                _spriteBatch.DrawLine(1, colors[ii],
                    new Vector2(_position.X - ii, _position.Y - ii),
                    new Vector2(_position.X - ii, _position.Y + _ySize + ii)
                    );
            }
        }
    }

    public static class SpriteExtensions
    {
        public static void DrawLine(this SpriteBatch sb, //Texture2D blank,
              float width, Color color, Vector2 point1, Vector2 point2)
        {
            Texture2D pointTexture = new Texture2D(Game1.DefaultGraphicsDevice, 1, 1, 1, TextureUsage.None, SurfaceFormat.Color);
            pointTexture.SetData(new[] { Color.White });

            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);

            sb.Draw(pointTexture, point1, null, color,
              angle, new Vector2(0, 0), new Vector2(length, width),
              SpriteEffects.None, 0);
        }
    }
}
