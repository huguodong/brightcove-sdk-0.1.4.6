namespace com.brightcove.player.samples.texture.basic
{

	using Activity = android.app.Activity;
	using Bundle = android.os.Bundle;
	using Log = android.util.Log;
	using MediaController = android.widget.MediaController;
	using DeliveryType = com.brightcove.player.media.DeliveryType;
	using Video = com.brightcove.player.model.Video;
	using BrightcovePlayer = com.brightcove.player.view.BrightcovePlayer;
	using BrightcoveTextureVideoView = com.brightcove.player.view.BrightcoveTextureVideoView;

	/// <summary>
	/// This app illustrates how to use the BrightcoveTextureVideoView.
	/// 
	/// @author Paul Matthew Reilly (original code)
	/// </summary>
	public class MainActivity : BrightcovePlayer
	{

		public static readonly string TAG = typeof(MainActivity).Name;

		protected internal override void onCreate(Bundle savedInstanceState)
		{
			ContentView = R.layout.activity_main;
			brightcoveVideoView = (BrightcoveTextureVideoView) findViewById(R.id.brightcove_video_view);
			base.onCreate(savedInstanceState);

			Video video = Video.createVideo("http://media.w3.org/2010/05/sintel/trailer.mp4", DeliveryType.MP4);
			brightcoveVideoView.add(video);
			brightcoveVideoView.start();
		}
	}

}