using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiDeskText : MonoBehaviour
{
    [SerializeField] private Bag bag;
    [SerializeField] private TextMeshProUGUI text;

    private void Start()
    {
        bag.ChangeCountDesk.AddListener(OnChangeCountDesk);
    }
    private void OnDestroy()
    {
        bag.ChangeCountDesk.RemoveListener(OnChangeCountDesk);
    }

    private void OnChangeCountDesk()
    {
        text.text = bag.Get_Desk().ToString();
    }
}
