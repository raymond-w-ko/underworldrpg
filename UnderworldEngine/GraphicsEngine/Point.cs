using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    }
}
