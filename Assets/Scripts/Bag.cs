using UnityEngine;

public class Bag : MonoBehaviour
{
    private int count_desk = 0;

    public int Get_Desk()
    {
        return count_desk;
    }

    public void Add_Desk(int amount)
    {
        count_desk += amount;
    }
    
    public void Draw_Desk(int amount)
    {
        count_desk = amount;
    }
}
