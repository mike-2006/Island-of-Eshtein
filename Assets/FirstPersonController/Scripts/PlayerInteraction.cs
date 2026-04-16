using TMPro;
using UnityEngine;

/// <summary>
/// Класс обработки взаимодействия игрока с объектами окружения.
/// Использует Raycast для определения объекта в прицеле и обрабатывает ввод клавиши.
/// </summary>
public class PlayerInteraction : MonoBehaviour
{
    public float interactDistance = 2.5f;
    public KeyCode interact_e = KeyCode.E;

    public Camera playerCamera;
    public TextMeshProUGUI hint_text;

    private Check_box currentBox;
    /// <summary>
    /// Основной цикл обновления. Вызывается каждый кадр.
    /// Проверяет наличие объекта для взаимодействия и нажатие клавиши.
    /// Входные данные: Ввод пользователя (Input.GetKeyDown) и данные от CheckForLootBox.
    /// Выходные данные: Вызов метода OnInteraction() у найденного объекта.
    /// </summary>
    private void Update()
    {
        CheckForLootBox();

        if (currentBox != null && Input.GetKeyDown(interact_e))
        {
            currentBox.OnInteraction();
        }
    }

    /// <summary>
    /// Метод проверки наличия интерактивного объекта в поле зрения камеры.
    /// Пускает луч (Raycast) из центра камеры и ищет компонент Check_box.
    /// Входные данные: Позиция и направление камеры, настройка дистанции.
    /// Выходные данные: Обновление переменной currentBox (ссылка на объект или null),
    /// </summary>
    private void CheckForLootBox()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            Check_box lootbox = hit.collider.GetComponent<Check_box>();
            if (lootbox != null)
            {
                currentBox = lootbox;
                hint_text.gameObject.SetActive(true);
                if (currentBox.isopen)
                {
                    hint_text.text = "Нажми [E] чтобы закрыть";
                }
                else
                {
                    hint_text.text = "Нажми [E] чтобы открыть";
                }
                return;
            }
        }

        currentBox = null; // Забываем ящик
        hint_text.gameObject.SetActive(false);
    }

}
