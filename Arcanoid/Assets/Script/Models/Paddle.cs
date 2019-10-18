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
        private SpriteRenderer renderer;
        private Vector3 standartSize;
        public bool isGrowed { get; private set; }

        public Paddle(Transform paddlePosition, Camera camera)
        {
            this.paddlePosition = paddlePosition;
            nextPaddlePosition = new Vector2(0, paddleY);
            this.camera = camera;
            renderer = paddlePosition.gameObject.GetComponent<SpriteRenderer>();
            standartSize = paddlePosition.localScale;
            CalculatePaddleBorders();
        }

        public void CalculatePaddleBorders()
        {
            var halfPaddleSize = renderer.bounds.size.x / 2;
            leftBorderPositionX = Constants.LEFT_SIDE_POSITION_X + halfPaddleSize;
            RightBorderPositionX = Constants.RIGHT_SIDE_POSITION_X - halfPaddleSize;
        }

        public void IncreasePaddleSize()
        {
            var size = paddlePosition.localScale;
            size.x += Constants.PADDLE_INCREASE_AMOUNT;
            paddlePosition.localScale = size;
            CalculatePaddleBorders();
            isGrowed = true;
        }

        public void DecreasePaddleSize()
        {
            var size = paddlePosition.localScale;
            size.x -= Constants.PADDLE_INCREASE_AMOUNT;
            paddlePosition.localScale = size;
            CalculatePaddleBorders();
            isGrowed = false;
        }

        public void ResetPaddleSize()
        {
            paddlePosition.localScale = standartSize;
            CalculatePaddleBorders();
            isGrowed = false;
        }

        public Vector3 OnModelUpdate(float time)
        {
            float PositionX = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, 0, 0)).x;
            float clampedPosition = Mathf.Clamp(PositionX, leftBorderPositionX, RightBorderPositionX);
            nextPaddlePosition.x = clampedPosition;
            return Vector2.LerpUnclamped(paddlePosition.position, nextPaddlePosition, Speed * time);
        }
    }
}

