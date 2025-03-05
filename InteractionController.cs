using UnityEngine;
using UnityEngine.Events;

public class InteractionController : MonoBehaviour
{
    [Header("Controller Parameters")]
    public float ActivationRange = 3f;
    public KeyCode ActivationKey = KeyCode.F;
    private InteractionModule ActivatedObjModule;
    private DisplayTextComponent ObjDescModule;
    private UIManager UIControl;


    private void Start()
    {
        UIControl = GameObject.FindAnyObjectByType<UIManager>();
    }

    void Update()
    {
        RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, ActivationRange))
            {
               
                InspectObject(hit.transform.gameObject);
                
            }
            else
            {
                if(ObjDescModule != null)
                {
                    ObjDescModule.OnLookedAway();
                }

            }

            if (Input.GetKeyDown(ActivationKey))
            {
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, ActivationRange))
                {
                    ActivateObject(hit.transform.gameObject);
                }
                
            }

    }

    void ActivateObject(GameObject ActivatedObj)
    {
        if(ActivatedObj.GetComponent<InteractionModule>())
        {
            ActivatedObjModule = ActivatedObj.GetComponent<InteractionModule>();
            //ActivatedObjModule.OnInteracted();
            ActivatedObjModule.EventRaise();
            ActivatedObjModule = null;
        }
    }

    void InspectObject(GameObject InspectedObj)
    {
        if (InspectedObj.GetComponent<DisplayTextComponent>())
        {
            ObjDescModule = InspectedObj.GetComponent<DisplayTextComponent>();
            ObjDescModule.OnInspected();
        }
        else
        {
            if(ObjDescModule != null)
            {
                ObjDescModule.OnLookedAway();
            }
        }
    }
}
