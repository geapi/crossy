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
	

	public class nomogram:dialoguebox
	{
		//crossy Main;
		public int color, strokethickness, saturation;
		
		
		public class Nomo_Color_Button: button
		{
			public Nomo_Color_Button()
			{
				this.Text = "button";
				this.buttonlinePen = new Pen(Color.Black, 2);
				this.stopperPen = new Pen(Color.Transparent, 4);
				this.label = "";
				this.font = new Font("Verdana", 10);
			}

			public override void OnCrossing(HowCrossed fromwhere)
			{
				if (fromwhere == HowCrossed.fromleft  || fromwhere == HowCrossed.fromright )
				{
					this.Invalidate();
					//Console.Write("Color"+base.cross_position.Y);
					Main.NomoGram.color =  base.cross_position.Y;
					this.Invalidate();
					Main.central_TabControl.get_active_TabPanel().nomographic_ChangePen(Main.NomoGram.color, Main.NomoGram.strokethickness);
					Main.NomoGram.set_backgroundimages();
				}
	
			}
			protected override void	OnPaint(PaintEventArgs e)
			{
				base.OnPaint(e);
				Graphics feedback = e.Graphics;
				Pen blackPen = new Pen(Color.Black, 4);


				feedback.DrawLine(blackPen, 0, Main.NomoGram.color, 5, Main.NomoGram.color);
				feedback.DrawLine(blackPen, 15, Main.NomoGram.color, 20, Main.NomoGram.color);
			}
			protected override void OnMouseDown(MouseEventArgs e)
			{
				base.OnMouseDown (e);
				this.Main.Palette.find_replace.Visible = false;
			}
	
		}
		public class Nomo_Stroke_Button: button
		{
			public Nomo_Stroke_Button()
			{
				this.Text = "button";
				this.buttonlinePen = new Pen(Color.Black, 4);
				this.stopperPen = new Pen(Color.Transparent, 4);
				this.label = "";
				this.font = new Font("Verdana", 10);
			}
			public override void OnCrossing(HowCrossed fromwhere)
			{
				if (fromwhere == HowCrossed.fromleft  || fromwhere == HowCrossed.fromright )
				{
					//Console.Write("stroke"+base.cross_position.Y);
					this.Invalidate();
					Main.NomoGram.strokethickness =  base.cross_position.Y;
					this.Invalidate();
					Main.central_TabControl.get_active_TabPanel().nomographic_ChangePen(Main.NomoGram.color, Main.NomoGram.strokethickness);
					Main.NomoGram.set_backgroundimages();
				}
			}
			protected override void	OnPaint(PaintEventArgs e)
			{
				base.OnPaint(e);
				Graphics feedback = e.Graphics;
				Pen blackPen = new Pen(Color.Black, 4);
				Pen whitePen = new Pen(Color.White, 2);
				feedback.DrawLine(blackPen, 0,Main.NomoGram.strokethickness, 5, Main.NomoGram.strokethickness);
				feedback.DrawLine(whitePen, 5, Main.NomoGram.strokethickness, 15, Main.NomoGram.strokethickness);
				feedback.DrawLine(blackPen, 15, Main.NomoGram.strokethickness, 21, Main.NomoGram.strokethickness);
			}
		
			protected override void OnMouseDown(MouseEventArgs e)
			{
				base.OnMouseDown (e);
				this.Main.Palette.find_replace.Visible = false;
			}



			
			
		}
		
		public class Nomo_Saturation_Button: button
		{
			
			public Nomo_Saturation_Button()
			{
				this.Text = "button";
				this.buttonlinePen = new Pen(Color.Black, 2);
				this.stopperPen = new Pen(Color.Transparent, 4);
				this.label = "";
				this.font = new Font("Verdana", 10);
			}

			public override void OnCrossing(HowCrossed fromwhere)
			{
				if (fromwhere == HowCrossed.fromleft  || fromwhere == HowCrossed.fromright )
				{
				
					//Console.Write("saturation"+base.cross_position.X);
					Main.NomoGram.saturation =  base.cross_position.Y;
				}
			}
			protected override void OnMouseDown(MouseEventArgs e)
			{
				//Console.Write("v");
				Win32Application.Win32.ReleaseCapture();
				this.Main.Palette.find_replace.Visible = false;
				//this.Capture = false;
			}
			protected override void OnGotFocus(EventArgs e)
			{
				//Console.Write("F");
			}
			protected override void OnMouseEnter(EventArgs e)
			{
				
			}
		}
		
		public class Nomogram_MoveButton:MoveButton
		{
			public Nomogram_MoveButton(crossy Main, Point original_position):base(Main,original_position)
			{
		
			}
	
			public override void back_to_original_position()
			{
				Main.NomoGram.Location = new System.Drawing.Point(0 , 10);
				set_initial_position(Main.NomoGram.Location);
			}
			public override void new_position(Point new_position)
			{
				Main.NomoGram.Location = new_position;
				set_initial_position(new_position);
			}
			protected override void OnMouseDown(MouseEventArgs e)
			{
				base.OnMouseDown(e);
				set_initial_position(Main.NomoGram.Location);
				this.Main.Palette.find_replace.Visible = false;
				
			}
			
		}
	
		//crossy Main;



		public Nomo_Color_Button ButtonColor;
		public Nomo_Stroke_Button ButtonThickness;


		private Nomo_Saturation_Button ButtonSaturation;
		private Nomogram_MoveButton move_nomogram_button;

		private void set_backgroundimages()
		{
			/*Main.Palette.penpanel.current_PenOptions.color.myradiogroup.unselected(Main.Palette.penpanel.current_PenOptions.color);
			Main.Palette.penpanel.current_PenOptions.thickness.myradiogroup.unselected(Main.Palette.penpanel.current_PenOptions.thickness);
			Main.Palette.penpanel.current_PenOptions.thickness = null;
			Main.Palette.penpanel.current_PenOptions.color = null;
			Main.Palette.penpanel.Invalidate();*/

			Main.Palette.penButton.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\penon.gif");
			Main.Palette.ButtonErase.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\eraser.gif");
			Main.Palette.ButtonSelect.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\lasso.gif");
			Main.Palette.ButtonHiLi.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\hili.gif");
			Main.Palette.ButtonFind.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\lupe.gif");
		}
		

		public override void OnCrossing(HowCrossed fromwhere)
		{
			//Console.WriteLine(fromwhere);

			if ((fromwhere == HowCrossed.fromleft ||fromwhere == HowCrossed.fromtop))
			{
				Main.Palette.penButton.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\penon.gif");
				Main.Palette.ButtonErase.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\eraser.gif");
				Main.Palette.ButtonSelect.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\lasso.gif");
				Main.Palette.ButtonHiLi.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\hili.gif");
				Main.Palette.ButtonFind.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\lupe.gif");
			}
			if ((fromwhere == HowCrossed.frombottom || fromwhere == HowCrossed.fromright))
			{
				
			}
			
		}
		
		public nomogram(crossy theForm)
		{
			Main = theForm;
			
			this.ButtonColor = new Nomo_Color_Button();
			this.ButtonThickness = new Nomo_Stroke_Button();
			this.ButtonSaturation = new Nomo_Saturation_Button();

			//
			// move it
			//
			this.move_nomogram_button = new Nomogram_MoveButton(Main,this.Location);
			this.move_nomogram_button.SuspendLayout();
			this.move_nomogram_button.Location = new System.Drawing.Point(0,0);
			this.move_nomogram_button.Size = new System.Drawing.Size(90,20);
			this.move_nomogram_button.Main = Main;
			this.Controls.Add(this.move_nomogram_button);
			//
			// color
			//
			this.Controls.Add(this.ButtonColor);
			this.ButtonColor.SuspendLayout();
			this.ButtonColor.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\rainbow2.gif");
			this.ButtonColor.Location = new System.Drawing.Point(15, 22);
			this.ButtonColor.Size = new System.Drawing.Size(20, 150);
			this.ButtonColor.BringToFront();
			this.ButtonColor.Main = Main;
			//
			// thickness
			//
			
			this.Controls.Add(this.ButtonThickness);
			this.ButtonThickness.SuspendLayout();
			this.ButtonThickness.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\strokethickness.gif");
			this.ButtonThickness.Location = new System.Drawing.Point(52, 22);
			this.ButtonThickness.Size = new System.Drawing.Size(20, 150);
			this.ButtonThickness.BringToFront();
			this.ButtonThickness.Main = Main;
			//
			// saturation
			//
			
			//this.Controls.Add(this.ButtonSaturation);
			this.ButtonSaturation.SuspendLayout();
			this.ButtonSaturation.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\saturation2.gif");
			this.ButtonSaturation.Location = new System.Drawing.Point(90, 5);
			this.ButtonSaturation.Size = new System.Drawing.Size(20, 150);
			this.ButtonSaturation.BringToFront();
			this.ButtonSaturation.Main = Main;
			
			
		}
		protected override void OnPaint(PaintEventArgs e)
		{
			
		}
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			this.Main.Palette.find_replace.Visible = false;
			Win32Application.Win32.ReleaseCapture();

		}

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave (e);
			Win32Application.Win32.ReleaseCapture();
		}
		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter (e);
			Win32Application.Win32.ReleaseCapture();
		}
	}


}
