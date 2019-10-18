using BaseScripts;
using Helper;
using Models;
using System.Collections.Generic;
using UnityEngine;

namespace Controllers
{
    public class BallController: BaseController
    {
        private GameController gameController;

        private List<Ball> ballInGame;

        public BallController(GameController gameController)
        {
            this.gameController = gameController;
            ballInGame = gameController.ballInGame;
        }

        public override void ControllerLateUpdate(float time)
        {
            for (int i = 0; i < ballInGame.Count; i++)
            {
                ballInGame[i].OnModelUpdate(time);
            }
        }
    }
}

