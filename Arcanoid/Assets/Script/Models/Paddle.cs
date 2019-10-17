using UnityEngine;
using Helper;

namespace Models
{
    public class Paddle
    {
        private Transform paddlePosition;
        private Vector3 nextPaddlePosition;
        private float paddleY = Constants.PADDLE_Y_POSITION;
        private float leftBorderPositionX = Constants.LEFT_SIDE_POSITION_X;
        private float RightBorderPositionX = Constants.RIGHT_SIDE_POSITION_X;
        private float Speed = 5f;
        private Camera camera;

        public Paddle(Transform paddlePosition, Camera camera)
        {
            this.paddlePosition = paddlePosition;
            nextPaddlePosition = new Vector3(0, paddleY, 0);
            this.camera = camera;
        }

        public Vector3 OnModelUpdate(float time)
        {
            float PositionX = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, 0,0)).x;
            float clampedPosition = Mathf.Clamp(PositionX, leftBorderPositionX, RightBorderPositionX);
            nextPaddlePosition.x = clampedPosition;
            return Vector3.Lerp(paddlePosition.position, nextPaddlePosition, Speed * time);
        }
    }
}

