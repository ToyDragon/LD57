using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(ShipEnemyShootScreen))]
public class ShipEnemyShootScreenEditor : Editor {
    private List<Label> labels = new List<Label>();
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
        var t = (ShipEnemyShootScreen)target;

        labels.Clear();
        int idx = 0;
        foreach (var target in t.screenTarget)
        {
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
            y.style.left = w*target.x;
            y.style.top = w*(1 - target.y) - idx * 17f;
            y.pickingMode = PickingMode.Ignore;
            labels.Add(y);
            idx++;
        }
             
        circle.RegisterCallback<ClickEvent>(evt => {
            Vector2 target = new Vector2(
                evt.localPosition.x/w,
                1 - evt.localPosition.y/w
            );
            t.screenTarget.Add(target);
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
            y.style.left = w*target.x;
            y.style.top = w*(1 - target.y) - 17f * (t.screenTarget.Count - 1);
            y.pickingMode = PickingMode.Ignore;
            container.Add(y);
            EditorUtility.SetDirty(t);
        }, TrickleDown.TrickleDown);
        container.Add(circle);
        foreach (var label in labels){
            container.Add(label);
        }
        return container;
    }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
    }
}
#endif

public class ShipEnemyShootScreen : MonoBehaviour
{
    public string patternName;
    public List<Vector2> screenTarget = new List<Vector2>(){
        new Vector2(0.5f,0.5f)
    };
    [SerializeField]
    public int currentIndex = 0;
    public float shootDelay;
    public float triggerDistance;
    public float minTriggerDistance = 15;
    public List<Transform> guns;
    private int lastGun;
    public float hitScale = 1;
    public bool targetShip;
    public float bulletSpeed = 60;
    private float lastShoot = -100;
    public bool shootingDone = false;

    void OnEnable() {
        if (guns.Count == 0) {
            guns.Add(transform);
        }
    }
    void Update() {
        var ship = ShipMovement.instance;
        var shipPos = ship.transform.position;
        var shipLocal = ship.transform.localPosition;
        if (transform.position.z > shipPos.z + minTriggerDistance && transform.position.z < shipPos.z + triggerDistance) {
            if (Time.time - lastShoot > shootDelay && !shootingDone) {
                lastShoot = Time.time;
                var shipScreenPos = new Vector2(.5f + .5f*shipLocal.x/ship.radialLimit, .5f + .5f*shipLocal.y/ship.radialLimit);
                var target = targetShip ? shipScreenPos : screenTarget[currentIndex];
                EnemyBulletManager.Create(guns[lastGun].position, target, bulletSpeed, hitScale);
                lastGun = (lastGun + 1) % guns.Count;
                currentIndex++;
                if(currentIndex >= screenTarget.Count){
                    currentIndex = 0;
                    shootingDone = true;
                } 
            }
        }
    }
}
