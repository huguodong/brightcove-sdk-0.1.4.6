namespace com.brightcove.player.samples.webvtt
{

	using Uri = android.net.Uri;
	using Bundle = android.os.Bundle;
	using BrightcoveCaptionFormat = com.brightcove.player.captioning.BrightcoveCaptionFormat;
	using BrightcovePlayer = com.brightcove.player.view.BrightcovePlayer;
	using BrightcoveVideoView = com.brightcove.player.view.BrightcoveVideoView;

	/// <summary>
	/// This activity demonstrates how to play a video with closed
	/// captions in multiple languages.
	/// </summary>
	public class MainActivity : BrightcovePlayer
	{

		protected internal override void onCreate(Bundle savedInstanceState)
		{
			ContentView = R.layout.activity_main;
			brightcoveVideoView = (BrightcoveVideoView) findViewById(R.id.brightcove_video_view);
			base.onCreate(savedInstanceState);

			brightcoveVideoView.VideoURI = Uri.parse("android.resource://" + PackageName + "/" + R.raw.sintel_trailer);

			BrightcoveCaptionFormat brightcoveCaptionFormat = BrightcoveCaptionFormat.createCaptionFormat("text/vtt", "de");
			brightcoveVideoView.addSubtitleSource(Uri.parse("android.resource://" + PackageName + "/" + R.raw.sintel_trailer_de), brightcoveCaptionFormat);
			brightcoveCaptionFormat = BrightcoveCaptionFormat.createCaptionFormat("text/vtt", "en");
			brightcoveVideoView.addSubtitleSource(Uri.parse("android.resource://" + PackageName + "/" + R.raw.sintel_trailer_en), brightcoveCaptionFormat);
			brightcoveCaptionFormat = BrightcoveCaptionFormat.createCaptionFormat("text/vtt", "es");
			brightcoveVideoView.addSubtitleSource(Uri.parse("android.resource://" + PackageName + "/" + R.raw.sintel_trailer_es), brightcoveCaptionFormat);
			brightcoveCaptionFormat = BrightcoveCaptionFormat.createCaptionFormat("text/vtt", "fr");
			brightcoveVideoView.addSubtitleSource(Uri.parse("android.resource://" + PackageName + "/" + R.raw.sintel_trailer_fr), brightcoveCaptionFormat);
			brightcoveCaptionFormat = BrightcoveCaptionFormat.createCaptionFormat("text/vtt", "it");
			brightcoveVideoView.addSubtitleSource(Uri.parse("android.resource://" + PackageName + "/" + R.raw.sintel_trailer_it), brightcoveCaptionFormat);
			brightcoveCaptionFormat = BrightcoveCaptionFormat.createCaptionFormat("text/vtt", "nl");
			brightcoveVideoView.addSubtitleSource(Uri.parse("android.resource://" + PackageName + "/" + R.raw.sintel_trailer_nl), brightcoveCaptionFormat);
		}
	}

}