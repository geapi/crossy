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
	

	
	public class erasePanel:dialoguebox
	{
		public class EraserButton: optionbox
		{
			erasePanel container;
			public EraserButton(penattr penStyle, erasePanel container)
			{
				assigned_penStyle = penStyle;
				this.container =container;
				this.Size = new System.Drawing.Size(60, 30);
				this.buttonlinePen = new Pen(Color.Black, 2);
				this.stopperPen = new Pen(Color.Black, 4);
				this.font = new Font("Verdana", 8, FontStyle.Bold);
				this.BackColor = Color.White;
				this.brushcolor = Color.Black;
				Enabled = false;
				
			}

			public override void OnCrossing(string what, HowCrossed fromwhere, bool selected, optionbox thisopt)
			{
				if (fromwhere == HowCrossed.fromleft || fromwhere == HowCrossed.fromright)
				{
					container.inTransit_Eraser.EraserTyp = thisopt;
					container.eraserType = assigned_penStyle;
				}
				if(selected == true)
				{
					thisopt.myradiogroup.selected(thisopt);
					//if (assigned_penStyle == penattr.allerase ||assigned_penStyle == penattr.selecterase)
					//{
					//	thisopt.myradiogroup.unselected(thisopt);
					//}
				}
				if(selected == false)
				{
					thisopt.myradiogroup.unselected(thisopt);
				}
			}
			private penattr assigned_penStyle;
		}
		
		public penattr eraserType;
		option_queue current_Eraser;
		option_queue inTransit_Eraser;
		public EraserButton ButtonEraseAll, ButtonEraseStroke, ButtonErasePoint, ButtonEraseSelected;
		public radiogroup erasegroup;
		public override void OnCrossing(HowCrossed fromwhere)
		{
			if ((fromwhere == HowCrossed.fromright ||fromwhere == HowCrossed.fromtop))
			{
				// low and right, apply changed values\
				//Console.WriteLine("OK");
				
				Main.central_TabControl.get_active_TabPanel().Erase(eraserType, 0);
				if (eraserType == penattr.selecterase|| eraserType == penattr.allerase)
				{
					Main.Palette.ButtonErase.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\eraser.gif");
					Main.Palette.penButton.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\penon.gif");
					//Main.Palette.erasepanel.ButtonEraseAll.myradiogroup.current.BackColor = Color.Orange;
					//Main.Palette.erasepanel.ButtonEraseAll.myradiogroup.current = null;
				}
				current_Eraser.EraserTyp = inTransit_Eraser.EraserTyp;
				this.Visible = false;

			}
		
			if ((fromwhere == HowCrossed.frombottom||fromwhere == HowCrossed.fromleft))
			{
				//Console.WriteLine("CANCEL");
				//top and left, discharge changed values
				current_Eraser.EraserTyp.myradiogroup.selected(current_Eraser.EraserTyp);
				this.Invalidate();
				//Main.Palette.ButtonErase.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\eraser.gif");
				this.Visible = false;
			}
			Win32Application.Win32.ReleaseCapture();
		}
		protected override void OnMouseUp(MouseEventArgs e)
		{
			//this.Visible =false;
		}
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove (e);
			ButtonEraseAll.OnMouseMove_private_dispatch(new MouseEventArgs(e.Button, e.Clicks, e.X - ButtonEraseAll.Location.X, e.Y - ButtonEraseAll.Location.Y, e.Delta));
			ButtonEraseStroke.OnMouseMove_private_dispatch(new MouseEventArgs(e.Button, e.Clicks, e.X - ButtonEraseStroke.Location.X, e.Y - ButtonEraseStroke.Location.Y, e.Delta));
			ButtonErasePoint.OnMouseMove_private_dispatch(new MouseEventArgs(e.Button, e.Clicks, e.X - ButtonErasePoint.Location.X, e.Y - ButtonErasePoint.Location.Y, e.Delta));
			ButtonEraseSelected.OnMouseMove_private_dispatch(new MouseEventArgs(e.Button, e.Clicks, e.X - ButtonEraseSelected.Location.X, e.Y - ButtonEraseSelected.Location.Y, e.Delta));
		}
		public erasePanel(crossy theForm)
		{
			crossy Main = theForm;
			this.ButtonEraseAll = new EraserButton(penattr.allerase, this);
			this.ButtonEraseStroke =  new EraserButton(penattr.strokeerase, this);
			this.ButtonErasePoint =  new EraserButton(penattr.pointerase, this);
			this.ButtonEraseSelected =  new EraserButton(penattr.selecterase, this);
			this.erasegroup = new radiogroup();
			//
			// ButtonErasePoint on erasepanel
			//
			this.Controls.Add(this.ButtonErasePoint);
			this.ButtonErasePoint.SuspendLayout();
			this.ButtonErasePoint.myradiogroup = this.erasegroup;
			this.ButtonErasePoint.Location = new System.Drawing.Point(10, 10);
			this.ButtonErasePoint.label = "point";
			this.ButtonErasePoint.trigger = "point";
			this.ButtonErasePoint.BringToFront();
			this.ButtonErasePoint.Main = Main;
			this.ButtonErasePoint.myradiogroup.selected(ButtonErasePoint);

			current_Eraser.EraserTyp = ButtonErasePoint;
			//
			// ButtonEraseStroke on erasepanel
			//
			this.Controls.Add(this.ButtonEraseStroke);
			this.ButtonEraseStroke.SuspendLayout();
			this.ButtonEraseStroke.myradiogroup = this.erasegroup;
			this.ButtonEraseStroke.Location = new System.Drawing.Point(this.ButtonErasePoint.Location.X, this.ButtonErasePoint.Location.Y + this.ButtonErasePoint.Height);
			this.ButtonEraseStroke.label = "stroke";
			this.ButtonEraseStroke.trigger = "stroke";
			this.ButtonEraseStroke.BringToFront();
			this.ButtonEraseStroke.Main = Main;
			//
			// ButtonEraseAll on erasepanel
			//
			this.Controls.Add(this.ButtonEraseAll);
			this.ButtonEraseAll.SuspendLayout();
			this.ButtonEraseAll.myradiogroup = this.erasegroup;
			this.ButtonEraseAll.Location = new System.Drawing.Point(this.ButtonErasePoint.Location.X, this.ButtonEraseStroke.Location.Y + this.ButtonEraseStroke.Height);
			this.ButtonEraseAll.label = "all";
			this.ButtonEraseAll.trigger = "selected";
			this.ButtonEraseAll.BringToFront();
			this.ButtonEraseAll.Main = Main;
			//
			// ButtonEraseAll on erasepanel
			//
			this.Controls.Add(this.ButtonEraseSelected);
			this.ButtonEraseSelected.SuspendLayout();
			this.ButtonEraseSelected.myradiogroup = this.erasegroup;
			this.ButtonEraseSelected.Location = new System.Drawing.Point(this.ButtonErasePoint.Location.X, this.ButtonEraseAll.Location.Y + this.ButtonEraseAll.Height);
			this.ButtonEraseSelected.label = "selected";
			this.ButtonEraseSelected.trigger = "selected";
			this.ButtonEraseSelected.BringToFront();
			this.ButtonEraseSelected.Main = Main;
		}
	}
	

}
