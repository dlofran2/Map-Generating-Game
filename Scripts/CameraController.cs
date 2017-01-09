using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	//public Transform rotatePlayer;
	public GameObject player;
	private Vector3 offset;

	public float turnSpeed = 15f;

	void Awake () {
		offset = transform.position;
	}
	
	void LateUpdate () {
		if (Enemy.hasTarget) {
			transform.position = (player.transform.position + offset);
		}
	}
}
