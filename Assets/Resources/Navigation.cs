using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


namespace WasaaMP {
    public class Navigation : MonoBehaviourPunCallbacks {

        #region Public Fields

        // to be able to manage the offset of the camera
        public float speed = 3f;
        public float sensitivity = 100f;
        public Vector3 cameraPositionOffset = new Vector3 (0, 0, 0) ;
        public Quaternion cameraOrientationOffset = new Quaternion () ;
 
        [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
        public static GameObject LocalPlayerInstance;

        #endregion
        Transform cameraTransform;
        float angle = 0f;
        Transform getChildByName(string name, Transform transform)
        {
            Transform[] allChildren = GetComponentsInChildren<Transform>(transform);
            foreach (Transform child in allChildren)
            {
                if (child.name == name)
                {
                    return child;
                }
            }
            return null;
        }


        void Awake () {
            // #Important
            // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
            if (photonView.IsMine) {
                LocalPlayerInstance = this.gameObject;
            }
            // #Critical
            // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
            //DontDestroyOnLoad (this.gameObject) ;
        }

        void Start () {
            if (photonView.IsMine) {
                // attach the camera to the navigation rig
                Transform head = getChildByName("Head", transform);
                Camera theCamera = (Camera)GameObject.FindObjectOfType (typeof(Camera)) ;
                cameraTransform = theCamera.transform ;
                cameraTransform.SetParent (head) ;
                cameraTransform.localPosition = cameraPositionOffset ;
                // cameraTransform.localRotation = cameraOrientationOffset ;
            }
        }

        void Update () {
            if (photonView.IsMine) {
                // Movement behaviour modifies to resemble classic fps controls
                // Head moves up and down when looking up and down but it is not synced using photon
                float mouse_x = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
                float mouse_y = -Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
                var x = Input.GetAxis ("Horizontal") * Time.deltaTime * speed ;
                var z = Input.GetAxis ("Vertical") * Time.deltaTime * speed ;
                angle -= mouse_y;
                angle = Mathf.Clamp(angle, -70f, 80f);
                transform.Rotate(Vector3.up * mouse_x);
                cameraTransform.parent.localRotation = Quaternion.Euler(0f, 0f, angle);
                transform.Translate (x, 0, z) ;    
                    
            }
        }


    }

}