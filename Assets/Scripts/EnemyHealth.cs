using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int life;
    void Update() {
        
    }
    public void Hit() {
        life -= 1;
        if (life < 0) {
            Destroy(gameObject);
        }
    }
}
