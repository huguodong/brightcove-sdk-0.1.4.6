namespace com.brightcove.player.samples.onceux.basic
{

	using Context = android.content.Context;
	using Bundle = android.os.Bundle;
	using Log = android.util.Log;
	using View = android.view.View;
	using ViewGroup = android.view.ViewGroup;

	using EventEmitter = com.brightcove.player.@event.EventEmitter;
	using EventListener = com.brightcove.player.@event.EventListener;
	using EventType = com.brightcove.player.@event.EventType;
	using Event = com.brightcove.player.@event.Event;

	using BrightcovePlayer = com.brightcove.player.view.BrightcovePlayer;
	using BrightcoveVideoView = com.brightcove.player.view.BrightcoveVideoView;

	using OnceUxComponent = com.brightcove.onceux.OnceUxComponent;
	using OnceUxEventType = com.brightcove.onceux.@event.OnceUxEventType;

	/// <summary>
	/// This app illustrates how to use the Once UX plugin to ensure that:
	/// 
	/// - player controls are hidden during ad playback,
	/// 
	/// - tracking beacons are fired from the client side,
	/// 
	/// - videos are clickable during ad playback and visit the appropriate website,
	/// 
	/// - the companion banner is shown on page switched appropriately as new ads are played
	/// 
	/// @author Paul Michael Reilly
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


		// Private class constants

		private string TAG;

		// Private instance variables

		// The OnceUX plugin VMAP data URL, which tells the plugin when to
		// send tracking beacons, when to hide the player controls and
		// what the click through URL for the ads shoud be.  The VMAP data
		// will also identify what the companion ad should be and what
		// it's click through URL is.
		private string onceUxAdDataUrl = "http://onceux.unicornmedia.com/now/ads/vmap/od/auto/95ea75e1-dd2a-4aea-851a-28f46f8e8195/43f54cc0-aa6b-4b2c-b4de-63d707167bf9/9b118b95-38df-4b99-bb50-8f53d62f6ef8??umtp=0";

		private OnceUxComponent plugin;
		public virtual OnceUxComponent OnceUxPlugin
		{
			get
			{
				return plugin;
			}
		}

		protected internal override void onCreate(Bundle savedInstanceState)
		{
			// When extending the BrightcovePlayer, we must assign the BrightcoveVideoView before
			// entering the superclass.  This allows for some stock video player lifecycle
			// management.
			ContentView = R.layout.onceux_activity_main;
			brightcoveVideoView = (BrightcoveVideoView) findViewById(R.id.brightcove_video_view);
			base.onCreate(savedInstanceState);

			// Setup the event handlers for the OnceUX plugin, set the companion ad container,
			// register the VMAP data URL inside the plugin and start the video.  The plugin will
			// detect that the video has been started and pause it until the ad data is ready or an
			// error condition is detected.  On either event the plugin will continue playing the
			// video.
			registerEventHandlers();
			plugin = new OnceUxComponent(this, brightcoveVideoView);
			View view = findViewById(R.id.ad_frame);
			if (view != null && view is ViewGroup)
			{
				plugin.addCompanionContainer((ViewGroup) view);
			}
			plugin.processVideo(onceUxAdDataUrl);
		}

		// Private instance methods

		/// <summary>
		/// Procedural abstraction used to setup event handlers for the OnceUX plugin.
		/// </summary>
		private void registerEventHandlers()
		{
			// Handle the case where the ad data URL has not been supplied to the plugin.
			EventEmitter eventEmitter = brightcoveVideoView.EventEmitter;
			eventEmitter.on(OnceUxEventType.NO_AD_DATA_URL, new EventListenerAnonymousInnerClassHelper(this));
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
				// Log the event and display a warning message (later)
				Log.e(outerInstance.TAG, @event.Type);
				// TODO: throw up a stock Android warning widget.
			}
		}
	}

}