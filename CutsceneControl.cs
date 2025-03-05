using UnityEngine;
using UnityEngine.Video;
using System.Collections.Generic;

public class CutsceneControl : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private VideoPlayer CutscenePlayer;
    [SerializeField] private LevelSystemManagement levelSystem;
    [Header("Cutscene Seq")]
    [SerializeField] private List<VideoClip> CutsceneClipSeqs;
    private bool IsSeqsFinished = false;
    private int SeqIndex = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        levelSystem = GameObject.FindAnyObjectByType<LevelSystemManagement>();
        CutscenePlayer = GetComponent<VideoPlayer>();
        IsSeqsFinished = false;
        SeqIndex = 0;
        DisplaySeq();
    }

    void Update()
    {
       
            if (Input.anyKeyDown)
            {
                if (!IsSeqsFinished)
                {
                    SeqIndex += 1;
                    DisplaySeq();
                }
                else
                {
                    levelSystem.OnNextLevel();
                }
            }
        
    }

    void DisplaySeq()
    {
        if(SeqIndex < CutsceneClipSeqs.Count)
        {
            CutscenePlayer.clip = CutsceneClipSeqs[SeqIndex];
            CutscenePlayer.Play();
        }
        else
        {
            IsSeqsFinished=true;
        }
       
    }
}
