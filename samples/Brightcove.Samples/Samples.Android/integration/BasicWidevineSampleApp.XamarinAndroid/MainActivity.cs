namespace com.brightcove.player.samples.widevine.basic
{

	using Activity = android.app.Activity;
	using Bundle = android.os.Bundle;
	using Log = android.util.Log;
	using MediaController = android.widget.MediaController;

	using WidevinePlugin = com.brightcove.drm.widevine.WidevinePlugin;
	using Catalog = com.brightcove.player.media.Catalog;
	using VideoListener = com.brightcove.player.media.VideoListener;
	using Video = com.brightcove.player.model.Video;
	using BrightcovePlayer = com.brightcove.player.view.BrightcovePlayer;
	using BrightcoveVideoView = com.brightcove.player.view.BrightcoveVideoView;

	/// <summary>
	/// This app illustrates how to use the Widevine Plugin with the
	/// Brightcove Player for Android.
	/// 
	/// @author Paul Matthew Reilly (original code)
	/// @author Paul Michael Reilly (added explanatory comments)
	/// </summary>
	public class MainActivity : BrightcovePlayer
	{

		public static readonly string TAG = typeof(MainActivity).Name;

		protected internal override void onCreate(Bundle savedInstanceState)
		{
			// Establish the video object and use it's event emitter to get important notifications
			// and to control logging and media.
			ContentView = R.layout.basic_widevine;
			brightcoveVideoView = (BrightcoveVideoView) findViewById(R.id.bc_video_view);
			base.onCreate(savedInstanceState);

			// Set up the DRM licensing server to be handled by Brightcove with arbitrary device and
			// portal identifiers to fulfill the Widevine API contract.  These arguments will
			// suffice to create a Widevine plugin instance.
			string drmServerUri = "https://wvlic.brightcove.com/widevine/cypherpc/cgi-bin/GetEMMs.cgi";
			string deviceId = "device1234";
			string portalId = "brightcove";
			new WidevinePlugin(this, brightcoveVideoView, drmServerUri, deviceId, portalId);

			// Create the catalog object which will start and play the video.
			Catalog catalog = new Catalog("FqicLlYykdimMML7pj65Gi8IHl8EVReWMJh6rLDcTjTMqdb5ay_xFA..");
			catalog.findVideoByID("2142125168001", new VideoListenerAnonymousInnerClassHelper(this));
		}

		private class VideoListenerAnonymousInnerClassHelper : VideoListener
		{
			private readonly MainActivity outerInstance;

			public VideoListenerAnonymousInnerClassHelper(MainActivity outerInstance)
			{
				this.outerInstance = outerInstance;
			}


			public override void onError(string error)
			{
				Log.e(TAG, error);
			}

			public override void onVideo(Video video)
			{
				brightcoveVideoView.add(video);
				brightcoveVideoView.start();
			}
		}

	}

}