using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;

using System.Windows.Forms;
using System.Data;
using Microsoft.Ink;

namespace crossy
{
	
	
	
	public class checkgroup
	{
		private Pen whitePen = new Pen(Color.White, 2);
		private Pen blackPen = new Pen(Color.Black, 2);
		int counter = 0;
		private checkbox []selectedvalues = new checkbox[10];
		//Stack oldindices = new Stack();
		public checkgroup()
		{
			for(int y= 0; y < 10; y++)
			{
				selectedvalues[y] = null;
			}
		}
		public void selected(checkbox thisopt)
		{
			selectedvalues[counter]=thisopt;
			thisopt.selected = true;
			thisopt.between = blackPen;
			thisopt.Invalidate();
			counter++;
		}
		public void unselected(checkbox thisopt)
		{
			thisopt.between = whitePen;
			thisopt.selected = false;
			thisopt.Invalidate();
			for(int y= 0; y < counter; y++)
			{
				if(selectedvalues[y] == thisopt)
				{
					selectedvalues[y]=null;
					counter--;
				}
			}	
		}
	}
}
