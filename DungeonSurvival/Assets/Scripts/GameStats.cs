using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameStats : MonoBehaviour {

	public float health = 100f;
	public float shield = 0.0f;
	public Slider healthLevel;
	public Slider shieldLevel;
	public Text score;
	public Text highScore;
	public Text Health;
	public Text Shield;
	public GameObject controls;
	public GameObject gameOver;
	public bool dead = false;
	public bool activeShield = false;
	public bool toggle = true;
	//private GameObject[] gameObjects;

	public float points = 0;
	public float bestScore;

	// Use this for initialization
	void Start () {

		if (PlayerPrefs.HasKey ("HighScore")) {
			bestScore = PlayerPrefs.GetFloat ("HighScore");
		}

	}
	
	// Update is called once per frame
	void Update () {


		score.text = "Score: " + points;
		Health.text = "Health: " + health + "/100";
		highScore.text = "High Score: " + bestScore;

		shieldLevel.value = shield;
		Shield.text = "Shield: " + Mathf.RoundToInt(100 * (shield/100f))+"%";

		//PlayerPrefs.SetFloat ("HighScore", 0);
		if (points > bestScore) {
			bestScore = points;
			PlayerPrefs.SetFloat ("HighScore", bestScore);
		}


		if (dead && Input.GetKeyDown (KeyCode.Return)) {
			SceneManager.LoadScene ("LvL1");
		}
	
		if (Input.GetKeyUp (KeyCode.C)){
				toggle = !toggle;
				controls.SetActive (toggle);
				
		}
	}

	public void TakeDamage(float damage){

		if (shield > 0) {
			activeShield = true;
		} else {
			activeShield = false;
		}

		if (!dead && !activeShield) {
			if (health - damage <= 0) {
				health = 0;
			} else {
				health -= damage;
			}
			healthLevel.value = health;
		}

		if (activeShield) {
			if (shield - damage <= 0) {
				shield = 0;
			} else {
				shield -= damage;
			}
		}

		if (health <= 0) {
			dead = true;
			GetComponent<Controller> ().battleMusic.Stop ();
			gameOver.SetActive (true);
			DestroyAllObjects ();
		}
	}

	void DestroyAllObjects() {
		GameObject[] gameObjects = GameObject.FindGameObjectsWithTag ("Bomb");
		for(var i = 0 ; i < gameObjects.Length ; i ++)
		{
			Destroy(gameObjects[i]);
		}
	} 
}

