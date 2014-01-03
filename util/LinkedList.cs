using System;

namespace LinkedList
{
	/// <summary>
	/// Summary description for LinkedList.
	/// </summary>
	public class LinkedList
	{
		int iCount = 1;
		int iCurrent = 0;
		Node nCurrent, nHead, nTail;


		#region Properties
		/// <summary>
		/// Give back how many nodes there are.
		/// </summary>
		public int Count
		{
			get
			{
				return iCount;
			}
		}

		/// <summary>
		/// Gives back the current Node
		/// </summary>
		public Node CurrentNode
		{
			get
			{
				return nCurrent;
			}
		}

		/// <summary>
		/// Keeps track of the index where you are
		/// </summary>
		public int CurrentNodeIndex
		{
			get
			{
				return iCurrent;
			}
		}
		#endregion

		public object GetFirst()
		{
			ToFirst();
			if(nCurrent == null)
				return null;
			return nCurrent.Value;
		}
		

		public object GetNext()
		{
			ToNext();
			if(nCurrent == null)
				return null;
			return nCurrent.Value;
		}

		/// <summary>
		/// Default and only Constructor
		/// SetUp our LinkedList
		/// </summary>
		public LinkedList()
		{
			nHead = nCurrent = nTail = null;
		}

		/// <summary>
		/// This function will add another Node
		/// </summary>
		/// <param name="obj">Value for the added Node</param>
		public void AddNode(object obj)
		{
			if(nHead == null)
			{
				nHead = nTail = new Node(null, null, obj);
			}
			else if(nCurrent == null)
			{
				nTail = new Node(nTail, null, obj);
			}
			else
			{
				nCurrent = new Node(nCurrent, nCurrent.Next, obj);
				if(nCurrent.Next != null)
				{
					nCurrent.Next.Previous = nCurrent;
				}
				if(nCurrent.Previous != null)
				{
					nCurrent.Previous.Next = nCurrent;
				}
			}
			iCount++;
		}
		public void RemoveNode(object obj)
		{
			search(obj);
			if(nCurrent == null)
			{
				Console.WriteLine("nothing removed");
				return;
				//return null;
			}

			if(nCurrent == nHead)
			{
				nHead = nHead.Next;
			}
			if(nCurrent == nTail)
			{
				nTail = nTail.Previous;
			}

			if(nCurrent.Previous != null)
			{
				nCurrent.Previous.Next = nCurrent.Next;
			}
			if(nCurrent.Next != null)
			{
				nCurrent.Next.Previous = nCurrent.Previous;
			}
			iCount--;	
		}
		private void search(object obj)
		{
			nCurrent = nHead;
			while (nCurrent!=null && !nCurrent.Value.Equals(obj))
			{
				ToNext();
			}
		}
		public void addAfter(object toAddAfter, object toInsert)
		{
			search(toAddAfter);
			AddNode(toInsert);
		}
		public object get_object_from_index(int id)
		{
			try
			{
				GoTo(id);
			}
			catch(Exception)
			{
				return null;
			}
			if(nCurrent == null)
				return null;
			return nCurrent.Value;
		}
			
		

		/// <summary>
		/// Goes to the next Node
		/// </summary>
		public void ToFirst()
		{
			// Checks whether the Next Node is null
			// if so it throws an exception.
			// You can also do nothing but I choos for this.
			nCurrent = nHead;
		}

		/// <summary>
		/// Goes to the next Node
		/// </summary>
		public void ToNext()
		{
			// Checks whether the Next Node is null
			// if so it throws an exception.
			// You can also do nothing but I choos for this.
			if(nCurrent != null)
			{
				nCurrent = nCurrent.Next;
			}
		}

		/// <summary>
		/// Goes to the previous Node
		/// </summary>
		public void ToPrevious()
		{
			// Look at ToNext();
			if(nCurrent != null)
			{
				nCurrent = nCurrent.Previous;
			}	
		}

		/// <summary>
		/// Goes to the index you fill in
		/// </summary>
		/// <param name="index">Index Where to go?</param>
		public void GoTo(int index)
		{
			nCurrent = nHead;
			for(int i = 1; i < index && nCurrent != null; i++)
			{
				ToNext();
			}
		}
	}
}
