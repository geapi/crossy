using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;

using System.Windows.Forms;
using System.Data;
using Microsoft.Ink;

namespace crossy
{
	
	
	public class optionbox:System.Windows.Forms.Control,Selectable
	{
		
		// label and register to radiobutton or checkbox class
		public Color brushcolor;
		
		public Point TopPoint;
		public String label = "";
		public String trigger;
		public Point LowPoint;
		Point current;
		public Queue strokequeue = new Queue();
		public Pen buttonlinePen;
		public Pen stopperPen;
		public crossy Main;
		
		public bool ishorizontal = false;
		
		
		public bool selected = false;
		public string myFather;
		public radiogroup myradiogroup;
		//public checkgroup mycheckgroup;
		public String myValue;
		public Font font;
		public Pen IconPen;
		private Pen whitePen = new Pen(Color.White, 2);
		private Pen blackPen = new Pen(Color.Black, 2);
		public Pen between = new Pen(Color.Transparent, 2);
		bool valuechecked = false;
		private line optionline;
		static int NOTAPOINT =-99999;
		bool NOOLDVALUE = true;
		public System.Drawing.Point crosspoint = new Point(-1,-1);
		public Point oldcoord = new Point(NOTAPOINT,NOTAPOINT);
		public Point landingpoint;
		public virtual bool Active
		{
			get {return selected;}
			set {selected = value;}
		}
		

		public optionbox()
		{
			this.brushcolor = Color.Black;
			Pen beetween = blackPen;
			this.BackColor = Color.White;
			this.optionline = new line(LineOrientation.vertical);
		
			
		}
	
		public virtual void OnCrossing(string what,HowCrossed fromwhere, bool selected, optionbox optbox)
		{
		}
		


		protected override void OnMouseDown(MouseEventArgs e)
		{
			//Console.Write("v");
			Win32Application.Win32.ReleaseCapture();
			this.Cursor = util.CursorSwitcher.change_mouse_cursor(true);
			
			landingpoint = new Point(e.X,e.Y);
			oldcoord = landingpoint;
			NOOLDVALUE = false;
			this.optionline.init_line();
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
			
			this.optionline.init_line();

		
		}

		protected override void OnMouseHover(EventArgs e)
		{
			//Console.Write("?");
			this.Cursor = util.CursorSwitcher.change_mouse_cursor(false);
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			//Console.Write("<\n");
			NOOLDVALUE =true;
			//oldcoord = new Point(NOTAPOINT,NOTAPOINT);
			//this.optionline.init_line();
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
				
				HowCrossed fromwhere = HowCrossed.none;
				Point pixelcoord = new Point(e.X,e.Y);
				
				strokequeue.Enqueue(pixelcoord);
				//Console.Write(oldcoord + " \t " + pixelcoord );
				if(NOOLDVALUE)
				{
					oldcoord = pixelcoord;
					NOOLDVALUE = false;
				}
				if(((oldcoord.X != pixelcoord.X) || (oldcoord.Y != pixelcoord.Y)))
				{
					//Console.WriteLine("in if");
					fromwhere = (HowCrossed)this.optionline.how_crossed(oldcoord, pixelcoord);
					optionline.init_line();
				}
				//fromwhere = (HowCrossed)this.optionline.crossed(pixelcoord);
				if(fromwhere != HowCrossed.none)
				{

					//selected = Main.eventhandler.optcrossed(myFather, trigger, (int)fromwhere, selected);
					//Console.WriteLine(selected);
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
			
							OnCrossing(trigger, fromwhere, !selected, this);
							Invalidate();
						
							break;
					
						default:
							break;
					}
				}
				oldcoord = pixelcoord;
			
			}
			else
			{
				this.Cursor = util.CursorSwitcher.change_mouse_cursor(false);
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			if(ishorizontal == false)
			{
				this.TopPoint = new Point(this.Width/2,2);;
				this.LowPoint = new Point(this.Width/2,this.Height-2);
				this.optionline.orientation = LineOrientation.vertical;
				
			}
			else
			{
				this.TopPoint = new Point(2,this.Height-5);;
				this.LowPoint = new Point(this.Width-2,this.Height-5);
				this.optionline.orientation = LineOrientation.horizontal;
			}
			this.optionline.TopPoint = this.TopPoint;
			this.optionline.LowPoint = this.LowPoint;
			/*if(ishorizontal == false)
			{
				this.TopPoint = new Point(this.Width-10,2);;
				this.LowPoint = new Point(this.Width-10,this.Height-2);
			}
			else
			{
				this.TopPoint = new Point(2,this.Height-10);;
				this.LowPoint = new Point(this.Width-2,this.Height-10);
			}*/
			System.Drawing.Drawing2D.LinearGradientBrush myBrush = new 
				System.Drawing.Drawing2D.LinearGradientBrush(ClientRectangle, brushcolor,
				Color.Blue, System.Drawing.Drawing2D.LinearGradientMode.Horizontal);
			System.Drawing.StringFormat format= new StringFormat();
			
			Graphics buttonline = e.Graphics;
			/*Pen blackPenFrame = new Pen(Color.Black, 1);
			buttonline.DrawLine(blackPenFrame, 0, 0, 60, 0);
			buttonline.DrawLine(blackPenFrame, 0, 0, 0, 30);
			buttonline.DrawLine(blackPenFrame, 59, 0, 59, 29);
			buttonline.DrawLine(blackPenFrame, 0, 29, 59, 29);*/
			if(ishorizontal == false)
			{
				
				// the icon
				//buttonline.DrawLine(IconPen, 10, this.Height/2 + 2  , this.Width-10, this.Height/2 -2 );

				buttonline.DrawLine(between, TopPoint.X, TopPoint.Y, LowPoint.X, LowPoint.Y);
				// top stopper
				buttonline.DrawLine(stopperPen, TopPoint.X, TopPoint.Y, TopPoint.X , TopPoint.Y + (LowPoint.Y - TopPoint.Y)/20);
				//buttom stopper
				buttonline.DrawLine(stopperPen, LowPoint.X, LowPoint.Y, LowPoint.X, LowPoint.Y - (LowPoint.Y - TopPoint.Y)/20);
				buttonline.DrawString(label,font ,myBrush,4, (float)TopPoint.Y +((LowPoint.Y - TopPoint.Y)/2)-(font.GetHeight()/2)  ,format);
			}
			else
			{
				buttonline.DrawLine(between, TopPoint.X, TopPoint.Y, LowPoint.X, LowPoint.Y);
				// left stopper
				buttonline.DrawLine(stopperPen, TopPoint.X, TopPoint.Y, TopPoint.X + (LowPoint.X - TopPoint.X)/20, TopPoint.Y);
				//right stopper
				buttonline.DrawLine(stopperPen, LowPoint.X, LowPoint.Y, LowPoint.X-(LowPoint.X - TopPoint.X)/20, LowPoint.Y);
				buttonline.DrawString(label,font ,myBrush,(float)TopPoint.X + (((LowPoint.X - TopPoint.X )/2)-((label.Length*7) /2)) , (float)(TopPoint.Y - 10) - font.GetHeight() ,format);
			}
			/*System.Collections.IEnumerator myEnumerator = strokequeue.GetEnumerator();
			Point previous =(Point) myEnumerator.Current;
			current =(Point) myEnumerator.Current;
			while(myEnumerator.MoveNext())
			{
				//buttonline.DrawLine(blackPen, previous.X, previous.Y, current.X , current.Y );
				previous = current;
			}*/
		
		}
	
			
			
		
		
	}
	
	
}
