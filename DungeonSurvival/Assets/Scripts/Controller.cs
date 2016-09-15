using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {

	public float speed = 1.0f;
	public float damage = 1.0f;
	public float SlerpSpeed = 150.0f;
	public GameObject spawnpoint;
	public GameStats player;
	private float coinCount;
	private GameObject[] gameObjects;
	private bool canFireFront = true;
	private bool canFireBack = true;
	private bool canFireSpecial = true;
	private bool canFireTurret = true;

	public AudioSource[] sounds;
	public AudioSource idleMusic;
	public AudioSource battleMusic;

	public Transform firePoint;
	public Transform firePointBack;
	public Transform firePointBackLeft;
	public Transform firePointBackRight;
	public Transform Turret;
	public GameObject projectile;
	public GameObject rearWeapon;
	public GameObject specialWeapon;
	public GameObject turret;

	void Start () {

		transform.position = spawnpoint.transform.position;
		player = GetComponent<GameStats> ();
		sounds = GetComponents<AudioSource>();
		sounds[0] = idleMusic;
		sounds[1] = battleMusic;
		idleMusic.Play ();

	}
	
	void FixedUpdate () {
		float y = Input.GetAxis ("Vertical");
		float x = Input.GetAxis ("Horizontal");

		if (!player.dead){
			var move = new Vector3 (x, y, 0f);
			transform.position += move * speed * Time.deltaTime;


			if (Input.GetKey("up") || Input.GetKey("down") || Input.GetKey("right") || Input.GetKey("left")) {
				float angle = Mathf.Atan2 (y, x);
				transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.AngleAxis ((angle * Mathf.Rad2Deg) + 90.0f, Vector3.forward), SlerpSpeed);
			}

			if (Input.GetKeyUp (KeyCode.Space) && canFireFront) {
				canFireFront = false;
				Instantiate (projectile, firePoint.position, firePoint.rotation);
				StartCoroutine (ResetFire(true ,false, 0.1f));
			}
			if (Input.GetKeyUp (KeyCode.A) && canFireBack) {
				canFireBack = false;
				Instantiate (rearWeapon, firePointBack.position, firePointBack.rotation);
				StartCoroutine (ResetFire(true, true, 2.0f));
			}
			if (Input.GetKeyUp (KeyCode.W) && canFireSpecial) {
				canFireSpecial = false;
				Instantiate (specialWeapon, firePointBack.position, firePointBack.rotation);
				Instantiate (specialWeapon, firePointBackLeft.position, firePointBackLeft.rotation);
				Instantiate (specialWeapon, firePointBackRight.position,firePointBackRight.rotation);
				StartCoroutine (ResetFire(false, false,0.25f));
			}
			if (Input.GetKeyUp (KeyCode.D) && canFireTurret) {
				canFireTurret = false;
				Instantiate (turret, Turret.position, Quaternion.Euler(0,0,0));
				StartCoroutine (ResetFire(false, true, 30.0f));
			}
			if (Input.GetKeyUp (KeyCode.M)) {
				for (int i = 0; i < sounds.Length; i++) {
					//if (sounds [i].isPlaying) {
						sounds [i].mute = !sounds[i].mute;
					//}
				}
			}
		} 

		//if (coinCount == 10) {
		//	DestroyAllObjects ();
		//}
	}
		
	void OnCollisionEnter2D(Collision2D obj){

		/*if (obj.gameObject.tag == "Enemy") {
				player.TakeDamage (obj.gameObject.GetComponent<EnemyController> ().damage);
		}*/
	}

	void OnTriggerEnter2D(Collider2D obj){

		if (obj.gameObject.tag == "Coins") {
			player.points += 10;
			//coinCount++; 
			Destroy (obj.gameObject);
		}
		if (obj.gameObject.tag == "Health") {
			player.health += 10;
			Destroy (obj.gameObject);
		}
		if (obj.gameObject.tag == "Shield") {
			player.shield += 10;
			Destroy (obj.gameObject);
		}
	}

	/*void DestroyAllObjects() {
		gameObjects = GameObject.FindGameObjectsWithTag ("Object");

		for(var i = 0 ; i < gameObjects.Length ; i ++)
		{
			Destroy(gameObjects[i]);
		}
	} */

	IEnumerator ResetFire(bool x,bool y, float t){
		yield return new WaitForSeconds (t);
		if (x && y) {
			canFireBack = true;
		} else if (x && !y) {
			canFireFront = true;
		} else if (!x && !y) {
			canFireSpecial = true;
		} else {
			canFireTurret = true;
		}
	}
}
