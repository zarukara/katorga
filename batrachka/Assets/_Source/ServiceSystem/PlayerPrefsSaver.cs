using UnityEngine;

namespace ServiceSystem
{
    public class PlayerPrefsSaver : ISaver
    {
        private const string KEY = "score";

        public void SaveScore(int score, string path = null)
        {
            PlayerPrefs.SetInt(KEY, score);

            PlayerPrefs.Save();
        }

        public int LoadScore(string path = null)
        {
            return PlayerPrefs.GetInt(KEY, 0);
        }
    }
}