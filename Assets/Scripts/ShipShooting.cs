using System.Collections.Generic;
using UnityEngine;

public class ShipShooting : MonoBehaviour
{
    public static ShipShooting instance;
    public GameObject bulletPrefab;
    public List<Transform> guns;
    public GameObject indicator3D;
    public GameObject indicator2D;
    public float max3dTargetingDistance = 50;
    public float bulletSpeed = 10;
    public float bulletDuration = 3;
    private int lastGun;
    public enum TargetingMode {
        ThreeD,
        TwoD
    }
    public TargetingMode targetingMode;
    public LayerMask targetingLayers;
    public LayerMask aimAssistLayers;
    private Camera cam;
    private float? lastShoot;
    public float shootDelay = .25f;
    public AudioSource shootSource;
    public Animator gunAnimator;
    void OnEnable() {
        cam = Camera.main;
        Cursor.visible = false;
        instance = this;
        gunAnimator.SetBool("GunsOpen", true);
    }
    void Update() {
        if (LevelEndAnimator.AnimPlaying()) { return; }
        indicator2D.SetActive(targetingMode == TargetingMode.TwoD);
        indicator3D.SetActive(targetingMode == TargetingMode.ThreeD);
        if (targetingMode == TargetingMode.ThreeD) {
            indicator3D.transform.position = GetCamTargetPoint();
            indicator3D.transform.LookAt(cam.transform);
            TryShootTo(indicator3D.transform.position);
        }

        if (targetingMode == TargetingMode.TwoD) {
            var rt = (RectTransform)indicator2D.transform;
            rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, Input.mousePosition.x - rt.sizeDelta.x*.5f, rt.sizeDelta.x);
            rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, Input.mousePosition.y - rt.sizeDelta.y*.5f, rt.sizeDelta.y);
            TryShootTo(GetCamTargetPoint());
        }
    }
    private Vector3 GetCamTargetPoint() {
        var ray = cam.ScreenPointToRay(Input.mousePosition);
        var hitPoint = cam.transform.position + ray.direction * max3dTargetingDistance;
        if (Physics.SphereCast(ray, 1, out var hit, 9999, targetingLayers, QueryTriggerInteraction.Ignore)) {
            hitPoint = ray.origin + ray.direction*Mathf.Max(1, hit.distance - 2);
            if ((aimAssistLayers & (1 << hit.collider.gameObject.layer)) != 0) {
                hitPoint = hitPoint*.25f + hit.point*.75f;
            }
            var del = hitPoint - cam.transform.position;
            if (del.magnitude > max3dTargetingDistance) {
                hitPoint = cam.transform.position + del.normalized * max3dTargetingDistance;
            }
        }
        return hitPoint;
    }
    private void TryShootTo(Vector3 destination) {
        if (Input.GetKey(KeyCode.Mouse0)) {
            if (!lastShoot.HasValue || Time.time - lastShoot.Value > shootDelay) {
                lastShoot = Time.time;
                SendBulletTo(destination);
            }
        }
    }
    private void SendBulletTo(Vector3 destination) {
        var go = GameObject.Instantiate(bulletPrefab);
        go.transform.position = guns[lastGun].transform.position;
        lastGun = (lastGun + 1) % guns.Count;
        var bullet = go.GetComponent<BulletController>();
        bullet.direction = (destination - go.transform.position).normalized;
        bullet.speed = bulletSpeed;
        bullet.expireTime = Time.time + bulletDuration;
        shootSource.Play();
    }
}
