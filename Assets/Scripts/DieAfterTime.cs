using UnityEngine;

public class DieAfterTime : MonoBehaviour
{
    private float startTime;
    public float lifeTime;
    void OnEnable() {
        startTime = Time.time;
    }
    void Update() {
        if (Time.time - startTime > lifeTime) {
            Destroy(gameObject);
        }
    }
}
