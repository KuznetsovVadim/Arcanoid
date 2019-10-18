using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Helper
{
    public enum BrickStrength
    {
        OneHit = 1,
        Random = 2
    }
    public static class Constants
    {
        public const float PADDLE_Y_POSITION = -4.5f;
        public const float BALL_Y_POSITION = -4f;
        public const float LEFT_SIDE_POSITION_X = -11f;
        public const float RIGHT_SIDE_POSITION_X = 11f;
        public const int START_LEVEL = 1;
        public const int MAX_LEVEL = 3;
        public const float BALL_SPEED = 5f;
        public const float PADDLE_SPEED = 2f;
        public const float BALL_LOST_POSITION = -5f;

        public const string NEW_GAME_MESSAGE = "New Game Start in";
        public const string CONTINUE_GAME_MESSAGE = "Game resum in";
        public const string NEW_LEVEL_MESSAGE = "New Level Start in";
        public const string GAME_OVER_MESSAGE = "Game Over. To start new game Press Restart";
    }
}

