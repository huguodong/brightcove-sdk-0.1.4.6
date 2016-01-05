
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
using BrightcoveSDK.Player.Media;

namespace QuickStart.Player.XamarinAndroid
{

	[Activity (Label = "ActivityPlayerSimpleFromCatalog")]			
	public class ActivityPlayerSimpleFromCatalog : Activity
	{
		static BrightcoveVideoView brightcoveVideoView;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Create your application here
			SetContentView (Resource.Layout.PlayerSimpleFromCatalog);

			brightcoveVideoView = FindViewById<BrightcoveVideoView>(Resource.Id.brightcove_video_view);

	        MediaController controller = new MediaController(this);
	        brightcoveVideoView.SetMediaController(controller);

	        // b489a672-c3ba-4765-af8e-7fc876ef06f7
	        // ayINLNrUaYhfX9K0AI6hpqVRUlVXCRysABFpxHS0mT_4fWYoPnD5pA..
	        Catalog catalog = new Catalog("XGuquNMCweRY0D3tt_VUotzuyIASMAzhUS4F8ZIWa_e0cYlKpA4WtQ..");

	        catalog.FindPlaylistByID("2764931905001", new PlaylistListener() );

        return;
		}

		public partial class PlaylistListener : Java.Lang.Object, IPlaylistListener
		{
	        //@Override
	        public void OnPlaylist(Playlist playlist) 
	        {
	            brightcoveVideoView.AddAll(playlist.Videos);
	            brightcoveVideoView.Start();

	            return;
	        }

	        // @Override
	        public void OnError(String s) 
	        {
	            throw new Java.Lang.RuntimeException(s);
	        }
		}
	}

}

