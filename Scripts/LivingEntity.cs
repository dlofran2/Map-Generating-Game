using UnityEngine;
using System.Collections;

public class LivingEntity : MonoBehaviour, IDamamagable {

	public float startingHealth;
	public float health { get; protected set; }
	protected bool dead;

	public event System.Action OnDeath;

	public virtual void Start() {
		health = startingHealth;
	}

	public virtual void TakeHit(float damage, RaycastHit hit) {
		TakeDamage (damage);
	}

	public void TakeDamage(float damage) {
		health -= damage;

		if (health <= 0) {
			Die ();
		}
	}

	[ContextMenu("Self-Distruct")]
	protected void Die() {
		dead = true;
		if (OnDeath != null) {
			OnDeath ();
		}
		GameObject.Destroy (gameObject);
	}
}



