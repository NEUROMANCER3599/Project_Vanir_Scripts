using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBreaking : MonoBehaviour
{
    [SerializeField] private GameObject BreakParticle;
    [SerializeField] private GameObject BreakSoundObj;
    

    private void Start()
    {
        /*
        if (gameObject.GetComponent<AudioSource>() != null)
        {
            BreakSoundPlayer = GetComponentInChildren<AudioSource>();
        }
        else
        {
            BreakSoundPlayer = GetComponent<AudioSource>();
        }
        */
    }
    public void Breaking()
    {
        Instantiate(BreakParticle, transform.position, transform.rotation);
        Instantiate(BreakSoundObj, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }
}
