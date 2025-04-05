using UnityEngine;

public class CameraFollow : MonoBehaviour {
    void LateUpdate() {
        var ship = ShipMovement.instance.transform;
        transform.localPosition = .8f*ship.localPosition + Vector3.back * 12f;
        transform.rotation = Quaternion.Lerp(Quaternion.identity, ship.rotation, .15f);
    }
}
