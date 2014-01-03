using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;

using System.Windows.Forms;
using System.Data;
using Microsoft.Ink;

namespace crossy
{
	// label is missing and return value
	public delegate void EventHandler(object sender, System.EventArgs e);
	public class button:System.Windows.Forms.Control
	{

		//public eventHandler handler
		public crossy Main;
		public line myline;
		
		
		public Color brushcolor;
		private Point TopPoint;
		private Point LowPoint;
		private Point position;
		public Pen between = new Pen(Color.Transparent, 2);
		public Pen buttonlinePen = new Pen(Color.Transparent, 2);
		public Pen stopperPen;
		public String label ="";
		public String trigger = "";
		public Font font;
		public bool ishorizontal;
		static  int NOTAPOINT =-99999;
		protected bool NOOLDVALUE = true;
		Point old_coord = new Point(NOTAPOINT,NOTAPOINT);
		public System.Drawing.Point crosspoint = new Point(-1,-1);
		public Point landingpoint;
		
		
		public bool mouseIN;
		public Point cross_position;

		
	
		
		public button()
		{
			ishorizontal = false;
			this.brushcolor = Color.Black;
			this.myline = new line(LineOrientation.vertical);
			
		}

		public virtual void OnCrossing(HowCrossed fromwhere)
		{
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			//Console.Write("v");
			Win32Application.Win32.ReleaseCapture();
			//this.Cursor = System.Windows.Forms.Cursors.Hand;
			this.Cursor = util.CursorSwitcher.change_mouse_cursor(true);
			
			//this.Cursor = new System.Windows.Forms.Cursor(new IntPtr(Win32Application.Win32.LoadCursorFromFile(@"C:\WINDOWS\Cursors\pen_rm.cur")));
			//Console.WriteLine(this.Cursor.ToString());
			this.Invalidate();
				
			landingpoint = new Point(e.X,e.Y);
			NOOLDVALUE = false;
			old_coord = landingpoint;
			myline.init_line();
			
		}

		protected override void OnGotFocus(EventArgs e)
		{
			//Console.Write("F");
		}

		protected override void OnLostFocus(EventArgs e)
		{
			//Console.Write("f");
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			//Console.Write(">");
			
			this.myline.init_line();
		
		}

		protected override void OnMouseHover(EventArgs e)
		{
			this.Cursor = util.CursorSwitcher.change_mouse_cursor(false);
			//Console.Write("?");
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			NOOLDVALUE = true;
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			//Console.Write("^");
			this.Cursor = util.CursorSwitcher.change_mouse_cursor(false);
			this.Invalidate();
			
			
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			
			if (e.Button == MouseButtons.Left)
			{
				this.Cursor = util.CursorSwitcher.change_mouse_cursor(true);
				HowCrossed fromwhere = HowCrossed.none;
				Point pixelcoord = new Point(e.X,e.Y);
				
				if(NOOLDVALUE)
				{
					old_coord = pixelcoord;
					NOOLDVALUE = false;
				}
				if(((old_coord.X != pixelcoord.X) || (old_coord.Y != pixelcoord.Y)))
				{
					//Console.WriteLine("in button if");
					fromwhere = (HowCrossed)this.myline.how_crossed(old_coord, pixelcoord);
				}
				
				//fromwhere = (HowCrossed)this.myline.crossed(pixelcoord);
				//Console.WriteLine(fromwhere);
				if(fromwhere != HowCrossed.none)
				{
					//Console.WriteLine(pixelcoord);
					cross_position = pixelcoord;
					OnCrossing(fromwhere);
					position = pixelcoord;
					
				}
				
				old_coord = pixelcoord; // this is quite dangerous: we need to compute the realcoordinate!
			}
			else
			{
				this.Cursor = util.CursorSwitcher.change_mouse_cursor(false);
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
			//Console.WriteLine("Mouse down");
		}
		public void OnMouseUp_private_dispatch(MouseEventArgs e)
		{
			OnMouseUp(e);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			if(ishorizontal == false)
			{
				this.TopPoint = new Point(this.Width/2,2);;
				this.LowPoint = new Point(this.Width/2,this.Height-2);
				this.myline.orientation = LineOrientation.vertical;

			}
			else
			{
				this.TopPoint = new Point(2,this.Height-5);;
				this.LowPoint = new Point(this.Width-2,this.Height-5);
				this.myline.orientation = LineOrientation.horizontal;
				
			}
			this.myline.TopPoint = this.TopPoint;
			this.myline.LowPoint = this.LowPoint;
			
			
			System.Drawing.Drawing2D.LinearGradientBrush myBrush = new 
				System.Drawing.Drawing2D.LinearGradientBrush(ClientRectangle, brushcolor,
				Color.Blue, System.Drawing.Drawing2D.LinearGradientMode.Horizontal);
			System.Drawing.StringFormat format= new StringFormat();
			
			Graphics buttonline = e.Graphics;
			if(ishorizontal == false)
			{
				buttonline.DrawLine(between, TopPoint.X, TopPoint.Y, LowPoint.X, LowPoint.Y);
				// top stopper
				buttonline.DrawLine(stopperPen, TopPoint.X, TopPoint.Y, TopPoint.X , TopPoint.Y + 2);//(LowPoint.Y - TopPoint.Y)/20);
				//buttom stopper
				buttonline.DrawLine(stopperPen, LowPoint.X, LowPoint.Y, LowPoint.X, LowPoint.Y - 2);//(LowPoint.Y - TopPoint.Y)/20);
				buttonline.DrawString(label,font ,myBrush, 4 , (float)TopPoint.Y +((LowPoint.Y - TopPoint.Y)/2)-(font.GetHeight()/2)  ,format);
			}
			else
			{
				//buttonline.DrawLine(buttonlinePen, TopPoint.X, TopPoint.Y, LowPoint.X, LowPoint.Y);
				// left stopper
				buttonline.DrawLine(stopperPen, TopPoint.X, TopPoint.Y, TopPoint.X + (LowPoint.X - TopPoint.X)/20, TopPoint.Y);
				//right stopper
				buttonline.DrawLine(stopperPen, LowPoint.X, LowPoint.Y, LowPoint.X-(LowPoint.X - TopPoint.X)/20, LowPoint.Y);
				buttonline.DrawString(label,font ,myBrush,(float)TopPoint.X + (((LowPoint.X - TopPoint.X )/2)-((label.Length * 11 -7)/2)) , (float)(TopPoint.Y - 2) - font.GetHeight() /*(LowPoint.X - TopPoint.X)/30*/ ,format);
			}

		}
		
		
	}

}
