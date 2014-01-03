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
	/// the basic radiogroup class
	/// </summary>
	public class C_radiogroup
	{

		protected Selectable current = null;

		public C_radiogroup()
		{
			
		}
		public void selected(Selectable thisopt)
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
				//current.Invalidate();
				current = thisopt;
			
			}
			else if (current == thisopt)
			{
				
			}
			OnSelected(current);
			//current.Invalidate();
			
		}
		public void unselected(Selectable thisopt)
		{
			
			
			if (current == thisopt)
			{

				current.Active = false;
				//current.Invalidate();
				current = null;
				OnDeSelected(thisopt);
			
			}
			
		}
		public virtual void OnSelected(Selectable current)
		{
		}
		public virtual void OnDeSelected(Selectable old)
		{
		}
		public virtual void FirstSelected(Selectable thisoption)
		{
		}
	}
	
}
