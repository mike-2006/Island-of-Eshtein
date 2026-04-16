using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
/// <summary>
/// Класс управления перемещением NPC с использованием NavMeshAgent.
/// Реализует логику преследования игрока с периодическим пересчетом пути
/// </summary>
public class Agent_Move : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float updateInterval = 0.5f;

    private NavMeshAgent agent;
    private float _nextUpdateTime;
    /// <summary>
    /// Инициализация компонента при старте игры.
    /// Получает ссылку на NavMeshAgent и устанавливает начальную цель.
    /// Входные данные: Отсутствуют (используются сериализуемые поля).
    /// Выходные данные: Инициализированный агент с установленным маршрутом.
    /// </summary>
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(player.position);
        _nextUpdateTime = Time.time + updateInterval;
    }
    /// <summary>
    /// Основной цикл обновления логики движения. Вызывается каждый кадр.
    /// Проверяет истечение интервала и обновляет destination агента.
    /// Входные данные: Текущее состояние сцены и позиция игрока.
    /// Выходные данные: Обновленный маршрут агента (побочный эффект).
    /// </summary>
    void Update()
    {
        // Обновляем цель с интервалом
        if (Time.time >= _nextUpdateTime)
        {
            agent.SetDestination(player.position);
            _nextUpdateTime = Time.time + updateInterval;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (agent == null) return;

        // Рисуем весь путь
        Vector3[] pathCorners = agent.path.corners;

        for (int i = 0; i < pathCorners.Length - 1; i++)
        {
            // Линия пути
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(pathCorners[i], pathCorners[i + 1]);

            // Точки (углы)
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(pathCorners[i], 0.3f);
        }

        // Последняя точка
        if (pathCorners.Length > 0)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(pathCorners[pathCorners.Length - 1], 0.5f);
        }

        // Текущая цель
        if (player != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(player.position, 1f);
        }
    }
}
