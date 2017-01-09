using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody))]
public class Enemy : LivingEntity {

	public GameObject playerG;
	Transform player;
	Material skinMaterial;
	Color originialColor;
	LivingEntity playerEntity;

	public float moveSpeed;
	float damage = 1;

	private float minDist = 2;
	float attackDistanceThreshold = 2;
	float timeBetweenAttacks = 1;
	float nextAttackTime;


	public static bool hasTarget;

	void Awake() {
		if (GameObject.FindGameObjectWithTag ("Player") != null) {
			hasTarget = true;

			player = GameObject.FindGameObjectWithTag ("Player").transform;
			playerEntity = player.GetComponent<LivingEntity> ();

		}
	}

	public override void Start () {
		base.Start ();
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		skinMaterial = GetComponent<Renderer> ().material;
		originialColor = skinMaterial.color;
		playerEntity = player.GetComponent<LivingEntity> ();
		playerEntity.OnDeath += OnTargetDeath;

	}

	public void SetCharacteristics(float _moveSpeed, int hitsToKillPlayer, float enemyHealth, Color skinColor) {
		moveSpeed = _moveSpeed;

		damage = hitsToKillPlayer;
		startingHealth = enemyHealth;

		skinMaterial = GetComponent<Renderer> ().material;
		skinMaterial.color = skinColor;
		originialColor = skinColor;
	}
		
	//public override void TakeDamage(float damage) {
		/*if (damage >= health) {
			if (OnDeathStatic != null) {
				OnDeathStatic ();
			}
		} */
	//}

	void OnTargetDeath() {
		hasTarget = false;
	}

	void Update() {
		if (hasTarget) {
			if (Time.time > nextAttackTime) {
				float sqrDistToTarget = (player.position - transform.position).sqrMagnitude;
				if (sqrDistToTarget < Mathf.Pow (attackDistanceThreshold, 2)) {
					nextAttackTime = Time.time + timeBetweenAttacks;
					StartCoroutine (Attack ());
				}
			}
		}
	}

	void FixedUpdate() {
		if (hasTarget) {
			transform.LookAt (player);
			float distance = Vector3.Distance (transform.position, player.position);

			if (distance > minDist) {
				transform.position += transform.forward * moveSpeed * Time.deltaTime;
			}
		}
	}

	IEnumerator Attack() {
		Vector3 originalPosition = transform.position;
		Vector3 attackPosition = player.position;

		float attackSpeed = 3;
		float percent = 0;

		skinMaterial.color = Color.red;
		bool hasAppliedDamage = false;

		while (percent <= 1) {

			if (percent >= .5f && !hasAppliedDamage) {
				hasAppliedDamage = true;
				playerEntity.TakeDamage (damage);
			}

			percent += Time.deltaTime * attackSpeed;
			float interpolation = (-percent * percent + percent) * 4;
			transform.position = Vector3.Lerp (originalPosition, attackPosition, interpolation);

			yield return null;
		}
		skinMaterial.color = originialColor;
	}
		
}
