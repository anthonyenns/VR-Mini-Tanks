using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HiFi
{
    public static class HiFi_Extensions
    {
        /// <summary>
        /// Extension method for Transforms to find a child by name by deep searching recursively in all children.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Transform FindInAllChildren(this Transform parent, string name)
        {
            var result = parent.Find(name);
            if (result != null)
                return result;
            foreach (Transform child in parent)
            {
                result = child.FindInAllChildren(name);
                if (result != null)
                    return result;
            }
            return null;
        }

        /// <summary>
        /// Extension method for GameObjects to find a child even when disabled, as long as the parent is enabled.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static GameObject FindObject(this GameObject parent, string name)
        {
            Transform[] trs = parent.GetComponentsInChildren<Transform>(true);
            foreach (Transform t in trs)
            {
                if (t.name == name)
                {
                    return t.gameObject;
                }
            }
            return null;
        }

        /// <summary>
        /// Extension method. Sets a Transform's local position, rotation, and scale to 0.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Transform ResetLocalIdentity(this Transform t)
        {
            t.localPosition = Vector3.zero;
            t.localRotation = Quaternion.identity;
            return null;
        }

        /// <summary>
        /// Extension method to set Transform pos/rot to a Pose
        /// </summary>
        /// <param name="t"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static Transform SetPose(this Transform t, Pose p)
        {
            t.position = p.position;
            t.rotation = p.rotation;
            return null;
        }

        #region SetPos, SetRot

        /// <summary>
        /// Extension method. Quickly sets a transform's local x position value.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Transform SetXPos(this Transform t, float value, Space orient)
        {
            if (orient == Space.Self)
                t.localPosition = new Vector3(value, t.localPosition.y, t.localPosition.z);
            else
                t.position = new Vector3(value, t.position.y, t.position.z);

            return null;
        }

        /// <summary>
        /// Extension method. Quickly sets a transform's local y position value.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Transform SetYPos(this Transform t, float value, Space orient)
        {
            if (orient == Space.Self)
                t.localPosition = new Vector3(t.localPosition.x, value, t.localPosition.z);
            else
                t.position = new Vector3(t.position.x, value, t.position.z);
            return null;
        }

        /// <summary>
        /// Extension method. Quickly sets a transform's local z position value.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Transform SetZPos(this Transform t, float value, Space orient)
        {
            if (orient == Space.Self)
                t.localPosition = new Vector3(t.localPosition.x, t.localPosition.y, value);
            else
                t.position = new Vector3(t.position.x, t.position.y, value);
            return null;
        }

        /// <summary>
        /// Extenion method. Quickly sets a transfor's X rotation value.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="value"></param>
        /// <param name="orient"></param>
        /// <returns></returns>
        public static Transform SetXRot(this Transform t, float value, Space orient)
        {
            if (orient == Space.Self)
                t.localEulerAngles = new Vector3(value, t.localEulerAngles.y, t.localEulerAngles.z); 
            else
                t.eulerAngles = new Vector3(value, t.eulerAngles.y, t.eulerAngles.z);
            return null;
        }

        /// <summary>
        /// Extenion method. Quickly sets a transfor's Y rotation value.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="value"></param>
        /// <param name="orient"></param>
        /// <returns></returns>
        public static Transform SetYRot(this Transform t, float value, Space orient)
        {
            if (orient == Space.Self)
                t.localEulerAngles = new Vector3(t.localEulerAngles.x, value, t.localEulerAngles.z);
            else
                t.eulerAngles = new Vector3(t.eulerAngles.x, value, t.eulerAngles.z);
            return null;
        }

        /// <summary>
        /// Extenion method. Quickly sets a transfor's Z rotation value.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="value"></param>
        /// <param name="orient"></param>
        /// <returns></returns>
        public static Transform SetZRot(this Transform t, float value, Space orient)
        {
            if (orient == Space.Self)
                t.localEulerAngles = new Vector3(t.localEulerAngles.x, t.localEulerAngles.y, value);
            else
                t.eulerAngles = new Vector3(t.eulerAngles.x, t.eulerAngles.y, value);
            return null;
        }

        #endregion
    }
}
