using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace StocksHelper
{
    public class ChartColumn : DataGridViewColumn
    {
        public ChartColumn() : base(new ChartCell())
        {

        }

        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                // Ensure that the cell used for the template is a CalendarCell.
                if (value != null &&
                    !value.GetType().IsAssignableFrom(typeof(ChartCell)))
                {
                    throw new InvalidCastException("Must be a ChartCell");
                }
                base.CellTemplate = value;
            }
        }


        //public override object Clone()
        //{
        //    return base.Clone();
        //}

    }


    public class ChartCell : DataGridViewCell
    {

        public ChartCell() : base()
        {
            // Use the short date format.
            //this.Style.Format = "d";
        }

        //public override void InitializeEditingControl(int rowIndex, object
        //    initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        //{
        //    //// Set the value of the editing control to the current cell value.
        //    //base.InitializeEditingControl(rowIndex, initialFormattedValue,
        //    //    dataGridViewCellStyle);
        //    //CalendarEditingControl ctl =
        //    //    DataGridView.EditingControl as CalendarEditingControl;
        //    //// Use the default row value when Value property is null.
        //    //if (this.Value == null)
        //    //{
        //    //    ctl.Value = (DateTime)this.DefaultNewRowValue;
        //    //}
        //    //else
        //    //{
        //    //    ctl.Value = (DateTime)this.Value;
        //    //}
        //}

        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            try
            {
                var ctrl = (Chart)value;
            if (ctrl != null)
            {
                var img = new Bitmap(cellBounds.Width, cellBounds.Height);
                ctrl.DrawToBitmap(img, new Rectangle(0, 0, ctrl.Width, ctrl.Height));
                graphics.DrawImage(img, cellBounds.Location);
            }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //protected override void OnClick(DataGridViewCellEventArgs e)
        //{
        //    List<InfoObject> objs = DataGridView.DataSource as List<InfoObject>;
        //    if (objs == null)
        //        return;
        //    if (e.RowIndex < 0 || e.RowIndex >= objs.Count)
        //        return;

        //    CustomUserControl ctrl = objs[e.RowIndex].Ctrl;
        //    // Take any action - I will just change the color for now.
        //    ctrl.BackColor = Color.Red;
        //    ctrl.Refresh();
        //    DataGridView.InvalidateCell(e.ColumnIndex, e.RowIndex);
        //}

        public override Type EditType
        {
            get
            {
                // Return the type of the editing control that CalendarCell uses.
                return typeof(Chart); ;
            }
        }

        public override Type ValueType
        {
            get
            {
                // Return the type of the value that CalendarCell contains.

                return typeof(Chart);
            }
        }

        public override Type FormattedValueType
        {
            get
            {
                // Return the type of the value that CalendarCell contains.

                return typeof(Chart);
            }
        }

        public override object DefaultNewRowValue
        {
            get
            {
                // Use the current date and time as the default value.
                return new Chart();
            }
        }

        public override object Clone()
        {
            return base.Clone();
        }
    }


}
