using UnityEngine;

[RequireComponent(typeof(InteractionModule))]
public class HeathChargeBehavior : MonoBehaviour
{
    [Header("Customizable Parameters")]
    [SerializeField] private float HealthRegainMultiplier;
    [SerializeField] private bool IsInfinite;
    [SerializeField] private int TimeActivatable;

    [Header("Components")]
    [SerializeField] private AudioSource SoundPlayer;
    [SerializeField] private AudioClip SuccessSfx;
    [SerializeField] private AudioClip FailSfx;
    [SerializeField] private Transform AuraSpawnPoint;
    [SerializeField] private GameObject AuraProjectile;
    private InteractionModule interactionModule;

    [Header("Visual Components")]
    [SerializeField] private MeshRenderer CenterCoreRenderer;
    [SerializeField] private Material ActivatedMaterial;
    [SerializeField] private Material DeactivatedMaterial;

    private PlayerBehavior player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindAnyObjectByType<PlayerBehavior>();
        SoundPlayer = GetComponent<AudioSource>();
        interactionModule = GetComponent<InteractionModule>();
        interactionModule.Is_Infinitely_Repeatable = IsInfinite;
        interactionModule.Activate_Count = TimeActivatable;

    }
    public void OnActivated()
    {
        if (IsInfinite)
        {
            ActivationFunc();
        }
        else
        {
            if (TimeActivatable > 0)
            {
                ActivationFunc();
                TimeActivatable--;
            }
        }


    }

    void ActivationFunc()
    {
        SoundPlayer.clip = SuccessSfx;
        SoundPlayer.Play();
        player.InstantHPRecover(HealthRegainMultiplier);
        Instantiate(AuraProjectile, AuraSpawnPoint);
    }

    public void OnDenied()
    {
        SoundPlayer.clip = FailSfx;
        SoundPlayer.Play();
    }

    private void Update()
    {
        if (!IsInfinite)
        {
            if (TimeActivatable > 0)
            {
                CenterCoreRenderer.material = ActivatedMaterial;
            }
            else
            {
                CenterCoreRenderer.material = DeactivatedMaterial;
            }
        }
        else
        {
            CenterCoreRenderer.material = ActivatedMaterial;
        }
    }
}
