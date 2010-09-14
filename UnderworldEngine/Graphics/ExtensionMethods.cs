using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace UnderworldEngine.Graphics
{
    public static class ExtensionMethods
    {
        public static void ResetFor3d(this SpriteBatch spriteBatch)
        {
            Game1.DefaultGraphicsDevice.RenderState.DepthBufferEnable = true;
            Game1.DefaultGraphicsDevice.RenderState.AlphaBlendEnable = false;
            Game1.DefaultGraphicsDevice.RenderState.AlphaTestEnable = false;

            Game1.DefaultGraphicsDevice.SamplerStates[0].AddressU = TextureAddressMode.Wrap;
            Game1.DefaultGraphicsDevice.SamplerStates[0].AddressV = TextureAddressMode.Wrap;
        }
    }
}
