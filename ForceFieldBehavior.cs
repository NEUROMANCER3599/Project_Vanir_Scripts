using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ForceFieldBehavior : MonoBehaviour
{
    [Header("ForceField Component Reference")]
    //[SerializeField] private BoxCollider FieldCollider;
    [SerializeField] private NavMeshObstacle FieldNavObstacle;
    [SerializeField] private MeshRenderer FieldMeshRenderer;

    [Header("ForceField Keyswitches/WeightSwitches/KeyEnemies")]
    [SerializeField] private List<KeySwitchComponent> KeySwitchlist;
    [SerializeField] private List<WeightKeyComponent> WeightSwitchlist;
    [SerializeField] private List<EnemyBehavior> KeyEnemies;
    [SerializeField] private List<EnemySequenceKey> EnemyWaveSequenceKeyList;
    [SerializeField] private int ActivateCheck;
    
    

    
    // Start is called before the first frame update
    void Start()
    {
        //FieldCollider = GetComponent<BoxCollider>();
        FieldNavObstacle = GetComponent<NavMeshObstacle>();
        FieldMeshRenderer = GetComponent<MeshRenderer>();

        OnLocked();
    }

    // Update is called once per frame
    void Update()
    {


        if (ActivateCheck < KeySwitchlist.Count + WeightSwitchlist.Count + KeyEnemies.Count + EnemyWaveSequenceKeyList.Count)
        {
            for (int i = 0; i < KeySwitchlist.Count; i++)
            {
                if (KeySwitchlist[i].isActivated == true)
                {
                    ActivateCheck += 1;
                }
                else
                {
                    if(ActivateCheck > 0)
                    {
                        ActivateCheck -= ActivateCheck;
                    }
                }
 
            }

            for(int i = 0;i < WeightSwitchlist.Count; i++)
            {
                if (WeightSwitchlist[i].isActivated == true)
                {
                    ActivateCheck += 1;
                }
                else
                {
                    if (ActivateCheck > 0)
                    {
                        ActivateCheck -= ActivateCheck;
                    }
                }
            }

            for (int i = 0; i < KeyEnemies.Count; i++)
            {
                if (KeyEnemies[i].GetIsActive() == false)
                {
                    ActivateCheck += 1;
                }
                else
                {
                    if (ActivateCheck > 0)
                    {
                        ActivateCheck -= ActivateCheck;
                    }
                }
            }

            for(int i = 0; i < EnemyWaveSequenceKeyList.Count; i++)
            {
                if (EnemyWaveSequenceKeyList[i].IsFinished){

                    ActivateCheck += 1;
                }
                else
                {
                    if (ActivateCheck > 0)
                    {
                        ActivateCheck -= ActivateCheck;
                    }
                }
            }

            OnLocked();
        }
        else
        {
            OnUnlocked();
        }

    }

    public void OnLocked()
    {
        
        //FieldCollider.enabled = true;
        FieldNavObstacle.carving = true;
        FieldMeshRenderer.enabled = true;
        
        //gameObject.SetActive(true);
        gameObject.layer = 6;
    }

    public void OnUnlocked()
    {
        
        
        //FieldCollider.enabled = false;
        FieldNavObstacle.carving = false;
        FieldMeshRenderer.enabled = false;
        
        //gameObject.SetActive(false);
        gameObject.layer = 0;
        Destroy(gameObject);
    }

    
}
