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
    float sphereRadius = 0.1f;

    public Camera playerCamera;
    public TextMeshProUGUI hint_text;

    private Check_box currentBox;
    private Key currentKey;
    private RotateTo currentDoor;

    private bool hasKey = false;
    private bool doorOpen = false;
    /// <summary>
    /// Основной цикл обновления. Вызывается каждый кадр.
    /// Проверяет наличие объекта для взаимодействия и нажатие клавиши.
    /// Входные данные: Ввод пользователя (Input.GetKeyDown) и данные от CheckForLootBox.
    /// Выходные данные: Вызов метода OnInteraction() у найденного объекта.
    /// </summary>
    private void Update()
    {
        CheckForInteractable();
        if (currentBox != null && Input.GetKeyDown(interact_e))
        {
            currentBox.OnInteraction();
            
        }
        else if (Input.GetKeyDown(interact_e) && currentKey != null)
        {
            currentKey.Pickup();
            hasKey = true;
        }
        else if (currentDoor != null && Input.GetKeyDown(interact_e) && hasKey)
        {
            currentDoor.enabled = true;
            doorOpen = true;
        }

    }

    /// <summary>
    /// Метод проверки наличия интерактивного объекта в поле зрения камеры.
    /// Пускает луч (Raycast) из центра камеры и ищет компонент Check_box.
    /// Входные данные: Позиция и направление камеры, настройка дистанции.
    /// Выходные данные: Обновление переменной currentBox (ссылка на объект или null),
    /// </summary>
    private void CheckForInteractable()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        currentBox = null;
        currentKey = null;
        currentDoor = null;

        if (Physics.SphereCast(ray, sphereRadius, out hit, interactDistance))
        {
            Check_box lootbox = hit.collider.GetComponent<Check_box>();
            if (lootbox != null)
            {
                currentBox = lootbox;
                hint_text.gameObject.SetActive(true);
                hint_text.text = currentBox.isopen ? "Нажми [E] чтобы закрыть" : "Нажми [E] чтобы открыть";
                return;
            }

            Key key = hit.collider.GetComponent<Key>();
            if (key != null)
            {
                currentKey = key;

                hint_text.gameObject.SetActive(true);
                hint_text.text = "Нажми [E] чтобы взять ключ";


                return;
            }

            RotateTo door = hit.collider.GetComponent<RotateTo>();
            if (door != null)
            {
                currentDoor = door;

                
                if (hasKey == false)
                {
                    hint_text.gameObject.SetActive(true);
                    hint_text.text = "Дверь заперта, возможно нужен ключ чтобы открыть её";
                }
                else if (doorOpen == false && hasKey == true)
                {
                    hint_text.gameObject.SetActive(true);
                    hint_text.text = "Нажми [E] чтобы открыть дверь";
                }

                return;
            }

        }

        
        hint_text.gameObject.SetActive(false);
    }

}
