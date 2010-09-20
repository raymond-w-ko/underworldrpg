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
using UnderworldEngine.GameState;
using System.Xml;

namespace UnderworldEngine.Game
{
    public class GameObjectModel : GameObject, IScreen
    {
        private ContentManager content;
        private string modelName;
        private Model model;
        public BoundingBox BoundingBox;
        private bool isFocused;
        public bool IsFocused
        {
            get { return isFocused; }
            set { isFocused = value; }
        }

        public GameObjectModel(string name)
            : base()
        {
            this.content = Game1.DefaultContent;
            this.modelName = name;
            this.model = content.Load<Model>(modelName);
            CalculateBoundingBox();
        }

        public GameObjectModel(ContentManager c, string name)
            : base()
        {
            this.content = c;
            this.modelName = name;
            this.model = content.Load<Model>(modelName);
            CalculateBoundingBox();
        }

        public void CalculateBoundingBox()
        {
            BoundingBox = GetBoundingBoxFromModel(model);
        }

        public void Unload()
        {
            //nothing to unload
        }

        public void Update(GameTime gameTime)
        {
            //nothing to update
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

        public BoundingBox GetBoundingBoxFromModel(Model model)
        {
            //Game1.Debug.WriteLine("Creating Bounding Box...");
            BoundingBox boundingBox = new BoundingBox();
            foreach (ModelMesh mesh in model.Meshes) {
                VertexPositionNormalTexture[] vertices =
                    new VertexPositionNormalTexture[mesh.VertexBuffer.SizeInBytes / VertexPositionNormalTexture.SizeInBytes];

                mesh.VertexBuffer.GetData<VertexPositionNormalTexture>(vertices);

                Vector3[] vertexs = new Vector3[vertices.Length];

                for (int index = 0; index < vertexs.Length; index++) {
                    vertexs[index] = vertices[index].Position;
                }

                boundingBox = BoundingBox.CreateMerged(boundingBox,  BoundingBox.CreateFromPoints(vertexs));
            }

            //Game1.Debug.WriteLine(boundingBox.Min.ToString());
            //Game1.Debug.WriteLine(boundingBox.Max.ToString());

            return boundingBox;
        }

        public override void Save(XmlDocument xmlDocument, XmlNode rootNode)
        {
            XmlNode modelNode = xmlDocument.CreateElement("Model");

            XmlAttribute modelName = xmlDocument.CreateAttribute("name");
            modelName.Value = this.modelName;
            modelNode.Attributes.Append(modelName);

            base.Save(xmlDocument, modelNode);

            rootNode.AppendChild(modelNode);
        }

        public static GameObjectModel Load(XmlDocument xmlDocument, XmlNode rootNode)
        {
            string modelName = rootNode.Attributes["name"].Value;

            GameObjectModel gom = new GameObjectModel(modelName);

            gom.LoadTransformations(xmlDocument, rootNode["Transformations"]);
            gom.CompileTransformations();

            return gom;
        }
    }

    public static class MyExtensions
    {
        //public class MapNotSquareException : ApplicationException { }
        public static float FindScaleToUnitFactor(this BoundingBox bb)
        {
            float length = bb.Max.X - bb.Min.X;
            float length2 = bb.Max.X - bb.Min.X;

            /*
            if (length != length2) {
                throw new MapNotSquareException();
            }
            */

            if (length2 > length) {
                length = length2;
            }

            return 1.0f / length;
        }
    }
}