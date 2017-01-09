using UnityEngine;

public interface IDamamagable {

	void TakeHit (float damage, RaycastHit hit);

	void TakeDamage (float damage);

}
