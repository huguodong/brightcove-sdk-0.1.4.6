package mono.com.brightcove.player.captioning.preferences;


public class ListDialogPreference_OnValueChangedListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.brightcove.player.captioning.preferences.ListDialogPreference.OnValueChangedListener
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onValueChanged:(Lcom/brightcove/player/captioning/preferences/ListDialogPreference;I)V:GetOnValueChanged_Lcom_brightcove_player_captioning_preferences_ListDialogPreference_IHandler:BrightcoveSDK.Player.Captioning.Preferences.ListDialogPreference/IOnValueChangedListenerInvoker, Brightcove.Bindings.XamarinAndroid\n" +
			"";
		mono.android.Runtime.register ("BrightcoveSDK.Player.Captioning.Preferences.ListDialogPreference+IOnValueChangedListenerImplementor, Brightcove.Bindings.XamarinAndroid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", ListDialogPreference_OnValueChangedListenerImplementor.class, __md_methods);
	}


	public ListDialogPreference_OnValueChangedListenerImplementor () throws java.lang.Throwable
	{
		super ();
		if (getClass () == ListDialogPreference_OnValueChangedListenerImplementor.class)
			mono.android.TypeManager.Activate ("BrightcoveSDK.Player.Captioning.Preferences.ListDialogPreference+IOnValueChangedListenerImplementor, Brightcove.Bindings.XamarinAndroid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onValueChanged (com.brightcove.player.captioning.preferences.ListDialogPreference p0, int p1)
	{
		n_onValueChanged (p0, p1);
	}

	private native void n_onValueChanged (com.brightcove.player.captioning.preferences.ListDialogPreference p0, int p1);

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
