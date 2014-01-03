/*
 * written by Georg Apitz, 2004/05
 * */
using System;
using System.Windows.Forms;
using System.Drawing;

// here we hold all the interfaces
namespace crossy
{
	/// <summary>
	/// selectable interface
	/// </summary>
	public interface Selectable
	{
		bool Active
		{get; set;}
		//void Refresh();
		
	}
	/// <summary>
	/// Dispatchable  Interface
	/// </summary>
	public interface Dispatchable
	{
		void OnMouseMove_private_dispatch(MouseEventArgs e);
		void OnMouseDown_private_dispatch(MouseEventArgs e);
		void OnMouseUp_private_dispatch(MouseEventArgs e);
		void OnMouseHover_private_dispatch(EventArgs e);
		void OnMouseLeave_private_dispatch(EventArgs e);
		void OnMouseEnter_private_dispatch(EventArgs e);
		void OnNewLine(Point First, Point Second);
		Point origin{get; set;}
		Point origin2Screen{get; set;}
		int width{get; set;}
		int height{get; set;}
		bool visible{get;set;}
		//bool isMoving {get ; set; }
	}
}
