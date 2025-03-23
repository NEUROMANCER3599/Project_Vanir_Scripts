
using StarterAssets;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehavior : MonoBehaviour
{
    
    [Header("Movement Stats")]
    public float moveSpeed = 6;

    public float JumpForce = 10;

    float defaultjumpforce;
    float defaultmovespeed;
    bool grounded = true;
    

    [Header("Player Stats")]
    public float PushForce = 0f;
    public float PowerLevel = 100f;
    public float DefaultPowerLevel = 100f;
    public float MeleeCooldown = 1f;
    public bool IsPowerDepleted = false;
    public bool IsAttacking = false;
    public float HoldingObjMass = 0;
    public float HP = 100;
    public float CameraSensitvity = 1;
    [Header("Player Upgrades")]
    public int PerkPoint = 0;
    public float PowerReserveBonus = 0;
    public float PushPowerBonus = 0;
    public float PushPowerChargeBonus = 0;
    public float skillcooldownBonus = 0;
    public float PowerRecoveryBonus = 0;
    public float HealthBonus = 0;
    public float MoveSpeedBonus = 0;
    public float PickupRangeBonus = 0;
    public float SkillPowerBonus = 0;
    public float HealthRegenBonus = 0;

    [Header("Player Components")]
    [SerializeField] private Light PowerLight;
    [SerializeField] private GameObject PowerAura;
    [SerializeField] private GameObject MeleeBoxObject;
    [SerializeField] private GameObject ForceShieldObject;
    [SerializeField] private GameObject SkillProjectileObject;
    public Transform MeleeBoxSpawnPoint;
    public Transform ForceShieldSpawnPoint;
    public Transform SkillProjectileSpawnPoint;
    public Transform orientation;
    public PlayerMovement PlayerMovementComponent;
    public PlayerLook PlayerLookComponent;
    public UIManager UIControl;
    public PickupController PlayerTelekinesisComponent;
    //public CharacterController CharacterControllerComponent;
    private LevelSystemManagement levelcontrol;
    private SaveLoadManager saveloader;
    public bool IsTargetingValidObj;
    public SettingControl ConfigFile;

    [Header("Player SoundPlayer")]
    [SerializeField] private AudioSource PlayerSoundPlayer;
    [SerializeField] private AudioSource AttackSoundPlayer;
    [SerializeField] private AudioSource PlayerMoveSoundPlayer;
    [Header("Player Sfx")]
    [SerializeField] private AudioClip AttackInit_Sfx;
    [SerializeField] private AudioClip AttackHold_Sfx;
    [SerializeField] private AudioClip AttackThrow_Sfx;
    [SerializeField] private AudioClip AttackMelee_Sfx;
    [SerializeField] private AudioClip PowerDeplete_Sfx;
    [SerializeField] private AudioClip PlayerHurt_Sfx;
    [SerializeField] private AudioClip PlayerDeath_Sfx;
    [SerializeField] private AudioClip PlayerJump_Sfx;
    [SerializeField] private AudioClip NullTargetSfx;
    [Header("Movement Sfx")]
    [SerializeField] private List<AudioClip> Footstep_Concrete;
    [SerializeField] private List<AudioClip> Footstep_Metal;
    [SerializeField] private List<AudioClip> Footstep_Water;
    private int TerrainFootstepParameter;


    // Start is called before the first frame update
    void Start()
    {
        //CharacterControllerComponent = GetComponent<CharacterController>();
        saveloader = FindAnyObjectByType<SaveLoadManager>();
        saveloader.UpdatePlayerStats();
        saveloader.SaveScene();
        PlayerTelekinesisComponent = FindAnyObjectByType<PickupController>();
        PlayerMovementComponent = GetComponent<PlayerMovement>();
        levelcontrol = GameObject.FindFirstObjectByType<LevelSystemManagement>();
        PlayerLookComponent = GetComponent<PlayerLook>();
        UIControl = GameObject.FindAnyObjectByType<UIManager>();
        defaultmovespeed = moveSpeed;
        defaultjumpforce = JumpForce;
        Cursor.lockState = CursorLockMode.Locked;
        PowerLight = GetComponentInChildren<Light>();
        PowerLight.enabled = false;
        PowerAura.SetActive(false);
        
        HP = 100 + (HealthBonus * 10);
    }

    void Update()
    {
        PushForceSystem();
        PowerLimitSystem();
        SkillSystem();
        HPRegen();
        
        
        PlayerMovementComponent.walkSpeed = moveSpeed;
        PlayerMovementComponent.sprintSpeed = moveSpeed * 1.5f;
        PlayerMovementComponent.jumpForce = JumpForce;
        CameraSensitvity = ConfigFile.MouseSensitivity;

        if (Input.GetMouseButtonDown(0) && IsTargetingValidObj == false || Input.GetMouseButtonDown(0) && IsPowerDepleted == true)
        {
            PlayAudio_NullTarget();
            
        }
        if (!IsTargetingValidObj)
        {
            StopAttackAudio();
        }
       
       

    }

   private void SkillSystem()
   {
        MeleeBoxSpawnPoint = GameObject.Find("SkillMeleePoint").transform;
        SkillProjectileSpawnPoint = GameObject.Find("SkillProjectilePoint").transform;
        ForceShieldSpawnPoint = GameObject.Find("SkillShieldPoint").transform;

        if (MeleeCooldown < 1)
        {
            MeleeCooldown += (0.10f + (skillcooldownBonus / 20)) * Time.deltaTime;
        }

        if(MeleeCooldown >= 1)
        {
            if (Input.GetKeyDown(KeyCode.E)) //Melee Skill
            {
                //PlayAudio_Melee();
                Instantiate(MeleeBoxObject, MeleeBoxSpawnPoint.position, MeleeBoxSpawnPoint.rotation);
                MeleeCooldown = 0;
                UIControl.HandPunchAnimTrigger();
            }
            else if (Input.GetKeyDown(KeyCode.R)) //Shield Skill
            {
                
                Instantiate(ForceShieldObject, ForceShieldSpawnPoint.position, ForceShieldSpawnPoint.rotation);
                MeleeCooldown = 0;
                UIControl.HandPunchAnimTrigger();
                
            }
            else if (Input.GetKeyDown(KeyCode.Q)) //Projectile Skill
            {
                var Projectile =  Instantiate(SkillProjectileObject, SkillProjectileSpawnPoint.position, SkillProjectileSpawnPoint.rotation);
                MeleeCooldown = 0;
                UIControl.HandPunchAnimTrigger();
            }
            /*
            else if (Input.GetKeyDown(KeyCode.E) && Input.GetKey(KeyCode.D)) //Projectile Skill
            {
                var Projectile = Instantiate(SkillProjectileObject, SkillProjectileSpawnPoint.position, SkillProjectileSpawnPoint.rotation);
                MeleeCooldown = 0;
                UIControl.HandPunchAnimTrigger();
            }
            */


        }
   }
    private void HPRegen()
    {
        if(HP < 100f + (HealthBonus * 10f))
        {
            HP += (1.5f + (HealthRegenBonus/5f)) * Time.deltaTime;
        }

        if(HP > 100 + (HealthBonus * 10f))
        {
            HP = 100 + (HealthBonus * 10f);
        }
    }
    
       

    private void PushForceSystem()
    {
        
        if (Input.GetMouseButton(1) && Input.GetMouseButton(0) && IsTargetingValidObj == true)
        {
            PushForce += (0.25f + (PushPowerChargeBonus/20)) * Time.deltaTime;
        }
        else if(PushForce > 0)
        {
            PushForce -= ((0.25f + (PushPowerChargeBonus / 20))*2) * Time.deltaTime;
        }


        if (PushForce > 1f + (PushPowerBonus * 0.5f))
        {
            PushForce = 1f + (PushPowerBonus * 0.5f);
        }
    }

    private void PowerLimitSystem()
    {


        if (Input.GetMouseButton(0) && IsPowerDepleted == false && IsTargetingValidObj == true)
        {
            //PlayAudio_AttackHold();
            IsAttacking = true;
            PowerLevel -= (10f + HoldingObjMass) * Time.deltaTime;
            PowerLight.enabled = true;
            PowerAura.SetActive(true);
            UIControl.HandPullAnim(true);
        }
        else if(PowerLevel > 0 && IsPowerDepleted == false)
        {
            PowerLevel += (10f + (PowerRecoveryBonus * 2f)) * Time.deltaTime;
        }
        else if(IsPowerDepleted == true)
        {
            PowerLevel += ((10f + (PowerRecoveryBonus * 2f))*0.5f) * Time.deltaTime;
        }
        
        if(Input.GetMouseButtonUp(0))
        {
            //PlayAudio_PowerStop();
            PowerLight.enabled = false;
            PowerAura.SetActive(false);
            IsAttacking = false;
            UIControl.HandPullAnim(false);

        }
        if (IsPowerDepleted)
        {
            PowerLight.enabled = false;
            PowerAura.SetActive(false);
            IsAttacking = false;
            //UIControl.CharPortrait_Idle();
        }


        if (PowerLevel <= 0)
        {
            PlayAudio_PowerStop();
            PowerLevel = 0;
            IsPowerDepleted = true;
        }

        if(PowerLevel > DefaultPowerLevel + (PowerReserveBonus * 10))
        {
            PowerLevel = DefaultPowerLevel + (PowerReserveBonus * 10);
            IsPowerDepleted = false;
        }
    }

    public void InstantPowerRecover(float multiplier)
    {
        PowerLevel += ((DefaultPowerLevel + (PowerReserveBonus * 10)) * 0.1f) * multiplier;
    }

    public void InstantHPRecover(float multiplier)
    {
        HP += ((100 + (HealthBonus * 10f)) * 0.1f) * multiplier;
    }
    
    private void OnCollisionEnter(Collision other)
    {
       
       grounded = true;

        if(other.gameObject.GetComponent<ProjectileBehavior>() != null)
        {
            ProjectileBehavior projectile = other.gameObject.GetComponent<ProjectileBehavior>();

            HP -= projectile.FixedDamageValue/2;

            projectile.OnHit();

            PlayAudio_Dmg();
        }

        if(other.gameObject.tag == "Ground" && other.gameObject.GetComponent<TerrainParameter>() != null)
        {
            TerrainParameter CurrentTerrain = other.gameObject.GetComponent<TerrainParameter>();

            TerrainFootstepParameter = CurrentTerrain.TerrainType;

            PlayMoveSound();
        }
        
    }

    private void OnCollisionExit(Collision collision)
    {
        grounded = false;
    }


    public float GetDefaultMoveSpeed()
    {
        return defaultmovespeed;
    }

    public float GetDefaultJumpForce()
    {
        return defaultjumpforce;
    }
    

    public void PlayAudio_AttackStart()
    {
        AttackSoundPlayer.clip = AttackInit_Sfx;
        AttackSoundPlayer.loop = false;
        AttackSoundPlayer.Play();
        Invoke(nameof(PlayAudio_AttackHold), 0.5f);
    }

    public void PlayAudio_AttackHold()
    {
        AttackSoundPlayer.clip = AttackHold_Sfx;
        AttackSoundPlayer.loop = true;
        AttackSoundPlayer.Play();
    }

    public void PlayAudio_AttackThrow()
    {
        AttackSoundPlayer.clip = AttackThrow_Sfx;
        AttackSoundPlayer.loop = false;
        AttackSoundPlayer.Play();
    }

    public void PlayAudio_Melee()
    {
        AttackSoundPlayer.clip = AttackMelee_Sfx;
        AttackSoundPlayer.loop = false;
        AttackSoundPlayer.Play();
    }

    public void PlayAudio_PowerStop()
    {
        AttackSoundPlayer.clip = PowerDeplete_Sfx;
        AttackSoundPlayer.loop = false;
        AttackSoundPlayer.Play();
    }

    public void PlayAudio_Dmg()
    {
        PlayerSoundPlayer.clip = PlayerHurt_Sfx;
        PlayerSoundPlayer.Play();
    }

    public void PlayAudio_Death()
    {
        PlayerSoundPlayer.clip = PlayerDeath_Sfx;
        PlayerSoundPlayer.Play();
    }

    public void PlayAudio_Jump()
    {
        PlayerSoundPlayer.clip = PlayerJump_Sfx;
        PlayerSoundPlayer.Play();
    }

    public void PlayMoveSound()
    {
        if(TerrainFootstepParameter == 1)
        {
            PlayerMoveSoundPlayer.clip = Footstep_Concrete[Random.Range(0, Footstep_Concrete.Count)];
            PlayerMoveSoundPlayer.Play();
        }
        else if(TerrainFootstepParameter == 2)
        {
            PlayerMoveSoundPlayer.clip = Footstep_Metal[Random.Range(0, Footstep_Metal.Count)];
            PlayerMoveSoundPlayer.Play();
        }
        else if (TerrainFootstepParameter == 3)
        {
            PlayerMoveSoundPlayer.clip = Footstep_Water[Random.Range(0, Footstep_Water.Count)];
            PlayerMoveSoundPlayer.Play();
        }
    }

    public void StopAttackAudio()
    {
        AttackSoundPlayer.Stop();
        //PlayerSoundPlayer.Stop();
    }

    public void StopMovementSound()
    {
        PlayerMoveSoundPlayer.Stop();
    }

    public void OnExploded()
    {
        UIControl.OnExploded_CharPortrait();
    }

    public void PlayAudio_NullTarget()
    {
        PlayerSoundPlayer.clip = NullTargetSfx;
        PlayerSoundPlayer.Play();
    }
}
