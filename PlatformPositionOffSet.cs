using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPositionOffSet : MonoBehaviour
{
    private GameObject target;
    private Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        target = null;
    }
 

    private void OnCollisionStay(Collision col)
    {
        //target.Add(col.gameObject);
        target = col.gameObject;
        offset = target.transform.position - transform.position;
        /*
        for (int i = 0; i < target.Count; i++)
        {
            offset = target[i].transform.position - transform.position;
        }
        */
       
    }

    private void OnCollisionExit(Collision collision)
    {
        target = null;
        
    }

    void LateUpdate()
    {
        if (target != null)
        {
            target.transform.position = transform.position + offset;
            /*
            for(int i = 0;i < target.Count; i++)
            {
                target[i].transform.position = transform.position + offset;
            }
            */
        }
    }
}
