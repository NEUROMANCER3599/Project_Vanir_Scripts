using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainmenuManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject MainmenuPanel;
    [SerializeField] private GameObject LevelSelectPanel;
    [SerializeField] private GameObject SettingsPanel;

    [Header("Level Select Panels Components")]
    [SerializeField] private TextMeshProUGUI LevelNumberText;
    [SerializeField] private TextMeshProUGUI LevelTitleText;
    [SerializeField] private Image LevelScreenshotImg;

    [Header("Settings Panels Components")]
    [SerializeField] private Slider MusicVolSlider;
    [SerializeField] private Slider VoiceVolSlider;
    [SerializeField] private Slider PlayerVolSlider;
    [SerializeField] private Slider AttackVolSlider;
    [SerializeField] private Slider EnvVolSlider;
    [SerializeField] private Slider EnemyVolSlider;
    [SerializeField] private Slider SystemAudioVolSlider;
    [SerializeField] private Slider MouseSensSlider;
    [SerializeField] private SettingControl SettingConfigFile;

    [Header("References")]
    [SerializeField] private LevelSystemManagement levelSystem;
    [SerializeField] private SaveLoadManager saveLoadManager;
    
    // Start is called before the first frame update
    void Start()
    {
        
        levelSystem = GameObject.FindAnyObjectByType<LevelSystemManagement>();
        saveLoadManager = FindAnyObjectByType<SaveLoadManager>();
        MainmenuPanel.SetActive(true);
        LevelSelectPanel.SetActive(false);
        SettingsPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (SettingsPanel.activeSelf)
        {
            ConfigValueAssignment();
        }
    }

    void SyncSlidersToConfig()
    {
        MusicVolSlider.value = SettingConfigFile.MusicVolume;
        VoiceVolSlider.value = SettingConfigFile.VoiceLineVolume;
        PlayerVolSlider.value = SettingConfigFile.PlayerSfxVolume;
        AttackVolSlider.value = SettingConfigFile.AttackSfxVolume;
        EnvVolSlider.value = SettingConfigFile.EnvSfxVolume;
        EnemyVolSlider.value = SettingConfigFile.EnemySfxVolume;
        MouseSensSlider.value = SettingConfigFile.MouseSensitivity;
        SystemAudioVolSlider.value = SettingConfigFile.SystemVolume;
    }

    void ConfigValueAssignment()
    {
        SettingConfigFile.MusicVolume = MusicVolSlider.value;
        SettingConfigFile.VoiceLineVolume = VoiceVolSlider.value;
        SettingConfigFile.PlayerSfxVolume = PlayerVolSlider.value;
        SettingConfigFile.AttackSfxVolume = AttackVolSlider.value;
        SettingConfigFile.EnvSfxVolume = EnvVolSlider.value;
        SettingConfigFile.EnemySfxVolume = EnemyVolSlider.value;
        SettingConfigFile.MouseSensitivity = MouseSensSlider.value;
        SettingConfigFile.SystemVolume = SystemAudioVolSlider.value;
    }


    public void OnReturnToMenu()
    {
        levelSystem.PlayGenericClickSound();
        MainmenuPanel.SetActive(true);
        LevelSelectPanel.SetActive(false);
        SettingsPanel.SetActive(false);
    }

  

    public void OnSelectSettingPanel()
    {
        SyncSlidersToConfig();
        levelSystem.PlayGenericClickSound();
        MainmenuPanel.SetActive(false);
        LevelSelectPanel.SetActive(false);
        SettingsPanel.SetActive(true);
    }

    public void OnNewGame()
    {
        levelSystem.PlayGenericClickSound();
        saveLoadManager.ResetEverything();
        levelSystem.NextScene = "Cutscene_Arc1";
        levelSystem.OnNextLevel();
    }


    //---Old Level Select System (Unused)---
    /*
    public void OnSelectLevelSelectionPanel()
    {
        levelSystem.PlayGenericClickSound();
        MainmenuPanel.SetActive(false);
        LevelSelectPanel.SetActive(true);
        SettingsPanel.SetActive(false);
    }
    public void OnSelectLevel(LevelBtnAttribute levelbtn)
    {
        levelSystem.PlayGenericClickSound();
        LevelNumberText.text = "Level " + levelbtn.LevelNumber.ToString();
        LevelTitleText.text = levelbtn.LevelTitle.ToString();
        levelSystem.NextScene = levelbtn.LevelName;
        LevelScreenshotImg.sprite = levelbtn.LevelScreenshot;

    }
    */
}
