using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour {

	public GameObject player;

	void Update () {
		transform.LookAt(Camera.main.transform.position);
	}
}