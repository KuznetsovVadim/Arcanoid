using System;

namespace Models
{
    public class GameModel
    {
        private int life;
        private int score;
        private int level;
        public Action modelHasChanged;

        public int PlayerLife
        {
            get
            {
                return life;
            }
            set
            {
                life = value;
                modelHasChanged?.Invoke();
            }
        }

        public int PlayerScore
        {
            get
            {
                return score;
            }
            set
            {
                score = value;
                modelHasChanged?.Invoke();
            }
        }

        public int PlayerLevel
        {
            get
            {
                return level;
            }
            set
            {
                level = value;
                modelHasChanged?.Invoke();
            }
        }

        public GameModel()
        {
            life = 3;
            score = 0;
            level = 1;
        }

        public void ResetModel()
        {
            life = 3;
            score = 0;
            level = 1;
        }
    }
}

