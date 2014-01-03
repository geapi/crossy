using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace crossy.util
{
	/// <summary>
	/// small util that loads cursors from files and has method that changes the cursor
	/// </summary>
	public class CursorSwitcher
	{
		static System.Windows.Forms.Cursor Cursor_hover;
		static System.Windows.Forms.Cursor Cursor_down;
		static IntPtr Cursor_down_ptr;
		static IntPtr Cursor_hover_ptr;
		static CursorSwitcher()
		{
			Cursor_down_ptr  = Win32Application.Win32.LoadCursorFromFile(@"cursors/pen_down.cur");
			Cursor_hover_ptr = Win32Application.Win32.LoadCursorFromFile(@"cursors/pen_up.cur");
			Cursor_down = new System.Windows.Forms.Cursor(Cursor_down_ptr);
			Cursor_hover = new System.Windows.Forms.Cursor(Cursor_hover_ptr);
		}
		public static System.Windows.Forms.Cursor change_mouse_cursor(bool down)
		{
			if(down)
			{
				return System.Windows.Forms.Cursors.Default; //Cursor_down;
			}
			else
			{	
				return System.Windows.Forms.Cursors.Default;//Cursor_hover;
			}
		}
	}
}
