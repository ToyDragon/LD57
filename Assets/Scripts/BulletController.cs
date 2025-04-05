using UnityEngine;

public class BulletController : MonoBehaviour
{
    public Vector3 direction;
    public float speed;
    public float expireTime;
    void Update()
    {
        transform.LookAt(transform.position + direction);
        transform.position += direction * Time.deltaTime * speed;
        if (Time.time > expireTime) {
            Destroy(gameObject);
        }
    }
}
