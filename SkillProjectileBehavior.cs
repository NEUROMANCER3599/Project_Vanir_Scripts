using UnityEngine;

public class SkillProjectileBehavior : MonoBehaviour
{
    [Header("Skill Projectile Parameter")]
    public float BaseDmg = 20;
    public float BaseProjectileSpeed = 20;

    [Header("Skill Projectile Components")]
    public GameObject HitParticlePrefab;
    public GameObject HitSoundObj;
    public GameObject SpawnParticlePrefab;
    private float DMG;
    private float ProjSpeed;
    private PlayerBehavior player;
    private Transform SkillProjectileSpawnPoint;
    private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = FindAnyObjectByType<PlayerBehavior>();
        SkillProjectileSpawnPoint = GameObject.Find("SkillProjectilePoint").transform;

        Instantiate(SpawnParticlePrefab, SkillProjectileSpawnPoint.transform.position, transform.rotation);
        

    }

    // Update is called once per frame
    void Update()
    {
        //SkillProjectileSpawnPoint = GameObject.Find("SkillProjectilePoint").transform;
        DMG = BaseDmg + (player.SkillPowerBonus * 2);
        ProjSpeed = BaseProjectileSpeed + (player.SkillPowerBonus * 1);
        rb.linearVelocity = SkillProjectileSpawnPoint.transform.forward * ProjSpeed;


    }

    public float ProjectileSpeed()
    {
        return ProjSpeed;
    }

    public void OnHit()
    {
        Instantiate(HitParticlePrefab, transform.position, transform.rotation);
        Instantiate(HitSoundObj, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Wall"))
        {
            OnHit();
        }

        if(other.gameObject.GetComponent<EnemyBehavior>())
        {
            EnemyBehavior Enemy = other.gameObject.GetComponent<EnemyBehavior>();
            Enemy.Stun();
            Enemy.HP -= DMG;
            OnHit();
        }

        if (other.gameObject.GetComponent<Rigidbody>() != null && other.gameObject.GetComponent<ObjectPickup>() != null && !other.gameObject.GetComponent<ProjectileBehavior>())
        {
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            ObjectPickup PhysObj = other.gameObject.GetComponent<ObjectPickup>();
            PhysObj.HP -= DMG;
            rb.AddExplosionForce(rb.mass * 10f, other.gameObject.transform.position, 1f, Random.Range(1, 3), ForceMode.Impulse);
            OnHit();
            //rb.AddForce(rb.transform.forward * (rb.mass * 10f),ForceMode.Impulse);

        }
    }
}
