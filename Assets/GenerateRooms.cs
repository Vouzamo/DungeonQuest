using UnityEngine;
using System;
using System.Collections;

public class GenerateRooms : MonoBehaviour {
	
	public Int32 numberOfRooms;
	public Int32 roomWidth;
	public Int32 roomHeight;
	public GameObject room;
	public GameObject wall;
	public GameObject corner;
	public GameObject cornerT;
	
	// Use this for initialization
	void Start () {
		Debug.Log ("Hello World!");
		//generate rooms
		for(Int32 i = 0; i < numberOfRooms; i++)
		{
			Vector3 position = transform.position;
			//replace the following with room positioning algorythm
			position.x += i * (roomWidth + 1);
			Quaternion rotation = transform.rotation;
			
			GameObject thisRoom = (GameObject)GameObject.Instantiate(room, position, rotation);
			thisRoom.name = "Room";
			thisRoom.transform.parent = transform;
			
			Debug.Log ("Room " + i + " created...");
			
			for(Int32 x = -1; x <= roomWidth; x++)
			{
				for(Int32 y = -1; y <= roomHeight; y++)
				{
					Debug.Log("Place wall @ (" + x + "," + y +")");
					if((x == -1 || x == roomWidth) || (y == -1 || y == roomHeight))
					{
						if((x == -1 || x == roomWidth) && (y == -1 || y == roomHeight))
						{
							if(i == 0 || i == numberOfRooms - 1)
							{
								if((x == -1 && i == 0) || (x == roomWidth && i == numberOfRooms - 1))
								{
									Vector3 position2 = thisRoom.transform.position;
									position2.x += x;
									position2.z += y;
									Quaternion rotation2 = thisRoom.transform.rotation;
									
									GameObject thisCorner = (GameObject)GameObject.Instantiate(corner, position2, rotation2);
									thisCorner.name = "Corner";
									thisCorner.transform.parent = transform;
									if((x == roomWidth || y == roomHeight) && i == 0)
									{
										thisCorner.transform.Rotate(new Vector3(0,90,0));
									}
									if(i == numberOfRooms - 1)
									{
										thisCorner.transform.Rotate(new Vector3(0,180,0));
									}
									if(i == numberOfRooms - 1 && y == -1)
									{
										thisCorner.transform.Rotate(new Vector3(0,90,0));	
									}
								}
							}
							else
							{
								Vector3 position2 = thisRoom.transform.position;
								position2.x += x;
								position2.z += y;
								Quaternion rotation2 = thisRoom.transform.rotation;
								
								GameObject thisCornerT = (GameObject)GameObject.Instantiate(cornerT, position2, rotation2);
								thisCornerT.name = "CornerT";
								thisCornerT.transform.parent = transform;
								if(y == -1)
								{
									thisCornerT.transform.Rotate(new Vector3(0,270,0));
								}
								else
								{
									thisCornerT.transform.Rotate(new Vector3(0,90,0));
								}
							}
						}
						else
						{
							Vector3 position2 = thisRoom.transform.position;
							position2.x += x;
							position2.z += y;
							Quaternion rotation2 = thisRoom.transform.rotation;
							
							GameObject thisWall = (GameObject)GameObject.Instantiate(wall, position2, rotation2);
							thisWall.name = "Wall";
							thisWall.transform.parent = transform;
							if(x == -1 || x == roomWidth)
							{
								thisWall.transform.Rotate(new Vector3(0,90,0));
							}
						}
					}
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
