using UnityEngine;

public class RingBehavior : MonoBehaviour
{
    public float collectedTime = 0;
    private float rotAtCollectTime = -1;
    void Update() {
        if (collectedTime > 0) {
            if (rotAtCollectTime == -1) {
                rotAtCollectTime = transform.localRotation.eulerAngles.z;
            }
            float tRot = Time.time - collectedTime;
            transform.localRotation = Quaternion.Euler(0, 0, rotAtCollectTime + tRot*270);
        } else {
            transform.localRotation = Quaternion.Euler(0, 0, Time.time * 90);
        }
    }
}
