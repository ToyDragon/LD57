using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    public static ShipMovement instance;
    public Vector2 moveSpeed = new Vector2(3, 2);
    public float xLimit = 3;
    public float yUpperLimit = 1.5f;
    public float yLowerLimit = -1.3f;
    private Vector3 laggingLookOffset;
    private PlayerDamage playerDamage;
    void OnEnable() {
        instance = this;
        playerDamage = GetComponent<PlayerDamage>();
    }
    void Update() {
        Vector2 inputDir = Vector2.zero;
        if (Input.GetKey(KeyCode.W)) { inputDir += Vector2.up; }
        if (Input.GetKey(KeyCode.A)) { inputDir += Vector2.left; }
        if (Input.GetKey(KeyCode.S)) { inputDir += Vector2.down; }
        if (Input.GetKey(KeyCode.D)) { inputDir += Vector2.right; }

        transform.localPosition = ClampPos(
            transform.localPosition +
            Time.deltaTime*new Vector3(moveSpeed.x*inputDir.x, moveSpeed.y*inputDir.y, 0)
        );

        var lookOffset = new Vector3(moveSpeed.x*inputDir.x, moveSpeed.y*inputDir.y);
        float t = Mathf.Clamp01(Time.deltaTime*5);
        laggingLookOffset = laggingLookOffset*(1-t) + lookOffset*t;
        float damageT = 1 - Mathf.Clamp01((Time.time - playerDamage.lastHitTime)*3f);
        transform.LookAt(ClampPos(transform.position + laggingLookOffset + Vector3.forward*18) + damageT*10*new Vector3(Mathf.Sin(Time.time * 10), Mathf.Cos(Time.time*7)));
    }

    private Vector3 ClampPos(Vector3 pos) {
        return new Vector3(
            Mathf.Clamp(pos.x, -xLimit, xLimit),
            Mathf.Clamp(pos.y, yLowerLimit, yUpperLimit),
            pos.z
        );
    }
    public static Vector3 GetScreenPos(Vector2 screenPos) {
        return new Vector3(
            -instance.xLimit + instance.xLimit*2*screenPos.x,
            instance.yLowerLimit + (instance.yUpperLimit - instance.yLowerLimit)*screenPos.y,
            instance.transform.position.z
        );
    }
}
