using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeySwitchComponent : MonoBehaviour
{
    [Header("Switch Attributes")]
    [SerializeField] private Renderer ObjRenderer;
    [SerializeField] private Material ActivatedMat, DeactivatedMat;
    [SerializeField] private float DeactivateTimer;
    [SerializeField] private bool IsTimed;
    [SerializeField] private TextMeshProUGUI StatusdisplayText;
    [SerializeField] private Color32 deactivatedColor;
    [SerializeField] private Color32 activatedColor;
    [SerializeField] private AudioSource ActivateSoundPlayer;
    public bool isActivated;
    private float interval;

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
        if (isActivated)
        {
            ObjRenderer.material = ActivatedMat;
            StatusdisplayText.color = activatedColor;
            if (IsTimed)
            {
                interval -= 1 * Time.deltaTime;
                StatusdisplayText.text = Mathf.RoundToInt(interval).ToString();

            }
            else
            {
                StatusdisplayText.text = "Key Switch: Activated";
            }
           
        }
        else
        {
            ObjRenderer.material = DeactivatedMat;
            StatusdisplayText.color = deactivatedColor;
            StatusdisplayText.text = "Key Switch: Deactivated";
        }

        if (IsTimed)
        {
            if (interval <= 0)
            {
                AutoDeactivate();
            }
        }
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<ObjectPickup>() == true && other.gameObject.CompareTag("PhysicObj"))
        {
            if (!isActivated)
            {
                ActivateSoundPlayer.Play();
                isActivated = true;

                if (IsTimed)
                {
                    //Invoke(nameof(AutoDeactivate), DeactivateTimer);
                    interval = DeactivateTimer;
                }
            }
           
        }

        if(other.gameObject.tag == "Player")
        {
            if (!isActivated)
            {
                ActivateSoundPlayer.Play();
                isActivated = true;

                if (IsTimed)
                {
                    //Invoke(nameof(AutoDeactivate), DeactivateTimer);
                    interval = DeactivateTimer;
                }
            }
            
            
        }
    }

    private void AutoDeactivate()
    {
        //ActivateSoundPlayer.Play();
        isActivated = false;
    }
}
