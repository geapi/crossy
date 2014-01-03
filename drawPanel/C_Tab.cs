using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;

using System.Windows.Forms;
using System.Data;
using System.IO;




namespace crossy
{
	
	
	public class C_Tab:optionbox
	{
		
		private bool active = false;
		private string tabname = "a_tab";
		private int my_number;
		private int cross_counter =0;
		private int tab_pos;
		public C_TabPanel aDrawpanel;
		public tab_radiogroup my_radiogroup;
		public int tab_position
		{
			set{ tab_pos = value;}
			get{ return tab_pos; }
		}
		//+((tab_position*width)-width),
		//+((tab_pos*width)-width)


		/// <summary>
		/// The string that is shown on the Tab
		/// </summary>
		public string Tab_name
		{
			get { return this.tabname;}
			set 
			{
				this.tabname = value;
				this.Refresh();
			}
		}
		
		public int Tab_number
		{
			get {return this.my_number;}
			set {this.my_number = value;}
		}
		/// <summary>
		/// Status of the Tab
		/// </summary>
		public override bool Active
		{
			get { return active;}
			set 
			{
				active = value;
				this.Refresh();
			}
		}
		public C_Tab(crossy myMain)
		{
			Main = myMain;
			
			this.Size = new System.Drawing.Size(80, 20);
			this.brushcolor = Color.Transparent;
			this.label = "";
			this.buttonlinePen = new Pen(Color.Transparent,2);
			this.stopperPen = new Pen(Color.Transparent, 4);
			this.font = new Font("Verdana", 8);
			this.ishorizontal = false; 	
			this.IconPen = new Pen(Color.Transparent);
		}
		public override void OnCrossing(string what, HowCrossed fromwhere, bool selected, optionbox thisopt)
		{
			if(this.Active)
			{
				this.myradiogroup.selected(this);
			}
			if(!this.Active)
			{
				this.myradiogroup.selected(this);
				this.aDrawpanel.Visible = true;
				this.aDrawpanel.BringToFront();
			}
			cross_counter++;
			if(cross_counter == 4 & this.my_number!=0)
			{
				this.Main.central_TabControl.remove_tab(this);
			}
		}
		protected override void OnMouseDown(MouseEventArgs e)
		{
			init_tab();
		}
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			Pen activeTabPen = new Pen(Color.Black, 20);
			// top and buttom
			Pen passiveTabPen = new Pen(Color.Gray, 20);

			System.Drawing.Drawing2D.LinearGradientBrush myBrush = new System.Drawing.Drawing2D.LinearGradientBrush(ClientRectangle, Color.White,
				Color.White, System.Drawing.Drawing2D.LinearGradientMode.Horizontal);
			System.Drawing.StringFormat format= new StringFormat();
			Font schrift = new Font("Verdana", 10, System.Drawing.FontStyle.Bold);
			Graphics tab = e.Graphics;

			if(Active)
			{
				tab.DrawLine(activeTabPen, 0, 10, 85, 10);
				Main.Slider.Refresh();
			}
			else if(!Active)
			{
				tab.DrawLine(passiveTabPen, 0, 10, 85, 10);
			}

			tab.DrawString(tabname,schrift, myBrush,15,2,format);
		}

		public void init_tab()
		{
			cross_counter = 0;
		}

		
	}
	
}

