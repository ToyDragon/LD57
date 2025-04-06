using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(SnapToWall))]
public class SnapToWallEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        if (GUILayout.Button("snap")) {
            var t = (SnapToWall) target;
            var dir = new Vector3(
                t.transform.position.x,
                t.transform.position.y,
                0
            ).normalized;
            Debug.DrawLine(t.transform.position, t.transform.position+dir*20, Color.red, 10);
            if (Physics.SphereCast(t.transform.position - dir, 1, dir, out var hit, 20)) {
                t.transform.position = hit.point;
                t.transform.rotation = Quaternion.LookRotation(Vector3.back, Quaternion.AngleAxis(-90, Vector3.forward)*hit.normal);
            } else {
                Debug.Log($"No hit");
            }
        }
    }
}
#endif

public class SnapToWall : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
