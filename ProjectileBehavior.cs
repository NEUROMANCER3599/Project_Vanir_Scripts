using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ProjectileBehavior : MonoBehaviour
{
    [SerializeField] private GameObject BulletHitParticle;
    public int FixedDamageValue;
    public float ProjectileSpeed;
    public bool IsDestroyedOnCollision;
    public bool IsAutoDespawn;

    private void Start()
    {
        if (IsAutoDespawn)
        {
            Invoke(nameof(OnHit), 5f);
        }
    }

    // Update is called once per frame
    public void OnHit()
    {
        Instantiate(BulletHitParticle, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision other)
    {
        
        if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Wall"))
        {
            if (IsDestroyedOnCollision)
            {
                OnHit();
            }
            
        }
        
        /*
        Lean.Pool.LeanPool.Spawn(BulletHitParticle, transform.position, transform.rotation);
        AutoDespawn();
        */
    }

}
