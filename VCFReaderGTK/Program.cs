using System;
using Gtk;

namespace VCFReaderGTK
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Application.Init ();
			UI win = new UI ();
			win.Show ();
			Application.Run ();
		}
	}
}
