using UnityEngine;
using System.Collections.Generic;

public class DistanceManager : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float activationDistance = 50f;
    [SerializeField] private float checkInterval = 0.5f; // Проверяем раз в 0.5 сек

    [SerializeField] private LayerMask excludeLayer;

    private List<Renderer> _allRenderers = new List<Renderer>();
    private float _nextCheckTime = 0f;

    
    void Start()
    {

        // Находим ВСЕ рендереры ОДИН РАЗ
        Renderer[] all = FindObjectsByType<Renderer>(FindObjectsSortMode.None);

        foreach (var rend in all)
        {
            // ✅ ПРОПУСКАЕМ объекты на исключаемом слое
            if (((1 << rend.gameObject.layer) & excludeLayer.value) != 0)
                continue;

            // ✅ Также пропускаем игрока и важные объекты
            if (rend.CompareTag("Player") || rend.CompareTag("Important"))
                continue;

            _allRenderers.Add(rend);
        }
    }

    void Update()
    {
        if (Time.time < _nextCheckTime) return;

        _nextCheckTime = Time.time + checkInterval;

        foreach (var rend in _allRenderers)
        {
            if (rend == null) continue;

            float distance = Vector3.Distance(rend.transform.position, player.position);

            bool shouldBeActive = distance <= activationDistance;
            rend.enabled = shouldBeActive;
        }
    }
}