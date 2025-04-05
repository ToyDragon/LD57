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
            Destroy(gameObject);
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
        outerCircle.transform.position = worldHitSpot;
        outerCircle.transform.localScale = 2*(scale + PlayerDamage.instance.radius) * Vector3.one*t;
    }
}
