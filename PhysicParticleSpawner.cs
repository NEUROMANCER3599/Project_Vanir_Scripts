using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicParticleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject PhysicParticle;
    [SerializeField] private AudioSource ImpactSoundPlayer;
    [SerializeField] private List<AudioClip> ImpactSounds;
    public bool SoundOnlyMode = false;
    // Start is called before the first frame update
    void Start()
    {
        /*
        if(gameObject.GetComponent<AudioSource>() != null)
        {
            ImpactSoundPlayer = GetComponentInChildren<AudioSource>();
        }
        else
        {
            ImpactSoundPlayer = GetComponent<AudioSource>();
        }
        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!SoundOnlyMode)
        {
            Instantiate(PhysicParticle, transform.position, transform.rotation);
        }
        ImpactSoundPlayer.clip = ImpactSounds[Random.Range(0, ImpactSounds.Count)];
        ImpactSoundPlayer.Play();
    }
}
