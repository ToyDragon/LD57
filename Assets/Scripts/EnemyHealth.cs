using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public static int killsThisLevel = 0;
    public int life;
    private bool dead;
    void Update() {
        
    }
    public void Hit() {
        life -= 1;
        if (life < 0 && !dead) {
            dead = true;
            Destroy(gameObject);
            killsThisLevel++;
        }
    }
}
