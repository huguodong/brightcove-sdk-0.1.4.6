package md5e44603b235f21ebfbb6b3d0f2dc433d5;


public class ActivityPlayerSimple
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("Sample.ActivityPlayerSimple, Sample, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", ActivityPlayerSimple.class, __md_methods);
	}


	public ActivityPlayerSimple () throws java.lang.Throwable
	{
		super ();
		if (getClass () == ActivityPlayerSimple.class)
			mono.android.TypeManager.Activate ("Sample.ActivityPlayerSimple, Sample, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
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
