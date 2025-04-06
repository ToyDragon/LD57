using Unity.VisualScripting;
using UnityEngine;

public class EnemyBulletManager : MonoBehaviour
{
    public static EnemyBulletManager instance;
    public GameObject bulletPrefab;
    public GameObject indicatorPrefab;
    public GameObject indicatorPrefab3D;
    public GameObject canvas;
    void OnEnable() {
        instance = this;
    }
    public static void Create(Vector3 position, Vector2 screenDestination, float bulletSpeed, float scale) {
        var parent = GameObject.Find("Player scene");

        var go = GameObject.Instantiate(instance.bulletPrefab);
        go.transform.SetParent(parent.transform);
        go.transform.position = position;
        var bullet = go.GetComponent<EnemyBulletController>();
        bullet.sourcePos = position;
        bullet.speed = bulletSpeed;
        bullet.screenDestination = screenDestination;
        bullet.scale = scale;

        var ind = GameObject.Instantiate(instance.indicatorPrefab3D);
        var indicator = ind.GetComponent<IncomingBulletIndicator3D>();
        indicator.bullet = go.transform;
        indicator.screenDest = screenDestination;
        indicator.Init();
        indicator.scale = scale;
    }
}
