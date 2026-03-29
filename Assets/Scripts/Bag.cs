using UnityEngine;
using UnityEngine.Events;

public class Bag : MonoBehaviour
{
    public UnityEvent ChangeCountDesk;
    private int count_desk = 0;


    public int Get_Desk()
    {
        return count_desk;
    }

    public void Add_Desk(int amount)
    {
        count_desk += amount;
        ChangeCountDesk.Invoke();
    }
    
    public void Draw_Desk(int amount)
    {
        count_desk = amount;
        ChangeCountDesk.Invoke();
    }
}
