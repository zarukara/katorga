using System.Collections.Generic;
using UnityEngine;
using WeaponSystem;
using Zenject;

namespace PoolSystem
{
    public class BulletPool
    {
        private readonly DiContainer container;
        private readonly Bullet bulletPrefab;
        private readonly Transform poolParent;

        private readonly Queue<Bullet> bullets = new Queue<Bullet>();

        public BulletPool(
            DiContainer container,
            Bullet bulletPrefab,
            Transform poolParent)
        {
            this.container = container;
            this.bulletPrefab = bulletPrefab;
            this.poolParent = poolParent;
        }

        public Bullet Get()
        {
            if (bullets.Count > 0)
            {
                Bullet bullet = bullets.Dequeue();
                bullet.gameObject.SetActive(true);
                return bullet;
            }

            Bullet newBullet = container.InstantiatePrefabForComponent<Bullet>(
                bulletPrefab,
                poolParent);

            return newBullet;
        }

        public void Release(Bullet bullet)
        {
            bullet.gameObject.SetActive(false);
            bullets.Enqueue(bullet);
        }
    }
}