using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettingsConfig", menuName = "Custom Object/Game Settings Config")]
public class SettingControl : ScriptableObject
{
    [Header("Audio Parameters")]
    public float MusicVolume;
    public float VoiceLineVolume;
    public float PlayerSfxVolume;
    public float AttackSfxVolume;
    public float EnvSfxVolume;
    public float EnemySfxVolume;
    public float SystemVolume;

    [Header("Control Parameters")]
    public float MouseSensitivity;
}
