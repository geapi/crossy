using System;
using System.Windows.Forms;
using System.Data;
using System.Drawing;

namespace crossy
{
	/// <summary>
	/// Summary description for dispatcher.
	/// </summary>
	public class dispatcher: util.TransparentWindow
	{
		private LinkedList.LinkedList WidgetList;
		private Point current, old;
		private crossy main;
		//private bool mousedown = false;
		//private Dispatchable currentWidget = null;
		public dispatcher(crossy myMain)
		{
			WidgetList = new LinkedList.LinkedList();
			main = myMain;
		}
		protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
		{
			//if(e.Button == MouseButtons.Left)
				//Console.WriteLine(e.X.ToString()+":"+ e.Y.ToString());
			dispatchMouseMove(e);
		}

		protected override void OnMouseHover(EventArgs e)
		{
			dispatchMouseHover(e);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			//main.central_TabControl.get_active_TabPanel().strokecollector.Enabled = false;
			dispatchMouseDown(e);
			Win32Application.Win32.SetCapture(Handle);
			this.Focus();
			//main.central_TabControl.get_active_TabPanel().strokecollector.Enabled = onDrawPanel;
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			dispatchMouseUp(e);	
		}
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint (e);
			//main.Refresh();
		}


	/*	protected override void OnMouseEnter(EventArgs e)
		{

		}*/

		/*protected override void OnMouseLeave(EventArgs e)
		{
			
		}*/

		private void dispatchMouseMove(MouseEventArgs e)
		{
			//main.Refresh();
			if(e.Button == MouseButtons.Left)
			{
				bool onDrawPanel = true;
				int counter = 0;
				
				current = this.PointToScreen(new Point(e.X,e.Y));
				Console.WriteLine(new Point(e.X,e.Y));
				//if(!main.central_TabControl.get_active_TabPanel().Drawing)
				{
					while(GoThroughWidgets(counter)!=null)
					{
						Dispatchable aWidget = GoThroughWidgets(counter);
						if(( (TestIfPointInWidget(old, aWidget)&& !(TestIfPointInWidget(current, aWidget))))&&aWidget.visible)
						{
							aWidget.OnMouseLeave_private_dispatch(new EventArgs());
							onDrawPanel = false;
							//Console.WriteLine("raus "+ aWidget);
						}
						if ((!(TestIfPointInWidget(old, aWidget))&& TestIfPointInWidget(current, aWidget))&&aWidget.visible)
						{
							aWidget.OnMouseEnter_private_dispatch(new EventArgs());
							onDrawPanel = false;
							//Console.WriteLine("rein "+ aWidget);
						}
						// TestIfLineIntersectsWidget(old, current, aWidget) 
						if((TestIfLineIntersectsWidget(old, current, aWidget) || (TestIfPointInWidget(old, aWidget) && TestIfPointInWidget(current, aWidget)))&&aWidget.visible)
						{
							//main.central_TabControl.get_active_TabPanel().strokecollector.
							//DefaultDrawingAttributes.Transparency = 255;
							aWidget.OnNewLine(old, current);
							onDrawPanel = false;
							//Console.WriteLine(aWidget.GetType());
							//Console.WriteLine((GoThroughWidgets(counter)).origin.X+":"+ (GoThroughWidgets(counter)).origin.Y);
							//Console.WriteLine((GoThroughWidgets(counter)).width + "x" +(GoThroughWidgets(counter)).height);
						}
						counter++;
					}
				}
				old = current;
				if(onDrawPanel)
				{
					//main.central_TabControl.get_active_TabPanel().OnMouseMove_private_dispatch(e);
				}
				//main.central_TabControl.get_active_TabPanel().Refresh();
			}
		}
		private void dispatchMouseHover(EventArgs e)
		{
			/*int counter = 0;
			while(GoThroughWidgets(counter)!=null)
			{
				Dispatchable aWidget = GoThroughWidgets(counter);
				if(TestIfPointInWidget(current, aWidget))
					aWidget.OnMouseHover_private_dispatch(e);
				counter++;
			}*/
		}
		private void dispatchMouseDown(MouseEventArgs e)
		{
			old = this.PointToScreen(new Point(e.X,e.Y));
			bool onDrawPanel = true;
			//main.central_TabControl.get_active_TabPanel().strokecollector.Enabled = false;
			//Console.WriteLine(old +" old");
			int counter = 0;
			//main.central_TabControl.get_active_TabPanel().strokecollector.Enabled = false;
			while(GoThroughWidgets(counter)!=null)
			{
				Dispatchable aWidget = GoThroughWidgets(counter);
				//Console.WriteLine("v");
				//Console.WriteLine(GoThroughWidgets(5));
				//Console.WriteLine("---");
				//Console.WriteLine(aWidget.origin2Screen +" " + aWidget.width + " "+ aWidget.height + " " + aWidget.GetType()+ " " + old);
				if((TestIfPointInWidget(this.PointToScreen(new Point(e.X,e.Y)), aWidget))&& aWidget.visible)
				{
					//Console.WriteLine(GoThroughWidgets(counter));
					//Console.WriteLine(aWidget.origin2Screen +" " + aWidget.width + " "+ aWidget.height + " " + aWidget.GetType()+ " " + old);
					//main.central_TabControl.get_active_TabPanel().strokecollector.Enabled = false;
					aWidget.OnMouseDown_private_dispatch(e);
					onDrawPanel = false;
					Console.WriteLine("Maus runter auf: "+ aWidget.GetType());
				}
				counter++;
			}
			if(onDrawPanel)
			{
				//main.central_TabControl.get_active_TabPanel().OnMouseDown_private_dispatch(e);
			}
			else
			{
				//main.central_TabControl.get_active_TabPanel().Reset();
			}
			//main.central_TabControl.get_active_TabPanel().strokecollector.Enabled = onDrawPanel;
			//main.central_TabControl.get_active_TabPanel().strokecollector.AutoRedraw = onDrawPanel;
			//main.central_TabControl.get_active_TabPanel().strokecollector.DynamicRendering = onDrawPanel;	

		}
		private void dispatchMouseUp(MouseEventArgs e)
		{
			//main.central_TabControl.get_active_TabPanel().strokecollector.Enabled = false;
			bool onDrawPanel = true;
			int counter = 0;
			while(GoThroughWidgets(counter)!=null)
			{
				
				Dispatchable aWidget = GoThroughWidgets(counter);
	
				if((TestIfPointInWidget(this.PointToScreen(new Point(e.X,e.Y)), aWidget))&& aWidget.visible)
				{
					aWidget.OnMouseUp_private_dispatch(e);
					onDrawPanel = false;
				}
				counter++;
			}
			if(onDrawPanel)
			{
				//main.central_TabControl.get_active_TabPanel().OnMouseUp_private_dispatch(e);
			}
			//main.central_TabControl.get_active_TabPanel().strokecollector.Enabled = false;
		}

		public void registerWidget(Dispatchable newWidget)
		{
			WidgetList.addAfter(WidgetList.GetFirst(),newWidget);
			Console.WriteLine(newWidget.GetType());//" "+ newWidget.origin +" " + newWidget.origin2Screen);
			//test loop to see if widgets get registered
			/*object tmp = WidgetList.GetFirst();
			while(tmp!=null)
			{
				Console.WriteLine(tmp.GetType());
				tmp = WidgetList.GetNext();
			}*/
			
		}
		public void unregisterWidget(Dispatchable aWidget)
		{
			WidgetList.RemoveNode(aWidget);
		}
		private Dispatchable GoThroughWidgets(int current)
		{
			/*if(previous == null)
			{
				object tmp = WidgetList.GetFirst();
				while(tmp!=null)
				{
					((Dispatchable)tmp).OnMouseMove_private_dispatch(e);
					tmp = WidgetList.GetNext();
					tmp = WidgetList.get_object_from_index
					return tmp;
				}
			}*/
			if(current < WidgetList.Count)
			{
				Dispatchable tmp =  (Dispatchable)WidgetList.get_object_from_index(current);
				//Console.WriteLine(tmp.GetType()+"\t "+ tmp.origin +" " + tmp.origin2Screen);
				//Point WidgetOriginInScreen = this.PointToScreen(tmp.origin);
				//Console.WriteLine("o:"+tmp.origin2Screen +" TSize:" +" " + tmp.width + " "+ tmp.height + " " + tmp.GetType());
				return tmp;
			}
			else
				return null;
		}
		private bool TestIfPointInWidget(Point currentCoord, Dispatchable aWidget)
		{
			//Point WidgetOriginInScreen = this.PointToScreen(aWidget.origin);
			//if( ((currentCoord.X - aWidget.origin2Screen.X>=0)) &&(((aWidget.origin2Screen.X + aWidget.width) - currentCoord.X)  >= 0) &&
			//	((currentCoord.Y - WidgetOriginInScreen.Y)>=0) && (((WidgetOriginInScreen.Y + aWidget.height) - currentCoord.Y) >=0))
			if( ((currentCoord.X - aWidget.origin2Screen.X>=0)) &&(((aWidget.origin2Screen.X + aWidget.width) - currentCoord.X)  >= 0) &&
				((currentCoord.Y - aWidget.origin2Screen.Y)>=0) && (((aWidget.origin2Screen.Y + aWidget.height) - currentCoord.Y) >=0))
			return true;

			else 
				return false;

		}
		
		private bool TestIfLineIntersectsWidget(Point FirstCoord, Point SecondCoord, Dispatchable aWidget)
		{
			line tmp_line_Left = new line(LineOrientation.vertical), tmp_line_Right = new line(LineOrientation.vertical);
			line tmp_line_Top = new line(LineOrientation.horizontal), tmp_line_Bottom = new line(LineOrientation.horizontal);

			tmp_line_Left.TopPoint = aWidget.origin2Screen;
			tmp_line_Left.LowPoint = new Point(aWidget.origin2Screen.X,aWidget.origin2Screen.Y + aWidget.height);
			tmp_line_Right.TopPoint = new Point(aWidget.origin2Screen.X + aWidget.width, aWidget.origin2Screen.Y);
			tmp_line_Right.LowPoint = new Point(aWidget.origin2Screen.X+ aWidget.width,aWidget.origin2Screen.Y + aWidget.height);
			tmp_line_Top.TopPoint = aWidget.origin2Screen;
			tmp_line_Top.LowPoint = new Point(aWidget.origin2Screen.X+aWidget.width,aWidget.origin2Screen.Y);
			tmp_line_Bottom.TopPoint = new Point(aWidget.origin2Screen.X, aWidget.origin2Screen.Y + aWidget.height);
			tmp_line_Bottom.LowPoint = new Point(aWidget.origin2Screen.X+ aWidget.width ,aWidget.origin2Screen.Y + aWidget.height);

			if((HowCrossed)tmp_line_Left.how_crossed(FirstCoord, SecondCoord) != HowCrossed.none ||(HowCrossed)tmp_line_Right.how_crossed(FirstCoord, SecondCoord) != HowCrossed.none||
				(HowCrossed)tmp_line_Top.how_crossed(FirstCoord, SecondCoord) != HowCrossed.none || (HowCrossed)tmp_line_Bottom.how_crossed(FirstCoord, SecondCoord) != HowCrossed.none)
			{
				//Console.WriteLine("line hits target");
				return true;
			}
			/*if(TestIfPointInWidget(FirstCoord, aWidget) && TestIfPointInWidget(SecondCoord, aWidget)) 
				return true;*/
			else
				return false;
		}


	}
}
