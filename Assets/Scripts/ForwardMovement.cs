using UnityEngine;

public class ForwardMovement : MonoBehaviour
{
    public float speed;
    private PlayerDamage damageController;
    public LevelInfo currentLevel;
    void OnEnable() {
        damageController = GetComponentInChildren<PlayerDamage>();
    }
    void Update() {
        if (!GameDirector.PlayingLevel() || LevelEndAnimator.AnimPlaying()) { return; }
        if (EnemyHealth.killsThisLevel > 0 && speed == 0) speed = 100f;
        transform.position += Vector3.forward * Time.deltaTime * speed * (.2f + .8f*Mathf.Clamp01(1 - damageController.damage/3) + Mathf.Clamp01(damageController.boost*2)*.5f);
    }
}
