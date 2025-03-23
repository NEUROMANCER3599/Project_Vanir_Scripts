using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseMenuBehavior : MonoBehaviour

{
    [Header("Components")]
    [SerializeField] private GameObject MinimapPanel;
    [SerializeField] private GameObject SettingsPanel;
    [SerializeField] private GameObject UpgradePanel;

    [Header("Current Panel | Settings,Minimap,Upgrades")]
    [SerializeField] private string CurrentPanel = "Settings";

    [Header("Settings Components")]
    [SerializeField] private Slider MusicVolSlider;
    [SerializeField] private Slider VoiceVolSlider;
    [SerializeField] private Slider PlayerVolSlider;
    [SerializeField] private Slider AttackVolSlider;
    [SerializeField] private Slider EnvVolSlider;
    [SerializeField] private Slider EnemyVolSlider;
    [SerializeField] private Slider SystemAudioVolSlider;
    [SerializeField] private Slider MouseSensSlider;
    [SerializeField] private SettingControl SettingConfigFile;

    [Header("Upgrade Menu Components")]
    [SerializeField] private TextMeshProUGUI PerkPointDisplayTxt;
    [SerializeField] private TextMeshProUGUI MaxPowerReserveLvlTxt;
    [SerializeField] private TextMeshProUGUI MaxPushPowerLvlTxt;
    [SerializeField] private TextMeshProUGUI MaxPushChargeLvlTxt;
    [SerializeField] private TextMeshProUGUI SkillCooldownLvlTxt;
    [SerializeField] private TextMeshProUGUI PowerRecoveryLvlTxt;
    [SerializeField] private TextMeshProUGUI PickUpRangeLvlTxt;
    [SerializeField] private TextMeshProUGUI HealthBonusLvlTxt;
    [SerializeField] private TextMeshProUGUI MoveSpeedBonusLvlTxt;
    [SerializeField] private TextMeshProUGUI SkillPowerLvlTxt;
    [SerializeField] private TextMeshProUGUI HealthRegenLvlTxt;

    [Header("References")]
    [SerializeField] private LevelSystemManagement levelSystem;
    [SerializeField] private PlayerBehavior playerSystem;
    private SaveLoadManager saveloader;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        saveloader = FindAnyObjectByType<SaveLoadManager>();
        saveloader.LoadSetting();
        SyncSlidersToConfig();
        levelSystem = GameObject.FindAnyObjectByType<LevelSystemManagement>();
        playerSystem = GameObject.FindAnyObjectByType<PlayerBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (CurrentPanel)
        {
            case "Settings": MinimapPanel.SetActive(false); SettingsPanel.SetActive(true); UpgradePanel.SetActive(false); break;
            case "Minimap": MinimapPanel.SetActive(true); SettingsPanel.SetActive(false); UpgradePanel.SetActive(false); break;
            case "Upgrades": MinimapPanel.SetActive(false); SettingsPanel.SetActive(false); UpgradePanel.SetActive(true); break;
        }

        if (SettingsPanel.activeSelf)
        {
            ConfigValueAssignment();
        }

        if (UpgradePanel.activeSelf)
        {
            PerkPointDisplayTxt.text = "Upgrade Points : " + playerSystem.PerkPoint;
            MaxPowerReserveLvlTxt.text = "Level : " + playerSystem.PowerReserveBonus;
            MaxPushChargeLvlTxt.text = "Level : " + playerSystem.PushPowerChargeBonus;
            MaxPushPowerLvlTxt.text = "Level : " + playerSystem.PushPowerBonus;
            SkillCooldownLvlTxt.text = "Level : " + playerSystem.skillcooldownBonus;
            PowerRecoveryLvlTxt.text = "Level : " + playerSystem.PowerRecoveryBonus;
            PickUpRangeLvlTxt.text = "Level : " + playerSystem.PickupRangeBonus;
            HealthBonusLvlTxt.text = "Level : " + playerSystem.HealthBonus;
            MoveSpeedBonusLvlTxt.text = "Level : " + playerSystem.MoveSpeedBonus;
            SkillPowerLvlTxt.text = "Level : " + playerSystem.SkillPowerBonus;
            HealthRegenLvlTxt.text = "Level : " + playerSystem.HealthRegenBonus;
            
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

    public void OnSelectPanel(string panel)
    {
        levelSystem.PlayGenericClickSound();
        saveloader.SaveSetting();
        CurrentPanel = panel;
        if(panel == "Settings")
        {
            SyncSlidersToConfig();
        }
    }

    public void OnSelectUpgrade(string option)
    {
        if (playerSystem.PerkPoint > 0)
        {
            switch (option)
            {
                case "MaxPowerReserve": playerSystem.PowerReserveBonus += 1; playerSystem.PerkPoint -= 1; break;
                case "MaxPushCharge": playerSystem.PushPowerChargeBonus += 1; playerSystem.PerkPoint -= 1; break;
                case "MaxPushPower": playerSystem.PushPowerBonus += 1; playerSystem.PerkPoint -= 1; break;
                case "MaxPowerRecovery": playerSystem.PowerRecoveryBonus += 1; playerSystem.PerkPoint -= 1; break;
                case "SkillCooldown": playerSystem.skillcooldownBonus += 1; playerSystem.PerkPoint -= 1; break;
                case "PickUpRange": playerSystem.PickupRangeBonus += 1; playerSystem.PerkPoint -= 1; break;
                case "HealthBonus": playerSystem.HealthBonus += 1; playerSystem.PerkPoint -= 1; break;
                case "MoveSpeed": playerSystem.MoveSpeedBonus += 1; playerSystem.PerkPoint -= 1; break;
                case "SkillPower": playerSystem.SkillPowerBonus += 1; playerSystem.PerkPoint -= 1; break;
                case "HealthRegen": playerSystem.HealthRegenBonus += 1; playerSystem.PerkPoint -= 1; break;
            }
            levelSystem.PlayApproveSound();
        }
        else
        {
            levelSystem.PlayDenySound();
        }
    }
}
