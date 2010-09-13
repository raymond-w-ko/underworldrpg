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

using UnderworldEngine.GraphicsEngine;

namespace UnderworldEngine.Game
{
    public abstract class Quad : GameObject
    {
        protected Vector3 origin;
        protected Vector3 normal;
        protected Vector3 up;

        protected Vector3 left;

        protected Vector3 upperLeft;
        protected Vector3 upperRight;
        protected Vector3 lowerLeft;
        protected Vector3 lowerRight;

        protected BasicEffectManager effect;

        public Quad(Vector3 origin, Vector3 normal, Vector3 up, float width, float height)
        {
            this.Position = this.origin = origin;
            this.normal = normal;
            this.up = up;

            this.left = Vector3.Cross(normal, up);
            Vector3 uppercenter = (up * height / 2) + origin;

            this.upperLeft = uppercenter + (left * width / 2);
            this.upperRight = uppercenter - (left * width / 2);
            this.lowerLeft = upperLeft - (up * height);
            this.lowerRight = upperRight - (up * height);

            this.effect = new BasicEffectManager();
        }
    }
}
