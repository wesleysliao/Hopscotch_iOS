using System;

using UIKit;
using CoreBluetooth;
using CoreFoundation;
using System.Collections.Generic;

namespace Hopscotch_iOS
{
	public partial class ViewController : UIViewController
	{
		MySimpleCBCentralManagerDelegate myDel;

		List<Tile> tileList;

		int tileMapWidth;
		int tileMapHeight;
		int tileMap_x_offset;
		int tileMap_y_offset;


		protected ViewController(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.

			//Important to retain reference, else will be GC'ed
			myDel = new MySimpleCBCentralManagerDelegate();
			var myMgr = new CBCentralManager(myDel, DispatchQueue.CurrentQueue);

			tileList = new List<Tile>();

			ParseTileMap(new int[,]
			{
				{1, 2, 3, 255, 0, 3, 0, 1, 2},
				{2, 4, 1, 255, 255, 0, 1, 2, 3},
				{4, 3, 2, 255, 5, 1, 2, 3, 0},
				{3, 1, 4, 255, 255, 2, 3, 0, 1},
				{5, 4, 255, 255, 255, 2, 3, 0, 1},
				{6, 2, 255, 7, 255, 1, 2, 3, 0},
				{7, 6, 255, 255, 255, 1, 2, 3, 0},
				{8, 255, 255, 255, 255, 0, 1, 2, 3},
				{9, 255, 255, 255, 255, 0, 1, 2, 3},
				{10, 255, 255, 255, 255, 0, 1, 2, 3},
				{11, 255, 255, 255, 255, 0, 1, 2, 3},
				{12, 255, 255, 255, 255, 0, 1, 2, 3},
				{13, 255, 255, 255, 255, 0, 1, 2, 3},
				{14, 255, 255, 255, 255, 0, 1, 2, 3},
				{15, 255, 255, 255, 255, 0, 1, 2, 3},
				{16, 255, 255, 255, 255, 0, 1, 2, 3}
			},5);


			for (int i = 0; i < tileList.Count; i++)
			{
				int padding = (int)(View.Frame.Width / 10);
				int tilesize;
				if ((float)tileMapWidth > (float)(tileMapHeight / 2))
				{
					tilesize = (int)((View.Frame.Width - (2 * padding)) / tileMapWidth);
				}
				else
				{
					tilesize = (int)((View.Frame.Height - (2 * padding)) / tileMapHeight);
				}
				var frame = new CoreGraphics.CGRect(View.Frame.Left + padding + (-tileMap_x_offset * tilesize) + (tileList[i].x_pos * tilesize), View.Frame.Bottom - padding + (tileList[i].y_pos * -tilesize), tilesize, tilesize);

				tileList[i].Frame = frame;

				tileList[i].SetTitle(tileList[i].ID.ToString(), UIControlState.Normal);
				tileList[i].BackgroundColor = UIColor.FromRGB(0, 50, 100 + ((tileList[i].ID % 3) * 60));
				tileList[i].TouchUpInside += (sender, e) => SendSelectedCommandToTile((Hopscotch_iOS.Tile)sender, e);
				View.Add(tileList[i]);
			}

		}

		int[,] neighbors;

		private void ParseTileMap(int[,] net_map, int net_map_len)
		{
			

			if ((net_map.GetLength(0) != 16) || (net_map.GetLength(1) != 9))
			{
				// net map incorrect size.
				throw new FormatException();
			}

			neighbors = new int[net_map_len + 1, 4];

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

			addTile(0, 1, 1); //This is recursive and adds all the tiles

			int smallest_x = 0;
			int biggest_x = 0;
			int smallest_y = 1;
			int biggest_y = 1;

			for (int i = 0; i < tileList.Count; i++)
			{
				if (tileList[i].x_pos < smallest_x)
					smallest_x = tileList[i].x_pos;
				if (tileList[i].x_pos > biggest_x)
					biggest_x = tileList[i].x_pos;
				if (tileList[i].y_pos < smallest_y)
					smallest_y = tileList[i].y_pos;
				if (tileList[i].y_pos > biggest_y)
					biggest_y = tileList[i].y_pos;
			}

			tileMapWidth = biggest_x - smallest_x + 1;
			tileMapHeight = biggest_y - smallest_y + 1;

			tileMap_x_offset = smallest_x;
			tileMap_y_offset = smallest_y;
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

			if (!tileWithIDExists(neighbors[ID, 0]))
			{
				addTile(x_pos, y_pos + 1, neighbors[ID, 0]);
			}

			if (!tileWithIDExists(neighbors[ID, 1]))
			{
				addTile(x_pos + 1, y_pos, neighbors[ID, 1]);
			}

			if (!tileWithIDExists(neighbors[ID, 2]))
			{
				addTile(x_pos, y_pos - 1, neighbors[ID, 2]);
			}

			if (!tileWithIDExists(neighbors[ID, 3]))
			{
				addTile(x_pos - 1, y_pos, neighbors[ID, 3]);
			}
		}

		public int Count()
		{
			return tileList.Count;
		}

		public void UpdateTileLitState()
		{
			for (int i = 0; i < tileList.Count; i++)
			{
				if (tileList[i].lit)
					tileList[i].BackgroundColor = UIColor.White;
				else
					tileList[i].BackgroundColor = UIColor.FromRGB(0, 50, 100 + ((tileList[i].ID % 3) * 60));
			}
		}

		public void SendSelectedCommandToTile(Tile sender, EventArgs e)
		{
			sender.lit = !sender.lit;
			UpdateTileLitState();
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}

		partial void UIButton29_TouchUpInside(UIButton sender)
		{
			//Send tile config request
			throw new NotImplementedException();
		}

		partial void modeChanged(UISwitch sender)
		{
			//send tile mode change
			throw new NotImplementedException();
		}
	}
}
