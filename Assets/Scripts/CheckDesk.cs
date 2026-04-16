using SimpleFPS;
using System.Linq;
using UnityEngine;

/// <summary>
/// Класс обработки события постройки корабля.
/// Активируется при входе триггера, проверяет наличие ресурсов в инвентаре (Bag)
/// и изменяет материал объектов, делая их непрозрачными.
/// </summary>
public class CheckDesk : MonoBehaviour
{
    [SerializeField] private Bag bag;
    [SerializeField] private Renderer[] material;

   
    private int i = 0;
    /// <summary>
    /// Обработчик события входа другого коллайдера в триггер этого объекта.
    /// Проверяет, является ли вошедший объектом игрока, и пытается подобрать предмет.
    /// </summary>
    /// Входные данные: Collider other (объект-триггер).
    /// Выходные данные: 
    /// <summary>
    /// 1. Уменьшение количества предметов в Bag.
    /// 2. Активация BoxCollider у следующего объекта в списке.
    /// 3. Изменение материала объекта на непрозрачный (побочный эффект).
    /// 4. Инкремент счетчика i.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        FirstPersonController fps = other.GetComponent<FirstPersonController>();
        Debug.Log(bag.Get_Desk() > 0);
        if (fps != null && bag.Get_Desk() > 0 && i < material.Length)
        {
            bag.Remove_Desk(1);

           material[i].gameObject.GetComponent<BoxCollider>().enabled = true;
           SetMaterialOpaque(material[i]);

            Debug.Log("Изменил материал");
            i ++;

        }
    }
    /// <summary>
    /// Метод настройки материала рендерера для имитации непрозрачности.
    /// Изменяет параметры шейдера (Surface, Blend modes, ZWrite) для корректного отображения.
    /// Входные данные: Renderer renderer (объект для модификации).
    /// Выходные данные: Измененный материал объекта (побочный эффект).
    /// Параметры шейдера устанавливаются в режим opaque (непрозрачный).
    /// </summary>
    private void SetMaterialOpaque(Renderer renderer)
    {
        Material mat = renderer.material;

        mat.SetFloat("_Surface", 0f);

        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        mat.SetInt("_ZWrite", 1);
        mat.renderQueue = 2000;

        renderer.material = mat;
    }
}
