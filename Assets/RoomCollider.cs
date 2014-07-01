using UnityEngine;
using System.Collections;

public class RoomCollider : MonoBehaviour {

	public int x;
	public int y;
	public int index;
	public bool discovered;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider other)
	{
		//Update other with the currentRoom
		if(other.name == "Player")
		{
			PlayerLocator locator = other.GetComponent<PlayerLocator>();
			if(locator != null)
			{
				locator.room = this;
			}
			//Debug.Log("Entered room " + this.index);
		}
	}
	
	void OnTriggerExit(Collider other)
	{
		//This is probably not required unless we have some garbage collection after exiting a room.
		if(other.name == "Player")
		{
			PlayerLocator locator = other.GetComponent<PlayerLocator>();
			if(locator != null)
			{
				if(locator.room == this)
				{
					locator.room = null;
				}
			}
			//Debug.Log("Exited room " + this.index);
		}
	}
}
