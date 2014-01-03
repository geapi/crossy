using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;

using System.Windows.Forms;
using System.Data;
using Microsoft.Ink;

using System.Runtime.InteropServices;



namespace crossy
{
	public class MoveButton:button
	{
		
		bool isonthemove =false;
		Point startpoint = new Point();
		public Point actual_coord;
		public Point delta = new Point(0,0);

		bool firstrun = true;
		Point initial_client_point = new Point();
		Point initial_screen_coords = new Point(0,0);
		Point saved_screen_coords = new Point();
		Point toScreen = new Point();
		Point initial_position_local = new Point();
		Point initial_position_global = new Point();
		public System.Drawing.Point crosspoint = new Point(-1,-1);
		public Point oldcoord = new Point(-1,-1);
		public Point landingpoint;
		
		private Point original_Location = new Point();

		public MoveButton(crossy myMain, Point Location)
		{
			original_Location = Location;
			ishorizontal = false;
			brushcolor = Color.Black;
			this.Text = "button";
			this.buttonlinePen = new Pen(Color.Black, 2);
			this.stopperPen = new Pen(Color.Black, 4);
			this.font = new Font("Verdana", 12);
			this.BackColor = Color.White;
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			this.Cursor = util.CursorSwitcher.change_mouse_cursor(true);
			
			firstrun=true;
			landingpoint = new Point(e.X,e.Y);
			oldcoord = landingpoint;
			NOOLDVALUE = false;
			this.myline.init_line();
			

		}
		protected override void OnMouseMove(MouseEventArgs e)
		{
			
			if (e.Button == MouseButtons.Left)
			{
				this.Cursor = util.CursorSwitcher.change_mouse_cursor(true);
				//Console.Write("!");
				HowCrossed fromwhere = HowCrossed.none;
				Point pixelcoord = new Point(e.X,e.Y);
				toScreen = this.PointToScreen(pixelcoord);
			
				if(NOOLDVALUE)
				{
					oldcoord = pixelcoord;
					NOOLDVALUE = false;
				}
				if((oldcoord.X != pixelcoord.X) || (oldcoord.Y != pixelcoord.Y))
				{
					fromwhere = (HowCrossed)this.myline.how_crossed(oldcoord, pixelcoord);
				}

				if(fromwhere != HowCrossed.none)
				{
					OnCrossing(fromwhere);
					if(firstrun == true)
					{
						initial_position_global = this.PointToScreen(pixelcoord);
						initial_position_local = original_Location;
						// local is missing
						firstrun= false;
					}

				}
				if(isonthemove == true)
				{
					panelsOff();

					//Console.Write("@");
					delta = new Point(toScreen.X-initial_position_global.X, toScreen.Y-initial_position_global.Y);
					//Console.Write(delta);
					new_position(new System.Drawing.Point(initial_position_local.X + delta.X,initial_position_local.Y + delta.Y));
					//Main.FlowMenu.filemenu.Location = new System.Drawing.Point(initial_position_local.X + delta.X,initial_position_local.Y + delta.Y);
					this.Capture = true;
				}
			}
			else
			{
				this.Cursor = util.CursorSwitcher.change_mouse_cursor(false);
			}
		}
		protected override void OnMouseUp(MouseEventArgs e)
		{
			this.Cursor = util.CursorSwitcher.change_mouse_cursor(false);
			//Console.Write("^");
			isonthemove = false;
			firstrun = true;
			NOOLDVALUE = true;
			this.Capture = false;
			
		}
		protected override void OnMouseLeave(EventArgs e)
		{
			//Main.Palette.palette_location = Main.Palette.Location;
			//isonthemove = false;
			this.Capture = false;

				NOOLDVALUE = true;

		}
			
			
		public override void OnCrossing(HowCrossed fromwhere)
		{
				
			if (fromwhere == HowCrossed.fromleft)
			{
				isonthemove = true;
				//Console.WriteLine(fromwhere);
			}
			if (fromwhere == HowCrossed.fromright)
			{
				if(isonthemove == false)
				{
					back_to_original_position();
					//Main.FlowMenu.filemenu.Location = new System.Drawing.Point(0, 200);
					panelsOff();
				}
				//else
				//	isonthemove = false;
				//Console.WriteLine(fromwhere);
			}
		}
		public virtual void back_to_original_position()
		{
		}
		public virtual void new_position(Point new_Position)
		{
		}
		public void set_initial_position(Point initial_position)
		{
			original_Location = initial_position;
		}
		public void panelsOff()
		{
				
			Main.Palette.penpanel.Visible = false;
			Main.Palette.selectpanel.Visible = false;
			Main.Palette.erasepanel.Visible = false;
			Main.Palette.hilipanel.Visible = false;
			Main.Palette.penpanel.strokepanel.Visible = false;
		}
	}
	
	

}
