using System.Collections.Generic;
using UnityEngine;

public class ShipShooting : MonoBehaviour
{
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
    private Camera cam;
    void OnEnable() {
        cam = Camera.main;
    }
    void Update() {
        if (targetingMode == TargetingMode.ThreeD) {
            indicator2D.SetActive(false);
            indicator3D.SetActive(true);
            indicator3D.transform.position = GetCamTargetPoint();
            indicator3D.transform.LookAt(cam.transform);
            if (Input.GetKeyDown(KeyCode.Mouse0)) {
                SendBulletTo(indicator3D.transform.position);
            }
        }

        if (targetingMode == TargetingMode.TwoD) {
            indicator2D.SetActive(true);
            indicator3D.SetActive(false);

            var rt = (RectTransform)indicator2D.transform;
            rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, Input.mousePosition.x - rt.sizeDelta.x*.5f, rt.sizeDelta.x);
            rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, Input.mousePosition.y - rt.sizeDelta.y*.5f, rt.sizeDelta.y);
            if (Input.GetKeyDown(KeyCode.Mouse0)) {
                SendBulletTo(GetCamTargetPoint());
            }
        }
    }
    private Vector3 GetCamTargetPoint() {
        var ray = cam.ScreenPointToRay(Input.mousePosition);
        var hitPoint = cam.transform.position + ray.direction * max3dTargetingDistance;
        if (Physics.Raycast(ray, out var hit, 9999, targetingLayers)) {
            hitPoint = hit.point - ray.direction * 2;
            var del = hitPoint - cam.transform.position;
            if (del.magnitude > max3dTargetingDistance) {
                hitPoint = cam.transform.position + del.normalized * max3dTargetingDistance;
            }
        }
        return hitPoint;
    }

    private void SendBulletTo(Vector3 destination) {
        var go = GameObject.Instantiate(bulletPrefab);
        go.transform.position = guns[lastGun].transform.position;
        lastGun = (lastGun + 1) % guns.Count;
        var bullet = go.GetComponent<BulletController>();
        bullet.direction = (destination - go.transform.position).normalized;
        bullet.speed = bulletSpeed;
        bullet.expireTime = Time.time + bulletDuration;
    }
}
