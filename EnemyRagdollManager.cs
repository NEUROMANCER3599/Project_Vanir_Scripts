using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRagdollManager : MonoBehaviour
{
    [SerializeField] private List<Rigidbody> ragdollparts;
    [SerializeField] private List<GameObject> ModelParts;
    public bool IsDead = false;
    private PlayerBehavior mPlayer;
    private LevelSystemManagement LevelSystem;
    // Start is called before the first frame update
    void Start()
    {
        mPlayer = GameObject.FindAnyObjectByType<PlayerBehavior>();
        LevelSystem = FindAnyObjectByType<LevelSystemManagement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && mPlayer.IsTargetingValidObj && !LevelSystem.IsPaused)
        {
            if (!IsDead)
            {
                for(int i = 0; i < ModelParts.Count; i++)
                {
                    ModelParts[i].layer = 9;
                }
               // GunModel.layer = 9;
            }
        }
        else
        {
            for (int i = 0; i < ModelParts.Count; i++)
            {
                ModelParts[i].layer = 10;
            }
            //GunModel.layer = 10;
        }
    }

   public void OnDeath(float forcepower)
    {
        IsDead = true;
        for(int i = 0; i < ragdollparts.Count; i++)
        {
            ragdollparts[i].isKinematic = false;
            ragdollparts[i].AddExplosionForce(forcepower, gameObject.transform.position, 1, 0, ForceMode.Impulse);
        }
    }
}
