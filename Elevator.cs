using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] private GameObject Platform;
    [SerializeField] private Transform Point_A, Point_B;
    [SerializeField][Range(0, 1)] private float LERPDistance;
    [SerializeField] private float ElevatorSpeed;
    public bool IsActivated;
    private bool IsUp;
    public LevelSystemManagement levelSystem;
    public AudioSource ElevatorSound;
    // Start is called before the first frame update
    void Start()
    {
        levelSystem = GameObject.FindAnyObjectByType<LevelSystemManagement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsActivated)
        {
            ElevatorFunction();
        }
    }

    public void ElevatorCondition()
    {
        if (Platform.transform.position == Point_A.position)
        {
            ElevatorSoundPlay();
            IsUp = true;
        }
        else if (Platform.transform.position == Point_B.position)
        {
            ElevatorSoundPlay();
            IsUp = false;
        }
    }

    void ElevatorFunction()
    {
       
            if (IsUp)
            {
                
                    if (LERPDistance < 1)
                    {
                        LERPDistance += ElevatorSpeed * Time.deltaTime;
                    }
                    else
                    {
                        IsActivated = false;
                        ElevatorSoundStop();
                    }
                
               
            }
            else
            {
               
                    if (LERPDistance > 0)
                    {
                        LERPDistance -= ElevatorSpeed * Time.deltaTime;
                    }
                    else
                    {
                        IsActivated = false;
                        ElevatorSoundStop();
                    }
                
            }

             Platform.transform.position = Vector3.Lerp(Point_A.position, Point_B.position, LERPDistance);
    }

    public void ElevatorSoundPlay()
    {
        ElevatorSound.Play();
    }

    public void ElevatorSoundStop()
    {
        ElevatorSound.Stop();
    }
}
