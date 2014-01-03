using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.Collections.Specialized;

namespace crossy
{
	/// <summary>
	/// A Stroke class tha handles all the inking
	/// </summary>
	public class StrokeF:ScreenObject
	{
		protected ArrayList myPoints;
		protected Color myColor;
		protected int myWidth;
		protected bool selected; 
       
		public StrokeF()
		{   
			myPoints = new ArrayList();
			myColor  = Color.Black;
			myWidth = 2;
			selected = false;
           
		}
		public StrokeF(Color _color, int _width)
		{
			myPoints = new ArrayList();
			myColor  = _color;
			myWidth = _width;
			selected = false;
		}
		public override Rectangle getBoundingBox()
		{
			BoundingBox strokeBoundingBox;
			strokeBoundingBox.Top = boundBox.Top - Width;
			strokeBoundingBox.Bottom = boundBox.Bottom + Width;
			strokeBoundingBox.Left = boundBox.Left - Width;
			strokeBoundingBox.Right = boundBox.Right + Width;
			return new Rectangle(strokeBoundingBox.Left, strokeBoundingBox.Top, 
											strokeBoundingBox.Right - strokeBoundingBox.Left, strokeBoundingBox.Bottom - strokeBoundingBox.Top);
		}


		public virtual int AddPoint(PointF p)
		{
			if(myPoints.Count==0)
			{
				boundBox.Left = (int)p.X;
				boundBox.Right = (int)p.X;
				boundBox.Top = (int)p.Y;
				boundBox.Bottom = (int)p.Y;		
			}
			else
			{
				updateBoundingBox(p);
			}
			myPoints.Add(p);           
			int pos = myPoints.Count - 1;
			return pos;
		}
		public bool Selected
		{
			get{ return selected;}
			set{ selected = value;}
		}

		public PointF[] GetPoints()
		{
			PointF[] newArray = (PointF[])myPoints.ToArray(typeof(PointF));
			return newArray;
		}
		public PointF GetPointFromPos(int where)
		{
			System.Collections.IEnumerator myEnumerator = myPoints.GetEnumerator();
			PointF tmp = new PointF(float.NaN,float.NaN);
			while ( myEnumerator.MoveNext() )
				if(myPoints.IndexOf(myEnumerator.Current) == where)
				{
					tmp = (PointF)myEnumerator.Current;
				}
			
			return tmp;
		}
		public void updateBoundingBox(PointF p)
		{
			if(boundBox.Bottom<p.Y)
			{
				boundBox.Bottom = (int)p.Y;
			}
			if(boundBox.Top>p.Y)
			{
				boundBox.Top = (int)p.Y;
			}
			if(boundBox.Left>p.X)
			{
				boundBox.Left =(int) p.X;
			}
			if(boundBox.Right<p.X)
			{
				boundBox.Right =(int) p.X;
			}
		}
		public void Clear()
		{
			myPoints.Clear();
		}   
		public Color Color
		{
			set {myColor = value;}
			get {return myColor;}
		}
		public int Width
		{
			set {myWidth = value;}
			get {return myWidth;}
		}

	}

	public class StrokeCollection
	{
		protected ArrayList myStrokes;
		protected int strokeCounter = 0;
		public StrokeCollection()
		{
			myStrokes = new ArrayList();
		}
		public virtual int AddStroke(StrokeF new_stroke)
		{
			myStrokes.Add(new_stroke);
			int pos = myStrokes.Count - 1;
			return pos;
		}
		public ArrayList GetStrokes()
		{
			return myStrokes;
		}
		public StrokeF getStroke(int which)
		{
			System.Collections.IEnumerator myEnumerator = myStrokes.GetEnumerator();
			StrokeF tmp = null;
			while ( myEnumerator.MoveNext() )
				if(myStrokes.IndexOf(myEnumerator.Current)== which)
				{
					tmp = (StrokeF)myEnumerator.Current;
				}
			
			return tmp;
		}
		public int Count
		{
			get{return (myStrokes.Count);}
			set{}
		}
		public StrokeF getCurrentStroke()
		{
			System.Collections.IEnumerator myEnumerator = myStrokes.GetEnumerator();
			StrokeF tmp = new StrokeF();;
			while ( myEnumerator.MoveNext() )
				tmp = (StrokeF)myEnumerator.Current;
			
			return tmp;
		}
		public void  Clear()
		{
			//oreach (StrokeF stroke in this)
			{
				//stroke.Clear();
			}


		}
		
	}
	public class ScreenObject
	{
		static int counter = 0;
		protected int id;
		public struct BoundingBox
		{
			public int Top;
			public int Bottom;
			public int Left;
			public int Right;
		}
		public BoundingBox boundBox;
		public ScreenObject()
		{
			boundBox.Top = 0;
			boundBox.Bottom = 0;
			boundBox.Left = 0;
			boundBox.Right = 0;
		}
		public void advanceID()
		{
			id = counter++;
		}
		public int getID()
		{
			return id;
		}
		public virtual Rectangle getBoundingBox()
		{
			return new Rectangle(boundBox.Left, boundBox.Top, boundBox.Right - boundBox.Left, boundBox.Bottom - boundBox.Top);
		}

	}
}
