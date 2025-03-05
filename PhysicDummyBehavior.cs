using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PhysicDummyBehavior : MonoBehaviour
{
    [SerializeField] private GameObject DmgTxtObj;
    [SerializeField] private TextMeshPro DmgTxt;
    [SerializeField] private PlayerBehavior PlayerSystem;
    // Start is called before the first frame update
    void Start()
    {
        PlayerSystem = GameObject.FindAnyObjectByType<PlayerBehavior>();
    }

    // Update is called once per frame
    void Update()
    {

        //Rotating Txt

        Vector3 Direction = PlayerSystem.transform.position - transform.position;

        Quaternion rotation = Quaternion.LookRotation(new Vector3(-Direction.x, 0, Direction.z));

        DmgTxtObj.transform.rotation = rotation;

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<ObjectPickup>() == true && other.gameObject.CompareTag("PhysicObj"))
        {
            DmgTxt.text = other.gameObject.GetComponent<ObjectPickup>().PhysicsDmg.ToString();
        }
    }

}
