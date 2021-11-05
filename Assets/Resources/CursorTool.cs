using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace WasaaMP {
	public class CursorTool : MonoBehaviourPun {
		private bool caught ;
		public InteractiveObject interactiveObjectToInstanciate ;
		private InteractiveHandle target ;
		private MonoBehaviourPun targetParent ;
		private Transform oldParent = null ;
		private Vector3 oldPosition;
		private Quaternion oldRotation;
		private LayerMask mask;
		void Start () {
			caught = false ;
			mask = LayerMask.NameToLayer("Handle");
		}
        private void Update()
        {
			if (!caught && target!=null && !target.caught)
			{
				bool inside = false;
				Collider[] colliders = Physics.OverlapSphere(transform.position, 0.1f, mask);
				foreach (Collider collider in colliders)
				{
					if (target == collider.GetComponent<InteractiveHandle>())
					{
						inside = true;
					}
				}
				if (!inside && target.catchable)
				{
					target.photonView.RPC("HideCatchable", RpcTarget.All);
					target = null;
					PhotonNetwork.SendAllOutgoingCommands();
				}
			}
        }
        public void Catch () {
			print ("Catch ?") ;
			if (target != null && !target.caught) {
				print ("Catch :") ;
				if ((! caught) && (transform != target.transform)) { // pour ne pas prendre 2 fois l'objet et lui faire perdre son parent
					oldParent = target.transform.parent ;
					oldPosition = target.transform.localPosition;
					oldRotation = target.transform.localRotation;
					target.transform.SetParent (transform) ;
					target.photonView.TransferOwnership (PhotonNetwork.LocalPlayer) ;
					target.photonView.RPC ("ShowCaught", RpcTarget.All) ;
					PhotonNetwork.SendAllOutgoingCommands () ;
					caught = true ;
				}
				print ("Catch !") ;
			} else {
				print ("Catch failed") ;
			}
		}
			public void Release () {

			if (caught && target.caught) {
				print ("Release :") ;
				target.photonView.RPC("ShowReleased", RpcTarget.All);
				target.transform.SetParent (oldParent) ;
				target.transform.localPosition = oldPosition;
				target.transform.localRotation = oldRotation;
				
							
				PhotonNetwork.SendAllOutgoingCommands () ;
				print ("Release !") ;
				caught = false ;			}
		}

		public void CreateInteractiveCube () {
			var objectToInstanciate = PhotonNetwork.Instantiate (interactiveObjectToInstanciate.name, transform.position, transform.rotation, 0) ;
		}

		void OnTriggerEnter (Collider other) {
			if (other.tag == "Handle")
			{
				if (!caught)
				{
					print(name + " : CursorTool OnTriggerEnter");
                    if (target != null && target.catchable)
                    {
						target.photonView.RPC("HideCatchable", RpcTarget.All);
					}
					target = other.gameObject.GetComponent<InteractiveHandle>();

					if (target != null)
					{
                        target.photonView.RPC("ShowCatchable", RpcTarget.All);
                        PhotonNetwork.SendAllOutgoingCommands();
					}
				}
			}
		}

		void OnTriggerExit (Collider other) {
			if (! caught) {
				print (name + " : CursorTool OnTriggerExit") ;
				if (target != null) {
					target.photonView.RPC ("HideCatchable", RpcTarget.All) ;
                    PhotonNetwork.SendAllOutgoingCommands();
                    target = null;
				}
			}
		}

	}

}