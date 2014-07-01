using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	
	public GameObject target;
	public GameObject dungeon;
	private Vector3 targetOffset;
	private float damping;
	
	// Use this for initialization
	void Start () {
		targetOffset = new Vector3(-0.5f, 10, -1.5f);
		damping = 1.5f;
	}
	
	// Update is called once per frame
	void Update () {
		if(target != null)
		{
			Grid grid = dungeon.GetComponent<Grid>();
			PlayerLocator locator = target.GetComponent<PlayerLocator>();
			if(locator != null)
			{
				if(locator.room != null)
				{
					Vector3 position = new Vector3((locator.room.x * (grid.roomWidth + 1)) + (1 + (grid.roomWidth / 2)), 0, (locator.room.y * (grid.roomHeight + 1)) + 1);
					position = position + targetOffset;
					transform.position = Vector3.Lerp (transform.position, position, (float)(Time.deltaTime * damping));
				}
				Quaternion rotation = Quaternion.LookRotation(target.transform.position - transform.position, Vector3.up);
				transform.rotation = Quaternion.Slerp(transform.rotation, rotation, (float)(Time.deltaTime * damping));
			}
		}
	}
}
