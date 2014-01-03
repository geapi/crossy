using System;
using System.Windows.Forms;
using System.Data;
using System.Drawing;

namespace crossy.util
{
	/// <summary>
	/// A class that provides a real transparent window
	/// </summary>
	public class TransparentWindow: System.Windows.Forms.Panel
	{
		public TransparentWindow()
		{
		}
		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
				
		}
		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams cp = base.CreateParams;
				//cp.ExStyle |= 0x20;
				return cp;
			}
		}
		protected override void OnMove(EventArgs e)
		{
			RecreateHandle();
		}
//		protected override void OnMouseMove(MouseEventArgs e)
//		{
//			if(e.Button == MouseButtons.Left)
//				Console.WriteLine(e.X.ToString()+":"+ e.Y.ToString());
//			//Win32Application.Win32.ReleaseCapture();
//				
//		}

	}
}
