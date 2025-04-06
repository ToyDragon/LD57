using UnityEngine;

public class LevelEndAnimator : MonoBehaviour
{
    public static LevelEndAnimator instance;
    private float endAnimStartTime = -1;
    public LevelInfo currentLevel;
    public AudioClip clipDrillA;
    public AudioClip clipDrillB;
    public GameObject ship;
    private Quaternion originalCamRot;
    private Vector3 originalCamPos;
    private int soundState = 0;
    private AudioSource audioSource;
    void OnEnable() {
        instance = this;
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        var cam = Camera.main;
        if (endAnimStartTime < 0) {
            if (transform.position.z > currentLevel.endBlock.position.z - currentLevel.endBlock.localScale.z*.5f - 20) {
                endAnimStartTime = Time.time;
                originalCamPos = cam.transform.position;
                originalCamRot = cam.transform.rotation;
                soundState = 0;

                ShipShooting.instance.gunAnimator.SetBool("GunsOpen", false);
            }
            return;
        }

        float dt = Time.time - endAnimStartTime;
        float moveT = Mathf.Clamp01(dt / .5f);
        float rotT = dt / 3f;
        var newCameraPos = ship.transform.position + Vector3.back*2 + Quaternion.AngleAxis(rotT*180, Vector3.forward)*Vector3.right*20f*Mathf.Clamp01(moveT);
        cam.transform.position = (1-moveT)*originalCamPos + moveT*newCameraPos;
        cam.transform.LookAt(ship.transform, Vector3.back);

        if (dt > 0f && soundState == 0) { PlaySound(clipDrillB); soundState++; }
        if (dt > .6f && soundState == 1) { audioSource.Stop(); PlaySound(clipDrillB); soundState++; }
        if (dt > 1.5f && soundState == 2) { PlaySound(clipDrillA); soundState++; }
    }
    private void PlaySound(AudioClip clip) {
        audioSource.PlayOneShot(clip);
    }
    public static bool AnimPlaying() {
        return instance.endAnimStartTime > 0;
    }
}
