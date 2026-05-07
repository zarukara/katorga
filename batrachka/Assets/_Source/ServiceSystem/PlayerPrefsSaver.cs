using UnityEngine;

namespace ServiceSystem
{
    public class PlayerаPrefsSaver : ISaver
    {
        private const string KEY = "score";

        public void SaveScore(int score, string path = null)
        {
            PlayerPrefs.SetInt(KEY, score);
            PlayerPrefs.Save();
        }
    }
}