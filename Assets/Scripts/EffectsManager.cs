using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    public static EffectsManager instance;
    public GameObject bulletHitEffectPrefab;
    public GameObject bulletHitEnemyEffectPrefab;
    void OnEnable() {
        instance = this;
    }
    void Update() {
        
    }
    public static void BulletHit(Vector3 pos, Vector3 normal, bool hitEnemy) {
        var go = GameObject.Instantiate(instance.bulletHitEffectPrefab);
        var fwd = Random.insideUnitSphere;
        fwd = (fwd - Vector3.Dot(fwd, normal)*normal).normalized;
        var rot = Quaternion.LookRotation(normal, fwd);
        go.transform.rotation = rot;
        go.transform.position = pos;
        if (hitEnemy) {
            go = GameObject.Instantiate(instance.bulletHitEnemyEffectPrefab);
            go.transform.position = pos;
            go.transform.rotation = rot;
        }
    }
}
