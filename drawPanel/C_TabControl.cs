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
	/// a class to control a tabbed panel
	/// </summary>
	public class C_TabControl:System.Windows.Forms.Control, Dispatchable
	{
		
		private crossy Main;
		private tab_radiogroup tabs;
		private line tabcontrolline;

		
		private LinkedList.LinkedList TabList;
		private bool crossed_once = false;
		protected int number_of_tabs;
		public virtual bool visible
		{
			get{return this.Visible;}
			set{Visible = value;}
		}

		private int how_many_tabs
		{
			set {number_of_tabs = value;}
			get {return number_of_tabs;}
		}
		public C_Tab transferTab;
		public virtual int height
		{
			get {return this.Size.Height;}
			set {this.Height = value;}
		}
		public virtual int width
		{
			get {return this.Size.Width;}
			set {this.Width = value;}
		}
		public virtual Point origin
		{
			//get {return (this.PointToScreen(Location));}
			get {return (Location);}
			set {}
		}
		public virtual Point origin2Screen
		{
			get {return (new Point(Main.Location.X,Main.Location.Y+Main.Height-60));}
			//get {return (Location);}
			set {}
		}

		public C_TabControl(crossy myMain)
		{
			C_Tab aTab;

			Main = myMain;
			this.tabs = new tab_radiogroup();
			aTab = new C_Tab(this.Main);
			aTab.myradiogroup = tabs;
			aTab.myradiogroup.selected(aTab);
			aTab.Location =  new Point(5,2);
			aTab.Size = new Size(80, 20);
			aTab.Tab_name = 1.ToString();
			aTab.tab_position =0;
			this.Controls.Add(aTab);
			this.TabList = new LinkedList.LinkedList();
			this.TabList.AddNode(aTab);
			this.how_many_tabs = 1;

			aTab.aDrawpanel = new C_TabPanel();
			this.Main.panel.Controls.Add(aTab.aDrawpanel);
			aTab.aDrawpanel.Location = new System.Drawing.Point(8, 0);
			aTab.aDrawpanel.Name = "Drawpanel";
			aTab.aDrawpanel.Size = new System.Drawing.Size(960, 6000);
			aTab.aDrawpanel.BackColor = Color.WhiteSmoke;
			aTab.aDrawpanel.Main = this.Main;
			aTab.aDrawpanel.BringToFront();
			transferTab = aTab;

			
			this.tabcontrolline = new line(LineOrientation.horizontal);
		}
		protected override void OnPaint(PaintEventArgs e)
		{

			Pen TabLinePen = new Pen(Color.Black, 1);

			Graphics tabController = e.Graphics;
			tabController.DrawLine(TabLinePen, 3, 10, Main.Width-45, 10);
			this.tabcontrolline.TopPoint = new Point(3,10);
			this.tabcontrolline.LowPoint = new Point(Main.Width-45, 10);
			
			
			
		}
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove (e);
			HowCrossed fromwhere = HowCrossed.none;
			
			fromwhere = (HowCrossed)this.tabcontrolline.crossed(new Point(e.X,e.Y));
			if(fromwhere !=HowCrossed.none)
			{
				if(e.Button ==MouseButtons.Left)
				{
					//Console.WriteLine("TabControlLine crossed");
					if(!crossed_once)
					{
						crossed_once = true;
					}
					else if(crossed_once & this.TabList.Count < 7)
					{
						//Console.WriteLine(this.Main.TabWidget.how_many_tabs);
						int x_coordinate = e.X;
						int where = this.how_many_tabs;
						if(x_coordinate<5)
						{ where = 0;}
						else if(x_coordinate<95)
						{ where = 1;}
						else if(95 < x_coordinate & x_coordinate<185)
						{ where = 1;}
						else if(185 < x_coordinate & x_coordinate<275)
						{ where = 2;}
						else if(275 < x_coordinate & x_coordinate<365)
						{ where = 3;}
						else if(365 < x_coordinate & x_coordinate<455)
						{ where = 4;}
						else if(455 < x_coordinate)
						{ where = 5;}
						else
						{
							Console.WriteLine("Eeewwww");
						}
						create_tab(where);
						
					}

				}
			}
		}
		public void OnNewLine(Point First, Point Second)
		{
			HowCrossed fromwhere = HowCrossed.none;
			
			//fromwhere = (HowCrossed)this.tabcontrolline.crossed(Second);
			Point First_client = this.PointToClient(First);
			Point Second_client = this.PointToClient(Second);
			fromwhere = (HowCrossed)this.tabcontrolline.how_crossed(First_client, Second_client);
			if(fromwhere !=HowCrossed.none)
			{
				//if(e.Button ==MouseButtons.Left)
				{
					//Console.WriteLine("TabControlLine crossed");
					if(!crossed_once)
					{
						crossed_once = true;
					}
					else if(crossed_once & this.TabList.Count < 7)
					{
						//Console.WriteLine(this.Main.TabWidget.how_many_tabs);
						int x_coordinate = Second_client.X;
						int where = this.how_many_tabs;
						if(x_coordinate<5)
						{ where = 0;}
						else if(x_coordinate<95)
						{ where = 1;}
						else if(95 < x_coordinate & x_coordinate<185)
						{ where = 1;}
						else if(185 < x_coordinate & x_coordinate<275)
						{ where = 2;}
						else if(275 < x_coordinate & x_coordinate<365)
						{ where = 3;}
						else if(365 < x_coordinate & x_coordinate<455)
						{ where = 4;}
						else if(455 < x_coordinate)
						{ where = 5;}
						else
						{
							Console.WriteLine("Eeewwww");
						}
						create_tab(where);
						crossed_once = false;
					}

				}
			}
		}
		public void OnMouseMove_private_dispatch(MouseEventArgs e)
		{
			OnMouseMove(e);
		}

		public void OnMouseEnter_private_dispatch(EventArgs e)
		{
			OnMouseEnter(e);
		}
		public void OnMouseLeave_private_dispatch(EventArgs e)
		{
			OnMouseLeave(e);
		}

		public void OnMouseDown_private_dispatch(MouseEventArgs e)
		{
			OnMouseDown(e);
		}
		public void OnMouseUp_private_dispatch(MouseEventArgs e)
		{
			OnMouseUp(e);
		}
		public void OnMouseHover_private_dispatch(EventArgs e)
		{
			OnMouseHover(e);
		}
		protected override void OnMouseDown(MouseEventArgs e)
		{
			init_tab_line();
		}

		private void init_tab_line()
		{
			crossed_once = false;
		}
		/// <summary>
		/// creates a new tab 
		/// </summary>
		/// <param name="where">position after which the new tab is to be inserted</param>
		public void create_tab(int where)
		{
			C_Tab aTab;
			
			//this.Main.TabWidget.active_tab_index = where + 1;

			aTab = new C_Tab(this.Main);
			aTab.tab_position = where;
			aTab.myradiogroup = tabs;
			aTab.myradiogroup.selected(aTab);
			
			aTab.Size = new Size(80, 20);
			this.how_many_tabs+=1;
			aTab.Tab_name = (this.how_many_tabs).ToString();
			aTab.Tab_number = where;
			this.Controls.Add(aTab);

			aTab.aDrawpanel = new C_TabPanel();
			this.Main.panel.Controls.Add(aTab.aDrawpanel);
			aTab.aDrawpanel.Location = new System.Drawing.Point(8, 0);
			aTab.aDrawpanel.Name = "Drawpanel";
			aTab.aDrawpanel.Size = new System.Drawing.Size(960, 1000);
			aTab.aDrawpanel.BackColor = Color.WhiteSmoke;
			aTab.aDrawpanel.Main = this.Main;
			aTab.aDrawpanel.BringToFront();

			
			object tmp = null;
			try	
			{
				tmp = this.TabList.get_object_from_index(where);
			}
			catch(Exception e)
			{
				tmp = null;
			}
			if(tmp == null)
			{
				Console.WriteLine("Couldn't find an object at index " + where + " create Tab at end instead");
				int numberOfTabs = TabList.Count;
				tmp = this.TabList.get_object_from_index(numberOfTabs-1);
				this.TabList.addAfter(tmp, aTab);
				refresh();
			}
			else
			{
				//this.TabList.addAfter(this.TabList.get_object_from_index(where), aTab);
				this.TabList.addAfter(tmp, aTab);
				refresh();
				
			}
			
			
		}
		/// <summary>
		/// causes the tabs to be redrawn
		/// </summary>
		private void refresh()
		{
			C_Tab aTab;
			Main.FlowMenu.firstrun = true;
			Point previous_location = new Point(5,2);
			int tab_pos_adjuster = 0;
			aTab = (C_Tab)this.TabList.GetFirst();
			while(aTab != null)
			{
				aTab.Location = previous_location;
				aTab.tab_position = tab_pos_adjuster;
				aTab.Refresh();
				if(aTab.Active)
					aTab.aDrawpanel.Visible = true;
				else
					aTab.aDrawpanel.Visible = false;

				previous_location.X += 90;
				tab_pos_adjuster++;
				aTab = (C_Tab)this.TabList.GetNext();
			}
			
		}
		/// <summary>
		/// removes the specified tab
		/// </summary>
		/// <param name="aTab">Tab to remove</param>
		public void remove_tab(C_Tab aTab)
		{
			C_Tab first = (C_Tab)this.TabList.GetFirst();
			first.myradiogroup.selected(first);
			if(aTab != first)
			{

				this.TabList.RemoveNode(aTab);
				aTab.Dispose();
			}
			first.aDrawpanel.Visible = true;
			first.aDrawpanel.BringToFront();
			refresh();
		}
		/// <summary>
		/// tells us which panel is currently active
		/// </summary>
		/// <returns>active C_TabPanel is returned</returns>
		public C_TabPanel get_active_TabPanel()
		{
			C_Tab aTab;
			aTab = (C_Tab)this.TabList.GetFirst();
			while(aTab != null)
			{
				if(aTab.Active)
					return aTab.aDrawpanel;
				else
				aTab = (C_Tab)this.TabList.GetNext();
				
			}
			return null;
		}
	
	}
	
}

