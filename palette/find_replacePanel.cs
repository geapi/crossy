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
	

	

	public class find_replacePanel:dialoguebox
	{
		Strokes findresults;
		Strokes prevfindresults;
		static private int findcounter= 0;
		Stack oldvalues = new Stack();
		Stack oldindices = new Stack();
		Stack reverse_find_stack = new Stack();
		pen_values f_and_r_queue;
		

		private class Find_Replace_MoveButton:MoveButton
		{
			public Find_Replace_MoveButton(crossy Main, Point original_location):base(Main, original_location)
			{
				
			}
			public override void back_to_original_position()
			{
				Main.Palette.find_replace.Location = new Point(Main.Palette.Location.X-250, Main.Palette.Location.Y +260);
				set_initial_position(Main.Palette.find_replace.Location);
			}
			public override void new_position(Point new_position)
			{
				Main.Palette.find_replace.Location = new_position;
				set_initial_position(new_position);
			}
			protected override void OnMouseDown(MouseEventArgs e)
			{
				base.OnMouseDown(e);
				set_initial_position(Main.Palette.find_replace.Location);
			}
		}
	
		public class Stroke_Find_Button: optionbox
		{
			//public Pen IconPen = new Pen(Color.Black, 5);
			find_replacePanel container;
			public Stroke_Find_Button(penattr penStyle, find_replacePanel container)
			{
				assigned_penStyle = penStyle;
				this.container = container;
				this.Size = new System.Drawing.Size(60, 30);
				this.buttonlinePen = new Pen(Color.Black, 2);
				this.stopperPen = new Pen(Color.Black, 4);
				this.BackColor = Color.White;
				this.font = new Font("Verdana", 8);
				this.label = "";
				//Enabled = false;
				
				
				
			}

			public override void OnCrossing(string what, HowCrossed fromwhere, bool selected, optionbox thisopt)
			{
				if (fromwhere == HowCrossed.fromleft || fromwhere == HowCrossed.fromright)
				{
					container.f_and_r_queue.find_thickness = assigned_penStyle;
				}
				if(selected == true)
				{
					thisopt.myradiogroup.selected(thisopt);
			
				}
				if(selected == false)
				{
					thisopt.myradiogroup.unselected(thisopt);
				}
			}
			protected override void OnMouseUp(MouseEventArgs e)
			{
				
				container.do_find_replace();
				//Main.Palette.penpanel.Visible = false;
			
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
		public class Stroke_Replace_Button: optionbox
		{
			//public Pen IconPen = new Pen(Color.Black, 5);
			find_replacePanel container;
			public Stroke_Replace_Button(penattr penStyle, find_replacePanel container)
			{
				assigned_penStyle = penStyle;
				this.container = container;
				this.Size = new System.Drawing.Size(60, 30);
				this.buttonlinePen = new Pen(Color.Black, 2);
				this.stopperPen = new Pen(Color.Black, 4);
				this.BackColor = Color.White;
				this.font = new Font("Verdana", 8);
				this.label = "";
				//Enabled = false;
				
				
			}

			public override void OnCrossing(string what, HowCrossed fromwhere, bool selected, optionbox thisopt)
			{
				if (fromwhere == HowCrossed.fromleft || fromwhere == HowCrossed.fromright)
				{
					
					container.f_and_r_queue.replace_thickness = assigned_penStyle;

				}
				if(selected == true)
				{
					
					thisopt.myradiogroup.selected(thisopt);
			
				}
				if(selected == false)
				{
					thisopt.myradiogroup.unselected(thisopt);
				}
			}
			protected override void OnMouseUp(MouseEventArgs e)
			{
				
				container.do_find_replace();
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
		public class Color_Find_Button: optionbox
		{
			//public Pen IconPen = new Pen(Color.Black, 5);
			find_replacePanel container;
			private Color assigned_color;
			public Color_Find_Button(Color color, find_replacePanel container)
			{
				assigned_color = color;
				this.container = container;
				this.Size = new System.Drawing.Size(60, 30);
				this.brushcolor = Color.White;
				this.label = "";
				this.buttonlinePen = new Pen(Color.White, 2);
				this.stopperPen = new Pen(Color.Black, 4);
				this.font = new Font("Verdana", 8);
				//Enabled = false;
				
				this.IconPen = new Pen(assigned_color, 6);
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

				if (fromwhere == HowCrossed.fromleft || fromwhere == HowCrossed.fromright)
				{

					container.f_and_r_queue.find_color = assigned_color;

					
				}
				if(selected == true)
				{
					thisopt.myradiogroup.selected(thisopt);
			
				}
				if(selected == false)
				{
					thisopt.myradiogroup.unselected(thisopt);
				}
			}
			protected override void OnMouseUp(MouseEventArgs e)
			{
				container.do_find_replace();
			}
			
		}
		
		public class Color_Replace_Button: optionbox
		{
			//public Pen IconPen = new Pen(Color.Black, 5);
			find_replacePanel container;
			private Color assigned_color;
			public Color_Replace_Button(Color color, find_replacePanel container)
			{
				assigned_color = color;
				this.container = container;
				this.Size = new System.Drawing.Size(60, 30);
				this.brushcolor = Color.White;
				this.label = "";
				this.buttonlinePen = new Pen(Color.White, 2);
				this.stopperPen = new Pen(Color.Black, 4);
				this.font = new Font("Verdana", 8);
				
				this.IconPen = new Pen(assigned_color, 6);
				//Enabled = false;
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


				if (fromwhere == HowCrossed.fromleft || fromwhere == HowCrossed.fromright)
				{

					container.f_and_r_queue.replace_color = assigned_color;

					
				}
				if(selected == true)
				{

					thisopt.myradiogroup.selected(thisopt);
			
				}
				if(selected == false)
				{
					thisopt.myradiogroup.unselected(thisopt);
				}
			}
			protected override void OnMouseUp(MouseEventArgs e)
			{
				
				//Main.Palette.penpanel.Visible = false;
				container.do_find_replace();
			
			}
			
		}
		
		public class FindButton: button
		{
			find_replacePanel container;
			public FindButton(find_replacePanel container)
			{
				ishorizontal = false;
				brushcolor = Color.Black;
				this.container = container;
				this.Size = new System.Drawing.Size(80, 40);
				this.font = new Font("Tahoma",10, FontStyle.Bold);
				this.buttonlinePen = new Pen(Color.Black, 2);
				this.stopperPen = new Pen(Color.Black, 4);
				this.BackColor = Color.White;
				//Enabled = false;
			}
			public override void OnCrossing(HowCrossed fromwhere)
			{
				//Console.WriteLine("fc");
				//Main.Palette.find_replace.find.BackColor = Color.Orange;
				//Main.Palette.find_replace.find.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\find_replace_over.gif");
				container.find.between= new Pen(Color.Orange, 2);
				
				container.find.Invalidate();
				if(fromwhere == HowCrossed.fromright)
				{
					container.f_and_r_queue.find_crossed = true;
				}
				if(fromwhere == HowCrossed.fromleft)
				{
					container.f_and_r_queue.reverse_find_crossed = true;
				}
				container.do_find_replace();
				
			}
			protected override void OnMouseLeave(EventArgs e)
			{
				base.OnMouseLeave(e);
				container.find.between = new Pen(Color.Transparent, 2);
				container.find.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\find_replace_normal.gif");
				container.find.Invalidate();
			}
			protected override void OnMouseUp(MouseEventArgs e)
			{
				container.find.between = new Pen(Color.Transparent, 2);
				//Main.Palette.find_replace.find.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\find_replace_normal.gif");
				//Main.Palette.find_replace.find.BackColor = Color.White;
				container.find.Invalidate();
				
				container.do_find_replace();
			
			}

			protected override void OnMouseEnter(EventArgs e)
			{
				base.OnMouseEnter (e);
				container.initialize_find_replace_queue();
			}

			
		}
		public class ReplaceButton: button
		{
			find_replacePanel container;
			public Pen tempPen;
			public ReplaceButton(find_replacePanel container)
			{
				ishorizontal = false;
				brushcolor = Color.Black;
				this.container = container;
				this.Size = new System.Drawing.Size(80, 40);
				this.font = new Font("Tahoma",10, FontStyle.Bold);
				this.buttonlinePen = new Pen(Color.Black, 2);
				this.stopperPen = new Pen(Color.Black, 4);
				this.BackColor = Color.White;
				//Enabled = false;
			}
			public override void OnCrossing(HowCrossed fromwhere)
			{
				//Main.Palette.find_replace.replace.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\find_replace_over.gif");
				//Main.Palette.find_replace.replace.BackColor = Color.Orange;
				container.replace.between = new Pen(Color.Orange, 2);
				container.replace.Invalidate();
				if(fromwhere == HowCrossed.fromright)
				{
					container.f_and_r_queue.undo_replace_crossed = true;
					container.f_and_r_queue.replace_crossed = false;

				}
				if(fromwhere == HowCrossed.fromleft)
				{
					container.f_and_r_queue.replace_crossed = true;
					//container.f_and_r_queue.one_stroke_after_replace =true;
				}
				container.do_find_replace();
				
				
				
			}
			protected override void OnMouseLeave(EventArgs e)
			{
				base.OnMouseLeave(e);
				//Main.Palette.find_replace.replace.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\find_replace_normal.gif");
				container.replace.between = new Pen(Color.Transparent, 2);
				container.replace.Invalidate();
			}
			protected override void OnMouseUp(MouseEventArgs e)
			{
				container.replace.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\find_replace_normal.gif");
				//Main.Palette.find_replace.replace.BackColor = Color.White;
				container.replace.between = new Pen(Color.Transparent, 2);
				container.replace.Invalidate();
				
				container.f_and_r_queue.replace_crossed = false;
				container.do_find_replace();
				
				//container.f_and_r_queue.undo_replace_crossed = false;
				

			}
		}
		
		
		private Stroke_Find_Button optBox, optBox2,optBox3;
		private Stroke_Replace_Button optBox4,optBox5,optBox6;
		private Color_Find_Button optBox7,optBox8, colorFind3;
		private Color_Replace_Button optBox9,optBox10, colorReplace3;
		private FindButton find;
		public ReplaceButton replace;
		private Pen undo_Pen;
		private Strokes undo_Stroke;
		bool reverse_find_done= false;
		bool undo_replace_first= false;
		bool straight_reverse_find = true;
		private Find_Replace_MoveButton Find_Replace_Move;
		private radiogroup widthFind, widthReplace, colorFind, colorReplace;
		public override void OnCrossing(HowCrossed fromwhere)
		{
			//Console.WriteLine(fromwhere);
			if ((fromwhere == HowCrossed.fromright ||fromwhere == HowCrossed.fromtop))
			{
				// low and right, apply changed values\
				//Console.WriteLine("OK");
				//Main.Drawpanel.changePen(penStyle, penColor);
				//Main.Drawpanel.mySelect(penStyle);
				
				Main.central_TabControl.get_active_TabPanel().strokecollector.Selection = Main.central_TabControl.get_active_TabPanel().strokecollector.Ink.CreateStrokes();
				Main.Palette.ButtonFind.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\lupe.gif");
				Main.Palette.find_replace.Visible = false;
			}
			if ((fromwhere == HowCrossed.fromleft|| fromwhere == HowCrossed.frombottom))
			{
				Main.central_TabControl.get_active_TabPanel().strokecollector.Selection = Main.central_TabControl.get_active_TabPanel().strokecollector.Ink.CreateStrokes();
				Main.Palette.ButtonFind.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\lupe.gif");
				Main.Palette.find_replace.Visible = false;
				/*Main.Palette.find_replace.replace.tempPen = new Pen(Main.Palette.find_replace.strokeColorF, stroke_width);
				Main.Palette.find_replace.oldvalues.Push(Main.Palette.find_replace.replace.tempPen);
				Main.Palette.find_replace.oldindices.Push(Main.Palette.find_replace.findresults);
				if(Main.Palette.find_replace.findresults != null)
				{

					Main.Drawpanel.replacestrokes(Main.Palette.find_replace.findresults,stroke_width,
						Main.Palette.find_replace.strokeColorR);
				}*/
				//Console.WriteLine("CANCEL");
				//top and left, discharge changed values
				//Main.Drawpanel.strokecollector.Selection = Main.Drawpanel.strokecollector.Ink.CreateStrokes();
				//Main.Palette.ButtonFind.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\lupe.gif");
				//Main.Palette.find_replace.Visible = false;
			}
			initialize_find_replace_queue();
			oldvalues.Clear();
			oldindices.Clear();
			//Main.eventhandler.crossed(trigger, (int)fromwhere);
		}
		protected override void OnMouseUp(MouseEventArgs e)
		{
			
			do_find_replace();
			initialize_find_replace_queue();
			/*optBox.OnMouseUp_private_dispatch(new MouseEventArgs(e.Button, e.Clicks, e.X - optBox.Location.X, e.Y - optBox.Location.Y, e.Delta));
			optBox2.OnMouseUp_private_dispatch(new MouseEventArgs(e.Button, e.Clicks, e.X - optBox2.Location.X, e.Y - optBox2.Location.Y, e.Delta));
			optBox3.OnMouseUp_private_dispatch(new MouseEventArgs(e.Button, e.Clicks, e.X - optBox3.Location.X, e.Y - optBox3.Location.Y, e.Delta));
			optBox4.OnMouseUp_private_dispatch(new MouseEventArgs(e.Button, e.Clicks, e.X - optBox4.Location.X, e.Y - optBox4.Location.Y, e.Delta));
			optBox5.OnMouseUp_private_dispatch(new MouseEventArgs(e.Button, e.Clicks, e.X - optBox5.Location.X, e.Y - optBox5.Location.Y, e.Delta));
			optBox6.OnMouseUp_private_dispatch(new MouseEventArgs(e.Button, e.Clicks, e.X - optBox6.Location.X, e.Y - optBox6.Location.Y, e.Delta));
			optBox7.OnMouseUp_private_dispatch(new MouseEventArgs(e.Button, e.Clicks, e.X - optBox7.Location.X, e.Y - optBox7.Location.Y, e.Delta));
			optBox8.OnMouseUp_private_dispatch(new MouseEventArgs(e.Button, e.Clicks, e.X - optBox8.Location.X, e.Y - optBox8.Location.Y, e.Delta));
			optBox9.OnMouseUp_private_dispatch(new MouseEventArgs(e.Button, e.Clicks, e.X - optBox9.Location.X, e.Y - optBox9.Location.Y, e.Delta));
			optBox10.OnMouseUp_private_dispatch(new MouseEventArgs(e.Button, e.Clicks, e.X - optBox10.Location.X, e.Y - optBox10.Location.Y, e.Delta));

			colorFind3.OnMouseUp_private_dispatch(new MouseEventArgs(e.Button, e.Clicks, e.X - colorFind3.Location.X, e.Y - colorFind3.Location.Y, e.Delta));
			colorReplace3.OnMouseUp_private_dispatch(new MouseEventArgs(e.Button, e.Clicks, e.X - colorReplace3.Location.X, e.Y - colorReplace3.Location.Y, e.Delta));

			find.OnMouseUp_private_dispatch(new MouseEventArgs(e.Button, e.Clicks, e.X - find.Location.X, e.Y - find.Location.Y, e.Delta));
			replace.OnMouseUp_private_dispatch(new MouseEventArgs(e.Button, e.Clicks, e.X - replace.Location.X, e.Y - replace.Location.Y, e.Delta));*/
		}
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove (e);
			/*optBox.OnMouseMove_private_dispatch(new MouseEventArgs(e.Button, e.Clicks, e.X - optBox.Location.X, e.Y - optBox.Location.Y, e.Delta));
			optBox2.OnMouseMove_private_dispatch(new MouseEventArgs(e.Button, e.Clicks, e.X - optBox2.Location.X, e.Y - optBox2.Location.Y, e.Delta));
			optBox3.OnMouseMove_private_dispatch(new MouseEventArgs(e.Button, e.Clicks, e.X - optBox3.Location.X, e.Y - optBox3.Location.Y, e.Delta));
			optBox4.OnMouseMove_private_dispatch(new MouseEventArgs(e.Button, e.Clicks, e.X - optBox4.Location.X, e.Y - optBox4.Location.Y, e.Delta));
			optBox5.OnMouseMove_private_dispatch(new MouseEventArgs(e.Button, e.Clicks, e.X - optBox5.Location.X, e.Y - optBox5.Location.Y, e.Delta));
			optBox6.OnMouseMove_private_dispatch(new MouseEventArgs(e.Button, e.Clicks, e.X - optBox6.Location.X, e.Y - optBox6.Location.Y, e.Delta));
			optBox7.OnMouseMove_private_dispatch(new MouseEventArgs(e.Button, e.Clicks, e.X - optBox7.Location.X, e.Y - optBox7.Location.Y, e.Delta));
			optBox8.OnMouseMove_private_dispatch(new MouseEventArgs(e.Button, e.Clicks, e.X - optBox8.Location.X, e.Y - optBox8.Location.Y, e.Delta));
			optBox9.OnMouseMove_private_dispatch(new MouseEventArgs(e.Button, e.Clicks, e.X - optBox9.Location.X, e.Y - optBox9.Location.Y, e.Delta));
			optBox10.OnMouseMove_private_dispatch(new MouseEventArgs(e.Button, e.Clicks, e.X - optBox10.Location.X, e.Y - optBox10.Location.Y, e.Delta));

			colorFind3.OnMouseMove_private_dispatch(new MouseEventArgs(e.Button, e.Clicks, e.X - colorFind3.Location.X, e.Y - colorFind3.Location.Y, e.Delta));
			colorReplace3.OnMouseMove_private_dispatch(new MouseEventArgs(e.Button, e.Clicks, e.X - colorReplace3.Location.X, e.Y - colorReplace3.Location.Y, e.Delta));

			find.OnMouseMove_private_dispatch(new MouseEventArgs(e.Button, e.Clicks, e.X - find.Location.X, e.Y - find.Location.Y, e.Delta));
			replace.OnMouseMove_private_dispatch(new MouseEventArgs(e.Button, e.Clicks, e.X - replace.Location.X, e.Y - replace.Location.Y, e.Delta));*/



		}

		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter (e);
			Win32Application.Win32.ReleaseCapture();
			/*optBox.OnMouseEnter_private_dispatch(e);
			optBox2.OnMouseEnter_private_dispatch(e);
			optBox3.OnMouseEnter_private_dispatch(e);
			optBox4.OnMouseEnter_private_dispatch(e);
			optBox5.OnMouseEnter_private_dispatch(e);
			optBox6.OnMouseEnter_private_dispatch(e);
			optBox7.OnMouseEnter_private_dispatch(e);
			optBox8.OnMouseEnter_private_dispatch(e);
			optBox9.OnMouseEnter_private_dispatch(e);
			optBox10.OnMouseEnter_private_dispatch(e);

			colorFind3.OnMouseEnter_private_dispatch(e);
			colorReplace3.OnMouseEnter_private_dispatch(e);
			find.OnMouseEnter_private_dispatch(e);
			replace.OnMouseEnter_private_dispatch(e);*/
		}
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave (e);
			/*optBox.OnMouseLeave_private_dispatch(e);
			optBox2.OnMouseLeave_private_dispatch(e);
			optBox3.OnMouseLeave_private_dispatch(e);
			optBox4.OnMouseLeave_private_dispatch(e);
			optBox5.OnMouseLeave_private_dispatch(e);
			optBox6.OnMouseLeave_private_dispatch(e);
			optBox7.OnMouseLeave_private_dispatch(e);
			optBox8.OnMouseLeave_private_dispatch(e);
			optBox9.OnMouseLeave_private_dispatch(e);
			optBox10.OnMouseLeave_private_dispatch(e);

			colorFind3.OnMouseLeave_private_dispatch(e);
			colorReplace3.OnMouseLeave_private_dispatch(e);
			find.OnMouseLeave_private_dispatch(e);
			replace.OnMouseLeave_private_dispatch(e);*/
		}


		public find_replacePanel(crossy theForm)
		{
			crossy Main = theForm;
			//this.optBox.SuspendLayout();
			first_initialize_find_replace_queue();
		
			
			this.optBox = new Stroke_Find_Button(penattr.small, this);
			this.optBox2 = new Stroke_Find_Button(penattr.medium, this);
			this.optBox3 = new Stroke_Find_Button(penattr.thick, this);
			this.optBox4 = new Stroke_Replace_Button(penattr.small, this);
			this.optBox5 = new Stroke_Replace_Button(penattr.medium, this);
			this.optBox6 = new Stroke_Replace_Button(penattr.thick, this);
			this.optBox7 = new Color_Find_Button(Color.Red, this);
			this.optBox8 = new Color_Find_Button(Color.Blue, this);
			this.colorFind3 = new Color_Find_Button(Color.Black, this);
			this.optBox9 = new Color_Replace_Button(Color.Red, this);
			this.optBox10 = new Color_Replace_Button(Color.Blue, this);
			this.colorReplace3 = new Color_Replace_Button(Color.Black, this);
			this.find = new FindButton(this);
			this.replace = new ReplaceButton(this);

			this.Find_Replace_Move = new Find_Replace_MoveButton(Main, this.Location);
			this.Find_Replace_Move.Location = new Point((this.Width/2),3);
			this.Find_Replace_Move.Size = new Size(60,17);
			this.Find_Replace_Move.Main = Main;
			this.Find_Replace_Move.BackColor = Color.White;

			this.Controls.Add(this.Find_Replace_Move);

			//
			// optbox (1pt)
			//
			int dis_Strokes = 0;
			
			this.widthFind = new radiogroup();
			this.optBox.Location = new System.Drawing.Point(this.Location.X + 170, this.Location.Y + 21);
			this.optBox.trigger = "1ptF";
			this.optBox.Name = "optBox";
			this.optBox.myradiogroup = this.widthFind;
			this.optBox.myFather = "findsize";
			
			this.optBox.myValue = "1ptF";
			this.optBox.Main = Main;
			this.optBox.IconPen = new Pen(Color.Black, 2);
			this.optBox.BringToFront();
			this.Controls.Add(this.optBox);
			//
			// optbox 2 (2pt)
			//
			this.optBox2.Location = new System.Drawing.Point(this.optBox.Location.X, this.optBox.Location.Y + this.optBox.Height + dis_Strokes);
		
			this.optBox2.Name = "optBox2";
			this.optBox2.trigger = "2ptF";
			this.optBox2.myradiogroup = this.widthFind;
			this.optBox2.myFather = "findsize";
			this.optBox2.myValue = "2ptF";
			this.optBox2.BringToFront();
			this.optBox2.IconPen = new Pen(Color.Black,4);
			this.optBox2.Main = Main;
			this.Controls.Add(this.optBox2);
			//
			// optbox 3 (3pt)
			//
			this.optBox3.Location = new System.Drawing.Point(this.optBox.Location.X, this.optBox2.Location.Y + this.optBox2.Height + dis_Strokes);
			this.optBox3.trigger = "3ptF";
			this.optBox3.myFather = "findsize";
			this.optBox3.myradiogroup = this.widthFind;
		
			this.optBox3.myValue = "3ptF";
			this.optBox3.BringToFront();
			this.optBox3.Main = Main;
			this.optBox3.IconPen = new Pen(Color.Black, 6);
			this.Controls.Add(this.optBox3);
			//
			// optbox4 (1pt)
			//
			this.widthReplace = new radiogroup();
			this.optBox4.Location = new System.Drawing.Point(this.optBox.Location.X, this.optBox3.Location.Y + this.optBox3.Height  + 10);
			this.optBox4.trigger = "1ptR";
			this.optBox4.myradiogroup = this.widthReplace;
			this.optBox4.myFather = "replacesize";
			this.optBox4.myValue = "1ptR";
			this.optBox4.BringToFront();
			this.optBox4.Main = Main;
			this.optBox4.IconPen = new Pen(Color.Black, 2);
			this.Controls.Add(this.optBox4);
			//
			// optbox5 2 (2pt)
			//
			this.optBox5.Location = new System.Drawing.Point(this.optBox.Location.X, this.optBox4.Location.Y + this.optBox4.Height + dis_Strokes);
			this.optBox5.trigger = "2ptR";
			this.optBox5.myFather = "replacesize";
			this.optBox5.myradiogroup = this.widthReplace;
			this.optBox5.myValue = "2ptR";
			this.optBox5.Text = "opt2";
			this.optBox5.BringToFront();
			this.optBox5.Main = Main;
			this.optBox5.IconPen = new Pen(Color.Black, 4);
			this.Controls.Add(this.optBox5);
			//
			// optbox6 3 (3pt)
			//
			this.optBox6.Location = new System.Drawing.Point(this.optBox.Location.X, this.optBox5.Location.Y + this.optBox5.Height + dis_Strokes);
			this.optBox6.trigger = "3ptR";
			this.optBox6.myFather = "replacesize";
			this.optBox6.myradiogroup = this.widthReplace;
			this.optBox6.myValue = "3ptR";
			this.optBox6.BringToFront();
			this.optBox6.Main = Main;
			this.optBox6.IconPen = new Pen(Color.Black, 6);
			this.Controls.Add(this.optBox6);
			
			//
			// optbox7 (red find)
			//
			this.colorFind =new radiogroup();
			this.optBox7.Location = new System.Drawing.Point(this.optBox.Location.X - this.optBox.Width - 10, this.optBox.Location.Y );
			this.optBox7.trigger = "redF";
			this.optBox7.myFather = "findcolor";
			this.optBox7.myradiogroup = this.colorFind;
			this.optBox7.myValue = "redF";
			this.optBox7.BringToFront();
			this.optBox7.Main = Main;
			this.Controls.Add(this.optBox7);
			
			//
			// optbox8 2 (blue find)
			//
			this.optBox8.Location = new System.Drawing.Point(this.optBox7.Location.X, this.optBox7.Location.Y + this.optBox7.Height );
			this.optBox8.trigger = "blueF";
			this.optBox8.myFather = "findcolor";
			this.optBox8.myradiogroup = this.colorFind;
			this.optBox8.myValue = "blueF";
			this.optBox8.BringToFront();
			this.optBox8.Main = Main;
			this.Controls.Add(this.optBox8);

			this.colorFind3.Location = new System.Drawing.Point(this.optBox8.Location.X, this.optBox8.Location.Y + this.optBox8.Height );
			this.colorFind3.trigger = "blueF";
			this.colorFind3.myFather = "findcolor";
			this.colorFind3.myradiogroup = this.colorFind;
			this.colorFind3.myValue = "blueF";
			this.colorFind3.BringToFront();
			this.colorFind3.Main = Main;
			this.Controls.Add(this.colorFind3);
			
			//
			// optbox9 (red replace)
			//
			this.colorReplace = new radiogroup();
			this.optBox9.Location = new System.Drawing.Point(this.optBox7.Location.X, this.colorFind3.Location.Y + this.colorFind3.Height + 10 );
			this.optBox9.trigger = "redR";
			this.optBox9.myFather = "replacecolor";
			this.optBox9.myradiogroup = this.colorReplace;
			this.optBox9.myValue = "redR";
			this.optBox9.BringToFront();
			this.optBox9.Main = Main;
			this.Controls.Add(this.optBox9);
			
			//
			// optbox10 2 (blue replace)
			//
			this.optBox10.Location = new System.Drawing.Point(this.optBox7.Location.X, this.optBox9.Location.Y + this.optBox9.Height );
			this.optBox10.trigger = "blueR";
			this.optBox10.myFather = "replacecolor";
			this.optBox10.myradiogroup = this.colorReplace;
			this.optBox10.myValue = "blueR";
			this.optBox10.BringToFront();
			this.optBox10.Main = Main;
			this.Controls.Add(this.optBox10);
			//
			//
			this.colorReplace3.Location = new System.Drawing.Point(this.optBox7.Location.X, this.optBox10.Location.Y + this.optBox10.Height );
			this.colorReplace3.trigger = "blueR";
			this.colorReplace3.myFather = "replacecolor";
			this.colorReplace3.myradiogroup = this.colorReplace;
			this.colorReplace3.myValue = "blueR";
			this.colorReplace3.BringToFront();
			this.colorReplace3.Main = Main;
			this.Controls.Add(this.colorReplace3);
			
			//
			// optboxfind  (find)
			//
			this.find.Location = new System.Drawing.Point(this.optBox7.Location.X - this.optBox7.Width - 30, this.colorFind3.Location.Y -10);
			this.find.label = "    find";
			this.find.trigger = "find";
			
			this.find.BringToFront();
			this.find.Main = Main;
			this.Controls.Add(this.find);
		

			//
			// optboxreplace  (replace)
			//
			this.replace.Location = new System.Drawing.Point(this.optBox7.Location.X - this.optBox7.Width -  30, this.optBox9.Location.Y );
			this.replace.label = "  replace";
			this.replace.trigger = "replace";
			
			this.replace.BringToFront();
			this.replace.Main = Main;
			this.Controls.Add(this.replace);
		}
		
		protected override void OnPaint(PaintEventArgs e)
		{
			
			base.OnPaint(e);
			Graphics feedback = e.Graphics;
			Pen bluePen = new Pen(Color.Black,1);
			//feedback.DrawLine(bluePen, 20, 115, 230, 115);
			
		}

		void initialize_find_replace_queue()
		{
			//f_and_r_queue.find_color = Color.Transparent;
			//f_and_r_queue.find_thickness = penattr.none;
			//f_and_r_queue.replace_color = Color.Transparent;
			//f_and_r_queue.replace_thickness = penattr.none;
			f_and_r_queue.find_crossed = false;
			f_and_r_queue.reverse_find_crossed = false;
			f_and_r_queue.replace_crossed = false;
			f_and_r_queue.undo_replace_crossed = false;
			reverse_find_done = false;
			undo_replace_first = false;
			//straight_reverse_find = true;
			//oldvalues.Clear();
			//oldindices.Clear();
			
		}
		void first_initialize_find_replace_queue()
		{
			f_and_r_queue.find_color = Color.Transparent;
			f_and_r_queue.find_thickness = penattr.none;
			f_and_r_queue.replace_color = Color.Transparent;
			f_and_r_queue.replace_thickness = penattr.none;
			f_and_r_queue.find_crossed = false;
			f_and_r_queue.reverse_find_crossed = false;
			f_and_r_queue.replace_crossed = false;
			f_and_r_queue.undo_replace_crossed = false;
			reverse_find_done = false;
			undo_replace_first = false;
			straight_reverse_find = true;
			
		}
		void do_find_replace()
		{

			//Console.WriteLine(f_and_r_queue.find_thickness+" "+f_and_r_queue.find_color+" "+f_and_r_queue.find_crossed+" "+f_and_r_queue.reverse_find_crossed);

			// forwards find
			if(f_and_r_queue.find_crossed && f_and_r_queue.find_color != Color.Transparent && f_and_r_queue.find_thickness != penattr.none)
			{
				straight_reverse_find = true;
				Pen tempPen = new Pen(f_and_r_queue.find_color, (int)f_and_r_queue.find_thickness);
				
				//Console.WriteLine("pushed in find");
				findresults = Main.central_TabControl.get_active_TabPanel().findstrokesforward((int)f_and_r_queue.find_thickness,
					f_and_r_queue.find_color, ref findcounter);
				oldvalues.Push(tempPen);
				oldindices.Push(findresults);
				if(findresults == null)
					Main.central_TabControl.get_active_TabPanel().strokecollector.Selection = Main.central_TabControl.get_active_TabPanel().strokecollector.Ink.CreateStrokes();
				prevfindresults = findresults;
				f_and_r_queue.find_crossed = false;
			}
			// backwards find
			if(f_and_r_queue.reverse_find_crossed && f_and_r_queue.find_color != Color.Transparent && f_and_r_queue.find_thickness != penattr.none)
			{
				//
				try
				{
					if(!undo_replace_first)
					{
						undo_Pen = (Pen)oldvalues.Pop();
						undo_Stroke = (Strokes)oldindices.Pop();
						if(straight_reverse_find)
						{
							//Console.WriteLine("we are undqueuin");
							undo_Pen = (Pen)oldvalues.Pop();
							undo_Stroke = (Strokes)oldindices.Pop();
							straight_reverse_find = false;
						}
						//undo_replace_first = true;
						//Console.WriteLine("popped in reverse find");
					}
					Main.central_TabControl.get_active_TabPanel().strokecollector.Selection = Main.central_TabControl.get_active_TabPanel().strokecollector.Ink.CreateStrokes(new int[] {undo_Stroke[0].Id});
					reverse_find_done = true;
					//reverse_find_stack.Push(stroke);
				}
				catch(Exception e)
				{
					
				}
				f_and_r_queue.reverse_find_crossed = false;
			}


			// replace
			if(f_and_r_queue.replace_crossed && f_and_r_queue.replace_color!= Color.Transparent && f_and_r_queue.replace_thickness != penattr.none)
			{
				//Pen tempPen = new Pen(f_and_r_queue.find_color, (int)f_and_r_queue.find_thickness);
				//oldvalues.Push(tempPen);
				//oldindices.Push(findresults);
				if(findresults != null)
				{
						
					Main.central_TabControl.get_active_TabPanel().replacestrokes(findresults,(int)f_and_r_queue.replace_thickness,
						f_and_r_queue.replace_color);
				}
				//f_and_r_queue.replace_crossed = false;
			}

			
			// undo replace
			if(f_and_r_queue.undo_replace_crossed && f_and_r_queue.find_color != Color.Transparent && f_and_r_queue.find_thickness != penattr.none
				&& f_and_r_queue.replace_color != Color.Transparent && f_and_r_queue.replace_thickness != penattr.none)
			{
				//if(oldvalues.Count != 0)
				straight_reverse_find = false;
				{
					Pen tmpPen  = undo_Pen;
					Strokes stroke = undo_Stroke;
					bool stack_empty = false;
					if (!reverse_find_done)
					{
						try
						{
							tmpPen = (Pen)oldvalues.Pop();
							stroke = (Strokes)oldindices.Pop();
							undo_replace_first = true;
							//Console.WriteLine("popped in undo replace");
						}
						catch(Exception e)
						{
							stack_empty = true;
							//Console.WriteLine(e);
						}
					}
					
					if (!stack_empty)
					{
						Main.central_TabControl.get_active_TabPanel().replacestrokes(stroke, tmpPen.Width, tmpPen.Color);
						try
						{
							Main.central_TabControl.get_active_TabPanel().strokecollector.Selection = Main.central_TabControl.get_active_TabPanel().strokecollector.Ink.CreateStrokes(new int[] {stroke[0].Id});	
						}
						catch(Exception e){}
						
					}
				}
				f_and_r_queue.undo_replace_crossed = false;
				replace.Invalidate();
			}
		 
			

		}
	}
	
	


}
