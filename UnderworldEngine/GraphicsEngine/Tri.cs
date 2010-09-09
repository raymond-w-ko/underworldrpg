using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnderworldEngine.GraphicsEngine
{
    class Tri
    {
        Point _a;
        public Point A
        {
            get
            {
                return _a;
            }
            set
            {
                _a = value;
            }
        }

        Point _b;
        public Point B
        {
            get
            {
                return _b;
            }
            set
            {
                _b = value;
            }
        }

        Point _c;
        public Point C
        {
            get
            {
                return _c;
            }
            set
            {
                _c = value;
            }
        }

        public Tri(Point a, Point b, Point c)
        {
            this.A = a;
            this.B = b;
            this.C = c;
        }
    }
}
