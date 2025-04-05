using UnityEngine;

public class ForwardMovement : MonoBehaviour
{
    public float speed;
    void Update() {
        transform.position += Vector3.forward * Time.deltaTime * speed;
    }
}
