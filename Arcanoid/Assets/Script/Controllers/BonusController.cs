using BaseScripts;
using Helper;
using Models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Controllers
{
    public class BonusController: BaseController
    {
        private PaddleController paddleController;
        private GameController gameController;
        private BallController ballController;

        private GameObject bonusPrefab;
        private Dictionary<int, Sprite> spriteDict;
        private Dictionary<int, Action<Bonus>> bonusActions;

        private List<Bonus> currentBonuses;
        private Queue<Bonus> poolBonuses;

        private CoreUI coreUI;

        public BonusController(Sprite[] bonusSprites, GameObject bonusPrefab)
        {
            spriteDict = new Dictionary<int, Sprite>()
            {
                {0, bonusSprites[0]},
                {1, bonusSprites[1]},
                {2, bonusSprites[2]},
                {3, bonusSprites[3]},
            };

            bonusActions = new Dictionary<int, Action<Bonus>>()
            {
                {0, ExtraBall},
                {1, IncreasePaddleLenght},
                {2, SpeedUpBalls},
                {3, SLowDownBalls},
            };

            this.bonusPrefab = bonusPrefab;
            currentBonuses = new List<Bonus>();
            poolBonuses = new Queue<Bonus>();
            coreUI = Core.GetCoreUI;

            var core = Core.GetCore;
            paddleController = core.GetController<PaddleController>();
            gameController = core.GetController<GameController>();
            ballController = core.GetController<BallController>();
        }

        public void CreateBonus(Vector2 postion)
        {
            if(poolBonuses.Count > 0)
            {
                var poolBonus = poolBonuses.Dequeue();
                var randomBonus = UnityEngine.Random.Range(0, 4);
                var sprite = spriteDict[randomBonus];
                var action = bonusActions[randomBonus];
                poolBonus.SetBonusSettings(sprite, action, postion);
                currentBonuses.Add(poolBonus);
            }
            else
            {
                Vector2 position = postion;
                Quaternion rotation = new Quaternion();

                var tempBonus = GameObject.Instantiate(bonusPrefab, position, rotation);
                var randomBonus = UnityEngine.Random.Range(0, 4);
                var sprite = spriteDict[randomBonus];
                var action = bonusActions[randomBonus];
                var bonus = new Bonus(tempBonus, sprite, action);
                currentBonuses.Add(bonus);
            }
        }

        public override void ControllerLateUpdate(float time)
        {
            for (int i = 0; i < currentBonuses.Count; i++)
            {
                currentBonuses[i].OnModelUpdate(time);
            }
        }

        public void ResetBunuses()
        {
            if(currentBonuses.Count > 0)
            {
                for (int i = 0; i < currentBonuses.Count; i++)
                {
                    currentBonuses[i].InActive();
                    poolBonuses.Enqueue(currentBonuses[i]);
                }
                currentBonuses.Clear();
            }
        }

        #region BonusMethods

        private void SpeedUpBalls(Bonus bonus)
        {
            if (ballController != null && !ballController.isSpeedUp)
            {
                ballController.IncreaseBallSpeed();

                if (currentBonuses.Contains(bonus))
                {
                    currentBonuses.Remove(bonus);
                    poolBonuses.Enqueue(bonus);
                }

                coreUI.StartBonusTimer(Constants.BONUS_TIME, () =>
                {
                    if (ballController.isSpeedUp)
                    {
                        ballController.DecreaseBallSpeed();
                    }
                });
            }
        }

        private void SLowDownBalls(Bonus bonus)
        {
            if (ballController != null && !ballController.isSlowedDown)
            {
                ballController.DecreaseBallSpeed();

                if (currentBonuses.Contains(bonus))
                {
                    currentBonuses.Remove(bonus);
                    poolBonuses.Enqueue(bonus);
                }

                coreUI.StartBonusTimer(Constants.BONUS_TIME, () =>
                {
                    if (ballController.isSlowedDown)
                    {
                        ballController.IncreaseBallSpeed();
                    }
                });
            }
        }

        private void ExtraBall(Bonus bonus)
        {
            gameController.GetBallFromPool();

            if (currentBonuses.Contains(bonus))
            {
                currentBonuses.Remove(bonus);
                poolBonuses.Enqueue(bonus);
            }
        }

        private void IncreasePaddleLenght(Bonus bonus)
        {
            if(paddleController != null)
            {
                var paddle = paddleController.paddleModel;
                if (paddle.isGrowed)
                {
                    return;
                }

                paddle.IncreasePaddleSize();

                if (currentBonuses.Contains(bonus))
                {
                    currentBonuses.Remove(bonus);
                    poolBonuses.Enqueue(bonus);
                }

                coreUI.StartBonusTimer(Constants.BONUS_TIME, () =>
                {
                    paddle.DecreasePaddleSize();
                });
            }
        }

        #endregion
    }
}
