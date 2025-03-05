using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyRangeBehavior))]
[RequireComponent(typeof(EnemyBehavior))]
public class EnemyTeleportingModule : MonoBehaviour
{
    [Header("Module Parameters")]
    [SerializeField] private float TeleportingPositionRadius;
    [SerializeField] private float MinTimeBeforeTeleportation;
    [SerializeField] private float MaxTimeBeforeTeleportation;

    [Header("Module Components")]
    [SerializeField] private GameObject TeleportationFX;
    private EnemyRangeBehavior EnemyRangeModule;
    private EnemyBehavior EnemyCoreModule;
    private PlayerBehavior player;
    private float TeleportationInterval; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EnemyRangeModule = GetComponent<EnemyRangeBehavior>();
        EnemyCoreModule = GetComponent<EnemyBehavior>();
        player = FindAnyObjectByType<PlayerBehavior>();
        TeleportationInterval = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (EnemyCoreModule.IsActive && !EnemyCoreModule.IsStunned && EnemyRangeModule.CheckPlayerDetection())
        {
            if(TeleportationInterval > 0)
            {
                TeleportationInterval -= 1f * Time.deltaTime;
            }
            else
            {
                CombatTeleportation();
            }
        }
    }

    void CombatTeleportation()
    {
        TeleportationInterval = Random.Range(MinTimeBeforeTeleportation, MaxTimeBeforeTeleportation);
        Instantiate(TeleportationFX, transform.position,Quaternion.identity);
        Vector3 newPos = RandomNavSphere(transform.position, TeleportingPositionRadius, -1);
        Instantiate(TeleportationFX, newPos, Quaternion.identity);
        EnemyCoreModule.transform.position = newPos;
        
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }
}
