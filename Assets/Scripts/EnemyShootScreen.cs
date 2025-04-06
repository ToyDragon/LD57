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
        var circle = new Label("");
        float w = 400;
        circle.style.width = w;
        circle.style.height = w;
        circle.style.backgroundColor = new Color(.1f, .1f, .1f);
        circle.style.borderTopLeftRadius = w/2;
        circle.style.borderTopRightRadius = w/2;
        circle.style.borderBottomLeftRadius = w/2;
        circle.style.borderBottomRightRadius = w/2;
        circle.style.overflow = Overflow.Hidden;
        var t = (EnemyShootScreen)target;

        var y = new Label(" ");
        y.style.position = Position.Relative;
        y.style.marginBottom = w;
        float h = 33 * t.hitScale;
        y.style.width = h;
        y.style.height = h;
        y.style.marginLeft = -h*.5f;
        y.style.marginTop = -h*.5f - w;
        y.style.borderTopLeftRadius = h*.5f;
        y.style.borderTopRightRadius = h*.5f;
        y.style.borderBottomLeftRadius = h*.5f;
        y.style.borderBottomRightRadius = h*.5f;
        y.style.backgroundColor = new Color(1, 0, 0);
        y.style.left = w*t.screenTarget.x;
        y.style.top = w*(1 - t.screenTarget.y);
        y.pickingMode = PickingMode.Ignore;        
        circle.RegisterCallback<ClickEvent>(evt => {
            t.screenTarget = new Vector2(
                evt.localPosition.x/w,
                1 - evt.localPosition.y/w
            );
            y.style.left = w*t.screenTarget.x;
            y.style.top = w*(1 - t.screenTarget.y);
            float h = 33 * t.hitScale;
            y.style.width = h;
            y.style.height = h;
            y.style.marginLeft = -h*.5f;
            y.style.marginTop = -h*.5f - w;
            y.style.borderTopLeftRadius = h*.5f;
            y.style.borderTopRightRadius = h*.5f;
            y.style.borderBottomLeftRadius = h*.5f;
            y.style.borderBottomRightRadius = h*.5f;
            EditorUtility.SetDirty(t);
        }, TrickleDown.TrickleDown);
        container.Add(circle);
        container.Add(y);
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
    public float minTriggerDistance = 15;
    public List<Transform> guns;
    private int lastGun;
    public float hitScale = 1;
    private float lastShoot = -100;
    void OnEnable() {
        if (guns.Count == 0) {
            guns.Add(transform);
        }
    }
    void Update() {
        var shipPos = ShipMovement.instance.transform.position;
        if (transform.position.z > shipPos.z + minTriggerDistance && transform.position.z < shipPos.z + triggerDistance) {
            if (Time.time - lastShoot > shootDelay) {
                lastShoot = Time.time;
                EnemyBulletManager.Create(guns[lastGun].position, screenTarget, hitScale);
                lastGun = (lastGun + 1) % guns.Count;
            }
        }
    }
}
