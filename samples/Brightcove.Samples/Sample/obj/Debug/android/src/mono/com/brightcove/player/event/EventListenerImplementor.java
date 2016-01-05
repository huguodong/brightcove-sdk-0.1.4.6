package mono.com.brightcove.player.event;


public class EventListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.brightcove.player.event.EventListener
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_processEvent:(Lcom/brightcove/player/event/Event;)V:GetProcessEvent_Lcom_brightcove_player_event_Event_Handler:BrightcoveSDK.Player.Events.IEventListenerInvoker, Brightcove.Bindings.XamarinAndroid\n" +
			"";
		mono.android.Runtime.register ("BrightcoveSDK.Player.Events.IEventListenerImplementor, Brightcove.Bindings.XamarinAndroid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", EventListenerImplementor.class, __md_methods);
	}


	public EventListenerImplementor () throws java.lang.Throwable
	{
		super ();
		if (getClass () == EventListenerImplementor.class)
			mono.android.TypeManager.Activate ("BrightcoveSDK.Player.Events.IEventListenerImplementor, Brightcove.Bindings.XamarinAndroid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void processEvent (com.brightcove.player.event.Event p0)
	{
		n_processEvent (p0);
	}

	private native void n_processEvent (com.brightcove.player.event.Event p0);

	java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
