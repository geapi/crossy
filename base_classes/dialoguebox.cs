using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;

using System.Windows.Forms;
using System.Data;
using Microsoft.Ink;

namespace crossy
{

	public class dialoguebox: System.Windows.Forms.Panel //util.TransparentWindow //System.Windows.Forms.Panel //
	{
		


		public Pen buttonlinePen;
		public Pen stopperPen;
		public String label, trigger;
		public crossy Main;
		public bool isRadioBox;
		public bool isCheckBox;
		
		public Point coordinate;
		private line top_line;
		private line left_line;
		private line right_line;
		private line buttom_line;
		public System.Drawing.Point crosspoint = new Point(-1,-1);
		public Point oldcoord = new Point(-1,-1);
		public Point landingpoint;
		bool NOOLDVALUE = true;
	
		public dialoguebox()
		{
			this.top_line = new line(LineOrientation.horizontal);
			this.left_line = new line(LineOrientation.vertical);
			this.right_line = new line(LineOrientation.vertical);
			this.buttom_line = new line(LineOrientation.horizontal);
		}
		public virtual void OnCrossing(HowCrossed fromwhere)
		{
		}
		protected override void OnMouseDown(MouseEventArgs e)
		{
			//Console.Write("v");
			this.Cursor = util.CursorSwitcher.change_mouse_cursor(true);
			//Win32Application.Win32.ReleaseCapture();
			this.Focus();
			//Console.WriteLine(this.Focused);
			if (Main != null)
			{
				
			}
			landingpoint = new Point(e.X,e.Y);
			oldcoord = landingpoint;
			NOOLDVALUE = false;
			this.left_line.init_line();
			this.top_line.init_line();
			this.right_line.init_line();
			this.buttom_line.init_line();
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
			//this.Focus();
			Win32Application.Win32.SetCapture(Handle);
			//Console.WriteLine(this.Focused);
			this.right_line.TopPoint = new Point(this.Width-4,0);
			this.right_line.LowPoint = new Point(this.Width-4,this.Height);
			this.left_line.TopPoint = new Point(4,0);
			this.left_line.LowPoint = new Point(4,this.Height);
			this.top_line.TopPoint = new Point(0,4);
			this.top_line.LowPoint = new Point(this.Width,4);
			this.buttom_line.TopPoint = new Point(0,this.Height-4);
			this.buttom_line.LowPoint = new Point(this.Width,this.Height-4);
			//Console.WriteLine(TopPointR);
			//Console.WriteLine(LowPointR);
			this.left_line.init_line();
			this.top_line.init_line();
			this.right_line.init_line();
			this.buttom_line.init_line();
		
		}

		protected override void OnMouseHover(EventArgs e)
		{
			///Console.Write("?");
			this.Cursor = util.CursorSwitcher.change_mouse_cursor(false);
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			//this.Invalidate();
			//Console.Write("out");
			NOOLDVALUE = true;
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			this.Cursor = util.CursorSwitcher.change_mouse_cursor(false);
			if (Main != null)
			{
				
			} 
			//stroki.Ink.DeleteStrokes(stroki.Ink.Strokes);
			//this.Invalidate();
			//Main.DrawTheStrokes();
			//Console.Write("^");
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{

				//Console.WriteLine(e.X + " " + e.Y);

				this.Cursor = util.CursorSwitcher.change_mouse_cursor(true);
				Point crosscoord;
				HowCrossed fromwhereleft = HowCrossed.none, fromwhereright= HowCrossed.none, 
					fromwheretop= HowCrossed.none, fromwherebottom= HowCrossed.none;
				//Point pixelcoord = new Point();
				//Graphics ge = this.CreateGraphics();
				Point pixelcoord = new Point(e.X,e.Y);
				coordinate = pixelcoord;
				
				//Console.Write(pixelcoord+" np ");
				//strokecollector.Renderer.InkSpaceToPixel(ge, ref pixelcoord);
				//Console.WriteLine(pixelcoord);
				// call the guy who calculates the angle
				if(NOOLDVALUE)
				{
					oldcoord = pixelcoord;
					NOOLDVALUE = false;
				}
				if((oldcoord.X != pixelcoord.X) || (oldcoord.Y != pixelcoord.Y))
				{
					fromwhereleft = (HowCrossed)this.left_line.how_crossed(oldcoord, pixelcoord);
					fromwhereright = (HowCrossed)this.right_line.how_crossed(oldcoord, pixelcoord);
					fromwheretop = (HowCrossed)this.top_line.how_crossed(oldcoord, pixelcoord);
					fromwherebottom = (HowCrossed)this.buttom_line.how_crossed(oldcoord, pixelcoord);
				}

				fromwhereleft = (HowCrossed)this.left_line.crossed(pixelcoord);
				fromwhereright = (HowCrossed)this.right_line.crossed(pixelcoord);
				fromwheretop = (HowCrossed)this.top_line.crossed(pixelcoord);
				fromwherebottom = (HowCrossed)this.buttom_line.crossed(pixelcoord);
				//Console.WriteLine(fromwhereleft+" "+ fromwhereright+" "+ fromwheretop+" "+ fromwherebottom);
			
				if(fromwhereright != HowCrossed.none)	
				{	
					if(fromwhereright == HowCrossed.fromleft)																										 																			
						OnCrossing(fromwhereright);
				}
				if(fromwheretop != HowCrossed.none)
				{																																 																			
					OnCrossing(fromwheretop);
				}
				if(fromwherebottom != HowCrossed.none)
				{																																 																			
					OnCrossing(fromwherebottom);
				}
				if(fromwhereleft != HowCrossed.none)
				{
					//Console.WriteLine(fromwhereleft);
					OnCrossing(fromwhereleft);
				}
				oldcoord = pixelcoord;
			}
			else
			{
				this.Cursor = util.CursorSwitcher.change_mouse_cursor(false);
				//Console.Write(".");
			}
		}


		
		
		
	
		
		public void what_is_selected(string []nowselected)
		{
			string [] prevselected = new string[20];
			if(isCheckBox == true)
			{
				// call function with the current value "added" to the last one
			}
			else if(isRadioBox ==true)
			{
				// call function with old value as new value and set
				// old value as false
			}
			prevselected = nowselected;
			

		}
		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics feedback = e.Graphics;
			Pen blackPen = new Pen(Color.Black,3);
			Pen redPen = new Pen(Color.FromArgb(255,090,084),3);
			Pen greenPen = new Pen(Color.FromArgb(128,255,149), 3);
			//feedback.DrawLine(redPen, Location.X, Location.Y, Location.X + Width, Location.Y);
			//feedback.DrawLine(blackPen, coordinate.X, coordinate.Y, coordinate.X +1, coordinate.Y+1);
			feedback.DrawLine(redPen, 0, 0, Width, 0);
			feedback.DrawLine(redPen, Width-3, 0, Width-3, Height);
			feedback.DrawLine(greenPen, 1, Height-3, Width, Height-3);
			feedback.DrawLine(greenPen, 1, 0, 1, Height);
			
		}
		
	}
}
