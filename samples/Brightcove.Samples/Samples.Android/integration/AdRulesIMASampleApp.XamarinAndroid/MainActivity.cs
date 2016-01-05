using System.Collections.Generic;

using BrightcoveSDK.Player.Views;
using BrightcoveSDK.Player.Media;
using BrightcoveSDK.Player.MediaController;
using BrightcoveSDK.Player.Events;
using BrightcoveSDK.Player.Edge;
using BrightcoveSDK.Player.Utils;
using BrightcoveSDK.Player.Model;
using Android.OS;

namespace com.brightcove.player.samples.ima.adrules
{

	/// <summary>
	/// This app illustrates how to use "Ad Rules" the Google IMA plugin
	/// and the Brightcove Player for Android.  Note: cue points are not
	/// used with Ad Rules.
	/// 
	/// @author Paul Matthew Reilly (original code)
	/// @author Paul Michael Reilly (added explanatory comments)
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

		private IEventEmitter eventEmitter;
		private GoogleIMAComponent googleIMAComponent;
		private string adRulesURL = "http://pubads.g.doubleclick.net/gampad/ads?sz=640x480&iu=%2F15018773%2Feverything2&ciu_szs=300x250%2C468x60%2C728x90&impl=s&gdfp_req=1&env=vp&output=xml_vast2&unviewed_position_start=1&url=dummy&correlator=[timestamp]&cmsid=133&vid=10XWSh7W4so&ad_rule=1";

		protected override void OnCreate(Bundle savedInstanceState)
		{
			// When extending the BrightcovePlayer, we must assign the BrightcoveVideoView before
			// entering the superclass. This allows for some stock video player lifecycle
			// management.
			SetContentView(Resource.layout.ima_activity_main);
			brightcoveVideoView = FindViewById<BrightcoveVideoView>(Resource.id.brightcove_video_view);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.brightcove.player.mediacontroller.BrightcoveMediaController mediaController = new com.brightcove.player.mediacontroller.BrightcoveMediaController(brightcoveVideoView);
			BrightcoveMediaController mediaController = new BrightcoveMediaController(brightcoveVideoView);

			// Add "Ad Markers" where the Ads Manager says ads will appear.
			mediaController.AddListener(GoogleIMAEventType.ADS_MANAGER_LOADED, new EventListenerAnonymousInnerClassHelper(this, mediaController));
			brightcoveVideoView.MediaController = mediaController;
			base.onCreate(savedInstanceState);
			eventEmitter = brightcoveVideoView.EventEmitter;

			// Use a procedural abstraction to setup the Google IMA SDK via the plugin.
			setupGoogleIMA();

			IDictionary<string, string> options = new Dictionary<string, string>();
			IList<string> values = new List<string>(Arrays.asList(VideoFields.DEFAULT_FIELDS));
			values.Remove(VideoFields.HLS_URL);
			options["video_fields"] = StringUtil.join(values, ",");

			BrightcoveSDK.Player.Media.Catalog catalog = new Catalog("ErQk9zUeDVLIp8Dc7aiHKq8hDMgkv5BFU7WGshTc-hpziB3BuYh28A..");
			catalog.findVideoByReferenceID("shark", new VideoListenerAnonymousInnerClassHelper(this));
		}

		private class EventListenerAnonymousInnerClassHelper : IEventListener
		{
			private readonly MainActivity outerInstance;

			private BrightcoveMediaController mediaController;

			public EventListenerAnonymousInnerClassHelper(MainActivity outerInstance, BrightcoveMediaController mediaController)
			{
				this.outerInstance = outerInstance;
				this.mediaController = mediaController;
			}

			public override void ProcessEvent(Android.Util.EventLog.Event e)
			{
				AdsManager manager = (AdsManager) e.Properties.Get("adsManager");
				IList<float?> cuepoints = manager.AdCuePoints;
				for (int i = 0; i < cuepoints.Count; i++)
				{
					float? cuepoint = cuepoints[i];
					mediaController.BrightcoveSeekBar.AddMarker((int)(cuepoint * DateUtils.SECOND_IN_MILLIS));
				}
			}
		}

		private class VideoListenerAnonymousInnerClassHelper : Java.Lang.Object, IVideoListener
		{
			private readonly MainActivity outerInstance;

			public VideoListenerAnonymousInnerClassHelper(MainActivity outerInstance)
			{
				this.outerInstance = outerInstance;
			}

			public virtual void onVideo(Video video)
			{
				brightcoveVideoView.Add(video);

				// Auto play: the GoogleIMAComponent will postpone
				// playback until the Ad Rules are loaded.
				brightcoveVideoView.Start();
			}

			public virtual void onError(string error)
			{
				Log.e(outerInstance.TAG, error);
			}
		}

		/// <summary>
		/// Setup the Brightcove IMA Plugin.
		/// </summary>
		private void setupGoogleIMA()
		{
			// Establish the Google IMA SDK factory instance.
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.google.ads.interactivemedia.v3.api.ImaSdkFactory sdkFactory = com.google.ads.interactivemedia.v3.api.ImaSdkFactory.getInstance();
			ImaSdkFactory sdkFactory = ImaSdkFactory.Instance;

			// Enable logging up ad start.
			eventEmitter.on(GoogleIMAEventType.DID_START_AD, new EventListenerAnonymousInnerClassHelper2(this));

			// Enable logging any failed attempts to play an ad.
			eventEmitter.on(GoogleIMAEventType.DID_FAIL_TO_PLAY_AD, new EventListenerAnonymousInnerClassHelper3(this));

			// Enable Logging upon ad completion.
			eventEmitter.on(GoogleIMAEventType.DID_COMPLETE_AD, new EventListenerAnonymousInnerClassHelper4(this));

			// Set up a listener for initializing AdsRequests. The Google
			// IMA plugin emits an ad request event as a result of
			// initializeAdsRequests() being called.
			eventEmitter.on(GoogleIMAEventType.ADS_REQUEST_FOR_VIDEO, new EventListenerAnonymousInnerClassHelper5(this, sdkFactory));

			// Create the Brightcove IMA Plugin and pass in the event
			// emitter so that the plugin can integrate with the SDK.
			googleIMAComponent = new GoogleIMAComponent(brightcoveVideoView, eventEmitter, true);

			// Calling GoogleIMAComponent.initializeAdsRequests() is no longer necessary.
		}

		private class EventListenerAnonymousInnerClassHelper2 : Java.Lang.Object, IEventListener
		{
			private readonly MainActivity outerInstance;

			public EventListenerAnonymousInnerClassHelper2(MainActivity outerInstance)
			{
				this.outerInstance = outerInstance;
			}

			public override void ProcessEvent(Event @event)
			{
				Log.v(outerInstance.TAG, @event.Type);
			}
		}

		private class EventListenerAnonymousInnerClassHelper3 : Java.Lang.Object, IEventListener
		{
			private readonly MainActivity outerInstance;

			public EventListenerAnonymousInnerClassHelper3(MainActivity outerInstance)
			{
				this.outerInstance = outerInstance;
			}

			public override void processEvent(Event @event)
			{
				Log.v(outerInstance.TAG, @event.Type);
			}
		}

		private class EventListenerAnonymousInnerClassHelper4 : Java.Lang.Object, IEventListener
		{
			private readonly MainActivity outerInstance;

			public EventListenerAnonymousInnerClassHelper4(MainActivity outerInstance)
			{
				this.outerInstance = outerInstance;
			}

			public override void ProcessEvent(Android.Util.EventLog.Event e)
			{
				Log.v(outerInstance.TAG, e.Type);
			}
		}

		private class EventListenerAnonymousInnerClassHelper5 : Java.Lang.Object, IEventListener
		{
			private readonly MainActivity outerInstance;

			private ImaSdkFactory sdkFactory;

			public EventListenerAnonymousInnerClassHelper5(MainActivity outerInstance, ImaSdkFactory sdkFactory)
			{
				this.outerInstance = outerInstance;
				this.sdkFactory = sdkFactory;
			}

			public override void ProcessEvent(Android.Util.EventLog.Event e)
			{
				// Create a container object for the ads to be presented.
				AdDisplayContainer container = sdkFactory.CreateAdDisplayContainer();
				container.Player = outerInstance.googleIMAComponent.VideoAdPlayer;
				container.AdContainer = brightcoveVideoView;

				// Build an ads request object and point it to the ad
				// display container created above.
				AdsRequest adsRequest = sdkFactory.CreateAdsRequest();
				adsRequest.AdTagUrl = outerInstance.adRulesURL;
				adsRequest.AdDisplayContainer = container;

				List<AdsRequest> adsRequests = new List<AdsRequest>(1);
				adsRequests.Add(adsRequest);

				// Respond to the event with the new ad requests.
				e.Properties.Put(GoogleIMAComponent.ADS_REQUESTS, adsRequests);
				outerInstance.eventEmitter.Respond(@event);
			}
		}
	}

}