using System;
using Gtk;
using vCardLib;
using System.IO;
using vCardLib.Collections;
using vCardLibUI.Models;
using vCardLib.Deserializers;

public partial class UI: Gtk.Window
{
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
		//Bind a text changed event handler for the search box
		txt_search.Buffer.Changed += new EventHandler(txt_search_TextChanged);
		//
		dgv_contacts.NodeStore = Store;
		//
		TreeViewColumn fullNameCol = new TreeViewColumn("Full Name", new Gtk.CellRendererText(), "text", 0);
		fullNameCol.Resizable = true;
		fullNameCol.Expand = true;
		fullNameCol.MinWidth = 150;
		fullNameCol.Sizing = TreeViewColumnSizing.Autosize;
		//
		TreeViewColumn emailCol = new TreeViewColumn("Email", new Gtk.CellRendererText(), "text", 1);
		emailCol.Resizable = true;
		emailCol.Expand = true;
		emailCol.MinWidth = 150;
		emailCol.Sizing = TreeViewColumnSizing.Autosize;
		//
		TreeViewColumn phoneOneCol = new TreeViewColumn("Phone Number One", new Gtk.CellRendererText(), "text", 2);
		phoneOneCol.Resizable = true;
		phoneOneCol.Expand = true;
		phoneOneCol.MinWidth = 150;
		phoneOneCol.Sizing = TreeViewColumnSizing.Autosize;
		//
		TreeViewColumn phoneTwoCol = new TreeViewColumn("Phone Number Two", new Gtk.CellRendererText(), "text", 3);
		phoneTwoCol.Resizable = true;
		phoneTwoCol.Expand = true;
		phoneTwoCol.MinWidth = 150;
		phoneTwoCol.Sizing = TreeViewColumnSizing.Autosize;
		//
		dgv_contacts.AppendColumn(fullNameCol);
		dgv_contacts.AppendColumn(emailCol);
		dgv_contacts.AppendColumn(phoneOneCol);
		dgv_contacts.AppendColumn(phoneTwoCol);
		dgv_contacts.ShowAll ();
		//Customize the file chooser button
		FileFilter filter  = new FileFilter();
		filter.Name = "vCard Files";
		filter.AddPattern ("*.vcf");
		filter.AddPattern ("*.vcard");
		ofd_select_vcard.AddFilter (filter);
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
		    // Remove previous content
		    Store.Clear();
		    // Get new data
		    try
		    {
		        vCardCollection collection = Deserializer.FromFile(ofd_select_vcard.Filename);
		        foreach (vCard vcard in collection)
		        {
		            Node node = new Node();
		            node.FullName = vcard.FormattedName;
		            node.EmailAddress = vcard.EmailAddresses.Count > 0 ? vcard.EmailAddresses[0].Email.Address : "";
		            node.PhoneNumber1 = vcard.PhoneNumbers.Count > 0 ? vcard.PhoneNumbers[0].Number : "";
		            node.PhoneNumber2 = vcard.PhoneNumbers.Count > 1 ? vcard.PhoneNumbers[1].Number : "";

		            Store.AddNode(node);
		        }
		    }
		    catch (NotImplementedException exception)
		    {
		        Console.WriteLine(exception.Message);
		        MessageDialog md = new MessageDialog(this, DialogFlags.DestroyWithParent, MessageType.Error,
		            ButtonsType.Close, "The selected file not yet supported by this library.\nError: " + exception.Message);
		        md.Run();
		        md.Destroy();
		    }
		    catch (Exception exception)
		    {
		        Console.WriteLine(exception.Message);
		        MessageDialog md = new MessageDialog (this, DialogFlags.DestroyWithParent, MessageType.Error,
		            ButtonsType.Close, "The selected file is of an incorrect format.\nError: " + exception.Message);
		        md.Run ();
		        md.Destroy ();
		    }

		}
		else
		{
		    MessageDialog md = new MessageDialog (this, DialogFlags.DestroyWithParent, MessageType.Error,
		        ButtonsType.Close, "The selected file either doesn't exist, or is of an incorrect format.");
		    md.Run ();
		    md.Destroy ();
		}
	}

	protected void txt_search_TextChanged(object sender, EventArgs e)
	{
		NodeStore searchStore = new NodeStore (typeof(Node));
		string searchQuery = txt_search.Buffer.Text;
		foreach (Node node in Store) {
			if (node.EmailAddress.Contains (searchQuery) ||
			    node.FullName.Contains (searchQuery) ||
			    node.PhoneNumber1.Contains (searchQuery) ||
			    node.PhoneNumber2.Contains (searchQuery)) {
				searchStore.AddNode (node);
			}
		}
		dgv_contacts.NodeStore = searchStore;
	}
}
