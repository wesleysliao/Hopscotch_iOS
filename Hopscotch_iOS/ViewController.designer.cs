// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace Hopscotch_iOS
{
    [Register ("ViewController")]
    partial class ViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton tileA { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton tileB { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (tileA != null) {
                tileA.Dispose ();
                tileA = null;
            }

            if (tileB != null) {
                tileB.Dispose ();
                tileB = null;
            }
        }
    }
}