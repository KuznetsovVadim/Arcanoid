using Helper;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Models
{
    public class Ball
    {
        private Vector2 direction;
        private Vector2[] rayDirections =
        {
            new Vector3(0,1),
            new Vector3(1,0),
            new Vector3(0,-1),
            new Vector3(-1,0),
            new Vector3(1,1),
            new Vector3(1,-1),
            new Vector3(-1,-1),
            new Vector3(-1,1),
        };
        private List<Vector2> currentRayDirections;
        private Transform ballPosition;
        private GameObject ballView;
        public float speed = Constants.BALL_SPEED;
        private float ballHalfSize;
        private bool isBallLost;
        private Vector2 newDirection = Vector2.zero;
        private Action<Ball> onBallLose;
        private Action<GameObject> onBallHitBrick;

        public Ball(GameObject ball, Action<Ball> onBallLose, Action<GameObject> onBallHitBrick)
        {
            ballView = ball;
            this.onBallLose = onBallLose;
            this.onBallHitBrick = onBallHitBrick;
            ballPosition = ballView.transform;
            currentRayDirections = new List<Vector2>();
            ballHalfSize = ball.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        }

        public void ActivateBall(Vector3 newBallPosition)
        {
            isBallLost = false;
            ballPosition.position = newBallPosition;
            ballView.SetActive(true);
            direction = GetRandomDirection();
            CalculateRayDirections();
        }

        private Vector2 GetRandomDirection()
        {
            int x = 0;

            while (x == 0)
            {
                x = UnityEngine.Random.Range(-1, 1);
            }

            return new Vector2(x, 1);
        }

        public void OnModelUpdate(float time)
        {
            CheckBallPosition();
            if (!isBallLost)
            {
                DrawRays();
                BallMove(time);
            }
        }

        private void CalculateRayDirections()
        {
            currentRayDirections.Clear();

            for (int i = 0; i < rayDirections.Length; i++)
            {
                if (Vector3.Dot(rayDirections[i], direction) > 0)
                {
                    currentRayDirections.Add(Vector3.ClampMagnitude(rayDirections[i], ballHalfSize));
                }
            }
        }

        private void CheckBallPosition()
        {
            if (ballPosition.position.y <= Constants.BALL_LOST_POSITION)
            {
                isBallLost = true;
                ballView.SetActive(false);
                onBallLose?.Invoke(this);
            }
        }

        public void InActiveObject()
        {
            ballView.SetActive(false);
        }

        private void DrawRays()
        {
            for (int i = 0; i < currentRayDirections.Count; i++)
            {
                var rayHit = Physics2D.Raycast(ballPosition.position, currentRayDirections[i], ballHalfSize);

                if (rayHit.collider != null)
                {
                    var collisionObject = rayHit.collider.gameObject;
                    if (collisionObject.name == "Paddle")
                    {
                        var center = collisionObject.transform.position.x;
                        var distance = rayHit.point.x - center;
                        newDirection = new Vector2(distance,  1);
                    }
                    else
                    {
                        newDirection = Vector2.Reflect(direction, rayHit.normal);
                        var randomRange = UnityEngine.Random.Range(-0.1f, 0.1f);
                        newDirection.x += randomRange;
                        newDirection.y += randomRange;
                        if (collisionObject.name == "Brick")
                        {
                            onBallHitBrick?.Invoke(collisionObject);
                        }
                    }
                    direction = Vector2.ClampMagnitude(newDirection, 1f);
                    CalculateRayDirections();
                    return;
                }
            }
        }

        private void BallMove(float time)
        {
            ballPosition.Translate(direction.normalized * speed * time, Space.World);
        }
    }
}

