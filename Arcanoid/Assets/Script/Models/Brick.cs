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

        private Action brickIsBroken;

        public bool hasBonus;

        public Brick(GameObject brick, Action brickIsBroken)
        {
            brickObject = brick;
            this.brickIsBroken = brickIsBroken;
            brickColors = new Dictionary<int, Color>()
            {
                {1, Color.gray },
                {2, Color.green },
                {3, Color.yellow },
                {4, Color.red },
            };
            brickStrength = 1;
            brickMaterial = brick.GetComponent<SpriteRenderer>().material;
            brickMaterial.color = brickColors[brickStrength];
        }

        public void ResetBrickStrength(BrickStrength strength)
        {
            switch (strength)
            {
                case BrickStrength.OneHit:
                    brickStrength = 1;
                    break;
                case BrickStrength.Random:
                    brickStrength = UnityEngine.Random.Range(1, 5);
                    break;
            }
            brickMaterial.color = brickColors[brickStrength];
        }

        public void GetDamage()
        {
            brickStrength--;
            if(brickStrength >= 1)
            {
                brickMaterial.color = brickColors[brickStrength];
            }
            else
            {
                brickObject.SetActive(false);
                brickIsBroken?.Invoke();
                if (hasBonus)
                {
                    //
                }
            }
        }
    }
}

