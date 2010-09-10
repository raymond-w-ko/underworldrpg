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

namespace UnderworldEngine.Game
{
    abstract class GameObject
    {
        public Vector3 Position { get; set; }
        public BoundingBox BoundingBox { get; set; }
        public bool IsActive { get; set; }
        public bool IsVisible { get; set; }

        public GameObject()
        {
            this.Position = Vector3.Zero;
            this.BoundingBox = new BoundingBox();
            this.IsActive = true;
            this.IsVisible = true;
        }

        public abstract void Draw();
    }
}
