using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class IncomingBulletIndicator : MonoBehaviour
{
    public Transform bullet;
    public RawImage outerCircle;
    public RawImage innerCircle;
    public Vector2 screenDest;
    public Color startColor = Color.white;
    public Color endColor = Color.red;
    private float maxdist = 40;
    public void Init() {
        var worldHitSpot = ShipMovement.GetScreenPos(screenDest);
        transform.position = Camera.main.WorldToScreenPoint(worldHitSpot);
        outerCircle.color = new Color(0,0,0,0);
        innerCircle.color = outerCircle.color;
        maxdist = (bullet.transform.position - worldHitSpot).magnitude;
    }
    void Update() {
        if (!bullet) {
            Destroy(gameObject);
            return;
        }
        var worldHitSpot = ShipMovement.GetScreenPos(screenDest);
        transform.position = Camera.main.WorldToScreenPoint(worldHitSpot);
        var t = Mathf.Clamp01(1 - (bullet.position - worldHitSpot).magnitude/maxdist);
        outerCircle.color = Color.Lerp(startColor, endColor, t);
        outerCircle.color = new Color(outerCircle.color.r, outerCircle.color.g, outerCircle.color.b, t*.4f);
        innerCircle.color = outerCircle.color;

        
        float border = 15;
        float width = 250;
        float w = border + (width-border)*t;
        innerCircle.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, (100 - w)*.5f, w);
        innerCircle.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, (100 - w)*.5f, w);

        outerCircle.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, (100 - width)*.5f, width);
        outerCircle.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, (100 - width)*.5f, width);
    }
}
