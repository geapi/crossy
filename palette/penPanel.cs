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
	

	public class penPanel:dialoguebox
	{
		public class StrokeButton: optionbox
		{
			//public Pen IconPen = new Pen(Color.Black, 5);
			penPanel container;
			public StrokeButton(penattr penStyle, penPanel container)
			{
				Enabled = false;
				assigned_penStyle = penStyle;
				this.container = container;
				this.Size = new System.Drawing.Size(60, 30);
				this.buttonlinePen = new Pen(Color.Black, 2);
				this.stopperPen = new Pen(Color.Black, 4);
				this.BackColor = Color.White;
				this.font = new Font("Verdana", 8);
				
				
			}

			public override void OnCrossing(string what, HowCrossed fromwhere, bool selected, optionbox thisopt)
			{
				if (fromwhere == HowCrossed.fromleft || fromwhere == HowCrossed.fromright)
				{
					//Main.Palette.penpanel.old_penThickness = Main.Palette.penpanel.penThickness;
			
					container.inTransit_PenOptions.thickness = thisopt;
					container.penThickness = assigned_penStyle;
				}
				if(this.Active)
				{
					//thisopt.myradiogroup.selected(thisopt);
			
				}
				if(!this.Active)
				{
					thisopt.myradiogroup.selected(thisopt);
				}
				
				
			}
			protected override void OnMouseUp(MouseEventArgs e)
			{
				//Main.Palette.penpanel.Visible = false;
			
			}
			protected override void OnMouseMove(MouseEventArgs e)
			{
				base.OnMouseMove (e);
				//container.OnMouseMove(new MouseEventArgs(e.Button, e.Clicks, e.X - container.Location.X, e.Y - container.Location.Y, e.Delta));
				
			}

			protected override void OnPaint(PaintEventArgs e)
			{
				base.OnPaint(e);
				//Pen IconPen = new Pen(Color.Black, 5);
				Graphics feedback = e.Graphics;
				
					// the icon
					feedback.DrawLine(IconPen, 10, this.Height/2 + 2  , this.Width-10, this.Height/2 -2 );	
			}
			
			private penattr assigned_penStyle;
		}
		public class ColorButton: optionbox
		{
			//public Pen IconPen = new Pen(Color.Black, 5);
			penPanel container;
			public ColorButton(Color color, penPanel container)
			{
				Enabled = false;
				assigned_color = color;
				this.container = container;
				this.Size = new System.Drawing.Size(60, 30);
				this.brushcolor = Color.White;
				this.label = "";
				this.buttonlinePen = new Pen(Color.White, 2);
				this.stopperPen = new Pen(Color.Black, 4);
				this.font = new Font("Verdana", 8);
				
				this.IconPen = new Pen(assigned_color, 6);
			}
			protected override void OnPaint(PaintEventArgs e)
			{
				base.OnPaint(e);
				Graphics feedback = e.Graphics;
				
					// the icon
					feedback.DrawLine(IconPen, 10, this.Height/2 + 2  , this.Width-10, this.Height/2 -2 );	
			}
			protected override void OnMouseMove(MouseEventArgs e)
			{
				base.OnMouseMove (e);
				//container.OnMouseMove(new MouseEventArgs(e.Button, e.Clicks, e.X - container.Location.X, e.Y - container.Location.Y, e.Delta));
				
			}

			public override void OnCrossing(string what, HowCrossed fromwhere, bool selected, optionbox thisopt)
			{
				//Console.WriteLine(selected);

				if (fromwhere == HowCrossed.fromleft || fromwhere == HowCrossed.fromright)
				{
					container.inTransit_PenOptions.color = thisopt;
					container.penColor = assigned_color;
					//Main.Palette.penButton.feedback_color = assigned_color;
					
				}
				if(this.Active)
				{
					//thisopt.myradiogroup.selected(thisopt);
			
				}
				if(!this.Active)
				{
					thisopt.myradiogroup.selected(thisopt);
				}
			}
			protected override void OnMouseUp(MouseEventArgs e)
			{
				//Main.Palette.penpanel.Visible = false;
			
			}
			private Color assigned_color;
		}
		
		public class strokeButton: button
		{
			penPanel container;
			public strokeButton(penPanel container)
			{
				this.container = container;
				Enabled = false;
			}
			public override void OnCrossing(HowCrossed fromwhere)
			{
				if (fromwhere == HowCrossed.fromright || fromwhere == HowCrossed.fromleft)
				{
					
					container.strokepanel.Location = new System.Drawing.Point(Main.Palette.penpanel.Location.X - 65, Main.Palette.penpanel.Location.Y+ 110);
					container.strokepanel.Visible = true;
					container.strokepanel.BringToFront();
					//Main.Palette.penButton.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\pen.gif");
					//Main.Palette.ButtonErase.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\eraser.gif");
					//Main.Palette.ButtonSelect.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\lasso.gif");
					//Main.Palette.ButtonHiLi.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\hili.gif");
					container.ButtonStroke.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\strokeon.gif");
					//Main.Palette.ButtonFind.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\lupe.gif");
					//Main.Palette.hilipanel.Visible = false;
					//Main.Palette.selectpanel.Visible = false;
					//Main.Palette.penpanel.Visible = false;
					//Main.Palette.erasepanel.Visible = false;
					//Main.Palette.find_replace.Visible = false;
					container.releaseCapture();

				}
			}
			protected override void OnMouseUp(MouseEventArgs e)
			{
				//Main.Palette.penpanel.Visible = false;
			
			}
		}
		

		public  Color penColor = Color.Black;
		public penattr penThickness = penattr.medium;
		public penattr old_penThickness;
		public optionbox old_color_option;
		public optionbox old_stroke_option;
		public option_queue current_PenOptions;
		option_queue inTransit_PenOptions;
		public Color old_penColor;
		public override void OnCrossing(HowCrossed fromwhere)
		{
			//Console.WriteLine(fromwhere);
			// handle the leaving of the pen-dialogue box
			//
			if ((fromwhere == HowCrossed.fromleft ||fromwhere == HowCrossed.frombottom))
			{
				//Console.WriteLine("CANCEL");
				//top and left, discharge changed values
				//Main.Palette.penButton.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\pen.gif");
				
				
				//inTransit_PenOptions.color.myradiogroup.unselected(inTransit_PenOptions.color);
				//inTransit_PenOptions.thickness.myradiogroup.unselected(inTransit_PenOptions.thickness);
				Main.Palette.penpanel.strokepanel.Invalidate();
				try
				{
					current_PenOptions.thickness.myradiogroup.selected(current_PenOptions.thickness);
					current_PenOptions.color.myradiogroup.selected(current_PenOptions.color);
				}
				catch(Exception e)
				{
					Console.WriteLine("no previous value" + e.ToString());
				}
				Main.Palette.penpanel.strokepanel.Invalidate();

				
				this.Visible = false;
				Main.Palette.penpanel.strokepanel.Visible = false;
				
			}
			if ((fromwhere == HowCrossed.fromtop||fromwhere == HowCrossed.fromright))
			{
				// low and right, apply changed values\
				//Console.WriteLine("OK");
				current_PenOptions.thickness = inTransit_PenOptions.thickness;
				current_PenOptions.color = inTransit_PenOptions.color;
				Main.central_TabControl.get_active_TabPanel().changePen(penThickness, penColor);
				//Main.Palette.penButton.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\pen.gif");
				if (penColor == Color.Red)
					this.Main.NomoGram.color = 145;
				if(penColor == Color.Blue)
					this.Main.NomoGram.color = 45;
				if(penColor == Color.Black)
					this.Main.NomoGram.color = 0;
				if(penColor == Color.Green)
					this.Main.NomoGram.color = 100;
				if(penColor == Color.Yellow)
					this.Main.NomoGram.color = 125;

				this.Main.NomoGram.ButtonColor.Invalidate();

				if(penThickness == penattr.small)
					this.Main.NomoGram.strokethickness = 135;
				if(penThickness == penattr.medium)
					this.Main.NomoGram.strokethickness = 110;
				if(penThickness == penattr.thick)
					this.Main.NomoGram.strokethickness = 90;

				this.Main.NomoGram.ButtonThickness.Invalidate();

				this.Visible = false;
				Main.Palette.penpanel.strokepanel.Visible = false;
				Main.Palette.penButton.feedbackPen = new Pen(penColor, (int)penThickness/20);
				Main.Palette.penButton.Invalidate();
				
			}
			Win32Application.Win32.ReleaseCapture();
		}
		protected override void OnMouseUp(MouseEventArgs e)
		{
			//this.Visible = false;
			
		}
		
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove (e);
			ButtonBlue.OnMouseMove_private_dispatch(new MouseEventArgs(e.Button, e.Clicks, e.X - ButtonBlue.Location.X, e.Y - ButtonBlue.Location.Y, e.Delta));
			ButtonBlack.OnMouseMove_private_dispatch(new MouseEventArgs(e.Button, e.Clicks, e.X - ButtonBlack.Location.X, e.Y - ButtonBlack.Location.Y, e.Delta));
			ButtonRed.OnMouseMove_private_dispatch(new MouseEventArgs(e.Button, e.Clicks, e.X - ButtonRed.Location.X, e.Y - ButtonRed.Location.Y, e.Delta));
			ButtonYellow.OnMouseMove_private_dispatch(new MouseEventArgs(e.Button, e.Clicks, e.X - ButtonYellow.Location.X, e.Y - ButtonYellow.Location.Y, e.Delta));
			ButtonGreen.OnMouseMove_private_dispatch(new MouseEventArgs(e.Button, e.Clicks, e.X - ButtonGreen.Location.X, e.Y - ButtonGreen.Location.Y, e.Delta));

			ButtonBig.OnMouseMove_private_dispatch(new MouseEventArgs(e.Button, e.Clicks, e.X - ButtonBig.Location.X, e.Y - ButtonBig.Location.Y, e.Delta));
			ButtonMed.OnMouseMove_private_dispatch(new MouseEventArgs(e.Button, e.Clicks, e.X - ButtonMed.Location.X, e.Y - ButtonMed.Location.Y, e.Delta));
			ButtonSmall.OnMouseMove_private_dispatch(new MouseEventArgs(e.Button, e.Clicks, e.X - ButtonSmall.Location.X, e.Y - ButtonSmall.Location.Y, e.Delta));

			ButtonStroke.OnMouseMove_private_dispatch(new MouseEventArgs(e.Button, e.Clicks, e.X - ButtonStroke.Location.X, e.Y - ButtonStroke.Location.Y, e.Delta));

		}

		private ColorButton ButtonBlue, ButtonRed, ButtonGreen, ButtonBlack, ButtonYellow;
		private StrokeButton ButtonBig, ButtonMed, ButtonSmall;
		private radiogroup strokewidth, color;
		public strokeButton ButtonStroke;
		public strokePanel strokepanel;
		public void releaseCapture()
		{
			Win32Application.Win32.ReleaseCapture();
			Console.WriteLine("rF");
		}

		public penPanel(crossy theForm)
		{
			Main = theForm;
			this.strokewidth = new radiogroup();
			this.color = new radiogroup();
			this.ButtonRed = new ColorButton(Color.Red, this);
			this.ButtonBlue = new ColorButton(Color.Blue, this);
			this.ButtonGreen = new ColorButton(Color.Green, this);
			this.ButtonBlack = new ColorButton(Color.Black, this);
			this.ButtonYellow = new ColorButton(Color.Yellow, this);
			this.ButtonBig = new StrokeButton(penattr.thick, this);
			this.ButtonMed = new StrokeButton(penattr.medium, this);
			this.ButtonSmall = new StrokeButton(penattr.small, this);

			current_PenOptions.color = ButtonBlack;
			current_PenOptions.thickness = ButtonMed;
			

			
			//
			// ButtonBlack on Penpanel
			//
			this.Controls.Add(this.ButtonBlack);
			this.ButtonBlack.SuspendLayout();
			this.ButtonBlack.myradiogroup = this.color;
			this.ButtonBlack.Location = new System.Drawing.Point(this.Location.X + 15, this.Location.Y + 10);
			this.ButtonBlack.Text = "button";
			this.ButtonBlack.Name = "Black";
			this.ButtonBlack.trigger = "black";
			this.ButtonBlack.myradiogroup.selected(ButtonBlack);
			this.ButtonBlack.BringToFront();
			this.ButtonBlack.Main = Main;
			//
			// ButtonRed on Penpanel
			//
			this.Controls.Add(this.ButtonRed);
			this.ButtonRed.SuspendLayout();
			this.ButtonRed.myradiogroup = this.color;
			this.ButtonRed.Location = new System.Drawing.Point(this.ButtonBlack.Location.X ,this.ButtonBlack.Location.Y + this.ButtonBlack.Height);
			this.ButtonRed.trigger = "red";
			this.ButtonRed.Name = "Red";
			this.ButtonRed.BringToFront();
			this.ButtonRed.Main = Main;
			//
			// ButtonYellow on Penpanel
			//
			this.Controls.Add(this.ButtonYellow);
			this.ButtonYellow.myradiogroup = this.color;
			this.ButtonYellow.SuspendLayout();
			this.ButtonYellow.Location = new System.Drawing.Point(this.ButtonBlack.Location.X , this.ButtonRed.Location.Y + this.ButtonRed.Height);
			this.ButtonYellow.trigger = "yellow";
			this.ButtonYellow.Name = "Yellow";
			this.ButtonYellow.BringToFront();
			this.ButtonYellow.Main = Main;
			//
			// ButtonBlue on Penpanel
			//
			this.Controls.Add(this.ButtonBlue);
			this.ButtonBlue.myradiogroup = this.color;
			this.ButtonBlue.SuspendLayout();
			this.ButtonBlue.Location = new System.Drawing.Point(this.ButtonBlack.Location.X ,this.ButtonYellow.Location.Y + this.ButtonYellow.Height);
			this.ButtonBlue.trigger = "blue";
			this.ButtonBlack.Name = "Blue";
			this.ButtonBlue.BringToFront();
			this.ButtonBlue.Main = Main;
			//
			// ButtonGreen on Penpanel
			//
			this.Controls.Add(this.ButtonGreen);
			this.ButtonGreen.myradiogroup = this.color;
			this.ButtonGreen.SuspendLayout();
			this.ButtonGreen.Location = new System.Drawing.Point(this.ButtonBlack.Location.X ,this.ButtonBlue.Location.Y + this.ButtonBlue.Height);
			this.ButtonGreen.trigger = "green";
			ButtonGreen.Name = "Green";
			this.ButtonGreen.BringToFront();
			this.ButtonGreen.Main = Main;
			//
			// ButtonBig on Penpanel
			//
			this.Controls.Add(this.ButtonBig);
			this.ButtonBig.myradiogroup = this.strokewidth;
			this.ButtonBig.SuspendLayout();
			this.ButtonBig.Location = new System.Drawing.Point(this.Location.X + 80, this.Location.Y + 10);
			this.ButtonBig.IconPen = new Pen(Color.Black, 6);
			this.ButtonBig.trigger = "thick";
			ButtonBig.Name = "thick";
			this.ButtonBig.BringToFront();
			this.ButtonBig.Main = Main;
			//
			// ButtonMed on Penpanel
			//
			this.Controls.Add(this.ButtonMed);
			this.ButtonMed.myradiogroup = this.strokewidth;
			this.ButtonMed.SuspendLayout();
			this.ButtonMed.Location = new System.Drawing.Point(this.ButtonBig.Location.X,this.ButtonBig.Location.Y + this.ButtonBig.Height);
			this.ButtonMed.trigger = "med";
			this.ButtonMed.IconPen = new Pen(Color.Black, 4);
			ButtonMed.Name = "medium";
			ButtonMed.myradiogroup.selected(ButtonMed);
			this.ButtonMed.BringToFront();
			this.ButtonMed.Main = Main;
			
			//
			// ButtonSmall on Penpanel
			//
			this.Controls.Add(this.ButtonSmall);
			this.ButtonSmall.SuspendLayout();
			this.ButtonSmall.myradiogroup = this.strokewidth;
			this.ButtonSmall.Location = new System.Drawing.Point(this.ButtonBig.Location.X,this.ButtonMed.Location.Y + this.ButtonMed.Height);
			this.ButtonSmall.IconPen = new Pen(Color.Black, 2);
			this.ButtonSmall.trigger = "thin";
			ButtonSmall.Name = "small";
			this.ButtonSmall.BringToFront();
			this.ButtonSmall.Main = Main;
			//
			// strokebutton and strokepanel
			//
			this.ButtonStroke = new strokeButton(this);
			this.strokepanel = new strokePanel(this.Main);
			//
			// ButtonStroke
			//
			this.ButtonStroke.SuspendLayout();
			this.ButtonStroke.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\stroke.gif");
			this.ButtonStroke.Location = new System.Drawing.Point(this.ButtonBig.Location.X, this.ButtonSmall.Location.Y + this.ButtonSmall.Height + 7);
			this.ButtonStroke.Size = new System.Drawing.Size(60, 60);
			this.ButtonStroke.buttonlinePen = new Pen(Color.Black, 2);
			this.ButtonStroke.stopperPen = new Pen(Color.Black, 4);
			this.ButtonStroke.trigger = "hili";
			this.ButtonStroke.label ="";
			this.ButtonStroke.font = new Font("Helvetica", 12);
			this.ButtonStroke.BringToFront();
			this.ButtonStroke.BackColor = Color.White;
			this.ButtonStroke.Main = Main;
			this.Controls.Add(this.ButtonStroke);
			//
			// strokepanel
			//
			this.strokepanel.SuspendLayout();
			this.strokepanel.BorderStyle = BorderStyle.FixedSingle;
			this.strokepanel.Location = new System.Drawing.Point(this.Location.X-47 , this.ButtonStroke.Location.Y);
			this.strokepanel.Size = new System.Drawing.Size(145,120);
			this.strokepanel.Visible = false;
			this.strokepanel.BringToFront();
			this.strokepanel.Main = Main;
			this.strokepanel.label = "hili";
			this.strokepanel.Main = Main;
			this.strokepanel.BackColor = Color.Orange;
			this.Main.Controls.Add(this.strokepanel);
		}
	}



}
