namespace Helper
{
    public enum BrickStrength
    {
        OneHit = 1,
        Random = 2
    }

    public enum BonusType
    {
        ExtraBall = 0,
        Grow = 1,
        Fast = 2,
        SLow = 3
    }
    public static class Constants
    {
        public const float PADDLE_Y_POSITION = -4.5f;
        public const float BALL_Y_POSITION = -4f;
        public const float LEFT_SIDE_POSITION_X = -11f;
        public const float RIGHT_SIDE_POSITION_X = 11f;
        public const int START_LEVEL = 1;
        public const int MAX_LEVEL = 3;
        public const float BALL_SPEED = 3f;
        public const float BONUS_SPEED = 2.5f;
        public const float PADDLE_SPEED = 4f;
        public const float BALL_LOST_POSITION = -5f;
        public const int BONUS_TIME = 10;
        public const int COUNT_DOWN_DURATION = 3;
        public const int BONUS_COUNT_PER_LEVEL = 10;
        public const float PADDLE_INCREASE_AMOUNT = 0.25f;
        public const int SCORE_PER_BROKEN_BRICK = 100;

        public const string NEW_GAME_MESSAGE = "New Game Start in";
        public const string CONTINUE_GAME_MESSAGE = "Game resum in";
        public const string NEW_LEVEL_MESSAGE = "New Level Start in";
        public const string GAME_OVER_MESSAGE = "Game Over. To start new game Press Restart";
        public const string BRICK_NAME = "Brick";
        public const string PADDLE_NAME = "Paddle";
    }
}

