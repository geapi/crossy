using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;

using System.Windows.Forms;
using System.Data;
using Microsoft.Ink;
using System.Timers;



namespace crossy
{
	// slider is the scrollbar
	public class slider: System.Windows.Forms.Control
	//	public class Scrollbar: Control
	{
		
		public enum Orienta:int
		{
			SouthEast = 1,
			NorthEast = 2,
			SouthWest = 3 ,
			NorthWest = 4,
			North ,
			East ,
			South,
			West,
			none = 0
		}	
		public enum Transform:int
		{
			NE_45 = 0 ,
			NW_45 = 1 ,
			crossedFL = 2,
			crossedFR = 3,
			directchange = 4,
			SE_45 = 5,
			SW_45 = 6,
			E_0 = 7, 
			N = 8,
			S = 9,
			W = 10,
			none = 11
		}
		public enum states:int
		{
			singleline = 2,
			pageup = 6,
			contpageup = 9,
			pagedown = 13,
			contpagedown = 16
		}
		
		
		// endpoints of the scrollbar
		// somehow we should get these from the defining frame
		public Point scrollTop;
		public Point scrollButt;
		private Point oldcoord = new Point(0,0);
		public double scrollthingY1;
		public int scrollthingY2;
		public int panelheight;

		// for the int angle(Point) function
		int window = 0;
		int counter = 0;
		int counter2 = 0;
		int oldangle = -1;
		
		double averageangle = 0;
		double realaverageangle = 0;
		public Queue shapequeue = new Queue();
		public Queue strokequeue = new Queue();
		public Queue pre_flight_queue = new Queue();
		public Queue post_flight_queue = new Queue();
		bool pre_flight;
		int post_flight_count;
		public Queue processed_shape_queue = new Queue();
		public Queue pure_shape_queue = new Queue();
		double angle;
		public containerPanel container;
		public crossy Main;
		int minpoints =0;  // min number of points before action is triggered
		Point CrossPoint;

		int [,]States = new int[12,19];
		
		// newPackets function
		Point crosscoord, previous, current;
		
		//scrolling
		System.Timers.Timer cont_scroll_timer = new System.Timers.Timer(500);
		bool cont_scroll_down = false;
		bool cont_scroll_up = false;
		bool hovering = false;
		bool finescroll = false;
		bool first_finescroll = true;
		bool NOOLDPOINT = true;
		Point crossPoint;
		

		// the slider in general
		//public InkOverlay strokecollector;
		
		// for HowCrossed

		private line scroll_line;
		

		// statemachine
		int oldstate =  0;
		int state =  0;


		// transform function
		Transform trans = Transform.none;
		Transform oldtrans = Transform.none;

		HowCrossed previouscross = HowCrossed.none;

		Point  old = new Point(999,999), now = new Point(999,999);  
			int angleprint = 999, aVangle = 999;

		
		public int scrollthinglength;
	

		public Point TopPoint;
		public Point LowPoint;

		

		
		/// <summary>
		/// a scrollbar in crossing terms, biggest thing in the constructor
		/// is an array that defines the state machine that is responsible for
		/// changing the postion of the scrollbar-slider
		/// </summary>
		public slider()
		{
			SetStyle( ControlStyles.AllPaintingInWmPaint, true );
			SetStyle( ControlStyles.DoubleBuffer, true );
			SetStyle( ControlStyles.UserPaint, true );
			SetStyle( ControlStyles.ResizeRedraw, true );
			UpdateStyles();
			//container = new containerPanel();
			scrollthingY1 = 21;
			cont_scroll_timer.Elapsed +=new ElapsedEventHandler(cont_scroll_timer_Elapsed);
			this.scroll_line = new line(LineOrientation.vertical);
			
			//this.ResizeRedraw = true;
			
			//Linestatus = new linestatus();

			for(int x = 0; x < 11;  x++)
			{
				for(int y= 0; y < 19; y++)
				{
					States[x,y] = 0;
				}
			}
			///
			/// this defines the state machine in form of an array
			///
			// first is what happens, second is the states it is in
			States[(int)Transform.NE_45,0] = 3;
			//States[(int)Transform.NE_45,1] = 1;
			States[(int)Transform.NE_45,1] = 3;
			States[(int)Transform.NE_45,3] = 3;
			States[(int)Transform.NE_45,4] = 4; //should be 4 now 6
			States[(int)Transform.NE_45,5] = 5;
			States[(int)Transform.NE_45,8] = 8;
			States[(int)Transform.NE_45,10] = 1;
			States[(int)Transform.NE_45,2] = 2;

			States[(int)Transform.NW_45,1] = 1;
			States[(int)Transform.NW_45,5] = 5;
			States[(int)Transform.NW_45,6] = 7;
			States[(int)Transform.NW_45,7] = 7;
			States[(int)Transform.NW_45,10] = 1;
			States[(int)Transform.NW_45,2] = 2;

			States[(int)Transform.crossedFL,1] = (int)states.singleline;
			States[(int)Transform.crossedFL,3] = 4;
			States[(int)Transform.crossedFL,8] = (int)states.contpageup;
			States[(int)Transform.crossedFL,10] = 11; // was 11
			States[(int)Transform.crossedFL,15] = (int)states.contpagedown;
			//States[(int)Transform.crossedFL,17] = 4;
			//States[(int)Transform.crossedFL,18] = 11;

			States[(int)Transform.crossedFR,5] = (int)states.pageup;
			States[(int)Transform.crossedFR,12] = (int)states.pagedown;

			States[(int)Transform.directchange,4] = 5;
			States[(int)Transform.directchange,7] = 8;
			States[(int)Transform.directchange,11] = 12;//
			States[(int)Transform.directchange,14] = 15;

			States[(int)Transform.SE_45,0] = 10;
			States[(int)Transform.SE_45,2] = 2;
			States[(int)Transform.SE_45,1] = 10;
			States[(int)Transform.SE_45,3] = 1;
			States[(int)Transform.SE_45,10] = 10;
			States[(int)Transform.SE_45,11] = 11;
			States[(int)Transform.SE_45,12] = 12;
			States[(int)Transform.SE_45,15] = 15;

			States[(int)Transform.SW_45,3] = 1;
			States[(int)Transform.SW_45,12] = 12;
			States[(int)Transform.SW_45,13] = 14;
			States[(int)Transform.SW_45,14] = 14;
			States[(int)Transform.SW_45,2] = 2;

			States[(int)Transform.E_0,0] = 1;
			States[(int)Transform.E_0,1] = 1;
			States[(int)Transform.E_0,2] = 2;
			States[(int)Transform.E_0,3] = 1;
			States[(int)Transform.E_0,4] = 5;
			States[(int)Transform.E_0,5] = 5;
			States[(int)Transform.E_0,7] = 8;
			States[(int)Transform.E_0,8] = 8;
			States[(int)Transform.E_0,10] = 1;
			States[(int)Transform.E_0,11] = 11;
			States[(int)Transform.E_0,12] = 12;
			States[(int)Transform.E_0,14] = 15;
			States[(int)Transform.E_0,15] = 15;
			
			//States[(int)Transform.SE_45,1] = 10;

			//States[(int)Transform.N,0] = 3;
			States[(int)Transform.N,1] = 3;
			States[(int)Transform.N,3] = 3;
			States[(int)Transform.N,4] = 5;
			States[(int)Transform.N,5] = 5;
			States[(int)Transform.N,6] = 7;
			States[(int)Transform.N,7] = 8;
			States[(int)Transform.N,8] = 8;
			States[(int)Transform.N,10] = 1;
			States[(int)Transform.N,11] = 2;
			States[(int)Transform.N,2] = 2;

			States[(int)Transform.S,0] = 10;
			States[(int)Transform.S,1] = 10;
			States[(int)Transform.S,2] = 2;
			States[(int)Transform.S,3] = 1;
			States[(int)Transform.S,4] = 2;
			States[(int)Transform.S,10] = 10;
			States[(int)Transform.S,11] = 12;
			States[(int)Transform.S,12] = 12;
			States[(int)Transform.S,13] = 14;
			States[(int)Transform.S,14] = 15;
			States[(int)Transform.S,15] = 15;
			States[(int)Transform.S,6] = 17;

			//States[(int)Transform.W,0] = 1;
			States[(int)Transform.W,2] = 2;
			States[(int)Transform.W,5] = 5;
			States[(int)Transform.W,6] = 7;
			States[(int)Transform.W,7] = 7;
			//States[(int)Transform.W,10] = 1;
			States[(int)Transform.W,11] = 12;
			States[(int)Transform.W,12] = 12;
			States[(int)Transform.W,13] = 14;
			States[(int)Transform.W,14] = 14;
			
			

			
			
			
		}

		/// <summary>
		/// responsible for drawing the rigth scroll-slider in the right position
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaint(PaintEventArgs e)
		{
			//
			
			double tmp = (double)this.Main.central_TabControl.get_active_TabPanel().Height/(double)731;
			double scrollthinglengthTMP = (double)(Main.panel.Height-40)/(tmp);
			scrollthinglength = (int)scrollthinglengthTMP;
			//scrollthingY2 = scrollthingY1 + scrollthinglength ;
			scrollthingY2 = (int)scrollthingY1 + scrollthinglength;
			
			//Console.WriteLine(Scale);
			scrollTop = new Point(20,0);
			scrollButt = new Point(20,Main.Height);
			this.scroll_line.TopPoint = new Point(this.scrollTop.X, this.scrollTop.Y);
			this.scroll_line.LowPoint = new Point(this.scrollButt.X, this.scrollButt.Y);
			//drawScrolly();
			//Console.WriteLine(scrollthinglength);
			Pen scrolllinePen = new Pen(Color.Blue, 3);
			Pen controlPen = new Pen(Color.Red, 3);
			// top and buttom
			Pen stopperPen = new Pen(Color.Black, 8);
			// the thing that scrolls
			Pen scrollthingPen = new Pen(Color.Blue, 7);
			
			Graphics scroll = e.Graphics;
			scroll.DrawLine(controlPen, scrollTop.X, scrollTop.Y, scrollButt.X, scrollButt.Y);
			scroll.DrawLine(scrolllinePen, scrollTop.X, scrollTop.Y, scrollButt.X, scrollButt.Y);
			// top stopper
			scroll.DrawLine(stopperPen, 20, 10 , 20, 0 + 20);
			//Console.WriteLine(Main.panel.Height);
			//buttom stopper
			scroll.DrawLine(stopperPen, 20, Main.panel.Height -20, 20, Main.panel.Height -10);
			// the scroll thing
			if( (scrollthingY1 + scrollthinglength) >  (Main.panel.Height - 21))
			{
				scroll.DrawLine(scrollthingPen, 20, (Main.panel.Height -21)- scrollthinglength
					, 20, Main.panel.Height -21);
				
			}
			else if ((scrollthingY2 - scrollthinglength) < (21))
			{
				scroll.DrawLine(scrollthingPen, 20, 21, 20, 21 + scrollthinglength );
				
			}
			else
				scroll.DrawLine(scrollthingPen, 20, (int)scrollthingY1, 20, scrollthingY2);

		{
			System.Collections.IEnumerator myEnumerator = processed_shape_queue.GetEnumerator();
			Pen test_pen = new Pen(System.Drawing.Color.Red, 4);
			Point current_point = new Point(100,250);
			Point previous_point = new Point(100,250);
			int step_size = 1; // points
			Point delta_point;
			while ( myEnumerator.MoveNext() )
			{
				switch ((Transform)myEnumerator.Current)
				{
					case Transform.NE_45:
						delta_point = new Point(step_size, -step_size);
						break;
					case Transform.NW_45:
						delta_point = new Point(-step_size, -step_size);
						break;
					case Transform.SE_45:
						delta_point = new Point(step_size, step_size);
						break;
					case Transform.SW_45:
						delta_point = new Point(-step_size, step_size);
						break;
					case Transform.E_0:
						delta_point = new Point(step_size, -0);
						break;
					case Transform.N:
						delta_point = new Point(0, -step_size);
						break;
					case Transform.S:
						delta_point = new Point(0, step_size);
						break;
					case Transform.W:
						delta_point = new Point(-step_size, 0);
						break;
					default:
						delta_point = new Point(0, 0);
						break;
				}
				/*previous_point.X = current.X;
				previous_point.Y = current.Y;
				current_point.X = current.X + delta_point.X;
				current_point.Y = current.Y + delta_point.Y;
				scroll.DrawLine(test_pen, previous_point, current_point);*/
				
			}
		
		{
			/*System.Collections.IEnumerator myEnumerator2 = pure_shape_queue.GetEnumerator();
			Pen test_pen2 = new Pen(System.Drawing.Color.Red, 2);
			Point current_point2 = new Point(0,0);
			Point previous_point2 = new Point(0,0);
			while ( myEnumerator2.MoveNext() )
			{
				previous_point2 = current_point2;
				//current_point2 = current;
				current_point2 = (Point)myEnumerator2.Current;
				//previous_point2 = current_point2;
				//Console.Write(current+" curr");
				//Console.Write(current_point2+" queue\n");
				//scroll.DrawLine(test_pen2, previous_point2, current_point2);
				//scroll.DrawLine(test_pen2, current_point2.X,current_point2.Y, current_point2.X+1, current_point2.Y +1);
				//previous_point2 = current_point2;
				
			}*/
		}
		}
			//strokecollector.Renderer.Draw(e.Graphics, strokecollector.Ink.Strokes);
		}
		
		
		/// <summary>
		/// gives the innocent configuration whenever needed
		/// </summary>
		private void init_state()
		{
			oldstate = 0;
			window = 0;
			minpoints=0;
			oldangle = -1;
			angle = -1;
			state = 0;
			trans = Transform.none;
			oldtrans = Transform.none;
			shapequeue.Clear();
			processed_shape_queue.Clear();
			pure_shape_queue.Clear();
			strokequeue.Clear();
			first_finescroll = true;
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{

			oldcoord = new Point(e.X,e.Y);
			NOOLDPOINT = false;

			this.Cursor = util.CursorSwitcher.change_mouse_cursor(true);

			
			
			init_state();
			Invalidate();
			hovering = false;
			finescroll = false;
			cont_scroll_down = false;
			cont_scroll_up = false;
			this.scroll_line.init_line();
			System.Collections.IEnumerator myEnumerator = pre_flight_queue.GetEnumerator();
			bool first_iteration = true;
			while ( myEnumerator.MoveNext() )
			{
				if (first_iteration == true)
				{	
					first_iteration = false;
					previous = (Point)myEnumerator.Current;
				}
				else
				{
					//Console.Write("[");
					process_one_point((Point)myEnumerator.Current);
				}
			}
			pre_flight_queue.Clear();
			post_flight_count = 0;
			if (first_iteration == false)
			{
				process_one_point(new Point(e.X, e.Y));
			}
			else
			{
				previous = new Point(e.X,e.Y);
			}
		}

		protected override void OnGotFocus(EventArgs e)
		{
			//Console.Write("F");
		}

		protected override void OnLostFocus(EventArgs e)
		{
			//Console.Write("f");
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			//Console.Write(">");
			pre_flight_queue.Clear();
			pre_flight = true;
		}

		protected override void OnMouseHover(EventArgs e)
		{
			this.Cursor = util.CursorSwitcher.change_mouse_cursor(false);
			//Console.Write("?");
			this.Cursor = util.CursorSwitcher.change_mouse_cursor(false);
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			//Console.Write("<\n");
			NOOLDPOINT = true;
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			this.Cursor = util.CursorSwitcher.change_mouse_cursor(false);
			//Console.Write("^\n");
			hovering = true;
			pre_flight = false;
			post_flight_count = 0;
			Invalidate();
			cont_scroll_timer.Stop();
			cont_scroll_down = false;
			cont_scroll_up = false;
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			
			if (e.Button == MouseButtons.Left)
			{
				this.Cursor = util.CursorSwitcher.change_mouse_cursor(true);
				process_one_point(new Point(e.X,e.Y));
				
				
			}
			else
			{
				this.Cursor = util.CursorSwitcher.change_mouse_cursor(false);
				// Hovering.
				if (pre_flight == true)
				{
					pre_flight_queue.Enqueue(new Point(e.X,e.Y));
					if (pre_flight_queue.Count > 6)
					{
						pre_flight_queue.Dequeue();
					}
				}
				else
				{
					if (post_flight_count <= 2)
					{
						process_one_point(new Point(e.X,e.Y));
						//Console.Write("]");
						post_flight_count++;
					}
				}
			}
		}
		/// <summary>
		/// goes through points to check what they are doing according to scrollbar gestures
		/// </summary>
		/// <param name="new_point">the latest aqcuired point from the queque</param>
		private void process_one_point(Point new_point)
		{
			//Console.Write("!");
			Point coordinate;
			int anglehere;
			Transform transhere;
			HowCrossed fromwhere = HowCrossed.none;
			Point pixelcoord; // = new Point();
			//Graphics ge = this.CreateGraphics();
			pixelcoord = new_point;
			coordinate = pixelcoord;
			current = new_point;

			//Console.Write(coordinate+" np ");
			// strokecollector.Renderer.InkSpaceToPixel(ge, ref pixelcoord);
			// call the guy who calculates the angle
			if(NOOLDPOINT)
			{
				oldcoord = pixelcoord;
				NOOLDPOINT = false;
			}
			if((oldcoord.X != pixelcoord.X) || (oldcoord.Y != pixelcoord.Y))
			{
				fromwhere = (HowCrossed)this.scroll_line.how_crossed(oldcoord, pixelcoord);
			}
			if (fromwhere == HowCrossed.fromleft)
			{
				crossPoint = pixelcoord;
				crosscoord = pixelcoord;
			}	
			anglehere = Angle(coordinate);
			//if (anglehere != -1)
			//{

			//Console.Write(anglehere +"\n");
			if (anglehere == -1)
			{
				transhere  = transform(anglehere, fromwhere, oldtrans);
			}
			else
			{
				transhere  = transform(anglehere, fromwhere, oldtrans);
			}
			//Console.Write(anglehere+" "+transhere+" ");
			processed_shape_queue.Enqueue(transhere);
			pure_shape_queue.Enqueue(pixelcoord);
			oldtrans = transhere;

			state = statemachine(transhere);
			//Console.Write(state+"\n");
			//Console.Write(transhere+" ||");
			//Console.Write(oldstate+" ||");
			//Console.Write(anglehere+" ||");
			//Console.WriteLine(state);
			//}
		
			//string lines = coordinate.X +"\t"+ coordinate.Y+"\t"+old.X +"\t"+old.Y+"\t"+now.X +"\t"+now.Y+"\t"+ angleprint+"\t"+aVangle+"\t"+transhere +"\t"+ state;
			/*string lines = anglehere +"\t"+ transhere;//+"\t"+old.X +"\t"+old.Y+"\t"+now.X +"\t"+now.Y

					state = statemachine(transhere);
					//Console.Write(transhere+" ||");
					//Console.Write(oldstate+" ||");
					//Console.Write(anglehere+" ||");
					//Console.WriteLine(state);
				}
		
				//string lines = coordinate.X +"\t"+ coordinate.Y+"\t"+old.X +"\t"+old.Y+"\t"+now.X +"\t"+now.Y+"\t"+ angleprint+"\t"+aVangle+"\t"+transhere +"\t"+ state;
				/*string lines = anglehere +"\t"+ transhere;//+"\t"+old.X +"\t"+old.Y+"\t"+now.X +"\t"+now.Y

			
				System.IO.StreamWriter file = new System.IO.StreamWriter("c:\\transform.txt", true);
				file.WriteLine(lines);

				file.Close();	*/
			if (state == 2 || state == 6 || state == 9 || state == 13 || state == 16)
			{
				produceOutput(state);
				Invalidate();
			}
			else
			{
				Invalidate();
			}

			if (finescroll == true && hovering == false)
			{
				fineScroll(new_point);
			}
			oldcoord=pixelcoord;
			
		}
		/// <summary>
		/// controls the finescroll amount once the user has crossed the bar, has a variing gain
		/// with going away from the bar
		/// </summary>
		private void fineScroll(Point new_point)
		{
			const int buffer_zone = 20;
			const double finescale_rate = .002;
			double distance_from_scrollbar;
			double finescale = 1.0;
			double applied_delta;
			/*if (first_finescroll)
			{
				scrollthingY1 = crossPoint.Y -scrollthinglength/2;
				first_finescroll = false;
			}*/
			double Scale = ((double)Main.central_TabControl.get_active_TabPanel().Height - (double)Main.panel.Height)/(Main.panel.Height-40);
			// int delta_y = Math.Abs(crossPoint.Y - crosscoord.Y);
			int delta_y = (oldcoord.Y - new_point.Y);
			// int delta_position;
			distance_from_scrollbar = Math.Abs(current.X - 20);
			if(distance_from_scrollbar > buffer_zone)
			{
				finescale = finescale_rate*(distance_from_scrollbar - buffer_zone)*(distance_from_scrollbar - buffer_zone);
			}
			else
			{
				finescale = 0;
			}
			
			// applied_delta =  1-(1-finescale);
			// if(applied_delta == 0)
			//	applied_delta =1;
			
			// delta_position = delta_y*(int)(Scale*applied_delta);
			// Console.WriteLine(delta_position);
			// double tmp = Scale*applied_delta;
			// double deltaY = Math.Abs((current.Y - crossPoint.Y));
			Console.Write(delta_y);
			Console.WriteLine(delta_y *(1/(1+finescale)));
			scrollthingY1 = scrollthingY1 - (delta_y *(1/(1+finescale)));
			container.changePos(0, -(int)(Scale*(((int)scrollthingY1)-20)),  true );
			Invalidate();
			/*if(oldcoord.Y > current.Y)
			{
				container.changePos(0, -(int)(Scale*(((int)scrollthingY1)+40)),  true );
				//container.changePos(0, -(int)(Scale*(((int)scrollthingY1))),  true );
				// Console.WriteLine("up");
				// scrollthingY1 = crossPoint.Y - (int)(deltaY *(1/(1+finescale))) - scrollthinglength/2;
				// Console.WriteLine(deltaY);
				//container.changePos(0, -(int)(Scale*(((int)scrollthingY1)-20)-(delta_y *(1/(1+finescale)))),  true );
				//container.changePos(0, -(int)(Scale*(crosscoord.Y-20)-(delta_y *(1/(1+finescale)))),  true );
			Invalidate();}
			else if(oldcoord.Y < current.Y){
				container.changePos(0, -(int)(Scale*(((int)scrollthingY1)-20)),  true );
				//container.changePos(0, -(int)(Scale*(((int)scrollthingY1))),  true );
				// Console.WriteLine("down");
				// scrollthingY1 = crossPoint.Y + (int)(deltaY *(1/(1+finescale))) - scrollthinglength/2;
				// Console.WriteLine(scrollthingY1);
				//container.changePos(0, -(int)(Scale*(((int)scrollthingY1)-20)+(delta_y *(1/(1+finescale)))),  true );
			Invalidate();}*/
			
			oldcoord = current;
			//Invalidate();

		}
		/// <summary>
		/// responsible for continuous scrolling
		/// </summary>
		/// <param name="down">is true if the direction is downwards</param>
		private void cont_scroll(bool down)
		{
			if (down == true)
			{
				//Console.WriteLine("CONT. PAGE DOWN");
				Invalidate();
				scrollthingY1 = scrollthingY1 + scrollthinglength;
				container.changePos(0, 731, false);
			}
			else
			{
				//Console.WriteLine("CONT. PAGE UP");
				Invalidate();
				scrollthingY1 = scrollthingY1 - scrollthinglength;
				container.changePos(0, -731, false);
			}
		}
		/// <summary>
		/// just the timer for the continuous scroll process, needs to be delayed other
		/// wise it would be to fast
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cont_scroll_timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			if(cont_scroll_down == true)
			{
				cont_scroll(true); // the bool is the direction (true is down)
			}
			else if (cont_scroll_up == true)
				cont_scroll(false);
		}

		

		/// <summary>
		/// function that tell how the scrollbar is going to change according to the 
		/// given condition
		/// </summary>
		/// <param name="angle">The angle between points</param>
		/// <param name="howCrossed">Crossed or not with direction</param>
		/// <param name="oldtrans">what was the previous transition</param>
		/// <returns>the current transition</returns>
		private Transform transform(int angle, HowCrossed howCrossed, Transform oldtrans)
		{
			/*
			if (howCrossed == HowCrossed.fromleft && (previouscross != HowCrossed.fromleft))
			{
				trans = Transform.crossedFL;
				howCrossed = HowCrossed.none;
			}// crossed from the right  
			else if (howCrossed == HowCrossed.fromright && ((previouscross != HowCrossed.fromright) ||(previouscross == HowCrossed.none)) )
			{
				trans = Transform.crossedFR;
				howCrossed = HowCrossed.none;
			}
			else if((angle > 21) && (angle < 204))
			{
				if(angle < 113)
				{
					if(angle > 67)
						trans = Transform.N;
					else
						trans = Transform.NE_45;
				}
				else
				{
					if(angle > 157)
						trans = Transform.W;
					else
						trans = Transform.NW_45;
				}
			}
			else if((angle > 203) && (angle < 338))
			{
				if(angle < 247)
					trans = Transform.SW_45;
				else if(angle < 293)
					trans = Transform.S;
				else
					trans = Transform.SE_45;
			}
			else if ((angle >(337) || (angle >= 0) && (angle < 23)))
			{
				trans = Transform.E_0;
			}
			else 
			{
				trans = oldtrans;
			}
			*/
			#region oldcode
			
			if (howCrossed == HowCrossed.fromleft && (previouscross != HowCrossed.fromleft))
			{
				trans = Transform.crossedFL;
				howCrossed = HowCrossed.none;
			}// crossed from the right  
			else if (howCrossed == HowCrossed.fromright && ((previouscross != HowCrossed.fromright) ||(previouscross == HowCrossed.none)) )
			{
				trans = Transform.crossedFR;
				howCrossed = HowCrossed.none;
			}
			else if ((angle > 21) && (angle < 68))// == 22 && upbound == 67)
			{
				trans = Transform.NE_45;
			}
			else if ((angle > 67) && (angle < 113))// == 22 && upbound == 67)
			{
				trans = Transform.N;
			}
			else if ((angle > 112) && (angle < 158))
			{
				trans = Transform.NW_45;
			}
			else if ((angle >(157)) && (angle <(204)))
			{
				trans = Transform.W;
			}
			else if ((angle > (203)) && (angle < (247)))
			{
				trans = Transform.SW_45;
			}
			else if ((angle >(246)) && (angle <(293)))
			{
				trans = Transform.S;
			}
			else if ((angle <(338)) && (angle >(292)))
			{
				trans = Transform.SE_45;
			}
			else if ((angle >(337) || (angle >= 0) && (angle < 23)))
			{
				trans = Transform.E_0;
			}
			else 
			{
				trans = oldtrans;
			}
			
			#endregion
			//else trans = oldtrans;
			/*if(oldtrans != trans && ( (oldtrans != Transform.crossedFL)&& (oldtrans != Transform.crossedFR)) && ( (trans != Transform.crossedFL)&& (trans != Transform.crossedFR)) && (oldtrans != Transform.directchange) )
			{
				//if(oldtrans == Transform.
				//Console.WriteLine("direX changed");
				trans = Transform.directchange;
			}*/
			//else return oldtrans; // = Transform.none;
			//Console.WriteLine(trans+" now");
			//Console.WriteLine(oldtrans+" before");		
			previouscross = howCrossed;
			//Console.Write(trans+" | ");
			//Console.Write((int)trans+" |");
			//Console.Write(oldstate+"|");
			//Console.WriteLine();
			oldtrans = trans;
			return trans;
		}
		/// <summary>
		/// use the "state-machine-array" to transform the oldstate into the new state
		/// </summary>
		/// <param name="trans">gets the Transition that came back from the Transition function</param>
		/// <returns>the new state</returns>
		private int statemachine(Transform trans)
		{

			//Console.Write((int)trans+"|");
			//Console.Write(oldstate+"|");
			//if (States[(int)trans, oldstate]!=0)
			//{
				state = States[(int)trans, oldstate];
				//return (int)state;
			//}
			//Console.Write(state);
			//Console.WriteLine();
			
			oldstate = state;	
			return (int)state;
			
			
		}

		/// <summary>
		/// The heart of the scrollbar, uses the current point to calculate the angle 
		/// according to the previous point
		/// </summary>
		/// <param name="current">takes the current point from the point-queuque</param>
		/// <returns>returns the angle between two consecutive points as int</returns>
		private int Angle(Point current)
		{
			if (previous != current && (previous != new Point(0,0)))
			{
				//Console.Write("$");
				const int NUMBER_OF_QUEUE_ELEMENTS = 6;
				const int POINT_DISTANCE = 2;
				const double MIN_POINT_DISTANCE = 1;
				minpoints++;
				old = previous;
				now = current;
				int negative = -1;
				int positive = 1;
				int equal = 0;
				int xdiff, ydiff;

				double distance = Math.Sqrt((Math.Pow((double)(previous.X-current.X),2))+(Math.Pow((double)(previous.Y-current.Y),2)));
				//Console.Write(angle+ "\n");
				#region distance
				if (distance > MIN_POINT_DISTANCE)
				{
					//Console.Write("#");
					window ++;
					xdiff = current.X - previous.X;
					ydiff = current.Y- previous.Y;
					if (Math.Sign(ydiff) == equal && Math.Sign(xdiff) == positive) 
					{
						angle = 0;  // 
					}
					else if (Math.Sign(ydiff) == equal && Math.Sign(xdiff) == negative)
					{
						angle = 180; // 
					}
					else if (Math.Sign(ydiff) == positive && Math.Sign(xdiff) == equal) 
					{
						angle = 270 ; //
					}
					else if (Math.Sign(ydiff) == negative && Math.Sign(xdiff) == equal) 
					{
						angle = 90; // 
					}
					else
					{
						double difference = ((double)ydiff/(double)xdiff);
						angle= Math.Atan(difference);
						angle = (angle*180)/(Math.PI);
						if (Math.Sign(ydiff) == negative && Math.Sign(xdiff) == positive) 
						{
							angle = angle*(-1) ;  // 45		
						}
						else if (Math.Sign(ydiff) == positive && Math.Sign(xdiff) == positive)
						{
							angle = 360 - angle; // + 270; // 315
						}
						else if (Math.Sign(ydiff) == positive && Math.Sign(xdiff) == negative) 
						{
							angle = angle + 270;//180 ; // 225
						}
						else if (Math.Sign(ydiff) == negative && Math.Sign(xdiff) == negative) 
						{
							angle = angle + 90; // 135
						}
					} // last case to compute the angle
					//Console.Write("\"" + angle + "\"");
					if (distance >= POINT_DISTANCE)
					{
						double current_distance = 0;
						while (current_distance <= distance)
						{
							shapequeue.Enqueue(angle);
							current_distance = current_distance + POINT_DISTANCE;
						}
					}
					else
					{
						shapequeue.Enqueue(angle);
					}
					while (shapequeue.Count > NUMBER_OF_QUEUE_ELEMENTS)
					{
						shapequeue.Dequeue();
					}
					double correction_for_cycle = 0;
					double average_with_correction_for_cycle;
					double average_without_correction_for_cycle;
					System.Collections.IEnumerator myEnumerator = shapequeue.GetEnumerator();
					while ( myEnumerator.MoveNext() )
					{
						averageangle = averageangle + (double)myEnumerator.Current;
						counter ++;
						if((double)myEnumerator.Current > 180)
						{
							correction_for_cycle = correction_for_cycle + -360;
						}
					}
					average_without_correction_for_cycle = averageangle/counter;
					average_with_correction_for_cycle = (averageangle + correction_for_cycle)/counter;
					if ((average_with_correction_for_cycle > -22.5) && (average_with_correction_for_cycle < 22.5))
					{
						if (average_with_correction_for_cycle < 0)
						{
							realaverageangle = average_with_correction_for_cycle + 360;
						}
						else
						{
							realaverageangle = average_with_correction_for_cycle;
						}
					}
					else
					{
						realaverageangle = average_without_correction_for_cycle;
					}
					aVangle = (int)realaverageangle;
					averageangle = 0;
					if (shapequeue.Count == 5)
					{
						// nothing to be done
					}
					else if (shapequeue.Count > 2)
					{
						//Console.Write("@");
					}
					else
					{
						//Console.Write("@@");
						realaverageangle = -1;
					}
					counter = 0;
					counter2 = 0;
					previous = current;
					oldangle = (int)realaverageangle;
					return (int)realaverageangle;
				} // distance less than something
				else
				{
					//Console.Write("%");
					return(oldangle);
				}
			} // same point
				#endregion distance
			else
			{
				previous = current;
				///Console.Write(oldangle+" |");
				//Console.Write("&");
				return oldangle;
			}
		}
		
		/// <summary>
		/// central function to coordinate the changes in the scrollbar and on the page
		/// triggers the functions that do the scrolling and change the viewport and the
		/// scrollbar position
		/// </summary>
		/// <param name="finalstate">takes a final state from that comes from the state machine</param>
		private void produceOutput(int finalstate)
		{
		// produces the	output to the command line
				switch (finalstate)
				{
						
					case (int)states.singleline:
						//Console.WriteLine("SINGLELINE");

						if(!finescroll)
						{
							scrollthingY1 = crosscoord.Y - scrollthinglength/2;
							container.changePos(0, -(int)(1*(crosscoord.Y-20)),true);
							Invalidate();
						}
						finescroll = true;
						//Console.WriteLine(crosscoord.Y);
						
						//crosscoord.Y;
						//scrollthingY2 = crosscoord.Y +20;
						
						break;
					case (int) states.pageup:

						scrollthingY1 = scrollthingY1-scrollthinglength;
						container.changePos(0, -731, false);

						//Console.WriteLine("PAGE UP");
						//Invalidate();
						break;
					case (int) states.pagedown:
						//Console.WriteLine("PAGE DOWN");
						scrollthingY1 = scrollthingY1+scrollthinglength;
						container.changePos(0, 731, false);
						
						//Invalidate();
						break;
					case (int) states.contpagedown:
						//Console.WriteLine("CONT. PAGE DOWN");
						cont_scroll_timer.Start();
						cont_scroll_down = true;
						break;
					case (int) states.contpageup:
						cont_scroll_timer.Start();
						cont_scroll_up = true;
						//Console.WriteLine("CONT. PAGE UP");
						break;
					
						//default : Console.WriteLine("not final"); break;

						
				}
			
		}

		
	}


}
