using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent (typeof(ObjectPickup))]
[RequireComponent(typeof(ProjectileBehavior))]
public class GrenadeBehavior : MonoBehaviour
{
    [Header("Grenade Parameter")]
    [SerializeField] private AudioClip CountdownSound;
    [SerializeField] private AudioSource SoundPlayer;
    [SerializeField] private GameObject ExplosionParticle;
    [SerializeField] private GameObject ExplosionSoundObj;
    [SerializeField] private float ExplosionDamage;
    [SerializeField] private float ExplosionRadius;
    [SerializeField] private float ExplosionForce;
    private float ExplosionTime;
    private ObjectPickup ObjectPickupModule;
    private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ObjectPickupModule = GetComponent<ObjectPickup>();
        SoundPlayer = GetComponent<AudioSource>();
        SoundPlayer.clip = CountdownSound;
        SoundPlayer.Play();
        ExplosionTime = CountdownSound.length;
        Invoke(nameof(OnExplode), ExplosionTime);
    }

    // Update is called once per frame
    void Update()
    {
        //rb.useGravity = true;
    }

    void OnExplode()
    {
        Instantiate(ExplosionParticle, transform.position, Quaternion.identity);
        Instantiate(ExplosionSoundObj, transform.position, Quaternion.identity);

        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, ExplosionRadius);

        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            EnemyBehavior Enemy = hit.GetComponent<EnemyBehavior>();
            PlayerBehavior player = hit.GetComponent<PlayerBehavior>();
            ExplosiveObject OtherBarrel = hit.GetComponent<ExplosiveObject>();

            if (rb != null)
            {
                rb.AddExplosionForce(ExplosionForce, explosionPos, ExplosionRadius, 3.0f, ForceMode.Impulse);
            }

            if (Enemy != null)
            {
                
                    Enemy.Stun();
                    Enemy.lastreceivedphysicforce = ExplosionDamage / 2;
                    Enemy.HP -= ExplosionDamage;
               
            }

            if (player != null)
            {
                player.HP -= ExplosionDamage;
                player.OnExploded();
            }

        }

        Destroy(gameObject, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<EnemyBehavior>() == true)
        {
            if (ObjectPickupModule.playerpicked)
            {
                OnExplode();
            }
        }

        if (collision.gameObject.GetComponent<PlayerBehavior>())
        {
            if (!ObjectPickupModule.playerpicked)
            {
                OnExplode();
            }
        }
    }
}
