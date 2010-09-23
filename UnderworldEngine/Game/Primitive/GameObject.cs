using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml;

namespace UnderworldEngine.Game
{
    /// <summary>
    /// Renderable object that exists within the game world
    /// </summary>
    public abstract class GameObject
    {
        #region Transformation Class
        /// <summary>
        /// A simple class that defines all possible transformations
        /// that can be done to a 3D object. This is queued into a list
        /// and then compiled into a World Matrix.
        /// </summary>
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
        #endregion

        /// <summary>
        /// The object's location in 3D space
        /// </summary>
        public Vector3 _position;
        public Vector3 Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
                _needTransformationCompile = true;
            }
        }

        public bool _autoPosition;
        public bool AutoPosition
        {
            get
            {
                return _autoPosition;
            }
            set
            {
                _autoPosition = value;
                _needTransformationCompile = true;
            }
        }

        /// <summary>
        /// Whether the object is visible / should be drawn
        /// </summary>
        public bool Visible;

        /// <summary>
        /// Matrix that is equivalent to the total sequences of transforms.
        /// </summary>
        protected Matrix _worldMatrix;
        private LinkedList<Transformation> _transformationList;
        private bool _needTransformationCompile;

        public GameObject()
        {
            this.Position = Vector3.Zero;
            this.Visible = true;

            this._worldMatrix = Matrix.Identity;
            this._transformationList = new LinkedList<Transformation>();
            this._needTransformationCompile = true;
            this._autoPosition = false;
        }

        public abstract void Draw(GameTime gameTime);

        #region Transformation methods
        /// <summary>
        /// Adds a scale transformation to the transformation queue
        /// </summary>
        /// <param name="degrees"></param>
        public void Scale(float degrees)
        {
            Transformation trans = new Transformation();
            trans.Operations |= (int)Transformation.Operation.Scale;
            trans.Value = degrees;
            _transformationList.AddLast(trans);
            this._needTransformationCompile = true;
        }

        /// <summary>
        /// Adds a rotation along the X axis transformation to the transformation queue
        /// </summary>
        /// <param name="degrees"></param>
        public void ApplyRotationX(float degrees)
        {
            Transformation trans = new Transformation();
            trans.Operations |= (int)Transformation.Operation.Rotate;
            trans.Axis = Transformation.Axes.X;
            trans.Value = degrees;
            _transformationList.AddLast(trans);
            this._needTransformationCompile = true;
        }

        /// <summary>
        /// Adds a rotation along the Y axis transformation to the transformation queue
        /// </summary>
        /// <param name="degrees"></param>
        public void ApplyRotationY(float degrees)
        {
            Transformation trans = new Transformation();
            trans.Operations |= (int)Transformation.Operation.Rotate;
            trans.Axis = Transformation.Axes.Y;
            trans.Value = degrees;
            _transformationList.AddLast(trans);
            this._needTransformationCompile = true;
        }

        /// <summary>
        /// Adds a rotation along the Z axis transformation to the transformation queue
        /// </summary>
        /// <param name="degrees"></param>
        public void ApplyRotationZ(float degrees)
        {
            Transformation trans = new Transformation();
            trans.Operations |= (int)Transformation.Operation.Rotate;
            trans.Axis = Transformation.Axes.Z;
            trans.Value = degrees;
            _transformationList.AddLast(trans);
            this._needTransformationCompile = true;
        }

        /// <summary>
        /// Adds a translation transformation to the transformation queue
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public void Translate(float x, float y, float z)
        {
            Transformation trans = new Transformation();
            trans.Operations |= (int)Transformation.Operation.Translate;
            trans.Offset = new Vector3(x, y, z);
            _transformationList.AddLast(trans);
            this._needTransformationCompile = true;
        }

        #endregion

        /// <summary>
        /// Converts all queued transformations into a usable World Matrix
        /// </summary>
        public void CompileTransformations()
        {
            if (!_needTransformationCompile) {
                return;
            }

            _worldMatrix = Matrix.Identity;

            foreach (Transformation trans in _transformationList) {
                if ((trans.Operations & (int)Transformation.Operation.Scale) != 0) {
                    _worldMatrix *= Matrix.CreateScale(trans.Value);
                }

                if ((trans.Operations & (int)Transformation.Operation.Rotate) != 0) {
                    if (trans.Axis == Transformation.Axes.X) {
                        _worldMatrix *= Matrix.CreateRotationX(MathHelper.ToRadians(trans.Value));
                    }
                    else if (trans.Axis == Transformation.Axes.Y) {
                        _worldMatrix *= Matrix.CreateRotationY(MathHelper.ToRadians(trans.Value));
                    }
                    else if (trans.Axis == Transformation.Axes.Z) {
                        _worldMatrix *= Matrix.CreateRotationZ(MathHelper.ToRadians(trans.Value));
                    }
                }

                if ((trans.Operations & (int)Transformation.Operation.Translate) != 0) {
                    _worldMatrix *= Matrix.CreateTranslation(trans.Offset);
                }
            }

            if (AutoPosition) {
                _worldMatrix *= Matrix.CreateTranslation(Position);
            }

            this._needTransformationCompile = false;
        }

        #region XML Load and Save

        /// <summary>
        /// Writes out stored transformations
        /// </summary>
        /// <param name="xmlDocument"></param>
        /// <param name="rootNode"></param>
        public virtual void Save(XmlDocument xmlDocument, XmlNode rootNode)
        {
            XmlNode transformationsNode = xmlDocument.CreateElement("Transformations");

            foreach (Transformation trans in _transformationList) {
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
                    float value = (float)Convert.ToDouble(trans["Value"].InnerText);
                    newTrans.Value = value;
                    newTrans.Operations = (int)Transformation.Operation.Scale;
                }
                else if (trans.Name == "Rotate") {
                    float value = (float)Convert.ToDouble(trans["Value"].InnerText);
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

                _transformationList.AddLast(newTrans);
            }

            this._needTransformationCompile = true;
        }

        #endregion
    }
}
