using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    public static PlayerDamage instance;
    public float damage;
    public List<AudioSource> hurtSounds;
    public float radius = 3;
    private float lastCollisionHit = -100;
    [HideInInspector]
    public float lastHitTime = -100;
    public LayerMask collisionMask;
    void OnEnable() {
        instance = this;
    }
    void Update() {
        damage = Mathf.Max(0, damage - Time.deltaTime);
    }
    public void BulletHit(Vector3 pos, float scale) {
        var del = transform.position - pos;
        del = new Vector3(del.x, del.y, 0);
        if (del.magnitude < radius + scale) {
            TakeHit();
        }
    }
    private void TakeHit() {
        lastHitTime = Time.time;
        damage += .5f + 1.5f*Mathf.Clamp01(1 - damage/4);
        foreach (var sound in hurtSounds) {
            sound.Play();
        }
    }
    void OnTriggerEnter(Collider other) {
        if ((collisionMask & (1 << other.gameObject.layer)) == 0) {
            return;
        }
        if (Time.time - lastCollisionHit > .5f) {
            lastCollisionHit = Time.time;
            TakeHit();
        }
    }
}
