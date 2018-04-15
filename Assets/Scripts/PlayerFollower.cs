using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollower : MonoBehaviour {

	public GameObject player;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (player != null) {
			this.transform.forward = player.transform.forward;
			this.transform.position = player.transform.position + this.transform.forward *(-5);
			this.transform.position = new Vector3 (this.transform.transform.position.x,
				this.transform.position.y + 2, this.transform.position.z);
			this.transform.LookAt (player.transform.position);
			//this.transform.LookAt (player.transform.position);
		}
	}

	public void SetTarget(GameObject target) {
		player = target;
	}
}
