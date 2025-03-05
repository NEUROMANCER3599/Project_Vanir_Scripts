using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class InteractionModule : MonoBehaviour
{
    [Header("Module Parameters")]
    public UnityEvent InteractionEvent;
    public UnityEvent ActivatedEvents;
    public UnityEvent DeniedEvents;
    public bool Is_Infinitely_Repeatable;
    public int Activate_Count;
    public int Activate_CooldownDuration;
    private float Activate_CooldownInterval;

    private void Start()
    {
        Activate_CooldownInterval = 0;
    }
    public void EventRaise()
    {
        InteractionEvent.Invoke();
    }
    public void OnInteracted()
    {
        if (Is_Infinitely_Repeatable)
        {
            if(Activate_CooldownInterval <= 0)
            {

                ActivatedEvents.Invoke();
                   
                
                Activate_CooldownInterval = Activate_CooldownDuration;
            }
            else
            {
                //Denied Event Here
                
                    DeniedEvents.Invoke();

            }
        }
        else
        {
            if(Activate_Count > 0)
            {
                if( Activate_CooldownInterval <= 0)
                {
                   
                        ActivatedEvents.Invoke();

                    Activate_CooldownInterval = Activate_CooldownDuration;
                    Activate_Count--;
                }
                else
                {
                    //Denied Event Here
                  
                        DeniedEvents.Invoke();

                }
            }
            else
            {
                //Denied Event Here
                
                    DeniedEvents.Invoke();

            }
        }
       
    }

    private void Update()
    {
        if(Activate_CooldownInterval > 0)
        {
            Activate_CooldownInterval -= 1f * Activate_CooldownInterval;
        }
    }
}
