using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class PlayerVirtualCamController : MonoBehaviour
{
    [SerializeField] private Cinemachine.CinemachineVirtualCamera VirtualCamComponent;
    [SerializeField] private Transform RootCameraPosition;
    [SerializeField] private float NormalFOV = 60;
    [SerializeField] private float SprintFOV = 90;
    [SerializeField] private Volume CurrentVolume;

    private UnityEngine.Rendering.Universal.ChromaticAberration chromaticAberration;
    private PlayerBehavior playerSystem;
    // Start is called before the first frame update
    void Start()
    {
        VirtualCamComponent = GetComponent<CinemachineVirtualCamera>();
        playerSystem = GetComponent<PlayerBehavior>();
        RootCameraPosition = GameObject.Find("PlayerCameraGroup").GetComponent<Transform>();
        //CurrentVolume = GameObject.FindAnyObjectByType<Volume>();
        VirtualCamComponent.Follow = RootCameraPosition;
    }

    // Update is called once per frame
    void Update()
    {
        //Sprinting FOV Change
        if (Input.GetKey(KeyCode.LeftShift)) 
        {
            if(VirtualCamComponent.m_Lens.FieldOfView < SprintFOV)
            {
                VirtualCamComponent.m_Lens.FieldOfView += 30f * Time.deltaTime;
            }
        }
        else
        {
            if (VirtualCamComponent.m_Lens.FieldOfView > NormalFOV)
            {
                VirtualCamComponent.m_Lens.FieldOfView -= 30f*Time.deltaTime;
            }
        }

        if(VirtualCamComponent.m_Lens.FieldOfView < NormalFOV)
        {
            VirtualCamComponent.m_Lens.FieldOfView = NormalFOV;
        }
        if(VirtualCamComponent.m_Lens.FieldOfView > SprintFOV)
        {
            VirtualCamComponent.m_Lens.FieldOfView = SprintFOV;
        }


        /*
        //Chromatics For Power
        if(Input.GetMouseButton(0) && playerSystem.IsPowerDepleted == false)
        {
            float increment = 1;
            increment += increment * Time.deltaTime;
            //CurrentVolume.GetComponent<ChromaticAberration>().intensity.Override(increment);
        }
        else
        {
            float decrement = 1;
            decrement -= decrement * Time.deltaTime;
            //chromaticAberration.intensity.Override(decrement);
            //CurrentVolume.GetComponent<ChromaticAberration>().intensity.Override(decrement);
        }
        */

    }
}
