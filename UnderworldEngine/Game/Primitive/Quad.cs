using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using UnderworldEngine.Graphics;

namespace UnderworldEngine.Game
{
    public class Quad : GameObject
    {
        public Vector3 UpperLeft;
        public Vector3 UpperRight;
        public Vector3 LowerLeft;
        public Vector3 LowerRight;

        public Quad(Vector3 origin, Vector3 normal, Vector3 up, float width, float height)
        {
            Vector3 left = Vector3.Cross(normal, up);
            Vector3 uppercenter = (up * height / 2) + origin;

            this.UpperLeft = uppercenter + (left * width / 2);
            this.UpperRight = uppercenter - (left * width / 2);
            this.LowerLeft = UpperLeft - (up * height);
            this.LowerRight = UpperRight - (up * height);
        }

        public override void Draw(GameTime gameTime)
        {
            throw new NotImplementedException("The Quad class just supplies vertex positions and can't be drawn.");
        }
    }
}
