using UnityEngine;

public class ForwardMovement : MonoBehaviour
{
    public float speed;
    private PlayerDamage damageController;
    void OnEnable() {
        damageController = GetComponentInChildren<PlayerDamage>();
    }
    void Update() {
        transform.position += Vector3.forward * Time.deltaTime * speed * (.2f + .8f*Mathf.Clamp01(1 - damageController.damage/3));
    }
}
