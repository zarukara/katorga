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
        
        public int LoadScore(string path = null)
        {
            if (!File.Exists(path))
            {
                return 0;
            }

            string json = File.ReadAllText(path);

            ScoreData data =
                JsonUtility.FromJson<ScoreData>(json);

            return data.score;
        }
    }
}