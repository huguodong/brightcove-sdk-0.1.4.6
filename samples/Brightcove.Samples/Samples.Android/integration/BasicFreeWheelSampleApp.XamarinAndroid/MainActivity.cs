using System;
using System.Collections.Generic;

namespace com.brightcove.player.samples.freewheel.basic
{

	using ActionBar = android.app.ActionBar;
	using Activity = android.app.Activity;
	using Bundle = android.os.Bundle;
	using ContactsContract = android.provider.ContactsContract;
	using Log = android.util.Log;
	using LayoutInflater = android.view.LayoutInflater;
	using Menu = android.view.Menu;
	using MenuItem = android.view.MenuItem;
	using View = android.view.View;
	using ViewGroup = android.view.ViewGroup;
	using Build = android.os.Build;
	using WindowManager = android.view.WindowManager;
	using FrameLayout = android.widget.FrameLayout;
	using MediaController = android.widget.MediaController;
	using RelativeLayout = android.widget.RelativeLayout;

	using FreeWheelController = com.brightcove.freewheel.controller.FreeWheelController;
	using FreeWheelEventType = com.brightcove.freewheel.@event.FreeWheelEventType;
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

	using IAdContext = tv.freewheel.ad.interfaces.IAdContext;
	using IConstants = tv.freewheel.ad.interfaces.IConstants;
	using ISlot = tv.freewheel.ad.interfaces.ISlot;

	/// <summary>
	/// This app illustrates how to use the FreeWheel plugin with the Brightcove Player for Android.
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
		private FreeWheelController freeWheelController;
		private FrameLayout adFrame;

		protected internal override void onCreate(Bundle savedInstanceState)
		{
			// When extending the BrightcovePlayer, we must assign the BrightcoveVideoView
			// before entering the superclass. This allows for some stock video player lifecycle
			// management.
			ContentView = R.layout.freewheel_activity_main;
			brightcoveVideoView = (BrightcoveVideoView) findViewById(R.id.brightcove_video_view);
			base.onCreate(savedInstanceState);

			adFrame = (FrameLayout) findViewById(R.id.ad_frame);
			eventEmitter = brightcoveVideoView.EventEmitter;

			setupFreeWheel();

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

		private void setupFreeWheel()
		{

			//change this to new FrameLayout based constructor.
			freeWheelController = new FreeWheelController(this, brightcoveVideoView, eventEmitter);
			//configure your own IAdManager or supply connection information
			freeWheelController.AdURL = "http://demo.v.fwmrm.net/";
			freeWheelController.AdNetworkId = 90750;
			freeWheelController.Profile = "3pqa_android";

			/*
			 * Choose one of these to determine the ad policy (basically server or client).
			 * - 3pqa_section - uses FW server rules - always returns a preroll and a postroll.  It should return whatever midroll slots you request though.
			 * - 3pqa_section_nocbp - returns the slots that you request.
			 */
			//freeWheelController.setSiteSectionId("3pqa_section");
			freeWheelController.SiteSectionId = "3pqa_section_nocbp";

			eventEmitter.on(FreeWheelEventType.SHOW_DISPLAY_ADS, new EventListenerAnonymousInnerClassHelper(this));

			eventEmitter.on(FreeWheelEventType.WILL_SUBMIT_AD_REQUEST, new EventListenerAnonymousInnerClassHelper2(this));
			freeWheelController.enable();
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
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") java.util.List<tv.freewheel.ad.interfaces.ISlot> slots = (java.util.List<tv.freewheel.ad.interfaces.ISlot>) event.properties.get(com.brightcove.freewheel.controller.FreeWheelController.AD_SLOTS_KEY);
				IList<ISlot> slots = (IList<ISlot>) @event.properties.get(FreeWheelController.AD_SLOTS_KEY);
				ViewGroup adView = (ViewGroup) findViewById(R.id.ad_frame);

				// Clean out any previous display ads
				for (int i = 0; i < adView.ChildCount; i++)
				{
					adView.removeViewAt(i);
				}

				foreach (ISlot slot in slots)
				{
					adView.addView(slot.Base);
					slot.play();
				}
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
				Video video = (Video) @event.properties.get(Event.VIDEO);
				IAdContext adContext = (IAdContext) @event.properties.get(FreeWheelController.AD_CONTEXT_KEY);
				IConstants adConstants = adContext.Constants;

				// This overrides what the plugin does by default for setVideoAsset() which is to pass in currentVideo.getId().
				adContext.setVideoAsset("3pqa_video", video.Duration / 1000, null, adConstants.VIDEO_ASSET_AUTO_PLAY_TYPE_ATTENDED(), (int)Math.Floor(new Random(1).NextDouble() * int.MaxValue), 0, adConstants.ID_TYPE_CUSTOM(), 0, adConstants.VIDEO_ASSET_DURATION_TYPE_EXACT()); // duration type -  fallback ID -  type of video ID passed (customer created or FW issued) -  setting networkId for 0 as it's the default value for this method -  a random number -  auto play type -  location -  FW uses their duration as seconds; Android is in milliseconds -  video ID

				adContext.addSiteSectionNonTemporalSlot("300x250slot", null, 300, 250, null, true, null, null);

				// Add preroll
				Log.v(outerInstance.TAG, "Adding temporal slot for prerolls");
				adContext.addTemporalSlot("larry", "PREROLL", 0, null, 0, 0, null, null, 0);

				// Add midroll
				Log.v(outerInstance.TAG, "Adding temporal slot for midrolls");

				int midrollCount = 1;
				int segmentLength = (video.Duration / 1000) / (midrollCount + 1);

				for (int i = 0; i < midrollCount; i++)
				{
					adContext.addTemporalSlot("moe" + i, "MIDROLL", segmentLength * (i + 1), null, 0, 0, null, null, 0);
				}

				// Add postroll
				Log.v(outerInstance.TAG, "Adding temporal slot for postrolls");
				adContext.addTemporalSlot("curly", "POSTROLL", video.Duration / 1000, null, 0, 0, null, null, 0);

				// Add overlay
				Log.v(outerInstance.TAG, "Adding temporal slot for overlays");
				adContext.addTemporalSlot("shemp", "OVERLAY", 8, null, 0, 0, null, null, 0);
			}
		}
	}

}