using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayTextComponent : MonoBehaviour
{
    [SerializeField] private UIManager UIControl;
    [SerializeField] private string DisplayText;
    [SerializeField] private Color32 TextColor;
    private void Start()
    {
        UIControl = GameObject.FindAnyObjectByType<UIManager>();
    }

    public void OnInspected()
    {
        UIControl.MiscTextDisplay(DisplayText, TextColor, true);
    }

    public void OnLookedAway()
    {
        UIControl.MiscTextDisplay(DisplayText, TextColor, false);
    }
    
}
