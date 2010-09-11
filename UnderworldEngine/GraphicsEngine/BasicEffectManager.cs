using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace UnderworldEngine.GraphicsEngine
{
    public class BasicEffectManager
    {
        private BasicEffect basicEffect;
        public BasicEffect BasicEffect
        {
            get
            {
                return basicEffect;
            }
        }

        public BasicEffectManager()
        {
            this.basicEffect = new BasicEffect(Game1.DefaultGraphicsDevice, null);

            basicEffect.World = Matrix.Identity;
            basicEffect.View = Game1.Camera.ViewMatrix;
            basicEffect.Projection = Game1.Camera.ProjectionMatrix;

            // Possible optimization for low end computers?
            basicEffect.PreferPerPixelLighting = true;
        }

        public void EnableVertexColor()
        {
            this.basicEffect.VertexColorEnabled = true;
        }

        public void DisableVertexColor()
        {
            this.basicEffect.VertexColorEnabled = false;
        }

        public void EnableTexture()
        {
            this.basicEffect.TextureEnabled = true;
        }

        public void DisableTexture()
        {
            this.basicEffect.TextureEnabled = false;
        }

        public void SetTexture(Texture2D texture)
        {
            this.basicEffect.Texture = texture;
        }

        public void UpdatePresentationMatrices(Matrix world)
        {
            this.basicEffect.World = world;
            this.basicEffect.View = Game1.Camera.ViewMatrix;
            this.basicEffect.Projection = Game1.Camera.ProjectionMatrix;
        }
    }
}
