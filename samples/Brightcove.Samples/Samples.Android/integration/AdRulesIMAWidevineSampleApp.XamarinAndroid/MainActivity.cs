using System.Collections.Generic;

namespace com.brightcove.player.samples.imawidevine.adrules
{

	using Bundle = android.os.Bundle;
	using Log = android.util.Log;
	using WidevinePlugin = com.brightcove.drm.widevine.WidevinePlugin;
	using GoogleIMAComponent = com.brightcove.ima.GoogleIMAComponent;
	using GoogleIMAEventType = com.brightcove.ima.GoogleIMAEventType;
	using GoogleIMAVideoAdPlayer = com.brightcove.ima.GoogleIMAVideoAdPlayer;
	using Event = com.brightcove.player.@event.Event;
	using EventEmitter = com.brightcove.player.@event.EventEmitter;
	using EventListener = com.brightcove.player.@event.EventListener;
	using EventType = com.brightcove.player.@event.EventType;
	using Catalog = com.brightcove.player.media.Catalog;
	using VideoFields = com.brightcove.player.media.VideoFields;
	using VideoListener = com.brightcove.player.media.VideoListener;
	using Video = com.brightcove.player.model.Video;
	using BrightcovePlayer = com.brightcove.player.view.BrightcovePlayer;
	using BrightcoveVideoView = com.brightcove.player.view.BrightcoveVideoView;
	using StringUtil = com.brightcove.player.util.StringUtil;
	using AdDisplayContainer = com.google.ads.interactivemedia.v3.api.AdDisplayContainer;
	using AdsRequest = com.google.ads.interactivemedia.v3.api.AdsRequest;
	using ImaSdkFactory = com.google.ads.interactivemedia.v3.api.ImaSdkFactory;

	/// <summary>
	/// This app illustrates how to use "Ad Rules" with the Google IMA
	/// plugin, the Widevine plugin, and the Brightcove Player for Android.
	/// Note: cue points are not used with Ad Rules.
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

		private EventEmitter eventEmitter;
		private GoogleIMAComponent googleIMAComponent;
		private string adRulesURL = "http://pubads.g.doubleclick.net/gampad/ads?sz=640x480&iu=%2F15018773%2Feverything2&ciu_szs=300x250%2C468x60%2C728x90&impl=s&gdfp_req=1&env=vp&output=xml_vast2&unviewed_position_start=1&url=dummy&correlator=[timestamp]&cmsid=133&vid=10XWSh7W4so&ad_rule=1";

		protected internal override void onCreate(Bundle savedInstanceState)
		{
			// When extending the BrightcovePlayer, we must assign the BrightcoveVideoView before
			// entering the superclass. This allows for some stock video player lifecycle
			// management.
			ContentView = R.layout.activity_main;
			brightcoveVideoView = (BrightcoveVideoView) findViewById(R.id.brightcove_video_view);
			base.onCreate(savedInstanceState);
			eventEmitter = brightcoveVideoView.EventEmitter;

			// Use a procedural abstraction to setup the Google IMA SDK via the plugin.
			setupGoogleIMA();

			// Initialize the widevine plugin.
			setupWidevine();

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

			public override void onVideo(Video video)
			{
				brightcoveVideoView.add(video);

				// Auto play: the GoogleIMAComponent will postpone
				// playback until the Ad Rules are loaded.
				brightcoveVideoView.start();
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
			eventEmitter.on(GoogleIMAEventType.DID_START_AD, new EventListenerAnonymousInnerClassHelper(this));

			// Enable logging any failed attempts to play an ad.
			eventEmitter.on(GoogleIMAEventType.DID_FAIL_TO_PLAY_AD, new EventListenerAnonymousInnerClassHelper2(this));

			// Enable Logging upon ad completion.
			eventEmitter.on(GoogleIMAEventType.DID_COMPLETE_AD, new EventListenerAnonymousInnerClassHelper3(this));

			// Set up a listener for initializing AdsRequests. The Google
			// IMA plugin emits an ad request event as a result of
			// initializeAdsRequests() being called.
			eventEmitter.on(GoogleIMAEventType.ADS_REQUEST_FOR_VIDEO, new EventListenerAnonymousInnerClassHelper4(this, sdkFactory));

			// Create the Brightcove IMA Plugin and pass in the event
			// emitter so that the plugin can integrate with the SDK.
			googleIMAComponent = new GoogleIMAComponent(brightcoveVideoView, eventEmitter, true);

			// Calling GoogleIMAComponent.initializeAdsRequests() is no longer necessary.
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
				Log.v(outerInstance.TAG, @event.Type);
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

			private ImaSdkFactory sdkFactory;

			public EventListenerAnonymousInnerClassHelper4(MainActivity outerInstance, ImaSdkFactory sdkFactory)
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

				// Build an ads request object and point it to the ad
				// display container created above.
				AdsRequest adsRequest = sdkFactory.createAdsRequest();
				adsRequest.AdTagUrl = outerInstance.adRulesURL;
				adsRequest.AdDisplayContainer = container;

				List<AdsRequest> adsRequests = new List<AdsRequest>(1);
				adsRequests.Add(adsRequest);

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