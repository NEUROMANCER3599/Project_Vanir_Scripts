using System.Runtime.Serialization;
using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class GenericPhysicObjectCustomizer : MonoBehaviour
{
    [Header("Physic Object Customizer | Auto Customizer Option")]
    [SerializeField] private bool IsAutoCustomize;
    [SerializeField] private bool IsRandomize;
    [Header("Subtype Selection | 1 = Small, 2 = Medium, 3 = Large")]
    [SerializeField] private int SubType;
    [Header("Physic Object Customizer | Manual Customizing")]
    [SerializeField] private float ObjectHP = 100;
    [SerializeField] private float ObjectMass;
    [SerializeField] private float ObjectScale;

    [Header("Physic Object Customizer | Auto Customizer Parameters")]
    [SerializeField] private float SSizeType_MinWeight;
    [SerializeField] private float SSizeType_MaxWeight;
    [SerializeField] private float SSizeType_MinScale;
    [SerializeField] private float SSizeType_MaxScale;
    [SerializeField] private float MSizeType_MinWeight;
    [SerializeField] private float MSizeType_MaxWeight;
    [SerializeField] private float MSizeType_MinScale;
    [SerializeField] private float MSizeType_MaxScale;
    [SerializeField] private float LSizeType_MinWeight;
    [SerializeField] private float LSizeType_MaxWeight;
    [SerializeField] private float LSizeType_MinScale;
    [SerializeField] private float LSizeType_MaxScale;
    [SerializeField] private float BaseObjHP;

    [Header("Texture Variants Customization")]
    [SerializeField] private bool RandomizedVisual;
    [SerializeField] private int TextureTypeIndex = 0;
    [SerializeField] private List<Material> Materials;
    private bool ValidVisualTypeCheck;


    [Header("Components")]
    private Rigidbody rb;
    private bool ValidTypeCheck = false;
    private ObjectPickup PhysicObjAttribute;
    private float ScaleSum;
    private float WeightSum;
    private MeshRenderer meshRenderer;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        rb = GetComponent<Rigidbody>();
        PhysicObjAttribute = rb.GetComponent<ObjectPickup>();

        if (RandomizedVisual)
        {
            while (!ValidVisualTypeCheck)
            {
                TextureTypeIndex = Random.Range(0, Materials.Count + 1);

                if(TextureTypeIndex < Materials.Count)
                {
                    ValidVisualTypeCheck = true;
                }
            }
        }

        if (IsRandomize)
        {
            while (!ValidTypeCheck)
            {
                SubType = Random.Range(1, 4);

                if (SubType < 4)
                {
                    ValidTypeCheck = true;
                }
            }
        }

        if (IsAutoCustomize)
        {
            switch (SubType)
            {
                case 1:
                    ScaleSum = Random.Range(SSizeType_MinScale, SSizeType_MaxScale);
                    WeightSum = Random.Range(SSizeType_MinWeight, SSizeType_MaxWeight);
                    break;
                case 2:
                    ScaleSum = Random.Range(MSizeType_MinScale, MSizeType_MaxScale);
                    WeightSum = Random.Range(MSizeType_MinWeight, MSizeType_MaxWeight);
                    break;
                case 3:
                    ScaleSum = Random.Range(LSizeType_MinScale, LSizeType_MaxScale);
                    WeightSum = Random.Range(LSizeType_MinWeight, LSizeType_MaxWeight);
                    break;
            }

            WeightSum = Mathf.Round(WeightSum);
            ScaleSum = Mathf.Round(ScaleSum);

            transform.localScale = new Vector3(ScaleSum,ScaleSum,ScaleSum);
            rb.mass = WeightSum;
            
        }

        meshRenderer.material = Materials[TextureTypeIndex];
        ObjectHP = (ScaleSum + WeightSum) * BaseObjHP;
        PhysicObjAttribute.HP = ObjectHP;
    }


    void Update()
    {
        
    }
}
