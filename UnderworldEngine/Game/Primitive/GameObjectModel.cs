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
using UnderworldEngine.GameState;
using System.Xml;

namespace UnderworldEngine.Game
{
    public class GameObjectModel : GameObject
    {
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
        }
        private Model _model;

        private BoundingBox _boundingBox;
        public BoundingBox BoundingBox
        {
            get
            {
                return _boundingBox;
            }
        }

        public GameObjectModel(string name)
            : base()
        {
            this._name = name;
            this._model = Game1.DefaultContent.Load<Model>(_name);
            CalculateBoundingBox();
        }

        public void CalculateBoundingBox()
        {
            _boundingBox = GetBoundingBoxFromModel(_model);
        }
        
        public override void Draw(GameTime gameTime)
        {
            if (!this.Visible) {
                return;
            }

            // Process transforms
            Matrix[] parentTransforms = new Matrix[this._model.Bones.Count];
            this._model.CopyAbsoluteBoneTransformsTo(parentTransforms);
            // Compiles all queued transforms into the worldMatrix member;
            this.CompileTransformations();

            foreach (ModelMesh mesh in this._model.Meshes) {
                foreach (BasicEffect effect in mesh.Effects) {
                    effect.EnableDefaultLighting();

                    // Smoother lighting enabled default, maybe become a performance option in the future?
                    effect.PreferPerPixelLighting = true;

                    effect.World = parentTransforms[mesh.ParentBone.Index] * this._worldMatrix;

                    effect.View = Game1.Camera.ViewMatrix;
                    effect.Projection = Game1.Camera.ProjectionMatrix;
                }

                mesh.Draw();
            }
        }

        public BoundingBox GetBoundingBoxFromModel(Model model)
        {
            BoundingBox boundingBox = new BoundingBox();
            foreach (ModelMesh mesh in model.Meshes) {
                VertexPositionNormalTexture[] vertices =
                    new VertexPositionNormalTexture[
                        mesh.VertexBuffer.SizeInBytes / VertexPositionNormalTexture.SizeInBytes
                        ];

                mesh.VertexBuffer.GetData<VertexPositionNormalTexture>(vertices);

                Vector3[] vertexs = new Vector3[vertices.Length];

                for (int index = 0; index < vertexs.Length; index++) {
                    vertexs[index] = vertices[index].Position;
                    Vector3.Transform(vertexs[index], _worldMatrix);
                }

                boundingBox = BoundingBox.CreateMerged(boundingBox,  BoundingBox.CreateFromPoints(vertexs));
            }

            return boundingBox;
        }

        public override void Save(XmlDocument xmlDocument, XmlNode rootNode)
        {
            XmlNode modelNode = xmlDocument.CreateElement("Model");

            XmlAttribute modelName = xmlDocument.CreateAttribute("name");
            modelName.Value = this._name;
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
}
