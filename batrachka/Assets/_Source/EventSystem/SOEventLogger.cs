using UnityEngine;

namespace EventSystem
{
    public class SOEventLogger : MonoBehaviour
    {
        [SerializeField] private string message;

        public void Log()
        {
            Debug.Log("SO-Observer reaction: " + message);
        }
    }
}