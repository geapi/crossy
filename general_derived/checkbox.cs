using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;

using System.Windows.Forms;
using System.Data;
using Microsoft.Ink;

namespace crossy
{
	
	
	public class checkbox:System.Windows.Forms.Control
	{
		public enum HowCrossed:int
		{
			fromright = 1 ,
			fromleft = 2 ,
			fromtop = 3,
			frombottom = 4,
			none = 0
		}
		// label and register to radiobutton or checkbox class
		public Color brushcolor;
		public Point current;
		public Point TopPoint;
		public String label;
		public String trigger;
		public Point LowPoint;
		public Pen buttonlinePen;
		public Pen stopperPen;
		public crossy Main;
		private line checkbox_line;
		

	
		public bool selected = false;
		public string myFather;

		public checkgroup mycheckgroup;
		public String myValue;
		public Font font;
		private Pen whitePen = new Pen(Color.White, 2);
		private Pen blackPen = new Pen(Color.Black, 2);
		public Pen between = new Pen(Color.White, 2);
		bool valuechecked = false;
		Point old_coord = new Point(-1,-1);
		bool NOOLDVALUE = true;
		public System.Drawing.Point crosspoint = new Point(-1,-1);
		public Point landingpoint;
		

		public checkbox()
		{
			brushcolor = Color.Black;
			Pen beetween = whitePen;
			
			this.BackColor = Color.White;
			this.checkbox_line = new line(LineOrientation.tilted_left);
			
			
		}
		
		public virtual void OnCrossing(string what,HowCrossed fromwhere, bool selected, checkbox optbox)
		{
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
		
		protected override void OnMouseDown(MouseEventArgs e)
		{
			//Console.Write("v");
			Win32Application.Win32.ReleaseCapture();
			this.Cursor = util.CursorSwitcher.change_mouse_cursor(true);
			
			landingpoint = new Point(e.X,e.Y);
			old_coord = landingpoint;
			checkbox_line.init_line();
			NOOLDVALUE = false;
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
			this.checkbox_line.init_line();
		
		}

		protected override void OnMouseHover(EventArgs e)
		{
			//Console.Write("?");
			this.Cursor = util.CursorSwitcher.change_mouse_cursor(false);
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			//Console.Write("<\n");
			NOOLDVALUE = true;
			
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			this.Cursor = util.CursorSwitcher.change_mouse_cursor(false);
			
			//Console.Write("^");
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				//Console.Write("!");
				this.Cursor = util.CursorSwitcher.change_mouse_cursor(true);
				HowCrossed fromwhere;
				Point pixelcoord = new Point(e.X,e.Y);
				current = pixelcoord;
				
				Invalidate();
				//Console.Write(" np ");
				//strokecollector.Renderer.InkSpaceToPixel(ge, ref pixelcoord);
				if(NOOLDVALUE)
				{
					old_coord = pixelcoord;
					NOOLDVALUE = false;
				}
			
				if((old_coord.X != pixelcoord.X) || (old_coord.Y != pixelcoord.Y))
				{
					fromwhere = (HowCrossed)this.checkbox_line.how_crossed(old_coord, pixelcoord);
					
					//fromwhere = (HowCrossed)this.checkbox_line.crossed(pixelcoord);
				
					
					if(fromwhere != HowCrossed.none)
					{
						//Console.WriteLine(fromwhere);

						//selected = Main.eventhandler.optcrossed(myFather, trigger, (int)fromwhere, selected);
						//Console.WriteLine(pixelcoord+""+ fromwhere);
						
						switch((int)fromwhere)
						{
							case 1: // left
								goto case 6;
							case 2:  // right
								goto case 6;
							case 3: // top
								goto case 6;
							case 4: // bottom
								goto case 6;
							case 6:
								//Console.WriteLine(fromwhere);
								OnCrossing(trigger, fromwhere, !selected, this);
								Invalidate();
								break;
					
							default:
								break;
						}
					}
				}
				old_coord = pixelcoord;
			
			}
			else
			{
				this.Cursor = util.CursorSwitcher.change_mouse_cursor(false);
			}
		}


		protected override void OnPaint(PaintEventArgs e)
		{

			this.TopPoint = new Point(this.Width-25,5);//this.Height-5);;
			this.LowPoint = new Point(this.Width-5,25);
			this.checkbox_line.TopPoint = this.TopPoint;
			this.checkbox_line.LowPoint = this.LowPoint;
			System.Drawing.Drawing2D.LinearGradientBrush myBrush = new 
				System.Drawing.Drawing2D.LinearGradientBrush(ClientRectangle, brushcolor,
				Color.Blue, System.Drawing.Drawing2D.LinearGradientMode.Horizontal);
			System.Drawing.StringFormat format= new StringFormat();
			
			Graphics buttonline = e.Graphics;
			// top stopper
			buttonline.DrawLine(blackPen, TopPoint.X, TopPoint.Y, TopPoint.X+4, TopPoint.Y+4);
			// buttom stopper
			buttonline.DrawLine(blackPen, LowPoint.X, LowPoint.Y, LowPoint.X-4, LowPoint.Y-4);
			// the line
		    buttonline.DrawLine(between, TopPoint.X+4, TopPoint.Y+4, LowPoint.X-4, LowPoint.Y-4);
			buttonline.DrawString(label,font ,myBrush,2 , 
									7 ,format);
			//buttonline.DrawLine(blackPen, current.X, current.Y, current.X + 1, current.Y +1 );
				//buttonline.DrawString(label,font ,myBrush,(float)TopPoint.X + (((LowPoint.X - TopPoint.X )/2)-((label.Length*7) /2)) , (float)(TopPoint.Y - 10) - font.GetHeight() ,format);
			
			
		}
	
			
			
		
		
	}
	
	
}
