using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UIElements;


#if UNITY_EDITOR
// using UnityEditor;
// [CustomEditor(typeof(EnemyShootScreen))]
// public class EnemyShootScreenEditor : Editor {
//     public override VisualElement CreateInspectorGUI()
//     {
//         VisualElement myInspector = new VisualElement();

//         // Add a simple label.
//         myInspector.Add(new Label("Targeting location:"));

//         // Return the finished Inspector UI.
//         return myInspector;
//     }

// }
#endif

public class EnemyShootScreen : MonoBehaviour
{
    public Vector2 screenTarget = new Vector2(.5f, .5f);
    public float shootDelay;
    public float triggerDistance;
    public List<Transform> guns;
    private int lastGun;
    private float lastShoot = -100;
    void OnEnable() {
        if (guns.Count == 0) {
            guns.Add(transform);
        }
    }
    void Update() {
        var shipPos = ShipMovement.instance.transform.position;
        if (transform.position.z > shipPos.z && transform.position.z < shipPos.z + triggerDistance) {
            if (Time.time - lastShoot > shootDelay) {
                lastShoot = Time.time;
                EnemyBulletManager.Create(guns[lastGun].position, screenTarget);
                lastGun = (lastGun + 1) % guns.Count;
            }
        }
    }
}
