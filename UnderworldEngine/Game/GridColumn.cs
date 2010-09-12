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
    public class GridColumn : GameObject
    {
        public const int FLOOR_HEIGHT = -5;
        // Top 1x1 Square
        public QuadTexture Top;

        // Sides
        public QuadTexture Left;
        public QuadTexture Right;
        public QuadTexture Front;
        public QuadTexture Back;

        BasicEffectManager bem;

        /// <summary>
        /// origin is defined as center of top Quad
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="height"></param>
        public GridColumn(Vector3 origin, int height) :
            base()
        {
            this.position = origin;

            bem = new BasicEffectManager();

            Top = new QuadTexture(origin, Vector3.Up, Vector3.Backward, 1, 1, "Textures/ground");
            Top.ScaleUvMap(0.5f);

            float yValue = height - GridColumn.FLOOR_HEIGHT;

            Front = new QuadTexture(new Vector3(origin.X, GridColumn.FLOOR_HEIGHT + (yValue / 2), origin.Z - 0.5f),
                Vector3.Forward, Vector3.Up,
                1.0f, yValue,
                "Textures/dirt_01"
                );

            Back = new QuadTexture(new Vector3(origin.X, GridColumn.FLOOR_HEIGHT + (yValue / 2), origin.Z + 0.5f),
                Vector3.Backward, Vector3.Up,
                1.0f, yValue,
                "Textures/dirt_01"
                );

            Left = new QuadTexture(new Vector3(origin.X - 0.5f, GridColumn.FLOOR_HEIGHT + (yValue / 2), origin.Z),
                Vector3.Left, Vector3.Up,
                1.0f, yValue,
                "Textures/dirt_01"
                );

            Right = new QuadTexture(new Vector3(origin.X + 0.5f, GridColumn.FLOOR_HEIGHT + (yValue / 2), origin.Z),
                Vector3.Right, Vector3.Up,
                1.0f, yValue,
                "Textures/dirt_01"
                );
        }

        public override void Draw()
        {
            Top.Draw();

            Front.Draw();
            Back.Draw();
            Left.Draw();
            Right.Draw();
        }

        public override void ManagedDraw(Effect bem)
        {
            throw new NotImplementedException();
        }
    }
}
