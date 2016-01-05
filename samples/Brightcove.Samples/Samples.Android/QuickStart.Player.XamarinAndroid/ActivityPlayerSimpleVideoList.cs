
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

namespace QuickStart.Player.XamarinAndroid
{
	[Activity (Label = "ActivityPlayerSimpleVideoList")]			
	public class ActivityPlayerSimpleVideoList : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Create your application here
			SetContentView (Resource.Layout.PlayerSimpleVideoList);

			BrightcoveVideoView brightcoveVideoView = FindViewById<BrightcoveVideoView>(Resource.Id.brightcove_video_view);

			List<Video> videoList = new List<Video>();
			videoList.Add(Video.CreateVideo("http://solutions.brightcove.com/bcls/assets/videos/Bird_Titmouse.mp4"));
			videoList.Add(Video.CreateVideo("http://solutions.brightcove.com/bcls/assets/videos/Water-Splashing.mp4"));

			MediaController controller = new MediaController(this);
			//overloads problems
			// 
			brightcoveVideoView.SetMediaController(controller);

			brightcoveVideoView.AddAll(videoList);
			brightcoveVideoView.Start();

			return;
		}
	}
}

