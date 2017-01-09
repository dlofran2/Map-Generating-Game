using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

	public Wave[] waves;
	public Enemy enemy;
	public GameObject player;
	public Transform playerT;

	Wave currentWave;
	int currentWaveNumber;
	float timeBetweenSpawn = 3;
	float timeTilNextSpawn;

	int enemiesRemainingToSpawn;
	public static int enemiesRemaining;

	public string seed;
	Vector3 spawnLocation;
	int x, z;

	public bool newWave = true;
	public bool newWaveWait = true;

	public event System.Action<int> OnNewWave;

	void Start() {
		NextWave ();
	}

	void Update() {

		if (newWave) {
			newWave = false;
			SpawnPlayer ();
		}

		if (enemiesRemainingToSpawn > 0) {
			spawnLocation = FindSpawnLocation ();
			SpawnEnemy ();
		}

		if (currentWave.infinite && Time.time > timeTilNextSpawn) {
			timeTilNextSpawn = Time.time + timeBetweenSpawn;
			spawnLocation = FindSpawnLocation ();
			Enemy spawnedEnemy = Instantiate (enemy, spawnLocation, Quaternion.identity) as Enemy;
			spawnedEnemy.OnDeath += OnEnemyDeath;

			enemiesRemaining++;
		}
		
	}


	void SpawnPlayer() {
		spawnLocation = FindSpawnLocation ();

		spawnLocation = new Vector3 (x, 1, z);
		playerT.position = spawnLocation;
	}

	void SpawnEnemy() {
		Enemy spawnedEnemy = Instantiate (enemy, spawnLocation, Quaternion.identity) as Enemy;
		spawnedEnemy.OnDeath += OnEnemyDeath;
		spawnedEnemy.SetCharacteristics (currentWave.moveSpeed, currentWave.hitsToKillPlayer, currentWave.enemyHealth, currentWave.skinColor);
		enemiesRemainingToSpawn--;
	}

	Vector3 FindSpawnLocation () {
		do {
			x = Random.Range (0, 127);
			z = Random.Range (0, 71);
		} while (MapGenerator.map[x, z] == 1);

		x -= 64;
		z -= 36;

		return new Vector3 (x, 1, z);

	}
	void OnEnemyDeath () {
		GameUI.score++;
		enemiesRemaining--;

		if (enemiesRemaining == 0) {
			NextWave ();
		}
	}

	void NextWave() {
		currentWaveNumber++;

		if (currentWaveNumber - 1 < waves.Length) {
			currentWave = waves [currentWaveNumber - 1];

			enemiesRemainingToSpawn = currentWave.enemyCount;
			enemiesRemaining = enemiesRemainingToSpawn;

			if (OnNewWave != null) {
				OnNewWave (currentWaveNumber);
			}


			newWave = true;
		}
			
	}

	[System.Serializable]
	public class Wave {
		public bool infinite;
		public int enemyCount;

		public float moveSpeed;
		public int hitsToKillPlayer;
		public float enemyHealth;
		public Color skinColor;
	}
}

