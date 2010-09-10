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
    class BasicEffectManager
    {
        private Matrix worldMatrix;
        public Matrix WorldMatrix
        {
            get
            {
                return worldMatrix;
            }
        }

        private BasicEffect effect;
        public BasicEffect Effect
        {
            get
            {
                return effect;
            }
        }

        public BasicEffectManager(GraphicsDevice gd)
        {
            this.effect = new BasicEffect(gd, null);

            this.worldMatrix = Matrix.CreateTranslation(0, 0, 0);

            effect.World = this.worldMatrix;
            effect.View = Game1.camera.ViewMatrix;
            effect.Projection = Game1.camera.ProjectionMatrix;

            this.EnableVertexColor();
        }

        public void EnableVertexColor()
        {
            this.effect.VertexColorEnabled = true;
        }

        public void DisableVertexColor()
        {
            this.effect.VertexColorEnabled = false;
        }

        public void Update()
        {
            this.effect.World = this.worldMatrix;
            this.effect.View = Game1.camera.ViewMatrix;
            this.effect.Projection = Game1.camera.ProjectionMatrix;
        }
    }
}
