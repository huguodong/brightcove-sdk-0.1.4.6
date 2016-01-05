using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;


namespace QuickStart.Player.XamarinAndroid
{
	[Activity (Label = "QuickStart.Player.XamarinAndroid", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		Button buttonPlayerSimple;
		Button buttonPlayerSimpleVideoList;
		Button buttonPlayerSimpleWithControls;
		Button buttonPlayerSimpleFromCatalog;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it

			buttonPlayerSimple = FindViewById<Button>(Resource.Id.buttonPlayerSimple);
			buttonPlayerSimpleVideoList = FindViewById<Button>(Resource.Id.buttonPlayerSimpleVideoList);
			buttonPlayerSimpleWithControls = FindViewById<Button>(Resource.Id.buttonPlayerSimpleWithControls);
			buttonPlayerSimpleFromCatalog = FindViewById<Button>(Resource.Id.buttonPlayerSimpleFromCatalog);

			buttonPlayerSimple.Click += ButtonPlayerSimple_Click;
			buttonPlayerSimpleVideoList.Click += ButtonPlayerSimpleVideoList_Click;
			buttonPlayerSimpleWithControls.Click += ButtonPlayerSimpleWithControls_Click;
			buttonPlayerSimpleFromCatalog.Click += ButtonPlayerSimpleFromCatalog_Click;

			return;
		}

		protected void ButtonPlayerSimple_Click (object sender, EventArgs e)
		{
			Intent i = new Intent(this, typeof(ActivityPlayerSimple));
			this.StartActivity(i);

			return;
		}

		protected void ButtonPlayerSimpleVideoList_Click (object sender, EventArgs e)
		{
			Intent i = new Intent(this, typeof(ActivityPlayerSimpleVideoList));
			this.StartActivity(i);

			return;
		}

		protected void ButtonPlayerSimpleWithControls_Click (object sender, EventArgs e)
		{
			Intent i = new Intent(this, typeof(ActivityPlayerSimpleWithControls));
			this.StartActivity(i);

			return;
		}

		protected void ButtonPlayerSimpleFromCatalog_Click (object sender, EventArgs e)
		{
			Intent i = new Intent(this, typeof(ActivityPlayerSimpleFromCatalog));
			this.StartActivity(i);

			return;
		}
	}
}