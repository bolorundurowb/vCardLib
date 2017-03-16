using System;

namespace vCardLibUI.Models
{
	[Gtk.TreeNode (ListOnly=true)]
	public class Node : Gtk.TreeNode
	{
		public Node ()
		{
		}

		[Gtk.TreeNodeValue (Column=0)]
		public string FullName;

		[Gtk.TreeNodeValue (Column=1)]
		public string EmailAddress;

		[Gtk.TreeNodeValue (Column=2)]
		public string PhoneNumber1;

		[Gtk.TreeNodeValue (Column=3)]
		public string PhoneNumber2;
	}
}

