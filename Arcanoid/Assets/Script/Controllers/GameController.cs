using BaseScripts;
using Helper;
using Models;
using System.Collections.Generic;
using UnityEngine;

namespace Controllers
{
    public class GameController : BaseController
    {
        private GameObject brickPrefab;
        private GameObject ballPrefab;
        private GameObject tempBrick;
        private CoreUI coreUI;

        public GameModel gameModel { get; private set; }
        public GameObject paddle { get; private set; }

        private Vector3 firstBrickPosition;
        private Vector3 paddleStartPosition;

        private int brickMatrixRow = 11;
        private int brickMatrixColumn = 3;
        private int brickCount;
        private int currentLevel;
        private int maxBonusCount = 4;

        private Dictionary<GameObject, Brick> BrickMatrix;
        private Brick[] allModel;

        public List<Ball> ballInGame { get; private set; }
        private Queue<Ball> ballPool;

        public GameController(Vector3 firstBrickPosition, GameObject paddle, GameObject brick, GameObject ball)
        {
            gameModel = new GameModel();
            coreUI = Core.GetCoreUI;
            this.firstBrickPosition = firstBrickPosition;
            this.paddle = paddle;
            brickPrefab = brick;
            ballPrefab = ball;

            BrickMatrix = new Dictionary<GameObject, Brick>();
            brickCount = brickMatrixRow * brickMatrixColumn;
            allModel = new Brick[brickCount];

            ballInGame = new List<Ball>();
            ballPool = new Queue<Ball>();

            paddleStartPosition = new Vector2(0, Constants.PADDLE_Y_POSITION);
            currentLevel = Constants.START_LEVEL;

            CreateBricks();
            CreatePaddle();
            CreateBall();
        }

        #region CreationMethods

        private void CreateBricks()
        {
            Vector2 position = firstBrickPosition;
            Quaternion rotation = new Quaternion();
            int offsetX = 2;
            int offsetY = 1;
            int brickIndex = 0;

            for (int i = 0; i < brickMatrixColumn; i++)
            {
                for (int j = 0; j < brickMatrixRow; j++)
                {
                    tempBrick = GameObject.Instantiate(brickPrefab, position, rotation);
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

        private void CreatePaddle()
        {
            Quaternion rotation = new Quaternion();
            paddle = GameObject.Instantiate(paddle, paddleStartPosition, rotation);
        }

        private void CreateBall()
        {
            Vector2 position = new Vector2(paddle.transform.position.x, Constants.BALL_Y_POSITION);
            Quaternion rotation = new Quaternion();

            var tempBall = GameObject.Instantiate(ballPrefab, position, rotation);
            var ball = new Ball(tempBall, BallWasLost, BallHitTheBrick);
            ballPool.Enqueue(ball);
        }

        #endregion

        #region ResetMethods

        private void ResetGameModel()
        {
            gameModel.ResetModel();
        }

        private void ResetPaddlePosition()
        {
            paddle.transform.position = paddleStartPosition;
        }

        private void ResetBallPosition()
        {
            {
                var ball = ballInGame[0];
                if (ballInGame.Count > 1)
                {
                    for (int i = 1; i < ballInGame.Count; i++)
                    {
                        ballPool.Enqueue(ballInGame[i]);
                    }
                    ballInGame.Clear();
                    ballInGame.Add(ball);
                }
                
                var position = new Vector3(paddle.transform.position.x, Constants.BALL_Y_POSITION, 1);
                ball.ActivateBall(position);
            }
        }

        #endregion

        #region BrickDamageMethods

        public void SetDamageToBrick(GameObject brickView)
        {
            if (BrickMatrix.ContainsKey(brickView))
            {
                BrickMatrix[brickView].GetDamage();
            }
        }

        private void BallHitTheBrick(GameObject brickView)
        {
            SetDamageToBrick(brickView);
        }

        #endregion

        public void GetBallFromPool()
        {
            if (ballPool.Count > 0)
            {
                var ball = ballPool.Dequeue();
                var position = new Vector2(paddle.transform.position.x, Constants.BALL_Y_POSITION);
                ballInGame.Add(ball);
                ball.ActivateBall(position);
            }
            else
            {
                CreateBall();
                GetBallFromPool();
            }
        }

        private void BallWasLost(Ball ball)
        {
            if (ballInGame.Contains(ball))
            {
                ballInGame.Remove(ball);
                ballPool.Enqueue(ball);

                if (ballInGame.Count < 1)
                {
                    LifeIsLost();
                }
            }
        }

        public void ContinueLevel()
        {
            ResetPaddlePosition();
            GetBallFromPool();
        }

        public void StartNewGame()
        {
            ResetGameModel();
            ResetPaddlePosition();
            StartNewLevel(1);
            GetBallFromPool();
        }

        public void StartNextLevel()
        {
            ResetPaddlePosition();
            ResetBallPosition();
        }

        public void LifeIsLost()
        {
            gameModel.PlayerLife--;
            if(gameModel.PlayerLife > 0)
            {
                coreUI.CotinueLevel();
            }
            else
            {
                coreUI.RestartGame();
            }
        }

        public void DecreaseBrickCount()
        {
            brickCount--;
            gameModel.PlayerScore += 100;

            if (brickCount == 0)
            {
                currentLevel = currentLevel < Constants.MAX_LEVEL ? currentLevel += 1 : Constants.START_LEVEL;
                coreUI.StartNextLevel();
                gameModel.PlayerLevel = currentLevel;
                StartNextLevel();
                StartNewLevel(currentLevel);
            }
        }

        public void StartNewLevel(int levelIndex)
        {
            brickCount = brickMatrixRow * brickMatrixColumn;
            foreach (var brick in BrickMatrix)
            {
                brick.Key.SetActive(true);
                BrickMatrix[brick.Key].ResetBrickStrength((BrickStrength)levelIndex);
            }

            if(levelIndex == 3)
            {
                var currentBonus = maxBonusCount;
                return;
                while(currentBonus != 0)
                {
                    var rand = UnityEngine.Random.Range(0, brickCount + 1);
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


