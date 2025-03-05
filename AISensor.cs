using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISensor : MonoBehaviour
{
    public float distance = 10;
    public float Detectangle = 30;
    //[SerializeField] private float height = 1;
    //[SerializeField] private Color MeshColor = Color.green;
    //public int scanFrequency = 30;
    //public LayerMask layers;
    public LayerMask occlusionLayers;
    //public List<GameObject> Objects = new List<GameObject>();
    /*
    Collider[] colliders = new Collider[50];
    Mesh mesh;
    int count;
    float scanInterval;
    float scanTimer;
    */

    bool IsInRange;
    bool IsNotHidden;
    bool IsInAngle;
    private PlayerBehavior player;

    void Start()
    {
        player = FindAnyObjectByType<PlayerBehavior>();
        //scanInterval = 1f / scanFrequency;
    }


    void Update()
    {
        /*
        scanTimer -= Time.deltaTime;
        if (scanTimer < 0)
        {
            scanTimer += scanInterval;
            Scan();
        }
        */
        
        Scan();
        
    }

    private void Scan()
    {
        /*
        count = Physics.OverlapSphereNonAlloc(transform.position, distance, colliders, layers, QueryTriggerInteraction.Collide);

        //Objects.Clear();
        
        for (int i = 0; i < count; ++i)
        {
            GameObject obj = colliders[i].gameObject;
            if (IsInSight(obj))
            {
                Objects.Add(obj);
            }
            else
            {
                Objects.Clear();
            }
        }
        */

        if(Vector3.Distance(transform.position, player.transform.position) < distance)
        {
            IsInRange = true;
            Debug.Log("Player Is In Range");
        }
        else
        {
            IsInRange = false;
            Debug.Log("Player Is Not In Range");
        }

        if(!Physics.Linecast(transform.position, player.transform.position, occlusionLayers))
        {
            IsNotHidden = true;
            Debug.Log("Player Is Not Hidden");
        }
        else
        {
            IsNotHidden = false;
            Debug.Log("Player Is Hidden");
        }

        Vector3 side1 = player.transform.position - transform.position;
        Vector3 side2 = transform.forward;

        float angle = Vector3.SignedAngle(side1, side2, Vector3.up);
        if(angle < Detectangle && angle > -1 * Detectangle)
        {
            IsInAngle = true;
            Debug.Log("Player Is In Angle");
        }
        else
        {
            IsInAngle = false;
            Debug.Log("Player Is Not In Angle");
        }

        
    }

    public bool IsInSight(GameObject Obj)
    {
        /*
        Vector3 origin = transform.position;
        Vector3 dest = Obj.transform.position;
        Vector3 direction = dest - origin;
        direction.y = 0;
        float deltaAngle = Vector3.Angle(direction, transform.forward);
        origin.y += height / 2;
        dest.y = origin.y;


        if (direction.y < 0 || direction.y > height)
        {
            return false;
        }
        else if (deltaAngle > angle)
        {
            return false;
        }
        else if (Physics.Linecast(origin, dest, occlusionLayers))
        {
            return false;
        }
        else if (direction.x > distance || direction.z > distance || direction.x < -distance || direction.z < -distance)
        {
            return false;
        }
        else
        {
            return true;
        }
        */

        if(IsInRange && IsNotHidden && IsInAngle)
        {
            return true;
        }
        else
        {
            return false;
        }
        


       
    }

    /*
    Mesh CreateWedgeMesh()
    {
        Mesh mesh = new Mesh();

        int segments = 10;
        int numTriangles = (segments * 4) + 2 + 2;
        int numVertices = numTriangles * 3;

        Vector3[] vertices = new Vector3[numVertices];
        int[] triangles = new int[numVertices];

        Vector3 bottomCenter = Vector3.zero;
        Vector3 bottomLeft = Quaternion.Euler(0, -angle, 0) * Vector3.forward * distance;
        Vector3 bottomRight = Quaternion.Euler(0, angle, 0) * Vector3.forward * distance;

        Vector3 topCenter = bottomCenter + Vector3.up * height;
        Vector3 topRight = bottomRight + Vector3.up * height;
        Vector3 topLeft = bottomLeft + Vector3.up * height;

        int vert = 0;

        //Left Side
        vertices[vert++] = bottomCenter;
        vertices[vert++] = bottomLeft;
        vertices[vert++] = topLeft;

        vertices[vert++] = topLeft;
        vertices[vert++] = topCenter;
        vertices[vert++] = bottomCenter;
        //Right Side
        vertices[vert++] = bottomCenter;
        vertices[vert++] = topCenter;
        vertices[vert++] = topRight;

        vertices[vert++] = topRight;
        vertices[vert++] = bottomRight;
        vertices[vert++] = bottomCenter;

        float currentAngle = -angle;
        float deltaAngle = (angle * 2) / segments;
        for (int i = 0; i < segments; ++i)
        {
            bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * distance;
            bottomRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * distance;

            topRight = bottomRight + Vector3.up * height;
            topLeft = bottomLeft + Vector3.up * height;

            //Far Side
            vertices[vert++] = bottomLeft;
            vertices[vert++] = bottomRight;
            vertices[vert++] = topRight;

            vertices[vert++] = topRight;
            vertices[vert++] = topLeft;
            vertices[vert++] = bottomLeft;
            //Top
            vertices[vert++] = topCenter;
            vertices[vert++] = topLeft;
            vertices[vert++] = topRight;
            //Bottom
            vertices[vert++] = bottomCenter;
            vertices[vert++] = bottomRight;
            vertices[vert++] = bottomLeft;

            currentAngle += deltaAngle;
        }


        for (int i = 0; i < numVertices; i++)
        {
            triangles[i] = i;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();


        return mesh;
    }

    private void OnValidate()
    {
        mesh = CreateWedgeMesh();
        scanInterval = 1f / scanFrequency;
    }

    private void OnDrawGizmos()
    {
        //Debugging
        if (mesh)
        {
            Gizmos.color = MeshColor;
            Gizmos.DrawMesh(mesh, new Vector3(transform.position.x, transform.position.y - transform.localScale.y, transform.position.z), transform.rotation);
        }

        Gizmos.DrawWireSphere(transform.position, distance);
        for (int i = 0; i < count; i++)
        {
            Gizmos.DrawSphere(colliders[i].transform.position, 1f);
        }

        Gizmos.color = Color.green;
        foreach (var obj in Objects)
        {
            Gizmos.DrawSphere(obj.transform.position, 1f);
        }
    }

    public void ConfigureSensor(float dist, float ang)
    {
        distance = dist;
        angle = ang;
    }
    */
}
