using System;

using UIKit;

namespace Hopscotch_iOS
{
	public partial class ViewController : UIViewController
	{
		protected ViewController(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.
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
			throw new NotImplementedException();
		}
	}
}
