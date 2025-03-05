using UnityEngine;
using UnityEngine.AI;

public class EnemyRangeBehavior : MonoBehaviour
{
    [Header("Movement Parameters (Customizable)")]
    [SerializeField] private bool IsStationary;
    [SerializeField] private bool IsWandering;
    [SerializeField] private bool IsDodging;
    [SerializeField] private bool IsThrowable;

    [Header("Movement & Attack Stats (Customizable)")]
    [SerializeField] private float AttackRadius = 10;
    private float movespeed = 5;
    [SerializeField] private float rotspeedModifier = 3;
    [SerializeField] private float CirclingSpeedModifier = 2;
    [SerializeField] private float Firerate = 1;
    [SerializeField] private float wanderRadius;
    [SerializeField] private GameObject Projectile;

    [Header("Movement System Components")]
    [SerializeField] private GameObject EnemyGun;
    [SerializeField] private Animator EnemyAnimator;
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
    private float defaultfirerate;
    private float wanderTimer;
    private float ScanningTimer;
    private ObjectPickup ObjPickUpController;
    private EnemyBehavior EnemyModule;
    private Vector3 Direction;
    private Quaternion rotation;
    private Quaternion AimRotation;
    private float Distance;

    void Start()
    {

        //Initialize Parameters

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
        defaultfirerate = Firerate;

        if (IsThrowable)
        {
            ObjPickUpController.IsPickable = true;
        }
        else
        {
            ObjPickUpController.IsPickable = false;
        }


    }

    
    void Update()
    {

        //NavMeshSpeed Setting
        movespeed = EnemyModule.movespeed;
        EnemyNavMeshAgent.speed = movespeed;

        //Turning & Aiming At Player
        Direction = player.orientation.transform.position - EnemyGun.transform.position;
        rotation = Quaternion.LookRotation(new Vector3(Direction.x, 0, Direction.z));
        AimRotation = Quaternion.LookRotation(Direction);
        Distance = Vector3.Distance(transform.position, player.transform.position);

        //Idle Scanning Rotation Speed Setting
        rotspeed = movespeed * rotspeedModifier;


        //Near Death Adrenaline
        /*
        if (EnemyModule.HP < EnemyModule.MaxHP * 0.35f) 
        {
            Firerate = defaultfirerate * 0.5f;
            Circlingspeed = movespeed * (CirclingSpeedModifier * 2f);
        }
        else
        {
            Circlingspeed = movespeed * CirclingSpeedModifier;
            Firerate = defaultfirerate;
        }
        */

        Circlingspeed = movespeed * CirclingSpeedModifier;
        Firerate = defaultfirerate;

        if (EnemyModule.IsActive) //Death Check
        {
            if(!EnemyModule.IsStunned) //Stunned Check
            {
                if (PlayerSpotted)
                {
                    Aggro();
                    EnemyModule.AlertedVoiceline();
                }
                else
                {
                    ScanningTarget();
                    if (IsWandering == true && IsStationary == false)
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
        EnemyGun.transform.rotation = AimRotation;

        scanninginterval = 0;
        wanderinginterval = 0;

        movespeed = defaultmovespeed * 1.5f;
        sensor.Detectangle = 180;
        sensor.distance = defaultsensordistance * 1.5f;

        if (Distance > AttackRadius)
        {

            if (IsStationary == false && ObjPickUpController.IsPickedUp() == false)
            {

                EnemyNavMeshAgent.isStopped = false;

                EnemyNavMeshAgent.SetDestination(player.transform.position);

                EnemyAnimator.SetBool("IsWalking", true);
               
            }



        }
        else if (Distance <= AttackRadius)
        {
            
            if (IsStationary == false && IsDodging == false && ObjPickUpController.IsPickedUp() == false)
            {
                //StopMoveSfx();
                EnemyNavMeshAgent.isStopped = true;
                EnemyAnimator.SetBool("IsWalking", false);
            }
            else if (IsStationary == false && IsDodging == true && ObjPickUpController.IsPickedUp() == false)
            {
                //PlaySfx_Move();
                EnemyNavMeshAgent.isStopped = true;
                EnemyAnimator.SetBool("IsWalking", true);
                transform.RotateAround(player.transform.position, Vector3.up, CDirChange * Circlingspeed * Time.deltaTime);
            }
           

        }

        if (sensor.IsInSight(player.gameObject))
        {
            Attacking();
        }

    }

    void Attacking()
    {
        if (IsFiring == false && ObjPickUpController.IsPickedUp() == false)
        {
            Invoke(nameof(FireProjectile), Firerate);
            IsFiring = true;
        }
    }

    void FireProjectile()
    {
        EnemyAnimator.SetTrigger("Attacking");

        Debug.Log("Weapon fired!");
        var bullet = Instantiate(Projectile, EnemyGun.transform.position, Quaternion.identity);

        //bullet.GetComponent<Rigidbody>().useGravity = false;
        bullet.GetComponent<Rigidbody>().linearVelocity = EnemyGun.transform.forward * bullet.GetComponent<ProjectileBehavior>().ProjectileSpeed;

        IsFiring = false;
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

    void CirclingDirection()
    {
        if (CDirChange == 1)
        {
            CDirChange = -1;
        }
        else
        {
            CDirChange = 1;
        }
        CirclingDirChange = false;
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
