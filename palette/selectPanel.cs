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
	
	public class selectPanel:dialoguebox
	{
		public class SelectButton: optionbox
		{
			selectPanel container;
			public SelectButton(penattr penStyle, selectPanel container)
			{
				assigned_penStyle = penStyle;
				this.container = container;
				this.SuspendLayout();
				this.Size = new System.Drawing.Size(60, 30);
				this.buttonlinePen = new Pen(Color.Black, 2);
				this.stopperPen = new Pen(Color.Black, 4);
				this.font = new Font("Verdana", 8, FontStyle.Bold);
				this.BackColor = Color.White;
				Enabled = false;

			}
			public override void OnCrossing(string what, HowCrossed fromwhere, bool selected, optionbox thisopt)
			{
				if (fromwhere == HowCrossed.fromleft || fromwhere == HowCrossed.fromright)
				{
					container.selectType = assigned_penStyle;
					container.inTransit_Selector.SelectType = thisopt;
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
			private penattr assigned_penStyle;
		}
		public class CopyButton: button
		{
			selectPanel container;
			public CopyButton(selectPanel container)
			{
				ishorizontal = false;
				brushcolor = Color.Black;
				this.container = container;
				this.Size = new System.Drawing.Size(60, 30);
				this.font = new Font("Verdana", 8, FontStyle.Bold);
				this.buttonlinePen = new Pen(Color.Black, 2);
				this.stopperPen = new Pen(Color.Black, 4);
				this.BackColor = Color.White;
				Enabled = false;
			}
			public override void OnCrossing(HowCrossed fromwhere)
			{
				container.ButtonCopy.between= new Pen(Color.Orange, 2);
				container.ButtonCopy.Invalidate();
				container.Main.central_TabControl.get_active_TabPanel().copy_selection();
			}
			protected override void OnMouseLeave(EventArgs e)
			{
				base.OnMouseLeave(e);
				container.ButtonCopy.between = new Pen(Color.Transparent, 2);
				container.ButtonCopy.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\find_replace_normal.gif");
				container.ButtonCopy.Invalidate();
			}
			protected override void OnMouseUp(MouseEventArgs e)
			{
				container.ButtonCopy.between = new Pen(Color.Transparent, 2);
				container.ButtonCopy.Invalidate();		
			}
			protected override void OnMouseEnter(EventArgs e)
			{
				base.OnMouseEnter (e);
			}
			public void init()
			{
				container.ButtonCopy.between = new Pen(Color.Transparent, 2);
				container.ButtonCopy.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\find_replace_normal.gif");
				container.ButtonCopy.Invalidate();
			}

		}
		public class PasteButton: button
		{
			selectPanel container;
			public PasteButton(selectPanel container)
			{
				ishorizontal = false;
				brushcolor = Color.Black;
				this.container = container;
				this.Size = new System.Drawing.Size(60, 30);
				this.font = new Font("Verdana", 8, FontStyle.Bold);;
				this.buttonlinePen = new Pen(Color.Black, 2);
				this.stopperPen = new Pen(Color.Black, 4);
				this.BackColor = Color.White;
				Enabled = false;
			}
			public override void OnCrossing(HowCrossed fromwhere)
			{
				container.ButtonPaste.between= new Pen(Color.Orange, 2);
				container.ButtonPaste.Invalidate();
				container.Main.central_TabControl.get_active_TabPanel().paste_selection();
			}
			protected override void OnMouseLeave(EventArgs e)
			{
				base.OnMouseLeave(e);
				container.ButtonPaste.between = new Pen(Color.Transparent, 2);
				container.ButtonPaste.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\find_replace_normal.gif");
				container.ButtonPaste.Invalidate();
			}
			protected override void OnMouseUp(MouseEventArgs e)
			{
				container.ButtonPaste.between = new Pen(Color.Transparent, 2);
				container.ButtonPaste.Invalidate();		
			}
			protected override void OnMouseEnter(EventArgs e)
			{
				base.OnMouseEnter (e);
			}
			public void init()
			{
				container.ButtonPaste.between = new Pen(Color.Transparent, 2);
				container.ButtonPaste.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\find_replace_normal.gif");
				container.ButtonPaste.Invalidate();
			}
		}
		private SelectButton ButtonSelectRect, ButtonSelectAll;
		protected CopyButton ButtonCopy;
		public PasteButton ButtonPaste;

		private penattr selectType;
		option_queue current_Selector;
		option_queue inTransit_Selector;
		public radiogroup selectgroup;
		public override void OnCrossing(HowCrossed fromwhere)
		{
			if ((fromwhere == HowCrossed.fromright ||fromwhere == HowCrossed.fromtop))
			{
				// low and right, apply changed values\
				//Console.WriteLine("OK");
				
				Main.central_TabControl.get_active_TabPanel().mySelect(selectType);
				current_Selector.SelectType = inTransit_Selector.SelectType;
				this.ButtonPaste.init();
				this.ButtonCopy.init();
				this.Visible = false;
				
				//Main.Palette.ButtonSelect.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\lasso.gif");
			}
			if ((fromwhere == HowCrossed.frombottom||fromwhere == HowCrossed.fromleft))
			{
				//Console.WriteLine("CANCEL");
				//top and left, discharge changed values
				current_Selector.SelectType.myradiogroup.selected(current_Selector.SelectType);
				this.ButtonPaste.init();
				this.ButtonCopy.init();
				this.Visible = false;

				//Main.Palette.ButtonSelect.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\lasso.gif");
			}
			Win32Application.Win32.ReleaseCapture();
		}
		protected override void OnMouseUp(MouseEventArgs e)
		{
			ButtonCopy.OnMouseUp_private_dispatch(new MouseEventArgs(e.Button, e.Clicks, e.X - ButtonCopy.Location.X, e.Y - ButtonCopy.Location.Y, e.Delta));
			ButtonPaste.OnMouseUp_private_dispatch(new MouseEventArgs(e.Button, e.Clicks, e.X - ButtonPaste.Location.X, e.Y - ButtonPaste.Location.Y, e.Delta));
	
		}
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove (e);
			ButtonSelectRect.OnMouseMove_private_dispatch(new MouseEventArgs(e.Button, e.Clicks, e.X - ButtonSelectRect.Location.X, e.Y - ButtonSelectRect.Location.Y, e.Delta));
			ButtonSelectAll.OnMouseMove_private_dispatch(new MouseEventArgs(e.Button, e.Clicks, e.X - ButtonSelectAll.Location.X, e.Y - ButtonSelectAll.Location.Y, e.Delta));
			ButtonCopy.OnMouseMove_private_dispatch(new MouseEventArgs(e.Button, e.Clicks, e.X - ButtonCopy.Location.X, e.Y - ButtonCopy.Location.Y, e.Delta));
			ButtonPaste.OnMouseMove_private_dispatch(new MouseEventArgs(e.Button, e.Clicks, e.X - ButtonPaste.Location.X, e.Y - ButtonPaste.Location.Y, e.Delta));
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave (e);
			ButtonSelectRect.OnMouseLeave_private_dispatch(e);
			ButtonSelectAll.OnMouseLeave_private_dispatch(e);
			ButtonCopy.OnMouseLeave_private_dispatch(e);
			ButtonPaste.OnMouseLeave_private_dispatch(e);

		}


		public selectPanel(crossy theForm)
		{
			crossy Main = theForm;
			this.selectgroup = new radiogroup();

			this.ButtonSelectRect = new SelectButton(penattr.selfselect, this);
			this.ButtonSelectAll = new SelectButton(penattr.allselect, this);

			this.ButtonSelectRect = new SelectButton(penattr.selfselect, this);
			this.ButtonSelectAll = new SelectButton(penattr.allselect, this);
			ButtonCopy = new CopyButton(this);
			ButtonPaste = new PasteButton(this);

			//
			// ButtonSelectAll 
			//
			this.Controls.Add(this.ButtonSelectAll);
			this.ButtonSelectAll.myradiogroup =this.selectgroup;
			this.ButtonSelectAll.Location = new System.Drawing.Point(10, 10);
			this.ButtonSelectAll.label = "all";
			this.ButtonSelectAll.trigger = "all-s";
			this.ButtonSelectAll.BringToFront();
			this.ButtonSelectAll.Main = Main;
			//
			// ButtonSelectRect 
			//
			this.Controls.Add(this.ButtonSelectRect);
			this.ButtonSelectRect.myradiogroup =this.selectgroup;
			this.ButtonSelectRect.Location = new System.Drawing.Point(this.ButtonSelectAll.Location.X, this.ButtonSelectAll.Location.Y + this.ButtonSelectAll.Height);
			this.ButtonSelectRect.label = "lasso";
			this.ButtonSelectRect.trigger = "lasso";
			this.ButtonSelectRect.BringToFront();
			this.ButtonSelectRect.Main = Main;
			ButtonSelectRect.myradiogroup.selected(ButtonSelectRect);
			current_Selector.SelectType = ButtonSelectRect;

			//
			// CopyButton
			//
			this.Controls.Add(this.ButtonCopy);
			this.ButtonCopy.Location = new System.Drawing.Point(this.ButtonSelectRect.Location.X, this.ButtonSelectRect.Location.Y + this.ButtonSelectRect.Height);
			this.ButtonCopy.label = "copy";
			this.ButtonCopy.trigger = "copy-s";
			this.ButtonCopy.BringToFront();
			ButtonCopy.Main = Main;

			//
			// CopyButton
			//
			this.Controls.Add(this.ButtonPaste);
			this.ButtonPaste.Location = new System.Drawing.Point(this.ButtonCopy.Location.X, this.ButtonCopy.Location.Y + this.ButtonCopy.Height);
			this.ButtonPaste.label = "paste";
			this.ButtonPaste.trigger = "paste-s";
			this.ButtonPaste.BringToFront();
			ButtonPaste.Main = Main;
			
		}
	}


}
