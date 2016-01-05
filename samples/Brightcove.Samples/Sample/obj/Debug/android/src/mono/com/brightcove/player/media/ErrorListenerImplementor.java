package mono.com.brightcove.player.media;


public class ErrorListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.brightcove.player.media.ErrorListener
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onError:(Ljava/lang/String;)V:GetOnError_Ljava_lang_String_Handler:BrightcoveSDK.Player.Media.IErrorListenerInvoker, Brightcove.Bindings.XamarinAndroid\n" +
			"";
		mono.android.Runtime.register ("BrightcoveSDK.Player.Media.IErrorListenerImplementor, Brightcove.Bindings.XamarinAndroid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", ErrorListenerImplementor.class, __md_methods);
	}


	public ErrorListenerImplementor () throws java.lang.Throwable
	{
		super ();
		if (getClass () == ErrorListenerImplementor.class)
			mono.android.TypeManager.Activate ("BrightcoveSDK.Player.Media.IErrorListenerImplementor, Brightcove.Bindings.XamarinAndroid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onError (java.lang.String p0)
	{
		n_onError (p0);
	}

	private native void n_onError (java.lang.String p0);

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
