﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using MvvmZeroTestApp.iOS.CustomRenderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
 
[assembly: ExportRenderer(typeof(Page), typeof(BasePageRenderer))]
namespace MvvmZeroTestApp.iOS.CustomRenderers
{
    public class BasePageRenderer : PageRenderer
    {
        public override void ViewWillAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            ViewController.NavigationController.InteractivePopGestureRecognizer.Enabled = true;
            ViewController.NavigationController.InteractivePopGestureRecognizer.Delegate = new UIGestureRecognizerDelegate();
        }
    }
}