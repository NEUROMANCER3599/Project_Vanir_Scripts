using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ObjectPickup : MonoBehaviour
{

    [SerializeField] private Transform objTransform;
    [SerializeField] private Rigidbody objRigidbody;
    [SerializeField] private float throwAmount;
    [SerializeField] private PlayerBehavior playerSystem;

    
    


    [Header("Object Attributes")]
    public float PhysicsDmg;
    public float HP;
    public float TotalForce;
    public bool IsPickable = true;
    public bool IsAllowPick = true;
    public bool playerpicked = false;
    public bool pickedup = false;

    [Header("Components")]
    [SerializeField] private ObjectBreaking BreakingComponent;

    private HoldPointAdjustment HoldPointComponent;
    void Start()
    {
        
        HoldPointComponent = GameObject.FindAnyObjectByType<HoldPointAdjustment>();
        objTransform = GetComponent<Transform>();
        objRigidbody = objTransform.GetComponent<Rigidbody>();
        playerSystem = GameObject.FindAnyObjectByType<PlayerBehavior>();
        BreakingComponent = GetComponent<ObjectBreaking>();

        
    }
    /*
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            UIControl.CrosshairControl(true);
            UIControl.ObjMassDisplay(objRigidbody, true);
            playerSystem.IsTargetingValidObj = true;
            if(playerSystem.IsPowerDepleted == false)
            {
                //interactable = true;
            }
            
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            playerSystem.IsTargetingValidObj = false;
            if (pickedup == false)
            {
                UIControl.CrosshairControl(false);
                UIControl.ObjMassDisplay(objRigidbody, false);
                //interactable = false;
            }
            if (pickedup == true)
            {
                objTransform.parent = null;
                objRigidbody.useGravity = true;
                UIControl.CrosshairControl(false);
                UIControl.ObjMassDisplay(objRigidbody, false);
                //interactable = false;
                pickedup = false;
            }
        }
    }
  */
    void Update()
    {

        TotalForce = objRigidbody.mass * objRigidbody.linearVelocity.magnitude;
        PhysicsDmg = (objRigidbody.mass * objRigidbody.linearVelocity.magnitude) * 1.5f;
        PhysicsDmg = Mathf.RoundToInt(PhysicsDmg);

        if (HP <= 0 && BreakingComponent != null)
        {
            BreakingComponent.Breaking();
            playerSystem.IsTargetingValidObj = false;
        }


    }


    public bool IsPickedUp()
    {
        return pickedup;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Wall"))
        {
            playerpicked = false;
            HP -= Mathf.RoundToInt(PhysicsDmg / 2);
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            HP -= Mathf.RoundToInt(PhysicsDmg / 2);
        }

        if(collision.gameObject.GetComponent<ProjectileBehavior>() != null)
        {
            ProjectileBehavior collideProjectile = collision.gameObject.GetComponent<ProjectileBehavior>();
            HP -= collideProjectile.FixedDamageValue / 2;
            collideProjectile.OnHit();
        }
       
    }
}
