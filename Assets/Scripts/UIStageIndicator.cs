using UnityEngine;
using UnityEngine.UI;

public class UIStageIndicator : MonoBehaviour
{
    public RawImage outerCircle;
    public RawImage innerCircle;
    public TMPro.TMP_Text text;
    public int ix;
    public int stageCount;
    public float timeComplete = -1;
    void Update() {
        if (stageCount <= 0) { return; }
        if (timeComplete == -1 && GameDirector.instance.nextLevel > ix) {
            timeComplete = Time.time;
        }
        transform.position = new Vector3(
            Screen.width*.5f - 300*(stageCount - 1)*.5f + 300*ix,
            Screen.height * .4f,
            0
        );
        float a = 0;
        if (timeComplete > 0) {
            a = Mathf.Clamp01((Time.time - timeComplete - 2) / 2f);
        }
        innerCircle.color = new Color(innerCircle.color.r, innerCircle.color.g, innerCircle.color.b, a);
    }
}
