using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// General utility methods, with a static wrapper instance
/// </summary>

namespace HiFi
{
    public class HiFi_Utilities : MonoBehaviour
    {
        public static HiFi_Utilities instance = null;

        ///vars for AngularVelocity
        static Quaternion deltaRotation;
        static float angle;             ///also used for EulerTo180
        static Vector3 axis;
        static Rigidbody transferRB;

        ///vars for Detach
        static Rigidbody tempRB;

        ///for debugprint
        [Header("DebugText options")]
        public bool mirrorDebugToUnityConsole;
        public static event Action<string, bool> OnDebugEvent;

        ///********************************************************************************

        private void Awake()
        {
            instance = this;
        }

        ///********************************************************************************

        #region DebugPrint/Display

        /// <summary>
        /// Overloaded method that routes incoming strings to the LineAdderScroller of assigned Text objects.
        /// </summary>
        /// <param name="incomingText"></param>
        /// <param name="muteDuplicates"></param>
        public static void DebugText(string incomingText, bool muteDuplicates = false)
        {
            instance.DebugPrint(incomingText, muteDuplicates);
        }
        /// <summary>
        /// Overloaded method that routes incoming Lists of strings to the LineAdderScroller of assigned Text objects.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="message"></param>
        public static void DebugText(List<string> list, string message = null)
        {
            instance.DebugDisplayList(list, message);
        }
        /// <summary>
        /// Overloaded method that routes incoming array of strings to the LineAdderScroller of assigned Text objects.
        /// </summary>
        /// <param name="strings"></param>
        /// <param name="message"></param>
        public static void DebugText(string[] strings, string message = null)
        {
            instance.DebugDisplayList(strings, message);
        }

        //prints incoming text lines to HiFi_TextLineAdderScroller devices
        private void DebugPrint(string incomingText, bool muteDuplicates = true)
        {
            OnDebugEvent?.Invoke(incomingText, muteDuplicates);

            if (mirrorDebugToUnityConsole)
                Debug.Log(incomingText);
        }

        //prints out a list of strings
        private void DebugDisplayList(List<string> list, string message = null)   //prints list to debug panel
        {
            int num = 1;
            DebugPrint(message);
            foreach (string item in list)
            {
                DebugPrint(num + " " + item);
                num++;
            }
        }

        //prints out an array of strings
        private void DebugDisplayList(string[] strings, string message = null)   //prints list to debug panel
        {
            int num = 1;
            DebugPrint(message);
            foreach (string line in strings)
            {
                DebugPrint(num + " " + line);
                num++;
            }
        }
        #endregion

        #region Physics Layers

        /// <summary>
        /// Returns current Kinetic Energy of a Rigidbody (in Joules)
        /// </summary>
        /// <param name="rb"></param>
        /// <returns></returns>
        public static float KineticEnergy(Rigidbody rb)
        {
            // mass in kg, velocity in meters per second, result is joules
            return 0.5f * rb.mass * Mathf.Pow(rb.velocity.magnitude, 2);
        }

        /// <summary>
        /// Returns true if a specific layer is in a layermask
        /// </summary>
        /// <param name="layer"></param>
        /// <param name="layermask"></param>
        /// <returns></returns>
        public static bool IsInLayerMask(int layer, LayerMask layermask)
        {
            return layermask == (layermask | (1 << layer));
        }

        /// <summary>
        /// Returns current physics layermask of a GameObject
        /// </summary>
        /// <param name="go"></param>
        /// <returns></returns>
        public static LayerMask GetCollisionMaskOf(GameObject go)
        {
            int myLayer = go.layer;
            int layerMask = 0;
            for (int i = 0; i < 32; i++)
            {
                if (!Physics.GetIgnoreLayerCollision(myLayer, i))
                {
                    layerMask = layerMask | 1 << i;
                }
            }
            return layerMask;
        }

        /// <summary>
        /// Set physics engine to ignore collisions between given gameObject and all layers in given LayerMask
        /// </summary>
        public static void SetIgnoreLayers(GameObject go, LayerMask alwaysIgnoreLayers)
        {
            int goLayer = go.layer;
            uint bitstring = (uint)alwaysIgnoreLayers.value;
            for (int layer = 31; bitstring > 0; layer--)
                if ((bitstring >> layer) > 0)
                {
                    bitstring = ((bitstring << 32 - layer) >> 32 - layer);
                    Physics.IgnoreLayerCollision(goLayer, layer, true);
                }
        }

        /// <summary>
        /// Set Physics to ignore or resume collisions with characterColliderToggleLayers
        /// </summary>
        /// <param name="toggle"></param>
        public static void ToggleIgnoreLayers(GameObject go, LayerMask ignoreLayers, bool ignore = true)
        {
            int goLayer = go.layer;
            for (int layer = 31; layer >= 0; layer--)
            {
                if (HiFi_Utilities.IsInLayerMask(layer, ignoreLayers))
                    Physics.IgnoreLayerCollision(goLayer, layer, ignore);
            }
        }

        #endregion

        #region Offsets and Alignment

        /// <summary>
        /// Returns a Pose containing offsets of transform a to transform b
        /// </summary>
        public static Pose GetOffsetPose(Transform a, Transform b)
        {
            Pose relPose = new Pose();

            Vector3 relPos = a.position - b.position;
            relPose.position = Quaternion.Inverse(b.rotation) * relPos;

            relPose.rotation = Quaternion.Inverse(b.rotation) * a.rotation;

            return relPose;
        }

        #endregion

        #region Angles

        /// <summary>
        /// Converts Euler -180-270-360 to 180-(-90)-(0)
        /// </summary>
        /// <param name="eAngle"></param>
        /// <returns></returns>
        public static float EulerToRelative(float eAngle)
        {
            if (eAngle <= 180)
                return eAngle;
            else
                return -(360 - eAngle);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static float RelativeToEuler(float angle)
        {
            if (angle < 0)
                return angle + 360;
            else
                return angle;
        }

        /// <summary>
        /// From Unity user whidoidoit. 
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static float ClampAngle(float angle, float min, float max)
        {
            angle = Mathf.Repeat(angle, 360);
            min = Mathf.Repeat(min, 360);
            max = Mathf.Repeat(max, 360);
            bool inverse = false;
            var tmin = min;
            var tangle = angle;
            if (min > 180)
            {
                inverse = !inverse;
                tmin -= 180;
            }
            if (angle > 180)
            {
                inverse = !inverse;
                tangle -= 180;
            }
            var result = !inverse ? tangle > tmin : tangle < tmin;
            if (!result)
                angle = min;

            inverse = false;
            tangle = angle;
            var tmax = max;
            if (angle > 180)
            {
                inverse = !inverse;
                tangle -= 180;
            }
            if (max > 180)
            {
                inverse = !inverse;
                tmax -= 180;
            }

            result = !inverse ? tangle < tmax : tangle > tmax;
            if (!result)
                angle = max;
            return angle;
        }

        #endregion

        #region Physics

        /// <summary>
        /// Calculates angular rotation
        /// </summary>
        /// <param name="currentRotation"></param>
        /// <param name="lastFrameRotation"></param>
        /// <returns></returns>
        public static Vector3 AngularVelocity(Quaternion currentRotation, Quaternion lastFrameRotation)
        {
            deltaRotation = currentRotation * Quaternion.Inverse(lastFrameRotation);

            angle = 0;
            axis = Vector3.zero;

            deltaRotation.ToAngleAxis(out angle, out axis);

            angle *= Mathf.Deg2Rad;

            return (axis * angle * (1.0f / Time.deltaTime));
        }

        /// <summary>
        /// Adds matching force to target's rigidbody 
        /// </summary>
        /// <param name="forceObj"></param>
        /// <param name="target"></param>
        /// <param name="lastFramePosition"></param>
        /// <param name="lastFrameRotation"></param>
        /// <param name="physicsMultiplier"></param>
        public static void TransferForces(GameObject fromObject, GameObject toObject, Vector3 lastFramePosition, Quaternion lastFrameRotation, float physicsMultiplier = 1)
        {
            transferRB = toObject.GetComponent<Rigidbody>();
            if(transferRB != null)
            {
                transferRB.isKinematic = false;
                transferRB.velocity = (fromObject.transform.position - lastFramePosition) / Time.deltaTime * physicsMultiplier;
                transferRB.angularVelocity = AngularVelocity(fromObject.transform.rotation, lastFrameRotation) * physicsMultiplier;
                //HiFi_Utilities.DebugText("Transferring Forces from: " + fromObject + "to: " + toObject + "...velocity: " + transferRB.velocity + "angular velocity: " + transferRB.angularVelocity);
            }
            transferRB = null;
        }

        #endregion

        #region Find Closest Object

        /// <summary>
        /// Returns a transform from an array that is closest to a given transform
        /// </summary>
        /// <param name="array"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static Transform FindClosestTransform(Transform[] array, Transform point)  //takes a Transform[] and returns the closest transform
        {
            if (array.Length == 0)
                return point;

            Transform closest = array[0];

            for (int t = 0; t < array.Length; t++)
            {            
                if (Vector3.Distance(point.position, array[t].position) < Vector3.Distance(point.position, closest.position))
                    closest = array[t];
            }
            return closest;
        }
        public static T FindClosest<T>(T[] array, Transform point) where T : MonoBehaviour //BUG - doesn't seem to be working? needs test
        {
            if (array.Length == 0)
                return null;

            T closestG = array[0];

            for (int t = 0; t < array.Length; t++)
            {
                if(Vector3.Distance(point.position, array[t].transform.position) < Vector3.Distance(point.position, closestG.transform.position))
                    closestG = array[t];
            }
            HiFi_Utilities.DebugText("Static - Detected: " + closestG.name, true);
            return closestG;
        }

        #endregion

        #region Detach Objects - overloaded methods to detach/unparent object from another object with or without physics

        /// <summary>
        /// Unparent object with physics
        /// </summary>
        /// <param name="parentObj"></param>
        /// <param name="childObj"></param>
        /// <param name="lastFramePosition"></param>
        /// <param name="lastFrameRotation"></param>
        /// <param name="physicsMultiplier"></param>
        public static void DetachObjects(Transform parentObj, Transform childObj, Vector3 lastFramePosition, Quaternion lastFrameRotation, float physicsMultiplier = 1)
        {
            //look for RigidBody
            tempRB = childObj.GetComponent<Rigidbody>();
            if (tempRB == null)
                childObj.gameObject.AddComponent<Rigidbody>();

            tempRB.isKinematic = false;
            tempRB.velocity = (parentObj.position - lastFramePosition) / Time.deltaTime * physicsMultiplier;
            tempRB.angularVelocity = AngularVelocity(parentObj.rotation, lastFrameRotation) * physicsMultiplier;

            childObj.parent = null;
            tempRB = null;
        }
        /// <summary>
        /// Unparent object with physics, and reparent
        /// </summary>
        /// <param name="parentObj"></param>
        /// <param name="childObj"></param>
        /// <param name="newParent"></param>
        /// <param name="lastFramePosition"></param>
        /// <param name="lastFrameRotation"></param>
        /// <param name="physicsMultiplier"></param>
        public static void DetachObjects(Transform parentObj, Transform childObj, Transform newParent, Vector3 lastFramePosition, Quaternion lastFrameRotation, float physicsMultiplier = 1)
        {
            childObj.parent = newParent;
         
            //look for RigidBody
            tempRB = childObj.GetComponent<Rigidbody>();
            if (tempRB == null)
                childObj.gameObject.AddComponent<Rigidbody>();

            tempRB.isKinematic = false;
            tempRB.velocity = (parentObj.position - lastFramePosition) / Time.deltaTime * physicsMultiplier;
            tempRB.angularVelocity = AngularVelocity(parentObj.rotation, lastFrameRotation) * physicsMultiplier;

            tempRB = null;
        }

        #endregion
    }
}

