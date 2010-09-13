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

namespace UnderworldEngine.Graphics
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

        private const float NORMAL_CAMERA_BOX_SIZE = 500.0f;
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
        private Vector3 futurePosition;

        private bool IsAcceptingCommands;

        private Vector3 futureTarget;
        private float moveSpeed;
        private Vector3 direction;

        public Camera()
        {
            this.SetUpVector(Vector3.Up);

            this.SetNearPlaneDistance(0.0f);
            this.SetFarPlaneDistance(100.0f);

            allowableCameraPositions = new Vector3[4];
            ringPosition = new Vector3(cameraBoxSize, cameraBoxSize, -cameraBoxSize);

            // Velocities
            turnSpeed = MathHelper.ToRadians((float)(45.0 / (60.0 / 4.0)));
            turnMatrix = Matrix.CreateRotationY(turnSpeed);
            moveSpeed = (float)(10.0 / 60.0);

            futureCameraLocation = currentCameraLocation = CameraLocation.LowerRight;
            currentTarget = new Vector3(0, 0, 0);
            futureTarget = new Vector3(0, 0, 0);
            currentPosition = new Vector3(8, 8, 8);
            futurePosition = new Vector3(8, 8, 8);

            this.IsAcceptingCommands = true;
        }

        private void moveTo(float x, float y, float z)
        {
            moveTo(new Vector3(x, y, z));
        }
        private void moveTo(Vector3 vector)
        {
            this.currentPosition = vector;
        }

        public void LookAt(float x, float y, float z)
        {
            this.LookAt(new Vector3(x, y, z));
        }
        public void LookAt(Vector3 vector)
        {
            if (IsAcceptingCommands) {
                IsAcceptingCommands = false;
            }
            else {
                return;
            }
            futureTarget = vector;

            allowableCameraPositions[0].X = vector.X - cameraBoxSize;
            allowableCameraPositions[0].Y = vector.Y + cameraBoxSize;
            allowableCameraPositions[0].Z = vector.Z + cameraBoxSize;

            allowableCameraPositions[1].X = vector.X + cameraBoxSize;
            allowableCameraPositions[1].Y = vector.Y + cameraBoxSize;
            allowableCameraPositions[1].Z = vector.Z + cameraBoxSize;

            allowableCameraPositions[2].X = vector.X + cameraBoxSize;
            allowableCameraPositions[2].Y = vector.Y + cameraBoxSize;
            allowableCameraPositions[2].Z = vector.Z - cameraBoxSize;

            allowableCameraPositions[3].X = vector.X - cameraBoxSize;
            allowableCameraPositions[3].Y = vector.Y + cameraBoxSize;
            allowableCameraPositions[3].Z = vector.Z - cameraBoxSize;

            futurePosition = allowableCameraPositions[(int)currentCameraLocation];
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

            direction = futureTarget - currentTarget;
            direction.Normalize();
            direction *= moveSpeed;
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
            if (dist < 0.0f) {
                throw new PlaneDistanceException();
            }
            this.nearPlaneDistance = dist;

            this.recalculateProjectionMatrix();
        }

        public void SetFarPlaneDistance(float dist)
        {
            if ((dist < 0) || (dist > 10000)) {
                throw new PlaneDistanceException();
            }
            this.farPlaneDistance = dist;

            this.recalculateProjectionMatrix();
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

        private void rotateAlongRing()
        {
            Vector3.Transform(ref ringPosition,
                ref turnMatrix,
                out ringPosition
                );
            currentPosition = currentTarget + ringPosition;
        }

        private void moveSmoothly()
        {
            currentTarget += direction;
            currentPosition = currentTarget + ringPosition;
        }

        public void PrevCameraView()
        {
            if (IsAcceptingCommands) {
                IsAcceptingCommands = false;
            }
            else {
                return;
            }

            int temp = (int)currentCameraLocation;
            temp--;
            temp += Camera.NUM_CAMERA_LOCATIONS;
            temp %= Camera.NUM_CAMERA_LOCATIONS;
            futureCameraLocation = (CameraLocation)temp;
            futurePosition = allowableCameraPositions[(int)futureCameraLocation];
            turnMatrix = Matrix.CreateRotationY(-turnSpeed);
            LookAt(currentTarget);
        }

        public void NextCameraView()
        {
            if (IsAcceptingCommands) {
                IsAcceptingCommands = false;
            }
            else {
                return;
            }

            IsAcceptingCommands = false;
            int temp = (int)currentCameraLocation;
            temp++;
            temp += Camera.NUM_CAMERA_LOCATIONS;
            temp %= Camera.NUM_CAMERA_LOCATIONS;
            futureCameraLocation = (CameraLocation)temp;
            futurePosition = allowableCameraPositions[(int)futureCameraLocation];
            turnMatrix = Matrix.CreateRotationY(turnSpeed);
            LookAt(currentTarget);
        }

        public void Update()
        {
            if (currentCameraLocation != futureCameraLocation) {
                rotateAlongRing();
                if (currentPosition.AlmostEquals(futurePosition, 0.001f)) {
                    currentCameraLocation = futureCameraLocation;
                    currentPosition = this.allowableCameraPositions[(int)currentCameraLocation];
                    IsAcceptingCommands = true;
                }
            }
            else if (currentTarget != futureTarget) {
                moveSmoothly();
                if (currentTarget.AlmostEquals(futureTarget, moveSpeed)) {
                    currentTarget = futureTarget;
                    currentPosition = this.allowableCameraPositions[(int)currentCameraLocation];
                    IsAcceptingCommands = true;
                }
            }

            recalculateViewMatrix();
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
