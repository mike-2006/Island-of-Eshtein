using TMPro;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactDistance = 2.5f;
    public KeyCode interact_e = KeyCode.E;

    public Camera playerCamera;
    public TextMeshProUGUI hint_text;

    private Check_box currentBox;

    private void Update()
    {
        CheckForLootBox();

        if (currentBox != null && Input.GetKeyDown(interact_e))
        {
            currentBox.OnInteraction();
        }
    }
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
