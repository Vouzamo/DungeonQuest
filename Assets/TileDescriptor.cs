using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TileDescriptor
{
	public Grid grid;
	public Int32 x;
	public Int32 y;
	public Boolean passable;
	public Boolean discovered;
	public TileType type;
	
	public TileDescriptor(Grid grid, Int32 x, Int32 y, Boolean passable, Boolean discovered, TileType type)
	{
		this.grid = grid;
		this.x = x;
		this.y = y;
		this.passable = passable;
		this.discovered = discovered;
		this.type = type;
	}

	public Int32 CountNeighbours(Boolean allowDiagonals)
	{
		return GetNeighbours(allowDiagonals).Count;
	}
	
	public Int32 CountNeighbours(Boolean allowDiagonals, TileType type)
	{
		Int32 count = 0;
		Dictionary<CompassDirections, TileDescriptor> neighbours = GetNeighbours(allowDiagonals);
		foreach(KeyValuePair<CompassDirections, TileDescriptor> neighbour in neighbours)
		{
			if(neighbour.Value.type == type)
			{
				count++;
			}
		}
		return count;
	}
	
	public Dictionary<CompassDirections,TileDescriptor> GetNeighbours(Boolean allowDiagonals)
	{
		Dictionary<CompassDirections,TileDescriptor> neighbours = new Dictionary<CompassDirections, TileDescriptor>();
		for(int direction = 0; direction < 8; direction++)
		{
			if(allowDiagonals || direction % 2 == 0)
			{
				CompassDirections compassDirection = (CompassDirections)direction;
				Vector2 directionVector = CompassDirectionToVector(compassDirection);
				Vector2 pointer = new Vector2(this.x + directionVector.x, this.y + directionVector.y);
				if(pointer.x >= 0 && pointer.x <= grid.map.GetUpperBound(0) && pointer.y >= 0 && pointer.y <= grid.map.GetUpperBound(1))
				{
					if(grid.map[(Int32)pointer.x, (Int32)pointer.y] != null)
					{
						neighbours.Add(compassDirection, grid.map[(Int32)pointer.x, (Int32)pointer.y]);
					}
				}
			}
		}
		return neighbours;
	}
	
	public Dictionary<CompassDirections,TileDescriptor> GetNeighbours(Boolean allowDiagonals, TileType type)
	{
		Dictionary<CompassDirections,TileDescriptor> neighbours = GetNeighbours(allowDiagonals);
		for(int direction = 0; direction < 8; direction++)
		{
			if(neighbours.ContainsKey((CompassDirections)direction))
			{
				if(neighbours[(CompassDirections)direction].type != type)
				{
					neighbours.Remove((CompassDirections)direction);
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