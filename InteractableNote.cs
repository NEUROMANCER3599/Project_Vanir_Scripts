using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class InteractableNote : MonoBehaviour
{
    [Header("Module Parameter")]
    [SerializeField] private bool IsAutoPromptTrigger;

    [Header("UI Manager Component")]
    [SerializeField] private UIManager UIControl;
    //public bool IsActivated;
    //private float ActivateCoolDownInterval;
    [Header("Type Selection | True = Note System | False = Video System")]
    [SerializeField] private bool IsNote;
    [Header("Note System Parameters")]
    [SerializeField] private string NoteText;
    [SerializeField] private Color32 NoteTextColor;
    [SerializeField] private int NoteBackgroundSelection;
    [Header("Video System Parameters")]
    [SerializeField] private VideoClip vidClip;

    void Start()
    {
        UIControl = FindAnyObjectByType<UIManager>();
    }

    public void OnInteracted()
    {
        if (IsNote)
        {
            UIControl.DisplayNote(NoteText, NoteTextColor, NoteBackgroundSelection);
        }
        else
        {
            UIControl.DisplayVideo(vidClip);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<PlayerBehavior>())
        {
            if (IsAutoPromptTrigger)
            {
                OnInteracted();
                Destroy(gameObject);
            }
        }
       
    }

}
