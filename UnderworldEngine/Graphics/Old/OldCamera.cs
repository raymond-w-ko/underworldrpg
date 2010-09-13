/*
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

namespace UnderworldEngine.Graphics.Old
{
    class OldCamera
    {
        private Matrix projectionMatrix;
        public Matrix ProjectionMatrix
        {
            get
            {
                return projectionMatrix;
            }
        }

        public OldCamera()
        {
            this.SetFovDegrees(45.0f);

            
        }
        

        public void CalculateAspectRatio(Viewport viewport)
        {
            this.aspectRatio = ((float)viewport.Width) / ((float)viewport.Height);

            this.Update();
        }

        private float fov = (float)(Math.PI / 2);  //90degree field of view by default
        private float aspectRatio = 1.33f; //4:3 default
        public class FovOutOfRangeException : System.ApplicationException { };
        public void SetFovDegrees(float degrees)
        {
            if ((degrees <= 0) || (degrees >= 180)) {
                throw new FovOutOfRangeException();
            }

            this.fov = MathHelper.ToRadians(degrees);

            this.Update();
        }

        public void SetFovRadians(float radians)
        {
            if ((radians <= 0) || (radians >= Math.PI)) {
                throw new FovOutOfRangeException();
            }

            this.fov = radians;

            this.Update();
        }

        public void Update()
        {
            //projectionMatrix = Matrix.CreatePerspectiveFieldOfView(fov, aspectRatio, nearPlaneDistance,
            //    farPlaneDistance); ;
        }
    }
}
*/