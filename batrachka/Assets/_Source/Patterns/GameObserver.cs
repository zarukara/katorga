using UnityEngine;

public class GameObserver : MonoBehaviour
{
    private void OnEnable()
    {
        EventManager.OnPlayerDamaged += OnPlayerDamaged;
        EventManager.OnEnemyKilled += OnEnemyKilled;
    }

    private void OnDisable()
    {
        EventManager.OnPlayerDamaged -= OnPlayerDamaged;
        EventManager.OnEnemyKilled -= OnEnemyKilled;
    }

    private void OnPlayerDamaged(int damage)
    {
        Debug.Log("Урон по игроку: " + damage);
    }

    private void OnEnemyKilled()
    {
        Debug.Log("+фраг");
    }
}