using UnityEngine;


public class LaserBeamObstacleBehavior : MonoBehaviour
{
    [Header("Components & Parameters")]
    [SerializeField] private LineRenderer LaserBeam;
    [SerializeField] private GameObject BeamOrigin;
    [SerializeField] private GameObject LaserSparkVFX;
    [SerializeField] private float DamagePerSecs;
    [SerializeField] private float MaxBeamRange;
    [SerializeField] private bool IsDamagingProp;
    [SerializeField] private float JumpForceModifier;
    [SerializeField] private float MoveSpeedModifier;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit HitTarget;
        LaserBeam.SetPosition(0, Vector3.zero);

        if (Physics.Raycast(BeamOrigin.transform.position,BeamOrigin.transform.forward,out HitTarget,MaxBeamRange))
        {

            LaserBeam.SetPosition(1, new Vector3(0,0,HitTarget.distance));
            Damaging(HitTarget.transform.gameObject);
            Debug.Log("Beam hit at: " + HitTarget.transform.gameObject.name);
            LaserSparkVFX.transform.localPosition = new Vector3(0, 0, HitTarget.distance);
        }
        else
        {
            LaserBeam.SetPosition(1, new Vector3(0, 0, MaxBeamRange));
            LaserSparkVFX.transform.localPosition = new Vector3(0, 0, MaxBeamRange);
        }
    }

    void Damaging(GameObject Target)
    {
        if (Target.GetComponent<EnemyBehavior>() != null)
        {
            //Debug.Log("Beam hit at Enemy!");
            EnemyBehavior enemyTarget = Target.GetComponent<EnemyBehavior>();
            enemyTarget.HP -= DamagePerSecs * Time.deltaTime;
            enemyTarget.movespeed = enemyTarget.movespeed * MoveSpeedModifier;
        }
        else if (Target.GetComponent<PlayerBehavior>() != null)
        {
            //Debug.Log("Beam hit at Player!");
            PlayerBehavior playerTarget = Target.GetComponent<PlayerBehavior>();
            playerTarget.HP -= DamagePerSecs * Time.deltaTime;
            playerTarget.PlayerMovementComponent.moveSpeed *= MoveSpeedModifier;
            playerTarget.PlayerMovementComponent.jumpForce *= JumpForceModifier;
        }
        else if(Target.GetComponent<ObjectPickup>() != null && IsDamagingProp)
        {
           ObjectPickup PropTarget = Target.GetComponent<ObjectPickup>();
            PropTarget.HP -= DamagePerSecs * Time.deltaTime;
        }
       
    }
}
