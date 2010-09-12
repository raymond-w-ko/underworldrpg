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

        private const float NORMAL_CAMERA_BOX_SIZE = 10.0f;
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

        private Vector3 ringPosition;
        float turnSpeed;
        Matrix turnMatrix;

        private CameraLocation futureCameraLocation;
        private Vector3 futureCameraPosition;

        public Camera()
        {
            this.MoveTo(0, 0, 0);
            this.LookAt(0, 0, 0);
            this.SetUpVector(Vector3.Up);

            this.SetNearPlaneDistance(0.0f);
            this.SetFarPlaneDistance(100.0f);

            allowableCameraPositions = new Vector3[4];
            ringPosition = new Vector3(0, cameraBoxSize, 0);
            turnSpeed = MathHelper.ToRadians((float)(45.0 / (60.0 / 3.0)));
            
            turnMatrix = Matrix.CreateRotationY(turnSpeed);

            futureCameraLocation = currentCameraLocation;
            futureCameraPosition = currentPosition;
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
            if (currentCameraLocation == CameraLocation.UpperLeft) {
                ringPosition.X = -cameraBoxSize;
                ringPosition.Z = +cameraBoxSize;
            }
            else if (currentCameraLocation == CameraLocation.UpperRight) {
                ringPosition.X = +cameraBoxSize;
                ringPosition.Z = +cameraBoxSize;
            }
            else if (currentCameraLocation == CameraLocation.LowerRight) {
                ringPosition.X = +cameraBoxSize;
                ringPosition.Z = -cameraBoxSize;
            }
            else if (currentCameraLocation == CameraLocation.LowerLeft) {
                ringPosition.X = -cameraBoxSize;
                ringPosition.Z = -cameraBoxSize;
            }
            ringPosition.Y = cameraBoxSize;

            recalculateViewMatrix();
        }

        public void SetUpVector(float x, float y, float z)
        {
            SetUpVector(x, y, z);
        }
        public void SetUpVector(Vector3 vector)
        {
            currentUpVector = vector;
            recalculateViewMatrix();
        }

       
        public void SetNearPlaneDistance(float dist)
        {
            if (dist < 1.0f) {
                //throw new PlaneDistanceException();
            }
            this.nearPlaneDistance = dist;

            this.recalculateProjectionMatrix();
        }

        public void SetFarPlaneDistance(float dist)
        {
            if ((dist < 0) || (dist > 1000)) {
                throw new PlaneDistanceException();
            }
            this.farPlaneDistance = dist;

            this.recalculateProjectionMatrix();
        }

        
        public void SetZoomFactor(float zoom)
        {
            if (zoom <= 0.0f) {
                throw new InvalidZoomFactor();
            }

            this.zoomFactor = zoom;

            this.recalculateViewMatrix();
        }

        private void recalculateViewMatrix()
        {
            viewMatrix = Matrix.CreateLookAt(currentPosition, currentTarget, currentUpVector);

        }

        private void recalculateProjectionMatrix()
        {
            float viewWidth = cameraBoxSize;
            float viewHeight = viewWidth / Game1.DefaultGraphicsDevice.Viewport.AspectRatio;
            projectionMatrix = Matrix.CreateOrthographic(
                viewWidth, viewHeight,
                this.nearPlaneDistance, this.farPlaneDistance);
        }

        Keys lastPressedKey = Keys.F13;

        public void Update()
        {
            KeyboardState keyState = Keyboard.GetState();
            if (currentCameraLocation != futureCameraLocation)
            {
                advanceCamera();
                if (currentPosition.AlmostEquals(futureCameraPosition, 0.01f)) {
                    currentCameraLocation = futureCameraLocation;
                }
            }
            else if (keyState.IsKeyDown(Keys.A)) {
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

            recalculateViewMatrix();
        }

        private void advanceCamera()
        {
            Vector3.Transform(ref ringPosition,
                ref turnMatrix,
                out ringPosition
                );
            currentPosition.X = ringPosition.X + currentTarget.X;
            currentPosition.Z = ringPosition.Z + currentTarget.Z;
        }

        private void prevCameraView()
        {
            int temp = (int)currentCameraLocation;
            temp--;
            temp += Camera.NUM_CAMERA_LOCATIONS;
            temp %= Camera.NUM_CAMERA_LOCATIONS;
            futureCameraLocation = (CameraLocation)temp;
            futureCameraPosition = allowableCameraPositions[(int)futureCameraLocation];
            turnMatrix = Matrix.CreateRotationY(-turnSpeed);
            LookAt(currentTarget);
        }

        private void nextCameraView()
        {
            int temp = (int)currentCameraLocation;
            temp++;
            temp += Camera.NUM_CAMERA_LOCATIONS;
            temp %= Camera.NUM_CAMERA_LOCATIONS;
            futureCameraLocation = (CameraLocation)temp;
            futureCameraPosition = allowableCameraPositions[(int)futureCameraLocation];
            turnMatrix = Matrix.CreateRotationY(turnSpeed);
            LookAt(currentTarget);
        }
    }

    public static class MyExtensions
    {
        public static bool AlmostEquals(this Vector3 vec, Vector3 vec2, float nEpsilon)
        {
	        bool bRet1 = (((vec2.X - nEpsilon) < vec.X) && (vec.X < (vec2.X + nEpsilon)));
            bool bRet2 = (((vec2.Y - nEpsilon) < vec.Y) && (vec.Y < (vec2.Y + nEpsilon)));
            bool bRet3 = (((vec2.Z - nEpsilon) < vec.Z) && (vec.Z < (vec2.Z + nEpsilon)));
	        return bRet1 && bRet2 && bRet3;
        }
    }
}
