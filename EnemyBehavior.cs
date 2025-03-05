using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Animations;
using UnityEditor;

[RequireComponent(typeof(AISensor))]
public class EnemyBehavior : MonoBehaviour
{
    

    [Header("Enemy Stats (Customizable)")]
    public float MaxHP;
    [SerializeField] private float MaxStunTime;
    [SerializeField] private int Score;
    public float movespeed = 5;
    private float defaultmovespeed;
    

    [Header("Enemy System")]
    private PlayerBehavior player;
    private NavMeshAgent EnemyNavMeshAgent;
    [SerializeField] private Slider HPindicator;
    [SerializeField] private GameObject HPIndicatorObj;
    [SerializeField] private Animator EnemyAnimator;
    [SerializeField] private GameObject EnemyStunParticle;
    [SerializeField] private EnemyRagdollManager RagdollControl;
    [SerializeField] private PickupController TelekinesisModule;
    public bool IsActive;
    public bool IsStunned;
    public float HP;
    private float stuninterval;
    private Rigidbody rb;
    private Collider col;
    private ObjectPickup ObjPickUpController;
    private LevelSystemManagement levelsystem;
    private AISensor aisensor;
    public float lastreceivedphysicforce;

    [Header("Enemy Sound")]
    [SerializeField] private AudioSource EnemySoundPlayer;
    [SerializeField] private AudioSource EnemyMovementSoundPlayer;
    [SerializeField] private List<AudioClip> EnemyDmgSfx;
    [SerializeField] private List<AudioClip> EnemyDeathSfx;
    [SerializeField] private List<AudioClip> EnemyMovementSfx;
    [SerializeField] private List<AudioClip> EnemyCombatChatter;
    [SerializeField] private List<AudioClip> EnemyIdleChatter;
    private float ChatterInterval;

    // Start is called before the first frame update
    void Start()
    {
        aisensor = GetComponent<AISensor>();
        TelekinesisModule = GameObject.FindAnyObjectByType<PickupController>();
        RagdollControl = GetComponentInChildren<EnemyRagdollManager>();
        player = GameObject.FindAnyObjectByType<PlayerBehavior>();
        levelsystem = GameObject.FindAnyObjectByType<LevelSystemManagement>();
        EnemyNavMeshAgent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        ObjPickUpController = GetComponent<ObjectPickup>();
        EnemyAnimator = GetComponentInChildren<Animator>();
        col = GetComponent<Collider>();
        HPIndicatorObj = HPindicator.gameObject;
        IsActive = true;
        IsStunned = false;
        HP = MaxHP;
        defaultmovespeed = movespeed;
        HPindicator.maxValue = HP;
    }

    // Update is called once per frame
    void Update()
    {

        if (IsActive == true) // Check if Enemy state = alive
        {
            EnemyAnimator.SetBool("IsDead", false);

            if(IsStunned == false)
            {
                EnemyStunParticle.SetActive(false);
            }
            
        }

        //Death Check
        if (HP <= 0 && IsActive == true)
        {

            Death();

        }

        if (IsStunned == true)
        {
            gameObject.tag = "PhysicObj";
            EnemyNavMeshAgent.enabled = false;
            EnemyAnimator.SetBool("IsStunned", true);
            EnemyStunParticle.SetActive(true);
            ObjPickUpController.IsAllowPick = true;
            stuninterval -= 1 * Time.deltaTime;
            if (stuninterval <= 0)
            {
                StunStop();
            }
        }
        else
        {
            ObjPickUpController.IsAllowPick = false;
        }

        HPindicator.value = HP;
    }

    void StunStop()
    {
        EnemyNavMeshAgent.enabled = true;
        IsStunned = false;
        EnemyAnimator.SetBool("IsStunned", false);
        ObjPickUpController.IsAllowPick = false;
        TelekinesisModule.DropObject();
        gameObject.tag = "Enemy";

    }

    void Death()
    {
        Debug.Log("Enemy Killed");

        HP = 0;
        aisensor.enabled = false;
        StunStop();
        rb.linearVelocity = Vector3.zero;
        rb.isKinematic = true;
        col.enabled = false;
        HPIndicatorObj.SetActive(false);
        IsActive = false;
        TelekinesisModule.DropObject();
        ObjPickUpController.enabled = false;
        gameObject.tag = "Enemy";
        IsStunned = false;
        EnemyStunParticle.SetActive(false);
        EnemyAnimator.SetBool("IsDead", true);
        EnemyAnimator.SetBool("IsWalking", false);
        EnemyAnimator.SetBool("IsAggro", false);
        EnemyAnimator.enabled = false;
        PlaySfx_Death();
        StopMoveSfx();
        levelsystem.killcount += 1;
        levelsystem.killscore += Score;
        RagdollControl.OnDeath(lastreceivedphysicforce);
    }
    public void AlertedVoiceline()
    {
        EnemyAnimator.SetBool("IsAggro", true);
        

        ChatterInterval -= 1 * Time.deltaTime;

        if (ChatterInterval <= 0)
        {
            PlayCombatChatter();
            ChatterInterval = Random.Range(3, 5);
        }
    }

    public void CalmVoiceline()
    {

        ChatterInterval -= 1 * Time.deltaTime;

        if(ChatterInterval <= 0)
        {
            PlayIdleChatter();
            ChatterInterval = Random.Range(10, 15);
        }


    }


    public void Stun()
    {
        IsStunned = true;
        stuninterval = MaxStunTime;
    }

   


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<ObjectPickup>() == true && other.gameObject.CompareTag("PhysicObj"))
        {
           
                ObjectPickup PhysObj = other.gameObject.GetComponent<ObjectPickup>();
                if( PhysObj.playerpicked == true)
                {
                    PlaySfx_Hit();
                    HP -= PhysObj.PhysicsDmg;
                    lastreceivedphysicforce = other.gameObject.GetComponent<ObjectPickup>().TotalForce;
                    EnemyAnimator.SetTrigger("Hit");
                    //Alerted();
                }            
               
        }

        if(other.gameObject.GetComponent<ProjectileBehavior>() == true)
        {
            if (other.gameObject.GetComponent<ObjectPickup>().playerpicked == true || ObjPickUpController.IsPickedUp() == true)
            {
                PlaySfx_Hit();
                ProjectileBehavior projectile = other.gameObject.GetComponent<ProjectileBehavior>();
                ObjectPickup PhysObj = other.gameObject.GetComponent<ObjectPickup>();
                HP -= projectile.FixedDamageValue + PhysObj.PhysicsDmg;
                projectile.OnHit();
                EnemyAnimator.SetTrigger("Hit");
                //Alerted();
            }
            
            
        }
    }

    public bool GetIsActive()
    {
        return IsActive;
    }

   

   
    public void PlaySfx_Hit()
    {
        EnemySoundPlayer.clip = EnemyDmgSfx[Random.Range(0, EnemyDmgSfx.Count)];
        EnemySoundPlayer.Play();
    }

    public void PlayIdleChatter()
    {
        EnemySoundPlayer.clip = EnemyIdleChatter[Random.Range(0, EnemyIdleChatter.Count)];
        EnemySoundPlayer.Play();
    }

    public void PlayCombatChatter()
    {
        EnemySoundPlayer.clip = EnemyCombatChatter[Random.Range(0, EnemyCombatChatter.Count)];
        EnemySoundPlayer.Play();
    }
    public void PlaySfx_Death()
    {
        EnemySoundPlayer.clip = EnemyDeathSfx[Random.Range(0, EnemyDeathSfx.Count)];
        EnemySoundPlayer.Play();
    }

    public void PlaySfx_Move()
    {
        EnemyMovementSoundPlayer.clip = EnemyMovementSfx[Random.Range(0, EnemyMovementSfx.Count)];
        EnemyMovementSoundPlayer.Play();
    }

    public void StopMoveSfx()
    {
        EnemyMovementSoundPlayer.Stop();
    }

    public float ReturnDefaultMoveSpeed()
    {
        return defaultmovespeed;
    }
}
