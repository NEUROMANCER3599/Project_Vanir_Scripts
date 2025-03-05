using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeControl : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private AudioSource AudioPlayer;
    [SerializeField] private SettingControl VolumeConfig;

    [Header("Audio Type: Music,Voice,Player,Attack,Env,Enemy,System")]
    [SerializeField] private string AudioTypeParam;
    // Start is called before the first frame update
    void Start()
    {
        AudioPlayer = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (AudioTypeParam)
        {
            case "Music": AudioPlayer.volume = VolumeConfig.MusicVolume; break;
            case "Voice": AudioPlayer.volume = VolumeConfig.VoiceLineVolume; break;
            case "Player": AudioPlayer.volume = VolumeConfig.PlayerSfxVolume; break;
            case "Attack": AudioPlayer.volume = VolumeConfig.AttackSfxVolume; break;
            case "Env": AudioPlayer.volume = VolumeConfig.EnvSfxVolume; break;
            case "Enemy": AudioPlayer.volume = VolumeConfig.EnemySfxVolume; break;
            case "System": AudioPlayer.volume = VolumeConfig.SystemVolume; break;
        }
    }
}
