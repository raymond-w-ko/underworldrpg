using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace UnderworldEngine.Graphics
{
    class Orthographic
    {
        public static void SetOrthoEffect(BasicEffect ef)
        {
            ef.VertexColorEnabled = false;
            ef.LightingEnabled = false;
            ef.TextureEnabled = false;
            ef.View = Matrix.Identity;
            ef.World = Matrix.Identity;

            ef.Projection = Matrix.CreateOrthographicOffCenter(0,
                Game1.DefaultGraphics.GraphicsDevice.Viewport.Width,
                Game1.DefaultGraphics.GraphicsDevice.Viewport.Height, 0,
                0, 1);

            ef.DiffuseColor = (new Color(10,10,10)).ToVector3();
            ef.Alpha = 0.8f;

            ef.World = Matrix.CreateTranslation(10, 10, 0);
        }

        static bool blend;
        public static void StartOrtho()
        {
            // save current blending mode
            blend = Game1.DefaultGraphics.GraphicsDevice.RenderState.AlphaBlendEnable;
            // enable alpha blending
            Game1.DefaultGraphics.GraphicsDevice.RenderState.AlphaBlendEnable = true;
            Game1.DefaultGraphics.GraphicsDevice.RenderState.AlphaBlendOperation = BlendFunction.Add;
            Game1.DefaultGraphics.GraphicsDevice.RenderState.SourceBlend = Blend.SourceAlpha;
            Game1.DefaultGraphics.GraphicsDevice.RenderState.DestinationBlend = Blend.InverseSourceAlpha;
            Game1.DefaultGraphics.GraphicsDevice.RenderState.SeparateAlphaBlendEnabled = false;
        }

        public static void EndOrtho()
        {
            // restore previous alpha blend
            Game1.DefaultGraphics.GraphicsDevice.RenderState.AlphaBlendEnable = blend;
        }
    }
}
