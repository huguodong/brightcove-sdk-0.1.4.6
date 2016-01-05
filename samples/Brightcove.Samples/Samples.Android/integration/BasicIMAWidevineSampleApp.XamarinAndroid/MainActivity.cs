using System.Collections.Generic;

namespace com.brightcove.player.samples.imawidevine.basic
{

	using Bundle = android.os.Bundle;
	using DateUtils = android.text.format.DateUtils;
	using Log = android.util.Log;
	using ViewGroup = android.view.ViewGroup;
	using WidevinePlugin = com.brightcove.drm.widevine.WidevinePlugin;
	using GoogleIMAComponent = com.brightcove.ima.GoogleIMAComponent;
	using GoogleIMAEventType = com.brightcove.ima.GoogleIMAEventType;
	using GoogleIMAVideoAdPlayer = com.brightcove.ima.GoogleIMAVideoAdPlayer;
	using Event = com.brightcove.player.@event.Event;
	using EventEmitter = com.brightcove.player.@event.EventEmitter;
	using EventListener = com.brightcove.player.@event.EventListener;
	using EventType = com.brightcove.player.@event.EventType;
	using Catalog = com.brightcove.player.media.Catalog;
	using DeliveryType = com.brightcove.player.media.DeliveryType;
	using VideoListener = com.brightcove.player.media.VideoListener;
	using CuePoint = com.brightcove.player.model.CuePoint;
	using Source = com.brightcove.player.model.Source;
	using Video = com.brightcove.player.model.Video;
	using BrightcovePlayer = com.brightcove.player.view.BrightcovePlayer;
	using BrightcoveVideoView = com.brightcove.player.view.BrightcoveVideoView;
	using AdDisplayContainer = com.google.ads.interactivemedia.v3.api.AdDisplayContainer;
	using AdsRequest = com.google.ads.interactivemedia.v3.api.AdsRequest;
	using CompanionAdSlot = com.google.ads.interactivemedia.v3.api.CompanionAdSlot;
	using ImaSdkFactory = com.google.ads.interactivemedia.v3.api.ImaSdkFactory;


	/// <summary>
	/// This app illustrates how to use the IMA and Widevine plugins
	/// together with the Brightcove Player for Android.
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
		private GoogleIMAComponent googleIMAComponent;

		protected internal override void onCreate(Bundle savedInstanceState)
		{
			// When extending the BrightcovePlayer, we must assign the BrightcoveVideoView before
			// entering the superclass. This allows for some stock video player lifecycle
			// management.  Establish the video object and use it's event emitter to get important
			// notifications and to control logging.
			ContentView = R.layout.ima_widevine_activity_main;
			brightcoveVideoView = (BrightcoveVideoView) findViewById(R.id.brightcove_video_view);
			base.onCreate(savedInstanceState);
			eventEmitter = brightcoveVideoView.EventEmitter;

			// Use a procedural abstraction to setup the Google IMA SDK via the plugin and establish
			// a playlist listener object for our sample video: the Potter Puppet show.
			setupGoogleIMA();

			// Initialize the widevine plugin.
			setupWidevine();

			// Create the catalog object which will start and play the video.
			Catalog catalog = new Catalog("FqicLlYykdimMML7pj65Gi8IHl8EVReWMJh6rLDcTjTMqdb5ay_xFA..");
			catalog.findVideoByID("2223563028001", new VideoListenerAnonymousInnerClassHelper(this));
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
				Log.e(outerInstance.TAG, error);
			}

			public override void onVideo(Video video)
			{
				brightcoveVideoView.add(video);
				brightcoveVideoView.start();
			}
		}

		/// <summary>
		/// Provide a sample illustrative ad.
		/// </summary>
		private string[] googleAds = new string[] {"http://pubads.g.doubleclick.net/gampad/ads?sz=400x300&iu=%2F6062%2Fhanna_MA_group%2Fvideo_comp_app&ciu_szs=&impl=s&gdfp_req=1&env=vp&output=xml_vast2&unviewed_position_start=1&m_ast=vast&url=[referrer_url]&correlator=[timestamp]"};

		/// <summary>
		/// Specify where the ad should interrupt the main video.  This code provides a procedural
		/// abastraction for the Google IMA Plugin setup code.
		/// </summary>
		private void setupCuePoints(Source source)
		{
			string cuePointType = "ad";
			IDictionary<string, object> properties = new Dictionary<string, object>();
			IDictionary<string, object> details = new Dictionary<string, object>();

			// preroll
			CuePoint cuePoint = new CuePoint(CuePoint.PositionType.BEFORE, cuePointType, properties);
			details[Event.CUE_POINT] = cuePoint;
			eventEmitter.emit(EventType.SET_CUE_POINT, details);

			// midroll
			// Due HLS bugs in the Android MediaPlayer, midrolls are not supported.
			if (!source.DeliveryType.Equals(DeliveryType.HLS))
			{
				cuePoint = new CuePoint(10 * (int) DateUtils.SECOND_IN_MILLIS, cuePointType, properties);
				details[Event.CUE_POINT] = cuePoint;
				eventEmitter.emit(EventType.SET_CUE_POINT, details);
			}

			// postroll
			cuePoint = new CuePoint(CuePoint.PositionType.AFTER, cuePointType, properties);
			details[Event.CUE_POINT] = cuePoint;
			eventEmitter.emit(EventType.SET_CUE_POINT, details);
		}

		/// <summary>
		/// Setup the Brightcove IMA Plugin: add some cue points; establish a factory object to
		/// obtain the Google IMA SDK instance.
		/// </summary>
		private void setupGoogleIMA()
		{

			// Defer adding cue points until the set video event is triggered.
			eventEmitter.on(EventType.DID_SET_SOURCE, new EventListenerAnonymousInnerClassHelper(this));

			// Establish the Google IMA SDK factory instance.
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.google.ads.interactivemedia.v3.api.ImaSdkFactory sdkFactory = com.google.ads.interactivemedia.v3.api.ImaSdkFactory.getInstance();
			ImaSdkFactory sdkFactory = ImaSdkFactory.Instance;

			// Enable logging of ad starts.
			eventEmitter.on(GoogleIMAEventType.DID_START_AD, new EventListenerAnonymousInnerClassHelper2(this));

			// Enable logging of any failed attempts to play an ad.
			eventEmitter.on(GoogleIMAEventType.DID_FAIL_TO_PLAY_AD, new EventListenerAnonymousInnerClassHelper3(this));

			// Enable logging of ad completions.
			eventEmitter.on(GoogleIMAEventType.DID_COMPLETE_AD, new EventListenerAnonymousInnerClassHelper4(this));

			// Set up a listener for initializing AdsRequests. The Google IMA plugin emits an ad
			// request event in response to each cue point event.  The event processor (handler)
			// illustrates how to play ads back to back.
			eventEmitter.on(GoogleIMAEventType.ADS_REQUEST_FOR_VIDEO, new EventListenerAnonymousInnerClassHelper5(this, sdkFactory));

			// Create the Brightcove IMA Plugin and register the event emitter so that the plugin
			// can deal with video events.
			googleIMAComponent = new GoogleIMAComponent(brightcoveVideoView, eventEmitter);
		}

		private class EventListenerAnonymousInnerClassHelper : EventListener
		{
			private readonly MainActivity outerInstance;

			public EventListenerAnonymousInnerClassHelper(MainActivity outerInstance)
			{
				this.outerInstance = outerInstance;
			}

			public override void processEvent(Event @event)
			{
				outerInstance.setupCuePoints((Source) @event.properties.get(Event.SOURCE));
			}
		}

		private class EventListenerAnonymousInnerClassHelper2 : EventListener
		{
			private readonly MainActivity outerInstance;

			public EventListenerAnonymousInnerClassHelper2(MainActivity outerInstance)
			{
				this.outerInstance = outerInstance;
			}

			public override void processEvent(Event @event)
			{
				Log.v(outerInstance.TAG, @event.Type);
			}
		}

		private class EventListenerAnonymousInnerClassHelper3 : EventListener
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

		private class EventListenerAnonymousInnerClassHelper4 : EventListener
		{
			private readonly MainActivity outerInstance;

			public EventListenerAnonymousInnerClassHelper4(MainActivity outerInstance)
			{
				this.outerInstance = outerInstance;
			}

			public override void processEvent(Event @event)
			{
				Log.v(outerInstance.TAG, @event.Type);
			}
		}

		private class EventListenerAnonymousInnerClassHelper5 : EventListener
		{
			private readonly MainActivity outerInstance;

			private ImaSdkFactory sdkFactory;

			public EventListenerAnonymousInnerClassHelper5(MainActivity outerInstance, ImaSdkFactory sdkFactory)
			{
				this.outerInstance = outerInstance;
				this.sdkFactory = sdkFactory;
			}

			public override void processEvent(Event @event)
			{
				// Create a container object for the ads to be presented.
				AdDisplayContainer container = sdkFactory.createAdDisplayContainer();
				container.Player = outerInstance.googleIMAComponent.VideoAdPlayer;
				container.AdContainer = brightcoveVideoView;

				// Populate the container with the companion ad slots.
				List<CompanionAdSlot> companionAdSlots = new List<CompanionAdSlot>();
				CompanionAdSlot companionAdSlot = sdkFactory.createCompanionAdSlot();
				ViewGroup adFrame = (ViewGroup) findViewById(R.id.ad_frame);
				companionAdSlot.Container = adFrame;
				companionAdSlot.setSize(adFrame.Width, adFrame.Height);
				companionAdSlots.Add(companionAdSlot);
				container.CompanionSlots = companionAdSlots;

				// Build the list of ad request objects, one per ad, and point each to the ad
				// display container created above.
				List<AdsRequest> adsRequests = new List<AdsRequest>(outerInstance.googleAds.Length);
				foreach (string adURL in outerInstance.googleAds)
				{
					AdsRequest adsRequest = sdkFactory.createAdsRequest();
					adsRequest.AdTagUrl = adURL;
					adsRequest.AdDisplayContainer = container;
					adsRequests.Add(adsRequest);
				}

				// Respond to the event with the new ad requests.
				@event.properties.put(GoogleIMAComponent.ADS_REQUESTS, adsRequests);
				outerInstance.eventEmitter.respond(@event);
			}
		}

		private void setupWidevine()
		{
			// Set up the DRM licensing server to be handled by Brightcove with arbitrary device and
			// portal identifiers to fulfill the Widevine API contract.  These arguments will
			// suffice to create a Widevine plugin instance.
			string drmServerUri = "https://wvlic.brightcove.com/widevine/cypherpc/cgi-bin/GetEMMs.cgi";
			string deviceId = "device1234";
			string portalId = "brightcove";
			new WidevinePlugin(this, brightcoveVideoView, drmServerUri, deviceId, portalId);
		}
	}

}