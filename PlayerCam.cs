using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;

    float xRotation;
    float yRotation;

    float mouseX;
    float mouseY;

    private Quaternion lookAt;
    private Quaternion targetRotation;

    private UIManager UIControl;
    // Start is called before the first frame update
    void Start()
    {
        orientation = GameObject.Find("Orientation").transform;
        UIControl = GameObject.FindAnyObjectByType<UIManager>();
        OnStart();
    }

    // Update is called once per frame
    void Update()
    {
        mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    public void OnStart()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void OnEnded()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<DisplayTextComponent>() == null)
        {
            UIControl.MiscTextDisplay("",Color.white, false);
        }
    }
}
