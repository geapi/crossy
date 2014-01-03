using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;

using System.Windows.Forms;
using System.Data;
using Microsoft.Ink;

namespace crossy
{
	
	
	/// <summary>
	/// radiogroup for optionboxes
	/// </summary>
	public class radiogroup
	{
		private Pen whitePen = new Pen(Color.White, 2);
		private Pen blackPen = new Pen(Color.Black, 2);
		private Pen old_IconPen;
		private Pen transparentPen = new Pen(Color.Transparent, 2);
		protected optionbox current;
		public radiogroup()
		{
		}

		public void selected(optionbox thisopt)
		{
			if(current == null)
			{
				current = thisopt;
				FirstSelected(thisopt);
				
				
			}
			else if (current != thisopt)
			{

				current.Active = false;
				OnDeSelected(current);
				current.Invalidate();
				current = thisopt;
			
			}
			else if (current == thisopt)
			{
				
			}
			OnSelected(current);
			current.Invalidate();
			
		}
		public void unselected(optionbox thisopt)
		{
			
			
			if (current == thisopt)
			{

				current.Active = false;
				current.Invalidate();
				current = null;
				OnDeSelected(thisopt);
			
			}
			
		}
	
		public virtual void OnSelected(optionbox current)
		{
			current.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\optbox_over.gif");
		}
		public virtual void OnDeSelected(optionbox current)
		{
			current.between = transparentPen;
			current.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\optbox_normal.gif");
		}
		public virtual void FirstSelected(optionbox thisoption)
		{
			old_IconPen = thisoption.IconPen;
		}


	}
	
}
