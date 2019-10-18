using Helper;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Models
{
    public class Brick
    {
        private GameObject brickObject;
        private int brickStrength;

        private Dictionary<int, Color> brickColors;
        private Material brickMaterial;

        private Action<Brick> brickIsBroken;
        public Transform brickPosition { get; private set; }

        public bool hasBonus;

        public Brick(GameObject brick, Action<Brick> brickIsBroken)
        {
            brickPosition = brick.transform;
            brickObject = brick;
            this.brickIsBroken = brickIsBroken;
            brickColors = new Dictionary<int, Color>()
            {
                {0, Color.gray },
                {1, Color.green },
                {2, Color.yellow },
                {3, Color.red },
            };
            brickStrength = 0;
            brickMaterial = brick.GetComponent<SpriteRenderer>().material;
            brickMaterial.color = brickColors[brickStrength];
        }

        public void ResetBrickStrength(BrickStrength strength)
        {
            switch (strength)
            {
                case BrickStrength.OneHit:
                    brickStrength = 0;
                    break;
                default:
                    brickStrength = UnityEngine.Random.Range(0, 4);
                    break;
            }
            
            if(brickColors.ContainsKey(brickStrength))
            {
                brickMaterial.color = brickColors[brickStrength];
            }
        }

        public void GetDamage()
        {
            brickStrength--;
            if(brickStrength >= 0)
            {
                brickMaterial.color = brickColors[brickStrength];
            }
            else
            {
                brickObject.SetActive(false);

                brickIsBroken?.Invoke(this);
            }
        }
    }
}

