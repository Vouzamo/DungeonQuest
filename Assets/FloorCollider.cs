using UnityEngine;
using System.Collections;

public class FloorCollider : MonoBehaviour {
	
	public int x;
	public int y;
	public bool discovered;
	public GameObject occupied;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other)
	{
		this.occupied = other.gameObject;
		//Update other with the currentTile
		if(other.name == "Player")
		{
			PlayerLocator locator = other.GetComponent<PlayerLocator>();
			if(locator != null)
			{
				locator.tile = this;
			}
			//Debug.Log("Entered tile (" + this.x + "," + this.y + ")");
		}
	}
	
	void OnTriggerExit(Collider other)
	{
		if(this.occupied.name == other.name)
		{
			this.occupied = null;
		}
		//This is probably not required unless we have some garbage collection after exiting a tile.
		if(other.name == "Player")
		{
			PlayerLocator locator = other.GetComponent<PlayerLocator>();
			if(locator != null)
			{
				if(locator.tile == this)
				{
					locator.tile = null;
				}
			}
			//Debug.Log("Exited tile (" + this.x + "," + this.y + ")");
		}
	}
}
