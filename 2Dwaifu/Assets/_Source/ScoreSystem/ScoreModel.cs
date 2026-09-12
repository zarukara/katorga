using System;

namespace ScoreSystem
{
    public sealed class ScoreModel
    {
        public event Action<int> ScoreChanged;

        public int Score { get; private set; }

        public void AddPoint()
        {
            Score++;
            ScoreChanged?.Invoke(Score);
        }

        public void Reset()
        {
            Score = 0;
            ScoreChanged?.Invoke(Score);
        }
    }
}
