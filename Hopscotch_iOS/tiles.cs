﻿using System;
using System.Collections.Generic;
            
namespace Hopscotch_iOS
{
	public class Tiles
	{
		private List<Tile> tileList;
		private int[,] neighbors; 
		
		public Tiles(int[,] net_map, int net_map_len)
		{
			if ((net_map.GetLength(0) != 16) || (net_map.GetLength(1) != 9))
			{
				// net map incorrect size.
				throw new FormatException();
			}

			neighbors = new int[net_map_len+1, 4];

			neighbors[0, 0] = 1;
			neighbors[0, 1] = 255;
			neighbors[0, 2] = 255;
			neighbors[0, 3] = 255;

			for (int i = 0; i < net_map_len; i++)
			{
				switch (net_map[i, 5])
				{
					case 0: //N side facing N
						neighbors[net_map[i, 0], 0] = net_map[i, 1];
						neighbors[net_map[i, 0], 1] = net_map[i, 2];
						neighbors[net_map[i, 0], 2] = net_map[i, 3];
						neighbors[net_map[i, 0], 3] = net_map[i, 4];
						break;
					case 1: //W side facing N
						neighbors[net_map[i, 0], 0] = net_map[i, 4];
						neighbors[net_map[i, 0], 1] = net_map[i, 1];
						neighbors[net_map[i, 0], 2] = net_map[i, 2];
						neighbors[net_map[i, 0], 3] = net_map[i, 3];
						break;
					case 2: //S side facing N
						neighbors[net_map[i, 0], 0] = net_map[i, 3];
						neighbors[net_map[i, 0], 1] = net_map[i, 4];
						neighbors[net_map[i, 0], 2] = net_map[i, 1];
						neighbors[net_map[i, 0], 3] = net_map[i, 2];
						break;
					case 3: //E side facing N
						neighbors[net_map[i, 0], 0] = net_map[i, 2];
						neighbors[net_map[i, 0], 1] = net_map[i, 3];
						neighbors[net_map[i, 0], 2] = net_map[i, 4];
						neighbors[net_map[i, 0], 3] = net_map[i, 1];
						break;
				}
			}

			tileList = new List<Tile>(net_map_len);

			addTile(0, 1, 1);
		}


		public bool tileWithIDExists(int ID)
		{
			for (int i = 0; i < tileList.Count; i++)
			{
				if (tileList[i].ID == ID)
				{
					return true;
				}
			}

			return false;
		}

		private void addTile(int x_pos, int y_pos, int ID)
		{
			if (ID == 255 || ID == 0)
				return;
			
			tileList.Add(new Tile(x_pos, y_pos, ID));

			if (!tileWithIDExists(neighbors[ID,0]))
			{
				addTile(x_pos, y_pos+1, neighbors[ID,0]);
			}

			if (!tileWithIDExists(neighbors[ID,1]))
			{
				addTile(x_pos+1, y_pos, neighbors[ID,1]);
			}

			if (!tileWithIDExists(neighbors[ID,2]))
			{
				addTile(x_pos, y_pos-1, neighbors[ID,2]);
			}

			if (!tileWithIDExists(neighbors[ID,3]))
			{
				addTile(x_pos-1, y_pos, neighbors[ID,3]);
			}
		}
	}
}