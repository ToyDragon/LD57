using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public static CameraFollow instance;
    public float followDist = 12f;
    private float initialFOV;
    private float fovOffset = 0;
    void OnEnable() {
        instance = this;
        initialFOV = Camera.main.fieldOfView;
    }
    void LateUpdate() {
        if (PlayerDamage.instance.boost > 0) {
            fovOffset = Mathf.Clamp01(fovOffset + Time.deltaTime * 4f);
        } else {
            fovOffset = Mathf.Clamp01(fovOffset - Time.deltaTime * 3f);
        }
        Camera.main.fieldOfView = initialFOV + fovOffset*-10;
        if (!GameDirector.PlayingLevel() || LevelEndAnimator.AnimPlaying()) { return; }
        var ship = ShipMovement.instance.transform;
        transform.localPosition = .8f*ship.localPosition + Vector3.back * followDist - Vector3.Dot(ship.forward, Vector3.right)*Vector3.right*5;
        transform.rotation = Quaternion.Lerp(Quaternion.identity, ship.rotation, .05f);
    }
}
