namespace com.brightcove.player.samples.bundledvideo.basic
{

	using Uri = android.net.Uri;
	using Bundle = android.os.Bundle;
	using MediaController = android.widget.MediaController;
	using EventEmitter = com.brightcove.player.@event.EventEmitter;
	using Video = com.brightcove.player.model.Video;
	using BrightcovePlayer = com.brightcove.player.view.BrightcovePlayer;
	using BrightcoveVideoView = com.brightcove.player.view.BrightcoveVideoView;

	/// <summary>
	/// This app illustrates how to load and play a bundled video with the Brightcove Player for Android.
	/// 
	/// @author Billy Hnath
	/// </summary>
	public class MainActivity : BrightcovePlayer
	{

		protected internal override void onCreate(Bundle savedInstanceState)
		{
			// When extending the BrightcovePlayer, we must assign the BrightcoveVideoView
			// before entering the superclass. This allows for some stock video player lifecycle
			// management.
			ContentView = R.layout.bundled_video_activity_main;
			brightcoveVideoView = (BrightcoveVideoView) findViewById(R.id.brightcove_video_view);
			base.onCreate(savedInstanceState);

			// Add a test video from the res/raw directory to the BrightcoveVideoView.
			string PACKAGE_NAME = ApplicationContext.PackageName;
			Uri video = Uri.parse("android.resource://" + PACKAGE_NAME + "/" + R.raw.shark);
			brightcoveVideoView.add(Video.createVideo(video.ToString()));
		}
	}
}