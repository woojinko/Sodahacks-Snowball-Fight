using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {
	public GameObject bulletPrefab;
	public Transform bulletSpawn;

	public GameObject blockPrefab;
	public Transform blockSpawn;


	public Vector3 jump;
	public float jumpForce = 2.0f;

	public bool isGrounded;
	Rigidbody rb;
	void Start(){
		rb = GetComponent<Rigidbody>();
		jump = new Vector3(0.0f, 2.0f, 0.0f);
	}

	void OnCollisionStay()
	{
		isGrounded = true;
	}
		

	public override void OnStartLocalPlayer() {
		var Material = GetComponent<MeshRenderer> ();
		Material.material.color = Color.blue;
		//Material.material.color = new Color(0,0,255,0.0f);

		Camera.main.GetComponent<PlayerFollower> ().SetTarget (gameObject);
	}

	void Update() {
		if (!isLocalPlayer) {
			return;
		}

		if (Input.GetKeyDown (KeyCode.M)) {
			CmdFire ();
		}

		if (Input.GetKeyDown (KeyCode.N)) {
			CmdLayBlock ();
		}
		/*
		if (Input.GetKeyDown (KeyCode.B)) {
			CmdLayManyBlocks ();
		}
		*/

		if(Input.GetKeyDown(KeyCode.Space) && isGrounded){
			rb.AddForce(jump * jumpForce, ForceMode.Impulse);
			isGrounded = false;
		}

		var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
		var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

		Quaternion rot = rb.rotation;
		rot [0] = 0;
		rot [2] = 0;
		rb.rotation = rot;

		transform.Rotate(0, x, 0);
		transform.Translate(0, 0, z);
	}

	// This [Command] code is called on the Client …
	// … but it is run on the Server!
	[Command]
	void CmdFire() {
		// Create the Bullet from the Bullet Prefab
		var bullet = (GameObject)Instantiate(
			bulletPrefab,
			bulletSpawn.position,
			bulletSpawn.rotation);

		// Add velocity to the bullet
		bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 8;

		// Spawn the bullet on the Clients
		NetworkServer.Spawn(bullet);

		// Destroy the bullet after 2 seconds
		Destroy(bullet, 5.0f);        
	}

	[Command]
	void CmdLayBlock()
	{
		//create trail block
		var block = (GameObject)Instantiate (
			            blockPrefab,
			            blockSpawn.position,
			            blockSpawn.rotation);
		block.GetComponent<Rigidbody> ().position = blockSpawn.position;

		// Spawn the block on the Clients
		NetworkServer.Spawn(block);
	}

	[Command]
	void CmdLayManyBlocks()
	{
		for (int i = 0; i < 100; i++) {
			//create trail block
			var block = (GameObject)Instantiate (
				           blockPrefab,
				           blockSpawn.position,
				           blockSpawn.rotation);
			block.GetComponent<Rigidbody> ().position = blockSpawn.position;
			// Spawn the block on the Clients
			NetworkServer.Spawn (block);
		}
	}
}