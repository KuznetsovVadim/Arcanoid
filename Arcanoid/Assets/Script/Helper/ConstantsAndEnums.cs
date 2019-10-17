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
        public const float LEFT_SIDE_POSITION_X = -9.6f;
        public const float RIGHT_SIDE_POSITION_X = 9.6f;
        public const int START_LEVEL = 1;
        public const int MAX_LEVEL = 3;
    }
}

