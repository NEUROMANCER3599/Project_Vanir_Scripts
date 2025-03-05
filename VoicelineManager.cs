using System.Collections.Generic;
using UnityEngine;

public class VoicelineManager : MonoBehaviour
{
    [SerializeField] private AudioSource VoicelinePlayer;
    [SerializeField] private List<VoicelineSequence> VoicelinePlaylist;
    private int ListIndex;
    private int MaxIndex;
    private float VoicelineDuration;
    private string VoicelineScript;
    private UIManager UIControl;

    private void Start()
    {
        VoicelinePlayer = GetComponent<AudioSource>();
        UIControl = Object.FindAnyObjectByType<UIManager>();
    }

    private void Update()
    {
        if(VoicelinePlaylist != null)
        {
            if (!VoicelinePlayer.isPlaying)
            {
                    if(ListIndex < MaxIndex)
                    {
                        VoicelinePlayer.clip = VoicelinePlaylist[ListIndex].VoiceClip;
                        UIControl.SubtitleText.text = VoicelinePlaylist[ListIndex].VoiceScript;
                        VoicelinePlayer.Play();
                        Invoke(nameof(NextClip), VoicelinePlayer.clip.length);
                    }
                    
            }
            
           

        }

       
    }
    public void PlaySeq(List<VoicelineSequence> VoiceSeqs)
    {
        if(VoicelinePlaylist == null)
        {
            ListIndex = 0;
        }
        for(int i = 0; i < VoiceSeqs.Count; i++)
        {
            VoicelinePlaylist.Add(VoiceSeqs[i]);
        }

        MaxIndex = VoicelinePlaylist.Count;
    }

    public void NextClip()
    {
        UIControl.SubtitleText.text = "";
        ListIndex++;
        
    }
}
