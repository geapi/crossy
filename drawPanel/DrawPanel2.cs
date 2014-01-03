using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;

using System.Windows.Forms;
using Microsoft.Ink;
using System.IO;




namespace crossy
{
	
	public class drawPanel2:System.Windows.Forms.Panel, Dispatchable
	{
		// for debugginh only
		//public InkOverlay strokecollector;
		// basic variable for Dispatchable
		// needed for Dispatchable objects 
		public virtual int height
		{
			get {return this.Size.Height;}
			set {this.Height = value;}
		}
		public virtual int width
		{
			get {return this.Size.Width;}
			set {this.Width = value;}
		}
		public virtual Point origin
		{
			//get {return (this.PointToScreen(Location));}
			get {return (Location);}
			set {}
		}
		public virtual Point origin2Screen
		{
			get {return (this.PointToScreen(Location));}
			//get {return (Location);}
			set {}
		}
		public virtual bool visible
		{
			get{return this.Visible;}
			set{Visible = value;}
		}

		//StrokeF currentStroke;
		StrokeCollection allStrokes = new StrokeCollection();
		StrokeCollection selectStrokes = new StrokeCollection();
		StrokeCollection selectedStrokes = new StrokeCollection();
		bool Selecting = false;
		penattr selectTyp;
		PointF prev;
		Point current;
		Color currentColor = Color.Black;
		GraphicsPath currentGraphPath = new GraphicsPath();
		int currentWidth =2;
		public crossy Main;
		private InkMode inkMode = InkMode.Ink;
		private Color inkColor = Color.Black;
		private Color highlightColor = Color.Yellow;
		public bool Drawing = false;
		//private RasterOperation oldRast = RasterOperation.CopyPen;
		//private float oldWidth = 53;
		//private float oldHeight = 1;
		//private PenTip oldPenTip = PenTip.Ball;
		//private Point current;
		//private Stroke currStroke, cancelStroke;

		//private Strokes copied_strokes = null;
		private Rectangle target_Rectangle;

		public Point delta = new Point(0,0);

		public bool firstrun = true;
		Point initial_client_point = new Point();
		Point initial_screen_coords = new Point(0,0);
		Point saved_screen_coords = new Point();
		Point toScreen = new Point();
		Point initial_position_local = new Point();
		Point initial_position_global = new Point();

		/// <summary>
		/// genric drawpanel with functionality
		/// </summary>
		public drawPanel2()
		{
			SetStyle( ControlStyles.AllPaintingInWmPaint, true );
			SetStyle( ControlStyles.DoubleBuffer, true );
			SetStyle( ControlStyles.UserPaint, true );
			SetStyle( ControlStyles.ResizeRedraw, true );
			UpdateStyles();
			//Main.dispatch.registerWidget(this);
			//strokecollector = new InkOverlay(this.Handle);

		}
		public void Reset()
		{
			Drawing = false;
			Selecting = false;
		}
		protected override void OnMouseDown(MouseEventArgs e)
		{
			Drawing = true;
			allStrokes.AddStroke(new StrokeF(currentColor, currentWidth));
			turn_widgets_off();
			prev = new PointF(e.X,e.Y);
			current = new Point(e.X,e.Y+(-Main.panel.AutoScrollPosition.Y));
			if(!Selecting)
			{
				ArrayList strokes = allStrokes.GetStrokes();
				if(strokes.Count!=0)
				{
					foreach (StrokeF aStroke in strokes)
					{
						if(aStroke.GetPoints().Length!=0)
						{
							aStroke.Selected = false;
						}
					
					}

				}
			}
			if(e.Button == MouseButtons.Right)
				call_FlowMenu();
			base.OnMouseDown (e);
		}
		
		protected override void OnMouseUp(MouseEventArgs e)
		{
			Drawing = false;
			if(Selecting)
			{
				if(selectTyp == penattr.selfselect)
					processLasso();
				else if(selectTyp == penattr.allselect)
				{
					ArrayList strokes = allStrokes.GetStrokes();
					if(strokes.Count!=0)
					{
						foreach (StrokeF aStroke in strokes)
						{
							if(aStroke.GetPoints().Length!=0)
							{
								aStroke.Selected = true;
							}
					
						}

					}
				}
			}
			else
			{
				ArrayList strokes = allStrokes.GetStrokes();
				if(strokes.Count!=0)
				{
					foreach (StrokeF aStroke in strokes)
					{
						if(aStroke.GetPoints().Length!=0)
						{
							aStroke.Selected = false;
						}
					
					}

				}
			}
			currentGraphPath = new GraphicsPath();
			selectStrokes.getCurrentStroke().Clear();
			Refresh();
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			if(Drawing)
			{
				Graphics g = CreateGraphics();
				g.SmoothingMode = SmoothingMode.AntiAlias;
				g.SmoothingMode = SmoothingMode.HighQuality;
				//PointF[] current_line = {prevprev,prev, new Point(e.X,e.Y)};
				if(e.Button == MouseButtons.Left)
				{
					
					if(Selecting)
					{
						selectStrokes.getCurrentStroke().AddPoint(new PointF(e.X,e.Y+(-Main.panel.AutoScrollPosition.Y)));
					}
					else
					{
						int where = allStrokes.getCurrentStroke().AddPoint(new PointF(e.X,e.Y+(-Main.panel.AutoScrollPosition.Y)));	
					}
					//Refresh();
					Invalidate();
					//
				}
			
				//prevprev = prev;
				prev = new PointF(e.X,e.Y);
			}
		}

		// the dispatch functions for the private dispatch
		public void OnMouseMove_private_dispatch(MouseEventArgs e)
		{
			OnMouseMove(e);
		}

		public void OnMouseEnter_private_dispatch(EventArgs e)
		{
			OnMouseEnter(e);
		}
		public void OnMouseLeave_private_dispatch(EventArgs e)
		{
			OnMouseLeave(e);
		}

		public void OnMouseDown_private_dispatch(MouseEventArgs e)
		{
			OnMouseDown(e);
			//Console.WriteLine("Mouse down");
		}
		public void OnMouseUp_private_dispatch(MouseEventArgs e)
		{
			OnMouseUp(e);
		}
		public void OnMouseHover_private_dispatch(EventArgs e)
		{
			OnMouseHover(e);
		}
		public void OnNewLine(Point old_coord, Point current)
		{
		}
	
		
		protected override void OnPaint(PaintEventArgs pe)
		{
			Graphics g = pe.Graphics;
			Pen myPen = new Pen( Color.Red,1);
			g.SmoothingMode = SmoothingMode.AntiAlias;
			GraphicsPath graphPath = new GraphicsPath();
			ArrayList strokes = allStrokes.GetStrokes();
			if(strokes.Count!=0)
			{
				foreach (StrokeF aStroke in strokes)
				{
					if(aStroke.GetPoints().Length!=0)
					{
						graphPath = new GraphicsPath();
						graphPath.AddLines(aStroke.GetPoints());
						Pen tmpPen;
						if(aStroke.Selected)
						{
							tmpPen = new Pen(Color.Red,aStroke.Width);
							//tmpPen.
						}

						else
							tmpPen = new Pen(aStroke.Color,aStroke.Width);
						//tmpPen.LineJoin = LineJoin.Round;
						tmpPen.StartCap = LineCap.Round;
						tmpPen.EndCap = LineCap.Round;
						g.DrawPath(tmpPen, graphPath);
						// draws the bounding box
						//g.DrawRectangle(myPen,aStroke.getBoundingBox());
						tmpPen.Dispose();
					}
					
				}
				if(Selecting)
				{
					//ArrayList Selstrokes = selectStrokes.GetStrokes();
					//foreach (StrokeF aStroke in strokes)
					{
						if(selectStrokes.getCurrentStroke().GetPoints().Length!=0)
						{
							graphPath = new GraphicsPath();
							graphPath.AddLines(selectStrokes.getCurrentStroke().GetPoints());
							Pen tmpPen = new Pen(selectStrokes.getCurrentStroke().Color,selectStrokes.getCurrentStroke().Width);
							tmpPen.DashStyle = DashStyle.Dot;
							tmpPen.StartCap = LineCap.Round;
							tmpPen.EndCap = LineCap.Round;
							g.DrawPath(tmpPen, graphPath);
							// draws the bounding box
							//g.DrawRectangle(myPen,aStroke.getBoundingBox());
							tmpPen.Dispose();
						}
					
					}
				}
			}
			// test line for highlighter
			//g.DrawLine(new Pen(Color.Black,5),new Point(0,0),new Point (200,200));
			
		}
	
	
		/*
		 *  methods to modify the strokes
		 * */
		public void changeColor(Color color)
		{
			if(allStrokes.getCurrentStroke().Color == color)
			{
			}
			else
			{
				currentColor = color;
				allStrokes.AddStroke(new StrokeF(currentColor, currentWidth));
			}
		}
		public void changeWidth(int width)
		{
			if(allStrokes.getCurrentStroke().Width == width)
			{
			}
			else
			{
				currentWidth = width;
				allStrokes.AddStroke(new StrokeF(currentColor, currentWidth));
			}
		}
		public void switchHiLighter(bool on)
		{
			if(on)
			{
				currentColor = Color.FromArgb(100, currentColor);
				allStrokes.AddStroke(new StrokeF(currentColor, currentWidth));
				currentWidth = 15;
			}
			else
			{
				currentColor = Color.FromArgb(255, currentColor);
				currentWidth = 2;	
				allStrokes.AddStroke(new StrokeF(currentColor, currentWidth));
			}
			
		}
		
	
		/*
		 * FlowMenu Management
		 */
		 private void call_FlowMenu()
		{
			 
			//Console.WriteLine("init_flow");
			Point current_screen = this.PointToScreen(current);
			Point pixelcoord = current;
			Point theorigin = new Point(0,0);
				
			
			theorigin = this.PointToScreen(this.Location);
			//initial_position_global = this.PointToScreen(pixelcoord);
			toScreen =  this.PointToScreen(pixelcoord);
			if(Main.FlowMenu.firstrun == true)
			{
				initial_position_global = this.PointToScreen(pixelcoord);
				initial_position_local = new System.Drawing.Point(current_screen.X-(125+theorigin.X),
					current_screen.Y-(125+theorigin.Y+(-Main.panel.AutoScrollPosition.Y)));
				Main.FlowMenu.firstrun= false;
			}
				
			delta = new Point(toScreen.X-initial_position_global.X, toScreen.Y-initial_position_global.Y);
			 //delta = new Point(toScreen.X-initial_position_global.X, -Main.panel.AutoScrollPosition.Y-toScreen.Y);
			Main.FlowMenu.Location = new System.Drawing.Point(initial_position_local.X + delta.X 
				,initial_position_local.Y + delta.Y);
			//cancelStroke = currStroke;
				
				
			//strokecollector.DynamicRendering = false;
			//cancelStroke.DrawingAttributes.Transparency = 255;
			//strokecollector.DynamicRendering = true;
				
			this.Refresh();
			Main.FlowMenu.showFlowMenu();
			this.Refresh();
			
		}

		/*
		 * general management
		 * */
		private void turn_widgets_off()
		{
			Main.Palette.penpanel.Visible = false;
			Main.Palette.selectpanel.Visible = false;
			Main.Palette.erasepanel.Visible = false;
			Main.Palette.find_replace.Visible = false;
			Main.Palette.hilipanel.Visible = false;
			Main.Palette.penpanel.strokepanel.Visible = false;
			Main.FlowMenu.Visible = false;
			Main.FlowMenu.filemenu.Visible = false;
		}
		/// <summary>
		/// changes the pen-tip if the nonogramm is used
		/// works if only one value is changed since both values are attached to the palette
		/// </summary>
		/// <param name="color">a color value</param>
		/// <param name="strokethickness">a pen tip thickness value</param>
		public void nomographic_ChangePen(int color, int strokethickness)
		{
			Selecting = false;
			//InkMode=InkMode.Ink;
			Color color_set;
			int stroke_width = 4;
			//Console.WriteLine("Color:"+color+" Thickn.:"+strokethickness+" Saturation:"+saturation);
			//strokecollector.DefaultDrawingAttributes.Width=  200 - strokethickness;
			stroke_width = (150 - strokethickness)/9;
			changeWidth(stroke_width);
			if (0 < color && color  < 12)
			{
				color_set = Color. FromArgb(255,0,0);
				//strokecollector.DefaultDrawingAttributes.Color = Color. FromArgb(255,0,0);
				changeColor(color_set);
				Main.Palette.penButton.feedbackPen = new Pen(color_set, 4);
			}
			if (13 < color && color  < 35)
			{
				color_set = Color.FromArgb(255,192,203);
				//strokecollector.DefaultDrawingAttributes.Color = Color.FromArgb(255,192,203); // pink
				changeColor(color_set);
				Main.Palette.penButton.feedbackPen = new Pen(color_set, 4);
			}
			if (35< color && color  < 60)
			{
				color_set = Color.FromArgb(0,0,255);
				//strokecollector.DefaultDrawingAttributes.Color = Color.FromArgb(0,0,255);
				changeColor(color_set);
				Main.Palette.penButton.feedbackPen = new Pen(color_set, 4);
			}
			if (61< color && color  < 79)
			{
				color_set = Color.FromArgb(173,216,230);
				//strokecollector.DefaultDrawingAttributes.Color = Color.FromArgb(173,216,230); // light blue
				changeColor(color_set);
				Main.Palette.penButton.feedbackPen = new Pen(color_set, 4);
			}
			if (80< color && color  < 119)
			{
				color_set = Color.FromArgb(0,255,0);
				//strokecollector.DefaultDrawingAttributes.Color = Color.FromArgb(0,255,0);
				changeColor(color_set);
				Main.Palette.penButton.feedbackPen = new Pen(color_set, 4);
			}
			if (120< color && color  < 134)
			{
				color_set = Color.FromArgb(255,255,0);
				//strokecollector.DefaultDrawingAttributes.Color = Color.FromArgb(255,255,0); 
				changeColor(color_set);
				Main.Palette.penButton.feedbackPen = new Pen(color_set, 4);// yellow
			}
			if (135< color && color  < 145)
			{
				color_set = Color.FromArgb(255,130,0);
				//strokecollector.DefaultDrawingAttributes.Color = Color.FromArgb(255,130,0);
				changeColor(color_set);
				Main.Palette.penButton.feedbackPen = new Pen(color_set, 4);
			}
			//Main.Palette.penButton.feedbackPen = new Pen(color_set, 4);
			Main.Palette.penButton.Refresh();
			//strokecollector.DefaultDrawingAttributes.Transparency = (byte)(220 - saturation);
			//Console.WriteLine(Color.Orange.R+"|"+Color.Orange.G+"|"+Color.Orange.B);
			
		}
		

		/*
		 * searching the strokes
		 * */
		/// <summary>
		/// used to find strokes(forwards) for the find and replace panel
		/// the parameters tell which strokes to find
		/// </summary>
		/// <param name="width">the width of the strokes</param>
		/// <param name="color">the thickness of the strokes</param>
		/// <param name="i">the current index of the stroke</param>
		/// <returns>the matched strokes</returns>
		public StrokeF findstrokesforward(float width, Color color,  ref int i)
		{
			
			/*if(strokecollector.Ink.Strokes.Count != 0)
			{
				Strokes findstrokes = strokecollector.Ink.Strokes;
				DrawingAttributes da = new DrawingAttributes(200);
			
				int startidx = i;
				i++;
				if(i>=strokecollector.Ink.Strokes.Count)i=0;

				while(i != startidx)
				{
					if(strokecollector.Ink.Strokes[i].DrawingAttributes.Width == 
						width && strokecollector.Ink.Strokes[i].DrawingAttributes.Color == color  
						&& strokecollector.Ink.Strokes[i].DrawingAttributes.Transparency != 255)	
					{
								
					
						return strokecollector.Selection = 
							strokecollector.Ink.CreateStrokes(new int []
										{ strokecollector.Ink.Strokes[i].Id  });
					}
					i++;
					if(i>=strokecollector.Ink.Strokes.Count)i=0;
				}
			
				if(strokecollector.Ink.Strokes[i].DrawingAttributes.Width == width 
					&& strokecollector.Ink.Strokes[i].DrawingAttributes.Color == color
					&& strokecollector.Ink.Strokes[i].DrawingAttributes.Transparency != 255)	
				{
					return strokecollector.Selection = 
						strokecollector.Ink.CreateStrokes(new int [] {strokecollector.Ink.Strokes[i].Id  });
				}
			}*/
			return null;
		}
		/// <summary>
		/// used to find strokes(backwards) for the find and replace panel
		/// the parameters tell which strokes to find
		/// </summary>
		/// <param name="width">the width of the strokes</param>
		/// <param name="color">the thickness of the strokes</param>
		/// <param name="i">the current index of the stroke</param>
		/// <returns>the matched strokes</returns>
		public StrokeF findstrokesreverse(float width, Color color,  ref int i)
		{
		/*if(strokecollector.Ink.Strokes.Count != 0)
			{
				Strokes findstrokes = strokecollector.Ink.Strokes;
				int startidx = i;
				i--;
				if(i<0)i = strokecollector.Ink.Strokes.Count-1;
				while(i != startidx)
				{
					if(strokecollector.Ink.Strokes[i].DrawingAttributes.Width == width 
						&& strokecollector.Ink.Strokes[i].DrawingAttributes.Color == color
						&& strokecollector.Ink.Strokes[i].DrawingAttributes.Transparency != 255)	
					{
						return 
							strokecollector.Selection = 
							strokecollector.Ink.CreateStrokes(new int [] {strokecollector.Ink.Strokes[i].Id  });
					}
					i--;
					if(i<0)i = strokecollector.Ink.Strokes.Count-1;

				}
				if(strokecollector.Ink.Strokes[i].DrawingAttributes.Width == width 
					&& strokecollector.Ink.Strokes[i].DrawingAttributes.Color == color
					&& strokecollector.Ink.Strokes[i].DrawingAttributes.Transparency != 255)	
				{
					return 
						strokecollector.Selection = 
						strokecollector.Ink.CreateStrokes(new int [] { strokecollector.Ink.Strokes[i].Id  });
				}
			}*/
			return null;
		}
		/// <summary>
		/// used to replace strokes which is actually changing the attributes of the strokes
		/// </summary>
		/// <param name="strokes">the strokes to change</param>
		/// <param name="newWidth">the new width</param>
		/// <param name="newColor">the new color</param>
		public void replacestrokes(StrokeF strokes, float newWidth, Color newColor)
		{
			/*Pen thisPen = new Pen(newColor, newWidth);
			DrawingAttributes da = new DrawingAttributes(thisPen);
			if(strokecollector.Ink.Strokes.Count != 0 && strokes != null)
			{
				strokes.ModifyDrawingAttributes(da);
			}
			Refresh();*/
			
		}
		/// <summary>
		/// function to select strokes
		/// </summary>
		/// <param name="how">type of selection (lasso, all)</param>
		public void mySelect(penattr how)
		{
			Selecting = true;
			selectStrokes.AddStroke(new StrokeF(Color.Gray,5));
			selectTyp = how;
			/*switch((int)how)
			{
				case (int)penattr.selfselect:
					//InkMode = InkMode.Select;
					break;
				case (int)penattr.allselect:
					//InkMode =InkMode.Select;
					//strokecollector.Selection = strokecollector.Ink.Strokes;
					break;
				default:
					break;
			}*/
			
		}
		protected void processLasso()
		{
		}

		/// <summary>
		/// function to erase strokes
		/// </summary>
		/// <param name="how">point, strokes, all</param>
		/// <param name="width">thickness of the eraser</param>
		public void Erase(penattr how, int width)
		{
			switch((int)how)
			{
				case (int)penattr.pointerase:
					//strokecollector.DefaultDrawingAttributes.PenTip = PenTip.Ball;
					//InkMode =InkMode.Delete;
					
					//strokecollector.EraserMode = InkOverlayEraserMode.PointErase;
					
					break;
				case (int)penattr.strokeerase:
					//InkMode =InkMode.Delete;
					//strokecollector.EraserMode = InkOverlayEraserMode.StrokeErase;
					break;
				case (int)penattr.selecterase:
					//InkMode =InkMode.Delete;
					//strokecollector.Selection = strokecollector.Ink.Strokes; 
					//strokecollector.Ink.DeleteStrokes(strokecollector.Selection);
					//Refresh();
					//InkMode = InkMode.Ink;
					break;
				case (int)penattr.allerase:
					//InkMode =InkMode.Delete;
					//strokecollector.Selection = strokecollector.Ink.Strokes; 
					//strokecollector.Ink.DeleteStrokes(strokecollector.Ink.Strokes);
					ArrayList strokes = allStrokes.GetStrokes();
					if(strokes.Count!=0)
					{
						foreach (StrokeF aStroke in strokes)
						{
							if(aStroke.GetPoints().Length!=0)
							{
								aStroke.Clear();
							}
					
						}

					}
					Refresh();
					//InkMode = InkMode.Ink;
					break;
				default:
					break;
			}
			
			//InkMode = InkOverlayEraserMode.StrokeErase;
			
		}
			
		public void copy_selection()
		{
			//copied_strokes = strokecollector.Selection;
			//Console.WriteLine("copied");
			
			//target_Rectangle = new Rectangle(new Point(strokecollector.Selection.GetBoundingBox().Location.X - 80, strokecollector.Selection.GetBoundingBox().Location.Y - 30),
			//strokecollector.Selection.GetBoundingBox().Size);
		}
		public void paste_selection()
		{
			/*
			// = new Rectangle(80,20,600,600);
			try
			{
				strokecollector.Ink.AddStrokesAtRectangle(copied_strokes, new Rectangle(new Point((copied_strokes.GetBoundingBox().Location.X ), (copied_strokes.GetBoundingBox().Location.Y)),
					copied_strokes.GetBoundingBox().Size));
				copied_strokes.Move(400,400);
				strokecollector.Selection = copied_strokes;
				this.Refresh();
			}
			catch (Exception e){}
			*/
		}
		public void clear_selection()
		{
			//copied_strokes = null;
		}

		/// <summary>
		/// changes the pen Tip
		/// </summary>
		/// <param name="how"> thickness</param>
		/// <param name="color"> color of the pen</param>
		public void changePen(penattr how, Color color)
		{
			Selecting = false;
			//int x = (int)how;
			
			switch((int)how)
			{
				case (int)penattr.thick:
					changeColor(color);
					changeWidth((int)penattr.thick);
					break;
				case (int)penattr.medium:
					changeColor(color);
					changeWidth((int)penattr.medium);
					break;
				case (int)penattr.small: 
					changeColor(color);
					changeWidth((int)penattr.small);
					break;
				case (int)penattr.highlight: 
					
					currentColor = color;
					switchHiLighter(true);
					//InkMode = InkMode.Highlight;
					//HighlightColor = color;
					break;
				default:
					break;
			}
			
			
		}
		/// <summary>
		/// sets attributes for drawing the strokes
		/// </summary>
		/// <param name="pressure">pressure sensitive</param>
		/// <param name="antialiased">antialiased</param>
		/// <param name="fittocurve">fitted to curve</param>
		public void changeStrokeAttributes(penattr pressure, penattr antialiased, penattr fittocurve )
		{
			/*
			if(pressure == penattr.pressuresensitive)
			{
				strokecollector.DefaultDrawingAttributes.IgnorePressure = false;
			}
			else if (pressure == penattr.none)
			{
				strokecollector.DefaultDrawingAttributes.IgnorePressure = true;
			}
			if(antialiased == penattr.antialiased)
			{
				strokecollector.DefaultDrawingAttributes.AntiAliased = true;
			}
			else if(antialiased == penattr.none)
			{
				strokecollector.DefaultDrawingAttributes.AntiAliased = false;
			}
			if(fittocurve == penattr.fittocurve)
			{
				strokecollector.DefaultDrawingAttributes.FitToCurve = true;
			}
			if(fittocurve == penattr.none)
			{
				strokecollector.DefaultDrawingAttributes.FitToCurve = false;
			}
			*/
		}
		/// <summary>
		/// sets/gets the different inkmodes
		/// </summary>
		public InkMode InkMode
		{
			get { return inkMode; }
			set{}
			/*
			{
				// Simplify things later by ensuring it changes
				if ( value == inkMode )
					return;
       
				strokecollector.Enabled = false;
       
				// Turn off highlighting
				if ( inkMode == InkMode.Highlight )
					SetHighlightMode( false );
					// Remove the old selection
				else if ( inkMode == InkMode.Select )
					strokecollector.Selection = strokecollector.Ink.CreateStrokes();
      
				inkMode = value;
      
				// Deal with the two special cases
				if ( inkMode == InkMode.Disabled )
					return;
				else if ( inkMode == InkMode.Highlight )
				{
					SetHighlightMode( true );
					strokecollector.Enabled = true;
					return;
				}
      
				// InkMode should map directly onto the EditingMode
				strokecollector.EditingMode = ( InkOverlayEditingMode )inkMode;
       
				strokecollector.Enabled = true;
			}*/
		}
		
		/// <summary>
		/// sets/gets the inkcolors
		/// </summary>
		public Color InkColor
		{
			get { return inkColor; }
			set{}
			/*
			{
				inkColor = value;
       
				// Don't bother with highlights, they are separate
				if ( inkMode == InkMode.Highlight )
					return;
       
				strokecollector.Enabled = false;
				try
				{
					strokecollector.DefaultDrawingAttributes.Color = inkColor;
      
				}
				catch
				{
				}
				// Change the color for every selected ink stroke
				if ( inkMode == InkMode.Select )
				{
					foreach( Stroke stroke in strokecollector.Selection )
					{
						ExtendedProperties ep;
						ep = stroke.ExtendedProperties;
          
						// Don't change highlights
						if ( !ep.DoesPropertyExist( HighlightGuid ) )
							stroke.DrawingAttributes.Color = inkColor;
					}
				}
				strokecollector.Enabled = true;
				
			}*/
		}
		
		/// <summary>
		/// sets/gets the highlight colors
		/// </summary>
		public Color HighlightColor
		{
			get { return highlightColor; }
			set{}
			/*
			{
				highlightColor = value;
       
				strokecollector.Enabled = false;
				if ( inkMode == InkMode.Select )
				{
					// Change the color of any selected highlights
					foreach( Stroke stroke in strokecollector.Selection )
					{
						ExtendedProperties ep;
						ep = stroke.ExtendedProperties;
          
						if ( ep.DoesPropertyExist( HighlightGuid ) )
							stroke.DrawingAttributes.Color = value;
					}
				}
       
				// Don't go any further if we aren't highlighting
				if ( inkMode != InkMode.Highlight )
				{
					strokecollector.Enabled = true;
					return;
				}
      
				SetHighlightMode( false );
				SetHighlightMode( true );
				strokecollector.Enabled = true;
			}*/
		}
		/// <summary>
		/// sets or turns of the drawing mode to highlighting to draw "transparent" colors
		/// in order to see the highlighted text
		/// </summary>
		/// <param name="highlighting">bool if in highlighting mode</param>
		private void SetHighlightMode( bool highlighting )
		{
			/*DrawingAttributes da = strokecollector.DefaultDrawingAttributes;
    
			if ( highlighting )
			{
				// Ensure we aren't erasing or selecting
				strokecollector.EditingMode = InkOverlayEditingMode.Ink;

				// Save the old values to go back to later
				oldPenTip = da.PenTip;
				oldWidth = da.Width;
				oldHeight = da.Height;
				oldRast = da.RasterOperation;
      
				// Set our highlighting values
				da.PenTip = PenTip.Rectangle;
				da.Width = 250;
				da.Height = 400;
				da.Color = highlightColor;
				da.RasterOperation = RasterOperation.MaskPen;
			}
			else
			{
				// Reset the old values, or defaults if
				// there aren't any values stored
				da.Color = inkColor;
				da.RasterOperation = oldRast;
				da.Height = oldHeight;
				da.Width = oldWidth;
				da.PenTip = oldPenTip;
			}*/
		}


		/// <summary>
		/// function to save created drawings
		/// </summary>
		/// <param name="name">the name under which the file is to be stored</param>
		public void SaveFile(string name)
		{
			/*byte[] isf;
			ExtendedProperties inkProperties = strokecollector.Ink.ExtendedProperties;
			isf = strokecollector.Ink.Save(PersistenceFormat.InkSerializedFormat);
			try
			{
				FileStream mystream = new FileStream(@"C:\crossy_saved_pictures\"+name+".isf", System.IO.FileMode.Create, System.IO.FileAccess.Write);
				mystream.Write(isf, 0, isf.Length);
				mystream.Close();
			}
			catch (Exception)
			{
				FileStream mystream = new FileStream(@"C:\"+name+".isf", System.IO.FileMode.Create, System.IO.FileAccess.Write);
				mystream.Write(isf, 0, isf.Length);
				mystream.Close();
			}*/
			
		}
		/// <summary>
		/// opens a file
		/// </summary>
		/// <param name="name">name of the file to open</param>
		public void OpenFile(string name)
		{
			/*if(name.EndsWith(".isf"))
			{
				try
				{
					Main.FlowMenu.filemenu.Visible = false;
					FileStream mystream = new FileStream(name, System.IO.FileMode.Open, System.IO.FileAccess.Read);
					Ink loadedInk = new Ink();
					byte[] isfBytes = new byte[mystream.Length];
					mystream.Read(isfBytes, 0, (int)mystream.Length);
					loadedInk.Load(isfBytes);
					strokecollector.Enabled = false;
					strokecollector.Ink = loadedInk;
					strokecollector.Enabled = true;
				}
				catch(Exception e)
				{
				}
			}*/

			
		}
	}




}

