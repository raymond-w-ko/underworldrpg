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

namespace UnderworldEngine.GraphicsEngine
{
    class Camera
    {
        private Matrix viewMatrix;
        public Matrix ViewMatrix
        {
            get
            {
                return viewMatrix;
            }
        }

        private Matrix projectionMatrix;
        public Matrix ProjectionMatrix
        {
            get
            {
                return projectionMatrix;
            }
        }

        private Vector3 currentPosition;
        private Vector3 currentTarget;
        private Vector3 currentUpVector;

        private float fov = (float)(Math.PI/2);  //90degree field of view by default
        private float aspectRatio = 1.33f; //4:3 default
        public class FovOutOfRangeException : System.ApplicationException { };

        private float nearPlaneDistance = 1.0f;
        private float farPlaneDistance = 5.0f;
        public class PlaneDistanceException : System.ApplicationException { };

        private Viewport viewPort;

        public Camera(Viewport view)
        {
            this.MoveTo(0, 0, 5);
            this.LookAt(0, 0, 0);
            this.SetUpVector(Vector3.Up);

            this.SetFovDegrees(45.0f);

            this.SetNearPlaneDistance(1.0f);
            this.SetFarPlaneDistance(100.0f);

            this.viewPort = view;
        }

        public void SetNearPlaneDistance(float dist)
        {
            if (dist < 1.0f) {
                throw new PlaneDistanceException();
            }
            this.nearPlaneDistance = dist;

            this.Update();
        }

        public void SetFarPlaneDistance(float dist)
        {
            if ((dist < 0) || (dist > 1000)) {
                throw new PlaneDistanceException();
            }
            this.farPlaneDistance = dist;

            this.Update();
        }

        public void CalculateAspectRatio(Viewport viewport)
        {
            this.aspectRatio = ((float)viewport.Width) / ((float)viewport.Height);

            this.Update();
        }

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

        public void MoveTo(float x, float y, float z)
        {
            MoveTo(new Vector3(x, y, z));
        }

        public void MoveTo(Vector3 vector)
        {
            this.currentPosition = vector;
            Update();
        }

        public void LookAt(float x, float y, float z)
        {
            this.LookAt(new Vector3(x, y, z));
        }

        public void LookAt(Vector3 vector)
        {
            currentTarget = vector;
            Update();
        }

        public void SetUpVector(float x, float y, float z)
        {
            SetUpVector(x, y, z);
        }

        public void SetUpVector(Vector3 vector)
        {
            currentUpVector = vector;
            Update();
        }

        private void Update()
        {
            viewMatrix = Matrix.CreateLookAt(currentPosition, currentTarget, currentUpVector);

            if (this.nearPlaneDistance > this.farPlaneDistance) {
                throw new PlaneDistanceException();
            }
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(fov, aspectRatio, nearPlaneDistance,
                farPlaneDistance);
            projectionMatrix = Matrix.CreateOrthographic(viewPort.Width/20.0f, viewPort.Height/20.0f, 0, 1000);

        }
    }
}
