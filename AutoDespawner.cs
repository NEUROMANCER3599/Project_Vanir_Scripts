
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDespawner : MonoBehaviour
{
    [SerializeField] private float DespawnTime;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject,DespawnTime);
    }


    // Update is called once per frame
    void Update()
    {
        
    }


}
