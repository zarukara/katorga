using UISystem;
using UnityEngine;

namespace ServiceSystem
{
    public class PlayerPrefsSaver : ISaver
    {
        private const string KEY = "score";

        private readonly Score score;

        public PlayerPrefsSaver(Score score)
        {
            this.score = score;
        }

        public void SaveScore(string path = null)
        {
            PlayerPrefs.SetInt(KEY, score.Value);

            PlayerPrefs.Save();
        }

        public int LoadScore(string path = null)
        {
            return PlayerPrefs.GetInt(KEY, 0);
        }
    }
}
