using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace UnderworldEngine.Game
{
    class BillboardQuadTexture : QuadTexture
    {
        public Vector2 GridPosition;
        public float Height;
        private Vector2 _dimensions;
        private float _aspectRatio;
        private float _scale;
        public new float Scale
        {
            get
            {
                return _scale;
            }
            set
            {
                if (_scale <= 0) {
                    throw new ApplicationException("Scale Value must be greater than 0.");
                }
                _scale = value;
            }
        }

        public Vector3 Offset;

        private Vector3 _origin;
        private Vector3 _normal;
        private Vector3 _up;

        public BillboardQuadTexture(Vector3 gridPosition, float aspectRatio, string textureName)
            : base(new Vector3(0, 0, 0), Vector3.Up, Vector3.Forward, 0, 0, textureName)
        {
            GridPosition = new Vector2(gridPosition.X, gridPosition.Z);
            Height = gridPosition.Y;
            _dimensions = new Vector2(1.0f, 1.0f);
            _aspectRatio = aspectRatio;
            _scale = 1.0f;

            CalculateVertices();
        }

        public void CalculateVertices()
        {
            _origin = new Vector3(GridPosition.X + .5f, Height + Offset.Y, GridPosition.Y + .5f);
            _normal = (Game1.Camera.CurrentPosition - Game1.Camera.CurrentTarget);
            _normal.Normalize();
            _up = Vector3.Up;

            float width = _scale;
            float height = _scale * _aspectRatio;

            base.CalculateVertices(_origin, _normal, _up, width, height);
            base.FillVertices();
        }

        public void Update(GameTime gameTime)
        {
            CalculateVertices();
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
