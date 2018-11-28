package md5f5fc32c321d1313fcd33300b855b9111;


public class Adapter
	extends android.widget.BaseAdapter
	implements
		mono.android.IGCUserPeer,
		android.widget.SectionIndexer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_getItemId:(I)J:GetGetItemId_IHandler\n" +
			"n_getCount:()I:GetGetCountHandler\n" +
			"n_getItem:(I)Ljava/lang/Object;:GetGetItem_IHandler\n" +
			"n_getView:(ILandroid/view/View;Landroid/view/ViewGroup;)Landroid/view/View;:GetGetView_ILandroid_view_View_Landroid_view_ViewGroup_Handler\n" +
			"n_getPositionForSection:(I)I:GetGetPositionForSection_IHandler:Android.Widget.ISectionIndexerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"n_getSectionForPosition:(I)I:GetGetSectionForPosition_IHandler:Android.Widget.ISectionIndexerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"n_getSections:()[Ljava/lang/Object;:GetGetSectionsHandler:Android.Widget.ISectionIndexerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("TidePredictor.Adapter, TidePredictor", Adapter.class, __md_methods);
	}


	public Adapter ()
	{
		super ();
		if (getClass () == Adapter.class)
			mono.android.TypeManager.Activate ("TidePredictor.Adapter, TidePredictor", "", this, new java.lang.Object[] {  });
	}


	public long getItemId (int p0)
	{
		return n_getItemId (p0);
	}

	private native long n_getItemId (int p0);


	public int getCount ()
	{
		return n_getCount ();
	}

	private native int n_getCount ();


	public java.lang.Object getItem (int p0)
	{
		return n_getItem (p0);
	}

	private native java.lang.Object n_getItem (int p0);


	public android.view.View getView (int p0, android.view.View p1, android.view.ViewGroup p2)
	{
		return n_getView (p0, p1, p2);
	}

	private native android.view.View n_getView (int p0, android.view.View p1, android.view.ViewGroup p2);


	public int getPositionForSection (int p0)
	{
		return n_getPositionForSection (p0);
	}

	private native int n_getPositionForSection (int p0);


	public int getSectionForPosition (int p0)
	{
		return n_getSectionForPosition (p0);
	}

	private native int n_getSectionForPosition (int p0);


	public java.lang.Object[] getSections ()
	{
		return n_getSections ();
	}

	private native java.lang.Object[] n_getSections ();

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
