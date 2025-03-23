using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveLoadManager : MonoBehaviour
{
    [Header("System Components")]
    private PlayerBehavior player;
    private LevelSystemManagement levelSystem;
    public SettingControl settingControl;
    private MainmenuManager mainmenuManager;

    private void Awake()
    {
        player = FindAnyObjectByType<PlayerBehavior>();
        levelSystem = FindAnyObjectByType<LevelSystemManagement>();
        mainmenuManager = FindAnyObjectByType<MainmenuManager>();
        //SaveSetting();
        LoadSetting();
    }
    public void SaveGame()
    {
        Debug.Log("Stat Saved");
        SaveData saveData = new SaveData();
        saveData.LatestPerkPointReserve = player.PerkPoint;
        saveData.LatestPowerReserveBonus = player.PowerReserveBonus;
        saveData.LatestPowerRecoveryBonus = player.PowerRecoveryBonus;
        saveData.LatestPushPowerBonus = player.PushPowerBonus;
        saveData.LatestPushPowerChargeBonus = player.PushPowerChargeBonus;
        saveData.LatestSkillPowerBonus = player.SkillPowerBonus;
        saveData.LatestskillcooldownBonus = player.skillcooldownBonus;
        saveData.LatestPickupRangeBonus = player.PickupRangeBonus;
        saveData.LatestHealthBonus = player.HealthBonus;
        saveData.LatestMoveSpeedBonus = player.MoveSpeedBonus;
        saveData.LatestHealthRegenBonus = player.HealthRegenBonus;

        string Json = JsonUtility.ToJson(saveData);
        string Path = Application.persistentDataPath + "/SaveData.json";
        System.IO.File.WriteAllText(Path, Json);
    }

    public void SaveScene()
    {
        Debug.Log("Scene Saved");
        SceneData sceneData = new SceneData();
        sceneData.CurrentScene = SceneManager.GetActiveScene().name;
        string Json = JsonUtility.ToJson(sceneData);
        string Path = Application.persistentDataPath + "/SceneData.json";
        System.IO.File.WriteAllText(Path, Json);
    }

    public void LoadGameScene()
    {
        
        string path = Application.persistentDataPath + "/SceneData.json";
        if (File.Exists(path))
        {
            Debug.Log("Scene Loaded");
            string Json = System.IO.File.ReadAllText(path);
            SceneData LoadedData = JsonUtility.FromJson<SceneData>(Json);

            levelSystem.NextScene = LoadedData.CurrentScene;
            //levelSystem.PlayGenericClickSound();
            //levelSystem.OnNextLevel();
           
        }
        else
        {
           if(mainmenuManager != null)
            {
                mainmenuManager.OnNewGame();
            }
        }
    }

    public void UpdatePlayerStats()
    {
        string path = Application.persistentDataPath + "/SaveData.json";
        if (File.Exists(path))
        {
            Debug.Log("Stats Updated");
            string Json = System.IO.File.ReadAllText(path);
            SaveData LoadedData = JsonUtility.FromJson<SaveData>(Json);

            player.PerkPoint = LoadedData.LatestPerkPointReserve;
            player.PowerReserveBonus = LoadedData.LatestPowerReserveBonus;
            player.PowerRecoveryBonus = LoadedData.LatestPowerRecoveryBonus;
            player.PushPowerBonus = LoadedData.LatestPushPowerBonus;
            player.PushPowerChargeBonus = LoadedData.LatestPushPowerChargeBonus;
            player.SkillPowerBonus = LoadedData.LatestSkillPowerBonus;
            player.skillcooldownBonus = LoadedData.LatestskillcooldownBonus;
            player.PickupRangeBonus = LoadedData.LatestPickupRangeBonus;
            player.HealthBonus = LoadedData.LatestHealthBonus;
            player.MoveSpeedBonus = LoadedData.LatestMoveSpeedBonus;
            player.HealthRegenBonus = LoadedData.LatestHealthRegenBonus;
        }
    }

    public void SaveSetting()
    {
        Debug.Log("Settings Saved");

        SettingData settingData = new SettingData();

        settingData.AttackSfxVolume = settingControl.AttackSfxVolume;
        settingData.PlayerSfxVolume = settingControl.PlayerSfxVolume;
        settingData.EnvSfxVolume = settingControl.EnvSfxVolume;
        settingData.EnemySfxVolume = settingControl.EnemySfxVolume;
        settingData.SystemVolume = settingControl.SystemVolume;
        settingData.MusicVolume = settingControl.MusicVolume;
        settingData.VoiceLineVolume = settingControl.VoiceLineVolume;
        settingData.MouseSensitivity = settingControl.MouseSensitivity;

        string Json = JsonUtility.ToJson(settingData);
        string Path = Application.persistentDataPath + "/SettingsData.json";
        System.IO.File.WriteAllText(Path, Json);
    }

    public void LoadSetting()
    {
        string path = Application.persistentDataPath + "/SettingsData.json";
        if (File.Exists(path))
        {
            Debug.Log("Settings Loaded");
            string Json = System.IO.File.ReadAllText(path);
            SettingData LoadedData = JsonUtility.FromJson<SettingData>(Json);

            settingControl.AttackSfxVolume = LoadedData.AttackSfxVolume;
            settingControl.PlayerSfxVolume = LoadedData.PlayerSfxVolume;
            settingControl.EnvSfxVolume = LoadedData.EnvSfxVolume;
            settingControl.EnemySfxVolume = LoadedData.EnemySfxVolume;
            settingControl.SystemVolume = LoadedData.SystemVolume;
            settingControl.MusicVolume = LoadedData.MusicVolume;
            settingControl.VoiceLineVolume = LoadedData.VoiceLineVolume;
            settingControl.MouseSensitivity = LoadedData.MouseSensitivity;

        }
    }

    public void ResetEverything()
    {
        string SavePath = Application.persistentDataPath + "/SaveData.json";
        string ScenePath = Application.persistentDataPath + "/SceneData.json";
        if (File.Exists(SavePath))
        {
            System.IO.File.Delete(SavePath);
        }
        if (File.Exists(ScenePath))
        {
            System.IO.File.Delete(ScenePath);
        }


    }
}
