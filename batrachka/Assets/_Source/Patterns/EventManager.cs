using System;

public static class EventManager
{
    public static Action<int> OnPlayerDamaged;
    public static Action OnEnemyKilled;

    public static void PlayerDamaged(int damage)
    {
        OnPlayerDamaged?.Invoke(damage);
    }

    public static void EnemyKilled()
    {
        OnEnemyKilled?.Invoke();
    }
}