﻿using System;
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
    class GameObjectModel : GameObject
    {
        private ContentManager content;
        private string modelName;
        private Model model;

        public GameObjectModel(string name)
            : base()
        {
            this.content = Game1.DefaultContent;
            this.modelName = name;
            this.model = content.Load<Model>(modelName);
        }


        public GameObjectModel(ContentManager c, string name)
            : base()
        {
            this.content = c;
            this.modelName = name;
            this.model = content.Load<Model>(modelName);
        }

        public override void Draw()
        {
            if (!this.IsVisible) {
                return;
            }

            // Process transforms
            Matrix[] parentTransforms = new Matrix[this.model.Bones.Count];
            this.model.CopyAbsoluteBoneTransformsTo(parentTransforms);
            // Compiles all queued transforms into the worldMatrix member;
            this.CompileTransformations();

            foreach (ModelMesh mesh in this.model.Meshes) {
                foreach (BasicEffect effect in mesh.Effects) {
                    effect.EnableDefaultLighting();
                    // Smoother lighting enabled default, maybe become a performance option in the future?
                    effect.PreferPerPixelLighting = true;

                    effect.World = parentTransforms[mesh.ParentBone.Index] * this.worldMatrix;

                    effect.View = Game1.Camera.ViewMatrix;
                    effect.Projection = Game1.Camera.ProjectionMatrix;
                }

                mesh.Draw();
            }
        }
    }
}