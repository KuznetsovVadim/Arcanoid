using UnityEngine;
using Helper;

namespace Models
{
    public class Paddle
    {
        private Transform paddlePosition;
        private Vector2 nextPaddlePosition;
        private float paddleY = Constants.PADDLE_Y_POSITION;
        private float leftBorderPositionX;
        private float RightBorderPositionX;
        private float Speed = Constants.PADDLE_SPEED;
        private Camera camera;

        public Paddle(Transform paddlePosition, Camera camera)
        {
            this.paddlePosition = paddlePosition;
            nextPaddlePosition = new Vector2(0, paddleY);
            this.camera = camera;
            CalculatePaddleBorders();
        }

        public void CalculatePaddleBorders()
        {
            var halfPaddleSize = paddlePosition.gameObject.GetComponent<SpriteRenderer>().bounds.size.x / 2;
            leftBorderPositionX = Constants.LEFT_SIDE_POSITION_X + halfPaddleSize;
            RightBorderPositionX = Constants.RIGHT_SIDE_POSITION_X - halfPaddleSize;
        }

        public Vector3 OnModelUpdate(float time)
        {
            float PositionX = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, 0,0)).x;
            float clampedPosition = Mathf.Clamp(PositionX, leftBorderPositionX, RightBorderPositionX);
            nextPaddlePosition.x = clampedPosition;
            return Vector2.LerpUnclamped(paddlePosition.position, nextPaddlePosition, Speed * time);
        }
    }
}

