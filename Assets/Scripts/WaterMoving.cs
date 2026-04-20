using UnityEngine;

public class WaterMoving : MonoBehaviour
{
    public float move_speed = 0.3f;

    private Renderer rend;

    private float offset_Y;
    private float offset_X;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        offset_X = Time.time * move_speed;
        offset_Y = Time.time * move_speed;
        rend.material.SetTextureOffset("_BaseMap", new Vector2(offset_X, offset_Y));
    }
}
