package md570c78e1fe63be0ff7e22e054333ab2d3;


public class SeparatorRenderer
	extends md5b60ffeb829f638581ab2bb9b1a7f4f3f.ViewRenderer_2
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("XLabs.Forms.Controls.SeparatorRenderer, XLabs.Forms.Droid, Version=2.0.5974.23358, Culture=neutral, PublicKeyToken=null", SeparatorRenderer.class, __md_methods);
	}


	public SeparatorRenderer (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == SeparatorRenderer.class)
			mono.android.TypeManager.Activate ("XLabs.Forms.Controls.SeparatorRenderer, XLabs.Forms.Droid, Version=2.0.5974.23358, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Util.IAttributeSet, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public SeparatorRenderer (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == SeparatorRenderer.class)
			mono.android.TypeManager.Activate ("XLabs.Forms.Controls.SeparatorRenderer, XLabs.Forms.Droid, Version=2.0.5974.23358, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Util.IAttributeSet, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0, p1 });
	}


	public SeparatorRenderer (android.content.Context p0)
	{
		super (p0);
		if (getClass () == SeparatorRenderer.class)
			mono.android.TypeManager.Activate ("XLabs.Forms.Controls.SeparatorRenderer, XLabs.Forms.Droid, Version=2.0.5974.23358, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0 });
	}

	private java.util.ArrayList refList;
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
