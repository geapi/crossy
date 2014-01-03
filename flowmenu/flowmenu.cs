using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;

using System.Windows.Forms;
using System.Data;
using Microsoft.Ink;
using System.IO;

namespace crossy
{
	public class flowmenu:System.Windows.Forms.Control
	{
		
		public crossy Main;
		public bool FlowMenuVisible = false;
		public bool firstrun = true;
		
		// side number
		const long NSides = 8;
		// true if we have a menu
		//the optioncaptionss for the menu
		string []optionCaptions = new string[20];
		
	
		
		private bool []isLeft =  new bool[8];//false;
		private bool []isRight = new bool[8];
		private bool []isTop = new bool[8];
		private bool []isBottom = new bool[8];
		private bool []crossed = new bool[8];
		public System.Drawing.Point crosspoint = new Point(-1,-1);
		public Point oldcoord = new Point(-1,-1);
		public Point landingpoint;
		
		public FileMenuContainer filemenu;
		private line []thelines = new line[8];

		
		int filecounter = 1;
		Point []TopPoint = new Point[8];
		Point []LowPoint = new Point[8];
		
		public virtual void OnCrossing(HowCrossed fromwhere)
		{
			//init_flow();
			//hide_flowmenu();
			
		}
		
	
		public flowmenu(crossy mycrossy)
		{
			Main = mycrossy;
			
			thelines[1]= new line(LineOrientation.tilted_right);
			thelines[3]= new line(LineOrientation.tilted_left);
			thelines[5]= new line(LineOrientation.tilted_right);
			thelines[7]= new line(LineOrientation.tilted_left);
			thelines[0]= new line(LineOrientation.horizontal);
			thelines[4]= new line(LineOrientation.horizontal);
			thelines[2]= new line(LineOrientation.vertical);
			thelines[6]= new line(LineOrientation.vertical);

			filemenu = new FileMenuContainer(mycrossy);
			Main.Controls.Add(filemenu);
		
			this.BackColor = Color.White;
			//drawMenu(60,60,56);
			try 
			{
				System.IO.StreamReader counter_reader = new StreamReader(@"\crossy_saved_pictures\counter.txt");
				string bert  = counter_reader.ReadLine();
				filecounter =  Convert.ToInt32(bert);
				counter_reader.Close();
			}
				//FileStream mystream = new FileStream(@"\crossy_saved_pictures\counter.txt", System.IO.FileMode.Open, System.IO.FileAccess.Read);
			catch (Exception)
			{
				filecounter = 1;
			}
			
			//Console.WriteLine(bert);
			
		}
		protected override void OnMouseMove(MouseEventArgs e)
		{
			///Console.Write("  FM-->   \n");
			if (((e.Button & MouseButtons.Left) != 0) || ((e.Button & MouseButtons.Right) != 0))
			{
				this.Cursor = util.CursorSwitcher.change_mouse_cursor(true);
				//Main.Drawpanel.strokecollector.DynamicRendering = false;
				String filename = "CrossY_drawing_"+filecounter;
				HowCrossed []fromwhere = new HowCrossed[8];
				for(int i =0; i<8; i++)
				{
					fromwhere[i] = HowCrossed.none;
				}
				Point pixelcoord = new Point(e.X,e.Y);
				
				//Console.WriteLine(pixelcoord+" pixelcoord");
				for (int x=0; x<8; x++)
				{
					if((oldcoord.X != pixelcoord.X) || (oldcoord.Y != pixelcoord.Y))
					{
						fromwhere[x] = (HowCrossed)this.thelines[x].how_crossed(oldcoord, pixelcoord);
					}

					//Console.WriteLine(x+" "+fromwhere[x]);
					if(fromwhere[x] != HowCrossed.none)
					{
						
						//Console.WriteLine(fromwhere[x] +"|"+x);
						if(x== 1)
						{
							if(fromwhere[x] == HowCrossed.fromleft)
							{
								crossed[x]=true;
							}
							if(fromwhere[x] == HowCrossed.fromright && crossed[x])
							{
								
								Console.WriteLine("quit");
								//String line = filecounter.ToString();
								/*try 
								{
									System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\crossy_saved_pictures\counter.txt", false);
									file.WriteLine(filecounter);
									file.Close();
								}
								catch (Exception)
								{
									Console.WriteLine("couldn't write filecounter");
								}*/
								Main.Dispose();
							}
						}
						else if(x == 6)
						{
							if(fromwhere[x] == HowCrossed.fromright)
							{
								crossed[x]=true;
							}
							if(fromwhere[x] == HowCrossed.fromleft && crossed[x])
							{
								Console.WriteLine("lasso");
								
								///Main.central_TabControl.get_active_TabPanel().mySelect(penattr.selfselect);
								this.Capture = false;
								hide_flowmenu();
								crossed[x] = false;
							}
							break;

						}
						else if(x == 5)
						{
							if(fromwhere[x] == HowCrossed.fromright)
							{
								crossed[x]=true;
							}
							if(fromwhere[x] == HowCrossed.fromleft && crossed[x])
							{
								Console.WriteLine("open");
								filemenu.switch_FileMenuContainer(true, new Point(this.Location.X - 50, this.Location.Y -50));
								hide_flowmenu();
								crossed[x]=false;
							}
						}
						else if(x == 7)
						{
							if(fromwhere[x] == HowCrossed.fromright)
							{
								crossed[x]=true;
							}
							if(fromwhere[x] == HowCrossed.fromleft && crossed[x])
							{
								Console.WriteLine("save");
								
								Main.central_TabControl.get_active_TabPanel().SaveFile(filename);
								filecounter++;
								hide_flowmenu();
								try 
								{
									System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\crossy_saved_pictures\counter.txt", false);
									file.WriteLine(filecounter);
									file.Close();
								}
								catch (Exception)
								{
									Console.WriteLine("couldn't write filecounter");
								}
								break;
							}
						}
						else if(x == 3)
						{
							if(fromwhere[x] == HowCrossed.fromleft)
							{
								crossed[x]=true;
							}
							if(fromwhere[x] == HowCrossed.fromright && crossed[x])
							{
								Console.WriteLine("add_space");
								
								int heightnow = Main.central_TabControl.get_active_TabPanel().Height;
								
								Main.central_TabControl.get_active_TabPanel().Size = new System.Drawing.Size(Main.central_TabControl.get_active_TabPanel().Width, heightnow+200);
								Main.Slider.Refresh();
								
								hide_flowmenu();
								break;
							}
						}
						else if(x == 2)
						{
							if(fromwhere[x] == HowCrossed.fromleft)
							{
								crossed[x]=true;
							}
							if(fromwhere[x] == HowCrossed.fromright && crossed[x])
							{
								Console.WriteLine("copy");
								
								int heightnow = Main.central_TabControl.get_active_TabPanel().Height;
								
								Main.central_TabControl.get_active_TabPanel().copy_selection();
								//Main.Slider.Refresh();
								
								hide_flowmenu();
								break;
							}
						}
						else if(x == 0)
						{
							if(fromwhere[x] == HowCrossed.fromtop)
							{
								crossed[x]=true;
							}
							if(fromwhere[x] == HowCrossed.frombottom && crossed[x])
							{
								Console.WriteLine("paste");
								
								int heightnow = Main.central_TabControl.get_active_TabPanel().Height;
								
								Main.central_TabControl.get_active_TabPanel().paste_selection();
								//Main.central_TabControl.get_active_TabPanel().clear_selection();
								
								hide_flowmenu();
								break;
							}
						}
					}
					
				}
				oldcoord = pixelcoord;
			}
			else
			{
				this.Cursor = util.CursorSwitcher.change_mouse_cursor(false);
			}
	
		}
		protected override void OnMouseUp(MouseEventArgs e)
		{
			//Console.WriteLine("Mouse Up" + e.Button);
			//if(e.Button != MouseButtons.Right)
			//{
			this.Cursor = util.CursorSwitcher.change_mouse_cursor(false);
				hide_flowmenu();
				
			//}
			
		}
		protected override void OnMouseDown(MouseEventArgs e)
		{
			//Console.Write("v");
			//this.Cursor = util.CursorSwitcher.change_mouse_cursor(true);
			Win32Application.Win32.ReleaseCapture();
			
			
			landingpoint = new Point(e.X,e.Y);
			oldcoord = landingpoint;
			init_flow();

		}
		
		public void init_flow()
		{
			for (int i=0; i<8; i++)
			{
				this.thelines[i].init_line();
				this.crossed[i] = false;
			}
			Main.central_TabControl.get_active_TabPanel().Refresh();
		}
		public void hide_flowmenu()
		{
			firstrun= false;
			this.Visible = false;
			this.Capture = false;

			this.Invalidate();
			Main.central_TabControl.get_active_TabPanel().Invalidate();
			
			FlowMenuVisible = false;
			
			
			
		}

		public void showFlowMenu()
		{
			//this.firstrun = true;
			this.FlowMenuVisible = true;
			this.BringToFront();
			this.Capture = true;
			this.Visible = true;
			
			this.Refresh();
		
		}
		protected override void OnPaint(PaintEventArgs e)
			{
			
			init_flow();
			double X = 140;
			double Y = 120;

			System.Drawing.Drawing2D.LinearGradientBrush myBrush = new 
				System.Drawing.Drawing2D.LinearGradientBrush(ClientRectangle, Color.Black,
				Color.Black, System.Drawing.Drawing2D.LinearGradientMode.Horizontal);
			System.Drawing.StringFormat format= new StringFormat();
			Font schrift = new Font("Verdana", 10);
			Graphics menug = e.Graphics;//CreateGraphics();
			Pen myPen = new Pen(Color.Black, 2);
			
			double sideL = 50;
			double R = (Math.Sqrt(4+(2*Math.Sqrt(2)))/2)*sideL;
			double innerR =((Math.Sqrt(2)+1)/2)*sideL;
			double x1 = X-sideL/2;
			double y1 = Y+innerR;
			double x2 = X+sideL/2;
			double y2 = y1;
			double x3 = X+innerR;
			double y3 = Y+sideL/2;
			double x4 = x3;
			double y4 = y3-sideL;
			double x5 = x2;
			double y5 = Y-innerR;
			double x6 = x1;
			double y6 = y5;
			double x7 = X-innerR;
			double y7 = y4;
			double x8 = x7;
			double y8 = y3;


			// south
			menug.DrawLine(myPen,(int)x1, (int)y1, (int)x2, (int)y2);
			menug.DrawString("paste",schrift ,myBrush,(float)x1+5, (float)y1+20,format);
			this.thelines[0].LowPoint.X = (int)x1;
			this.thelines[0].TopPoint.X = (int)x2;
			this.thelines[0].LowPoint.Y = (int)y1;
			this.thelines[0].TopPoint.Y = (int)y2;
			menug.DrawLine(myPen,(int)x1, (int)y1, (int)(x1-sideL/4), (int)(y1+30));
			menug.DrawLine(myPen,(int)x2, (int)y2, (int)(x2+sideL/4), (int)(y2+30));

			menug.DrawLine(myPen,(int)x2, (int)y2, (int)x3, (int)y3);
			menug.DrawLine(myPen,(int)x3, (int)y3, (int)x4, (int)y4);
			menug.DrawLine(myPen,(int)x3, (int)y3, (int)(x3+30), (int)(y3+sideL/4));
			menug.DrawString("Quit",schrift ,myBrush,(float)x3-15, (float)y3 +25,format);
			menug.DrawLine(myPen,(int)x4, (int)y4, (int)(x4+30), (int)(y4-sideL/4));
			// east
			menug.DrawString("copy",schrift ,myBrush,(float)x4+10, (float)y4 +20,format);
			this.thelines[1].LowPoint.X = (int)x2;
			this.thelines[1].TopPoint.X = (int)x3;
			this.thelines[1].LowPoint.Y = (int)y2;
			this.thelines[1].TopPoint.Y = (int)y3;

			this.thelines[2].LowPoint.X = (int)x3;
			this.thelines[2].TopPoint.X = (int)x4;
			this.thelines[2].LowPoint.Y = (int)y3;
			this.thelines[2].TopPoint.Y = (int)y4;


			menug.DrawLine(myPen,(int)x4, (int)y4, (int)x5, (int)y5);
			menug.DrawString("v-space+",schrift ,myBrush,(float)x5+20, (float)y5-10,format);
			menug.DrawLine(myPen,(int)x5, (int)y5, (int)x6, (int)y6);
			this.thelines[3].LowPoint.X = (int)x4;
			this.thelines[3].TopPoint.X = (int)x5;
			this.thelines[3].LowPoint.Y = (int)y4;
			this.thelines[3].TopPoint.Y = (int)y5;
			
			menug.DrawLine(myPen,(int)x6, (int)y6, (int)x7, (int)y7);
			menug.DrawLine(myPen,(int)x5, (int)y5, (int)(x5+sideL/4), (int)(y5-30));
			menug.DrawLine(myPen,(int)x6, (int)y6, (int)(x6-sideL/4), (int)(y6-30));
			//menug.DrawString("other",schrift ,myBrush,(float)x6+5, (float)y6-30,format);
			this.thelines[4].LowPoint.X = (int)x5;
			this.thelines[4].TopPoint.X = (int)x6;
			this.thelines[4].LowPoint.Y = (int)y5;
			this.thelines[4].TopPoint.Y = (int)y6;
			menug.DrawString("Open",schrift ,myBrush,(float)x6-60, (float)y6-10,format);
			this.thelines[5].LowPoint.X = (int)x7;
			this.thelines[5].TopPoint.X = (int)x6;
			this.thelines[5].LowPoint.Y = (int)y7;
			this.thelines[5].TopPoint.Y = (int)y6;

			menug.DrawLine(myPen,(int)x7, (int)y7, (int)x8, (int)y8);
			menug.DrawLine(myPen,(int)x7, (int)y7, (int)(x7-30), (int)(y7-sideL/4));
			//menug.DrawString("Lasso",schrift ,myBrush,(float)x7-50, (float)y7 +20,format);
			menug.DrawLine(myPen,(int)x8, (int)y8, (int)(x8-30), (int)(y8+sideL/4));
			this.thelines[6].LowPoint.X = (int)x8;
			this.thelines[6].TopPoint.X = (int)x7;
			this.thelines[6].LowPoint.Y = (int)y8;
			this.thelines[6].TopPoint.Y = (int)y7;
			
			menug.DrawLine(myPen,(int)x8, (int)y8, (int)x1, (int)y1);
			menug.DrawString("Save",schrift ,myBrush,(float)x8-30, (float)y8 +30,format);
			this.thelines[7].LowPoint.X = (int)x8;
			this.thelines[7].TopPoint.X = (int)x1;
			this.thelines[7].LowPoint.Y = (int)y8;
			this.thelines[7].TopPoint.Y = (int)y1;
			
			
			
			

			
		}
		
		
	}
}
