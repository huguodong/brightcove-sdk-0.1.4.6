package mono.com.brightcove.player.captioning.tasks;


public class LoadCaptionsTask_ResponseStreamListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.brightcove.player.captioning.tasks.LoadCaptionsTask.ResponseStreamListener
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onStreamReady:(Ljava/io/InputStream;)V:GetOnStreamReady_Ljava_io_InputStream_Handler:BrightcoveSDK.Player.Captioning.Tasks.LoadCaptionsTask/IResponseStreamListenerInvoker, Brightcove.Bindings.XamarinAndroid\n" +
			"";
		mono.android.Runtime.register ("BrightcoveSDK.Player.Captioning.Tasks.LoadCaptionsTask+IResponseStreamListenerImplementor, Brightcove.Bindings.XamarinAndroid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", LoadCaptionsTask_ResponseStreamListenerImplementor.class, __md_methods);
	}


	public LoadCaptionsTask_ResponseStreamListenerImplementor () throws java.lang.Throwable
	{
		super ();
		if (getClass () == LoadCaptionsTask_ResponseStreamListenerImplementor.class)
			mono.android.TypeManager.Activate ("BrightcoveSDK.Player.Captioning.Tasks.LoadCaptionsTask+IResponseStreamListenerImplementor, Brightcove.Bindings.XamarinAndroid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onStreamReady (java.io.InputStream p0)
	{
		n_onStreamReady (p0);
	}

	private native void n_onStreamReady (java.io.InputStream p0);

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
