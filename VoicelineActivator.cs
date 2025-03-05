using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class VoicelineActivator : MonoBehaviour
{
    [SerializeField] private List<VoicelineSequence> VoicelineSequences;
    [SerializeField] private VoicelineManager VoicelineManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        VoicelineManager = GameObject.FindAnyObjectByType<VoicelineManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerBehavior>() != null)
        {
            VoicelineManager.PlaySeq(VoicelineSequences);
            Destroy(this.gameObject);
        }
    }
}
