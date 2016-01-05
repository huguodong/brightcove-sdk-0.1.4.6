namespace com.brightcove.player.samples.closedcaptioning.dfxp
{

	using Bundle = android.os.Bundle;
	using Log = android.util.Log;
	using Menu = android.view.Menu;
	using MenuInflater = android.view.MenuInflater;
	using MenuItem = android.view.MenuItem;

	using Catalog = com.brightcove.player.media.Catalog;
	using VideoListener = com.brightcove.player.media.VideoListener;
	using Video = com.brightcove.player.model.Video;
	using BrightcovePlayer = com.brightcove.player.view.BrightcovePlayer;
	using BrightcoveVideoView = com.brightcove.player.view.BrightcoveVideoView;

	/// <summary>
	/// A sample application for demonstrating playback and control of DFXP/TTML captioned media.
	/// 
	/// @author Billy Hnath (bhnath@brightcove.com)
	/// </summary>
	public class MainActivity : BrightcovePlayer
	{
		private bool InstanceFieldsInitialized = false;

		public MainActivity()
		{
			if (!InstanceFieldsInitialized)
			{
				InitializeInstanceFields();
				InstanceFieldsInitialized = true;
			}
		}

		private void InitializeInstanceFields()
		{
			TAG = this.GetType().Name;
		}

		private string TAG;

		protected internal override void onCreate(Bundle savedInstanceState)
		{
			// When extending the BrightcovePlayer, we must assign the BrightcoveVideoView
			// before entering the superclass. This allows for some stock video player lifecycle
			// management.
			ContentView = R.layout.activity_main;
			brightcoveVideoView = (BrightcoveVideoView) findViewById(R.id.brightcove_video_view);
			base.onCreate(savedInstanceState);

			// Add a test video to the BrightcoveVideoView.
			Catalog catalog = new Catalog("ZUPNyrUqRdcAtjytsjcJplyUc9ed8b0cD_eWIe36jXqNWKzIcE6i8A..");
			catalog.findVideoByID("3637288623001", new VideoListenerAnonymousInnerClassHelper(this));
		}

		private class VideoListenerAnonymousInnerClassHelper : VideoListener
		{
			private readonly MainActivity outerInstance;

			public VideoListenerAnonymousInnerClassHelper(MainActivity outerInstance)
			{
				this.outerInstance = outerInstance;
			}

			public override void onVideo(Video video)
			{
				brightcoveVideoView.add(video);
			}

			public override void onError(string s)
			{
				Log.e(outerInstance.TAG, "Could not load video: " + s);
			}
		}

		public override bool onCreateOptionsMenu(Menu menu)
		{
			MenuInflater menuInflater = MenuInflater;
			menuInflater.inflate(R.menu.menu, menu);
			return true;
		}

		public override bool onOptionsItemSelected(MenuItem item)
		{
			switch (item.ItemId)
			{
				case R.id.action_cc_settings:
					showClosedCaptioningDialog();
					return true;
				default:
					return base.onOptionsItemSelected(item);
			}
		}
	}
}