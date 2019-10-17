using BaseScripts;
using Helper;
using Models;
using System.Collections.Generic;
using UnityEngine;

namespace Controllers 
{
    public class GameController : BaseController
    {
        public GameObject paddle { get; private set; }
        public GameObject ball { get; private set; }

        private GameObject brick;
        private GameObject tempBrick;

        private Vector3 firstBrickPosition;
        private Vector3 paddleStartPosition;
        private Vector3 ballStartPosition;

        private int brickMatrixRow = 11;
        private int brickMatrixColumn = 3;
        private int brickCount;
        private int currentLevel;
        private int maxBonusCount = 4;

        private Dictionary<GameObject, Brick> BrickMatrix;
        private Brick[] allModel;

        public GameController(Vector3 firstBrickPosition, GameObject paddle, GameObject brick, GameObject ball)
        {
            this.brick = brick;
            this.ball = ball;
            this.paddle = paddle;
            BrickMatrix = new Dictionary<GameObject, Brick>();
            brickCount = brickMatrixRow * brickMatrixColumn;
            allModel = new Brick[brickCount];
            this.firstBrickPosition = firstBrickPosition;
            paddleStartPosition = new Vector3(0, Constants.PADDLE_Y_POSITION, 0);
            ballStartPosition = new Vector3(0, Constants.BALL_Y_POSITION, 0);
            currentLevel = Constants.START_LEVEL;
            PlaceBricks();
            PlaceBallAndPaddle();
        }

        private void PlaceBricks()
        {
            Vector3 position = firstBrickPosition;
            Quaternion rotation = new Quaternion();
            int offsetX = 2;
            int offsetY = 1;
            int brickIndex = 0;

            for (int i = 0; i < brickMatrixColumn; i++)
            {
                for (int j = 0; j < brickMatrixRow; j++)
                {
                    tempBrick = GameObject.Instantiate(brick, position, rotation);
                    var brickModel = new Brick(tempBrick, DecreaseBrickCount);
                    allModel[brickIndex] = brickModel;
                    BrickMatrix.Add(tempBrick, brickModel);
                    brickIndex++;
                    position.x += offsetX;
                }
                position.x = firstBrickPosition.x;
                position.y -= offsetY;
            }
        }

        private void PlaceBallAndPaddle()
        {
            Quaternion rotation = new Quaternion();
            paddle = GameObject.Instantiate(paddle, paddleStartPosition, rotation);
            ball = GameObject.Instantiate(ball, ballStartPosition, rotation);
        }

        public void ResetBallAndPaddle()
        {
            paddle.transform.position = paddleStartPosition;
            ball.transform.position = ballStartPosition;
        }

        public void DecreaseBrickCount()
        {
            brickCount--;
            if(brickCount == 0)
            {
                currentLevel = currentLevel < Constants.MAX_LEVEL ? currentLevel++ : Constants.START_LEVEL;
                StartNewLevel(currentLevel);
            }
        }

        public void StartNewLevel(int levelIndex)
        {
            foreach (var brick in BrickMatrix)
            {
                brick.Key.SetActive(true);
                BrickMatrix[brick.Key].ResetBrickStrength((BrickStrength)levelIndex);
            }

            if(levelIndex == 3)
            {
                var currentBonus = maxBonusCount;

                while(currentBonus != 0)
                {
                    var rand = Random.Range(0, brickCount + 1);
                    if (!allModel[rand].hasBonus)
                    {
                        currentBonus--;
                        allModel[rand].hasBonus = true;
                    }
                }
            }
        }
    }
}


