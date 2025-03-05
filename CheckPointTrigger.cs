using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointTrigger : MonoBehaviour
{
    [SerializeField] private LevelSystemManagement LevelManager;
    // Start is called before the first frame update
    void Start()
    {
        LevelManager = GameObject.FindAnyObjectByType<LevelSystemManagement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerBehavior>() != null)
        {
            LevelManager.ChangeCheckPoint(other.transform.position);
            Destroy(this.gameObject);
        }
    }
}
