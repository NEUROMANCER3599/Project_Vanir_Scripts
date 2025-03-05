using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WreckingBallBehavior : MonoBehaviour
{
    private ConstantForce ConstantForceComponent;
    [SerializeField] private float X_Axis, Y_Axis, Z_Axis;
    [SerializeField] private float Time;
    // Start is called before the first frame update
    void Start()
    {
        ConstantForceComponent = GetComponent<ConstantForce>();
        Invoke(nameof(ChangeDirection), Time);
    }

    // Update is called once per frame
    void Update()
    {
        ConstantForceComponent.force = new Vector3(X_Axis, Y_Axis, Z_Axis);
    }

    void ChangeDirection()
    {
        X_Axis = X_Axis * -1;
        Y_Axis = Y_Axis * -1;
        Z_Axis = Z_Axis * -1;
        Invoke(nameof(ChangeDirection), Time);
    }
}
