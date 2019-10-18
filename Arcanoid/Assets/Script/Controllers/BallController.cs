using BaseScripts;
using Helper;
using Models;
using System.Collections.Generic;

namespace Controllers
{
    public class BallController : BaseController
    {
        public List<Ball> ballInGame { get; private set; }
        public float BallSpeed { get; private set; } = Constants.BALL_SPEED;
        public bool isSpeedUp { get; private set; }
        public bool isSlowedDown { get; private set; }

        public BallController(GameController gameController)
        {
            ballInGame = gameController.ballInGame;
        }

        public void ResetBallSpeed()
        {
            BallSpeed = Constants.BALL_SPEED;
            ChangeBallsSpeed();
        }

        public void IncreaseBallSpeed()
        {
            isSpeedUp = isSlowedDown ? false : true;
            isSlowedDown = false;
            BallSpeed *= 1.5f;
            ChangeBallsSpeed();
        }

        public void DecreaseBallSpeed()
        {
            isSlowedDown = isSpeedUp ? false : true;
            isSpeedUp = false;
            BallSpeed /= 1.5f;
            ChangeBallsSpeed();
        }

        private void ChangeBallsSpeed()
        {
            for (int i = 0; i < ballInGame.Count; i++)
            {
                ballInGame[i].speed = BallSpeed;
            }
        }

        public override void ControllerUpdate(float time)
        {
            for (int i = 0; i < ballInGame.Count; i++)
            {
                ballInGame[i].OnModelUpdate(time);
            }
        }
    }
}

