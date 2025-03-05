using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HoldPointAdjustment : MonoBehaviour
{

    [SerializeField] private float mindistance, maxdistance,defaultdistance,currentdistance;
    [SerializeField] private GameObject HoldAura;
    [SerializeField] private GameObject PushBlastParticle;
    private PlayerBehavior mPlayerBehavior;


    // Start is called before the first frame update
    void Start()
    {
        transform.localPosition = new Vector3(0, 0, defaultdistance);
        mPlayerBehavior = GameObject.FindAnyObjectByType<PlayerBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
       

        float Increment = Input.GetAxis("Mouse ScrollWheel");

        currentdistance += ((Increment * 1000f) * Time.deltaTime);


        if (Input.GetMouseButton(0) && mPlayerBehavior.IsPowerDepleted == false && mPlayerBehavior.IsTargetingValidObj == true)
        {
            HoldAura.SetActive(true);
        }
        else
        {
            HoldAura.SetActive(false);
        }
        

        if(currentdistance > maxdistance)
        {
            currentdistance = maxdistance;
        }
        if(currentdistance < mindistance)
        {
            currentdistance = mindistance;
        }

        Vector3 newPosition = new Vector3(0, 0, currentdistance);
        // Adjust the object's position
        transform.localPosition = newPosition;
    }

    public void PushBlast()
    {
        Instantiate(PushBlastParticle,transform.position,Quaternion.identity);
    }
}
