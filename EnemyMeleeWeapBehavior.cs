using UnityEngine;

public class EnemyMeleeWeapBehavior : MonoBehaviour
{
    [SerializeField] private GameObject HitParticle;
    [SerializeField] private EnemyBehavior enemy;
    [SerializeField] private GameObject BladeModel;
    private float Damage = 30;
    private float AttackDuration;
    private PlayerBehavior player;
    private CapsuleCollider WeaponCol;
    private AudioSource MeleeWeaponAmbiSoundPlayer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindAnyObjectByType<PlayerBehavior>();
        WeaponCol = GetComponent<CapsuleCollider>();
        MeleeWeaponAmbiSoundPlayer = GetComponent<AudioSource>();
        WeaponCol.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!enemy.IsActive)
        {
            OnFinishingAttack();
        }
    }

    public void OnAttacking()
    {
        WeaponCol.enabled = true;
        //Debug.Log("StartAttacking!");
        //Invoke(nameof(OnFinishingAttack),AttackDuration);
    }

    public void OnFinishingAttack()
    {
        WeaponCol.enabled = false;
        MeleeWeaponAmbiSoundPlayer.Stop();
        BladeModel.SetActive(false);
        //Debug.Log("FinishAttacking!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerBehavior>())
        {
            Debug.Log("player slashed!");
            player.HP -= Damage;
            Instantiate(HitParticle,other.gameObject.transform.localPosition + new Vector3(0,0.6f,0),Quaternion.identity);
        }
    }

    public void SetDmg(float dmg)
    {
        Damage = dmg;
    }

    public void SetAtkDuration(float atkDuration)
    {
        AttackDuration = atkDuration;
    }
}
