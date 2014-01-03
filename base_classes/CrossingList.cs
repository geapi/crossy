using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace crossy
{
	/// <summary>
	/// Summary description for UserControl1.
	/// </summary>
	public class CrossingList : System.Windows.Forms.ListBox
	{
		protected class DisplayLevel
		{
			public class Item
			{
				public string full_display;
				public string suffix;
				public string prefix;
				public int next_index;
				public int previous_index;
				public int min_index_to_load;
				public int max_index_to_load;
				public bool final;
				public DisplayLevel my_level;
				public int index_in_level;
				public Item()
				{
					full_display = "";
					suffix = "";
					prefix = "";
					next_index = 0;
					min_index_to_load = 0;
					max_index_to_load = 0;
					final = true;
					my_level = null;
				}
			}

			public class CharBucket
			{
				public StringCollection members;
				public CharBucketTreeNode subtree;
				public string my_prefix;
				public char my_char;
				public int level;
				public int index_in_level;
				public Item assigned_item;
				public CharBucket(int current_level)
				{
					level = current_level;
					index_in_level = 0;
					subtree = null;
					members = new StringCollection();
				}
			}

			public class CharBucketTreeNode
			{
				public CharBucket[] buckets = new CharBucket[256];
				public CharBucket prefix_bucket;
				public bool is_leaf;
				public int level;
				public int max_level_bellow;
				public string prefix;

				public CharBucketTreeNode(StringCollection new_members):
					this(new_members, 0, "")
				{  // since we are doing compression of prefix we need to compute the index again.
					update_levels(0);
				}

				private void update_levels(int current_level)
				{
					level = current_level;
					if (is_leaf == false)
					{
						for (int i = 0; i < 256; i++)
						{
							if ((buckets[i] != null) && (buckets[i].subtree != null))
							{
								buckets[i].subtree.update_levels(current_level + 1);
							}
						}
					}
				}

				public CharBucketTreeNode(StringCollection new_members, int index_level, string my_prefix)
				{
					prefix = my_prefix;
					is_leaf = true;
					level = index_level;
					StringEnumerator myEnumerator = new_members.GetEnumerator();
					while ( myEnumerator.MoveNext() )
					{
						string current_string = ((string)myEnumerator.Current);
						if (current_string.Length > index_level)
						{
							if (buckets[current_string[index_level]] == null)
							{
								buckets[current_string[index_level]] = new DisplayLevel.CharBucket(index_level);
							}
							buckets[current_string[index_level]].members.Add(current_string);
						}
						else
						{
							if (current_string.Length == index_level)
							{
								// what if a prefix is in the collection?
								if (prefix_bucket == null)
								{
									prefix_bucket = new DisplayLevel.CharBucket(index_level);
								}
								prefix_bucket.members.Add(current_string);
							}
						}
					}
					max_level_bellow = 0;
					for (int i = 0; i < 256; i++)
					{
						if ((buckets[i] != null) && (buckets[i].members.Count > 1))
						{
							is_leaf = false;
							buckets[i].subtree = new CharBucketTreeNode(buckets[i].members, index_level + 1, prefix + (char)i);
							if (true)
							{ // this block collapse the common prefix (i.e. sub_tree with only one child at each level;
								int sub_tree_children_count = 0;
								int target_child = -2;
								if (buckets[i].subtree.prefix_bucket != null)
								{
									target_child = -1;
									sub_tree_children_count += 1;
								}
								for (int j = 0; (j < 256) && (sub_tree_children_count < 2); j++)
								{
									if (buckets[i].subtree.buckets[j] != null)
									{
										target_child = j;
										sub_tree_children_count += 1;
									}
								}
								if ((sub_tree_children_count == 1) && (buckets[i].subtree.prefix_bucket == null))
								{
									buckets[i].subtree = buckets[i].subtree.buckets[target_child].subtree;
									buckets[i].subtree.level = index_level + 1;
								}
							}
							if (max_level_bellow < buckets[i].subtree.max_level_bellow + 1)
							{
								max_level_bellow = buckets[i].subtree.max_level_bellow + 1;
							}
						}
					}
				}

				public CharBucket find_name(string name)
				{
					return(find_name(name, 0));
				}


				public CharBucket find_name(string name, int current_level)
				{
					if (current_level < name.Length)
					{
						if (buckets[(int)name[current_level]] != null)
						{
							if (buckets[(int)name[current_level]].members.Count > 1)
							{
								return(buckets[(int)name[current_level]].subtree.find_name(name,buckets[(int)name[current_level]].subtree.prefix.Length));
							}
							else
							{
								if (buckets[(int)name[current_level]].members.Count == 1)
								{
									if (buckets[(int)name[current_level]].members[0] == name)
									{
										return(buckets[(int)name[current_level]]);
									}
									else
									{
										return(null);
									}
								}
								else
								{
									return(null);
								}
							}
						}
						else
						{
							return(null);
						}
					}
					else
					{
						if (prefix_bucket != null)
						{
							if (prefix_bucket.members[0] == name)
							{
								return(prefix_bucket);
							}
							else
							{
								return(null);
							}
						}
						else
						{
							return(null);
						}
					}
				}

				public void get_buckets_in_level(ArrayList container, int target_level)
				{
					if (prefix_bucket != null)
					{
						prefix_bucket.my_prefix = prefix;
						container.Add(prefix_bucket);
					}
					if (target_level == level)
					{
						for (int i = 0; i < 256; i++)
						{
							if (buckets[i] != null)
							{
								if (buckets[i].subtree != null)
								{
									buckets[i].my_prefix = buckets[i].subtree.prefix;
									buckets[i].my_char = (char)i;
									container.Add(buckets[i]);
								}
								else
								{
									buckets[i].my_prefix = prefix + (char)i;
									buckets[i].my_char = (char)i;
									container.Add(buckets[i]);
								}
							}
						}
					} 
					else if (target_level > level)
					{
						if (is_leaf)
						{
							for (int i = 0; i < 256; i++)
							{
								if (buckets[i] != null)
								{
									buckets[i].my_prefix = prefix + (char)i;
									buckets[i].my_char = (char)i;
									container.Add(buckets[i]);
								}
							}
						} 
						else
						{
							for (int i = 0; i < 256; i++)
							{
								if (buckets[i] != null)
								{
									if (buckets[i].subtree != null)
									{
										buckets[i].subtree.get_buckets_in_level(container,target_level);
									}
									else
									{
										buckets[i].my_prefix = prefix + (char)i;
										buckets[i].my_char = (char)i;
										container.Add(buckets[i]);
									}
								}
							}
						}
					}
				}

			}

			
			protected DisplayLevel(StringCollection new_members)
			{
			}
			protected DisplayLevel()
			{
			}
			protected DisplayLevel(ArrayList buckets_in_level)
			{
				ArrayList items_in_level = new ArrayList();
				IEnumerator bucket_enumerator = buckets_in_level.GetEnumerator();
				int current_index = 0;
				while ( bucket_enumerator.MoveNext() )
				{
					CharBucket current_bucket = (CharBucket)(bucket_enumerator.Current);
					Item current_item;
					current_item = new Item();
					if (current_bucket.members.Count == 1)
					{
						current_item.full_display = current_bucket.members[0];
						current_item.next_index = 0;
						current_item.final = true;
					}
					else if (current_bucket.members.Count > 1)
					{
						current_item.prefix = current_bucket.my_prefix;
						// current_item.suffix = current_bucket.my_char + "...";
						current_item.suffix = "...";
						int first_bucket_in_subtree = 0;
						int children_count = 0;
						while (current_bucket.subtree.buckets[first_bucket_in_subtree] == null)
						{
							first_bucket_in_subtree++;
						}
						// index in the middle of the next level.
						for (int j = first_bucket_in_subtree; j < 256; j++)
						{
							if (current_bucket.subtree.buckets[j] != null)
							{
								children_count++;
								current_bucket.subtree.buckets[j].assigned_item.previous_index = current_index;
							}
						}
						current_item.next_index = current_bucket.subtree.buckets[first_bucket_in_subtree].index_in_level + (children_count - 1)/2;
						current_item.final = false;
					}
					current_bucket.assigned_item = current_item;
					current_item.my_level = this;
					current_item.index_in_level = current_index;
					items_in_level.Add(current_item);
					current_bucket.index_in_level = current_index;
					current_index++;
				}
				items = (Item[])items_in_level.ToArray((new Item()).GetType());
			}

			public void get_names_to_load(int index, int display_size, int scrolling_window_length,
				out string[] names, out int target_index_in_names)
			{
				int start_item_index = 0;
				int end_item_index = 0;
				int items_count = 0;
				int empty_string_before = 0;
				int empty_string_after = 0;
				int max_length = scrolling_window_length + 2*display_size;

				if ((index - max_length/2) >= 0)
				{
					empty_string_before = 0;
					start_item_index = index - max_length/2;
				}
				else
				{
					start_item_index = 0;
					empty_string_before = max_length/2 - index;
				}
				if ((index + max_length/2) < items.Length)
				{
					end_item_index = index + max_length/2;
					empty_string_after = 0;
				}
				else
				{
					end_item_index = items.Length - 1;
					empty_string_after = max_length/2 - (items.Length - index - 1);

				}
				items_count = end_item_index - start_item_index + 1;
				target_index_in_names = max_length/2;
				names = new string[items_count + empty_string_before + empty_string_after];
				int current_string = 0;
				int current_item;
				for (int i = 0; i < empty_string_before; i++)
				{
					names[current_string++] = " ";
				}
				current_item = start_item_index;
				for (int i = 0; i < items_count; i++)
				{
					if (items[current_item].final)
					{
						names[current_string++] = current_display_prefix + items[current_item].full_display;
					}
					else
					{
						names[current_string++] = current_display_prefix + items[current_item].prefix + items[current_item].suffix;
					}
					current_item++;
				}
				for (int i = 0; i < empty_string_after; i++)
				{
					names[current_string++] = " ";
				}
			}

			public void load_display(CrossingList target, int list_length, int scrolling_window_length, int highlighted_location)
			{
				load_display(target, list_length, scrolling_window_length, highlighted_location, current_item_displayed);
			}
			public void load_display(CrossingList target, int list_length, int scrolling_window_length, int highlighted_location, int item)
				{
				string[] names_to_load;
				int target_index_in_names;
				get_names_to_load(item, list_length, scrolling_window_length, out names_to_load, out target_index_in_names);
				target.Items.Clear();
				target.Items.AddRange(names_to_load);
				target.set_display(target_index_in_names - highlighted_location, target_index_in_names);
				// target.TopIndex = target_index_in_names - highlighted_location;
				// target.SelectedIndex = target_index_in_names;
				current_item_displayed = item;
				current_reference_item = item;
				current_reference_item_in_name = target_index_in_names;
			} 
			
			public DisplayLevel load_display_up(CrossingList target, int list_length, int windows_length, int highlighted_location)
			{
				return(load_display_up(target, list_length, windows_length, highlighted_location, current_item_displayed));
			}

			public DisplayLevel load_display_up(CrossingList target, int list_length, int windows_length, int highlighted_location, int from_item)
			{
				DisplayLevel result;
				if ((next_level != null) && !(items[from_item].final))
				{
					result = next_level;
					current_reference_item = from_item;
					int index_for_next_level = items[from_item].next_index;
					result.load_display(target,list_length, windows_length, highlighted_location, index_for_next_level);
					result = target.on_name_selected(result, result.items[from_item].prefix + result.items[from_item].suffix, false);
					// current_reference_item = index_for_next_level;
				}
				else
				{
					if (items[from_item].final)
					{
						result = target.on_name_selected(this, items[from_item].full_display, true);
					}
					else
					{
						result = this;
					}
				}
				return(result);
			}
			
			public DisplayLevel load_display_down(CrossingList target, int list_length, int windows_length, int highlighted_location)
			{
				DisplayLevel result;
				if (previous_level != null)
				{
					result = previous_level;
					int index_for_previous_level = items[current_item_displayed].previous_index;
					//result.load_display(target,list_length, scrolling_window_height, highlighted_location, result.current_reference_item);
					result.load_display(target,list_length, scrolling_window_height, highlighted_location, items[current_item_displayed].previous_index);
					result = target.on_name_selected(result, result.items[index_for_previous_level].prefix + result.items[index_for_previous_level].suffix, false);
					// current_reference_item = index_for_previous_level;
				}
				else
				{
					result = target.on_root_reached(this);
				}
				return(result);
			}

			public void update_display(double percent, CrossingList target, int scrolling_window_length, int highlighted_location)
			{
				int requested_delta = (int)Math.Round(percent*scrolling_window_length);
				int requested_item = requested_delta + current_reference_item;
				if (requested_item < 0)
				{
					requested_delta = -current_reference_item;
				}
				if (!(requested_item < items.Length))
				{
					requested_delta = items.Length - current_reference_item - 1;
				}
				current_item_displayed = requested_delta + current_reference_item;
				target.set_display(requested_delta + current_reference_item_in_name - highlighted_location,
				                   requested_delta + current_reference_item_in_name);
			}
			
			public static DisplayLevel create_levels(StringCollection new_members)
			{
				DisplayLevel[] displays;
				if (new_members.Count > 0)
				{
					CharBucketTreeNode tree = new CharBucketTreeNode(new_members);
					int tree_height = tree.max_level_bellow;
					displays = new DisplayLevel[tree_height + 1];
					for (int i = tree_height; i >= 0; i--)
					{
						ArrayList buckets_in_level = new ArrayList();
						tree.get_buckets_in_level(buckets_in_level,i);
						displays[i] = new DisplayLevel(buckets_in_level);
					}
					for (int i = 0; i < tree_height; i++)
					{
						displays[i].next_level = displays[i + 1];
						displays[tree_height - i].previous_level = displays[tree_height - i - 1];
					}
					displays[tree_height].next_level = null;
					displays[0].previous_level = null;
					displays[0].items_tree = tree;
				}
				else
				{
					displays = new DisplayLevel[1];
					displays[0] = new DisplayLevel();
					displays[0].next_level = null;
					displays[0].previous_level = null;
					displays[0].items = new Item[1];
					displays[0].items[0] = new Item();
				}
				return(displays[0]);
			}

			protected Item[] items;
			protected DisplayLevel next_level;
			protected DisplayLevel previous_level;
			protected string[] names;
			protected int list_height;
			protected int current_item_displayed;
			protected int current_reference_item;
			protected int current_reference_item_in_name;
			private CharBucketTreeNode items_tree = null;
			protected string current_display_prefix = "";

			public void set_display_prefix(string new_prefix)
			{
				current_display_prefix = new_prefix;
				if (next_level != null)
				{
					next_level.set_display_prefix(new_prefix);
				}
			}
			
			public int items_count()
			{
				return(items.Length);
			}

			public bool is_root_level()
			{
				return(previous_level == null);
			}

			public int current_item()
			{
				return(current_item_displayed);
			}

			public DisplayLevel(int new_list_height)
			{
				list_height = new_list_height;
			}

			public virtual int target_position_index(int current_offset, Point current_point)
			{
				return(0);
			}

			public string[] names_to_load()
			{
				return(names);
			}

			public void find_name(string target_name, out DisplayLevel target_level, out int item_in_level)
			{
				target_level = null;
				item_in_level = -1;
				if (items_tree != null)
				{
					CharBucket target_bucket = items_tree.find_name(target_name);
					if (target_bucket != null)
					{
						Item target_item = target_bucket.assigned_item;
						target_level = target_item.my_level;
						item_in_level = target_item.index_in_level;
					}
				}
			}
		}

		private const int control_height = 230;
		private const int control_height_in_items = 11;
		private const int selected_item_height = 5;
		private const int column_width = 18;
		private const int scrolling_window_height = 20;
		protected DisplayLevel display_tree_root;
		protected DisplayLevel current_level;

		private Point previous_point;
		private Point touch_down;
		private Point segment_started_at;
		private int starting_item;
		private int previous_level;
		private Queue angle_queue = new Queue();
		private int E_direction_count;
		private int W_direction_count;
		private const int direction_count_threshold = 12; // previous 12
		private Queue selected_items_queue = new Queue();
		private Queue points_queue = new Queue();
		private const int points_queue_length = 6; //previous 6
		private const int selected_items_queue_length = 6;
		private const double half_window_angle = 30;
		private const int trigger_segment_length = 20; // previous 20, how long to travel before entering next level
		private bool just_entered = false;

		private void enqueue_new_item(int item)
		{
			selected_items_queue.Enqueue(item);
			//Console.Write(item+ " ");
			if (selected_items_queue.Count > selected_items_queue_length)
			{
				selected_items_queue.Dequeue();
			}
		}
	
		private void clear_items_queue()
		{
			points_queue.Clear();
			//Console.Write("\n");
		}

		private int get_initial_item()
		{
			int result;
			if (selected_items_queue.Count > 1)
			{
				result = (int)selected_items_queue.Peek();
			}
			else
			{
				result = 0;
			}
			//Console.Write("->"+result+"!");
			return(result);
		}


		private void enqueue_new_point(Point new_point)
		{
			points_queue.Enqueue(new_point);
			//Console.Write(item+ " ");
			if (points_queue.Count > selected_items_queue_length)
			{
				points_queue.Dequeue();
			}
		}
		private void clear_points_queue()
		{
			selected_items_queue.Clear();
			//Console.Write("\n");
		}

		private Point get_initial_point()
		{
			Point result;
			if (points_queue.Count > 1)
			{
				result = (Point)points_queue.Peek();
			}
			else
			{
				result = new Point(0,0);
			}
			//Console.Write("->"+result+"!");
			return(result);
		}




		// possible improvement
		// Only make transition from right to left not the otherway around.
		// Two level per character

		void set_display(int top, int selected)
		{
			if (top < 0)
			{
				top = 0;
			}
			if (top > this.Items.Count - 1)
			{
				top = this.Items.Count - 1;
			}
			if (selected < 0)
			{
				selected = 0;
			}
			if (selected > this.Items.Count - 1)
			{
				selected = this.Items.Count - 1;
			}
			TopIndex = top;
			SelectedIndex = selected;
		}

		public CrossingList()
		{
			Font = new Font("Monaco",12);
			Location = new Point(100,100);
			Size = new Size(150,control_height);
			// UnixSpellDict unix_spell_dict = new UnixSpellDict();
			// load_names((new UnixSpellDict()).unix_spell_dict.words);
			// this.SelectionMode = SelectionMode.None;
			// display_levels[0] = new SimpleDisplayLevel(100,0);
			// display_levels[1] = new SimpleDisplayLevel(100,1);
			// display_levels[2] = new SimpleDisplayLevel(100,2);
		}

		protected virtual DisplayLevel on_name_selected(DisplayLevel current_level, string name, bool final)
		{
			if (final)
			{
				Console.Write(name + "\n");
			}
			return(current_level);
		}

		protected virtual DisplayLevel on_root_reached(DisplayLevel current_level)
		{
			Console.Write("Root reached!\n");
			return(current_level);
		}

		public void load_names(StringCollection new_names)
		{
			display_tree_root = DisplayLevel.create_levels(new_names);
			current_level = display_tree_root;
			current_level.load_display(this, control_height_in_items, scrolling_window_height, selected_item_height, (current_level.items_count() - 1)/2);
		}

		public void select_item(string target)
		{
			DisplayLevel target_level;
			int target_index;
			display_tree_root.find_name(target, out target_level, out target_index);
			if (target_level != null)
			{
				target_level.load_display(this, control_height_in_items, scrolling_window_height, selected_item_height, target_index);
				current_level = target_level;
			}
		}
		
		private void update_current_level(Point current_point, Point previous_point, bool first_point_in_stroke)
		{
			double current_angle = Angle(current_point,-1);
			// Console.Write("<" + current_angle);
			if (current_angle >= 0)
			{
				if ((current_angle < half_window_angle) || (current_angle > (360 - half_window_angle)))
				{
					if (E_direction_count == 0)
					{
						segment_started_at = get_initial_point();
						starting_item = get_initial_item();
						this.Invalidate();
					}
					E_direction_count++;
					//if (E_direction_count > direction_count_threshold)
					if (current_point.X - segment_started_at.X > trigger_segment_length)
					{
						current_level = current_level.load_display_up(this, control_height_in_items, scrolling_window_height, selected_item_height, starting_item);
						touch_down = current_point;
						angle_queue.Clear();
						clear_items_queue();
						clear_points_queue();
						E_direction_count = 0;
						this.Invalidate();
					}
					W_direction_count = 0;
				}
				else if ((current_angle < (180 + half_window_angle)) && (current_angle > (180 - half_window_angle)))
				{
					if (W_direction_count == 0)
					{
						segment_started_at = get_initial_point();
						this.Invalidate();
					}
					W_direction_count++;
					// if (W_direction_count > direction_count_threshold)
					if (segment_started_at.X - current_point.X > trigger_segment_length)
					{
						
						current_level = current_level.load_display_down(this, control_height_in_items, scrolling_window_height, selected_item_height);
						touch_down = current_point;
						angle_queue.Clear();
						clear_items_queue();
						clear_points_queue();
						W_direction_count = 0;
						this.Invalidate();
					}
					E_direction_count = 0;
				}
				else
				{
					if ((E_direction_count != 0) || (W_direction_count != 0))
					{
						E_direction_count = 0;
						W_direction_count = 0;
						// Invalidate();
					}
					else
					{
						E_direction_count = 0;
						W_direction_count = 0;
						// Invalidate();
					}
				}
			}
		}

		protected void init_mouse_events_processing(Point starting_point)
		{
			angle_queue.Clear();
			clear_points_queue();
			previous_point = starting_point;
			E_direction_count = 0;
			W_direction_count = 0;
			touch_down = starting_point;
			current_level.load_display(this, control_height_in_items, scrolling_window_height, selected_item_height);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			//Console.Write("v");
			Win32Application.Win32.ReleaseCapture();
			IntPtr down_cursor  = Win32Application.Win32.LoadCursorFromFile(@"cursors/pen_down.cur");
			this.Cursor = new System.Windows.Forms.Cursor(down_cursor);
			init_mouse_events_processing(new Point(e.X,e.Y));
			this.Capture = true;
			just_entered = false;
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
		}

		protected override void OnMouseHover(EventArgs e)
		{
			IntPtr down_cursor  = Win32Application.Win32.LoadCursorFromFile(@"cursors/pen_up.cur");
			this.Cursor = new System.Windows.Forms.Cursor(down_cursor);
			//Console.Write("?");
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			//Console.Write("<\n");
			just_entered = false;
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			//Console.Write("^");
			IntPtr down_cursor  = Win32Application.Win32.LoadCursorFromFile(@"cursors/pen_up.cur");
			this.Cursor = new System.Windows.Forms.Cursor(down_cursor);
			//this.Cursor = util.CursorSwitcher.change_mouse_cursor(true);
			this.Capture = false;
		}

		protected void draw_feedback(Graphics context)
		{
			Pen myPen = new Pen(Color.Blue, 3);
			if (E_direction_count != 0)
			{
				context.DrawLine(myPen,segment_started_at.X + trigger_segment_length, segment_started_at.Y - 10,
					segment_started_at.X + trigger_segment_length, segment_started_at.Y + 10);
			}
			if (W_direction_count != 0)
			{
				context.DrawLine(myPen,segment_started_at.X - trigger_segment_length, segment_started_at.Y - 10,
					segment_started_at.X - trigger_segment_length, segment_started_at.Y + 10);
			}
		}
		
		protected override void WndProc(ref Message m) 
		{
			// Listen for operating system messages.
			base.WndProc(ref m);
			switch (m.Msg)
			{
					// The WM_ACTIVATEAPP message occurs when the application
					// becomes the active application or becomes inactive.
				case 0x000F: // WM_PAINT

					//Console.Write("P");
					IntPtr hwnd = new IntPtr();
					hwnd = m.HWnd;
					Graphics context = Graphics.FromHwnd(hwnd);//CreateGraphics();
					draw_feedback(context);
					break;                
			}
		}
			protected void draw_feedback(PaintEventArgs e)
		{
			//Console.Write("P");
			Graphics menug = e.Graphics;//CreateGraphics();
			Pen myPen = new Pen(Color.Blue, 3);
			if ((E_direction_count != 0) || (W_direction_count != 0))
			{
				menug.DrawLine(myPen,segment_started_at.X + trigger_segment_length, segment_started_at.Y - 10,
					segment_started_at.X + trigger_segment_length, segment_started_at.Y + 10);
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			//if (e.Button == MouseButtons.Left)
			if (((e.Button & MouseButtons.Left) != 0) || ((e.Button & MouseButtons.Right) != 0))
			{
				if (just_entered)
				{
					//Console.Write("v");
					Win32Application.Win32.ReleaseCapture();
					init_mouse_events_processing(new Point(e.X,e.Y));
					this.Capture = true;
					just_entered = false;
				}
				
				// Console.Write("!");
				double target_position;
				Point current_point = new Point(e.X, e.Y);
				update_current_level(current_point,previous_point,false);
				target_position = (double)(current_point.Y - touch_down.Y)/(double)(this.Height);
				//if (target_position < -.5)
				//{
				//	target_position = -.5;
				//}
				//else
				//{
				//	if (target_position > .5)
				//	{
				//		target_position = .5;
				//	}
				//}
				// Console.Write(target_position + " ");
				int item_before_update = current_level.current_item();
				current_level.update_display(target_position,this, scrolling_window_height, selected_item_height);
				if (item_before_update != current_level.current_item())
				{
					Invalidate();
				}
				enqueue_new_item(current_level.current_item());
				enqueue_new_point(new Point(e.X,e.Y));
				// this.Invalidate();
			}
			else
			{
				// Console.Write(".");
			}
		}
		private int Angle(Point current, int old_angle)
		{
			if (previous_point != current && (previous_point != new Point(0,0)))
			{
				// Console.Write("$");
				int negative = -1;
				int positive = 1;
				int equal = 0;
				int xdiff, ydiff;
				double angle;
				double averageangle = 0.0;
				double realaverageangle = 0.0;
				double aVangle;

				double distance = Math.Sqrt((Math.Pow((double)(previous_point.X-current.X),2))+(Math.Pow((double)(previous_point.Y-current.Y),2)));
				//Console.Write(angle+ "\n");
				if (distance >= 1)
				{
					xdiff = current.X - previous_point.X;
					ydiff = current.Y- previous_point.Y;
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
					// Console.Write("\"" + angle + "\"");
					angle_queue.Enqueue(angle);
					while (angle_queue.Count > 6)
					{
						angle_queue.Dequeue();
					}
					double correction_for_cycle = 0;
					double average_with_correction_for_cycle;
					double average_without_correction_for_cycle;
					System.Collections.IEnumerator myEnumerator = angle_queue.GetEnumerator();
					while ( myEnumerator.MoveNext() )
					{
						averageangle = averageangle + (double)myEnumerator.Current;
						if((double)myEnumerator.Current > 180)
						{
							correction_for_cycle = correction_for_cycle + -360;
						}
					}
					average_without_correction_for_cycle = averageangle/angle_queue.Count;
					average_with_correction_for_cycle = (averageangle + correction_for_cycle)/angle_queue.Count;
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
					if (angle_queue.Count == 6)
					{
						// nothing to be done
					}
					else if (angle_queue.Count <= 5)
					{
						// Console.Write("#");
						realaverageangle = -1;
					}
					else
					{
						// Console.Write("@@");
						realaverageangle = -1;
					}
					previous_point = current;
					return (int)realaverageangle;
				} // distance less than something
				else
				{
					// Console.Write("%"+distance);
					return(old_angle);
				}
			} // same point
			else
			{
				previous_point = current;
				///Console.Write(oldangle+" |");
				// Console.Write("&");
				return old_angle;
		}
		}
		

	}

}
