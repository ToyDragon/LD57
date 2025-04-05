using Unity.VisualScripting;
using UnityEngine;

public class EnemyBulletManager : MonoBehaviour
{
    public static EnemyBulletManager instance;
    public GameObject bulletPrefab;
    public GameObject indicatorPrefab;
    public GameObject canvas;
    void OnEnable() {
        instance = this;
    }
    public static void Create(Vector3 position, Vector2 screenDestination) {
        var parent = GameObject.Find("Player scene");

        var go = GameObject.Instantiate(instance.bulletPrefab);
        go.transform.SetParent(parent.transform);
        go.transform.position = position;
        var bullet = go.GetComponent<EnemyBulletController>();
        bullet.sourcePos = position;
        bullet.arcRadius = 3;
        bullet.speed = 35;
        bullet.screenDestination = screenDestination;

        var ind = GameObject.Instantiate(instance.indicatorPrefab);
        ind.transform.SetParent(parent.transform);
        var indicator = ind.GetComponent<IncomingBulletIndicator>();
        indicator.bullet = go.transform;
        indicator.screenDest = screenDestination;
        ind.transform.SetParent(instance.canvas.transform);
        indicator.Init();
    }
}
