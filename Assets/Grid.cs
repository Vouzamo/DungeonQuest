using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour {
	
	public Int32 numberOfRooms;
	public Int32 roomWidth;
	public Int32 roomHeight;
	public RoomDescriptor[,] grid;
	public TileDescriptor[,] map;
	
	public GameObject room;
	
	public GameObject floor;
	public GameObject floor1;
	public GameObject floor2;
	public GameObject floor3;
	public GameObject floor4;
	public GameObject wall;
	public GameObject wall1;
	public GameObject wall2;
	public GameObject wall3;
	public GameObject wall4;
	public GameObject wall5;
	public GameObject divider;
	public GameObject junction;
	public GameObject corner;
	public GameObject door;
	
	private static System.Random rand = new System.Random();
	
	// Use this for initialization
	void Start () {
		//check roomWidth and roomHeight are multiples of 2
		if(roomWidth % 2 != 0)
		{
			roomWidth++;	
		}
		if(roomHeight % 2 != 0)
		{
			roomHeight++;
		}
		
		Int32 gridSize = (2 * numberOfRooms) + 1;
		grid = new RoomDescriptor[gridSize, gridSize];
		
		//iterate through the grid and instantiate RoomDescriptor objects
		Int32 roomCounter = 0;
		Vector2 pointer = new Vector2(numberOfRooms, numberOfRooms);
		
		//create first room
		grid[(Int32)pointer.x, (Int32)pointer.y] = new RoomDescriptor(this, (Int32)pointer.x, (Int32)pointer.y, roomCounter++);
		
		//initialise the current room
		RoomDescriptor currentRoom = grid[(Int32)pointer.x, (Int32)pointer.y];
		
		while(roomCounter < numberOfRooms)
		{
			//get the current room based on the highest index
			foreach(RoomDescriptor room in grid)
			{
				if(room != null)
				{
					if(room.roomIndex > currentRoom.roomIndex)	
					{
						currentRoom = room;
					}
				}
			}
			
			//update the pointer for the currentRoom
			pointer.x = currentRoom.x;
			pointer.y = currentRoom.y;
			
			//determine how many rooms to generate
			Int32 maxRoomsAlpha = numberOfRooms - roomCounter;
			Int32 maxRoomsBeta = 4 - grid[(Int32)pointer.x, (Int32)pointer.y].CountNeighbours(false);
			Int32 maxRooms = Math.Min(maxRoomsAlpha, maxRoomsBeta);
			
			Int32 roomsToGenerate = 0;
			if(maxRooms > 0)
			{
				roomsToGenerate = rand.Next(1, maxRooms);
			}
			
			if(roomsToGenerate > 0)
			{
				//initialise a direction queue
				Queue<CompassDirections> directionQueue = new Queue<CompassDirections>();
				for(Int32 i = 0; i < 4; i++)
				{
					directionQueue.Enqueue((CompassDirections)(i * 2));	
				}
				
				//determine a random starting direction (NESW)
				Int32 startingDirection = rand.Next(1,4);
				for(Int32 i = 0; i < startingDirection; i++)
				{
					directionQueue.Enqueue(directionQueue.Dequeue());
				}
				
				//create the rooms
				for(Int32 i = 0; i < roomsToGenerate; i++)
				{
					//retrieve the neighbours
					Dictionary<CompassDirections, RoomDescriptor> neighbours = currentRoom.GetNeighbours(false);
					
					//test for space to create room in a direction
					while(neighbours.ContainsKey(directionQueue.Peek()))
					{
						directionQueue.Enqueue(directionQueue.Dequeue());
					}
					
					//create a pointer for the room destination
					Vector2 pointerTemp = pointer + RoomDescriptor.CompassDirectionToVector(directionQueue.Peek());
					
					if(grid[(Int32)pointerTemp.x, (Int32)pointerTemp.y] == null)
					{
						grid[(Int32)pointerTemp.x, (Int32)pointerTemp.y] = new RoomDescriptor(this, (Int32)pointerTemp.x, (Int32)pointerTemp.y, roomCounter++);
					}
				}
			}
		}
		
		//shrink the grid
		Int32 x1 = grid.GetUpperBound(0);
		Int32 x2 = 0;
		Int32 y1 = grid.GetUpperBound(1);
		Int32 y2 = 0;
		for(Int32 x = 0; x <= grid.GetUpperBound(0); x++)
		{
			for(Int32 y = 0; y <= grid.GetUpperBound(1); y++)
			{
				if(grid[x,y] != null)
				{
					RoomDescriptor room = grid[x,y];
					if(room.x < x1)
					{
						x1 = room.x;	
					}
					if(room.x > x2)
					{
						x2 = room.x;
					}
					if(room.y < y1)
					{
						y1 = room.y;
					}
					if(room.y > y2)
					{
						y2 = room.y;
					}
				}
			}
		}
		
		//calculate the grid dimensions
		Int32 xDelta = (x2 - x1) + 1;
		Int32 yDelta = (y2 - y1) + 1;
		
		RoomDescriptor[,] newGrid = new RoomDescriptor[xDelta, yDelta];
		
		for(Int32 x = 0; x <= newGrid.GetUpperBound(0); x++)
		{
			for(Int32 y = 0; y <= newGrid.GetUpperBound(1); y++)
			{
				//calculate translation
				Int32 xT = x + x1;
				Int32 yT = y + y1;
				
				if(grid[xT,yT] != null)
				{
					newGrid[x,y] = grid[xT,yT];
					newGrid[x,y].x = x;
					newGrid[x,y].y = y;
				}
			}
		}
		grid = newGrid;
		
		//instantiate room objects
		for(Int32 x = 0; x <= grid.GetUpperBound(0); x++)
		{
			for(Int32 y = 0; y <= grid.GetUpperBound(1); y++)
			{
				if(grid[x,y] != null)
				{
					//calculate translation
					Int32 xMap = (x * (roomWidth + 1)) + 1;
					Int32 yMap = (y * (roomHeight + 1)) + 1;
					
					RoomDescriptor roomDescriptor = grid[x,y];
					GameObject thisRoom = (GameObject)GameObject.Instantiate(room, new Vector3(xMap, 0, yMap), Quaternion.identity);
					thisRoom.name = "Room " + roomDescriptor.roomIndex;
					thisRoom.transform.parent = transform;
					
					BoxCollider boxCollider = thisRoom.GetComponent<BoxCollider>();
					boxCollider.isTrigger = true;
					boxCollider.size = new Vector3(roomWidth, 1, roomHeight);
					boxCollider.center = new Vector3((roomWidth / 2) - 0.5f, 0, (roomHeight / 2) - 0.5f);
					
					RoomCollider roomObject = thisRoom.GetComponent<RoomCollider>();
					roomObject.index = roomDescriptor.roomIndex;
					roomObject.x = x;
					roomObject.y = y;
					roomObject.discovered = true;
				}
			}
		}
		
		//create map
		map = new TileDescriptor[(xDelta * (roomWidth + 1) + 1), (yDelta * (roomHeight + 1) + 1)];
		
		//populate with empty tiles
		for(Int32 x = 0; x <= map.GetUpperBound(0); x++)
		{
			for(Int32 y = 0; y <= map.GetUpperBound(1); y++)
			{
				map[x,y] = new TileDescriptor(this, x, y, false, false, TileType.None);
			}
		}
		
		//create floors
		for(Int32 x = 0; x <= grid.GetUpperBound(0); x++)
		{
			for(Int32 y = 0; y <= grid.GetUpperBound(1); y++)
			{
				if(grid[x,y] != null)
				{
					//calculate translation
					Int32 xMap = (x * (roomWidth + 1)) + 1;
					Int32 yMap = (y * (roomHeight + 1)) + 1;
					
					//loop through creating the floor tiles
					for(Int32 xMap2 = xMap; xMap2 < (xMap + roomWidth); xMap2++)
					{
						for(Int32 yMap2 = yMap; yMap2 < (yMap + roomHeight); yMap2++)
						{
							//Debug.Log("CREATE(FLOOR) - (" + x + "," + y + ") " + xDelta + "[" + map.GetUpperBound(0) + "," + map.GetUpperBound(1) + "] (" + xMap + "," + yMap + ") (" + xMap2 + "," + yMap2 + ")");
							map[xMap2, yMap2] = new TileDescriptor(this, xMap2, yMap2, true, false, TileType.Floor);
						}
					}
				}
			}
		}
		
		//create doors
		for(Int32 x = 0; x <= grid.GetUpperBound(0); x++)
		{
			for(Int32 y = 0; y <= grid.GetUpperBound(1); y++)
			{
				if(grid[x,y] != null)
				{
					RoomDescriptor room = grid[x,y];
					Dictionary<CompassDirections, RoomDescriptor> neighbours = room.GetNeighbours(false);
					
					KeyValuePair<CompassDirections, RoomDescriptor> lowestNeighbour = new KeyValuePair<CompassDirections, RoomDescriptor>(CompassDirections.None, null);
					//loop through and determine the lowest index room
					foreach(KeyValuePair<CompassDirections, RoomDescriptor> neighbour in neighbours)
					{
						if(lowestNeighbour.Key == CompassDirections.None)
						{
							lowestNeighbour = neighbour;
						}
						else
						{
							if(lowestNeighbour.Value.roomIndex > neighbour.Value.roomIndex)
							{
								lowestNeighbour = neighbour;
							}
						}
					}
					
					if(lowestNeighbour.Key != CompassDirections.None)
					{
						if(lowestNeighbour.Value.roomIndex < room.roomIndex)
						{
							//calculate translation
							Int32 xRoom = (x * (roomWidth + 1)) + 1;
							Int32 yRoom = (y * (roomHeight + 1)) + 1;
							
							switch(lowestNeighbour.Key)
							{
								case CompassDirections.North:
									xRoom += ((roomWidth / 2) - 1);
									yRoom--;
									break;
								case CompassDirections.East:
									xRoom += roomWidth;
									yRoom += ((roomHeight / 2) - 1);
									break;
								case CompassDirections.South:
									xRoom += ((roomWidth / 2) - 1);
									yRoom += roomHeight;
									break;
								case CompassDirections.West:
									xRoom--;
									yRoom += ((roomHeight / 2) - 1);
									break;
							}
							
							map[xRoom,yRoom] = new TileDescriptor(this, xRoom, yRoom, true, true, TileType.Door);
							
							switch(lowestNeighbour.Key)
							{
								case CompassDirections.North:
									xRoom++;
									break;
								case CompassDirections.East:
									yRoom++;
									break;
								case CompassDirections.South:
									xRoom++;
									break;
								case CompassDirections.West:
									yRoom++;
									break;
							}
							
							map[xRoom,yRoom] = new TileDescriptor(this, xRoom, yRoom, true, true, TileType.Door);
						}
					}
				}
			}
		}
		
		//create walls
		for(Int32 x = 0; x <= map.GetUpperBound(0); x++)
		{
			for(Int32 y = 0; y <= map.GetUpperBound(1); y++)
			{
				if(map[x,y] != null)
				{
					TileDescriptor tile = map[x,y];
					if(tile.type == TileType.None)
					{
						Int32 neighbourCountOrthagonal = tile.CountNeighbours(false, TileType.Floor);
						if(neighbourCountOrthagonal > 0)
						{
							map[x,y] = new TileDescriptor(this, x, y, false, true, TileType.Wall);
						}
					}
				}
			}
		}
		
		//create corners
		for(Int32 x = 0; x <= map.GetUpperBound(0); x++)
		{
			for(Int32 y = 0; y <= map.GetUpperBound(1); y++)
			{
				if(map[x,y] != null)
				{
					TileDescriptor tile = map[x,y];
					if(tile.type == TileType.None)
					{
						Int32 neighbourCountOrthagonal = tile.CountNeighbours(false, TileType.Wall);
						if(neighbourCountOrthagonal > 3)
						{
							map[x,y] = new TileDescriptor(this, x, y, false, true, TileType.Divider);
						}
						else if(neighbourCountOrthagonal > 2)
						{
							map[x,y] = new TileDescriptor(this, x, y, false, true, TileType.Junction);
						}
						else if(neighbourCountOrthagonal > 1)
						{
							Dictionary<CompassDirections, TileDescriptor> neighbours = tile.GetNeighbours(false, TileType.Wall);
							if((neighbours.ContainsKey(CompassDirections.North) && neighbours.ContainsKey(CompassDirections.South)) || (neighbours.ContainsKey(CompassDirections.East) && neighbours.ContainsKey(CompassDirections.West)))
							{
								map[x,y] = new TileDescriptor(this, x, y, false, true, TileType.Divider);
							}
							else
							{
								map[x,y] = new TileDescriptor(this, x, y, false, true, TileType.Corner);
							}
						}
					}
				}
			}
		}
		
		//reset TileType = None for empty rooms
		for(Int32 x = 0; x <= grid.GetUpperBound(0); x++)
		{
			for(Int32 y = 0; y <= grid.GetUpperBound(1); y++)
			{
				if(grid[x,y] == null)
				{
					//calculate translation
					Int32 xMap = (x * (roomWidth + 1)) + 1;
					Int32 yMap = (y * (roomHeight + 1)) + 1;
					
					for(Int32 xMap2 = xMap; xMap2 < (xMap + roomWidth); xMap2++)
					{
						for(Int32 yMap2 = yMap; yMap2 < (yMap + roomHeight); yMap2++)
						{
							map[xMap2, yMap2] = new TileDescriptor(this, xMap2, yMap2, false, false, TileType.None);
						}
					}
				}
			}
		}
		
		//instantiate the map
		foreach(TileDescriptor tile in map)
		{
			if(tile != null)
			{
				switch(tile.type)
				{
					case TileType.Floor:
						Int32 randomWeight2 = rand.Next(1, 100);
						Int32 floorIndex = 1;
						if(randomWeight2 > 98)
						{
							floorIndex = 4;
						}
						else
						{
							floorIndex = rand.Next(1, 4);
						}
						GameObject thisFloor;
						switch(floorIndex)
						{
							case 1:
								thisFloor = (GameObject)GameObject.Instantiate(floor1, new Vector3(tile.x, 0, tile.y), Quaternion.identity);
								break;
							case 2:
								thisFloor = (GameObject)GameObject.Instantiate(floor2, new Vector3(tile.x, 0, tile.y), Quaternion.identity);
								break;
							case 3:
								thisFloor = (GameObject)GameObject.Instantiate(floor3, new Vector3(tile.x, 0, tile.y), Quaternion.identity);
								break;
							case 4:
								thisFloor = (GameObject)GameObject.Instantiate(floor4, new Vector3(tile.x, 0, tile.y), Quaternion.identity);
								break;
							default:
								thisFloor = (GameObject)GameObject.Instantiate(floor, new Vector3(tile.x, 0, tile.y), Quaternion.identity);
								break;
						}
						thisFloor.name = "Floor";
						thisFloor.transform.parent = transform;
						
//						//calculate the room based on the (x,y) position
//						int roomX = (tile.x - 1) / (roomWidth + 1);
//						int roomY = (tile.y - 1) / (roomHeight + 1);
//					
//						Room room = thisFloor.AddComponent<Room>();
//						room.x = roomX;
//						room.y = roomY;
//						room.index = grid[roomX, roomY].roomIndex;
//						room.discovered = true;
						
						FloorCollider tileObject = thisFloor.GetComponent<FloorCollider>();
//						tileObject.room = room;
						tileObject.x = tile.x;
						tileObject.y = tile.y;
						tileObject.discovered = true;
						tileObject.occupied = null;
					
						break;
					case TileType.Wall:
						Int32 randomWeight = rand.Next(1, 100);
						Int32 wallIndex = 1;
						if(randomWeight > 80)
						{
							wallIndex = rand.Next(2, 6);
						}
						GameObject thisWall;
						switch(wallIndex)
						{
							case 1:
								thisWall = (GameObject)GameObject.Instantiate(wall1, new Vector3(tile.x, 0, tile.y), Quaternion.identity);
								break;
							case 2:
								thisWall = (GameObject)GameObject.Instantiate(wall2, new Vector3(tile.x, 0, tile.y), Quaternion.identity);
								break;
							case 3:
								thisWall = (GameObject)GameObject.Instantiate(wall3, new Vector3(tile.x, 0, tile.y), Quaternion.identity);
								break;
							case 4:
								thisWall = (GameObject)GameObject.Instantiate(wall4, new Vector3(tile.x, 0, tile.y), Quaternion.identity);
								break;
							case 5:
								thisWall = (GameObject)GameObject.Instantiate(wall5, new Vector3(tile.x, 0, tile.y), Quaternion.identity);
								break;
							default:
								thisWall = (GameObject)GameObject.Instantiate(wall, new Vector3(tile.x, 0, tile.y), Quaternion.identity);
								break;
						}
						thisWall.name = "Wall";
						thisWall.transform.parent = transform;
						Dictionary<CompassDirections, TileDescriptor> wallNeighbours = tile.GetNeighbours(false, TileType.Floor);
						if(wallNeighbours.ContainsKey(CompassDirections.East) || wallNeighbours.ContainsKey(CompassDirections.West))
						{
							thisWall.transform.Rotate(new Vector3(0,90,0));
						}
						break;
					case TileType.Divider:
						GameObject thisDivider = (GameObject)GameObject.Instantiate(divider, new Vector3(tile.x, 0, tile.y), Quaternion.identity);
						thisDivider.name = "Divider";
						thisDivider.transform.parent = transform;
						break;
					case TileType.Junction:
						GameObject thisJunction = (GameObject)GameObject.Instantiate(junction, new Vector3(tile.x, 0, tile.y), Quaternion.identity);
						thisJunction.name = "Divider";
						thisJunction.transform.parent = transform;
						Dictionary<CompassDirections, TileDescriptor> junctionNeighbours = tile.GetNeighbours(false, TileType.Wall);
						if(!junctionNeighbours.ContainsKey(CompassDirections.North))
						{
							thisJunction.transform.Rotate(new Vector3(0, 270, 0));
						}
						else if(!junctionNeighbours.ContainsKey(CompassDirections.East))
						{
							thisJunction.transform.Rotate(new Vector3(0, 180, 0));
						}
						else if(!junctionNeighbours.ContainsKey(CompassDirections.South))
						{
							thisJunction.transform.Rotate(new Vector3(0, 90, 0));
						}
						break;
					case TileType.Corner:
						GameObject thisCorner = (GameObject)GameObject.Instantiate(corner, new Vector3(tile.x, 0, tile.y), Quaternion.identity);
						thisCorner.name = "Corner";
						thisCorner.transform.parent = transform;
						Dictionary<CompassDirections, TileDescriptor> cornerNeighbours = tile.GetNeighbours(false, TileType.Wall);
						if(cornerNeighbours.ContainsKey(CompassDirections.North) && cornerNeighbours.ContainsKey(CompassDirections.East))
						{
							thisCorner.transform.Rotate(new Vector3(0, 90, 0));
						}
						else if(cornerNeighbours.ContainsKey(CompassDirections.West) && cornerNeighbours.ContainsKey(CompassDirections.North))
						{
							thisCorner.transform.Rotate(new Vector3(0, 180, 0));
						}
						else if(cornerNeighbours.ContainsKey(CompassDirections.South) && cornerNeighbours.ContainsKey(CompassDirections.West))
						{
							thisCorner.transform.Rotate(new Vector3(0, 270, 0));
						}
						break;
					case TileType.Door:
						if(tile.GetNeighbours(false, TileType.Door).ContainsKey(CompassDirections.North) || tile.GetNeighbours(false, TileType.Door).ContainsKey(CompassDirections.East))
						{
							GameObject thisDoor = (GameObject)GameObject.Instantiate(door, new Vector3(tile.x, 0, tile.y), Quaternion.identity);
							thisDoor.name = "Door";
							thisDoor.transform.parent = transform;
							Dictionary<CompassDirections, TileDescriptor> doorNeighbours = tile.GetNeighbours(false, TileType.Door);
							foreach(KeyValuePair<CompassDirections, TileDescriptor> neighbour in doorNeighbours)
							{
								switch(neighbour.Key)
								{
									case CompassDirections.North:
										thisDoor.transform.Translate(new Vector3(0, 0, -1));
										thisDoor.transform.Rotate(new Vector3(0, 90, 0));
										break;
									case CompassDirections.East:
										thisDoor.transform.Translate(new Vector3(1, 0, 0));
										break;
								}
							}
						}
						break;
				}
			}
		}
		
		RoomDescriptor startRoom = new RoomDescriptor(this, 0, 0, 99);
		foreach(RoomDescriptor roomDescriptor in grid)
		{
			if(roomDescriptor != null)
			{
				if(roomDescriptor.roomIndex == 0)
				{
					startRoom = roomDescriptor;
				}
			}
		}
		
		GameObject player = GameObject.Find("Player");
		player.transform.position = new Vector3((float)((startRoom.x * (roomWidth + 1)) + 1), 0, (float)((startRoom.y * (roomHeight + 1)) + 1));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

public enum TileType
{
	Floor,
	Wall,
	Corner,
	Divider,
	Junction,
	Door,
	None
}

public enum CompassDirections
{
	North,
	NorthEast,
	East,
	SouthEast,
	South,
	SouthWest,
	West,
	NorthWest,
	None
}
