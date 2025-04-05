using UnityEngine;

public class CameraFollow : MonoBehaviour {
    void LateUpdate() {
        var ship = ShipMovement.instance.transform;
        transform.localPosition = .8f*ship.localPosition + Vector3.back * 12f - Vector3.Dot(ship.right, Vector3.right)*Vector3.right*2;
        transform.rotation = Quaternion.Lerp(Quaternion.identity, ship.rotation, .05f);
    }
}
