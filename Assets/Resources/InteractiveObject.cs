using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace WasaaMP {
    public class InteractiveObject : MonoBehaviourPun {

        private Color colorBeforeHighlight;
        private Color oldColor;
        private float oldAlpha;
        public int required_players = 2;
        private bool catchable = false;
        private bool caught = false;
        private List<Transform> handles;
        private float rubberbanding = 0.45f;

        void Start() {
            var children = transform.GetComponentInChildren<Transform>();
            handles = new List<Transform>();
            foreach (Transform child in children)
            {
                if (child.parent == transform)
                    handles.Add(child);
            }
        }

        void Update() {
            Vector3 push =new Vector3(0,0,0);
            Vector3 pos = new Vector3(0, 0, 0);
            int active_handles = 0;
            foreach (Transform handle in handles)
            {
                if (handle.GetComponent<InteractiveHandle>().caught)
                {
                    push -= handle.forward;
                    active_handles++;
                    pos += handle.position;
                }
            }
            if (active_handles != 0)
            {
                transform.GetComponent<Rigidbody>().isKinematic = true;
                push.Normalize();
                push *= rubberbanding*active_handles;
                pos /= active_handles;
                transform.position = pos + push;
            }
            else
            {
                transform.GetComponent<Rigidbody>().isKinematic = false;

            }


        }
    }
}
