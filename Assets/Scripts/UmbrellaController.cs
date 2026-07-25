using UnityEngine;

public class UmbrellaController : MonoBehaviour
{
    public float speed = 8f;
    public float minX = -7f, maxX = 7f;

    void Update()
    {
        float move = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        Vector3 pos = transform.position + new Vector3(move, 0, 0);
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        transform.position = pos;
    }
}
