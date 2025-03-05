
using UnityEngine;

public class SkillShieldBehavior : MonoBehaviour
{
    [Header("Skill Shield Parameters")]
    public float BaseHP = 5;
    public float BaseTimeSpan = 3;

    [Header("Skill Shield Components")]
    public GameObject ShieldDestoyParticle;
    public GameObject ShieldDestoySound;
    public GameObject ShieldHitParticle;
    public GameObject ShieldHitSound;
    private float HP;
    private float LifeSpan;
    private PlayerBehavior player;
    private Transform ForceShieldSpawnPoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ForceShieldSpawnPoint = GameObject.Find("SkillShieldPoint").transform;
        player = FindFirstObjectByType<PlayerBehavior>();
        LifeSpan = BaseTimeSpan + (player.SkillPowerBonus * 0.5f);
        HP = BaseHP + (player.SkillPowerBonus * 1f);
        Instantiate(ShieldHitParticle, ForceShieldSpawnPoint.transform.position, Quaternion.identity);
        Invoke(nameof(AutoDestoy), LifeSpan);
    }

    public void AutoDestoy()
    {
        Instantiate(ShieldDestoyParticle,transform.position,transform.rotation);
        Instantiate(ShieldDestoySound,transform.position,transform.rotation);
        Destroy(this.gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        if(HP <= 0)
        {
            AutoDestoy();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        /*
        if(other.GetComponent<ProjectileBehavior>() != null)
        {
            ProjectileBehavior Projectile = other.GetComponent<ProjectileBehavior>();
            Instantiate(ShieldHitParticle,other.transform.position,transform.rotation);
            Instantiate(ShieldHitSound,other.transform.position,transform.rotation);
            Projectile.OnHit();
            HP--;

        }
        */
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<ProjectileBehavior>() != null)
        {
            ProjectileBehavior Projectile = collision.gameObject.GetComponent<ProjectileBehavior>();
            Instantiate(ShieldHitParticle, collision.gameObject.transform.position, transform.rotation);
            Instantiate(ShieldHitSound, collision.gameObject.transform.position, transform.rotation);
            Projectile.OnHit();
            HP--;

        }
    }
}
