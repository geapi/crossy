using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;

using System.Windows.Forms;
using System.Data;
using Microsoft.Ink;




namespace crossy
{
	
	// container for the panel or the textfield
	public class containerPanel:System.Windows.Forms.Panel
	{
		public crossy Main;
		
		public containerPanel()
		{	
		}
		public void changePos(int x, int y, bool finescroll)
		{
			this.AutoScrollMinSize = new System.Drawing.Size(10, 20);
			if (finescroll)
			{
			   this.AutoScrollPosition = new Point(0, -1*y);
			}
			else
				this.AutoScrollPosition = new Point(0, -1*(this.AutoScrollPosition.Y)+y);

			// set the scrollthing to top or button
			if(this.AutoScrollPosition ==new Point(0,0)) {Main.Slider.scrollthingY1 = 21;}
			
			if(this.AutoScrollPosition ==new Point(0,-(Main.central_TabControl.get_active_TabPanel().Height-Main.panel.Height)-1)) 
			{
				Main.Slider.scrollthingY1 = (Main.panel.Height -20)- Main.Slider.scrollthinglength;
			}
			

		}
		protected override void OnPaint(PaintEventArgs e)
		{
			//Console.WriteLine(AutoScrollPosition);
			
		}

		
	}

}

