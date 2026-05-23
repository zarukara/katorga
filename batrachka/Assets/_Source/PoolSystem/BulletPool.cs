using System.Collections.Generic;
using WeaponSystem;

namespace PoolSystem
{
    public class BulletPool
    {
        private readonly Bullet.Factory bulletFactory;

        private readonly Queue<Bullet> bullets = new Queue<Bullet>();

        public BulletPool(Bullet.Factory bulletFactory)
        {
            this.bulletFactory = bulletFactory;
        }

        public Bullet Get()
        {
            if (bullets.Count > 0)
            {
                Bullet bullet = bullets.Dequeue();
                bullet.gameObject.SetActive(true);
                return bullet;
            }

            Bullet newBullet = bulletFactory.Create();

            return newBullet;
        }

        public void Release(Bullet bullet)
        {
            bullet.gameObject.SetActive(false);
            bullets.Enqueue(bullet);
        }
    }
}