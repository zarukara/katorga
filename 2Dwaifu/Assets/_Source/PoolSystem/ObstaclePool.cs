using System;
using ObstacleSystem;
using UnityEngine;

namespace PoolSystem
{
    public sealed class ObstaclePool
    {
        private readonly Func<Obstacle> _createObstacle;
        private readonly ObjectPool<Obstacle> _pool;

        public ObstaclePool(Func<Obstacle> createObstacle, int initialSize)
        {
            _createObstacle = createObstacle ?? throw new ArgumentNullException(nameof(createObstacle));
            _pool = new ObjectPool<Obstacle>(CreateObstacle, obstacle => obstacle.Deactivate(), initialSize);
        }

        public Obstacle Get(Vector3 position)
        {
            Obstacle obstacle = _pool.Get();
            obstacle.Activate(position);
            return obstacle;
        }

        public bool Release(Obstacle obstacle) => _pool.Release(obstacle);

        public void ReleaseAll() => _pool.ReleaseAll();

        private Obstacle CreateObstacle()
        {
            Obstacle obstacle = _createObstacle();
            if (obstacle == null)
                throw new InvalidOperationException("The obstacle factory returned no obstacle.");

            obstacle.DespawnRequested += OnDespawnRequested;
            return obstacle;
        }

        private void OnDespawnRequested(Obstacle obstacle) => _pool.Release(obstacle);
    }
}
