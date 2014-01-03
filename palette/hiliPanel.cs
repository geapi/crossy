using System;
using System.Threading;
using System.Timers;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;


using System.Windows.Forms;
using System.Data;
using Microsoft.Ink;

namespace crossy
{
	
	
 	public class hiliPanel:dialoguebox
	{
		public Color hilicolor = Color.Yellow;
		option_queue current_HiLi;
		option_queue inTransit_HiLi;
		private HiLiColorButton ButtonBlue, ButtonRed, ButtonGreen, ButtonYellow, ButtonOrange;
		private radiogroup hilicolor_group;
		public class HiLiColorButton: optionbox
		{
			hiliPanel container;

			public HiLiColorButton(Color color, hiliPanel container)
			{
				assigned_color = color;
				this.container = container;
				this.SuspendLayout();
				this.Size = new System.Drawing.Size(60, 30);
				this.buttonlinePen = new Pen(Color.Black, 2);
				this.stopperPen = new Pen(Color.Black, 4);
				this.font = new Font("Helvetica", 8);
				this.BackColor = Color.White;
				this.IconPen = new Pen(assigned_color, 6);

				Enabled = false;
			}
			protected override void OnPaint(PaintEventArgs e)
			{
				base.OnPaint(e);
				Graphics feedback = e.Graphics;
				
				// the icon
				feedback.DrawLine(IconPen, 10, this.Height/2 + 2  , this.Width-10, this.Height/2 -2 );	
			}

			public override void OnCrossing(string what, HowCrossed fromwhere, bool selected, optionbox thisopt)
			{
				//Console.WriteLine(selected);

				if (fromwhere == HowCrossed.fromleft || fromwhere == HowCrossed.fromright)
				{
					container.hilicolor = assigned_color;
					container.inTransit_HiLi.HighLighterTyp = thisopt;
					//Main.Palette.penButton.feedback_color = assigned_color;
					
				}
			
					thisopt.myradiogroup.selected(thisopt);
			
				
			
			}
			private Color assigned_color;
		}
		public override void OnCrossing(HowCrossed fromwhere)
		{
			if ((fromwhere == HowCrossed.fromright ||fromwhere == HowCrossed.fromtop))
			{
				// low and right, apply changed values\
				//Console.WriteLine("OK");
				current_HiLi.HighLighterTyp = inTransit_HiLi.HighLighterTyp;
				Main.central_TabControl.get_active_TabPanel().changePen(penattr.highlight, hilicolor);
				//Main.Palette.ButtonHiLi.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\hili.gif");
				Main.Palette.ButtonHiLi.feedbackHiLi = new Pen(hilicolor, 4);
				Main.Palette.ButtonHiLi.Invalidate();
				this.Visible = false;
			}
		
			if ((fromwhere == HowCrossed.frombottom||fromwhere == HowCrossed.fromleft))
			{
				//Console.WriteLine("CANCEL");
				//top and left, discharge changed values
				current_HiLi.HighLighterTyp.myradiogroup.selected(current_HiLi.HighLighterTyp);
				this.Visible = false;
				
			}
			Win32Application.Win32.ReleaseCapture();
		}
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove (e);
			ButtonBlue.OnMouseMove_private_dispatch(new MouseEventArgs(e.Button, e.Clicks, e.X - ButtonBlue.Location.X, e.Y - ButtonBlue.Location.Y, e.Delta));
			ButtonOrange.OnMouseMove_private_dispatch(new MouseEventArgs(e.Button, e.Clicks, e.X - ButtonOrange.Location.X, e.Y - ButtonOrange.Location.Y, e.Delta));
			ButtonRed.OnMouseMove_private_dispatch(new MouseEventArgs(e.Button, e.Clicks, e.X - ButtonRed.Location.X, e.Y - ButtonRed.Location.Y, e.Delta));
			ButtonYellow.OnMouseMove_private_dispatch(new MouseEventArgs(e.Button, e.Clicks, e.X - ButtonYellow.Location.X, e.Y - ButtonYellow.Location.Y, e.Delta));
			ButtonGreen.OnMouseMove_private_dispatch(new MouseEventArgs(e.Button, e.Clicks, e.X - ButtonGreen.Location.X, e.Y - ButtonGreen.Location.Y, e.Delta));
		}
		public hiliPanel(crossy theForm)
		{
			crossy Main = theForm;
			this.ButtonRed = new HiLiColorButton(Color.Red, this);
			//this.ButtonBlue.myradiogroup = this.hilicolor_group;
			this.ButtonBlue = new HiLiColorButton(Color.Blue, this);
			this.ButtonGreen = new HiLiColorButton(Color.Green, this);
			this.ButtonOrange = new HiLiColorButton(Color.OrangeRed, this);
			this.ButtonYellow = new HiLiColorButton(Color.Yellow, this);
			this.hilicolor_group = new radiogroup();
			//
			// ButtonRed on hilipanel
			//
			this.Controls.Add(this.ButtonRed);
			this.ButtonRed.myradiogroup = this.hilicolor_group;
			this.ButtonRed.Location = new System.Drawing.Point(this.Location.X + 10, this.Location.Y +10);
			this.ButtonRed.Text = "button";
			this.ButtonRed.label = "";
			this.ButtonRed.trigger = "red";
			this.ButtonRed.BringToFront();
			this.ButtonRed.Main = Main;
			//
			// ButtonYellow on hilipanel
			//
			this.Controls.Add(this.ButtonYellow);
			this.ButtonYellow.myradiogroup = this.hilicolor_group;
			this.ButtonYellow.Location = new System.Drawing.Point(this.ButtonRed.Location.X, this.ButtonRed.Location.Y + this.ButtonRed.Height);
			this.ButtonYellow.Text = "button";
			this.ButtonYellow.label = "";
			this.ButtonYellow.trigger = "yellow";
			this.ButtonYellow.BringToFront();
			ButtonYellow.myradiogroup.selected(ButtonYellow);
			this.ButtonYellow.Main = Main;
			current_HiLi.HighLighterTyp = ButtonYellow;
			//
			// ButtonBlue on hilipanel
			//
			this.Controls.Add(this.ButtonBlue);
			this.ButtonBlue.myradiogroup = this.hilicolor_group;
			this.ButtonBlue.Location = new System.Drawing.Point(this.ButtonRed.Location.X, this.ButtonYellow.Location.Y + this.ButtonYellow.Height);
			this.ButtonBlue.label = "";
			this.ButtonBlue.trigger = "blue";
			this.ButtonBlue.BringToFront();
			this.ButtonBlue.Main = Main;
			//
			// ButtonOrange on hilipanel
			//
			this.Controls.Add(this.ButtonOrange);
			this.ButtonOrange.myradiogroup = this.hilicolor_group;
			this.ButtonOrange.Location = new System.Drawing.Point(this.ButtonRed.Location.X,this.ButtonBlue.Location.Y + this.ButtonBlue.Height);
			this.ButtonOrange.label = "";
			this.ButtonOrange.trigger = "green";
			this.ButtonOrange.BringToFront();
			this.ButtonOrange.Main = Main;
			//
			// ButtonGreen on hilipanel
			//
			this.Controls.Add(this.ButtonGreen);
			this.ButtonGreen.myradiogroup = this.hilicolor_group;
			this.ButtonGreen.Location = new System.Drawing.Point(this.ButtonRed.Location.X,this.ButtonOrange.Location.Y + this.ButtonOrange.Height);
			this.ButtonGreen.label = "";
			this.ButtonGreen.trigger = "";
			this.ButtonGreen.BringToFront();
			this.ButtonGreen.Main = Main;
			


		}
	}
	

}
