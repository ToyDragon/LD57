using UnityEngine;

public class Crystal : MonoBehaviour
{
    public Color color;

    void OnEnable()

    {
        var meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = new Material(meshRenderer.sharedMaterial);
        meshRenderer.sharedMaterial.color = color;
        meshRenderer.sharedMaterial.SetColor("_EmissionColor", color);
    }
}
