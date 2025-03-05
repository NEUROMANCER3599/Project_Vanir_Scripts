
using Unity.VisualScripting;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    [Header("Pickup Settings")]
    [SerializeField] Transform HoldArea;

    [Header("Debugging : Objects Status")]
    [SerializeField] private GameObject HeldObj;
    [SerializeField] private Rigidbody heldObjRB;
    [SerializeField] private ObjectPickup HeldObjAttributes;

    [Header("Physics Parameters")]
    [SerializeField] private float pickupRange = 5f;
    [SerializeField] private float defaultpickupForce = 150.0f;
    [SerializeField] private float PushPower;
    private float pickupForce = 150.0f;

    [Header("System References")]
    [SerializeField] private PlayerBehavior playerSystem;
    public HoldPointAdjustment HoldPointComponent;
    [SerializeField] private UIManager UIControl;
    private bool IsMovingObject;

    private void Start()
    {
        playerSystem = GameObject.FindAnyObjectByType<PlayerBehavior>();
        HoldPointComponent = GameObject.FindAnyObjectByType<HoldPointAdjustment>();
        UIControl = GameObject.FindAnyObjectByType<UIManager>();
        IsMovingObject = false;
    }

    private void Update()
    {

        pickupRange = 5 + (playerSystem.PickupRangeBonus * 0.5f);

        RaycastHit hit;


        //===== Telekinesis Control Checks =====
        if (Input.GetMouseButton(0) && playerSystem.IsPowerDepleted == false) // Picking Up Check
        {
            if(HeldObj == null)
            {
               
                
                if(Physics.Raycast(transform.position,transform.TransformDirection(Vector3.forward),out hit, pickupRange))
                {
                        
                            
                            PickUpObject(hit.transform.gameObject);
                            

                }
            }
            else
            {
                MoveObject();
                
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if(HeldObj != null)
            {
                if (playerSystem.PushForce > 0)
                {
                    ThrowingObj();
                }
                else
                {
                    
                    DropObject();
                }
            }
           
        }

        if (playerSystem.IsPowerDepleted == true)
        {
            DropObject();
        }

        if(HeldObj != null & heldObjRB != null)
        {
            if(heldObjRB.mass > 1)
            {
                PushPower = (playerSystem.PushForce * 1250f) * heldObjRB.mass;
            }
            else
            {
                PushPower = playerSystem.PushForce * 1250f;
            }
        }
        //======================================

        //===== Object Inspection =====
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickupRange))
        {
            OnInspectObject(hit.transform.gameObject);
        }
        else
        {
            if (!IsMovingObject)
            {
                OnLookAway();
            }
        }
        
        
    }

    void PickUpObject(GameObject PickObj)
    {
        if (PickObj.GetComponent<Rigidbody>() && PickObj.GetComponent<ObjectPickup>())
        {

            playerSystem.PlayAudio_AttackStart();
            heldObjRB = PickObj.GetComponent<Rigidbody>();
            HeldObjAttributes = PickObj.GetComponent<ObjectPickup>();

            playerSystem.IsTargetingValidObj = true;
            HeldObjAttributes.pickedup = true;
            HeldObjAttributes.playerpicked = true;
            playerSystem.HoldingObjMass = heldObjRB.mass;
            heldObjRB.useGravity = false;
            heldObjRB.linearDamping = 10;
            heldObjRB.constraints = RigidbodyConstraints.FreezeRotation;
            heldObjRB.transform.parent = HoldArea;
            HeldObj = PickObj;
        }
    }

    public void DropObject()
    {
            if(HeldObj != null)
            {
                playerSystem.PlayAudio_PowerStop();
                HeldObjAttributes.pickedup = false;
                heldObjRB.useGravity = true;
                heldObjRB.linearDamping = 1;
                heldObjRB.constraints = RigidbodyConstraints.None;
                heldObjRB.transform.parent = null;
                HeldObj = null;
                IsMovingObject = false;
            }
           
        
    }

    void MoveObject()
    {
        if(Vector3.Distance(HeldObj.transform.position, HoldArea.position) > 0.1f)
        {
            playerSystem.IsTargetingValidObj = true;
            pickupForce = defaultpickupForce * heldObjRB.mass;
            Vector3 moveDirection = HoldArea.position - HeldObj.transform.position;
            heldObjRB.AddForce(moveDirection * pickupForce);
            UIControl.ObjMassDisplay(heldObjRB, true);
            IsMovingObject = true;
        }
    }

    public void ThrowingObj()
    {
        playerSystem.StopAttackAudio();
 
        HoldPointComponent.PushBlast();
        UIControl.HandPushAnimTrigger();
        playerSystem.HoldingObjMass = 0;

        if (this.GetComponent<ProjectileBehavior>() == true)
        {
            heldObjRB.useGravity = false;
        }
        else
        {
            heldObjRB.useGravity = true;
        }

        heldObjRB.linearDamping = 1;
        heldObjRB.constraints = RigidbodyConstraints.None;
        heldObjRB.transform.parent = null;
        HeldObj = null;
        heldObjRB.AddForce(HoldArea.transform.forward * PushPower, ForceMode.Force);
        HeldObjAttributes.pickedup = false;
        HeldObjAttributes.playerpicked = true;
        IsMovingObject = false;
    }

    public void OnInspectObject(GameObject InspectedObj)
    {
        if(InspectedObj.GetComponent<Rigidbody>() && InspectedObj.GetComponent<ObjectPickup>()){

            //heldObjRB = InspectedObj.AddComponent<Rigidbody>();
            UIControl.CrosshairControl(true);
            playerSystem.IsTargetingValidObj = true;
           
            

        }
        /*
        else
        {
            if (!IsMovingObject)
            {
                OnLookAway();
            }
             
        }*/
        
    }

    public void OnLookAway()
    {
        UIControl.CrosshairControl(false);
      
        playerSystem.IsTargetingValidObj = false;
        
        UIControl.ObjMassDisplay(heldObjRB, false);

    }
}
