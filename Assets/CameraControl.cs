using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 direction = Vector3.zero;
		if(Input.GetKeyDown(KeyCode.LeftArrow))
		{
			direction = new Vector3(-1,0,0);
		}
		if(Input.GetKeyDown(KeyCode.RightArrow))
		{
			direction = new Vector3(1,0,0);
		}
		if(Input.GetKeyDown(KeyCode.UpArrow))
		{
			direction = new Vector3(0,0,1);
		}
		if(Input.GetKeyDown(KeyCode.DownArrow))
		{
			direction = new Vector3(0,0,-1);
		}
		gameObject.transform.Translate(direction);
		//Camera.mainCamera.transform.position = gameObject.transform.position + new Vector3(0, 10, -5);
	}
}
