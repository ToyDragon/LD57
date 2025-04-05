using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    public static PlayerDamage instance;
    public float damage;
    public List<AudioSource> hurtSounds;
    public float radius = 3;
    [HideInInspector]
    public float lastHitTime = -100;
    void OnEnable() {
        instance = this;
    }
    void Update() {
        damage = Mathf.Max(0, damage - Time.deltaTime);
    }
    public void BulletHit(Vector3 pos) {
        var del = transform.position - pos;
        del = new Vector3(del.x, del.y*3f, 0);
        if (del.magnitude < radius) {
            lastHitTime = Time.time;
            damage += .5f + 1.5f*Mathf.Clamp01(1 - damage/4);
            foreach (var sound in hurtSounds) {
                sound.Play();
            }
        }
    }
}
