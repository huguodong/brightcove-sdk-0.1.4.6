namespace com.brightcove.player.samples.vmap.basic
{

	using Bundle = android.os.Bundle;
	using Log = android.util.Log;
	using View = android.view.View;
	using ViewGroup = android.view.ViewGroup;
	using VideoView = android.widget.VideoView;
	using DeliveryType = com.brightcove.player.media.DeliveryType;
	using VideoFields = com.brightcove.player.media.VideoFields;
	using Video = com.brightcove.player.model.Video;
	using BrightcovePlayer = com.brightcove.player.view.BrightcovePlayer;
	using BrightcoveVideoView = com.brightcove.player.view.BrightcoveVideoView;
	using VMAPComponent = com.brightcove.vmap.VMAPComponent;

	/// <summary>
	/// Example of how to use the VMAP plugin.
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

		private VMAPComponent vmapComponent;

		protected internal override void onCreate(Bundle savedInstanceState)
		{
			ContentView = R.layout.activity_main;
			brightcoveVideoView = (BrightcoveVideoView) findViewById(R.id.brightcove_video_view);
			base.onCreate(savedInstanceState);

			vmapComponent = new VMAPComponent(brightcoveVideoView);

			View view = findViewById(R.id.ad_frame);

			if ((view != null) && (view is ViewGroup))
			{
				vmapComponent.addCompanionContainer((ViewGroup) view);
			}
			else
			{
				Log.e(TAG, "Companion container must be an instance of a ViewGroup");
			}

			Video video = Video.createVideo("http://media.w3.org/2010/05/sintel/trailer.mp4", DeliveryType.MP4);
			video.Properties.put(VMAPComponent.VMAP_URL, "http://pubads.g.doubleclick.net/gampad/ads?sz=640x480&iu=/124319096/external/ad_rule_samples&ciu_szs=300x250&ad_rule=1&impl=s&gdfp_req=1&env=vp&output=vmap&unviewed_position_start=1&cust_params=deployment%3Ddevsite%26sample_ar%3Dpremidpost&cmsid=496&vid=short_onecue&correlator=");

			brightcoveVideoView.add(video);
			brightcoveVideoView.start();
		}
	}

}