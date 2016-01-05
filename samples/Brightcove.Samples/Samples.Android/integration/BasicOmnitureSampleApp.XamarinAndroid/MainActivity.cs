namespace com.brightcove.player.samples.omniture.basic
{

	using ActionBar = android.app.ActionBar;
	using Activity = android.app.Activity;
	using Bundle = android.os.Bundle;
	using Log = android.util.Log;
	using Menu = android.view.Menu;
	using MenuItem = android.view.MenuItem;
	using WindowManager = android.view.WindowManager;
	using MediaController = android.widget.MediaController;
	using RelativeLayout = android.widget.RelativeLayout;

	using OmnitureComponent = com.brightcove.omniture.OmnitureComponent;
	using Event = com.brightcove.player.@event.Event;
	using EventEmitter = com.brightcove.player.@event.EventEmitter;
	using EventListener = com.brightcove.player.@event.EventListener;
	using EventLogger = com.brightcove.player.@event.EventLogger;
	using EventType = com.brightcove.player.@event.EventType;
	using Catalog = com.brightcove.player.media.Catalog;
	using PlaylistListener = com.brightcove.player.media.PlaylistListener;
	using Playlist = com.brightcove.player.model.Playlist;
	using Video = com.brightcove.player.model.Video;
	using BrightcovePlayer = com.brightcove.player.view.BrightcovePlayer;
	using BrightcoveVideoView = com.brightcove.player.view.BrightcoveVideoView;

	/// <summary>
	/// This app illustrates how to use the Omniture plugin with the Brightcove Player for Android.
	/// 
	/// @author Billy Hnath
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

		private EventEmitter eventEmitter;
		private OmnitureComponent omnitureComponent;

		protected internal override void onCreate(Bundle savedInstanceState)
		{
			// When extending the BrightcovePlayer, we must assign the BrightcoveVideoView
			// before entering the superclass. This allows for some stock video player lifecycle
			// management.
			ContentView = R.layout.omniture_activity_main;
			brightcoveVideoView = (BrightcoveVideoView) findViewById(R.id.brightcove_video_view);
			base.onCreate(savedInstanceState);

			eventEmitter = brightcoveVideoView.EventEmitter;

			setupOmniture();

			// Add a test video to the BrightcoveVideoView.
			Catalog catalog = new Catalog("ErQk9zUeDVLIp8Dc7aiHKq8hDMgkv5BFU7WGshTc-hpziB3BuYh28A..");
			catalog.findPlaylistByReferenceID("stitch", new PlaylistListenerAnonymousInnerClassHelper(this));
		}

		private class PlaylistListenerAnonymousInnerClassHelper : PlaylistListener
		{
			private readonly MainActivity outerInstance;

			public PlaylistListenerAnonymousInnerClassHelper(MainActivity outerInstance)
			{
				this.outerInstance = outerInstance;
			}

			public virtual void onPlaylist(Playlist playlist)
			{
				brightcoveVideoView.addAll(playlist.Videos);
			}

			public virtual void onError(string error)
			{
				Log.e(outerInstance.TAG, error);
			}
		}

		private void setupOmniture()
		{

			// Initializing the Omniture plugin with the JSON configuration file
			// and evar rulesets should be enough to get started.
			omnitureComponent = new OmnitureComponent(eventEmitter, this, "Android Sample App", "Android Sample Player");
		}
	}

}