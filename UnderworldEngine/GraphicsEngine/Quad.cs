﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnderworldEngine.GraphicsEngine
{
    class Quad
    {
        Point _a;
        Point A
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
        Point B
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
        Point C
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

        Point _d;
        Point D
        {
            get
            {
                return _d;
            }
            set
            {
                _d = value;
            }
        }

        public Quad(Point a, Point b, Point c, Point d)
        {
            this.A = a;
            this.B = b;
            this.C = c;
            this.D = d;
        }
    }
}
