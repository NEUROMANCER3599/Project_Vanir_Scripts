using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class InteractablePropSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> PrefabList;
    [SerializeField] private Transform SpawnPoint;
    [SerializeField] private AudioClip InteractSoundClip;
    [SerializeField] private AudioClip LockedSoundClip;
 
    private AudioSource InteractSoundPlayer;
  
    // Start is called before the first frame update
    void Start()
    {
        InteractSoundPlayer = GetComponent<AudioSource>();
    }

 
    public void SpawnObject()
    {
        PlaySfx_Interact();
        Instantiate(PrefabList[Random.Range(0, PrefabList.Count)], SpawnPoint.position, Quaternion.identity);
    }

    public void PlaySfx_Interact()
    {
        InteractSoundPlayer.clip = InteractSoundClip;
        InteractSoundPlayer.Play();
    }

    public void PlaySfx_Locked()
    {
        InteractSoundPlayer.clip = LockedSoundClip;
        InteractSoundPlayer.Play();
    }
}
