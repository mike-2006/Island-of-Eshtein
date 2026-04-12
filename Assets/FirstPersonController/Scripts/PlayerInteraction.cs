using TMPro;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactDistance = 2.5f;
    public KeyCode interact_e = KeyCode.E;

    public Camera playerCamera;
    public TextMeshProUGUI hint_text;

    private Check_box currentBox;

}
