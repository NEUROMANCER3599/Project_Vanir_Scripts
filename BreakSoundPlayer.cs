using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakSoundPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource AudioPlayer;
    [SerializeField] private List<AudioClip> AudioList;
    // Start is called before the first frame update
    void Start()
    {
        AudioPlayer = GetComponent<AudioSource>();
        AudioPlayer.clip = AudioList[Random.Range(0, AudioList.Count)];
        AudioPlayer.Play();
        Destroy(gameObject,5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
