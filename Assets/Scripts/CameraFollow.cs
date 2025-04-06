using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public static CameraFollow instance;
    public float followDist = 12f;
    void OnEnable() {
        instance = this;
    }
    void LateUpdate() {
        if (!GameDirector.PlayingLevel() || LevelEndAnimator.AnimPlaying()) { return; }
        var ship = ShipMovement.instance.transform;
        transform.localPosition = .8f*ship.localPosition + Vector3.back * followDist - Vector3.Dot(ship.forward, Vector3.right)*Vector3.right*5;
        transform.rotation = Quaternion.Lerp(Quaternion.identity, ship.rotation, .05f);
    }
}
