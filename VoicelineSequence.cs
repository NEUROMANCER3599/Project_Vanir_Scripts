using UnityEngine;


[CreateAssetMenu(fileName = "VoicelineSeq", menuName = "Custom Object/VoicelineSeq")]
public class VoicelineSequence : ScriptableObject
{
    [Header("Seq Data")]
    public AudioClip VoiceClip;
    public string VoiceScript;
}
