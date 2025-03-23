using UnityEngine;
using UnityEngine.Video;
using System.Collections.Generic;
using UnityEngine.UI;

public class CutsceneControl : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private VideoPlayer CutscenePlayer;
    [SerializeField] private LevelSystemManagement levelSystem;
    [SerializeField] private GameObject TextPrompt;
    [SerializeField] private Slider TimeSlider;
    [Header("Cutscene Seq")]
    [SerializeField] private List<VideoClip> CutsceneClipSeqs;
    private bool IsSeqsFinished = false;
    private int SeqIndex = 0;
    private float SeqInterval = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TextPrompt.SetActive(false);
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

            if(SeqInterval > 0)
            {
                SeqInterval -= 1f * Time.deltaTime;
                TimeSlider.value = SeqInterval;
                TextPrompt.SetActive(false);
                TimeSlider.gameObject.SetActive(true);
            }
            else
            {
                TextPrompt.SetActive(true);
                TimeSlider.gameObject.SetActive(false);
            }
        
    }

    void DisplaySeq()
    {
        if(SeqIndex < CutsceneClipSeqs.Count)
        {
            CutscenePlayer.clip = CutsceneClipSeqs[SeqIndex];
            SeqInterval = (float)CutsceneClipSeqs[SeqIndex].length;
            TimeSlider.maxValue = SeqInterval;
            TimeSlider.value = SeqInterval;
            CutscenePlayer.Play();
        }
        else
        {
            IsSeqsFinished=true;
        }
       
    }
}
