using UnityEngine;
using System.Collections;

public class PlayerLocator : MonoBehaviour {
	
	public FloorCollider tile;
	public RoomCollider room;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.L))
		{
			if(tile != null && room != null)
			{
				Debug.Log("Player is in room " + room.index + " (" + tile.x + "," + tile.y + ")");
			}
		}
	}
}
