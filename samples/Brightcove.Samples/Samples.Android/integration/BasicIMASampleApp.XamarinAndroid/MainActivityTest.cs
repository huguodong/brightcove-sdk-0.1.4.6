using System;
using System.Threading;

namespace com.brightcove.player.samples.ima.basic.test
{


	using Instrumentation = android.app.Instrumentation;
	using ActivityInstrumentationTestCase2 = android.test.ActivityInstrumentationTestCase2;
	using Log = android.util.Log;

	using GoogleIMAEventType = com.brightcove.ima.GoogleIMAEventType;
	using Event = com.brightcove.player.@event.Event;
	using EventEmitter = com.brightcove.player.@event.EventEmitter;
	using EventListener = com.brightcove.player.@event.EventListener;
	using EventType = com.brightcove.player.@event.EventType;
	using BrightcoveVideoView = com.brightcove.player.view.BrightcoveVideoView;

	/// <summary>
	/// White box style tests for the Google IMA plugin sample app.
	/// </summary>
	public class MainActivityTest : ActivityInstrumentationTestCase2<MainActivity>
	{
		private bool InstanceFieldsInitialized = false;

		private void InitializeInstanceFields()
		{
			TAG = this.GetType().Name;
		}


		private string TAG;

		private enum State
		{
			STARTED_AD,
			COMPLETED_AD,
			STARTING_CONTENT,
			STARTED_CONTENT,
			COMPLETED_CONTENT
		}

		private State state;
		private BrightcoveVideoView brightcoveVideoView;
		private EventEmitter eventEmitter;
		private int numPrerollsPlayed = 0;
		private int numMidrollsPlayed = 0;
		private int numPostrollsPlayed = 0;
		private MainActivity mainActivity;
		private bool hasResumed = false;
		private int playheadPosition = 0;

		public MainActivityTest() : base(typeof(MainActivity))
		{
			if (!InstanceFieldsInitialized)
			{
				InitializeInstanceFields();
				InstanceFieldsInitialized = true;
			}
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: @Override protected void setUp() throws Exception
		protected internal override void setUp()
		{
			base.setUp();
			ActivityInitialTouchMode = false;
			mainActivity = Activity;
			brightcoveVideoView = (BrightcoveVideoView) mainActivity.findViewById(R.id.brightcove_video_view);
			eventEmitter = brightcoveVideoView.EventEmitter;

			eventEmitter.once(EventType.DID_SET_VIDEO, new EventListenerAnonymousInnerClassHelper(this));
		}

		private class EventListenerAnonymousInnerClassHelper : EventListener
		{
			private readonly MainActivityTest outerInstance;

			public EventListenerAnonymousInnerClassHelper(MainActivityTest outerInstance)
			{
				this.outerInstance = outerInstance;
			}

			public override void processEvent(Event @event)
			{
				outerInstance.brightcoveVideoView.start();
			}
		}

		/// <summary>
		/// Test for pre, mid, and post rolls.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void testPlay() throws InterruptedException
		public virtual void testPlay()
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.concurrent.CountDownLatch latch = new java.util.concurrent.CountDownLatch(2);
			CountDownLatch latch = new CountDownLatch(2);
			Log.v(TAG, "testPlay");

			eventEmitter.on(GoogleIMAEventType.DID_START_AD, new EventListenerAnonymousInnerClassHelper2(this));

			eventEmitter.on(GoogleIMAEventType.DID_COMPLETE_AD, new EventListenerAnonymousInnerClassHelper3(this));

			eventEmitter.on(EventType.WILL_CHANGE_VIDEO, new EventListenerAnonymousInnerClassHelper4(this));

			eventEmitter.on(EventType.COMPLETED, new EventListenerAnonymousInnerClassHelper5(this, latch));

			assertTrue("Timeout occurred.", latch.@await(3, TimeUnit.MINUTES));
			brightcoveVideoView.stopPlayback();
			assertEquals("Should have played 2 prerolls.", 2, numPrerollsPlayed);
			assertEquals("Should have played 2 midrolls.", 2, numMidrollsPlayed);
			assertEquals("Should have played 2 postrolls.", 2, numPostrollsPlayed);
		}

		private class EventListenerAnonymousInnerClassHelper2 : EventListener
		{
			private readonly MainActivityTest outerInstance;

			public EventListenerAnonymousInnerClassHelper2(MainActivityTest outerInstance)
			{
				this.outerInstance = outerInstance;
			}

			public override void processEvent(Event @event)
			{
				assertTrue("Should not have started an ad: " + outerInstance.state, outerInstance.state != State.STARTED_AD);

				switch (outerInstance.state)
				{
				case com.brightcove.player.samples.ima.basic.test.MainActivityTest.State.STARTING_CONTENT:
					outerInstance.numPrerollsPlayed++;
					break;
				case com.brightcove.player.samples.ima.basic.test.MainActivityTest.State.STARTED_CONTENT:
					outerInstance.numMidrollsPlayed++;
					break;
				case com.brightcove.player.samples.ima.basic.test.MainActivityTest.State.COMPLETED_CONTENT:
					outerInstance.numPostrollsPlayed++;
					break;
				default:
					Log.e(outerInstance.TAG, "Unexpected state: " + outerInstance.state);
				break;
				}

				outerInstance.state = State.STARTED_AD;
			}
		}

		private class EventListenerAnonymousInnerClassHelper3 : EventListener
		{
			private readonly MainActivityTest outerInstance;

			public EventListenerAnonymousInnerClassHelper3(MainActivityTest outerInstance)
			{
				this.outerInstance = outerInstance;
			}

			public override void processEvent(Event @event)
			{
				assertTrue("Should have started an ad: " + outerInstance.state, outerInstance.state == State.STARTED_AD);
				outerInstance.state = State.COMPLETED_AD;
			}
		}

		private class EventListenerAnonymousInnerClassHelper4 : EventListener
		{
			private readonly MainActivityTest outerInstance;

			public EventListenerAnonymousInnerClassHelper4(MainActivityTest outerInstance)
			{
				this.outerInstance = outerInstance;
			}

			public override void processEvent(Event @event)
			{
				outerInstance.state = State.STARTING_CONTENT;

				if (outerInstance.eventEmitter != null)
				{
					outerInstance.eventEmitter.once(EventType.PROGRESS, new EventListenerAnonymousInnerClassHelper6(this, @event));
				}
			}

			private class EventListenerAnonymousInnerClassHelper6 : EventListener
			{
				private readonly EventListenerAnonymousInnerClassHelper4 outerInstance;

				private Event @event;

				public EventListenerAnonymousInnerClassHelper6(EventListenerAnonymousInnerClassHelper4 outerInstance, Event @event)
				{
					this.outerInstance = outerInstance;
					this.@event = @event;
				}

				public override void processEvent(Event @event)
				{
					assertTrue("Should have played a preroll: " + outerInstance.outerInstance.state, outerInstance.outerInstance.state == State.COMPLETED_AD);
					outerInstance.outerInstance.state = State.STARTED_CONTENT;
				}
			}
		}

		private class EventListenerAnonymousInnerClassHelper5 : EventListener
		{
			private readonly MainActivityTest outerInstance;

			private CountDownLatch latch;

			public EventListenerAnonymousInnerClassHelper5(MainActivityTest outerInstance, CountDownLatch latch)
			{
				this.outerInstance = outerInstance;
				this.latch = latch;
			}

			public override void processEvent(Event @event)
			{
				if (@event.properties.containsKey(Event.SKIP_CUE_POINTS))
				{
					latch.countDown();
				}
				else
				{
					outerInstance.state = State.COMPLETED_CONTENT;
				}
			}
		}

		/// <summary>
		/// Test pausing and resuming during content playback.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void testPlaybackPauseResume() throws InterruptedException
		public virtual void testPlaybackPauseResume()
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.concurrent.CountDownLatch latch = new java.util.concurrent.CountDownLatch(1);
			CountDownLatch latch = new CountDownLatch(1);

			eventEmitter.on(EventType.PROGRESS, new EventListenerAnonymousInnerClassHelper7(this, latch));

			assertTrue("Timeout occurred.", latch.@await(1, TimeUnit.MINUTES));
			assertTrue("Should have resumed.", hasResumed);
			assertTrue("Should not have repeated first five seconds: " + playheadPosition, playheadPosition > 5000);
		}

		private class EventListenerAnonymousInnerClassHelper7 : EventListener
		{
			private readonly MainActivityTest outerInstance;

			private CountDownLatch latch;

			public EventListenerAnonymousInnerClassHelper7(MainActivityTest outerInstance, CountDownLatch latch)
			{
				this.outerInstance = outerInstance;
				this.latch = latch;
			}

			public override void processEvent(Event @event)
			{
				if (!outerInstance.hasResumed)
				{
					if (@event.getIntegerProperty(Event.PLAYHEAD_POSITION) > 5000)
					{
						// Using a new thread, so we aren't sleeping the main thread.
						new ThreadAnonymousInnerClassHelper(this)
						.start();
					}
				}
				else
				{
					outerInstance.playheadPosition = @event.getIntegerProperty(Event.PLAYHEAD_POSITION);
					latch.countDown();
				}
			}

			private class ThreadAnonymousInnerClassHelper : System.Threading.Thread
			{
				private readonly EventListenerAnonymousInnerClassHelper7 outerInstance;

				public ThreadAnonymousInnerClassHelper(EventListenerAnonymousInnerClassHelper7 outerInstance)
				{
					this.outerInstance = outerInstance;
				}

				public virtual void run()
				{
					Instrumentation instrumentation = outerInstance.outerInstance.Instrumentation;
					instrumentation.callActivityOnPause(outerInstance.outerInstance.mainActivity);
					Log.v(outerInstance.outerInstance.TAG, "paused");
					outerInstance.outerInstance.sleep();
					instrumentation.callActivityOnResume(outerInstance.outerInstance.mainActivity);
					outerInstance.outerInstance.hasResumed = true;
					Log.v(outerInstance.outerInstance.TAG, "resumed");
				}
			}
		}

		/// <summary>
		/// Test pausing and resuming during ad playback.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void testAdPauseResume() throws InterruptedException
		public virtual void testAdPauseResume()
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.concurrent.CountDownLatch latch = new java.util.concurrent.CountDownLatch(1);
			CountDownLatch latch = new CountDownLatch(1);

			eventEmitter.on(GoogleIMAEventType.DID_START_AD, new EventListenerAnonymousInnerClassHelper8(this));

			eventEmitter.on(GoogleIMAEventType.DID_COMPLETE_AD, new EventListenerAnonymousInnerClassHelper9(this, latch));

			assertTrue("Timeout occurred.", latch.@await(1, TimeUnit.MINUTES));
			assertTrue("Should have resumed.", hasResumed);
		}

		private class EventListenerAnonymousInnerClassHelper8 : EventListener
		{
			private readonly MainActivityTest outerInstance;

			public EventListenerAnonymousInnerClassHelper8(MainActivityTest outerInstance)
			{
				this.outerInstance = outerInstance;
			}

			public override void processEvent(Event @event)
			{
				Instrumentation instrumentation = outerInstance.Instrumentation;
				instrumentation.callActivityOnPause(outerInstance.mainActivity);
				Log.v(outerInstance.TAG, "paused");
				instrumentation.callActivityOnResume(outerInstance.mainActivity);
				outerInstance.hasResumed = true;
				Log.v(outerInstance.TAG, "resumed");
			}
		}

		private class EventListenerAnonymousInnerClassHelper9 : EventListener
		{
			private readonly MainActivityTest outerInstance;

			private CountDownLatch latch;

			public EventListenerAnonymousInnerClassHelper9(MainActivityTest outerInstance, CountDownLatch latch)
			{
				this.outerInstance = outerInstance;
				this.latch = latch;
			}

			public override void processEvent(Event @event)
			{
				latch.countDown();
			}
		}

		private void sleep()
		{
			try
			{
				Thread.Sleep(5000);
			}
			catch (Exception exception)
			{
				Console.WriteLine(exception.ToString());
				Console.Write(exception.StackTrace);
			}
		}
	}

}