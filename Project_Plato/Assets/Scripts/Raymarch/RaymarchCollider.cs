using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Mathematics.math;
using UnityEngine.SceneManagement;

//Collision method based on:https://github.com/Jellevermandere/4D-Raymarching
//added differentiation in collision detection for front/back of the raymarch objects
namespace Unity.Mathematics
{
    public class RaymarchCollider : MonoBehaviour
    {
        [Tooltip("The transforms from which the raymarcher will test the distances and apply the collision")]
        public Transform[] rayMarchTransforms;
        public Transform gravity;
        public float maxDownMovement = 0.5f;

        public CircleAnimation ani;

        private DistanceFunctions Df;
        private RaymarchCamera cameraScript;
        private bool fall = false;
        private bool restart = true;

        void Start()
        {
            cameraScript = Camera.main.GetComponent<RaymarchCamera>();
            Df = GetComponent<DistanceFunctions>();
        }

        void Update()
        {
            CheckGravity();
            RayMarch(rayMarchTransforms);
            if (transform.position.y < -10f) fall = true;
            if (fall)
            {
                ani.activate = false;
                this.GetComponent<Rigidbody>().useGravity = true;
                this.GetComponent<CapsuleCollider>().isTrigger = false;
                transform.Rotate(90.0f*Time.deltaTime*2f, 0.0f, 0.0f, Space.Self);

            }
            if(fall && restart)
            {
                StartCoroutine(restartLevel());
                restart = false;
            }

        }

        // the raymarcher checks the distance to all the given transforms, if one is less than zero, the player is moved in the opposite direction
        void RayMarch(Transform[] ro)
        {

            int nrHits = 0;

            for (int i = 0; i < ro.Length; i++)
            {
                Vector3 p = ro[i].position;
                //check hit
                float d = DistanceField(p);

                if (d < 0) //hit
                {
                    //Debug.Log("hit" + i);
                    nrHits++;
                    //collision
                    if(ro[i].tag == "Back")
                    {
                        transform.Translate(ro[i].forward * -d, Space.World);
                    }
                    else if (ro[i].tag == "Left")
                    {
                        transform.Translate(ro[i].right * -d, Space.World);
                    }
                    else if (ro[i].tag == "Right")
                    {
                        transform.Translate(ro[i].right * d, Space.World);
                    }
                    else
                    {
                        transform.Translate(ro[i].forward * d, Space.World);
                    }
                }
            }
        }

        public float DistanceField(float3 p)
        {
            float4 p4D = float4(p, cameraScript._wPosition);
            Vector3 wRot = cameraScript._wRotation * Mathf.Deg2Rad;

            if ((wRot).magnitude != 0)
            {
                p4D.xw = mul(p4D.xw, float2x2(cos(wRot.x), -sin(wRot.x), sin(wRot.x), cos(wRot.x)));
                p4D.yw = mul(p4D.yw, float2x2(cos(wRot.y), -sin(wRot.y), sin(wRot.y), cos(wRot.y)));
                p4D.zw = mul(p4D.zw, float2x2(cos(wRot.z), -sin(wRot.z), sin(wRot.z), cos(wRot.z)));

            }

            float globalDst = Camera.main.farClipPlane;

            for (int i = 0; i < cameraScript.orderedShapes.Count; i++)
            {
                //Debug.Log(cameraScript.orderedShapes.Count);
                Shape4D shape = cameraScript.orderedShapes[i];
                int numChildren = shape.numChildren;

                ///Debug.Log("p4D: " + p4D);
                float localDst = GetShapeDistance(shape, p4D);

                
                for (int j = 0; j < numChildren; j++)
                {
                    Shape4D childShape = cameraScript.orderedShapes[i + j + 1];
                    float childDst = GetShapeDistance(childShape, p4D);

                    localDst = Df.Combine(localDst, childDst, childShape.operation, childShape.smoothRadius);

                }
                i += numChildren; // skip over children in outer loop

                globalDst = Df.Combine(globalDst, localDst, shape.operation, shape.smoothRadius);
            }

            return globalDst;

        }

        // the distancefunction for the shapes
        public float GetShapeDistance(Shape4D shape, float4 p4D)
        {
            p4D -= (float4)shape.Position();
            
            p4D.xz = mul(p4D.xz, float2x2(cos(shape.Rotation().y), sin(shape.Rotation().y), -sin(shape.Rotation().y), cos(shape.Rotation().y)));
            p4D.yz = mul(p4D.yz, float2x2(cos(shape.Rotation().x), -sin(shape.Rotation().x), sin(shape.Rotation().x), cos(shape.Rotation().x)));
            p4D.xy = mul(p4D.xy, float2x2(cos(shape.Rotation().z), -sin(shape.Rotation().z), sin(shape.Rotation().z), cos(shape.Rotation().z)));

            p4D.xw = mul(p4D.xw, float2x2(cos(shape.RotationW().x), sin(shape.RotationW().x), -sin(shape.RotationW().x), cos(shape.RotationW().x)));
            p4D.zw = mul(p4D.zw, float2x2(cos(shape.RotationW().z), -sin(shape.RotationW().z), sin(shape.RotationW().z), cos(shape.RotationW().z)));
            p4D.yw = mul(p4D.yw, float2x2(cos(shape.RotationW().y), -sin(shape.RotationW().y), sin(shape.RotationW().y), cos(shape.RotationW().y)));

            //Debug.Log("p4D: " + p4D);

            switch (shape.shapeType)
            {
                case Shape4D.ShapeType.HyperCube:
                    return Df.sdHypercube(p4D, shape.Scale());

                case Shape4D.ShapeType.HyperSphere:
                    return Df.sdHypersphere(p4D, shape.Scale().x);

                case Shape4D.ShapeType.DuoCylinder:
                    return Df.sdDuoCylinder(p4D, ((float4)shape.Scale()).xy);
                case Shape4D.ShapeType.plane:
                    return Df.sdPlane(p4D, shape.Scale());
                case Shape4D.ShapeType.Cone:
                    return Df.sdCone(p4D, shape.Scale());
                case Shape4D.ShapeType.FiveCell:
                    return Df.sd5Cell(p4D, shape.Scale());
                case Shape4D.ShapeType.SixteenCell:
                    return Df.sd16Cell(p4D, shape.Scale().x);

            }

            return Camera.main.farClipPlane;
        }

        IEnumerator restartLevel()
        {
            yield return new WaitForSeconds(5f);
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name, LoadSceneMode.Single);

        }

        void CheckGravity()
        {
            Vector3 p = gravity.position;
            float d = DistanceField(p);
            if (d < 0)
            {
                //transform.Translate(Vector3.up * d, Space.World);
            }
            else
            {
                transform.Translate(Vector3.down * 0.5f, Space.World);
            }


        }
    }
}

