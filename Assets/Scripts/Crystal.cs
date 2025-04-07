using UnityEngine;

public class Crystal : MonoBehaviour
{
    public Color color;

    void OnEnable()

    {
        var meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = new Material(meshRenderer.sharedMaterial);
        if(color == Color.black) color = new Color(Random.Range(0,1),Random.Range(0,1),Random.Range(0,1));
        meshRenderer.sharedMaterial.color = color;
        meshRenderer.sharedMaterial.SetColor("_EmissionColor", color);
    }
}
