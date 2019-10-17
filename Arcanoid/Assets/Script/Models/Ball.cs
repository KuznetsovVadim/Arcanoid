using System.Collections.Generic;
using UnityEngine;

namespace Models
{
    public class Ball
    {
        private Vector3 direction;
        private Vector3[] rayDirections =
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
        private List<Vector3> currentRayDirections;
        private Transform ballPosition;
        private Ray ray;

        private void Awake()
        {
            direction = new Vector3(0, -1); //RandomDirection();
            currentRayDirections = new List<Vector3>();
            //ballPosition = transform;
            ray = new Ray();
            CalculateRayDirections();
        }

        private Vector3 GetRandomDirection()
        {
            int x = 0, y = 0;

            while (x == 0 && y == 0)
            {
                x = Random.Range(-1, 1);
                y = Random.Range(-1, 1);
            }

            return new Vector3(x, y);
        }

        void FixedUpdate()
        {
            BallMove();
            DrawRays();
        }

        private void CalculateRayDirections()
        {
            currentRayDirections.Clear();

            for (int i = 0; i < rayDirections.Length; i++)
            {
                if (Vector3.Dot(rayDirections[i], direction) > 0)
                {
                    currentRayDirections.Add(Vector3.ClampMagnitude(rayDirections[i], 0.375f));
                }
            }
        }

        private void DrawRays()
        {
            RaycastHit rayHit;
            for (int i = 0; i < currentRayDirections.Count; i++)
            {
                Debug.DrawRay(ballPosition.position, currentRayDirections[i], Color.red);

                ray.origin = ballPosition.position;
                ray.direction = currentRayDirections[i];

                if (Physics.Raycast(ray, out rayHit, 0.375f))
                {
                    var collisionObject = rayHit.collider.gameObject;
                    Vector3 newDirection = new Vector3();
                    if (collisionObject.GetComponent<Paddle>() != null)
                    {
                        var center = collisionObject.transform.position.x;
                        var distance = rayHit.point.x - center;
                        newDirection = new Vector3(distance,  1);
                    }
                    else
                    {
                        newDirection = Vector3.Reflect(direction, rayHit.normal);
                        var randomRange = Random.Range(-0.25f, 0.25f);
                        newDirection.x += randomRange;
                        newDirection.y += randomRange;
                    }
                    direction = Vector3.ClampMagnitude(newDirection, 1f);
                    CalculateRayDirections();
                    return;
                }
            }
        }

        private void BallMove()
        {
            Debug.DrawRay(ballPosition.position, direction.normalized, Color.green, 5f);
            //transform.Translate(direction.normalized * (15f * Time.deltaTime), Space.World);
        }
    }
}

