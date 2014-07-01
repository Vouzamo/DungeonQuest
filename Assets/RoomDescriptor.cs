using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class RoomDescriptor
{
	public Grid grid;
	public Int32 x;
	public Int32 y;
	public Int32 roomIndex;
	
	public RoomDescriptor(Grid grid, Int32 x, Int32 y, Int32 roomIndex)
	{
		this.grid = grid;
		this.x = x;
		this.y = y;
		this.roomIndex = roomIndex;
	}
	
	public Int32 CountNeighbours(Boolean allowDiagonals)
	{
		return GetNeighbours(allowDiagonals).Count;
	}
	
	public Dictionary<CompassDirections,RoomDescriptor> GetNeighbours(Boolean allowDiagonals)
	{
		Dictionary<CompassDirections,RoomDescriptor> neighbours = new Dictionary<CompassDirections, RoomDescriptor>();
		for(int direction = 0; direction < 8; direction++)
		{
			if(allowDiagonals || direction % 2 == 0)
			{
				CompassDirections compassDirection = (CompassDirections)direction;
				Vector2 directionVector = CompassDirectionToVector(compassDirection);
				Vector2 pointer = new Vector2(this.x + directionVector.x, this.y + directionVector.y);
				if(pointer.x >= 0 && pointer.x <= grid.grid.GetUpperBound(0) && pointer.y >= 0 && pointer.y <= grid.grid.GetUpperBound(1))
				{
					if(grid.grid[(Int32)pointer.x, (Int32)pointer.y] != null)
					{
						neighbours.Add(compassDirection, grid.grid[(Int32)pointer.x, (Int32)pointer.y]);
					}
				}
			}
		}
		return neighbours;
	}
	
	public static Vector2 CompassDirectionToVector(CompassDirections direction)
	{
		Vector2 unitVector;
		switch(direction)
		{
			case CompassDirections.North:
				unitVector = new Vector2(0,-1);
				break;
			case CompassDirections.NorthEast:
				unitVector = new Vector2(1, -1);
				break;
			case CompassDirections.East:
				unitVector = new Vector2(1, 0);
				break;
			case CompassDirections.SouthEast:
				unitVector = new Vector2(1, 1);
				break;
			case CompassDirections.South:
				unitVector = new Vector2(0, 1);
				break;
			case CompassDirections.SouthWest:
				unitVector = new Vector2(-1, 1);
				break;
			case CompassDirections.West:
				unitVector = new Vector2(-1, 0);
				break;
			case CompassDirections.NorthWest:
				unitVector = new Vector2(-1, -1);
				break;
			default:
				unitVector = Vector2.zero;
				break;
		}
		return unitVector;
	}
}