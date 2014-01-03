using System;
using System.IO;
using System.Collections.Specialized;
using System.Windows.Forms;
using System.Drawing;

namespace crossy
{

	
	
	public class FileOpenMenu:CrossingDir
	{
		
		public crossy Main;
		public string toOpen;
		public FileOpenMenu(string root, crossy my_main): base(root)
		{

			Main = my_main;

			
		}
		protected override DisplayLevel on_name_selected(DisplayLevel current_level, string name, bool final)
		{
			DisplayLevel result = base.on_name_selected(current_level, name, final);
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
				DirectoryInfo[] sub_directories = base.current_directory.GetDirectories(target_name);
				//Console.WriteLine(name);
				Main.FlowMenu.filemenu.where_info.Text	= base.the_current_directory;
				Main.FlowMenu.filemenu.where_info.Refresh();
				//DirectoryInfo[] sub_directories = current_directory.GetDirectories(name);
				FileInfo[] files = base.current_directory.GetFiles(target_name);
				if (files.Length == 1)
				{
					//Console.Write("OPEN: " + files[0].FullName + "\n");
					toOpen = files[0].FullName;
					
					Main.central_TabControl.get_active_TabPanel().OpenFile(toOpen);
					//Main.FlowMenu.filemenu.Visible = false;
					
				}
			}
			return(result);
		}

		protected override DisplayLevel on_root_reached(DisplayLevel current_level)
		{
			DisplayLevel result = base.on_root_reached(current_level);
			//Console.WriteLine("root");
			Main.FlowMenu.filemenu.where_info.Text	= base.the_current_directory;
			Main.FlowMenu.filemenu.where_info.Refresh();
			
			return(result);
		}

		
		
	}
	public class FileMenuContainer:dialoguebox
	{
		private class FileMenu_MoveButton:MoveButton
		{
			public FileMenu_MoveButton(crossy Main, Point original_location):base(Main, original_location)
			{
				
			}
			public override void back_to_original_position()
			{
				Main.FlowMenu.filemenu.Location = new System.Drawing.Point(0, 200);
				set_initial_position(Main.FlowMenu.filemenu.Location);
			}
			public override void new_position(Point new_position)
			{
				Main.FlowMenu.filemenu.Location = new_position;
				set_initial_position(new_position);
			}
			protected override void OnMouseDown(MouseEventArgs e)
			{
				base.OnMouseDown(e);
				set_initial_position(Main.FlowMenu.filemenu.Location);
				

			}
			
		}
	

		private FileOpenMenu fileOpener;
		public Label where_info;
		private Panel Cover;
		private FileMenu_MoveButton FMmoveButton;
		private Label label_current_directory;

		public override void OnCrossing(HowCrossed fromwhere)
		{
			if ((fromwhere == HowCrossed.fromleft ||fromwhere == HowCrossed.fromtop))
			{
				// low and right, apply changed values\
				//Console.WriteLine("OK");
				//Main.Drawpanel.Erase(eraserType, 0);
				//Main.Palette.ButtonErase.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\eraser.gif");
				//Main.Drawpanel.OpenFile(this.fileOpener.toOpen);
				//Main.FlowMenu.filemenu.Visible = false;
				this.Visible = false;

			}
		
			if ((fromwhere == HowCrossed.frombottom||fromwhere == HowCrossed.fromright))
			{
				//Console.WriteLine("CANCEL");
				//top and left, discharge changed values
				//Main.Palette.ButtonErase.BackgroundImage = Image.FromFile(System.Environment.CurrentDirectory+ @"\pixs\eraser.gif");
				this.Visible = false;
			}
		}

		public FileMenuContainer(crossy Main)
		{
			//
			// this
			//
			this.Main = Main;
			this.Size = new System.Drawing.Size(425, 310);
			this.BackColor = Color.White;
			this.BorderStyle = BorderStyle.FixedSingle;
			//this.Location = new System.Drawing.Point(200,200);
			this.Visible = false;
			Main.Controls.Add(this);
			//
			// move it
			this.FMmoveButton = new FileMenu_MoveButton(Main, this.Location);
			this.FMmoveButton.Location = new Point((this.Width-20)/2,3);
			
			this.FMmoveButton.Size	= new Size(20,17);
			this.FMmoveButton.Main = Main;
			this.FMmoveButton.BackColor = Color.White;
			
			this.Controls.Add(this.FMmoveButton);
			//

			//
			// the label
			//
			this.label_current_directory =new Label();
			this.label_current_directory.Location = new Point(this.Location.X + 15,this.FMmoveButton.Location.Y + this.FMmoveButton.Height+ 5);
			this.label_current_directory.Size = new System.Drawing.Size(170, 20);
			this.label_current_directory.Text = "current directory:";
			this.label_current_directory.BackColor = Color.White;
			this.label_current_directory.Font = new Font("Verdana",10,FontStyle.Bold);
			this.Controls.Add(this.label_current_directory);
			//
			// the label where we are
			//
			string where = System.Environment.CurrentDirectory;

			this.where_info = new Label();
			this.where_info.Text = where;
			this.where_info.BorderStyle = BorderStyle.FixedSingle;
			this.where_info.Location = new Point(this.label_current_directory.Location.X +1 , this.label_current_directory.Location.Y + this.label_current_directory.Height+2);
			this.where_info.Size = new System.Drawing.Size(this.Width - 35, 18);
			this.where_info.TextAlign = ContentAlignment.MiddleLeft;
			this.where_info.Font = new Font("Verdana",10,FontStyle.Bold);
			this.where_info.BackColor = Color.White;
			this.Controls.Add(this.where_info);


			
			this.fileOpener = new FileOpenMenu(where, Main);
			this.fileOpener.SuspendLayout();
			this.fileOpener.Location = new Point(0,0);
			this.fileOpener.Size = new Size(430, 240);
			this.fileOpener.BackColor = Color.White;
			//this.fileOpener.Visible = false;
			this.Controls.Add(fileOpener);

			this.Cover = new Panel();
			this.Cover.BorderStyle  = BorderStyle.FixedSingle;
			this.Cover.Location = new Point(this.where_info.Location.X, this.where_info.Location.Y + this.where_info.Height );
			this.Cover.Size = new Size(390, 225);
			this.Cover.BackColor = Color.White;
			this.Cover.Visible = true;
			this.Cover.BringToFront();
			this.Cover.Controls.Add(this.fileOpener);
			this.Controls.Add(Cover);



			
			

		}
		public void switch_FileMenuContainer(bool on, Point FileMenuPosition)
		{
			
			this.Location = FileMenuPosition;
			this.FMmoveButton.set_initial_position(this.Location);
			this.Visible = on;
			this.BringToFront();
			
		}
		public void set_label_directory()
		{
			//this.where_info.Text = Main.FlowMenu.filemenu.fileOpener.the_current_directory;
		}
		protected override void OnPaint(PaintEventArgs e)
		{
			//base.OnPaint (e);
			Graphics feedback = e.Graphics;
			Pen redPen = new Pen(Color.OrangeRed,3);
			Pen greenPen = new Pen(Color.Olive, 3);
			//feedback.DrawLine(redPen, Location.X, Location.Y, Location.X + Width, Location.Y);
			feedback.DrawLine(greenPen, 0, 0, Width, 0);
			feedback.DrawLine(greenPen, Width-3, 0, Width-3, Height);
			feedback.DrawLine(greenPen, 1, Height-3, Width, Height-3);
			feedback.DrawLine(greenPen, 1, 0, 1, Height);
		
		}

		

		
		



	}
}
