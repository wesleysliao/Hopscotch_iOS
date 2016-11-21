using System;
namespace Hopscotch_iOS
{
	public class Tile
	{
		public Tile(int x_pos, int y_pos, int ID)
		{
			this.x_pos = x_pos;
			this.y_pos = y_pos;
			this.ID = ID;
		}

		public int x_pos;
		public int y_pos;
		public int ID;
		bool lit;

	}
}
