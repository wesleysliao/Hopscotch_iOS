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

		List<UIButton> tile_button;

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

			var tileMap = new Tiles(new int[,]
			{
				{1, 2, 3, 255, 0, 3, 0, 1, 2},
				{2, 4, 1, 255, 255, 0, 1, 2, 3},
				{4, 3, 2, 255, 5, 1, 2, 3, 0},
				{3, 1, 4, 255, 255, 2, 3, 0, 1},
				{5, 4, 255, 255, 255, 2, 3, 0, 1},
				{6, 255, 255, 255, 255, 0, 1, 2, 3},
				{7, 255, 255, 255, 255, 0, 1, 2, 3},
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

			var random = new Random();

			tile_button = new List<UIButton>();

			for (int i = 0; i < tileMap.tileList.Count; i++)
			{
				var frame = new CoreGraphics.CGRect(200+(tileMap.tileList[i].x_pos*40), 200+ (tileMap.tileList[i].y_pos * -40), 40, 40);
				
				tile_button.Add(new UIButton(frame));
				tile_button[tile_button.Count - 1].SetTitle(tileMap.tileList[i].ID.ToString(), UIControlState.Normal);
				tile_button[tile_button.Count - 1].BackgroundColor = UIColor.FromRGB(0,50,100+((i%2)*127));
				View.Add(tile_button[tile_button.Count - 1]);
			}

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
