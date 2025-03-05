using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeTriggerBoxBehavior : MonoBehaviour
{
    [SerializeField] private GameObject StunHitParticle;
    [SerializeField] private float CustomizableFixDmg;
    private float CalculatedDmg;
    private PlayerBehavior playerSystem;
    // Start is called before the first frame update
    void Start()
    {
        playerSystem = GameObject.FindAnyObjectByType<PlayerBehavior>(); 
    }

    // Update is called once per frame
    void Update()
    {
        CalculatedDmg = CustomizableFixDmg + (playerSystem.SkillPowerBonus * 2.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<EnemyBehavior>() != null)
        {
            EnemyBehavior enemy = other.GetComponent<EnemyBehavior>();
            Instantiate(StunHitParticle, enemy.transform.position, Quaternion.identity);
            enemy.HP -= CustomizableFixDmg;
            enemy.lastreceivedphysicforce = CustomizableFixDmg * 2;
            enemy.Stun();

        }

        if(other.GetComponent<Rigidbody>() != null && other.GetComponent<ObjectPickup>() != null)
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            ObjectPickup PhysObj = other.GetComponent<ObjectPickup>();
            Instantiate(StunHitParticle, other.transform.position, Quaternion.identity);
            PhysObj.HP -= CustomizableFixDmg;
            rb.AddExplosionForce(rb.mass * 10f, other.gameObject.transform.position, 1f, Random.Range(1,3), ForceMode.Impulse);
            //rb.AddForce(rb.transform.forward * (rb.mass * 10f),ForceMode.Impulse);

        }

        if(other.GetComponent<ProjectileBehavior>() != null)
        {
            ProjectileBehavior projectile = other.GetComponent<ProjectileBehavior>();
            projectile.OnHit();
        }
    }
}
