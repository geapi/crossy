using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Microsoft.Ink;






namespace crossy
{

	public struct pen_values
	{
		public Color find_color;
		public penattr find_thickness;
		public Color replace_color;
		public penattr replace_thickness;
		public bool find_crossed;
		public bool replace_crossed;
		public bool reverse_find_crossed;
		public bool undo_replace_crossed;
		
	}
	public struct option_queue
	{
		public optionbox color;
		public optionbox thickness;
		public optionbox EraserTyp;
		public optionbox HighLighterTyp;
		public optionbox SelectType;
	}
	public enum penattr:int
	{
		thick = 110,
		medium = 80,
		small = 20,
		highlight = 4,
		pointerase = 2,
		strokeerase = 6,
		selecterase = 14,
		allerase = 7,
		allselect = 8,
		selfselect = 9,
		pressuresensitive = 10,
		dotedline = 11,
		antialiased =12,
		fittocurve = 13,
		none
	}
	public enum LineOrientation:int
	{
		vertical = 1,
		horizontal = 2,
		tilted_right = 3,
		tilted_left = 4

	}

	public enum HowCrossed:int
	{
		fromright = 1 ,
		fromleft = 2 ,
		fromtop = 3,
		frombottom = 4,
		none = 0
	}
	
	
	public class crossy : System.Windows.Forms.Form 
	{
		public containerPanel panel;
		public palette Palette;
		public slider Slider; 
		public Point pencoords;
		public flowmenu FlowMenu;
		public nomogram NomoGram;
		public dispatcher dispatch;
		
		Point previous = new Point();
		Point current = new Point();
		/// <summary>
		/// the central institution to control the tab-panels
		/// </summary>
		public C_TabControl central_TabControl;
		/// <summary>
		/// constructor for CrossY
		/// takes nothing, sets the basic environment
		/// slider, palette, nomogram, FlowMenu and tab-panels are intinalized here
		/// </summary>
		public crossy()
		{

			this.MaximizeBox = false;
			this.panel = new containerPanel();
			this.Slider = new slider();
			this.central_TabControl = new C_TabControl(this);
			this.panel.SuspendLayout();
			this.SuspendLayout();

			
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(992, 700); //773

			// 
			// panel, that holds the tab-panels, so that we can access the scrolling
			// and over paint the scrollbar, at some point the scrollbar class should be overwritten
			// 
			this.panel.AutoScroll = true;
			this.panel.Location = new System.Drawing.Point(0, 0);
			//this.panel.AutoScrollMinSize = new System.Drawing.Size(10, 20);
			this.panel.Name = "panel";
			this.panel.Size = new System.Drawing.Size(995,695);
			this.panel.TabIndex = 0;
			//
			// the tab Control
			//
			this.central_TabControl.SuspendLayout();
			this.central_TabControl.Location = new System.Drawing.Point(0,this.Height-60);
			this.central_TabControl.Size = new System.Drawing.Size(this.Width-43, 40);
			//this.central_TabControl.panelheight = this.Drawpanel.Height;
			//this.central_TabControl.container = panel;
			this.central_TabControl.BringToFront();
			this.Controls.Add(this.central_TabControl);
			//
			// slider
			//
			this.Slider.SuspendLayout();
			this.Slider.Location = new System.Drawing.Point(this.Width-43, 0); //-43
			this.Slider.Name = "slider";
			this.Slider.Size = new System.Drawing.Size(40, this.Height);
			this.Slider.Text = "slider";
			this.Slider.Main = this;
			this.Slider.panelheight = this.central_TabControl.get_active_TabPanel().Height;
			this.Slider.container = panel;
			this.Slider.BringToFront();
			this.Controls.Add(this.Slider);
			//
			// the menu palette
			//
			this.Palette = new palette(this);
			//this.Palette.SuspendLayout();
			this.Palette.BorderStyle = BorderStyle.FixedSingle;
			this.Palette.Location = new System.Drawing.Point( 895 ,10);
			this.Palette.Size = new System.Drawing.Size(60, 320);
			this.Palette.BringToFront();
			this.Controls.Add(this.Palette);
			//this.Palette.ResumeLayout(false);

			this.FlowMenu = new flowmenu(this);
			this.FlowMenu.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\flowmenuback.gif");
			this.FlowMenu.SuspendLayout();
			this.FlowMenu.Location = new System.Drawing.Point(0 ,0);
			this.FlowMenu.Size = new System.Drawing.Size(250, 250);
			//this.FlowMenu.BackColor = Color.Transparent;
			
			this.FlowMenu.Visible = false;
			this.FlowMenu.Main = this;
			
			this.FlowMenu.BringToFront();
			this.Controls.Add(this.FlowMenu);
			//k
			// the nomography
			//
			this.NomoGram = new nomogram(this);
			this.NomoGram.SuspendLayout();
			this.NomoGram.BorderStyle = BorderStyle.FixedSingle;
			this.NomoGram.BackColor = Color.White;
			//this.NomoGram.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\nomogram.gif"); //nomogram2.jpg

				this.NomoGram.Location = new System.Drawing.Point(0 ,10); // 800  500
			this.NomoGram.Size = new System.Drawing.Size(90, 180); //90. 180
			//this.NomoGram.BringToFront();
			//this.NomoGram.Capture = true;
			this.Controls.Add(this.NomoGram);
			this.NomoGram.Main= this;
			this.NomoGram.ResumeLayout(false);
			// 
			// crossy
			//
			this.Controls.Add(this.panel);
			
			this.Name = "crossy";
			this.Text = "CrossY";
			this.panel.Main = this;
			this.panel.ResumeLayout(false);
			this.SetVisibleCore(  true);
			this.ResumeLayout(false);	
		}
		[STAThread]
		static void Main() 
		{
			Application.Run(new crossy());
			
		}
		protected override void OnLocationChanged(EventArgs e)
		{
			base.OnLocationChanged (e);
			if(this.FlowMenu != null)
				this.FlowMenu.firstrun = true;
		}
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint (e);
			
		}


		
		
		
		
	}

	
}
			
			
			
			

		
		
