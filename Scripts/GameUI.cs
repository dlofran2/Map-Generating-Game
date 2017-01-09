using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameUI : MonoBehaviour {

	public RectTransform newWaveBanner;

	public Image fadePlane;
	public GameObject gameOverUI;
	public Text scoreUI;
	public Text newWaveTitle;
	public Text enemyCount;
	public static int score;

	public RectTransform healthBar;

	Spawner spawner;
	Player player;

	void Start () {
		gameOverUI.SetActive (false);
		player = FindObjectOfType<Player> ();
		player.OnDeath += OnGameOver;
	}

	void Awake() {
		spawner = FindObjectOfType<Spawner> ();
		spawner.OnNewWave += OnNewWave;
	}

	void Update() {
		scoreUI.text = "Kill Count: " + score;
		enemyCount.text = "Enemies remaining: " + Spawner.enemiesRemaining;
		float healthPercent = 0;
		if (player != null) {
			healthPercent = player.health / player.startingHealth;
		}
		healthBar.localScale = new Vector3 (healthPercent, 1, 1);

	}

	void OnNewWave(int waveNumber) {
		string[] numbers = { "One", "Two", "Three", "Four"};
		if (waveNumber - 1 == 4) {
			newWaveTitle.text = " ~ INFINITE WAVE ~ ";
		} else {
			newWaveTitle.text = "~ Wave " + numbers [waveNumber - 1] + " ~";
		}

		StartCoroutine (AnimateWaveBanner ());
	}

	IEnumerator AnimateWaveBanner () {
		float delayTime = 1.5f;
		float speed = 2.5f;
		float animatePercent = 0;
		int dir = 1;

		float endDelayTime = Time.time + 1 / speed + delayTime;

		while (animatePercent >= 0) {
			animatePercent += Time.deltaTime * speed * dir;

			if (animatePercent >= 1) {
				animatePercent = 1;
				if (Time.time > endDelayTime) {
					dir = -1;
				}
			}

			newWaveBanner.anchoredPosition = Vector2.up * Mathf.Lerp (-500, -170, animatePercent);
			yield return null;
		}
	}

	void OnGameOver() {
		StartCoroutine(Fade (Color.clear, new Color(0,0,0,.75f) ,1));
		gameOverUI.SetActive(true);
	}

	IEnumerator Fade(Color from, Color to, float time) {
		float speed = 1 / time;
		float percent = 0;

		while (percent < 1) {
			percent += Time.deltaTime * speed;
			fadePlane.color = Color.Lerp (from, to, percent);
			yield return null;
		}
	}

	// UI input
	public void StartNewGame() {
		SceneManager.LoadScene ("Main");
	}
}
