using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace UnderworldEngine.GraphicsEngine
{
    class Point
    {
        float _x;
        float X
        {
            get
            {
                return _x;
            }
            set
            {
                _x = value;
            }
        }

        float _y;
        float Y
        {
            get
            {
                return _y;
            }
            set
            {
                _y = value;
            }
        }

        float _z;
        float Z
        {
            get
            {
                return _z;
            }
            set
            {
                _z = value;
            }
        }

        public Point(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public void Draw(GraphicsDevice gd)
        {
            VertexPositionColor[] pointList = new VertexPositionColor[1];
            pointList[0] = new VertexPositionColor(
                new Vector3(this.X, this.Y, this.Z), Color.White);
            VertexBuffer vb = new VertexBuffer(gd, 
                VertexPositionColor.SizeInBytes * pointList.Length, BufferUsage.None);

            vb.SetData<VertexPositionColor>(pointList);
            gd.DrawUserPrimitives<VertexPositionColor>(
                PrimitiveType.PointList,
                pointList, 
                0, 
                1
            );
        }
    }
}
