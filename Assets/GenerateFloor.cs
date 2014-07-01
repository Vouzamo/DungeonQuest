using UnityEngine;
using System;
using System.Collections;

public class GenerateFloor : MonoBehaviour {
	
	public GameObject floor;
	
	// Use this for initialization
	void Start () {
		GenerateRooms room = (GenerateRooms)transform.parent.GetComponent("GenerateRooms");
		
		for(Int32 x = 0; x < room.roomWidth; x++)
		{
			for(Int32 y = 0; y < room.roomHeight; y++)
			{
				Vector3 position = transform.position;
				position.x += x;
				position.z += y;
				Quaternion rotation = transform.rotation;
				
				GameObject thisFloor = (GameObject)GameObject.Instantiate(floor, position, rotation);
				thisFloor.name = "Floor";
				thisFloor.transform.parent = transform;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
