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

        public bool IsVisible = true;

        protected Vector2 _position;
        private float _width;
        private float _height;

        // graphics
        private SpriteBatch _spriteBatch;
        private SpriteFont _font;
        private BasicEffect _basicEffect;
        private VertexDeclaration _vertexDeclaration;

        // display
        private string _fontName;
        private float _fontHeight;
        private float _verticalSpacing = 0.0f;
        private Color _defaultTextColor;

        private Color _backgroundColor;
        private float _backgroundAlpha;

        public Menu(string fontName, Color defaultTextColor,
            Color backgroundColor, float backgroundAlpha,
            float width, float height)
        {
            _position = new Vector2(0, 0);
            _width = width;
            _height = height;

            _fontName = fontName;
            _defaultTextColor = defaultTextColor;
            _backgroundColor = backgroundColor;
            _backgroundAlpha = backgroundAlpha;

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
            float height = (_fontHeight + _verticalSpacing);

            // create vertices for the background quad
            Vector3[] vertices = new Vector3[]
            {
                new Vector3(0, 0, 0),
                new Vector3(_width, 0, 0),
                new Vector3(_width, _height, 0),
                new Vector3(0, _height, 0),
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
            Game1.console.Log(_position.ToString());
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

            // restore previous alpha blend
            Game1.DefaultGraphics.GraphicsDevice.RenderState.AlphaBlendEnable = blend;
        }
    }
}
