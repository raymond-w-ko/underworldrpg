using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UnderworldEngine.Scripting;

namespace UnderworldEngine.Graphics
{
    class Dialogue
    {
        public bool IsVisible = true;

        public static bool IsInitialized = false;

        // Font
        public static string FontName = "WascoSans18";
        public static uint FontVerticalSpacing = 5;

        public static SpriteFont Font;
        public static float FontHeight;
        public static Vector2 AverageCharSize;

        // Color Scheme
        public static Color TextColor = new Color(17, 11, 0);
        public static Color BackgroundColor = new Color(255, 240, 209);
        public static float BackgroundAlpha = 0.9f;
        public static Color BorderColor = new Color(54, 30, 26);
        private Color[] _borderColors;

        protected Vector2 _position;
        private float _xSize;
        private float _ySize;

        // graphics
        private SpriteBatch _spriteBatch;
        private BasicEffect _basicEffect;
        private VertexDeclaration _vertexDeclaration;
        private Vector3[] _vertices;
        private short[] _indices;

        private DialogueManager _dialogueManager;

        public Dialogue(DialogueManager dialogueManager,
            float xSize, float ySize, float xPosition, float yPosition)
        {
            _dialogueManager = dialogueManager;

            _position = new Vector2(xPosition, yPosition);
            _xSize = xSize;
            _ySize = ySize;

            initializeGraphics();

            calculateFancyBorder();
        }

        private void initializeGraphics()
        {
            _spriteBatch = new SpriteBatch(Game1.DefaultGraphicsDevice);

            _basicEffect = new BasicEffect(Game1.DefaultGraphicsDevice, null);
            Orthographic.SetOrthoEffect(_basicEffect);

            _vertexDeclaration = new VertexDeclaration(Game1.DefaultGraphics.GraphicsDevice,
                new VertexElement[] {
                    new VertexElement(0, 0, VertexElementFormat.Vector3,
                        VertexElementMethod.Default, VertexElementUsage.Position, 0)
                }
            );

            // create vertices for the background quad
            _vertices = new Vector3[]
            {
                new Vector3(0, 0, 0),
                new Vector3(_xSize, 0, 0),
                new Vector3(_xSize, _ySize, 0),
                new Vector3(0, _ySize, 0),
            };

            // create indices for the background quad
            _indices = new short[] { 0, 1, 2, 2, 3, 0 };

            // create an orthographic projection to draw the quad as a sprite
            _basicEffect.Projection = Matrix.CreateOrthographicOffCenter(0,
                Game1.DefaultGraphics.GraphicsDevice.Viewport.Width,
                Game1.DefaultGraphics.GraphicsDevice.Viewport.Height, 0,
                0, 1);

            _basicEffect.DiffuseColor = BackgroundColor.ToVector3();
            _basicEffect.Alpha = BackgroundAlpha;

            // move to correct position
            _basicEffect.World = Matrix.CreateTranslation(_position.X, _position.Y, 0);
        }

        public static void Initialize()
        {
            if (IsInitialized) {
                return;
            }

            ForceInitialize();
        }

        public static void ForceInitialize()
        {
            // initialize font
            Font = Game1.DefaultContent.Load<SpriteFont>(FontName);

            // measure the font height
            FontHeight = 0f;
            List<char> chars = ConsoleKeyMap.GetRegisteredCharacters();
            foreach (char c in chars) {
                Vector2 size = Font.MeasureString(c.ToString());

                if (size.Y > FontHeight)
                    FontHeight = size.Y;
            }

            calculateAverageCharLength();
            
            IsInitialized = true;
        }

        private static void calculateAverageCharLength()
        {
            string alphabet = "abcdefghijklmnopqrstuvwxyz" +
                              ",.?\"\"()";
            Vector2 measurement = Font.MeasureString(alphabet);

            measurement.X /= (float)alphabet.Length;

            AverageCharSize = measurement;
        }

        public void DrawLine()
        {
            if (!IsVisible) {
                return;
            }

            if (!IsInitialized) {
                throw new ApplicationException("Dialogue class was not initialized");
            }

             // set the vertex declaration
            Game1.DefaultGraphics.GraphicsDevice.VertexDeclaration = _vertexDeclaration;

            Orthographic.StartOrtho();

            _basicEffect.Begin();

            // draw the quad
            foreach (EffectPass pass in _basicEffect.CurrentTechnique.Passes) {
                pass.Begin();

                Game1.DefaultGraphics.GraphicsDevice.DrawUserIndexedPrimitives(
                    PrimitiveType.TriangleList,
                    _vertices, 0, 4, _indices, 0, 2);

                pass.End();
            }

            _basicEffect.End();

            _spriteBatch.Begin();

            drawFancyBorders();

            Orthographic.EndOrtho();

            // Draw lines of dialogue
            // Write strings to screen
            string[] lines = _dialogueManager.GetLinesToWrite();
            Vector2 textStart = new Vector2(_position.X + 5, _position.Y + 5);
            foreach (string s in lines) {
                _spriteBatch.DrawString(Font, s, textStart, TextColor);
                textStart.Y += Dialogue.FontHeight + Dialogue.FontVerticalSpacing;
            }

            _spriteBatch.End();

            _spriteBatch.ResetFor3d();
        }

        public string displayLine(string line, ref Vector2 textStart, ref Vector2 textEnd, float maxTextWidth)
        {
            // line of text to display
            String textToDisplay = line;
            return "";
        }

        private void drawBorders()
        {
            _spriteBatch.DrawLine(5, BorderColor,
                new Vector2(_position.X, _position.Y),
                new Vector2(_position.X + _xSize, _position.Y)
                );
            _spriteBatch.DrawLine(5, BorderColor,
                new Vector2(_position.X + _xSize, _position.Y),
                new Vector2(_position.X + _xSize, _position.Y + _ySize)
                );
            _spriteBatch.DrawLine(5, BorderColor,
                new Vector2(_position.X + _xSize, _position.Y + _ySize),
                new Vector2(_position.X, _position.Y + _ySize)
                );
            _spriteBatch.DrawLine(5, BorderColor,
                new Vector2(_position.X, _position.Y),
                new Vector2(_position.X, _position.Y + _ySize)
                );
        }

        private void calculateFancyBorder()
        {
            Vector3 startColor = BackgroundColor.ToVector3();
            Vector3 endColor = BorderColor.ToVector3();

            _borderColors = new Color[6];

            Vector3 diff = endColor - startColor;
            for (int ii = 1; ii <= 6; ii++) {
                float step = ((float)((float)ii / 6.0f));
                _borderColors[ii - 1] = new Color(startColor + (step * diff));
            }
        }

        private void drawFancyBorders()
        {
            Vector2 start;
            Vector2 end;
            for (int ii = 0; ii < 6; ii++) {
                // Top Border
                start.X = _position.X - ii;
                start.Y = _position.Y - ii;
                end.X = _position.X + _xSize + ii;
                end.Y = _position.Y - ii;
                _spriteBatch.DrawLine(1, _borderColors[ii], start, end);
                // Right Border
                start.X = _position.X + _xSize + ii;
                start.Y = _position.Y - ii;
                end.X = _position.X + _xSize + ii;
                end.Y = _position.Y + _ySize + ii;
                _spriteBatch.DrawLine(1, _borderColors[ii], start, end);
                // Bottom Border
                start.X = _position.X - ii - 1;
                start.Y = _position.Y + _ySize + ii;
                end.X = _position.X + _xSize + ii;
                end.Y = _position.Y + _ySize + ii;
                _spriteBatch.DrawLine(1, _borderColors[ii], start, end);
                // Left Border
                start.X = _position.X - ii;
                start.Y = _position.Y - ii;
                end.X = _position.X - ii;
                end.Y = _position.Y + _ySize + ii;
                _spriteBatch.DrawLine(1, _borderColors[ii], start, end);
            }
        }
    }
}
