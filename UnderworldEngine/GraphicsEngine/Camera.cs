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

        private Vector3 currentPosition = Vector3.Zero;
        private Vector3 currentTarget = Vector3.Zero;
        private Vector3 currentUpVector = Vector3.Zero;

        private float nearPlaneDistance = 0.0f;
        private float farPlaneDistance = 100.0f;
        public class PlaneDistanceException : System.ApplicationException { };

        private float zoomFactor = 50.0f;
        public class InvalidZoomFactor : ApplicationException { }

        private const float NORMAL_CAMERA_BOX_SIZE = 8.0f;
        private float cameraBoxSize = NORMAL_CAMERA_BOX_SIZE;
        private Vector3[] allowableCameraPositions = {
            Vector3.Zero,
            Vector3.Zero,
            Vector3.Zero,
            Vector3.Zero
                                                     };
        private enum CameraLocation
        {
            UpperLeft,
            UpperRight,
            LowerRight,
            LowerLeft
        }

        private const int NUM_CAMERA_LOCATIONS = 4;

        private CameraLocation currentCameraLocation = CameraLocation.LowerRight;

        public Camera()
        {
            this.MoveTo(0, 0, 0);
            this.LookAt(0, 0, 0);
            this.SetUpVector(Vector3.Up);

            this.SetNearPlaneDistance(0.0f);
            this.SetFarPlaneDistance(100.0f);

            allowableCameraPositions = new Vector3[4];
        }

        private void MoveTo(float x, float y, float z)
        {
            MoveTo(new Vector3(x, y, z));
        }
        private void MoveTo(Vector3 vector)
        {
            this.currentPosition = vector;
        }

        public void LookAt(float x, float y, float z)
        {
            this.LookAt(new Vector3(x, y, z));
        }
        public void LookAt(Vector3 vector)
        {
            currentTarget = vector;

            allowableCameraPositions[0].X = currentTarget.X - cameraBoxSize;
            allowableCameraPositions[0].Y = currentTarget.Y + cameraBoxSize;
            allowableCameraPositions[0].Z = currentTarget.Z + cameraBoxSize;

            allowableCameraPositions[1].X = currentTarget.X + cameraBoxSize;
            allowableCameraPositions[1].Y = currentTarget.Y + cameraBoxSize;
            allowableCameraPositions[1].Z = currentTarget.Z + cameraBoxSize;

            allowableCameraPositions[2].X = currentTarget.X + cameraBoxSize;
            allowableCameraPositions[2].Y = currentTarget.Y + cameraBoxSize;
            allowableCameraPositions[2].Z = currentTarget.Z - cameraBoxSize;

            allowableCameraPositions[3].X = currentTarget.X - cameraBoxSize;
            allowableCameraPositions[3].Y = currentTarget.Y + cameraBoxSize;
            allowableCameraPositions[3].Z = currentTarget.Z - cameraBoxSize;

            this.MoveTo(allowableCameraPositions[(int)currentCameraLocation]);

            recalculateMatrices();
        }

        public void SetUpVector(float x, float y, float z)
        {
            SetUpVector(x, y, z);
        }
        public void SetUpVector(Vector3 vector)
        {
            currentUpVector = vector;
            recalculateMatrices();
        }

       
        public void SetNearPlaneDistance(float dist)
        {
            if (dist < 1.0f) {
                //throw new PlaneDistanceException();
            }
            this.nearPlaneDistance = dist;

            this.recalculateMatrices();
        }

        public void SetFarPlaneDistance(float dist)
        {
            if ((dist < 0) || (dist > 1000)) {
                throw new PlaneDistanceException();
            }
            this.farPlaneDistance = dist;

            this.recalculateMatrices();
        }

        
        public void SetZoomFactor(float zoom)
        {
            if (zoom <= 0.0f) {
                throw new InvalidZoomFactor();
            }

            this.zoomFactor = zoom;

            this.recalculateMatrices();
        }

        private void recalculateMatrices()
        {
            viewMatrix = Matrix.CreateLookAt(currentPosition, currentTarget, currentUpVector);

            //float viewWidth = Game1.DefaultGraphicsDevice.Viewport.Width / this.zoomFactor;
            //float viewHeight = Game1.DefaultGraphicsDevice.Viewport.Height / this.zoomFactor;
            float viewWidth = cameraBoxSize;// *(float)Math.Sqrt(2.0);
            float viewHeight = viewWidth / Game1.DefaultGraphicsDevice.Viewport.AspectRatio;
            projectionMatrix = Matrix.CreateOrthographic(
                viewWidth, viewHeight,
                this.nearPlaneDistance, this.farPlaneDistance);
        }

        Keys lastPressedKey = Keys.F13;

        public void Update()
        {
            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.A)) {
                lastPressedKey = Keys.A;
            }
            else if (keyState.IsKeyDown(Keys.D)) {
                lastPressedKey = Keys.D;
            }
            else if (keyState.IsKeyUp(Keys.A) && lastPressedKey == Keys.A) {
                lastPressedKey = Keys.F13;
                prevCameraView();
            }
            else if (keyState.IsKeyUp(Keys.D) && lastPressedKey == Keys.D) {
                lastPressedKey = Keys.F13;
                nextCameraView();
            }

            /*
            Matrix curPosMatrix = new Matrix();
            curPosMatrix.M11 = currentPosition.X;
            curPosMatrix.M21 = currentPosition.Y;
            curPosMatrix.M31 = currentPosition.Z;

            //Game1.Debug.WriteLine("current");
            //Game1.Debug.WriteLine(curPosMatrix);

            Matrix newPosMatrix =
            Matrix.CreateFromYawPitchRoll(MathHelper.ToRadians((float)(45.0 / (60.0 * 2.0))), 0, 0)
            * curPosMatrix;

            //Game1.Debug.WriteLine("new");
            //Game1.Debug.WriteLine(newPosMatrix);

            currentPosition.X = newPosMatrix.M11;
            currentPosition.Y = newPosMatrix.M21;
            currentPosition.Z = newPosMatrix.M31;
            */

            currentPosition = Vector3.Transform(currentPosition,
                Matrix.CreateRotationY(MathHelper.ToRadians((float)(45.0 / (60.0))))
                );

            recalculateMatrices();
        }

        private void prevCameraView()
        {
            int temp = (int)currentCameraLocation;
            temp--;
            temp += Camera.NUM_CAMERA_LOCATIONS;
            temp %= Camera.NUM_CAMERA_LOCATIONS;
            currentCameraLocation = (CameraLocation)temp;
        }

        private void nextCameraView()
        {
            int temp = (int)currentCameraLocation;
            temp++;
            temp += Camera.NUM_CAMERA_LOCATIONS;
            temp %= Camera.NUM_CAMERA_LOCATIONS;
            currentCameraLocation = (CameraLocation)temp;
        }
    }
}
