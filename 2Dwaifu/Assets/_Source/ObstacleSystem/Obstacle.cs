using UnityEngine;

namespace ObstacleSystem
{
    public class Obstacle : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 4f;
        [SerializeField] private float destroyX = -12f;

        private void Update()
        {
            Move();
            TryDestroy();
        }

        private void Move()
        {
            transform.position += Vector3.left * (moveSpeed * Time.deltaTime);
        }

        private void TryDestroy()
        {
            if (transform.position.x <= destroyX)
            {
                Destroy(gameObject);
            }
        }
    }
}