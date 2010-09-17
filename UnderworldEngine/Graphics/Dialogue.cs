﻿using System;
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

        protected Vector2 _position;
        private float _xSize;
        private float _ySize;

        // graphics
        private SpriteBatch _spriteBatch;
        private BasicEffect _basicEffect;
        private VertexDeclaration _vertexDeclaration;

        private DialogueManager _dialogueManager;

        public Dialogue(DialogueManager dialogueManager,
            float xSize, float ySize, float xPosition, float yPosition)
        {
            _dialogueManager = dialogueManager;

            _position = new Vector2(xPosition, yPosition);
            _xSize = xSize;
            _ySize = ySize;

            _spriteBatch = new SpriteBatch(Game1.DefaultGraphicsDevice);

            _basicEffect = new BasicEffect(Game1.DefaultGraphicsDevice, null);
            _basicEffect.VertexColorEnabled = false;
            _basicEffect.LightingEnabled = false;
            _basicEffect.TextureEnabled = false;
            _basicEffect.View = Matrix.Identity;
            _basicEffect.World = Matrix.Identity;

            _vertexDeclaration = new VertexDeclaration(Game1.DefaultGraphics.GraphicsDevice,
                new VertexElement[] {
                    new VertexElement(0, 0, VertexElementFormat.Vector3,
                        VertexElementMethod.Default, VertexElementUsage.Position, 0)
                }
            );
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

            _basicEffect.DiffuseColor = BackgroundColor.ToVector3();
            _basicEffect.Alpha = BackgroundAlpha;

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

        private void drawFancyBorders()
        {
            Vector3 startColor = BackgroundColor.ToVector3();
            Vector3 endColor = BorderColor.ToVector3();

            Color[] colors = new Color[6];

            Vector3 diff = endColor - startColor;
            for (int ii = 1; ii <= 6; ii++) {
                float step = ((float)((float)ii / 6.0f));
                colors[ii - 1] = new Color(startColor + (step * diff));
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