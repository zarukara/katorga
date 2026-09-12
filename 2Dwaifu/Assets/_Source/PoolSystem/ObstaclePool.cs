using System.Collections.Generic;
using ObstacleSystem;
using UnityEngine;
using Zenject;

namespace PoolSystem
{
    public class ObstaclePool
    {
        private readonly DiContainer _container;
        private readonly Obstacle _prefab;
        private readonly Transform _parent;

        private readonly Queue<Obstacle> _availableObstacles = new();
        private readonly HashSet<Obstacle> _activeObstacles = new();

        public ObstaclePool(
            DiContainer container,
            Obstacle prefab,
            Transform parent,
            int initialSize)
        {
            _container = container;
            _prefab = prefab;
            _parent = parent;

            CreateInitialPool(initialSize);
        }

        public Obstacle Get(Vector3 position)
        {
            Obstacle obstacle = _availableObstacles.Count > 0
                ? _availableObstacles.Dequeue()
                : CreateObstacle();

            obstacle.transform.position = position;
            obstacle.gameObject.SetActive(true);

            _activeObstacles.Add(obstacle);

            return obstacle;
        }

        public void Release(Obstacle obstacle)
        {
            if (obstacle == null)
                return;

            if (!_activeObstacles.Remove(obstacle))
                return;

            obstacle.gameObject.SetActive(false);
            _availableObstacles.Enqueue(obstacle);
        }

        public void ReleaseAll()
        {
            Obstacle[] obstacles = new Obstacle[_activeObstacles.Count];

            _activeObstacles.CopyTo(obstacles);

            foreach (Obstacle obstacle in obstacles)
            {
                Release(obstacle);
            }
        }

        private void CreateInitialPool(int initialSize)
        {
            for (int i = 0; i < initialSize; i++)
            {
                Obstacle obstacle = CreateObstacle();

                obstacle.gameObject.SetActive(false);
                _availableObstacles.Enqueue(obstacle);
            }
        }

        private Obstacle CreateObstacle()
        {
            return _container.InstantiatePrefabForComponent<Obstacle>(
                _prefab,
                _parent
            );
        }
    }
}