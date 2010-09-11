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

using UnderworldEngine.Game.Interfaces;

namespace UnderworldEngine.Game
{
    /// <summary>
    /// Renderable object that exists within the game world
    /// </summary>
    public abstract class GameObject : IDraw, IManagedDraw
    {
        private class Transformation
        {
            public enum Operation
            {
                Scale = 1,
                Rotate = 2,
                Translate = 4,
            }

            public enum Axes
            {
                X,
                Y,
                Z
            }

            public int Operations;
            public Axes Axis = Axes.X;
            public float Degrees = 0;
            public Vector3 Offset = Vector3.Zero;

            public Transformation()
            {
                Operations = 0;
            }
        }

        public Vector3 position;
        public Vector3 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
                this.needTransformationCompile = true;
            }
        }
        public BoundingBox BoundingBox { get; set; }
        public bool IsVisible { get; set; }

        /// <summary>
        /// Matrix that is equivalent to the total sequences of transforms.
        /// </summary>
        protected Matrix worldMatrix;
        private List<Transformation> transformationList;
        private bool needTransformationCompile;

        public GameObject()
        {
            this.Position = Vector3.Zero;
            this.BoundingBox = new BoundingBox();
            this.IsVisible = true;
            this.worldMatrix = Matrix.Identity;

            this.transformationList = new List<Transformation>();
            this.needTransformationCompile = true;
        }

        public abstract void Draw();
        public abstract void ManagedDraw(Effect effect);

        public void Scale(float degrees)
        {
            Transformation trans = new Transformation();
            trans.Operations |= (int)Transformation.Operation.Scale;
            trans.Degrees = degrees;
            transformationList.Add(trans);
            this.needTransformationCompile = true;
        }

        public void ApplyRotationX(float degrees)
        {
            Transformation trans = new Transformation();
            trans.Operations |= (int)Transformation.Operation.Rotate;
            trans.Axis = Transformation.Axes.X;
            trans.Degrees = degrees;
            transformationList.Add(trans);
            this.needTransformationCompile = true;
        }

        public void ApplyRotationY(float degrees)
        {
            Transformation trans = new Transformation();
            trans.Operations |= (int)Transformation.Operation.Rotate;
            trans.Axis = Transformation.Axes.Y;
            trans.Degrees = degrees;
            transformationList.Add(trans);
            this.needTransformationCompile = true;
        }

        public void ApplyRotationZ(float degrees)
        {
            Transformation trans = new Transformation();
            trans.Operations |= (int)Transformation.Operation.Rotate;
            trans.Axis = Transformation.Axes.Z;
            trans.Degrees = degrees;
            transformationList.Add(trans);
            this.needTransformationCompile = true;
        }

        public void OffsetBy(float x, float y, float z)
        {
            Transformation trans = new Transformation();
            trans.Operations |= (int)Transformation.Operation.Translate;
            trans.Offset = new Vector3(x, y, z);
            transformationList.Add(trans);
            this.needTransformationCompile = true;
        }

        public void CompileTransformations()
        {
            if (!needTransformationCompile) {
                return;
            }

            worldMatrix = Matrix.Identity;

            foreach (Transformation trans in transformationList) {
                if ((trans.Operations & (int)Transformation.Operation.Scale) != 0) {
                    worldMatrix *= Matrix.CreateScale(trans.Degrees);
                }

                if ((trans.Operations & (int)Transformation.Operation.Rotate) != 0) {
                    if (trans.Axis == Transformation.Axes.X) {
                        worldMatrix *= Matrix.CreateRotationX(MathHelper.ToRadians(trans.Degrees));
                    }
                    else if (trans.Axis == Transformation.Axes.Y) {
                        worldMatrix *= Matrix.CreateRotationY(MathHelper.ToRadians(trans.Degrees));
                    }
                    else if (trans.Axis == Transformation.Axes.Z) {
                        worldMatrix *= Matrix.CreateRotationZ(MathHelper.ToRadians(trans.Degrees));
                    }
                }

                if ((trans.Operations & (int)Transformation.Operation.Translate) != 0) {
                    worldMatrix *= Matrix.CreateTranslation(trans.Offset);
                }
            }

            worldMatrix *= Matrix.CreateTranslation(this.Position);

            this.needTransformationCompile = false;
        }
    }
}
