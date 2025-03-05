using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticPropLauncher : MonoBehaviour
{
    [Header("Customizable Parameter")]
    [SerializeField] private List<GameObject> PropList;
    [SerializeField] private float MinSpawnTime;
    [SerializeField] private float MaxSpawnTime;
    [SerializeField] private float LaunchForce;
    [SerializeField] private Transform SpawnPoint;
    private AudioSource LaunchSound;
    private float SpawnInterval;
    // Start is called before the first frame update
    void Start()
    {
        SpawnInterval = Random.Range(MinSpawnTime,MaxSpawnTime);
        LaunchSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        SpawnInterval -= 1f * Time.deltaTime;

        if(SpawnInterval < 0f)
        {
            var Object = Instantiate(PropList[Random.Range(0, PropList.Count)],SpawnPoint.position,Quaternion.identity);
            Rigidbody rb = Object.GetComponent<Rigidbody>();
            ObjectPickup objPickup = Object.GetComponent<ObjectPickup>();

            rb.AddForce(Object.transform.forward * LaunchForce,ForceMode.Impulse);
            objPickup.playerpicked = true;

            LaunchSound.Play();
            SpawnInterval = Random.Range(MinSpawnTime, MaxSpawnTime);
        }
    }
}
