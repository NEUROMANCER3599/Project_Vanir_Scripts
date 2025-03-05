using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeightKeyComponent : MonoBehaviour
{
    [Header("Switch Attributes")]
    [SerializeField] private Renderer ObjRenderer;
    [SerializeField] private Material ActivatedMat, DeactivatedMat;
    [Header("Switch Parameter")]
    [SerializeField] private float RequiredWeight;
    [SerializeField] private float CurrentWeight;
    [SerializeField] private bool PreciseWeighting;
    public bool isActivated;
    [Header("Switch Components")]
    [SerializeField] private TextMeshProUGUI StatusText;
    [SerializeField] private Color32 deactivatedColor;
    [SerializeField] private Color32 activatedColor;
    [SerializeField] private AudioSource ActivateSoundPlayer;




    // Start is called before the first frame update
    void Start()
    {
        ObjRenderer = GetComponent<Renderer>();
        ActivateSoundPlayer = GetComponent<AudioSource>();
        isActivated = false;
    }

    // Update is called once per frame
    void Update()
    {
        StatusText.text = "Weight: " + CurrentWeight + " / " + RequiredWeight + " Kg.";
        if (PreciseWeighting == true)
        {
            if (CurrentWeight == RequiredWeight)
            {
                if (!isActivated)
                {
                    ActivateSoundPlayer.Play();
                }
                isActivated = true;
                ObjRenderer.material = ActivatedMat;
                StatusText.color = activatedColor;
            }
            else
            {
                isActivated = false;
                ObjRenderer.material = DeactivatedMat;
                StatusText.color = deactivatedColor;
            }
        }
        else
        {
            if (CurrentWeight >= RequiredWeight)
            {
                if (!isActivated)
                {
                    ActivateSoundPlayer.Play();
                }
                isActivated = true;
                ObjRenderer.material = ActivatedMat;
                StatusText.color = activatedColor;
            }
            else
            {
                isActivated = false;
                ObjRenderer.material = DeactivatedMat;
                StatusText.color = deactivatedColor;
            }
        }
       
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.GetComponent<Rigidbody>() != null && other.gameObject.tag == "PhysicObj" )
        {
            
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            CurrentWeight += rb.mass;
        }

    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.GetComponent<Rigidbody>() != null && other.gameObject.tag == "PhysicObj")
        {
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();

            CurrentWeight -= rb.mass;
        }
    }
}
