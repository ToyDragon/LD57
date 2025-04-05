using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UIElements;


#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(EnemyShootScreen))]
public class EnemyShootScreenEditor : Editor {
    public override VisualElement CreateInspectorGUI() {
        VisualElement container = new VisualElement();
        container.Add(new IMGUIContainer(OnInspectorGUI));
        var label = new Label("Targeting location:");
        float w = 300;
        float h = w*9/16;
        label.style.width = w;
        label.style.height = h;
        label.style.backgroundColor = new Color(.1f, .1f, .1f);
        var t = (EnemyShootScreen)target;

        var x = new Label("X");
        x.style.marginLeft = -7 + w*t.screenTarget.x;
        x.style.marginTop = -11 + h*(1 - t.screenTarget.y);
        label.Add(x);

        label.RegisterCallback<ClickEvent>(evt => {
            t.screenTarget = new Vector2(
                evt.localPosition.x/w,
                1 - evt.localPosition.y/h
            );
            x.style.marginLeft = -7 + w*t.screenTarget.x;
            x.style.marginTop = -11 + h*(1 - t.screenTarget.y);
            EditorUtility.SetDirty(t);
        });
        container.Add(label);
        return container;
    }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
    }
}
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
