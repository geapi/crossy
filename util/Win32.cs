using System;
using System.Runtime.InteropServices;

namespace Win32Application // Directly from the web: http://www.c-sharpcorner.com/Code/2002/Nov/win32api.asp (by Shrijeet Nair)
{
  public class Win32
    {
	  [DllImport("User32.dll")]
	  public static extern bool ReleaseCapture();
	  [DllImport("User32.dll")]
	  public static extern IntPtr SetCapture(IntPtr hWnd);
	  [DllImport("user32.dll", EntryPoint = "LoadCursorFromFile", CharSet =  CharSet.Unicode)]
	  public static extern IntPtr LoadCursorFromFile(string str);
    }
}