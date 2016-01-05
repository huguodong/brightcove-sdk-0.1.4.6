
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using BrightcoveSDK.Player.Views;
using BrightcoveSDK.Player.Model;

namespace Sample
{
	[Activity (Label = "ActivityPlayerSimple")]			
	public class ActivityPlayerSimple : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Create your application here
			SetContentView (Resource.Layout.PlayerSimple);
			BrightcoveVideoView brightcoveVideoView = FindViewById<BrightcoveVideoView>(Resource.Id.brightcove_video_view);

	        brightcoveVideoView.Add
	        					(
	        					Video.CreateVideo
	        							(
	        							GetString(Resource.String.video), 
	        							BrightcoveSDK.Player.Media.DeliveryType.Mp4
	        							)
	        					);
	        brightcoveVideoView.Start();

        	return;
		}
	}
}

