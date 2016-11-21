using System;

using UIKit;
using CoreBluetooth;
using CoreFoundation;

namespace Hopscotch_iOS
{
	public partial class ViewController : UIViewController
	{
		MySimpleCBCentralManagerDelegate myDel;

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
				{4, 3, 2, 255, 255, 1, 2, 3, 0},
				{3, 1, 4, 255, 255, 2, 3, 0, 1},
				{5, 255, 255, 255, 255, 0, 1, 2, 3},
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
			},4);

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
