using UnityEngine;
using UnityEditor;

public class EnableShadows : EditorWindow
{
    [MenuItem("Tools/Включить тени на всех объектах")]
    static void DisableAllShadows()
    {
        // ИСПОЛЬЗУЙ НОВЫЙ МЕТОД:
        MeshRenderer[] renderers = FindObjectsByType<MeshRenderer>(FindObjectsSortMode.None);

        int count = 0;
        foreach (var renderer in renderers)
        {
            // Пропускаем игрока и важные объекты
            if (renderer.CompareTag("Player") ||
                renderer.CompareTag("Enemy"))
                continue;

            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            count++;
        }

        Debug.Log($"? Включил тени на {count} объектах!");
    }
}