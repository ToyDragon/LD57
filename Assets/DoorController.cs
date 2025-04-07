using UnityEngine;

public class DoorController : MonoBehaviour
{
    public int HitsToOpen;
    private int hits = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hit()
    {
        Debug.Log("door got hit");
        hits++;
        Debug.Log("Hits is at " + hits);
        if(hits == HitsToOpen){
            Debug.Log("Opening door after being hit");
            var animation = GetComponent<Animator>();
            animation.SetBool("open", true);
        }
    }
}
