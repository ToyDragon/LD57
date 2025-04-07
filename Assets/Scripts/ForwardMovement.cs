using System.Collections.Generic;
using UnityEngine;

public class ForwardMovement : MonoBehaviour
{
    public float speed;
    private PlayerDamage damageController;
    public LevelInfo currentLevel;
    public AudioSource noEscape;
    private bool noEscapePlayed;
    private bool endMoved = false;
    void OnEnable() {
        damageController = GetComponentInChildren<PlayerDamage>();
    }
    void Update() {
        if (!GameDirector.PlayingLevel() || LevelEndAnimator.AnimPlaying()) { return; }
        transform.position += Vector3.forward * Time.deltaTime * speed * (.2f + .8f*Mathf.Clamp01(1 - damageController.damage/3) + Mathf.Clamp01(damageController.boost*2)*.5f);

        if (currentLevel.loopUntilAllDead) {
            if (EnemyHealth.killsThisLevel == 0) {
                if (transform.position.z > 1200) {
                    transform.position += Vector3.back*1000f;
                    foreach (var bullet in GameObject.FindObjectsByType<BulletController>(FindObjectsSortMode.None)) {
                        bullet.transform.position += Vector3.back*1000f;
                    }

                    if (!noEscapePlayed) {
                        noEscapePlayed = true;
                        noEscape.Play();
                    }
                }
            } else if (!endMoved) {
                endMoved = true;
                currentLevel.endBlock.transform.position = new Vector3(
                    currentLevel.endBlock.transform.position.x,
                    currentLevel.endBlock.transform.position.y,
                    transform.position.z + 530
                );
            }
        }
    }
}
