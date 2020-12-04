using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//************ Based on the Shape script from https://github.com/SebLague/Ray-Marching
//************ Added rotation support and 4D PSR by https://github.com/Jellevermandere/4D-Raymarching

public class Shape4D : MonoBehaviour
{
    public enum ShapeType { HyperSphere, HyperCube, DuoCylinder, plane, Cone, FiveCell, SixteenCell, Torus, Tetrahedron };
    public enum Operation { Union, Blend, Substract, Intersect};
    
    [Header("Shape Settings")]
    public ShapeType shapeType;
    public Operation operation;

    [Header("4D Transform Settings")]
    public float positionW;
    [Tooltip ("The rotation around the xw, yw and zw planes respectively")]
    public Vector3 rotationW;
    public float scaleW = 1f;

    [Header("Render Settings")]
    public Color color;
    [Range(0, 1)]
    public float transparency = 1f;
    [Range (0,1)]
    public float smoothRadius;

    [Header("Collision Settings")]
    public bool collide = true;

    [Header("4D Controller Settings")]
    public bool changePosW;
    public float MaxPosW;
    public float MinPosW;
    public bool changeRotW;
    public Vector3 MaxRotW;
    public Vector3 MinRotW;

    [HideInInspector]
    public float PosWChangeAmount = 1.5f;
    [HideInInspector]
    public Vector3 RotWChangeAmount;

    [HideInInspector]
    public int numChildren;
    Vector4 parentScale = Vector4.one;

    public void UpdateRotationW(Vector3 newRotatetion)
    {
        Debug.Log("rotationW before:" + rotationW);
        rotationW = newRotatetion;
        Debug.Log("rotationW after:" + rotationW);
    }

    // returns the 4D position of the object
    public Vector4 Position()
    {
        return new Vector4(transform.position.x, transform.position.y, transform.position.z, positionW);
    }

    //returns the 3D rotation of the object
    public Vector3 Rotation()
    {
        return transform.eulerAngles * Mathf.Deg2Rad;
    }
    //returns the 3 remaining 4D rotation axis
    public Vector3 RotationW()
    {
        return rotationW * Mathf.Deg2Rad;
    }
    
    //returns the 4D scale of the object
    public Vector4 Scale()
    {
        if (transform.parent != null && transform.parent.GetComponent<Shape4D>() != null)
        {
            parentScale = transform.parent.GetComponent<Shape4D>().Scale();
        }
        else parentScale = Vector4.one;

        return Vector4.Scale(new Vector4(transform.localScale.x, transform.localScale.y, transform.localScale.z, scaleW), parentScale);

    }

}
