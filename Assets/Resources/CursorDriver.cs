using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace WasaaMP {
	public class CursorDriver : MonoBehaviourPun {
		private bool active ;
		private CursorTool cursor ;
		private Camera theCamera ;
		public float scroll_sensitivity = 10f;

		void Start () {
			if (photonView.IsMine || ! PhotonNetwork.IsConnected) {
				// get the camera
				theCamera = (Camera)GameObject.FindObjectOfType (typeof(Camera)) ;
				active = false ;
				cursor = GetComponent<CursorTool> () ;
				Cursor.lockState = CursorLockMode.Locked;
			}
		}
		
		void Update () {
			if (photonView.IsMine  || ! PhotonNetwork.IsConnected) {
				if (Input.GetButtonDown ("Fire1"))  {
					cursor.Catch () ;
				}
				if (Input.GetButtonUp ("Fire1")) {
					cursor.Release () ;
				}
				if (Input.GetKeyDown (KeyCode.C)) {
					cursor.CreateInteractiveCube () ;
				}
				if (Input.mousePosition != null) {
					
					float deltaZ = Input.mouseScrollDelta.y / 10.0f ;
					transform.localRotation = Quaternion.Euler(0, transform.localRotation.y, transform.localRotation.z);
					transform.Translate( transform.InverseTransformDirection(theCamera.transform.forward)*deltaZ*scroll_sensitivity*Time.deltaTime);
	
				}
			}
		}

	}

}