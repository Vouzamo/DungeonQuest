using UnityEngine;
using System.Collections;

public class DoorCollider : MonoBehaviour {
	
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void OnTriggerEnter(Collider other) {
		if(other.gameObject.name == "Player") {
			transform.Find("steel_door").GetComponent("Animation").animation.Play("open-slowly", AnimationPlayMode.Queue);
		}
	}
}
