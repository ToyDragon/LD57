using UnityEngine;

public class StopMoving : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(EnemyHealth.killsThisLevel > 0) GameObject.Destroy(this.gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("TRIGGER ENTER with " + other.gameObject.name);
         if(other.gameObject.GetComponentInParent<ForwardMovement>() != null){
            other.gameObject.GetComponentInParent<ForwardMovement>().speed = 0;
            GameObject.Destroy(this.gameObject);
        }       
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("COLLISION ENTER with " + collision.gameObject.name);
        if(collision.gameObject.GetComponentInParent<ForwardMovement>() != null){
            collision.gameObject.GetComponentInParent<ForwardMovement>().speed = 0;
        }       
    }
}
