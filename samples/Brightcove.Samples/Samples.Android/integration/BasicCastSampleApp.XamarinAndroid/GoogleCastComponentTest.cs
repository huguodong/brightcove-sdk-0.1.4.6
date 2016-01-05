using System.Collections.Generic;

namespace com.brightcove.player.samples.cast.basic.test
{

	using ActivityInstrumentationTestCase2 = android.test.ActivityInstrumentationTestCase2;
	using Log = android.util.Log;
	using View = android.view.View;

	using Event = com.brightcove.player.@event.Event;
	using EventEmitter = com.brightcove.player.@event.EventEmitter;
	using EventListener = com.brightcove.player.@event.EventListener;
	using EventType = com.brightcove.player.@event.EventType;
	using BrightcoveVideoView = com.brightcove.player.view.BrightcoveVideoView;
	using VideoCastControllerActivity = com.google.sample.castcompanionlibrary.cast.player.VideoCastControllerActivity;
	using Solo = com.robotium.solo.Solo;


	/// <summary>
	/// Integration tests for the expected behavior of the Chromecast using the Cast
	/// plugin for the Brightcove Native Player SDK for Android.
	/// @author Billy Hnath (bhnath@brightcove.com)
	/// </summary>
	public class GoogleCastComponentTest : ActivityInstrumentationTestCase2<MainActivity>
	{
		private bool InstanceFieldsInitialized = false;

		private void InitializeInstanceFields()
		{
			TAG = this.GetType().Name;
		}


		private string TAG;

		private MainActivity castActivity;
		private BrightcoveVideoView brightcoveVideoView;
		private EventEmitter eventEmitter;
		private Solo solo;

		public GoogleCastComponentTest() : base(typeof(MainActivity))
		{
			if (!InstanceFieldsInitialized)
			{
				InitializeInstanceFields();
				InstanceFieldsInitialized = true;
			}
		}

		/// <summary>
		/// Initialize Robotium, the BrightcoveVideoView and the EventEmitter.
		/// Start media playback. </summary>
		/// <exception cref="Exception"> </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: @Override protected void setUp() throws Exception
		protected internal override void setUp()
		{
			base.setUp();
			ActivityInitialTouchMode = true;
			castActivity = Activity;

			solo = new Solo(Instrumentation, castActivity);
			brightcoveVideoView = (BrightcoveVideoView) castActivity.findViewById(R.id.brightcove_video_view);
			eventEmitter = brightcoveVideoView.EventEmitter;

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.concurrent.CountDownLatch countDownLatch = new java.util.concurrent.CountDownLatch(2);
			CountDownLatch countDownLatch = new CountDownLatch(2);
			eventEmitter.once(EventType.DID_SET_VIDEO, new EventListenerAnonymousInnerClassHelper(this, countDownLatch));

			eventEmitter.once(EventType.DID_PLAY, new EventListenerAnonymousInnerClassHelper2(this, countDownLatch));

			assertTrue("Timeout occurred.", countDownLatch.@await(1, TimeUnit.MINUTES));
		}

		private class EventListenerAnonymousInnerClassHelper : EventListener
		{
			private readonly GoogleCastComponentTest outerInstance;

			private CountDownLatch countDownLatch;

			public EventListenerAnonymousInnerClassHelper(GoogleCastComponentTest outerInstance, CountDownLatch countDownLatch)
			{
				this.outerInstance = outerInstance;
				this.countDownLatch = countDownLatch;
			}

			public override void processEvent(Event @event)
			{
				outerInstance.brightcoveVideoView.start();
				countDownLatch.countDown();
			}
		}

		private class EventListenerAnonymousInnerClassHelper2 : EventListener
		{
			private readonly GoogleCastComponentTest outerInstance;

			private CountDownLatch countDownLatch;

			public EventListenerAnonymousInnerClassHelper2(GoogleCastComponentTest outerInstance, CountDownLatch countDownLatch)
			{
				this.outerInstance = outerInstance;
				this.countDownLatch = countDownLatch;
			}

			public override void processEvent(Event @event)
			{
				assertTrue("BrightcoveVideoView is playing.", outerInstance.brightcoveVideoView.Playing);
				countDownLatch.countDown();
			}
		}

		/// <summary>
		/// Test the successful local to remote playback and then remote to local
		/// with the media starting in the PLAY state on the local device. </summary>
		/// <exception cref="InterruptedException"> </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void testCastMediaToRemoteDeviceAndBackFromPlaying() throws InterruptedException
		public virtual void testCastMediaToRemoteDeviceAndBackFromPlaying()
		{
			Log.v(TAG, "testCastMediaToRemoteDeviceAndBackFromPlaying");
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.concurrent.CountDownLatch countDownLatch = new java.util.concurrent.CountDownLatch(2);
			CountDownLatch countDownLatch = new CountDownLatch(2);

			solo.clickOnActionBarItem(R.id.media_router_menu_item);
			solo.clickInList(0);

			eventEmitter.once(EventType.DID_PAUSE, new EventListenerAnonymousInnerClassHelper3(this, countDownLatch));
			solo.waitForActivity(typeof(VideoCastControllerActivity));
			solo.sleep(10000);

			solo.assertCurrentActivity("VideoCastControllerActivity is displayed", typeof(VideoCastControllerActivity));

			IList<View> views = solo.CurrentViews;
			foreach (View v in views)
			{
				if (v is android.support.v7.app.MediaRouteButton)
				{
					solo.clickOnView(v);
				}
			}
			solo.sleep(3000);

			solo.clickOnButton("Disconnect");
			solo.sleep(3000);

			countDownLatch.countDown();

			assertTrue("Timeout occurred.", countDownLatch.@await(3, TimeUnit.MINUTES));
		}

		private class EventListenerAnonymousInnerClassHelper3 : EventListener
		{
			private readonly GoogleCastComponentTest outerInstance;

			private CountDownLatch countDownLatch;

			public EventListenerAnonymousInnerClassHelper3(GoogleCastComponentTest outerInstance, CountDownLatch countDownLatch)
			{
				this.outerInstance = outerInstance;
				this.countDownLatch = countDownLatch;
			}

			public override void processEvent(Event @event)
			{
				assertFalse("BrightVideoVideo is paused.", outerInstance.brightcoveVideoView.Playing);
				countDownLatch.countDown();
			}
		}

		/// <summary>
		/// Test the successful local to remote playback and then remote to local
		/// with the media starting in the PAUSE state on the local device.
		/// </summary>
		/// <exception cref="InterruptedException"> </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void testCastMediaToRemoteDeviceAndBackFromPaused() throws InterruptedException
		public virtual void testCastMediaToRemoteDeviceAndBackFromPaused()
		{
			Log.v(TAG, "testCastMediaToRemoteDeviceAndBackFromPaused");
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.concurrent.CountDownLatch countDownLatch = new java.util.concurrent.CountDownLatch(2);
			CountDownLatch countDownLatch = new CountDownLatch(2);

			brightcoveVideoView.pause();
			solo.sleep(3000);

			eventEmitter.once(EventType.DID_STOP, new EventListenerAnonymousInnerClassHelper4(this, countDownLatch));

			solo.clickOnActionBarItem(R.id.media_router_menu_item);
			solo.clickInList(0);
			solo.sleep(6000);

			brightcoveVideoView.start();
			assertFalse("BrightcoveVideoView playback did not start.", brightcoveVideoView.Playing);

			solo.waitForActivity(typeof(VideoCastControllerActivity));
			solo.sleep(10000);

			solo.assertCurrentActivity("VideoCastControllerActivity is displayed", typeof(VideoCastControllerActivity));

			IList<View> views = solo.CurrentViews;
			foreach (View v in views)
			{
				if (v is android.support.v7.app.MediaRouteButton)
				{
					solo.clickOnView(v);
				}
			}
			solo.sleep(3000);

			solo.clickOnButton("Disconnect");
			solo.sleep(3000);

			countDownLatch.countDown();

			assertTrue("Timeout occurred.", countDownLatch.@await(3, TimeUnit.MINUTES));
		}

		private class EventListenerAnonymousInnerClassHelper4 : EventListener
		{
			private readonly GoogleCastComponentTest outerInstance;

			private CountDownLatch countDownLatch;

			public EventListenerAnonymousInnerClassHelper4(GoogleCastComponentTest outerInstance, CountDownLatch countDownLatch)
			{
				this.outerInstance = outerInstance;
				this.countDownLatch = countDownLatch;
			}

			public override void processEvent(Event @event)
			{
				assertFalse("BrightVideoVideo is not playing.", outerInstance.brightcoveVideoView.Playing);
				countDownLatch.countDown();
			}
		}

		/// <summary>
		/// Clean up after each test. </summary>
		/// <exception cref="Exception"> </exception>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: @Override public void tearDown() throws Exception
		public override void tearDown()
		{
			//solo.finishOpenedActivities();
			base.tearDown();
		}
	}

}