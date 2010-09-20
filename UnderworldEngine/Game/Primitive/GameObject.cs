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

using UnderworldEngine.Graphics.Interfaces;
using System.Xml;

namespace UnderworldEngine.Game
{
    /// <summary>
    /// Renderable object that exists within the game world
    /// </summary>
    public abstract class GameObject : IDraw
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
            public float Value = 0;
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
            this.IsVisible = true;

            this.worldMatrix = Matrix.Identity;
            this.transformationList = new List<Transformation>();
            this.needTransformationCompile = true;
        }

        public abstract void Draw();

        public void Scale(float degrees)
        {
            Transformation trans = new Transformation();
            trans.Operations |= (int)Transformation.Operation.Scale;
            trans.Value = degrees;
            transformationList.Add(trans);
            this.needTransformationCompile = true;
        }

        public void ApplyRotationX(float degrees)
        {
            Transformation trans = new Transformation();
            trans.Operations |= (int)Transformation.Operation.Rotate;
            trans.Axis = Transformation.Axes.X;
            trans.Value = degrees;
            transformationList.Add(trans);
            this.needTransformationCompile = true;
        }

        public void ApplyRotationY(float degrees)
        {
            Transformation trans = new Transformation();
            trans.Operations |= (int)Transformation.Operation.Rotate;
            trans.Axis = Transformation.Axes.Y;
            trans.Value = degrees;
            transformationList.Add(trans);
            this.needTransformationCompile = true;
        }

        public void ApplyRotationZ(float degrees)
        {
            Transformation trans = new Transformation();
            trans.Operations |= (int)Transformation.Operation.Rotate;
            trans.Axis = Transformation.Axes.Z;
            trans.Value = degrees;
            transformationList.Add(trans);
            this.needTransformationCompile = true;
        }

        public void TranslateBy(float x, float y, float z)
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
                    worldMatrix *= Matrix.CreateScale(trans.Value);
                }

                if ((trans.Operations & (int)Transformation.Operation.Rotate) != 0) {
                    if (trans.Axis == Transformation.Axes.X) {
                        worldMatrix *= Matrix.CreateRotationX(MathHelper.ToRadians(trans.Value));
                    }
                    else if (trans.Axis == Transformation.Axes.Y) {
                        worldMatrix *= Matrix.CreateRotationY(MathHelper.ToRadians(trans.Value));
                    }
                    else if (trans.Axis == Transformation.Axes.Z) {
                        worldMatrix *= Matrix.CreateRotationZ(MathHelper.ToRadians(trans.Value));
                    }
                }

                if ((trans.Operations & (int)Transformation.Operation.Translate) != 0) {
                    worldMatrix *= Matrix.CreateTranslation(trans.Offset);
                }
            }

            this.needTransformationCompile = false;
        }

        /// <summary>
        /// Writes out stored transformations
        /// </summary>
        /// <param name="xmlDocument"></param>
        /// <param name="rootNode"></param>
        public virtual void Save(XmlDocument xmlDocument, XmlNode rootNode)
        {
            XmlNode transformationsNode = xmlDocument.CreateElement("Transformations");

            foreach (Transformation trans in transformationList) {
                XmlNode operNode = null;
                if ((Transformation.Operation)trans.Operations == Transformation.Operation.Scale) {
                    operNode = xmlDocument.CreateElement("Scale");

                    XmlNode valueNode = xmlDocument.CreateElement("Value");
                    valueNode.AppendChild(xmlDocument.CreateTextNode(trans.Value.ToString()));

                    operNode.AppendChild(valueNode);
                }
                else if ((Transformation.Operation)trans.Operations == Transformation.Operation.Rotate) {
                    operNode = xmlDocument.CreateElement("Rotate");

                    XmlNode valueNode = xmlDocument.CreateElement("Value");
                    valueNode.AppendChild(xmlDocument.CreateTextNode(trans.Value.ToString()));

                    XmlNode axisNode = xmlDocument.CreateElement("Axis");
                    string axisString = "";
                    if (trans.Axis == Transformation.Axes.X) {
                        axisString = "X";
                    }
                    else if (trans.Axis == Transformation.Axes.Y) {
                        axisString = "Y";
                    }
                    else if (trans.Axis == Transformation.Axes.Z) {
                        axisString = "Z";
                    }
                    axisNode.AppendChild(xmlDocument.CreateTextNode(axisString));

                    operNode.AppendChild(valueNode);
                    operNode.AppendChild(axisNode);
                }
                else if ((Transformation.Operation)trans.Operations == Transformation.Operation.Translate) {
                    operNode = xmlDocument.CreateElement("Translate");

                    XmlNode offsetNode = xmlDocument.CreateElement("Offset");
                    offsetNode.AppendChild(xmlDocument.CreateTextNode(
                        trans.Offset.X.ToString() + " " +
                        trans.Offset.Y.ToString() + " " +
                        trans.Offset.Z.ToString()
                        ));

                    operNode.AppendChild(offsetNode);
                }

                transformationsNode.AppendChild(operNode);
            }

            rootNode.AppendChild(transformationsNode);
        }

        public void LoadTransformations(XmlDocument xmlDocument, XmlNode rootNode)
        {
            foreach (XmlNode trans in rootNode.ChildNodes) {
                Transformation newTrans = new Transformation();

                if (trans.Name == "Scale") {
                    float value = (float) Convert.ToDouble(trans["Value"].InnerText);
                    newTrans.Value = value;
                    newTrans.Operations = (int)Transformation.Operation.Scale;
                }
                else if (trans.Name == "Rotate") {
                    float value = (float) Convert.ToDouble(trans["Value"].InnerText);
                    string axisString = trans["Axis"].InnerText;
                    newTrans.Value = value;
                    if (axisString == "X") {
                        newTrans.Axis = Transformation.Axes.X;
                    }
                    else if (axisString == "Y") {
                        newTrans.Axis = Transformation.Axes.Y;
                    }
                    else if (axisString == "Z") {
                        newTrans.Axis = Transformation.Axes.Z;
                    }
                    newTrans.Operations = (int)Transformation.Operation.Rotate;
                }
                else if (trans.Name == "Translate") {
                    Vector3 offset = new Vector3().GetFromString(trans["Offset"].InnerText);

                    newTrans.Offset = offset;
                    newTrans.Operations = (int)Transformation.Operation.Translate;
                }

                transformationList.Add(newTrans);
            }

            this.needTransformationCompile = true;
        }
    }
}
