using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UnderworldEngine.Graphics
{
    public class Camera
    {
        #region View and Projection Matrices Fields
        private Matrix _viewMatrix;
        public Matrix ViewMatrix
        {
            get
            {
                return _viewMatrix;
            }
        }
        

        private Matrix _projectionMatrix;
        public Matrix ProjectionMatrix
        {
            get
            {
                return _projectionMatrix;
            }
        }
        #endregion

        #region Current Position, Target, and Up Vectors
        private Vector3 _currentPosition;
        public Vector3 CurrentPosition
        {
            get
            {
                return _currentPosition;
            }
        }
        private Vector3 _currentTarget;
        public Vector3 CurrentTarget
        {
            get
            {
                return _currentTarget;
            }
        }
        private Vector3 _currentUpVector;
        public Vector3 CurrentUpVector
        {
            get
            {
                return _currentUpVector;
            }
        }
        #endregion

        #region Near & Far Plane Distance Fields
        private float _nearPlaneDistance = 0.1f;
        public float NearPlaneDistance
        {
            get
            {
                return _nearPlaneDistance;
            }
        }
        private float _farPlaneDistance = 100.0f;
        public float FarPlaneDistance
        {
            get
            {
                return _farPlaneDistance;
            }
        }
        public class PlaneDistanceException : System.ApplicationException { };
        #endregion

        #region Camera Box Fields

        private const float NORMAL_CAMERA_BOX_SIZE = 12.0f;

        private float _cameraBoxSize = NORMAL_CAMERA_BOX_SIZE;
        private float _futureCameraBoxSize = NORMAL_CAMERA_BOX_SIZE;

        private Vector3[] allowableCameraPositions = {
            Vector3.Zero,
            Vector3.Zero,
            Vector3.Zero,
            Vector3.Zero
            };
        public enum CameraLocation
        {
            UpperLeft,
            UpperRight,
            LowerRight,
            LowerLeft
        }
        private const int NUM_CAMERA_LOCATIONS = 4;

        public enum Direction
        {
            Up,
            Down,
            Left,
            Right
        }

        private Vector3[] _relativeDirectionOffsets;

        private CameraLocation _currentCameraLocation;
        private CameraLocation _futureCameraLocation;

        private Vector3 _ringPosition;

        private float _turnSpeed;
        /// <summary>
        /// Turn Speed is in terms of degrees per second
        /// </summary>
        public float TurnSpeed
        {
            get
            {
                return _turnSpeed;
            }
            set
            {
                float newSpeed = value / 60.0f;
                if (newSpeed >= (360.0f / 60.0f)) {
                    throw new ApplicationException("Camera Turn Velocity is too fast.");
                }
                _turnSpeed = MathHelper.ToRadians(newSpeed);
            }
        }

        private Matrix _turnMatrix;

        #endregion

        private Vector3 _futurePosition;
        
        private Vector3 _futureTarget;
        private float _moveSpeed;
        /// <summary>
        /// MoveSpeed is defined in terms of Direct X units per second
        /// </summary>
        public float MoveSpeed
        {
            get
            {
                return _moveSpeed * 60.0f;
            }
            set
            {
                float newSpeed = value / 60.0f;
                if (newSpeed > (1.0f)) {
                    throw new ApplicationException("Camera Movement Velocity is too fast.");
                }
                _moveSpeed = newSpeed;
            }
        }

        private float _zoomSpeed;
        public float ZoomSpeed
        {
            get
            {
                return _zoomSpeed;
            }
            set
            {
                float newSpeed = value / 60.0f;
                if (newSpeed >= (1.0f)) {
                    throw new ApplicationException("Camera Zoom Speed is too fast.");
                }
                _zoomSpeed = newSpeed;
            }
        }

        private Vector3 _direction;

        private bool IsAcceptingCommands;

        public Camera()
        {
            this.SetUpVector(Vector3.Up);

            this.SetNearPlaneDistance(1.0f);
            this.SetFarPlaneDistance(100.0f);

            allowableCameraPositions = new Vector3[4];
            _ringPosition = new Vector3(_cameraBoxSize, _cameraBoxSize, -_cameraBoxSize);

            // Velocities
            _turnSpeed = MathHelper.ToRadians((float)(45.0 / (60.0 / 4.0)));
            _turnMatrix = Matrix.CreateRotationY(_turnSpeed);
            _moveSpeed = (float)(10.0 / 60.0);
            _zoomSpeed = (float)(10.0 / 60.0);

            _futureCameraLocation = _currentCameraLocation = CameraLocation.LowerLeft;
            _currentTarget = new Vector3(0, 0, 0);
            _futureTarget = new Vector3(0, 0, 0);
            _currentPosition = new Vector3(8, 8, 8);
            _futurePosition = new Vector3(8, 8, 8);

            this.IsAcceptingCommands = true;

            calculateCameraBox(_currentTarget);
            _currentPosition = _currentTarget + _ringPosition;

            calculateRelativeDirectionOffset();

            recalculateProjectionMatrix();
            recalculateViewMatrix();
        }

        private void moveTo(float x, float y, float z)
        {
            moveTo(new Vector3(x, y, z));
        }
        private void moveTo(Vector3 vector)
        {
            this._currentPosition = vector;
        }

        #region Look At

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
            _futureTarget = vector;

            calculateCameraBox(vector);
            _futurePosition = allowableCameraPositions[(int)_currentCameraLocation];
            
            recalculateViewMatrix();

            _direction = _futureTarget - _currentTarget;
            _direction.Normalize();
            _direction *= _moveSpeed;
        }

        private void moveSmoothly()
        {
            _currentTarget += _direction;
            _currentPosition = _currentTarget + _ringPosition;
        }

        private void calculateRelativeDirectionOffset()
        {
            Vector3[] offsets = new Vector3[4];
            if (_currentCameraLocation == CameraLocation.LowerLeft) {
                offsets[(int)Direction.Up] = new Vector3(1, 0, 0);
                offsets[(int)Direction.Left] = new Vector3(0, 0, -1);
                offsets[(int)Direction.Down] = new Vector3(-1, 0, 0);
                offsets[(int)Direction.Right] = new Vector3(0, 0, 1);
            }
            else if (_currentCameraLocation == CameraLocation.LowerRight) {
                offsets[(int)Direction.Left] = new Vector3(1, 0, 0);
                offsets[(int)Direction.Down] = new Vector3(0, 0, -1);
                offsets[(int)Direction.Right] = new Vector3(-1, 0, 0);
                offsets[(int)Direction.Up] = new Vector3(0, 0, 1);
            }
            else if (_currentCameraLocation == CameraLocation.UpperRight) {
                offsets[(int)Direction.Down] = new Vector3(1, 0, 0);
                offsets[(int)Direction.Right] = new Vector3(0, 0, -1);
                offsets[(int)Direction.Up] = new Vector3(-1, 0, 0);
                offsets[(int)Direction.Left] = new Vector3(0, 0, 1);
            }
            else if (_currentCameraLocation == CameraLocation.UpperLeft) {
                offsets[(int)Direction.Right] = new Vector3(1, 0, 0);
                offsets[(int)Direction.Up] = new Vector3(0, 0, -1);
                offsets[(int)Direction.Left] = new Vector3(-1, 0, 0);
                offsets[(int)Direction.Down] = new Vector3(0, 0, 1);
            }

            _relativeDirectionOffsets = offsets;
        }

        public void LookUp()
        {
            float xDelta = _relativeDirectionOffsets[(int)Direction.Up].X;
            float zDelta = _relativeDirectionOffsets[(int)Direction.Up].Z;
            LookAt(_currentTarget.X + xDelta, _currentTarget.Y, _currentTarget.Z + zDelta);
        }

        public void LookDown()
        {
            float xDelta = _relativeDirectionOffsets[(int)Direction.Down].X;
            float zDelta = _relativeDirectionOffsets[(int)Direction.Down].Z;
            LookAt(_currentTarget.X + xDelta, _currentTarget.Y, _currentTarget.Z + zDelta);
        }

        public void LookLeft()
        {
            float xDelta = _relativeDirectionOffsets[(int)Direction.Left].X;
            float zDelta = _relativeDirectionOffsets[(int)Direction.Left].Z;
            LookAt(_currentTarget.X + xDelta, _currentTarget.Y, _currentTarget.Z + zDelta);
        }

        public void LookRight()
        {
            float xDelta = _relativeDirectionOffsets[(int)Direction.Right].X;
            float zDelta = _relativeDirectionOffsets[(int)Direction.Right].Z;
            LookAt(_currentTarget.X + xDelta, _currentTarget.Y, _currentTarget.Z + zDelta);
        }

        #endregion

        private void calculateCameraBox(Vector3 vector)
        {
            allowableCameraPositions[0].X = vector.X - _cameraBoxSize;
            allowableCameraPositions[0].Y = vector.Y + _cameraBoxSize;
            allowableCameraPositions[0].Z = vector.Z + _cameraBoxSize;

            allowableCameraPositions[1].X = vector.X + _cameraBoxSize;
            allowableCameraPositions[1].Y = vector.Y + _cameraBoxSize;
            allowableCameraPositions[1].Z = vector.Z + _cameraBoxSize;

            allowableCameraPositions[2].X = vector.X + _cameraBoxSize;
            allowableCameraPositions[2].Y = vector.Y + _cameraBoxSize;
            allowableCameraPositions[2].Z = vector.Z - _cameraBoxSize;

            allowableCameraPositions[3].X = vector.X - _cameraBoxSize;
            allowableCameraPositions[3].Y = vector.Y + _cameraBoxSize;
            allowableCameraPositions[3].Z = vector.Z - _cameraBoxSize;

            if (_currentCameraLocation == CameraLocation.UpperLeft) {
                _ringPosition.X = -_cameraBoxSize;
                _ringPosition.Z = +_cameraBoxSize;
            }
            else if (_currentCameraLocation == CameraLocation.UpperRight) {
                _ringPosition.X = +_cameraBoxSize;
                _ringPosition.Z = +_cameraBoxSize;
            }
            else if (_currentCameraLocation == CameraLocation.LowerRight) {
                _ringPosition.X = +_cameraBoxSize;
                _ringPosition.Z = -_cameraBoxSize;
            }
            else if (_currentCameraLocation == CameraLocation.LowerLeft) {
                _ringPosition.X = -_cameraBoxSize;
                _ringPosition.Z = -_cameraBoxSize;
            }
            _ringPosition.Y = _cameraBoxSize;
        }

        #region Up Vector

        public void SetUpVector(float x, float y, float z)
        {
            SetUpVector(x, y, z);
        }
        public void SetUpVector(Vector3 vector)
        {
            _currentUpVector = vector;
            recalculateViewMatrix();
        }

        #endregion

        #region Near and Far Plane
        public void SetNearPlaneDistance(float dist)
        {
            if (dist < 0.0f) {
                throw new PlaneDistanceException();
            }
            this._nearPlaneDistance = dist;

            this.recalculateProjectionMatrix();
        }

        public void SetFarPlaneDistance(float dist)
        {
            if ((dist < 0) || (dist > 10000)) {
                throw new PlaneDistanceException();
            }
            this._farPlaneDistance = dist;

            this.recalculateProjectionMatrix();
        }
        #endregion

        #region Recalculation of View and Projection Matrices
        private void recalculateViewMatrix()
        {
            _viewMatrix = Matrix.CreateLookAt(_currentPosition, _currentTarget, _currentUpVector);
        }

        private void recalculateProjectionMatrix()
        {
            float viewWidth = _cameraBoxSize;
            float viewHeight = viewWidth / Game1.DefaultGraphicsDevice.Viewport.AspectRatio;
            _projectionMatrix = Matrix.CreateOrthographic(
                viewWidth, viewHeight,
                this._nearPlaneDistance, this._farPlaneDistance
                );
        }
        #endregion

        #region Camera Rotation
        private void rotateAlongRing()
        {
            Vector3.Transform(ref _ringPosition,
                ref _turnMatrix,
                out _ringPosition
                );
            _currentPosition = _currentTarget + _ringPosition;
        }

        public void PrevCameraView()
        {
            if (IsAcceptingCommands) {
                IsAcceptingCommands = false;
            }
            else {
                return;
            }

            int temp = (int)_currentCameraLocation;
            temp--;
            temp += Camera.NUM_CAMERA_LOCATIONS;
            temp %= Camera.NUM_CAMERA_LOCATIONS;
            _futureCameraLocation = (CameraLocation)temp;
            _futurePosition = allowableCameraPositions[(int)_futureCameraLocation];
            _turnMatrix = Matrix.CreateRotationY(-_turnSpeed);
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
            int temp = (int)_currentCameraLocation;
            temp++;
            temp += Camera.NUM_CAMERA_LOCATIONS;
            temp %= Camera.NUM_CAMERA_LOCATIONS;
            _futureCameraLocation = (CameraLocation)temp;
            _futurePosition = allowableCameraPositions[(int)_futureCameraLocation];
            _turnMatrix = Matrix.CreateRotationY(_turnSpeed);
        }

        #endregion

        #region Camera Zoom
        private void zoomSmoothly()
        {
            if (_futureCameraBoxSize > _cameraBoxSize) {
                this._cameraBoxSize += _zoomSpeed;
            }
            else if (_futureCameraBoxSize < _cameraBoxSize) {
                this._cameraBoxSize -= _zoomSpeed;
            }

            calculateCameraBox(_currentTarget);
            _currentPosition = _currentTarget + _ringPosition;

            recalculateViewMatrix();
            recalculateProjectionMatrix();
        }

        public void ZoomCloser(uint dist)
        {
            // Check to make sure camera will not move too close
            // and invert to look at floor
            if (this._cameraBoxSize - dist < 2) {
                return;
            }

            if (IsAcceptingCommands) {
                IsAcceptingCommands = false;
            }
            else {
                return;
            }
            
            this._futureCameraBoxSize = this._cameraBoxSize - dist;
            this._futureCameraBoxSize = (float)Math.Round(this._futureCameraBoxSize);
        }

        public void ZoomFarther(uint dist)
        {
            if (IsAcceptingCommands) {
                IsAcceptingCommands = false;
            }
            else {
                return;
            }
            this._futureCameraBoxSize = this._cameraBoxSize + dist;
            this._futureCameraBoxSize = (float)Math.Round(this._futureCameraBoxSize);
        }
        #endregion

        public void Update(GameTime gameTime)
        {
            if (_currentCameraLocation != _futureCameraLocation) {
                rotateAlongRing();
                if (_currentPosition.AlmostEquals(_futurePosition, _turnSpeed)) {
                    _currentCameraLocation = _futureCameraLocation;
                    _currentPosition = this.allowableCameraPositions[(int)_currentCameraLocation];
                    calculateRelativeDirectionOffset();
                    IsAcceptingCommands = true;
                }
            }
            else if (_currentTarget != _futureTarget) {
                moveSmoothly();
                if (_currentTarget.AlmostEquals(_futureTarget, _moveSpeed)) {
                    _currentTarget = _futureTarget;
                    _currentPosition = this.allowableCameraPositions[(int)_currentCameraLocation];
                    IsAcceptingCommands = true;
                }
            }
            else if (_cameraBoxSize != _futureCameraBoxSize) {
                zoomSmoothly();
                if (_cameraBoxSize.AlmostEquals(_futureCameraBoxSize, _zoomSpeed)) {
                    _currentPosition = this.allowableCameraPositions[(int)_currentCameraLocation];
                    _cameraBoxSize = _futureCameraBoxSize;
                    IsAcceptingCommands = true;
                }
            }

            recalculateViewMatrix();
            recalculateProjectionMatrix();
        }
    }
}
