using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace EuroSoundExplorer2.CustomControls
{
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------
    public partial class ListView_ColumnSortingClick : ListView
    {
        private ColumnHeader SortingColumn = null;

        [StructLayout(LayoutKind.Sequential)]
        public struct LV_ITEM
        {
            public uint mask;
            public int iItem;
            public int iSubItem;
            public uint state;
            public uint stateMask;
            public string pszText;
            public int cchTextMax;
            public int iImage;
            public IntPtr lParam;
        }

        public const int LVM_FIRST = 0x1000;
        public const int LVM_GETITEM = LVM_FIRST + 5;
        public const int LVM_SETITEM = LVM_FIRST + 6;
        public const int LVIF_TEXT = 0x0001;
        public const int LVIF_IMAGE = 0x0002;

        public const int LVW_FIRST = 0x1000;
        public const int LVM_GETEXTENDEDLISTVIEWSTYLE = LVW_FIRST + 54;

        public const int LVS_EX_GRIDLINES = 0x00000001;
        public const int LVS_EX_SUBITEMIMAGES = 0x00000002;
        public const int LVS_EX_CHECKBOXES = 0x00000004;
        public const int LVS_EX_TRACKSELECT = 0x00000008;
        public const int LVS_EX_HEADERDRAGDROP = 0x00000010;
        public const int LVS_EX_FULLROWSELECT = 0x00000020; // applies to report mode only
        public const int LVS_EX_ONECLICKACTIVATE = 0x00000040;
        public const int LVS_EX_DOUBLEBUFFER = 0x00010000;

        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hWnd, int msg, int wParam, ref LV_ITEM lParam);

        //-------------------------------------------------------------------------------------------------------------------------------
        public static void AddImageToSubItem(ListViewItem itemToAdd, int subItemIndex, int imageIndex, IntPtr handle)
        {
            LV_ITEM lvi = new LV_ITEM
            {
                // Row
                iItem = itemToAdd.Index,
                // Column
                iSubItem = 1,
                pszText = itemToAdd.SubItems[subItemIndex].Text,
                mask = LVIF_IMAGE | LVIF_TEXT,
                // Image index on imagelist
                iImage = imageIndex
            };
            SendMessage(handle, LVM_SETITEM, 0, ref lvi);
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        public ListView_ColumnSortingClick()
        {
            InitializeComponent();
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            // Change the style of listview to accept image on subitems
            Message m = new Message
            {
                HWnd = Handle,
                Msg = LVM_GETEXTENDEDLISTVIEWSTYLE,
                LParam = (IntPtr)(LVS_EX_GRIDLINES | LVS_EX_FULLROWSELECT | LVS_EX_SUBITEMIMAGES | LVS_EX_DOUBLEBUFFER | LVS_EX_TRACKSELECT),
                WParam = IntPtr.Zero
            };
            WndProc(ref m);
        }

        //-------------------------------------------------------------------------------------------------------------------------------
        private void ListView_Extended_ColumnSorting_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            //Get the new sorting column.
            ColumnHeader new_sorting_column = Columns[e.Column];

            //Figure out the new sorting order.
            SortOrder sort_order;
            if (SortingColumn == null)
            {
                //New column. Sort ascending.
                sort_order = SortOrder.Ascending;
            }
            else
            {
                //See if this is the same column.
                if (new_sorting_column == SortingColumn)
                {
                    //Same column. Switch the sort order.
                    if (SortingColumn.Text.StartsWith("> "))
                    {
                        sort_order = SortOrder.Descending;
                    }
                    else
                    {
                        sort_order = SortOrder.Ascending;
                    }
                }
                else
                {
                    //New column. Sort ascending.
                    sort_order = SortOrder.Ascending;
                }

                //Remove the old sort indicator.
                SortingColumn.Text = SortingColumn.Text.Substring(2);
            }

            //Display the new sort order.
            SortingColumn = new_sorting_column;
            if (sort_order == SortOrder.Ascending)
            {
                SortingColumn.Text = "> " + SortingColumn.Text;
            }
            else
            {
                SortingColumn.Text = "< " + SortingColumn.Text;
            }

            //Create a comparer.
            ListViewItemSorter = new ListView_ColumnSorting(e.Column, sort_order);

            //Sort.
            Sort();
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
    internal class ListView_ColumnSorting : System.Collections.IComparer
    {
        private readonly int ColumnNumber;
        private readonly SortOrder SortOrder;

        internal ListView_ColumnSorting(int column_number, SortOrder sort_order)
        {
            ColumnNumber = column_number;
            SortOrder = sort_order;
        }

        //Compare two ListViewItems.
        public int Compare(object object_x, object object_y)
        {
            //Get the objects as ListViewItems.
            ListViewItem item_x = object_x as ListViewItem;
            ListViewItem item_y = object_y as ListViewItem;

            //Get the corresponding sub-item values.
            string string_x;

            if (item_x.SubItems.Count <= ColumnNumber)
            {
                string_x = "";
            }
            else
            {
                string_x = item_x.SubItems[ColumnNumber].Text;
            }

            string string_y;
            if (item_y.SubItems.Count <= ColumnNumber)
            {
                string_y = "";
            }
            else
            {
                string_y = item_y.SubItems[ColumnNumber].Text;
            }

            //Compare them.
            int result;
            if (double.TryParse(string_x, out double double_x) && double.TryParse(string_y, out double double_y))
            {
                //Treat as a number.
                result = double_x.CompareTo(double_y);
            }
            else
            {
                if (DateTime.TryParse(string_x, out DateTime date_x) && DateTime.TryParse(string_y, out DateTime date_y))
                {
                    //Treat as a date.
                    result = date_x.CompareTo(date_y);
                }
                else
                {
                    //Treat as a string.
                    result = string_x.CompareTo(string_y);
                }
            }

            //Return the correct result depending on whether
            //we're sorting ascending or descending.
            if (SortOrder == SortOrder.Ascending)
            {
                return result;
            }
            else
            {
                return -result;
            }
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
}
