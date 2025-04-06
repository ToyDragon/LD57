using UnityEngine;

public class HideOnStart : MonoBehaviour
{
    void Start() {
        foreach (var renderer in GetComponents<Renderer>()) {
            renderer.enabled = false;
        }
    }
}
