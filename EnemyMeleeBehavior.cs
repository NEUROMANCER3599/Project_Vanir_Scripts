using UnityEngine;
using UnityEngine.AI;

public class EnemyMeleeBehavior : MonoBehaviour
{
    [Header("Movement Parameters (Customizable)")]
    [SerializeField] private bool IsWandering;
    [SerializeField] private bool IsThrowable;

    [Header("Movement & Attack Stats (Customizable)")]
    [SerializeField] private float AttackRadius = 1;
    private float movespeed = 5;
    [SerializeField] private float rotspeedModifier = 3;
    [SerializeField] private float wanderRadius;
    [SerializeField] private float MeleeDamage;
    [SerializeField] private float MeleeDuration = 1.2f;

    [Header("Movement System Components")]
    [SerializeField] private GameObject EnemyWeapon;
    [SerializeField] private Animator EnemyAnimator;
    [SerializeField] private EnemyMeleeWeapBehavior MeleeWeap;
    private AISensor sensor;
    private PlayerBehavior player;
    private NavMeshAgent EnemyNavMeshAgent;
    private PickupController TelekinesisModule;
    private float Circlingspeed;
    private float rotspeed;
    private float CDirChange = 1;
    private bool IsFiring = false;
    private bool CirclingDirChange = false;
    private bool PlayerSpotted = false;
    private float wanderinginterval;
    private float scanninginterval;
    private float defaultsensordistance;
    private float defaultsensorangle;
    private float defaultmovespeed;
    private float wanderTimer;
    private float ScanningTimer;
    private ObjectPickup ObjPickUpController;
    private EnemyBehavior EnemyModule;
    private Vector3 Direction;
    private Quaternion rotation;
    private Quaternion AimRotation;
    private float Distance;
    private float MeleeInterval = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MeleeWeap.SetDmg(MeleeDamage);
        MeleeWeap.SetAtkDuration(MeleeDuration);
        sensor = GetComponent<AISensor>();
        player = FindAnyObjectByType<PlayerBehavior>();
        EnemyNavMeshAgent = GetComponent<NavMeshAgent>();
        TelekinesisModule = FindAnyObjectByType<PickupController>();
        ObjPickUpController = GetComponent<ObjectPickup>();
        EnemyModule = GetComponent<EnemyBehavior>();
        movespeed = EnemyModule.movespeed;
        PlayerSpotted = false;
        EnemyNavMeshAgent.speed = movespeed;
        defaultsensorangle = sensor.Detectangle;
        defaultsensordistance = sensor.distance;
        defaultmovespeed = movespeed;
        

        if (IsThrowable)
        {
            ObjPickUpController.IsPickable = true;
        }
        else
        {
            ObjPickUpController.IsPickable = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //NavMeshSpeed Setting
        movespeed = EnemyModule.movespeed;
        EnemyNavMeshAgent.speed = movespeed;

        //Turning & Aiming At Player
        Direction = player.orientation.transform.position - transform.position;
        rotation = Quaternion.LookRotation(new Vector3(Direction.x, 0, Direction.z));
        AimRotation = Quaternion.LookRotation(Direction);
        Distance = Vector3.Distance(transform.position, player.transform.position);

        //Idle Scanning Rotation Speed Setting
        rotspeed = movespeed * rotspeedModifier;

        if (EnemyModule.IsActive) //Death Check
        {
            if (!EnemyModule.IsStunned) //Stunned Check
            {
                if (PlayerSpotted)
                {
                    Aggro();
                    EnemyModule.AlertedVoiceline();
                }
                else
                {
                    ScanningTarget();
                    if (IsWandering == true)
                    {
                        IdleMovement();
                        EnemyModule.CalmVoiceline();
                    }
                }
            }

            //Player Detection Check
            if (sensor.IsInSight(player.gameObject))
            {
                PlayerSpotted = true;
            }

            if (!sensor.IsInSight(player.gameObject))
            {
                PlayerSpotted = false;
            }
        }
    }

    void Aggro()
    {
        transform.rotation = rotation;

        scanninginterval = 0;
        wanderinginterval = 0;

        movespeed = defaultmovespeed * 1.5f;
        sensor.Detectangle = 180;
        sensor.distance = defaultsensordistance * 1.5f;

        if (Distance > AttackRadius)
        {

            if (ObjPickUpController.IsPickedUp() == false)
            {

                EnemyNavMeshAgent.isStopped = false;

                EnemyNavMeshAgent.SetDestination(player.transform.position);

                EnemyAnimator.SetBool("IsWalking", true);

            }



        }
        else if (Distance <= AttackRadius)
        {

            if (ObjPickUpController.IsPickedUp() == false)
            {
                //StopMoveSfx();
                EnemyNavMeshAgent.isStopped = true;
                EnemyAnimator.SetBool("IsWalking", false);

                if (sensor.IsInSight(player.gameObject))
                {
                    if(MeleeInterval <= 0)
                    {
                        EnemyAnimator.SetTrigger("Attacking");
                        MeleeWeap.OnAttacking();
                        MeleeInterval = MeleeDuration;
                    }
                }
            }
        }

        if(MeleeInterval > 0)
        {
            MeleeInterval -= 1f * Time.deltaTime;
        }
        
    }



    void IdleMovement()
    {
        EnemyAnimator.SetBool("IsAggro", false);
        movespeed = defaultmovespeed;
        sensor.Detectangle = defaultsensorangle;
        sensor.distance = defaultsensordistance;

        wanderinginterval += Time.deltaTime;
        EnemyAnimator.SetBool("IsWalking", true);

        if (wanderinginterval >= wanderTimer)
        {
            //StopMoveSfx();
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            EnemyNavMeshAgent.SetDestination(newPos);
            wanderinginterval = 0;
            wanderTimer = Random.Range(1, 5);
        }

    }

    void ScanningTarget()
    {
        int lrchooser = 0;
        float dirmodifier = 1;

        scanninginterval += Time.deltaTime;

        if (scanninginterval >= ScanningTimer)
        {
            lrchooser = Random.Range(0, 3);
            switch (lrchooser)
            {
                case 0: dirmodifier = 1; break;
                case 1: dirmodifier = -1; break;
                case 2: dirmodifier = 1; break;
                case 3: dirmodifier = -1; break;
            }
            scanninginterval = 0;
            ScanningTimer = Random.Range(1, 10);
        }

        transform.Rotate(new Vector3(0, 1, 0), (rotspeed * dirmodifier) * Time.deltaTime);


    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }

    public float GetDefaultMoveSpeed()
    {
        return defaultmovespeed;
    }

    public bool CheckPlayerDetection()
    {
        return PlayerSpotted;
    }
}
