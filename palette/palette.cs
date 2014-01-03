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
	public class palette: dialoguebox
	{
		
		//private crossy Main;
		
		public class Palette_MoveButton:MoveButton
		{
			

			public Palette_MoveButton(crossy Main, Point original_location):base(Main, original_location)
			{
				
			}
			public override void back_to_original_position()
			{
				Main.Palette.Location = new Point(895 ,10);
				set_initial_position(Main.Palette.Location);
			}
			public override void new_position(Point new_position)
			{
				Main.Palette.Location = new_position;
				set_initial_position(new_position);
			}
			protected override void OnMouseDown(MouseEventArgs e)
			{
				base.OnMouseDown(e);
				set_initial_position(Main.Palette.Location);
			}
		}
	
		public class PenButton: button
		{
			public bool penline_crossed = false;
			//public Color feedback_color = Color.Black;
			public Pen feedbackPen = new Pen(Color.Black, 4);
			//public 
			public override void OnCrossing(HowCrossed fromwhere)
			{
				if (fromwhere == HowCrossed.fromright)
				{
					//
					Main.Palette.penButton.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\penon.gif");
					Main.Palette.ButtonErase.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\eraser.gif");
					Main.Palette.ButtonSelect.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\lasso.gif");
					Main.Palette.ButtonHiLi.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\hili.gif");
					//Main.Palette.ButtonStroke.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\stroke.gif");
					Main.Palette.ButtonFind.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\lupe.gif");
					//Main.Palette.penpanel.Invalidate();
					/*Main.Palette.penpanel.Location = new System.Drawing.Point(Main.Palette.Location.X -155, Main.Palette.Location.Y +10);
					Main.Palette.penpanel.Visible = true;*/
					penline_crossed = true;
					Main.Palette.penpanel.BringToFront();
					Main.Palette.selectpanel.Visible = false;
					Main.Palette.erasepanel.Visible = false;
					Main.Palette.find_replace.Visible = false;
					Main.Palette.hilipanel.Visible = false;
					Main.Palette.penpanel.strokepanel.Visible = false;
				}
			}
			protected override void OnPaint(PaintEventArgs e)
			{
				base.OnPaint(e);
				Point Start = new Point(this.Location.X + 10,this.Location.Y + 25);
				
				Graphics feedback = e.Graphics;
			
				
					//buttonline.DrawLine(buttonlinePen, TopPoint.X, TopPoint.Y, LowPoint.X, LowPoint.Y);
					// top stopper
					feedback.DrawLine(feedbackPen, Start.X, Start.Y, Start.X +20, Start.Y);//(LowPoint.Y - TopPoint.Y)/20);
					
			}
			protected override void OnMouseUp(MouseEventArgs e)
			{
				Main.Palette.penpanel.Visible = false;
				if (penline_crossed == true)
				{
					
					Main.central_TabControl.get_active_TabPanel().changePen(Main.Palette.penpanel.penThickness, Main.Palette.penButton.feedbackPen.Color);
				}
				penline_crossed = false;
			
			}
			protected override void OnMouseLeave(EventArgs e)
			{
				if (penline_crossed == true)
				{
					Main.Palette.penButton.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\penon.gif");
					Main.Palette.ButtonErase.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\eraser.gif");
					Main.Palette.ButtonSelect.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\lasso.gif");
					Main.Palette.ButtonHiLi.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\hili.gif");
					//Main.Palette.ButtonStroke.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\stroke.gif");
					Main.Palette.ButtonFind.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\lupe.gif");
					//Main.Palette.penpanel.Invalidate();
					Main.Palette.penpanel.Location = new System.Drawing.Point(Main.Palette.Location.X -155, Main.Palette.Location.Y +10);
					Main.Palette.penpanel.Visible = true;
					//penline_crossed = true;
					Main.Palette.penpanel.BringToFront();
					Main.Palette.selectpanel.Visible = false;
					Main.Palette.erasepanel.Visible = false;
					Main.Palette.find_replace.Visible = false;
					Main.Palette.hilipanel.Visible = false;
					Main.Palette.penpanel.strokepanel.Visible = false;
				}
				penline_crossed = false;
			}
			
		}
	
		public class eraseButton: button
		{
			private bool erasebutton_crossed = false;
			public override void OnCrossing(HowCrossed fromwhere)
			{
				if (fromwhere == HowCrossed.fromright)
				{
					/*Main.Palette.erasepanel.Location = new System.Drawing.Point(Main.Palette.Location.X -85, Main.Palette.Location.Y +50);
					Main.Palette.erasepanel.Visible = true;
					Main.Palette.erasepanel.BringToFront();*/
					Main.Palette.penButton.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\pen.gif");
					Main.Palette.ButtonErase.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\eraseron.gif");
					Main.Palette.ButtonSelect.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\lasso.gif");
					Main.Palette.ButtonHiLi.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\hili.gif");
					//Main.Palette.ButtonStroke.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\stroke.gif");
					Main.Palette.ButtonFind.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\lupe.gif");
					Main.Palette.penpanel.Visible = false;
					erasebutton_crossed = true;
					Main.Palette.selectpanel.Visible = false;
					Main.Palette.find_replace.Visible = false;
					Main.Palette.hilipanel.Visible = false;
					Main.Palette.penpanel.strokepanel.Visible = false;
				}
			}
			protected override void OnMouseUp(MouseEventArgs e)
			{
				Main.Palette.erasepanel.Visible = false;
				//Main.Palette.erasepanel.erasegroup.unselected(
				if (erasebutton_crossed == true)
				{
					if (Main.Palette.erasepanel.eraserType == penattr.pointerase || Main.Palette.erasepanel.eraserType == penattr.strokeerase)
					{
						
						Main.central_TabControl.get_active_TabPanel().Erase(Main.Palette.erasepanel.eraserType, 2);
						
					}
					else
					{
						
						Main.central_TabControl.get_active_TabPanel().Erase(penattr.pointerase, 2);	
					}
				}
				erasebutton_crossed = false;
			
			}
			protected override void OnMouseLeave(EventArgs e)
			{
				if (erasebutton_crossed == true)
				{
					Main.Palette.erasepanel.Location = new System.Drawing.Point(Main.Palette.Location.X -85, Main.Palette.Location.Y +50);
					Main.Palette.erasepanel.Visible = true;
					Main.Palette.erasepanel.BringToFront();
					Main.Palette.penButton.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\pen.gif");
					Main.Palette.ButtonErase.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\eraseron.gif");
					Main.Palette.ButtonSelect.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\lasso.gif");
					Main.Palette.ButtonHiLi.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\hili.gif");
					//Main.Palette.ButtonStroke.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\stroke.gif");
					Main.Palette.ButtonFind.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\lupe.gif");
					Main.Palette.penpanel.Visible = false;
					//erasebutton_crossed = true;
					Main.Palette.selectpanel.Visible = false;
					Main.Palette.find_replace.Visible = false;
					Main.Palette.hilipanel.Visible = false;
					Main.Palette.penpanel.strokepanel.Visible = false;
				}
				erasebutton_crossed = false;
			}
		}
	
		public class selectButton: button
		{
			private bool selectbutton_crossed;
			public override void OnCrossing(HowCrossed fromwhere)
			{
				if (fromwhere == HowCrossed.fromright)
				{
					Main.Palette.penButton.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\pen.gif");
					Main.Palette.ButtonErase.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\eraser.gif");
					Main.Palette.ButtonSelect.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\lassoon.gif");
					Main.Palette.ButtonHiLi.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\hili.gif");
					//Main.Palette.ButtonStroke.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\stroke.gif");
					Main.Palette.ButtonFind.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\lupe.gif");
					/*Main.Palette.selectpanel.Location = new System.Drawing.Point(Main.Palette.Location.X -85, Main.Palette.Location.Y +130);
					Main.Palette.selectpanel.Visible = true;*/
					selectbutton_crossed = true;
					Main.Palette.selectpanel.BringToFront();
					Main.Palette.penpanel.Visible = false;
					Main.Palette.erasepanel.Visible = false;
					Main.Palette.find_replace.Visible = false;
					Main.Palette.hilipanel.Visible = false;
					Main.Palette.penpanel.strokepanel.Visible = false;
					
				}
			}
			protected override void OnMouseUp(MouseEventArgs e)
			{
				Main.Palette.selectpanel.Visible = false;
				if (selectbutton_crossed == true)
				{
					
					Main.central_TabControl.get_active_TabPanel().mySelect(penattr.selfselect);
				}
				selectbutton_crossed = false;
			
			}
			protected override void OnMouseLeave(EventArgs e)
			{
				if (selectbutton_crossed == true)
				{
					Main.Palette.penButton.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\pen.gif");
					Main.Palette.ButtonErase.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\eraser.gif");
					Main.Palette.ButtonSelect.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\lassoon.gif");
					Main.Palette.ButtonHiLi.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\hili.gif");
					//Main.Palette.ButtonStroke.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\stroke.gif");
					Main.Palette.ButtonFind.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\lupe.gif");
					Main.Palette.selectpanel.Location = new System.Drawing.Point(Main.Palette.Location.X -85, Main.Palette.Location.Y +130);
					Main.Palette.selectpanel.Visible = true;
					//selectbutton_crossed = true;
					Main.Palette.selectpanel.BringToFront();
					Main.Palette.penpanel.Visible = false;
					Main.Palette.erasepanel.Visible = false;
					Main.Palette.find_replace.Visible = false;
					Main.Palette.hilipanel.Visible = false;
					Main.Palette.penpanel.strokepanel.Visible = false;
				}
				selectbutton_crossed = false;
			}
		}
		public class HiLiButton: button
		{
			public bool hilibutton_crossed = false;
			public Pen feedbackHiLi = new Pen(Color.Yellow, 5);
			public override void OnCrossing(HowCrossed fromwhere)
			{
				if (fromwhere == HowCrossed.fromright)
				{	
					/*Main.Palette.hilipanel.Location = new System.Drawing.Point(Main.Palette.Location.X - 85, Main.Palette.Location.Y +150);
					Main.Palette.hilipanel.Visible = true;
					Main.Palette.hilipanel.BringToFront();*/
					Main.Palette.penButton.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\pen.gif");
					Main.Palette.ButtonErase.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\eraser.gif");
					Main.Palette.ButtonSelect.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\lasso.gif");
					Main.Palette.ButtonHiLi.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\hilion.gif");
					//Main.Palette.ButtonStroke.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\stroke.gif");
					Main.Palette.ButtonFind.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\lupe.gif");
					Main.Palette.penpanel.strokepanel.Visible = false;
					hilibutton_crossed = true;
					Main.Palette.selectpanel.Visible = false;
					Main.Palette.penpanel.Visible = false;
					Main.Palette.erasepanel.Visible = false;
					Main.Palette.find_replace.Visible = false;
				}
			}
			protected override void OnPaint(PaintEventArgs e)
			{
				base.OnPaint(e);
				Point Start = new Point(13,47);
				
				Graphics feedback = e.Graphics;
				
					//buttonline.DrawLine(buttonlinePen, TopPoint.X, TopPoint.Y, LowPoint.X, LowPoint.Y);
					// top stopper
					feedback.DrawLine(feedbackHiLi, Start.X, Start.Y, Start.X +20, Start.Y);//(LowPoint.Y - TopPoint.Y)/20);
					
			}
			protected override void OnMouseUp(MouseEventArgs e)
			{
				Main.Palette.hilipanel.Visible = false;
				if (hilibutton_crossed == true)
				{
					
					Main.central_TabControl.get_active_TabPanel().changePen(penattr.highlight, Main.Palette.hilipanel.hilicolor);
				}
				hilibutton_crossed = false;
			
			}
			protected override void OnMouseLeave(EventArgs e)
			{
				if (hilibutton_crossed == true)
				{
					Main.Palette.hilipanel.Location = new System.Drawing.Point(Main.Palette.Location.X - 85, Main.Palette.Location.Y +150);
					Main.Palette.hilipanel.Visible = true;
					Main.Palette.hilipanel.BringToFront();
					Main.Palette.penButton.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\pen.gif");
					Main.Palette.ButtonErase.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\eraser.gif");
					Main.Palette.ButtonSelect.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\lasso.gif");
					Main.Palette.ButtonHiLi.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\hilion.gif");
					//Main.Palette.ButtonStroke.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\stroke.gif");
					Main.Palette.ButtonFind.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\lupe.gif");
					Main.Palette.penpanel.strokepanel.Visible = false;
					//hilibutton_crossed = true;
					Main.Palette.selectpanel.Visible = false;
					Main.Palette.penpanel.Visible = false;
					Main.Palette.erasepanel.Visible = false;
					Main.Palette.find_replace.Visible = false;
				}
				hilibutton_crossed = false;
			}
			
		}
		
		public class find_replaceButton: button
		{
			public override void OnCrossing(HowCrossed fromwhere)
			{
				if (fromwhere == HowCrossed.fromright)
				{
					Main.Palette.find_replace.Location = new System.Drawing.Point(Main.Palette.Location.X-250, Main.Palette.Location.Y +260);
					Main.Palette.find_replace.Visible = true;
					Main.Palette.find_replace.BringToFront();
					Main.Palette.penButton.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\pen.gif");
					Main.Palette.ButtonErase.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\eraser.gif");
					Main.Palette.ButtonSelect.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\lasso.gif");
					Main.Palette.ButtonHiLi.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\hili.gif");
					//Main.Palette.ButtonStroke.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\stroke.gif");
					Main.Palette.ButtonFind.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\lupeon.gif");
					Main.Palette.penpanel.Visible = false;
					Main.Palette.selectpanel.Visible = false;
					Main.Palette.erasepanel.Visible = false;
					Main.Palette.hilipanel.Visible = false;
					Main.Palette.penpanel.strokepanel.Visible = false;
				}
			}
		}
		
		
		public Palette_MoveButton moveButton;
		public PenButton penButton;
		public eraseButton ButtonErase;
		public selectButton  ButtonSelect;
		public HiLiButton ButtonHiLi;
		
		public find_replaceButton ButtonFind;
		
		public penPanel penpanel;
		public erasePanel erasepanel;
		public selectPanel selectpanel;
		public find_replacePanel find_replace;
		public hiliPanel hilipanel;
		
		

		
		public palette(crossy theForm)
		{
			SetStyle( ControlStyles.AllPaintingInWmPaint, true );
			SetStyle( ControlStyles.DoubleBuffer, true );
			SetStyle( ControlStyles.UserPaint, true );
			SetStyle( ControlStyles.ResizeRedraw, true );
			UpdateStyles();
			Main = theForm;

			this.moveButton = new Palette_MoveButton(Main, this.Location);
			this.moveButton.SuspendLayout();
			this.moveButton.Location = new System.Drawing.Point(0, 0);
			this.moveButton.Size = new System.Drawing.Size(60, 20);
			this.moveButton.Text = "button";
			this.moveButton.buttonlinePen = new Pen(Color.Black, 2);
			this.moveButton.stopperPen = new Pen(Color.Black, 4);
			this.moveButton.label = "";
			//this.moveButton.trigger = "pen";
			this.moveButton.font = new Font("Helvetica", 12);
			this.moveButton.BringToFront();
			this.moveButton.BackColor = Color.White;
			this.moveButton.Main = Main;
			this.Controls.Add(this.moveButton);

			this.penButton = new PenButton();
			this.penpanel = new penPanel(this.Main);
			//
			// button
			//
		
			this.penButton.SuspendLayout();
			
			this.penButton.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory + @"\pixs\penon.gif");
			this.penButton.Location = new System.Drawing.Point(0, 20);
			this.penButton.Size = new System.Drawing.Size(60, 60);
			this.penButton.Text = "button";
			this.penButton.buttonlinePen = new Pen(Color.Black, 2);
			this.penButton.stopperPen = new Pen(Color.Black, 4);
			this.penButton.label = "";
			this.penButton.trigger = "pen";
			this.penButton.font = new Font("Helvetica", 12);
			this.penButton.BringToFront();
			this.penButton.BackColor = Color.BlanchedAlmond;
			this.penButton.Main = Main;
			this.Controls.Add(this.penButton);
			//
			// pen panel
			//
			this.penpanel.SuspendLayout();
			this.penpanel.BorderStyle = BorderStyle.FixedSingle;
			this.penpanel.Location = new System.Drawing.Point(this.Location.X - 215, this.Location.Y +10);
			this.penpanel.Name = "button";
			this.penpanel.Size = new System.Drawing.Size(155, 180);
			this.penpanel.Visible = false;
			this.penpanel.BringToFront();
			this.penpanel.Main = Main;
			this.penpanel.trigger = "penpanel";
			this.penpanel.BackColor = Color.White;
			Main.Controls.Add(this.penpanel);

			this.ButtonErase = new eraseButton();
			this.erasepanel = new erasePanel(this.Main);
			//
			// ButtonErase
			//
			//this.panel.Controls.Add(this.Button);
			this.ButtonErase.SuspendLayout();
			this.ButtonErase.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\eraser.gif");
			this.ButtonErase.Location = new System.Drawing.Point(0, 80);
			this.ButtonErase.Name = "button";
			this.ButtonErase.Size = new System.Drawing.Size(60, 60);
			this.ButtonErase.Text = "button";
			this.ButtonErase.buttonlinePen = new Pen(Color.Black, 2);
			this.ButtonErase.stopperPen = new Pen(Color.Black, 4);
			this.ButtonErase.label = "";
			this.ButtonErase.trigger = "";
			this.ButtonErase.font = new Font("Helvetica", 12);
			this.ButtonErase.BringToFront();
			this.ButtonErase.BackColor = Color.BlanchedAlmond;
			this.ButtonErase.Main = Main;
			this.Controls.Add(this.ButtonErase);
			//
			// erasepanel
			//
			this.erasepanel.SuspendLayout();
			this.erasepanel.BorderStyle = BorderStyle.FixedSingle;
			this.erasepanel.Location = new System.Drawing.Point(this.Location.X - 85, 10);
			this.erasepanel.Name = "button";
			this.erasepanel.Size = new System.Drawing.Size(85, 140);
			this.erasepanel.Visible = false;
			this.erasepanel.BringToFront();
			this.erasepanel.Main = Main;
			this.erasepanel.label = "erasepanel";
			this.erasepanel.trigger = "erasepanel";
			this.erasepanel.BackColor = Color.White;
			this.erasepanel.Main = Main;
			Main.Controls.Add(this.erasepanel);
			//
			// select
			//
			this.ButtonSelect = new selectButton();
			this.selectpanel = new selectPanel(this.Main);
			//
			// ButtonSelect
			//
			this.ButtonSelect.SuspendLayout();
			this.ButtonSelect.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\lasso.gif");
			this.ButtonSelect.Location = new System.Drawing.Point(0, 140);
			this.ButtonSelect.Name = "button";
			this.ButtonSelect.Size = new System.Drawing.Size(60, 60);
			this.ButtonSelect.buttonlinePen = new Pen(Color.Black, 2);
			this.ButtonSelect.stopperPen = new Pen(Color.Black, 4);
			this.ButtonSelect.label = "";
			this.ButtonSelect.trigger = "";
			this.ButtonSelect.font = new Font("Helvetica", 12);
			this.ButtonSelect.BringToFront();
			this.ButtonSelect.BackColor = Color.White;
			this.ButtonSelect.Main = Main;
			this.Controls.Add(this.ButtonSelect);
			//
			// selectpanel
			//
			this.selectpanel.SuspendLayout();
			this.selectpanel.BorderStyle = BorderStyle.FixedSingle;
			this.selectpanel.Location = new System.Drawing.Point(this.Location.X -85, 130);
			this.selectpanel.Name = "button";
			this.selectpanel.Size = new System.Drawing.Size(85, 140);
			this.selectpanel.Visible = false;
			this.selectpanel.BringToFront();
			this.selectpanel.Main = Main;
			this.selectpanel.label = "selectpanel";
			this.selectpanel.trigger = "selectpanel";
			this.selectpanel.BackColor = Color.White;
			this.selectpanel.Main = Main;
			Main.Controls.Add(this.selectpanel);
			//
			// hilibutton and hilipanel
			//
			this.ButtonHiLi = new HiLiButton();
			this.hilipanel = new hiliPanel(this.Main);
			//
			// ButtonHiLi
			//
			this.ButtonHiLi.SuspendLayout();
			this.ButtonHiLi.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\hili.gif");
			this.ButtonHiLi.Location = new System.Drawing.Point(0, 200);
			this.ButtonHiLi.Size = new System.Drawing.Size(60, 60);
			this.ButtonHiLi.buttonlinePen = new Pen(Color.Black, 2);
			this.ButtonHiLi.stopperPen = new Pen(Color.Black, 4);
			this.ButtonHiLi.trigger = "hili";
			this.ButtonHiLi.label ="";
			this.ButtonHiLi.font = new Font("Helvetica", 12);
			this.ButtonHiLi.BringToFront();
			this.ButtonHiLi.BackColor = Color.Orange;
			this.ButtonHiLi.Main = Main;
			this.Controls.Add(this.ButtonHiLi);
			//
			// hilipanel
			//
			this.hilipanel.SuspendLayout();
			this.hilipanel.BorderStyle = BorderStyle.FixedSingle;
			this.hilipanel.Location = new System.Drawing.Point( this.Location.X - 85, 90);
			this.hilipanel.Size = new System.Drawing.Size(85, 170);
			this.hilipanel.Visible = false;
			this.hilipanel.BringToFront();
			this.hilipanel.Main = Main;
			this.hilipanel.label = "hili";
			this.hilipanel.BackColor = Color.White;
			this.hilipanel.Main = Main;
			Main.Controls.Add(this.hilipanel);
			
			
			//
			// find and replace
			//
			this.ButtonFind = new find_replaceButton();
			this.find_replace = new find_replacePanel(this.Main);
			//
			// ButtonFind
			//
			this.ButtonFind.SuspendLayout();
			this.ButtonFind.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\lupe.gif");
			this.ButtonFind.Location = new System.Drawing.Point(0, 260);
			this.ButtonFind.Size = new System.Drawing.Size(60, 60);
			this.ButtonFind.buttonlinePen = new Pen(Color.Black, 2);
			this.ButtonFind.stopperPen = new Pen(Color.Black, 4);
			this.ButtonFind.trigger = "find_re";
			this.ButtonFind.label ="";
			this.ButtonFind.font = new Font("Helvetica", 12);
			this.ButtonFind.BringToFront();
			this.ButtonFind.BackColor = Color.White;
			this.ButtonFind.Main = Main;
			this.Controls.Add(this.ButtonFind);
			//
			// find_replace
			//
			this.find_replace.SuspendLayout();
			this.find_replace.BorderStyle = BorderStyle.FixedSingle;
			this.find_replace.Location = new System.Drawing.Point(this.Location.X , 260);
			this.find_replace.Size = new System.Drawing.Size(250, 220);
			this.find_replace.Visible = false;
			this.find_replace.BringToFront();
			this.find_replace.Main = Main;
			this.find_replace.label = "find_replace";
			this.find_replace.trigger = "find_replace";
			this.find_replace.BackColor = Color.White;
			this.find_replace.Main = Main;
			Main.Controls.Add(this.find_replace);
		}
		
		
	}
	
}
