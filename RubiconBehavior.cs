using UnityEngine;

public class RubiconBehavior : MonoBehaviour
{
    [SerializeField] private PlayerBehavior playerSystem;
    [SerializeField] private int perkpointreserve;
    [SerializeField] private GameObject CoreObject;
    [SerializeField] private AudioSource SoundPlayer;
    private bool IsActivated = false;
    void Start()
    {
        playerSystem = GameObject.FindAnyObjectByType<PlayerBehavior>();
        SoundPlayer = GetComponent<AudioSource>();
    }
    
    public void OnActivated()
    {
        if (!IsActivated)
        {
            playerSystem.PerkPoint += perkpointreserve;
            SoundPlayer.Play();
            CoreObject.SetActive(false);
            IsActivated = true;
        }
    }
    

}
