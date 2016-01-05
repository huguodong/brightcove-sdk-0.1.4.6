package md51430c58e17cae80d0d18a8a89f933d5a;


public class ActivityPlayerSimpleWithControls
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("QuickStart.Player.XamarinAndroid.ActivityPlayerSimpleWithControls, QuickStart.Player.XamarinAndroid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", ActivityPlayerSimpleWithControls.class, __md_methods);
	}


	public ActivityPlayerSimpleWithControls () throws java.lang.Throwable
	{
		super ();
		if (getClass () == ActivityPlayerSimpleWithControls.class)
			mono.android.TypeManager.Activate ("QuickStart.Player.XamarinAndroid.ActivityPlayerSimpleWithControls, QuickStart.Player.XamarinAndroid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

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
