using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemySequenceKey : MonoBehaviour
{
    [Header("Enemy Wave Sequence Parameters")]
    [SerializeField] private float ActivateTime;
    [SerializeField] private List<GameObject> EnemyList;
    [SerializeField] private List<Transform> EnemySpawnPoints;
    [SerializeField] private float MinSpawnTime;
    [SerializeField] private float MaxSpawnTime;
    [Header("Enemy Wave Sequence Components")]
    [SerializeField] private TextMeshProUGUI StatusText;
    [SerializeField] private Material ActivatedMat;
    [SerializeField] private Material DeactivatedMat;
    [SerializeField] private Color32 deactivatedColor;
    [SerializeField] private Color32 activatedColor;
    private bool IsActivated = false;
    private float TimerInterval;
    private float SpawnInterval;
    private Renderer SwitchObjectRenderer;
    private AudioSource SoundEffectPlayer;
    [Header("Enemy Wave Sequence Check")]
    public bool IsFinished = false;
    void Start()
    {
        SwitchObjectRenderer = GetComponent<Renderer>();
        SoundEffectPlayer = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsActivated && !IsFinished)
        {
            TimerInterval -= 1f * Time.deltaTime;
            SpawnInterval -= 1f * Time.deltaTime;

            if(TimerInterval <= 0)
            {
                IsFinished = true;
            }

            if(SpawnInterval <= 0)
            {
                SpawnEnemies();
            }
        }

        if (IsFinished)
        {
            StatusText.color = activatedColor;
            SwitchObjectRenderer.material = ActivatedMat;
            StatusText.text = "Switch Activated";
        }
        else
        {
            StatusText.color = deactivatedColor;
            SwitchObjectRenderer.material = DeactivatedMat;

            if (IsActivated)
            {
                StatusText.text = Mathf.RoundToInt(TimerInterval).ToString();
            }
            else
            {
                StatusText.text = "Switch deactivated";
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.GetComponent<PlayerBehavior>() != null)
        {
            if (!IsActivated)
            {
                SoundEffectPlayer.Play();
                IsActivated = true;
                TimerInterval = ActivateTime;
                SpawnInterval = Random.Range(MinSpawnTime, MaxSpawnTime);
            }
           
        }
    }

    public void SpawnEnemies()
    {
        for(int i = 0; i < EnemySpawnPoints.Count; i++)
        {
            Instantiate(EnemyList[Random.Range(0,EnemyList.Count)],EnemySpawnPoints[i].position,Quaternion.identity);
        }

        SpawnInterval = Random.Range(MinSpawnTime, MaxSpawnTime);
    }
}
