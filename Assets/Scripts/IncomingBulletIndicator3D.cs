using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class IncomingBulletIndicator3D : MonoBehaviour
{
    public Transform bullet;
    public MeshRenderer outerCircle;
    public MeshRenderer innerCircle;
    public Vector2 screenDest;
    public Color startColor = Color.white;
    public Color endColor = Color.red;
    public AudioSource hitSound;
    private float endTime = -1;
    [HideInInspector]
    public float scale;
    private float maxdist = 40;
    void OnEnable() {   
        outerCircle.sharedMaterial = new Material(outerCircle.sharedMaterial);
        innerCircle.sharedMaterial = new Material(innerCircle.sharedMaterial);
    }
    public void Init() {
        var worldHitSpot = ShipMovement.GetScreenPos(screenDest);
        outerCircle.sharedMaterial.color = new Color(0,0,0,0);
        innerCircle.sharedMaterial.color = outerCircle.sharedMaterial.color;
        maxdist = (bullet.transform.position - worldHitSpot).magnitude;
    }
    void Update() {
        if (!bullet) {
            innerCircle.enabled = false;
            outerCircle.enabled = false;
            if (endTime == -1) {
                hitSound.Play();
                endTime = Time.time;
            }
            if (Time.time - endTime > 2f) {
                Destroy(gameObject);
            }
            return;
        }
        var worldHitSpot = ShipMovement.GetScreenPos(screenDest) + Vector3.back*2;
        var t = Mathf.Clamp01(1 - (bullet.position - worldHitSpot).magnitude/maxdist);
        outerCircle.sharedMaterial.color = Color.Lerp(startColor, endColor, t);
        outerCircle.sharedMaterial.color = new Color(
            outerCircle.sharedMaterial.color.r,
            outerCircle.sharedMaterial.color.g,
            outerCircle.sharedMaterial.color.b,
            t*.4f
        );
        innerCircle.sharedMaterial.color = outerCircle.sharedMaterial.color;

        innerCircle.transform.position = worldHitSpot;
        innerCircle.transform.localScale = 2*(scale + PlayerDamage.instance.radius) * Vector3.one;
        innerCircle.sharedMaterial.SetFloat("_Radius", t*.45f);
        outerCircle.transform.position = worldHitSpot;
        outerCircle.transform.localScale = 2*(scale + PlayerDamage.instance.radius) * Vector3.one;
    }
}
