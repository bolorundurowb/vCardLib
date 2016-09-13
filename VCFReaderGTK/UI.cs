using System;
using Gtk;
using vCardLib;
using System.IO;
using VCFReaderGTK;

public partial class UI: Gtk.Window
{
	private string dbPath = Environment.GetFolderPath (Environment.SpecialFolder.LocalApplicationData) + 
		System.IO.Path.DirectorySeparatorChar + "VCFReader" + System.IO.Path.DirectorySeparatorChar + "userstore.xml";
	private NodeStore store;
	private NodeStore Store
	{
		get {
			if (store == null) {
				store = new NodeStore (typeof(Node));
			}
			return store;
		}
	}

	public UI () : base (Gtk.WindowType.Toplevel)
	{
		Build ();
		if (File.Exists (dbPath)) {

		}
		dgv_contacts.NodeStore = Store;
		//
		dgv_contacts.AppendColumn ("Full Name", new Gtk.CellRendererText (), "text", 0).Resizable = true;
		dgv_contacts.AppendColumn ("Email Address", new Gtk.CellRendererText (), "text", 1).Resizable = true;
		dgv_contacts.AppendColumn ("Phone Number 1", new Gtk.CellRendererText (), "text", 2).Resizable = true;
		dgv_contacts.AppendColumn ("phone Number 2", new Gtk.CellRendererText (), "text", 3).Resizable = true;
		dgv_contacts.ShowAll ();
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected void OnOfdSelectVcardSelectionChanged (object sender, EventArgs e)
	{
		if (File.Exists (ofd_select_vcard.Filename) && (System.IO.Path.GetExtension (ofd_select_vcard.Filename) == ".vcf" ||
			System.IO.Path.GetExtension (ofd_select_vcard.Filename) == ".vcard")) {
			vCardCollection collection = vCard.FromFile (ofd_select_vcard.Filename);
			foreach (vCard vcard in collection) {
				Node node = new Node ();
				node.FullName = vcard.FormattedName;
				node.EmailAddress = vcard.EmailAddresses.Count > 0 ? vcard.EmailAddresses [0].Email.Address : "";
				node.PhoneNumber1 = vcard.PhoneNumbers.Count > 0 ? vcard.PhoneNumbers [0].Number : "";
				node.PhoneNumber2 = vcard.PhoneNumbers.Count > 1 ? vcard.PhoneNumbers [1].Number : "";

				Store.AddNode (node);
			}
		}
		else
		{
			MessageDialog md = new MessageDialog (this, DialogFlags.DestroyWithParent, MessageType.Error,
				                   ButtonsType.Close, "The selected file either doesnt exist, or is of an incorrect format");
			md.Run ();
			md.Destroy ();
		}
	}
}
