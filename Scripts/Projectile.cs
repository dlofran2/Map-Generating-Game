using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public LayerMask enemyMask;
	public LayerMask wallMask;

	float speed = 10;
	float distance;
	float maxDistance = 30;
	float damage = 1;
	float skinWidth = .1f;

	public void SetSpeed(float newSpeed) {
		speed = newSpeed;
	}
		
	void Update () {
		float moveDistance = speed * Time.deltaTime;
		CheckCollisions (moveDistance);
		transform.Translate (Vector3.forward * moveDistance);
		distance += Time.deltaTime * speed;

		if (distance >= maxDistance) {
			Destroy (gameObject);
		}
	}

	void CheckCollisions(float moveDistance) {
		Ray ray = new Ray (transform.position, transform.forward);
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit, moveDistance + skinWidth, enemyMask, QueryTriggerInteraction.Collide)) {
			OnHitObject (hit);
		}

		if (Physics.Raycast (ray, out hit, moveDistance, wallMask, QueryTriggerInteraction.Collide)) {
			GameObject.Destroy (gameObject);
		}
	}

	void OnHitObject(RaycastHit hit) {
		IDamamagable damagableObject = hit.collider.GetComponent<IDamamagable> ();
		if (damagableObject != null) {
			damagableObject.TakeHit (damage, hit);
		}
		GameObject.Destroy (gameObject);
	}
}
