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
	/// used to keep track of active tab
	/// </summary>
	public class tab_radiogroup:radiogroup
	{
		
		public  override void OnSelected(optionbox current)
		{
			current.Active = true ;
		}
		public override void OnDeSelected(optionbox current)
		{
			current.Active = false;
			
		}
		public override void FirstSelected(optionbox thisoption)
		{
			thisoption.Active = true ;
			
		}


	}
	/// <summary>
	/// used to keep track of active panel
	/// </summary>
	public class panel_radiogroup:C_radiogroup
	{
		
		public  void OnSelected(C_TabPanel current)
		{
			current.Active = true ;
		}
		public  void OnDeSelected(C_TabPanel current)
		{
			current.Active = false;
			
			
		}
		public  void FirstSelected(C_TabPanel thisoption)
		{
			thisoption.Active = true ;
			
		}


	}
	
}
