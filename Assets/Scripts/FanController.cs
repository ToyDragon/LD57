using UnityEngine;

public class FanController : MonoBehaviour
{

    public GameObject fanObject;
    public float rotationSpeed = 25f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fanObject.transform.Rotate(new Vector3(0f, Random.Range(0,360f), 0f));
    }

    // Update is called once per frame
    void Update()
    {
        fanObject.transform.Rotate(new Vector3(0f, -rotationSpeed * Time.deltaTime, 0f));
    }
}
