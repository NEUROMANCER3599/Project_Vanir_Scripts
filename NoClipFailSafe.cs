using UnityEngine;

public class NoClipFailSafe : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerBehavior>())
        {
            PlayerBehavior player = other.gameObject.GetComponent<PlayerBehavior>();

            player.HP = 0;
        }

        if (other.gameObject.GetComponent<EnemyBehavior>())
        {
            EnemyBehavior enemy = other.gameObject.GetComponent<EnemyBehavior>();
            enemy.HP = 0;   
        }
    }
}
