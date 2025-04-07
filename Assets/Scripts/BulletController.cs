using UnityEngine;

public class BulletController : MonoBehaviour
{
    public Vector3 direction;
    public float speed;
    public float expireTime;
    private Rigidbody rb;
    void OnEnable() {
        rb = GetComponent<Rigidbody>();
    }
    void Update() {
        transform.LookAt(transform.position + direction);
        rb.MovePosition(transform.position + direction * Time.deltaTime * speed);
        if (Time.time > expireTime) {
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter(Collider other) {
        Debug.Log("Trigger Enter!");
        var enemy = other.GetComponentInParent<EnemyHealth>();
        var door = other.GetComponentInParent<DoorController>();
        if (enemy) {
            enemy.Hit();
            EffectsManager.BulletHit(transform.position, (transform.position - other.transform.position).normalized, true);
            Destroy(gameObject);
        } else if(door) {
            EffectsManager.BulletHit(transform.position, (transform.position - other.transform.position).normalized, true);
            door.Hit();
        }
    }
    void OnCollisionEnter(Collision collision) {
        EffectsManager.BulletHit(collision.contacts[0].point, collision.contacts[0].normal, false);
        Destroy(gameObject);
    }
}
