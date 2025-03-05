using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorActivator : MonoBehaviour
{
    [SerializeField] private Elevator ConnectedElevator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnActivated()
    {
        if (!ConnectedElevator.IsActivated)
        {
            ConnectedElevator.levelSystem.PlayGenericClickSound();
            ConnectedElevator.ElevatorCondition();
            ConnectedElevator.IsActivated = true;
        }
    }
    
}
