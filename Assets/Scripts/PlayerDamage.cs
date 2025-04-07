using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    public static PlayerDamage instance;
    public float damage;
    public float boost;
    public List<AudioSource> hurtSounds;
    public float radius = 3;
    private float lastCollisionHit = -100;
    [HideInInspector]
    public float lastHitTime = -100;
    public LayerMask collisionMask;
    public LayerMask ringMask;
    public int hitsTakenThisLevel = 0;
    public AudioSource ringChimeSource;
    void OnEnable() {
        instance = this;
    }
    void Update() {
        damage = Mathf.Max(0, damage - Time.deltaTime);
        boost = Mathf.Max(0, boost - Time.deltaTime*2);
    }
    public void BulletHit(Vector3 pos, float scale) {
        var del = transform.position - pos;
        del = new Vector3(del.x, del.y, 0);
        if (del.magnitude < radius + scale) {
            TakeHit();
        }
    }
    private void TakeHit() {
        hitsTakenThisLevel++;
        lastHitTime = Time.time;
        damage += .5f + 1.5f*Mathf.Clamp01(1 - damage/4);
        foreach (var sound in hurtSounds) {
            sound.Play();
        }
    }
    void OnTriggerEnter(Collider other) {
        if ((ringMask & (1 << other.gameObject.layer)) != 0) {
            boost = 8f;
            if (other.gameObject.TryGetComponent<RingBehavior>(out var rb)) {
                rb.collectedTime = Time.time;
                ringChimeSource.Play();
            }
        }
        if ((collisionMask & (1 << other.gameObject.layer)) != 0) {
            if (Time.time - lastCollisionHit > .5f) {
                lastCollisionHit = Time.time;
                TakeHit();
            }
        }
    }
}
