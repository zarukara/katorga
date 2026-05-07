using System.IO;
using UISystem;
using UnityEngine;

namespace ServiceSystem
{
    public class JsonSaver : ISaver
    {
        public void SaveScore(int score, string path = null)
        {
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogError("Path is null");
                return;
            }

            ScoreData data = new ScoreData();
            data.score = score;

            string json = JsonUtility.ToJson(data, true);

            File.WriteAllText(path, json);
        }
    }
}