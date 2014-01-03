using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;

using System.Windows.Forms;
using System.Data;
using Microsoft.Ink;


namespace crossy
{
	public class line
	{
		
		


		public Point TopPoint = new Point();
		public Point LowPoint = new Point();
		

		
		private bool isLeft = false;
		private bool isRight = false;
		private bool isTop = false;
		private bool isBottom = false;
		private LineOrientation _orientation;
		public LineOrientation orientation
		{
			set { _orientation = value;}
			get { return _orientation;}
		}
	
		
	
		/// <summary>
		/// constructor for a generic crossable line
		/// takes the orientation since the crossing is calculated depending on that`
		/// </summary>
		/// <param name="orienta">orientation of the crossable line</param>
		public line(LineOrientation orienta)
		{
			_orientation = orienta;
			//Main = Main;
			//TopPoint = top;
			//LowPoint = low;
		}
		/// <summary>
		/// produces and innocent line
		/// </summary>
		public void init_line()
		{
			isLeft = false;
			isRight = false;
			isTop = false;
			isBottom = false;
		}


		/// <summary>
		/// functions that returns if the according line has been crossed
		/// given a point, it is registered to the line
		/// </summary>
		/// <param name="thiscoordinate">takes the point that needs to be checked for crossing</param>
		/// <returns>none or the crossing direction</returns>
		public int crossed(Point thiscoordinate)
		{
			int X = thiscoordinate.X;
			int Y = thiscoordinate.Y;
			int LineL = TopPoint.X;
			int LineR = LowPoint.X;
			int LineT = TopPoint.Y;
			int LineB = LowPoint.Y;															 
			HowCrossed direcion = HowCrossed.none;
			double m, b, y_ontheline;
			//Console.WriteLine("in the line crossed function");
			//Console.WriteLine(TopPoint[i]+"|"+ LowPoint[i]);
			int max_X = Math.Max(TopPoint.X,LowPoint.X);
			int min_X = Math.Min(TopPoint.X,LowPoint.X);
			int max_Y = Math.Max(TopPoint.Y,LowPoint.Y);
			int min_Y = Math.Min(TopPoint.Y,LowPoint.Y);

			

			//Console.Write(min_X+" "+max_X+" "+min_Y+" "+max_Y);
			
			switch ((int)_orientation)
			{
				case (int)LineOrientation.tilted_right:
					//Console.Write(" check tilted line"+thiscoordinate +"\n");
					if(X >= min_X && X <= max_X && Y >= min_Y && Y <= max_Y)
					{

						m = (double)(TopPoint.Y - LowPoint.Y) / (TopPoint.X - LowPoint.X);
						b = TopPoint.Y - m * TopPoint.X;
						
						y_ontheline = (m * X) + b;
						if (Y > y_ontheline)
						{
							if(isLeft)
							{
								direcion = HowCrossed.fromleft;
							}
							isRight = true;
							isLeft = false;
						}
						else if( y_ontheline > Y )
						{
							if(isRight)
							{
								direcion = HowCrossed.fromright;
							}
							isRight = false;
							isLeft = true;
				
						}
					}
					return (int)direcion;

				case (int)LineOrientation.tilted_left:
					//Console.WriteLine("left tilted");
					//Console.Write(" check tilted line"+thiscoordinate +"\n");
					if(X >= min_X && X <= max_X && Y >= min_Y && Y <= max_Y)
					{

						m = (double)(TopPoint.Y - LowPoint.Y) / (TopPoint.X - LowPoint.X);
						b = TopPoint.Y - m * TopPoint.X;
						
				
						y_ontheline = (m * X) + b;
						if (Y < y_ontheline)
						{
							if(isLeft)
							{
								direcion = HowCrossed.fromleft;
							}
							isRight = true;
							isLeft = false;
						}
						else if( Y > y_ontheline )
						{
							if(isRight)
							{
								direcion = HowCrossed.fromright;
							}
							isRight = false;
							isLeft = true;
				
						}
					}
					return (int)direcion;
					
				case ((int)LineOrientation.horizontal):
					if(Y>LineT)
					{
						//Console.Write("right turn \n");
					
						if ((isTop) && X >= min_X && X <= max_X) 
						{
							//Console.Write("no we are on the right side \n");
							direcion = HowCrossed.fromtop;
						}
						isBottom = true;
						isTop = false;
					}
					else if (Y<LineT) 
					{
						//Console.Write("left turn \n");
						if((isBottom)&& X >= min_X && X <= max_X)
						{
							direcion = HowCrossed.frombottom;
						}
						isTop = true;
						isBottom = false;
					}
					return (int)direcion;
				
				case (int)LineOrientation.vertical:
					if(X>LineL)
					{
						//Console.Write("right turn \n");
					
						if ((isLeft)&& Y >= min_Y && Y <= max_Y) 
						{
							//Console.Write("no we are on the right side \n");
							direcion = HowCrossed.fromleft;
						}
						isRight = true;
						isLeft = false;
					}
					else if (X<LineL ) 
					{
						//Console.Write("left turn \n");
						if((isRight)&& Y >= min_Y && Y <= max_Y)
						{
							//Console.Write("no we are on the left side \n");
							direcion = HowCrossed.fromright;
						}
						isLeft = true;
						isRight = false;
						
					}
					return (int)direcion;
					

			}
			
			return (int)direcion;
		
			
		}	

		public virtual int how_crossed(Point first, Point second)
		{
		
			HowCrossed direction = HowCrossed.none;
			double intersect_X = double.NaN;
			double intersect_Y = double.NaN;
			bool Crossed = false;
			int how = intersect_point(first, second, ref Crossed, ref intersect_X, ref intersect_Y);
			Point _crossPoint = new Point((int)intersect_X, (int)intersect_Y);
			//Console.WriteLine(_crossPoint);
			if (Crossed)
			{
				//crossPoint = _crossPoint;
				switch ((int)_orientation)
				{
					case (int)LineOrientation.tilted_right:
						if(how == 1 || how == 3)
							direction = HowCrossed.fromleft;
						else
							direction = HowCrossed.fromright;

						return (int)direction;

					case (int)LineOrientation.tilted_left:
						if(how == 1 || how == 4)
							direction = HowCrossed.fromleft;
						else
							direction = HowCrossed.fromright;

						return (int)direction;

					case ((int)LineOrientation.horizontal):
						if(first.Y < second.Y)
							direction = HowCrossed.fromtop;
						else
							direction = HowCrossed.frombottom;

						return (int)direction;
						
					case (int)LineOrientation.vertical:
						if(first.X < second.X)
							direction = HowCrossed.fromleft;
						else
							direction = HowCrossed.fromright;

						return (int)direction;

				}
			}
			return (int)direction;
		}
		public int intersect_point(Point FirstPoint, Point SecondPoint, ref bool crossed, ref double intersection_X, ref double intersection_Y)
		{
			int how = -1; //dml
			crossed = false; 
			intersection_X = Double.NaN;
			intersection_Y = Double.NaN;
			//int X = thiscoordinate.X;
			//int Y = thiscoordinate.Y;
			int LineL = TopPoint.X;
			int LineR = LowPoint.X;
			int LineT = TopPoint.Y;
			int LineB = LowPoint.Y;															 
			double A, B,C, A2, B2, C2;
			Point crossPoint = new Point(0,0);
			//Console.WriteLine("in the line crossed function");
			//Console.WriteLine(TopPoint[i]+"|"+ LowPoint[i]);

			// bounding box for the target line
			double max_X = (double)Math.Max(TopPoint.X,LowPoint.X);
			double min_X = (double)Math.Min(TopPoint.X,LowPoint.X);
			double max_Y = (double)Math.Max(TopPoint.Y,LowPoint.Y);
			double min_Y = (double)Math.Min(TopPoint.Y,LowPoint.Y);

			// bounding box for the crossline
			double min_X2 = (double)Math.Min(FirstPoint.X, SecondPoint.X);
			double max_X2 = (double)Math.Max(FirstPoint.X, SecondPoint.X);
			double min_Y2 = (double)Math.Min(FirstPoint.Y, SecondPoint.Y);
			double max_Y2 = (double)Math.Max(FirstPoint.Y, SecondPoint.Y);



			
			if(TopPoint.X == LowPoint.X)
			{
				B2 = 0;
				A2 = 1;
				C2 = -TopPoint.X;
			}
			else
			{
				B2 = 1;
				A2= -((double)(TopPoint.Y - LowPoint.Y)/(double)(TopPoint.X - LowPoint.X));
				C2 = -(A2 * TopPoint.X) - TopPoint.Y;
			}
			if(FirstPoint.X == SecondPoint.X)
			{
				B=0;
				A=1;
				C=-FirstPoint.X;
			}
			else
			{
				B= 1;
				A= -((double)(FirstPoint.Y - SecondPoint.Y)/(double)(FirstPoint.X - SecondPoint.X));
				C = -(A * FirstPoint.X) - FirstPoint.Y;
			}

			bool crossed_extended_line = false;
			double one_over_epsilon = 1000.0; // 1/acceptable_error_level here .001
			if((int)((A*B2-A2*B)*one_over_epsilon) == 0)
			{
				//Console.WriteLine("!");
				// parallel
			}
			else 
			{
				if((int)(B*one_over_epsilon) == 0 && (int)(B2*one_over_epsilon) == 0)
				{
					//Console.WriteLine("*");
					// parallel and vertical
				}
				else
				{
					if((int)(B2*one_over_epsilon) == 0)
					{
						// line is vertical
						intersection_Y = (-(A * TopPoint.X + C) / B);
						intersection_X = TopPoint.X;
						crossed_extended_line = true;
					}
					else
					{
						if ((int)(B*one_over_epsilon) == 0)
						{
							// user input is vertical
							intersection_Y = (-(A2 * FirstPoint.X + C2) / B2);
							intersection_X = FirstPoint.X;
							crossed_extended_line = true;
						}
						else
						{
							intersection_X = -((B2*C - B*C2)/(A*B2-A2*B));
							intersection_Y = (-(A2*intersection_X + C2)/B2);
							crossed_extended_line = true;
						}
					}
				}
			}
			// Console.WriteLine(A+"\t"+B+"\t"+C+"\t"+A2+"\t"+B2+"\t"+C2+"\t"+SecondPoint.X+"\t"+SecondPoint.Y+"\t"+intersection_X+"\t"+intersection_Y);
			if(crossed_extended_line)
			{
				crossPoint = new Point((int) intersection_X, (int) intersection_Y);
				if(((min_Y < intersection_Y) && (intersection_Y < max_Y))
					|| ((int)((intersection_Y -  max_Y)*one_over_epsilon) == 0)
					|| ((int)((intersection_Y - min_Y)*one_over_epsilon) == 0))
				{
					if (((min_X < intersection_X) && (intersection_X < max_X))
						|| ((int)((intersection_X - max_X)*one_over_epsilon) == 0)
						|| ((int)((intersection_X - min_X)*one_over_epsilon) == 0))
					{
						if(((min_Y2 < intersection_Y) && (intersection_Y < max_Y2))
							|| ((int)((intersection_Y - max_Y2)*one_over_epsilon) == 0)
							|| ((int)((intersection_Y - min_Y2)*one_over_epsilon) == 0))
						{
							if ((((double)min_X2 < intersection_X) && (intersection_X < max_X2))
								|| ((int)((intersection_X - max_X2)*one_over_epsilon) == 0)
								|| ((int)((intersection_X - min_X2)*one_over_epsilon) == 0))
							{
								// At this point the two segments do intersect.
								// To avoid the problem of a point sitting on the intersection line
								// (i.e. such a point will trigger a double crossing)
								// if the first point sits on the line, we do not consider that a cross
								if(((int)(((double)FirstPoint.X - intersection_X)*one_over_epsilon) != 0)
									|| ((int)(((double)FirstPoint.Y - intersection_Y)*one_over_epsilon) != 0))
								{
									// Console.WriteLine(TopPoint+"\t"+LowPoint+"\t"+FirstPoint+"\t"+SecondPoint.X+"\t"+SecondPoint.Y+"\t"+crossPoint);
									crossed = true;
									// Vertical widget
									if((int)(B2*one_over_epsilon) == 0) 
									{
										if(FirstPoint.X <= TopPoint.X) 
										{
											how = 1; //dml
										}
										else // if(FirstPoint.X >= TopPoint.X)
										{
											how = 2; //dml
										}
									}
									else 
									{
										double y1_projection = -(A2*(double)FirstPoint.X + C2)/B2; // on the crossing bar
										double y2_projection = -(A2*(double)SecondPoint.X + C2)/B2; // on the crossing bar
			
										if((int)(((double)FirstPoint.Y - y1_projection)*one_over_epsilon) < 0 ) 
										{
											how = 3; //dml
										}
										else // if(FirstPoint.Y >= y1_projection)
										{
											how = 4;//dml
										}
									}
								}
							}
						}
					}
				}
			}
			// Console.WriteLine(TopPoint+"\t"+LowPoint+"\t"/*+FirstPoint+"\t"*/+SecondPoint.X+"\t"+SecondPoint.Y+"\t"+crossPoint.X+"\t"+crossPoint.Y);
			return(how);	
		}	

		public bool get_isRight()
		{
			return isRight;
		}
		public bool get_isLeft()
		{
			return isLeft;
		}
		public bool get_isTop()
		{
			return isTop;
		}
		public bool get_isBottom()
		{
			return isBottom;
		}

	}

}
