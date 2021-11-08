using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

namespace WasaaMP {
    public class InteractiveObject : MonoBehaviourPun {
        public int required_players = 2;
        private List<Tuple<Transform,Transform>> handles;

        void Start() {
            // Defining the object's handles
            var children = transform.GetComponentInChildren<Transform>();
            handles = new List<Tuple<Transform,Transform>>();
            foreach (Transform child in children)
            {
                if (child.GetChild(0).tag == "Handle")
                    handles.Add(new Tuple<Transform,Transform>(child,child.GetChild(0)));
            }
            print("handle1 is "+handles[0].Item1+" and " + handles[0].Item2);
        }

        void Update() {
            // Updating the cubes positon and rotation according to all the active handles
            float count = 0;
            Vector3 move = new Vector3(0,0,0);
            Quaternion rotate = Quaternion.identity;
            foreach(Tuple<Transform,Transform> handle in handles)
            {
                if (handle.Item2.GetComponent<InteractiveHandle>().caught)
                {
                    count++;
                    move += handle.Item2.position - handle.Item1.position;
                    rotate *= Quaternion.Inverse(handle.Item1.rotation) * handle.Item2.rotation;
                }
            }
            move /= count;
            
            if (count >= required_players)
            {

                transform.GetComponent<Rigidbody>().isKinematic = true;
                transform.position += move;
                transform.rotation *= rotate;
            }
            else
            {
                transform.GetComponent<Rigidbody>().isKinematic = false;

            }

        }
    }
}
