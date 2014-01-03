using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;

using System.Windows.Forms;
using System.Data;
using Microsoft.Ink;
using System.IO;




namespace crossy
{
	
	/// <summary>
	/// wrapper class that makes the drawPanel selectable
	/// for tab browsing
	/// </summary>
	
	public class C_TabPanel:drawPanel, Selectable
	{
		private int my_index;
		public panel_radiogroup my_radiogroup;
		protected bool aActive;

		public int index
		{
			set { my_index = value;}
			get {return my_index;}

		}
		public bool Active
		{
			set {aActive = value;}
			get {return aActive;}
		}
	
		public C_TabPanel()
		{

		}
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint (e);
			if(Active)
			{
				this.BringToFront();
				
			}
		}


	}
}

