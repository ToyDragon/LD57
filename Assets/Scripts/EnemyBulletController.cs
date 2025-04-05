using System;
using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{
    public Vector2 screenDestination;
    public Vector3 sourcePos;
    public float speed;
    public float arcRadius;
    public float scale = 1;
    void Update() {
        var screenPos = ShipMovement.GetScreenPos(screenDestination);        
        var del = screenPos - transform.position;
        transform.localScale = Vector3.one * (5 - 4*Mathf.Clamp01(del.magnitude/40f));
        transform.LookAt(screenPos);
        if (del.magnitude < 1) {
            PlayerDamage.instance.BulletHit(transform.position, scale);
            Destroy(gameObject);
        } else {
            transform.position += del.normalized * Mathf.Clamp(speed * Time.deltaTime, 0, del.magnitude);
        }
    }
}
