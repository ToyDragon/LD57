using System.Net;
using UnityEditor.Build;
using UnityEngine;

public class LevelEndAnimator : MonoBehaviour
{
    public static LevelEndAnimator instance;
    private float endAnimStartTime = -1;
    public LevelInfo currentLevel;
    public AudioClip clipDrillA;
    public AudioClip clipDrillB;
    public AudioClip clipDrillLongA;
    public GameObject ship;
    private Vector3 originalCamPos;
    private int soundState = 0;
    private AudioSource audioSource;
    private Vector3 originalShipPos;
    private Quaternion originalShipRot;
    private float[] clipSample = new float[1024];
    public float avgVolume = .95f;
    private Vector3 cameraShakeOffset = Vector3.zero;
    private Vector3 currentShakeDir = Vector3.zero;
    public GameObject diggingParticleObject;
    public AudioSource windSource;
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
                soundState = 0;

                originalShipPos = ship.transform.position;
                originalShipRot = ship.transform.rotation;

                ShipShooting.instance.gunAnimator.SetBool("GunsOpen", false);
            }
            if (endAnimStartTime < 0) {
                diggingParticleObject.SetActive(false);
            }
            return;
        }
        windSource.volume = .03f;

        float dt = Time.time - endAnimStartTime;
        float moveT = Mathf.Clamp01(dt / .5f);
        float rotT = dt / 2f;
        var newCameraPos = ship.transform.position + Vector3.back*2 + Quaternion.AngleAxis(rotT*180, Vector3.back)*Vector3.right*20f*Mathf.Clamp01(moveT);
        cam.transform.position = (1-moveT)*originalCamPos + moveT*newCameraPos;
        cam.transform.LookAt(ship.transform, Vector3.back);

        var surfaceShipPos = currentLevel.endBlock.transform.position - Vector3.forward*currentLevel.endBlock.transform.localScale.z*.5f + Vector3.back*2.5f;
        if (dt > 4) {
            diggingParticleObject.SetActive(true);
            float shipT = Mathf.Clamp01((dt - 4) / .5f);
            ship.transform.position = surfaceShipPos + Vector3.forward*shipT*10f;

            audioSource.clip.GetData(clipSample, audioSource.timeSamples);
            float vol = 1 - Time.deltaTime;
            foreach (float sample in clipSample) {
                vol += Mathf.Abs(sample) * 0.001f;
            }
            avgVolume = avgVolume*.7f + vol*.3f;

            if (vol > avgVolume*.95f && currentShakeDir.magnitude < 1f) {
                currentShakeDir = Random.insideUnitSphere * 50f * Mathf.Clamp01((vol - avgVolume*.95f) * 10f);
            }
            currentShakeDir *= Mathf.Clamp01(1 - Time.deltaTime*1);
            cameraShakeOffset *= Mathf.Clamp01(1 - Time.deltaTime*1);
            cameraShakeOffset += currentShakeDir * Mathf.Clamp01((vol - avgVolume*.95f)*10) * Time.deltaTime;
            cam.transform.LookAt(ship.transform.position + cameraShakeOffset, Vector3.back);
        } else {
            float shipT = Mathf.Clamp01(dt / 2f);
            ship.transform.position = (1-shipT)*originalShipPos + shipT*surfaceShipPos;
            ship.transform.rotation = Quaternion.Lerp(originalShipRot, Quaternion.LookRotation(Vector3.forward, Vector3.up), shipT);
        }

        if (dt > 0f && soundState == 0) { PlaySound(clipDrillB); soundState++; }
        if (dt > .6f && soundState == 1) { audioSource.Stop(); PlaySound(clipDrillB); soundState++; }
        if (dt > 1.5f && soundState == 2) { PlaySound(clipDrillA); soundState++; }
        if (dt > 2.5f && soundState == 3) { PlaySound(clipDrillB); soundState++; }
        if (dt > 3.5f && soundState == 4) {
            audioSource.clip = clipDrillLongA;
            audioSource.loop = true;
            audioSource.Play();
            soundState++;
        }
    }
    private void PlaySound(AudioClip clip) {
        audioSource.PlayOneShot(clip);
    }
    public static bool AnimPlaying() {
        return instance.endAnimStartTime > 0;
    }
}
