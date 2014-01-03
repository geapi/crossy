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
	
	public class strokePanel:dialoguebox
	{
		public  penattr pressure;
		private penattr antialiased;
		private penattr fittocurve;
		private checkgroup strokeAttri;
		private Stroke_CheckBox optBox, optBox2, optBox3;
		public class Stroke_CheckBox: checkbox
		{
			strokePanel container;
			public Stroke_CheckBox(strokePanel container)
			{
				brushcolor = Color.Black;
				this.container = container;
				//Pen beetween = whitePen;
			
				this.BackColor = Color.White;
				this.Size = new System.Drawing.Size(120, 30);
				this.buttonlinePen = new Pen(Color.Black, 2);
				this.stopperPen = new Pen(Color.Black, 4);
				this.BackColor = Color.White;
				this.font = new Font("Verdana", 8);
				Enabled = false;

				
				
			}
			public override void OnCrossing(string what, HowCrossed fromwhere, bool selected, checkbox thisopt)
			{
				///Console.WriteLine("in checkgroup");
				// *************
				// find options
				// *************
				if (selected == true)
				{
					thisopt.mycheckgroup.selected(thisopt);
					if(what == "pressure")// && fromwhere == (int)HowCrossed.fromleft)
					{
						container.pressure = penattr.pressuresensitive;
					}
					if(what == "antialiased")// && fromwhere == (int)HowCrossed.fromleft)
					{
						container.antialiased = penattr.antialiased;
					}
					if(what == "fittocurve")// && fromwhere == (int)HowCrossed.fromleft)
					{
						container.fittocurve= penattr.fittocurve;
					}
				}
				else
				{
					thisopt.mycheckgroup.unselected(thisopt);
					if(what == "pressure")// && fromwhere == (int)HowCrossed.fromleft)
					{
						container.pressure = penattr.none;
					}
					if(what == "antialiased")// && fromwhere == (int)HowCrossed.fromleft)
					{
						container.antialiased = penattr.none;
					}
					if(what == "fittocurve")// && fromwhere == (int)HowCrossed.fromleft)
					{
						container.fittocurve= penattr.none;
					}
				}

			}
		}
		public override void OnCrossing(HowCrossed fromwhere)
		{
			if ((fromwhere == HowCrossed.fromright ||fromwhere == HowCrossed.fromtop))
			{
				// low and right, apply changed values\
				//Console.WriteLine("OK");
				
				Main.central_TabControl.get_active_TabPanel().changeStrokeAttributes(pressure, antialiased, fittocurve);
				Main.Palette.penpanel.ButtonStroke.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\stroke.gif");
				this.Visible = false;
				Main.Palette.penpanel.Visible = false;
			}
		
			if ((fromwhere == HowCrossed.frombottom||fromwhere == HowCrossed.fromleft))
			{
				//Console.WriteLine("CANCEL");
				//top and left, discharge changed values
				Main.Palette.penpanel.ButtonStroke.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\stroke.gif");
				this.Visible = false;
				//Main.Palette.penpanel.Visible = false;
			}
			Win32Application.Win32.ReleaseCapture();
		}
		public strokePanel(crossy theForm)
		{
			crossy Main = theForm;
			this.optBox = new Stroke_CheckBox(this);
			this.optBox2 = new Stroke_CheckBox(this);
			this.optBox3 = new Stroke_CheckBox(this);

			//
			// pressure
			//
			this.strokeAttri = new checkgroup();
			this.optBox.Location = new System.Drawing.Point(this.Location.X + 10, this.Location.Y + 13);
			this.optBox.trigger = "pressure";
			this.optBox.Name = "optBox";
			this.optBox.mycheckgroup = this.strokeAttri;
			this.optBox.myFather = "findsize";
			this.optBox.label = "pressure sens.";
			this.optBox.myValue = "1ptF";
			//this.optBox.BackColor = Color.SteelBlue;
			//this.optBox.brushcolor = Color.White;
			this.optBox.Main = Main;
			//this.optBox.ishorizontal = true;
			this.optBox.BringToFront();
			this.Controls.Add(this.optBox);
			//
			//  antialiased
			//
			this.optBox2.Location = new System.Drawing.Point(this.optBox.Location.X, this.optBox.Location.Y + this.optBox.Height);
			this.optBox2.label = "antialiased";
			this.optBox2.Name = "optBox2";
			this.optBox2.trigger = "antialiased";
			this.optBox2.mycheckgroup = this.strokeAttri;
			this.optBox2.myValue = "2ptF";
			//this.optBox2.BackColor = Color.SteelBlue;
			this.optBox2.BringToFront();
			//this.optBox2.ishorizontal = true;
			this.optBox2.Main = Main;
			this.Controls.Add(this.optBox2);
			//
			// fittocurve
			//
			this.optBox3.Location = new System.Drawing.Point(this.optBox.Location.X, this.optBox2.Location.Y + this.optBox2.Height);
			this.optBox3.trigger = "fittocurve";
			this.optBox3.mycheckgroup = this.strokeAttri;
			this.optBox3.label = "fit to curve";
			this.optBox3.myValue = "fittocurve";
			this.optBox3.BringToFront();
			//this.optBox3.ishorizontal = true;
			this.optBox3.Main = theForm;
			this.Controls.Add(this.optBox3);
		}
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove (e);
			optBox.OnMouseMove_private_dispatch(new MouseEventArgs(e.Button, e.Clicks, e.X - optBox.Location.X, e.Y - optBox.Location.Y, e.Delta));
			optBox2.OnMouseMove_private_dispatch(new MouseEventArgs(e.Button, e.Clicks, e.X - optBox2.Location.X, e.Y - optBox2.Location.Y, e.Delta));
			optBox3.OnMouseMove_private_dispatch(new MouseEventArgs(e.Button, e.Clicks, e.X - optBox3.Location.X, e.Y - optBox3.Location.Y, e.Delta));
		}
		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter (e);
			Main.Palette.penpanel.releaseCapture();
			Win32Application.Win32.SetCapture(Handle);
			optBox.OnMouseEnter_private_dispatch(e);
			optBox2.OnMouseEnter_private_dispatch(e);
			optBox3.OnMouseEnter_private_dispatch(e);
		}
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave (e);
			optBox.OnMouseLeave_private_dispatch(e);
			optBox2.OnMouseLeave_private_dispatch(e);
			optBox3.OnMouseLeave_private_dispatch(e);
		}

	}

	


}
