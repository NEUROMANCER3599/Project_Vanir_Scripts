using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class WorldSpaceUIFacing : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private Camera PlayerDisplayCam;
    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponent<Canvas>();
        PlayerDisplayCam = GameObject.Find("Player_Camera").GetComponent<Camera>();

        canvas.worldCamera = PlayerDisplayCam;
    }

    
    private void LateUpdate()
    {
        transform.LookAt(transform.position + PlayerDisplayCam.transform.rotation * Vector3.forward, PlayerDisplayCam.transform.rotation * Vector3.up);
    }
}
