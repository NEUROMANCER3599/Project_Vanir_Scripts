using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Video;

public class UIManager : MonoBehaviour
{
    [Header("UI Groups")]
    [SerializeField] private GameObject GamePlayUIGroup;
    [SerializeField] private GameObject PauseMenuGroup;
    [Header ("Panels")]
    [SerializeField] private GameObject NotePanel;
    [SerializeField] private GameObject GameplayUIPanel;
    [SerializeField] private GameObject VideoPanel;
    [SerializeField] private GameObject DeathScreenPanel;
    [SerializeField] private GameObject LevelSummaryPanel;

    [Header("CrossHair")]
    [SerializeField] private GameObject CrossHair_On; 
    [SerializeField] private GameObject CrossHair_Off;
    

    [Header("Indicator Bars")]
    [SerializeField] private Slider ForceIndicator;
    [SerializeField] private Image ForceIndicatorFill;
    [SerializeField] private Slider PowerIndicator;
    [SerializeField] private Image PowerIndicatorFill;
    [SerializeField] private Slider MeleeCooldownIndicator;
    [SerializeField] private Image MeleeCooldownIndicatorFill;
    [SerializeField] private Slider HPIndicator;
    [SerializeField] private Image DamageIndicatorScreen;
    [SerializeField] private Image Life_3;
    [SerializeField] private Image Life_2;
    [SerializeField] private Image Life_1;
    [SerializeField] private TextMeshProUGUI timetext;
    [SerializeField] private TextMeshProUGUI killcounttext;
    [SerializeField] private TextMeshProUGUI totalscoretext;
    [SerializeField] private TextMeshProUGUI gradesumtext;
 
    [Header("TextDisplay")]
    [SerializeField] private TextMeshProUGUI ObjMassDisplaytxt;
    [SerializeField] private TextMeshProUGUI ObjDisplaytxt;
    [Header("Note System")]
    [SerializeField] private TextMeshProUGUI NoteText;
    [SerializeField] private Image NoteBG;
    [SerializeField] private List<Sprite> NoteBGList;
    [Header("UI Sound System")]
    [SerializeField] private AudioSource UISoundPlayer;
    [SerializeField] private AudioClip NoteReadSound;
    [SerializeField] private AudioClip NoteCloseSound;
    [SerializeField] private AudioClip ClickSound;

    [Header("Player Portraits System")]
    [SerializeField] private Animator CharPortraitAnimator;

    [Header("Hand UI System")]
    [SerializeField] private Animator HandUIAnimator;
    [SerializeField] private List<GameObject> HandUIEffects;

    [Header("Video Player System")]
    [SerializeField] private VideoPlayer vidplayercomponent;
    [SerializeField] private Slider VidTimeSlider;
    private float VideoInterval;


    [Header("Voiceline Subtitle System")]
    public TextMeshProUGUI SubtitleText;



    private SaveLoadManager saveloader;
    private PlayerBehavior playerSystem;
    private LevelSystemManagement levelSystem;
    private bool IsDisplayingNote;
    private bool IsDisplayingVideo;
    // Start is called before the first frame update
    void Start()
    {
        saveloader = FindAnyObjectByType<SaveLoadManager>();
        HandPullAnim(false);
        CrosshairControl(false);
        IsDisplayingNote = false;
        IsDisplayingVideo = false;
        DeathScreenPanel.SetActive(false);
        LevelSummaryPanel.SetActive(false);
        playerSystem = Object.FindAnyObjectByType<PlayerBehavior>();
        levelSystem = Object.FindAnyObjectByType<LevelSystemManagement>();
        UISoundPlayer = GetComponent<AudioSource>();
        OnUnpaused();
    }

    // Update is called once per frame
    void Update()
    {
        //Update HP Indicator
        HPIndicator.maxValue = 100;
        HPIndicator.value = playerSystem.HP;
        //Update ForceIndicator
        ForceIndicator.value = playerSystem.PushForce;
       
        if (playerSystem.PushForce < ForceIndicator.maxValue * 0.5f){
            ForceIndicatorFill.color = Color.green;
        }
        else if(playerSystem.PushForce > ForceIndicator.maxValue * 0.5f && playerSystem.PushForce < ForceIndicator.maxValue * 0.75f)
        {
            ForceIndicatorFill.color = Color.yellow;
        }
        else if(playerSystem.PushForce > ForceIndicator.maxValue * 0.75f)
        {
            ForceIndicatorFill.color = Color.red;
        }


        //Update PowerIndicator
        PowerIndicator.maxValue = playerSystem.DefaultPowerLevel;
        PowerIndicator.value = playerSystem.PowerLevel;

        if(playerSystem.IsPowerDepleted == true)
        {
            PowerIndicatorFill.color = Color.grey;
        }
        else
        {
            PowerIndicatorFill.color = Color.cyan;
        }

        //Update MeleeCooldown Indicator
        MeleeCooldownIndicator.value = playerSystem.MeleeCooldown;
        if(playerSystem.MeleeCooldown >= 1)
        {
            MeleeCooldownIndicatorFill.color = new Color(255,143,0);
        }
        else
        {
            MeleeCooldownIndicatorFill.color = Color.grey;
        }


        CloseVideoPanel();
        CloseNotePanel();

        if (IsDisplayingNote)
        {
            //Debug.Log("DisplayingText");
            NotePanel.SetActive(true);
            VideoPanel.SetActive(false);
            GameplayUIPanel.SetActive(false);
        }
        else if(IsDisplayingVideo)
        {
            VideoPanel.SetActive(true);
            NotePanel.SetActive(false);
            GameplayUIPanel.SetActive(false);


            VidTimeSlider.value = (float)vidplayercomponent.time;

            PausingVideo();
            

        }
        else
        {
            
            VideoPanel.SetActive(false);
            NotePanel.SetActive(false);
            if(levelSystem.IsGameplayInProgress)
            {
                GameplayUIPanel.SetActive(true);
            }
        }

        //Damage Indicator
        DamageIndicatorScreen.color = new Color32(255, 255, 255, (byte)(255 * ((100 - playerSystem.HP) / 100)));

        //Portrait System
        CharPortraitAnimator.SetFloat("HP",playerSystem.HP);
        CharPortraitAnimator.SetBool("IsDepleted", playerSystem.IsPowerDepleted);
        CharPortraitAnimator.SetBool("IsAttacking",playerSystem.IsAttacking);

        //Dynamic Power and Force Indicator visibility
        if (Input.GetMouseButton(0))
        {
            ForceIndicator.gameObject.SetActive(true);
            PowerIndicator.gameObject.SetActive(true);
        }
        else
        {
            if(playerSystem.PowerLevel < playerSystem.DefaultPowerLevel)
            {
                PowerIndicator.gameObject.SetActive(true);
            }
            else
            {
                PowerIndicator.gameObject.SetActive(false);
            }
            ForceIndicator.gameObject.SetActive(false);
            
        }

        //Player Lives Indicator
        if(levelSystem.PlayerLife == 3)
        {
            Life_3.gameObject.SetActive(true);
            Life_2.gameObject.SetActive(true);
            Life_1.gameObject.SetActive(true);
        }
        else if(levelSystem.PlayerLife == 2)
        {
            Life_3.gameObject.SetActive(false);
            Life_2.gameObject.SetActive(true);
            Life_1.gameObject.SetActive(true);
        }
        else if(levelSystem.PlayerLife == 1)
        {
            Life_3.gameObject.SetActive(false);
            Life_2.gameObject.SetActive(false);
            Life_1.gameObject.SetActive(true);
        }
        else if(levelSystem.PlayerLife == 0)
        {
            Life_3.gameObject.SetActive(false);
            Life_2.gameObject.SetActive(false);
            Life_1.gameObject.SetActive(false);
        }
        

        
    }

    public void CloseVideoPanel()
    {
        if (IsDisplayingVideo == true && Input.GetKeyDown(KeyCode.Q))
        {
            UISoundPlayer.clip = NoteCloseSound;
            UISoundPlayer.Play();
            vidplayercomponent.Stop();
            IsDisplayingVideo = false;
            levelSystem.UnfreezeTime();
        }
    }
    public void CloseNotePanel()
    {
        if (IsDisplayingNote == true && Input.GetKeyDown(KeyCode.Q))
        {
            UISoundPlayer.clip = NoteCloseSound;
            UISoundPlayer.Play();
            IsDisplayingNote = false;
            levelSystem.UnfreezeTime();
        }
    }
    void PausingVideo()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!vidplayercomponent.isPaused)
            {
                UISoundPlayer.clip = NoteReadSound;
                UISoundPlayer.Play();
                vidplayercomponent.Pause();
            }
            else
            {
                UISoundPlayer.clip = NoteCloseSound;
                UISoundPlayer.Play();
                vidplayercomponent.Play();
            }
        }
    }

    public void CrosshairControl(bool status)
    {
        if (status)
        {
            CrossHair_On.SetActive(true);
            CrossHair_Off.SetActive(false);
        }
        else
        {
            CrossHair_On.SetActive(false);
            CrossHair_Off.SetActive(true);
        }
    }

    public void ObjMassDisplay(Rigidbody rb,bool IsOn)
    {
        if (IsOn)
        {
            ObjMassDisplaytxt.text = "Mass: " + rb.mass + " Kg.";
        }
        else
        {
            ObjMassDisplaytxt.text = "";
        }
    }

    public void MiscTextDisplay(string input,Color32 color,bool IsOn)
    {
        ObjDisplaytxt.color = color;

        if (IsOn)
        {
            ObjDisplaytxt.text = input;
            //NotePanel.SetActive(true);
        }
        else
        {
            ObjDisplaytxt.text = "";
            //NotePanel.SetActive(false);
        }
    }



    public void DisplayNote (string notetext,Color32 textcolor,int noteBGindex)
    {
            UISoundPlayer.clip = NoteReadSound;
            UISoundPlayer.Play();
            NoteText.color = textcolor;
            NoteBG.sprite = NoteBGList[noteBGindex];
            IsDisplayingNote = true;
            NoteText.text = notetext;
            levelSystem.FreezeTime();
    }

    public void DisplayVideo (VideoClip Video)
    {
        UISoundPlayer.clip = NoteReadSound;
        UISoundPlayer.Play();

        vidplayercomponent.clip = Video;
        VidTimeSlider.maxValue = (float)vidplayercomponent.length;
        //VidTimeSlider.value = (float)Video.length;
        vidplayercomponent.Play();

        IsDisplayingVideo = true;

        levelSystem.FreezeTime();
    }

   

    public void OnDeath()
    {
        DeathScreenPanel.SetActive(true);
        GameplayUIPanel.SetActive(false);
        NotePanel.SetActive(false);
        VideoPanel.SetActive(false);
    }

    public void OnSummary()
    {
        LevelSummaryPanel.SetActive(true);
        GameplayUIPanel.SetActive(false);
        NotePanel.SetActive(false);
        VideoPanel.SetActive(false);
        killcounttext.text = "Kills: " + levelSystem.killcount.ToString();
        timetext.text = "Time: " + levelSystem.timerinterval.ToString() + " Seconds";
        totalscoretext.text = "Total Score: " + levelSystem.totalscore.ToString();

        if(levelSystem.timerinterval <= levelSystem.MaxTierPlayTime && levelSystem.PlayerLife >= 3)
        {
            gradesumtext.color = Color.green;
            gradesumtext.text = "Grade: A";
        }
        else if (levelSystem.timerinterval <= levelSystem.MaxTierPlayTime && levelSystem.PlayerLife < 3)
        {
            gradesumtext.color = Color.yellow;
            gradesumtext.text = "Grade: B";
        }
        else if(levelSystem.timerinterval > levelSystem.MaxTierPlayTime && levelSystem.timerinterval < (levelSystem.LowTierPlayTime))
        {
            gradesumtext.color = Color.yellow;
            gradesumtext.text = "Grade: B";
        }
        else if(levelSystem.timerinterval >= levelSystem.LowTierPlayTime)
        {
            gradesumtext.color = Color.red;
            gradesumtext.text = "Grade: C";
        }

    }

    public void OnContinue()
    {
        PlayClickSound();
        saveloader.SaveGame();
        levelSystem.OnNextLevel();
    }

    public void OnExit()
    {
        PlayClickSound();
        saveloader.SaveSetting();
        levelSystem.OnMainmenu();
    }

    public void PlayClickSound()
    {
        UISoundPlayer.clip = ClickSound;
        UISoundPlayer.Play();
    }

    public void OnResume()
    {
        PlayClickSound();
        saveloader.SaveSetting();
        levelSystem.OnPause();
    }

    public void OnPaused()
    {
        GamePlayUIGroup.SetActive(false);
        PauseMenuGroup.SetActive(true);
    }

    public void OnUnpaused()
    {
        GamePlayUIGroup.SetActive(true);
        PauseMenuGroup.SetActive(false);
    }

    public void OnExploded_CharPortrait()
    {
        CharPortraitAnimator.SetTrigger("Exploded");
    }

    public void HandPullAnim(bool b)
    {
        HandUIAnimator.SetBool("IsHolding", b);
        if (HandUIAnimator.GetBool("IsHolding"))
        {
            for(int i = 0; i < HandUIEffects.Count; i++)
            {
                HandUIEffects[i].SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < HandUIEffects.Count; i++)
            {
                HandUIEffects[i].SetActive(false);
            }
        }
    }

    

    public void HandPushAnimTrigger()
    {
        HandUIAnimator.SetTrigger("Push");
    }

    public void HandPunchAnimTrigger()
    {
        HandUIAnimator.SetTrigger("Punch");
    }
}
