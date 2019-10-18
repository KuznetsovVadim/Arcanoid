using Helper;
using System;
using UnityEngine;

namespace Models
{
    public class Bonus
    {
        private Action<Bonus> catchMethod;
        private GameObject bonusView;
        private Vector2 direction;
        private Transform bonusPosition;

        private Vector2[] rayDirections =
        {
            new Vector3(1,0),
            new Vector3(0,-1),
            new Vector3(-1,0),
            new Vector3(1,-1),
            new Vector3(-1,-1),
        };

        private float speed = Constants.BONUS_SPEED;
        private float bonusHalfSize;

        private SpriteRenderer renderer;

        public Bonus(GameObject bonus, Sprite sprite, Action<Bonus> catchMethod)
        {
            bonusView = bonus;
            bonusPosition = bonus.transform;
            this.catchMethod = catchMethod;

            renderer = bonus.GetComponent<SpriteRenderer>();
            if(renderer != null)
            {
                renderer.sprite = sprite;
            }

            bonusHalfSize = renderer.bounds.size.x / 2;
            direction = new Vector2(0, -1);
        }

        public void InActive()
        {
            bonusView.SetActive(false);
        }

        public void SetBonusSettings(Sprite sprite, Action<Bonus> catchMethod, Vector2 postion)
        {
            if (renderer != null)
            {
                renderer.sprite = sprite;
            }
            this.catchMethod = catchMethod;
            bonusPosition.position = postion;
            bonusView.SetActive(true);
        }

        public void OnModelUpdate(float time)
        {
            DrawRays();
            BonusMove(time);
        }

        private void BonusMove(float time)
        {
            Debug.DrawRay(bonusPosition.position, direction.normalized, Color.yellow, 5f);
            bonusPosition.Translate(direction.normalized * (speed * time), Space.World);
        }

        private void DrawRays()
        {
            for (int i = 0; i < rayDirections.Length; i++)
            {
                Debug.DrawRay(bonusPosition.position, rayDirections[i], Color.magenta);

                var rayHit = Physics2D.Raycast(bonusPosition.position, rayDirections[i], bonusHalfSize);

                if (rayHit.collider != null)
                {
                    var collisionObject = rayHit.collider.gameObject;
                    if (collisionObject.name == "Paddle")
                    {
                        bonusView.SetActive(false);
                        catchMethod?.Invoke(this);
                    }
                    return;
                }
            }
        }
    }
}

