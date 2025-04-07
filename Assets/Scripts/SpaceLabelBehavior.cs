using UnityEngine;

public class SpaceLabelBehavior : MonoBehaviour
{
    void Update() {
        int ix = GameDirector.instance.nextLevel;
        if (ix >= GameDirector.instance.levels.Count) {
            transform.position = Vector3.one * -1000;
        } else {
            transform.position = GameDirector.instance.stageIndicators[ix].transform.position + Vector3.down * (Mathf.Abs(Mathf.Sin(Time.time * 4)) * 50 + 150);
        }
    }
}
