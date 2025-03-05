using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(ObjectPickup))]
public class ExplosiveObject : MonoBehaviour
{
    [SerializeField] protected float m_ImpulsePower = 10;
    [SerializeField] protected float m_Radius = 5;
    [SerializeField] private GameObject ExplosionParticle,ExplosionSoundObj;
    [SerializeField] private ObjectPickup PickableObjComponent;
    [SerializeField] private float ExplosionDmg;
    //[SerializeField] protected float m_TimeToExplode = 3;
    // Start is called before the first frame update
    void Start()
    {
        PickableObjComponent = GetComponent<ObjectPickup>();
    }

    // Update is called once per frame
    void Update()
    {
        if(PickableObjComponent.HP <= 0)
        {
            Explode();
        }
    }

    protected void Explode()
    {
        Instantiate(ExplosionParticle, transform.position, Quaternion.identity);
        Instantiate(ExplosionSoundObj, transform.position, Quaternion.identity);
        //Lean.Pool.LeanPool.Spawn(ExplosionParticle, transform.position, Quaternion.identity);
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, m_Radius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            EnemyBehavior Enemy = hit.GetComponent<EnemyBehavior>();
            PlayerBehavior player = hit.GetComponent<PlayerBehavior>();
            ExplosiveObject OtherBarrel = hit.GetComponent<ExplosiveObject>();

            if (rb != null)
            {
                rb.AddExplosionForce(m_ImpulsePower, explosionPos, m_Radius, 3.0f, ForceMode.Impulse);
            }
            
            if(Enemy != null)
            {
                Enemy.Stun();
                Enemy.lastreceivedphysicforce = ExplosionDmg / 2;
                Enemy.HP -= ExplosionDmg;
                
                
            }

            if (player != null)
            {
                player.HP -= ExplosionDmg * 0.35f;
                player.OnExploded();
            }

            if(OtherBarrel != null)
            {
                OtherBarrel.PickableObjComponent.HP = 0;
            }
        }

        Destroy(gameObject, 0);
        //Lean.Pool.LeanPool.Despawn(this);

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<ObjectPickup>() == true && other.gameObject.CompareTag("PhysicObj"))
        {
            if (other.gameObject.GetComponent<ObjectPickup>().playerpicked == true)
            {
                PickableObjComponent.HP -= other.gameObject.GetComponent<ObjectPickup>().PhysicsDmg;
                
            }

        }

        /*
        if (other.gameObject.GetComponent<ProjectileBehavior>() == true && other.gameObject.GetComponent<ObjectPickup>() == true)
        {
            if (other.gameObject.GetComponent<ObjectPickup>().playerpicked == true)
            {
                HP -= other.gameObject.GetComponent<ProjectileBehavior>().FixedDamageValue;
                
            }

        }
        */

        if(other.gameObject.GetComponent<ProjectileBehavior>() == true)
        {
            PickableObjComponent.HP -= other.gameObject.GetComponent<ProjectileBehavior>().FixedDamageValue;
        }

        if(other.gameObject.GetComponent<EnemyBehavior>() == true)
        {
            if(PickableObjComponent.playerpicked == true)
            {
                PickableObjComponent.HP = 0;
            }
        }
        
    }
}
