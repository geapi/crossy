using System;
using System.IO;
using System.Collections.Specialized;
using System.Drawing;

using System.Runtime.InteropServices;


namespace Win32Application  // http://samples.gotdotnet.com
{
	public class SoundAPI
	{
		private enum Flags 
		{
			SND_SYNC = 0x0000,  /* play synchronously (default) */
			SND_ASYNC = 0x0001,  /* play asynchronously */
			SND_NODEFAULT = 0x0002,  /* silence (!default) if sound not found */
			SND_MEMORY = 0x0004,  /* pszSound points to a memory file */
			SND_LOOP = 0x0008,  /* loop the sound until next sndPlaySound */
			SND_NOSTOP = 0x0010,  /* don't stop any currently playing sound */
			SND_NOWAIT = 0x00002000, /* don't wait if the driver is busy */
			SND_ALIAS = 0x00010000, /* name is a registry alias */
			SND_ALIAS_ID = 0x00110000, /* alias is a predefined ID */
			SND_FILENAME = 0x00020000, /* name is file name */
			SND_RESOURCE = 0x00040004  /* name is resource name or atom */
		}


		[DllImport("winmm.dll",EntryPoint="PlaySound", SetLastError=true)]
		private extern static int PlaySound(string szSound, IntPtr hMod, int flags);

		public static void play_sound(string file_name)
		{
			PlaySound(file_name, IntPtr.Zero, (int)((Flags.SND_ASYNC | Flags.SND_FILENAME)));
		}
	}
}

namespace crossy
{
	/// <summary>
	/// Summary description for CrossingDir.
	/// </summary>
	public class CrossingDir: CrossingList
	{
		public DirectoryInfo current_directory;
		public string the_current_directory;

		public CrossingDir(string root)
		{
			//
			// TODO: Add constructor logic here
			//
			set_root(root);
			Location = new Point(400,100);
		}
		public void set_root(string new_root)
		{
			current_directory = new DirectoryInfo(new_root);
			load_current_directory();
		}

		private void load_current_directory()
		{
			DirectoryInfo[] sub_directories = current_directory.GetDirectories();
			FileInfo[] files = current_directory.GetFiles();
			StringCollection all_names = new StringCollection();

			for (int i = 0; i < sub_directories.Length; i++)
			{
				all_names.Add(sub_directories[i].Name + "/");
			}
			for (int i = 0; i < files.Length; i++)
			{
				all_names.Add(files[i].Name);
			}
			load_names(all_names);
		}

		protected override DisplayLevel on_name_selected(DisplayLevel current_level, string name, bool final)
		{
			DisplayLevel result = current_level;
			Console.Write("Now in: " + current_directory.FullName + ". Going down\n");
			the_current_directory  = current_directory.FullName;
			if (final)
			{
				string target_name; // = name + "";
				if (name[name.Length - 1] == '/')
				{
					target_name = name.Remove(name.Length - 1,1);
				}
				else
				{
					target_name = name; // target_name.Remove(name.Length - 1,1);
				}
				DirectoryInfo[] sub_directories = current_directory.GetDirectories(target_name);
				FileInfo[] files = current_directory.GetFiles(target_name);
				if (sub_directories.Length == 1)
				{
					Console.Write("Current directory: " + sub_directories[0].FullName + "\n");
					the_current_directory = sub_directories[0].FullName;
					current_directory = sub_directories[0];
					load_current_directory();
					// this.current_level.set_display_prefix(current_directory.Name + "/");
					result = this.current_level;
				}
				else if (files.Length == 1)
				{
					Console.Write("File to open: " + files[0].FullName + "\n");
				}
				Win32Application.SoundAPI.play_sound("Sounds/select.wav");
			}
			else
			{
				//if (!current_level.is_root_level())
				//{
					Win32Application.SoundAPI.play_sound("Sounds/click.wav");
				//}
			}
			return(result);
		}

		protected override DisplayLevel on_root_reached(DisplayLevel current_level)
		{
			DisplayLevel result = current_level;
			string name_in_parent = current_directory.Name;
			Console.Write("Root reached!\n");
			if (current_directory.Parent != null)
			{
				Win32Application.SoundAPI.play_sound("Sounds/select.wav");
				current_directory = current_directory.Parent;
				Console.Write("Current directory: " + current_directory.FullName + "\n");
				load_current_directory();
				select_item(name_in_parent + "/");
				the_current_directory = current_directory.FullName;
				result = this.current_level;
			}
			return(result);
		}


	}
}
