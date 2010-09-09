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
        VertexPositionColor vpc;

        public float X
        {
            get
            {
                return vpc.Position.X;
            }
            set
            {
                vpc.Position.X = value;
            }
        }

        public float Y
        {
            get
            {
                return vpc.Position.Y;
            }
            set
            {
                vpc.Position.Y = value;
            }
        }

        public float Z
        {
            get
            {
                return vpc.Position.Z;
            }
            set
            {
                vpc.Position.Z = value;
            }
        }

        public VertexPositionColor VPC
        {
            get
            {
                return vpc;
            }
        }

        public Point()
        {
            vpc = new VertexPositionColor(new Vector3(0, 0, 0), Color.White);
        }

        public Point(float x, float y, float z)
        {
            vpc = new VertexPositionColor(new Vector3(x, y, z), Color.White);
        }

        public Point(float x, float y, float z, Color color)
        {
            vpc = new VertexPositionColor(new Vector3(x, y, z), color);
        }

        public void Draw(GraphicsDevice gd)
        {
            VertexPositionColor[] pointList = new VertexPositionColor[1];
            pointList[0] = this.VPC;
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
