using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Runtime.CompilerServices;

public class LevelSystemManagement : MonoBehaviour
{
    [Header("Essential Components")]
    [SerializeField] private PlayerBehavior player_ref;
    public string CurrentScene, NextScene;
    [SerializeField] private Vector3 CurrentCheckPoint;
    [SerializeField] private GameObject RespawningParticle;
    [SerializeField] private LoadingScreenComponent LoadingScreenUI;
    private UIManager UIControl;
    public bool IsGameplayInProgress = true;
    [Header("Score and Lives")]
    public int PlayerLife = 3;
    public int defaulttimescore;
    public int killscore;
    public int killcount;
    public int timescore;
    public int totalscore;
    public float LowTierPlayTime;
    public float MaxTierPlayTime;
    public float timerinterval = 0;

    [Header("Pausing System")]
    public bool IsPaused = false;

    [Header("System Audio")]
    [SerializeField] private AudioSource SystemAudioPlayer;
    [SerializeField] private AudioClip PauseSoundClip;
    [SerializeField] private AudioClip ClickSoundClip;
    [SerializeField] private AudioClip DenySoundClip;
    [SerializeField] private AudioClip ApproveSoundClip;

    private SaveLoadManager saveLoader;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        saveLoader = FindAnyObjectByType<SaveLoadManager>();
        CurrentScene = SceneManager.GetActiveScene().name;
        player_ref = GameObject.FindAnyObjectByType<PlayerBehavior>();
        SystemAudioPlayer = GetComponent<AudioSource>();
        UIControl = GameObject.FindAnyObjectByType<UIManager>();
        LoadingScreenUI = GameObject.FindAnyObjectByType<LoadingScreenComponent>();
        LoadingScreenUI.gameObject.SetActive(false);
        IsGameplayInProgress = true;
        killscore = 0;
        killcount = 0;
        timescore = 0;
        totalscore = 0;
        timerinterval = 0;
        CurrentCheckPoint = player_ref.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        CheckPointSystem();
        //Pausing();
        GameTimer();


        if(IsGameplayInProgress && player_ref != null)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                OnPause();
            }
        }
    }

    public void OnPause()
    {
        if (IsGameplayInProgress)
        {
            
            if (!IsPaused)
            {
                SystemAudioPlayer.clip = PauseSoundClip;
                SystemAudioPlayer.Play();
                FreezeTime();
                UIControl.OnPaused();
               
            }
            else
            {
                UnfreezeTime();
                UIControl.OnUnpaused();
                
            }
        }
    }

    public void FreezeTime()
    {
        Time.timeScale = 0;
        player_ref.enabled = false;
        player_ref.PlayerLookComponent.enabled = false;
        player_ref.PlayerMovementComponent.enabled = false;
        player_ref.PlayerTelekinesisComponent.enabled = false;
        player_ref.PlayerTelekinesisComponent.HoldPointComponent.enabled = false;
        Cursor.lockState = CursorLockMode.Confined;
        IsPaused = true;
    }

    public void UnfreezeTime()
    {
        Time.timeScale = 1;
        player_ref.enabled = true;
        player_ref.PlayerLookComponent.enabled = true;
        player_ref.PlayerMovementComponent.enabled = true;
        player_ref.PlayerTelekinesisComponent.enabled = true;
        player_ref.PlayerTelekinesisComponent.HoldPointComponent.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        IsPaused = false;
    }
    public void CheckPointSystem()
    {
        if (player_ref != null && UIControl != null)
        {
            if (player_ref.HP <= 0)
            {

                if (PlayerLife > 0)
                {
                    //player_ref.CharacterControllerComponent.enabled = false;
                    player_ref.transform.position = CurrentCheckPoint;
                    Instantiate(RespawningParticle, CurrentCheckPoint, Quaternion.identity);
                    //player_ref.CharacterControllerComponent.enabled = true;
                    player_ref.HP = 100;
                    PlayerLife -= 1;
                    player_ref.PlayAudio_Death();
                }
                else
                {
                    player_ref.gameObject.SetActive(false);
                    UIControl.OnDeath();
                    IsGameplayInProgress = false;
                    if (Input.GetKeyDown(KeyCode.R))
                    {
                        StartCoroutine(LoadSceneAsync(CurrentScene));
                    }
                }
            }
        }
       
    }

    public void OnFinish()
    {
        
        player_ref.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Confined;
        IsGameplayInProgress = false;
        timerinterval = Mathf.RoundToInt(timerinterval);
        timescore = Mathf.RoundToInt(defaulttimescore * (MaxTierPlayTime / timerinterval));
        totalscore = timescore + killscore;
        UIControl.OnSummary();
    }

    public void OnNextLevel()
    {
        if(NextScene != null)
        {
            StartCoroutine(LoadSceneAsync(NextScene));
            Cursor.lockState = CursorLockMode.Confined;
        }
       
    }

    public void OnMainmenu()
    {
        StartCoroutine(LoadSceneAsync("Mainmenu"));
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void OnQuitGame()
    {
        Application.Quit();
    }

    public void GameTimer()
    {
        if (IsGameplayInProgress)
        {
            timerinterval += 1f * Time.deltaTime;
        }
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        LoadingScreenUI.gameObject.SetActive(true);

        while (!operation.isDone)
        {
            float loadprogressvalue = Mathf.Clamp01(operation.progress / 0.9f);

            LoadingScreenUI.Loadingscreenslider.value = loadprogressvalue;

            yield return null;
        }


    }

    public void ChangeCheckPoint(Vector3 newCheckpoint)
    {
        CurrentCheckPoint = newCheckpoint;
        if(PlayerLife < 3)
        {
            PlayerLife++;
        }
    }


    public void PlayGenericClickSound()
    {
        SystemAudioPlayer.clip = ClickSoundClip;
        SystemAudioPlayer.Play();
    }

    public void PlayDenySound()
    {
        SystemAudioPlayer.clip = DenySoundClip;
        SystemAudioPlayer.Play();
    }

    public void PlayApproveSound()
    {
        SystemAudioPlayer.clip = ApproveSoundClip;
        SystemAudioPlayer.Play();
    }

}
