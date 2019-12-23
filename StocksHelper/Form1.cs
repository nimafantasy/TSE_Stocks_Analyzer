using HtmlAgilityPack;
using Ionic.Zip;
using StocksHelper.Data;
using StocksHelper.Enum;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml;
using static System.Windows.Forms.ListViewItem;

namespace StocksHelper
{
    public partial class Form1 : Form
    {
        public Market market { get; set; }

        public List<Criterion> criteria { get; set; }

        PersianCalendar pc = new PersianCalendar();

        private bool AnalyticMode = false;

        private List<Stock> CompareBundle = new List<Stock>();

        private List<StepBundle> online_data;

        private CancellationTokenSource cancellationTokenSource;
        CancellationTokenSource cancellationTokenSource1 = new CancellationTokenSource();
        CancellationTokenSource cancellationTokenSource2 = new CancellationTokenSource();

        private DataTable dgvdt;

        private List<Entry> Entries = new List<Entry>();

        public Form1()
        {
            InitializeComponent();
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            dtpProspects.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 00, 00, 0);
            dateTimePicker1.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 00, 00, 0);
            dtpProspects.ValueChanged += new EventHandler(dtpProspects_ValueChanged);
            dtpProspects.CustomFormat = "dd-MM-yyyy";
            online_data = new List<StepBundle>();
            dgvdt = new DataTable();
            //DataLoader.TempDataCollector();
            // create stocks objects and load data using dataloader
            criteria = DataLoader.LoadCriteria();
            //dgvProspects.RowTemplate.Height = 100;

            market = new Market();
            market = DataLoader.LoadMarket();

            LoadComboBoxes();

            market.ApplyCriteria(criteria, DateTime.Now);
            int i = 0;
            int y = 0;
            foreach (Criterion cri in criteria)
            {
                foreach (Enum.DataNames nm in System.Enum.GetValues(typeof(Enum.DataNames)))
                {
                    //Console.WriteLine(nm);
                    CheckBox chkbx = new CheckBox();
                    chkbx.Width = chkPanel.Width - 20;

                    chkbx.Text = string.Format(cri.Name, nm);

                    if (y % 2 == 0)
                        chkbx.ForeColor = Color.Crimson;

                    chkbx.Name = cri.id + "_" + nm;
                    chkPanel.Controls.Add(chkbx);
                    chkbx.CheckedChanged += new System.EventHandler(this.cb_CheckedChanged);
                    chkbx.RightToLeft = RightToLeft.Yes;
                    chkPanel.Controls[i].Location = new Point(0, 10 + 20 * i);
                    i++;
                }
                y++;

            }

            // load "draw chart by" check
            foreach (Enum.DataNames nm in System.Enum.GetValues(typeof(Enum.DataNames)))
            {
                //Console.WriteLine(nm);
                CheckBox chkbx = new CheckBox();
                chkbx.RightToLeft = RightToLeft.Yes;
                chkbx.Width = 300;

                chkbx.Text = nm.ToString();

                clbDrawChartBy.Items.Add(nm);

            }




            //XmlDocument xml = new XmlDocument();
            //XmlElement root = xml.CreateElement("market");
            //xml.AppendChild(root);

            //var directories = Directory.GetDirectories(@"D:\Stocks\TSE\Adjusted");

            //foreach (string dir in directories)
            //{
            //    XmlElement child = xml.CreateElement("category");
            //    child.SetAttribute("name", dir.Split(new[] { "\\" }, StringSplitOptions.None).LastOrDefault());
            //    child.SetAttribute("filename", "");
            //    root.AppendChild(child);

            //    var files = Directory.GetFiles(dir);

            //    foreach (string file in files)
            //    {
            //        var reader = new StreamReader(File.OpenRead(file));
            //        string stockname = "";
            //        bool first_line = true;
            //        while (!reader.EndOfStream)
            //        {
            //            var line = reader.ReadLine();
            //            if (first_line)
            //            {
            //                first_line = false;
            //                continue;
            //            }

            //            stockname = line.Split(',')[0];
            //            break;
            //        }
            //        XmlElement child2 = xml.CreateElement("stock");
            //        child2.SetAttribute("name", stockname);
            //        child.AppendChild(child2);

            //        XmlElement child3 = xml.CreateElement("filename");
            //        child3.InnerText = file.Split(new[] { "\\" }, StringSplitOptions.None).LastOrDefault();
            //        child2.AppendChild(child3);

            //    }

            //}
            //string ssssss = xml.OuterXml;

        }


        private void cb_CheckedChanged(Object sender, EventArgs e)
        {
            //dgvProspects.Rows.Clear();
            //dgvProspects.Columns.Clear();



            //CompareBundle.Clear();
            //dgvProspects.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "گروه" });
            //dgvProspects.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "سهام" });
            //dgvProspects.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "عرض کانال" });
            //dgvProspects.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "موقعیت حمایت/مقاومت" });
            //dgvProspects.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "تعداد روزهای مثبت یا منفی" });
            //if (AnalyticMode)
            //    dgvProspects.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "عملکرد" });



            ////lvProspects.Items.Clear();
            ////lvProspects.Columns.Clear();
            ////CompareBundle.Clear();
            ////lvProspects.FullRowSelect = true;
            ////lvProspects.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            ////lvProspects.Columns.Add("گروه");
            ////lvProspects.Columns.Add("سهام");
            ////lvProspects.Columns.Add("عرض کانال");
            ////lvProspects.Columns.Add("موقعیت حمایت/مقاومت");
            ////if (AnalyticMode)
            ////    lvProspects.Columns.Add("عملکرد");
            ////lvProspects.Columns[0].Width = 250;
            ////lvProspects.Columns[1].Width = 150;
            ////lvProspects.Columns[2].Width = 150;
            ////lvProspects.Columns[3].Width = 150;
            ////if (AnalyticMode)
            ////    lvProspects.Columns[4].Width = 150;


            //string NameSet = (sender as CheckBox).Name;
            ////MessageBox.Show(NameSet);
            //int counter = 0;
            Entries.Clear();
            foreach (Category cat in market.Categories)
            {
                if (!chkbxCat.Checked)
                {
                    foreach (Stock stk in cat.Stocks)
                    {

                        bool violated = false;
                        bool foundIt = false;
                        foreach (Control cnt in chkPanel.Controls)
                        {
                            CheckBox chkbx = (CheckBox)cnt;
                            foundIt = false;
                            if (chkbx.Checked)
                            {
                                // checked rule must be present in stock's passedcriteria list
                                foreach (Property pr in stk.PassedCriteria)
                                {
                                    if (pr.Criterion.id == chkbx.Name.Split('_')[0].ToString() && pr.Name.ToString() == chkbx.Name.Split('_')[1].ToString())
                                    {
                                        foundIt = true;
                                        break;
                                    }

                                }

                                if (!foundIt)
                                {
                                    violated = true;
                                }


                            }

                            if (violated)
                            {
                                break;
                            }
                        }

                        if (!violated && stk.IsActive && stk.IsCompetent)
                        {
                            //counter++;
                            // add to list
                           // string[] positionandwisth = GetSRPosition(stk).Split(':');
                            //ListViewItem lvi = new ListViewItem();
                            //*****************
                            //dgvProspects.Rows.Add(dgvr);
                            //****************
                            if (!AnalyticMode)
                            {

                            }
                            else
                            {

                            }

                            //if (counter % 2 == 0)
                            //    lvi.BackColor = Color.DarkGray;

                            //lvProspects.Items.Add(lvi);
                            //CompareBundle.Add(stk);

                            Entry ent = new Entry();
                            ent.name = stk.Name + Environment.NewLine + stk.ParentCategory.Name;
                            ent.stock = stk;
                            ent.index_chart = GetChart(stk.Close);
                            ent.mode = EntryMode.Stocks;
                            if (stk.IsActive && stk.IsCompetent)
                                Entries.Add(ent);
                        }
                    }
                }
                else
                {
                    bool violated = false;
                    bool foundIt = false;
                    foreach (Control cnt in chkPanel.Controls)
                    {
                        CheckBox chkbx = (CheckBox)cnt;
                        foundIt = false;
                        if (chkbx.Checked)
                        {
                            // checked rule must be present in stock's passedcriteria list
                            if (cat.CategoryIndex.PassedCriteria.Count == 0)
                            {
                                violated = true;
                            }
                            foreach (Property pr in cat.CategoryIndex.PassedCriteria)
                            {
                                if (pr.Criterion.id == chkbx.Name.Split('_')[0].ToString() && pr.Name.ToString() == chkbx.Name.Split('_')[1].ToString())
                                {
                                    foundIt = true;
                                    break;
                                }

                            }

                            if (!foundIt)
                            {
                                violated = true;
                            }


                        }

                        if (violated)
                        {
                            break;
                        }
                    }

                    if (!violated && cat.CategoryIndex.Close.List.Count != 0)
                    {
                        counter++;
                        // add to list
                        //string[] positionandwisth = GetSRPosition(cat.CategoryIndex).Split(':');
                        //ListViewItem lvi = new ListViewItem();
                        //*****************

                        //****************
                        if (!AnalyticMode)
                        {


                        }
                        else
                        {

                        }

                        Entry ent = new Entry();
                        ent.name = cat.CategoryIndex.Name;
                        ent.stock = cat.CategoryIndex;
                        ent.index_chart = GetChart(cat.CategoryIndex.Close);
                        ent.mode = EntryMode.Stocks;
                        
                        Entries.Add(ent);
                        //if (counter % 2 == 0)
                        //    lvi.BackColor = Color.DarkGray;

                        //lvProspects.Items.Add(lvi);
                    }
                }
            }

            this.dgvProspects.BeginInvoke((MethodInvoker)delegate ()
            {

                dgvProspects.Rows.Clear();
                dgvProspects.Columns.Clear();
                dgvProspects.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "گروه", Name = "instrument" });
                dgvProspects.Columns[0].Width = 90;
                dgvProspects.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "ملاحظات", Name = "desc" });
                dgvProspects.Columns[1].Width = 80;
                dgvProspects.Columns.Add(new ChartColumn() { HeaderText = "نمودار گروه", Name = "instrument_chart" });
                dgvProspects.Columns[2].Width = Constants.ChartCellWidth;


                dgvProspects.RowHeadersVisible = false;
                dgvProspects.VirtualMode = true;
                this.dgvProspects.CellValueNeeded += new DataGridViewCellValueEventHandler(dgvProspects_CellValueNeeded);
                this.dgvProspects.RowCount = Entries.Count;

            });

        }

        private DataGridViewTextBoxCell GetPerformanceList(DataBundle db, int days)
        {
            List<double> dList = market.GetSubListD(db, dtpProspects.Value, days);
            DataGridViewTextBoxCell c1 = new DataGridViewTextBoxCell();
            if (dList.Last() > 0)
            {
                int count = 0;
                var result = dList
                    .GroupBy(n => n < 0 ? ++count : count)
                    .Select(x => x.Count(n => n > 0));

                c1.Style = new DataGridViewCellStyle { ForeColor = Color.ForestGreen };
                c1.Value = result.Last();
            }
            else if (dList.Last() < 0)
            {
                int count = 0;
                var result = dList
                    .GroupBy(n => n > 0 ? ++count : count)
                    .Select(x => x.Count(n => n <= 0));

                c1.Style = new DataGridViewCellStyle { ForeColor = Color.Red };
                c1.Value = result.Last();
            }
            return c1;
        }


        //private DataGridViewTextBoxCell GetLastStreak(DataBundle db, int days)
        //{
        //    DataGridViewTextBoxCell c1 = new DataGridViewTextBoxCell();
        //    try
        //    {
        //        List<double> dList = market.GetSubListD(db, dtpProspects.Value, days);


        //        int counter = dList.Count - 1;
        //        double lstelemDaysNum = 0;
        //        if (dList.ElementAt(counter) > 0)
        //        {
        //            while (dList.ElementAt(counter) >= 0 && counter >= 1)
        //            {
        //                lstelemDaysNum += dList.ElementAt(counter) * 100;
        //                counter--;
        //            }


        //            c1.Style = new DataGridViewCellStyle { ForeColor = Color.ForestGreen };
        //        }
        //        else if (dList.ElementAt(counter) < 0)
        //        {
        //            while (dList.ElementAt(counter) <= 0 && counter >= 1)
        //            {
        //                lstelemDaysNum += dList.ElementAt(counter) * 100;
        //                counter--;
        //            }

        //            c1.Style = new DataGridViewCellStyle { ForeColor = Color.Red };
        //        }

        //        //if (lstelemDaysNum == 0)
        //        //lstelemDaysNum = 1.1;
        //        c1.Value = lstelemDaysNum;

        //        return c1;
        //    }
        //    catch (Exception ex)
        //    {
        //        c1.Value = 0;

        //        return c1;
        //    }
        //}

        //private DataGridViewTextBoxCell GetSecondLastStreak(DataBundle db, int days)
        //{
        //    DataGridViewTextBoxCell c1 = new DataGridViewTextBoxCell();

        //    try
        //    {


        //        List<double> dList = market.GetSubListD(db, dtpProspects.Value, days);

        //        int counter = dList.Count - 1;
        //        double lstelemDaysNum = 0;
        //        double sndlstelemDaysNum = 0;
        //        if (dList.ElementAt(counter) > 0)
        //        {
        //            while (dList.ElementAt(counter) >= 0 && counter >= 1)
        //            {
        //                lstelemDaysNum += dList.ElementAt(counter) * 100;
        //                counter--;
        //            }

        //            while (dList.ElementAt(counter) <= 0 && counter >= 1)
        //            {
        //                sndlstelemDaysNum += dList.ElementAt(counter) * 100;
        //                counter--;
        //            }

        //            c1.Style = new DataGridViewCellStyle { ForeColor = Color.Red };
        //        }
        //        else if (dList.ElementAt(counter) < 0)
        //        {
        //            while (dList.ElementAt(counter) <= 0 && counter >= 1)
        //            {
        //                lstelemDaysNum += dList.ElementAt(counter) * 100;
        //                counter--;
        //            }

        //            while (dList.ElementAt(counter) >= 0 && counter >= 1)
        //            {
        //                sndlstelemDaysNum += dList.ElementAt(counter) * 100;
        //                counter--;
        //            }
        //            c1.Style = new DataGridViewCellStyle { ForeColor = Color.ForestGreen };
        //        }
        //        c1.Value = sndlstelemDaysNum;

        //        return c1;
        //    }
        //    catch (Exception ex)
        //    {
        //        c1.Value = 0;

        //        return c1;
        //    }
        //}

        private string GetPerformance(Stock stk, DateTime dt)
        {
            try
            {
                int index = stk.Close.Dates.IndexOf(dt);
                if (index < 0)
                    return "-";
                int counter = 0;
                double Performance = 0;
                int ExistingStepsNumber = stk.Close.List.Count - 1 - index;

                List<double> tmp = market.GetSubListD(stk.Close, dt, ExistingStepsNumber);


                while (counter <= ExistingStepsNumber - 1 && tmp[counter] >= -0.01)
                {
                    Performance += tmp[counter];
                    counter++;
                }

                if (ExistingStepsNumber < 1)
                    return " برو عقب تر ";

                if (tmp[0] < -0.01)
                    return " ریده ";

                return Math.Round(Performance * 100, 2).ToString() + " در " + counter + " روز ";
            }
            catch (Exception ex)
            {
                return "ERROR";
            }
        }

        private string GetSRPosition(Stock stk)
        {
            double before = -99999;
            double next = 99999;
            foreach (Extreme ex in stk.Extremes)
            {
                if (ex.Level > stk.Close.List.Last() && ex.Level < next)
                {
                    next = ex.Level;
                }
                if (ex.Level < stk.Close.List.Last() && ex.Level > before)
                {
                    before = ex.Level;
                }
            }

            double canal_width = Math.Round(((next - before) / before) * 100, 2);
            double position = Math.Round(((stk.Close.List.Last() - before) / (next - before)) * 100, 2);
            if (next != 99999 && before != -99999)
                return canal_width + ":" + position;
            else if (next == 99999 || before != -99999)
                return "No Upper Bounderies: - ";
            else
                return "error:error";
        }

        private void LoadComboBoxes()
        {
            // analyse past combo boxes
            foreach (Category cat in market.Categories)
            {
                cbhCategories.Items.Add(cat.Name);
                cbCategories.Items.Add(cat.Name);
            }
        }

        private void cbhCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbhStocks.Items.Clear();
            ComboBox cb = (ComboBox)sender;

            foreach (Category cat in market.Categories)
            {
                if (cat.Name == cb.SelectedItem.ToString())
                {
                    foreach (Stock stk in cat.Stocks)
                    {
                        cbhStocks.Items.Add(stk.Name);
                    }
                }
            }

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            DateTimePicker dtp = (DateTimePicker)sender;
            PersianCalendar pc = new PersianCalendar();
            lblPersianDate.Text = string.Format("{0} / {1} / {2}", pc.GetYear(dtp.Value), pc.GetMonth(dtp.Value), pc.GetDayOfMonth(dtp.Value));
        }

        private void btnAnalyseDate_Click(object sender, EventArgs e)
        {
            market.ApplyCriteria(criteria, dateTimePicker1.Value);
            txtAnalyseResult.Clear();

            if (CompareBundle.Count == 0)
            {
                foreach (Category cat in market.Categories)
                {
                    if (cat.Name == cbhCategories.SelectedItem.ToString())
                    {
                        foreach (Stock stk in cat.Stocks)
                        {
                            if (stk.Name == cbhStocks.SelectedItem.ToString())
                            {
                                foreach (Property prp in stk.PassedCriteria)
                                {
                                    txtAnalyseResult.Text += string.Format(prp.Criterion.Name, prp.Name) + Environment.NewLine;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (Property prp in CompareBundle[0].PassedCriteria)
                {
                    //Console.WriteLine(CompareBundle.Count(x => x.PassedCriteria.Exists(y => y.Name == prp.Name && y.Criterion == prp.Criterion)));
                    if (CompareBundle.Count(x => x.PassedCriteria.Exists(y => y.Name == prp.Name && y.Criterion == prp.Criterion)) == CompareBundle.Count())
                        txtAnalyseResult.Text += string.Format(prp.Criterion.Name, prp.Name) + Environment.NewLine;
                }
            }
        }

        private void cbCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbStocks.Items.Clear();
            ComboBox cb = (ComboBox)sender;
            DrawMarketChart();

            foreach (Category cat in market.Categories)
            {
                if (cat.Name == cb.SelectedItem.ToString())
                {
                    DrawCategoryChart(cat);
                    foreach (Stock stk in cat.Stocks)
                    {
                        cbStocks.Items.Add(stk.Name);
                    }
                }
            }
        }

        private void cbStocks_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (Category cat in market.Categories)
            {
                foreach (Stock stk in cat.Stocks)
                {
                    if (stk.Name == cbStocks.SelectedItem.ToString())
                    {
                        DrawStockChart(stk, DataNames.Close);
                    }
                }

            }
            //DrawMarketChart();
            //DrawCategoryChart();
        }

        private void DrawMarketChart()
        {
            double max = Double.MinValue;
            double min = Double.MaxValue;
            chartMarket.ChartAreas.Clear();
            chartMarket.Series.Clear();
            var chartArea = new ChartArea();
            chartArea.AxisX.LabelStyle.Format = "yyyy/MM/dd";
            chartArea.AxisX.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisY.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisX.LabelStyle.Font = new Font("Consolas", 8);
            chartArea.AxisY.LabelStyle.Font = new Font("Consolas", 8);
            chartArea.AxisY.LabelStyle.Format = "{0:0}";
            chartArea.AxisX.MajorGrid.LineWidth = 0;
            chartArea.AxisY.MajorGrid.LineWidth = 0;
            chartMarket.ChartAreas.Add(chartArea);

            var series = new Series();
            series.Name = Constants.MarketIndexName;
            series.ChartType = SeriesChartType.Line;
            series.XValueType = ChartValueType.DateTime;
            chartMarket.Series.Add(series);

            foreach (Tick tck in market.MarketIndex.Ticks)
            {
                if (tck.Date > DateTime.Now.AddDays(-90))
                {
                    DataPoint dp = new DataPoint();
                    dp.XValue = tck.Date.ToOADate();
                    dp.YValues = new double[] { tck.Close };
                    chartMarket.Series[0].Points.Add(dp);
                    min = Math.Min(min, dp.YValues[0]);
                    max = Math.Max(max, dp.YValues[0]);

                }
            }
            // bind the datapoints

            chartMarket.ChartAreas[0].AxisY.Maximum = max + 500;
            chartMarket.ChartAreas[0].AxisY.Minimum = min - 500;


            int strmax = 0;
            if (market.MarketIndex.Extremes.Count > 0)
            {
                strmax = market.MarketIndex.Extremes.Max(x => x.Strength);
                foreach (Extreme ex in market.MarketIndex.Extremes)
                {
                    //draw a horizontal line
                    if (ex.Strength > Constants.ChartMinimumStrength && ex.Level >= min && ex.Level <= max)
                    {
                        StripLine stripline = new StripLine();
                        stripline.Interval = 0;
                        stripline.IntervalOffset = ex.Level; //average value of the y axis; eg: 35
                        stripline.StripWidth = 1;
                        //stripline.ToolTip = ex.Level.ToString();
                        stripline.Text = ex.Level.ToString() + " <> " + ex.Strength.ToString();
                        stripline.TextAlignment = StringAlignment.Center;
                        stripline.TextLineAlignment = StringAlignment.Center;

                        stripline.BackColor = ColorFactory.GetStrengthCodedColor(ex.Strength, strmax, Color.Red);
                        chartMarket.ChartAreas[0].AxisY.StripLines.Add(stripline);
                    }
                }
            }
            // draw!
            chartMarket.Invalidate();
        }

        private void DrawCategoryChart(Category cat)
        {
            double max = Double.MinValue;
            double min = Double.MaxValue;
            chartCategory.ChartAreas.Clear();
            chartCategory.Series.Clear();
            var chartArea = new ChartArea();
            chartArea.AxisX.LabelStyle.Format = "yyyy/MM/dd";
            chartArea.AxisX.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisY.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisX.LabelStyle.Font = new Font("Consolas", 8);
            chartArea.AxisY.LabelStyle.Font = new Font("Consolas", 8);
            chartArea.AxisY.LabelStyle.Format = "{0:0}";
            chartArea.AxisX.MajorGrid.LineWidth = 0;
            chartArea.AxisY.MajorGrid.LineWidth = 0;
            chartCategory.ChartAreas.Add(chartArea);

            var series = new Series();
            series.Name = "شاخص گروه " + cat.Name;
            series.ChartType = SeriesChartType.Line;
            series.XValueType = ChartValueType.DateTime;
            chartCategory.Series.Add(series);

            foreach (Tick tck in cat.CategoryIndex.Ticks)
            {
                if (tck.Date > DateTime.Now.AddDays(-90))
                {
                    DataPoint dp = new DataPoint();
                    dp.XValue = tck.Date.ToOADate();
                    dp.YValues = new double[] { tck.Close };
                    chartCategory.Series[0].Points.Add(dp);
                    min = Math.Min(min, dp.YValues[0]);
                    max = Math.Max(max, dp.YValues[0]);

                }
            }
            // bind the datapoints

            chartCategory.ChartAreas[0].AxisY.Maximum = max + min / 100;
            chartCategory.ChartAreas[0].AxisY.Minimum = min - min / 100;


            int strmax = 0;
            if (cat.CategoryIndex.Extremes.Count > 0)
            {
                strmax = cat.CategoryIndex.Extremes.Max(x => x.Strength);
                foreach (Extreme ex in cat.CategoryIndex.Extremes)
                {
                    //draw a horizontal line
                    if (ex.Strength > Constants.ChartMinimumStrength && ex.Level >= min && ex.Level <= max)
                    {
                        StripLine stripline = new StripLine();
                        stripline.Interval = 0;
                        stripline.IntervalOffset = ex.Level; //average value of the y axis; eg: 35
                        stripline.StripWidth = 1;
                        //stripline.ToolTip = ex.Level.ToString();
                        stripline.Text = ex.Level.ToString() + " <> " + ex.Strength.ToString();
                        stripline.BackColor = ColorFactory.GetStrengthCodedColor(ex.Strength, strmax, Color.Red);
                        stripline.TextAlignment = StringAlignment.Center;
                        stripline.TextLineAlignment = StringAlignment.Center;
                        chartCategory.ChartAreas[0].AxisY.StripLines.Add(stripline);
                    }
                }
            }
            // draw!
            chartCategory.Invalidate();
        }

        private void DrawStockChart(Stock stk, DataNames dn)
        {
            double max = Double.MinValue;
            double min = Double.MaxValue;
            chartStock.ChartAreas.Clear();
            chartStock.Series.Clear();
            var chartArea = new ChartArea();
            //chartArea.AxisX.LabelStyle.Format = string.Format("{0} / {1} / {2}",  );
            chartArea.AxisX.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisY.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisX.LabelStyle.Font = new Font("Consolas", 8);
            chartArea.AxisY.LabelStyle.Font = new Font("Consolas", 8);
            chartArea.AxisY.IntervalAutoMode = IntervalAutoMode.VariableCount;
            chartArea.AxisY.LabelStyle.Format = "{0:0}";
            chartArea.AxisX.MajorGrid.LineWidth = 0;
            chartArea.AxisY.MajorGrid.LineWidth = 0;
            chartArea.AxisX.Interval = 50;
            chartArea.CursorX.IsUserSelectionEnabled = true;
            chartArea.CursorY.IsUserSelectionEnabled = true;
            chartStock.ChartAreas.Add(chartArea);

            chartArea.AxisX.ScaleView.Zoomable = true;
            chartArea.AxisY.ScaleView.Zoomable = true;

            chartStock.MouseWheel += new MouseEventHandler(chData_MouseWheel);

            var series = new Series();
            series.Name = stk.Name;
            series.ChartType = SeriesChartType.Line;
            //series.XValueType = ChartValueType.String;// ChartValueType.DateTime;
            chartStock.Series.Add(series);


            foreach (Tick tck in stk.Ticks)
            {
                if (tck.Date > DateTime.Now.AddDays(-10000))
                {
                    DataPoint dp = new DataPoint();
                    dp.XValue = tck.Date.ToOADate();//  string.Format("{0} / {1} / {2}", pc.GetYear(tck.Date),pc.GetMonth(tck.Date), pc.GetDayOfMonth(tck.Date));
                    dp.AxisLabel = string.Format("{0}/{1}/{2}", pc.GetYear(tck.Date), pc.GetMonth(tck.Date), pc.GetDayOfMonth(tck.Date));
                    dp.ToolTip = tck.Close.ToString() + " on " + string.Format("{0}/{1}/{2}", pc.GetYear(tck.Date), pc.GetMonth(tck.Date), pc.GetDayOfMonth(tck.Date));
                    switch (dn)
                    {
                        case DataNames.Close:
                            dp.YValues = new double[] { tck.Close };
                            break;
                        case DataNames.Open:
                            dp.YValues = new double[] { tck.Open };
                            break;
                        case DataNames.High:
                            dp.YValues = new double[] { tck.High };
                            break;
                        case DataNames.Low:
                            dp.YValues = new double[] { tck.Low };
                            break;
                        case DataNames.Pendulum:
                            dp.YValues = new double[] { tck.Pendulum };
                            break;
                        case DataNames.Hammer:
                            dp.YValues = new double[] { tck.Hammer };
                            break;
                        case DataNames.NumberOfTrades:
                            dp.YValues = new double[] { tck.NumberOfTrades };
                            break;
                        case DataNames.ValueOfTrades:
                            dp.YValues = new double[] { tck.ValueOfTrades };
                            break;
                        case DataNames.Volume:
                            dp.YValues = new double[] { tck.Volume };
                            break;

                    }

                    chartStock.Series[0].Points.Add(dp);
                    min = Math.Min(min, dp.YValues[0]);
                    max = Math.Max(max, dp.YValues[0]);

                }
            }

            var series1 = new Series();
            series1.Name = Constants.MarketIndexName;
            series1.ChartType = SeriesChartType.Line;
            series1.XValueType = ChartValueType.DateTime;
            series1.Color = ColorFactory.GetTransparentColor(35, Color.LimeGreen);
            chartStock.Series.Add(series1);

            foreach (Tick tck in market.MarketIndex.Ticks)
            {
                if (tck.Date > DateTime.Now.AddDays(-10000))
                {
                    DataPoint dp = new DataPoint();
                    dp.XValue = tck.Date.ToOADate();
                    dp.YValues = new double[] { (tck.Close * max) / market.MarketIndex.Close.RecordHigh };
                    chartStock.Series[1].Points.Add(dp);

                }
            }
            var series2 = new Series();
            series2.Name = stk.ParentCategory.Name;
            series2.ChartType = SeriesChartType.Line;
            series2.XValueType = ChartValueType.DateTime;
            series2.Color = ColorFactory.GetTransparentColor(35, Color.Magenta);
            chartStock.Series.Add(series2);

            foreach (Tick tck in stk.ParentCategory.CategoryIndex.Ticks)
            {
                if (tck.Date > DateTime.Now.AddDays(-10000))
                {
                    DataPoint dp = new DataPoint();
                    dp.XValue = tck.Date.ToOADate();
                    dp.YValues = new double[] { (tck.Close * max) / stk.ParentCategory.CategoryIndex.Close.RecordHigh };
                    chartStock.Series[2].Points.Add(dp);

                }
            }
            // bind the datapoints

            chartStock.ChartAreas[0].AxisY.Maximum = max + min / 50;
            chartStock.ChartAreas[0].AxisY.Minimum = min - min / 50;


            int strmax = 0;
            if (stk.Extremes.Count > 0)
            {
                strmax = stk.Extremes.Max(x => x.Strength);
                foreach (Extreme ex in stk.Extremes)
                {
                    //draw a horizontal line
                    if (ex.Strength > Constants.ChartMinimumStrength && ex.Level >= min && ex.Level <= max)
                    {
                        StripLine stripline = new StripLine();
                        stripline.Interval = 0;
                        stripline.IntervalOffset = ex.Level; //average value of the y axis; eg: 35
                        stripline.StripWidth = 1;
                        //stripline.ToolTip = ex.Level.ToString();
                        stripline.Text = ex.Level.ToString() + " <> " + ex.Strength.ToString();
                        stripline.BackColor = ColorFactory.GetStrengthCodedColor(ex.Strength, strmax, Color.Red);
                        stripline.TextAlignment = StringAlignment.Center;
                        stripline.TextLineAlignment = StringAlignment.Center;
                        chartStock.ChartAreas[0].AxisY.StripLines.Add(stripline);
                    }
                }
            }
            // draw!
            chartStock.Invalidate();

        }


        private void chData_MouseWheel(object sender, MouseEventArgs e)
        {
            Chart chrt = (Chart)sender;
            try
            {
                if (e.Delta < 0)
                {
                    chrt.ChartAreas[0].AxisX.ScaleView.ZoomReset();
                    chrt.ChartAreas[0].AxisY.ScaleView.ZoomReset();
                }

                if (e.Delta > 0)
                {
                    double xMin = chrt.ChartAreas[0].AxisX.ScaleView.ViewMinimum;
                    double xMax = chrt.ChartAreas[0].AxisX.ScaleView.ViewMaximum;
                    double yMin = chrt.ChartAreas[0].AxisY.ScaleView.ViewMinimum;
                    double yMax = chrt.ChartAreas[0].AxisY.ScaleView.ViewMaximum;

                    double posXStart = chrt.ChartAreas[0].AxisX.PixelPositionToValue(e.Location.X) - (xMax - xMin) / 2;
                    double posXFinish = chrt.ChartAreas[0].AxisX.PixelPositionToValue(e.Location.X) + (xMax - xMin) / 2;
                    double posYStart = chrt.ChartAreas[0].AxisY.PixelPositionToValue(e.Location.Y) - (yMax - yMin) / 4;
                    double posYFinish = chrt.ChartAreas[0].AxisY.PixelPositionToValue(e.Location.Y) + (yMax - yMin) / 4;

                    chrt.ChartAreas[0].AxisX.ScaleView.Zoom(posXStart, posXFinish);
                    chrt.ChartAreas[0].AxisY.ScaleView.Zoom(posYStart, posYFinish);
                }
            }
            catch { }
        }

        void friendChart_MouseLeave(object sender, EventArgs e) { if (chartStock.Focused) chartStock.Parent.Focus(); }
        void friendChart_MouseEnter(object sender, EventArgs e) { if (!chartStock.Focused) chartStock.Focus(); }

        private void clbDrawChartBy_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            for (int ix = 0; ix < clbDrawChartBy.Items.Count; ++ix)
                if (ix != e.Index) clbDrawChartBy.SetItemChecked(ix, false);

            foreach (Category cat in market.Categories)
            {
                foreach (Stock stk in cat.Stocks)
                {
                    if (stk.Name == cbStocks.SelectedItem.ToString())
                    {
                        DrawStockChart(stk, (DataNames)clbDrawChartBy.SelectedItem);
                        double a = 0;
                        double b = 0;
                        double c = 0;
                        switch ((DataNames)clbDrawChartBy.SelectedItem)
                        {
                            case DataNames.Close:
                                {
                                    a = stk.Close.Average;
                                    b = stk.Close.RecordHigh;
                                    c = stk.Close.RecordLow;
                                    break;
                                }
                            case DataNames.Open:
                                {
                                    a = stk.Open.Average;
                                    b = stk.Open.RecordHigh;
                                    c = stk.Open.RecordLow;
                                    break;
                                }
                            case DataNames.High:
                                {
                                    a = stk.High.Average;
                                    b = stk.High.RecordHigh;
                                    c = stk.High.RecordLow;
                                    break;
                                }
                            case DataNames.Low:
                                {
                                    a = stk.Low.Average;
                                    b = stk.Low.RecordHigh;
                                    c = stk.Low.RecordLow;
                                    break;
                                }
                            case DataNames.Volume:
                                {
                                    a = stk.Volume.Average;
                                    b = stk.Volume.RecordHigh;
                                    c = stk.Volume.RecordLow;
                                    break;
                                }
                            case DataNames.ValueOfTrades:
                                {
                                    a = stk.ValueOfTrades.Average;
                                    b = stk.ValueOfTrades.RecordHigh;
                                    c = stk.ValueOfTrades.RecordLow;
                                    break;
                                }
                            case DataNames.Pendulum:
                                {
                                    a = stk.Pendulum.Average;
                                    b = stk.Pendulum.RecordHigh;
                                    c = stk.Pendulum.RecordLow;
                                    break;
                                }
                            case DataNames.NumberOfTrades:
                                {
                                    a = stk.NumberOfTrades.Average;
                                    b = stk.NumberOfTrades.RecordHigh;
                                    c = stk.NumberOfTrades.RecordLow;
                                    break;
                                }
                            default:
                                break;


                        }
                        lblRecordHigh.Text = String.Format("{0:n0}", b);
                        lblRecordLow.Text = String.Format("{0:n0}", c);
                        lblAverage.Text = String.Format("{0:n0}", a);
                        return;
                    }
                }

            }
        }

        private void dtpProspects_ValueChanged(object sender, EventArgs e)
        {
            
            AnalyticMode = true;
            market.ApplyCriteria(criteria, dtpProspects.Value);


        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            lblInfo.Text = "";

            if (txtSearch.Text.Count() > 2)
                foreach (Category cat in market.Categories)
                {
                    foreach (Stock stk in cat.Stocks)
                    {
                        if (stk.Name.Contains(txtSearch.Text))
                        {
                            lblInfo.Text += cat.Name + " **** " + stk.Name + " && ";
                        }
                    }
                }
        }

        private void clbDrawChartBy_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnAddToList_Click(object sender, EventArgs e)
        {
            foreach (Category cat in market.Categories)
            {
                foreach (Stock stk in cat.Stocks)
                {
                    if (cbhStocks.SelectedItem == null)
                    {
                        if (cbhCategories.SelectedItem.ToString() == stk.ParentCategory.Name)
                        {
                            CompareBundle.Add(stk);
                            Entry ent = new Entry();
                            ent.name = stk.Name + Environment.NewLine + stk.ParentCategory.Name;
                            ent.stock = stk;
                            ent.index_chart = GetChart(stk.Close);
                            ent.mode = EntryMode.Stocks;
                            Entries.Add(ent);
                        }

                    }
                    else if (cbhStocks.SelectedItem.ToString() == stk.Name)
                    {
                        CompareBundle.Add(stk);
                        Entry ent = new Entry();
                        ent.name = stk.Name + Environment.NewLine + stk.ParentCategory.Name;
                        ent.stock = stk;
                        ent.index_chart = GetChart(stk.Close);
                        ent.mode = EntryMode.Stocks;
                        Entries.Add(ent);
                    }
                }
            }

            txtSelection.Clear();
            foreach (Stock stk in CompareBundle)
            {
                txtSelection.Text += stk.Name + Environment.NewLine;
            }
        }

        private void btnClearList_Click(object sender, EventArgs e)
        {
            txtSelection.Clear();
            Entries.Clear();
            CompareBundle.Clear();
        }

        private void btnLiveStat_Click(object sender, EventArgs e)
        {
            btnLiveStat.Enabled = false;
            btnStopLiveMonitoring.Enabled = true;

            cancellationTokenSource = new CancellationTokenSource();
            Task task = Repeat.Interval(
                    TimeSpan.FromSeconds(Convert.ToInt32(txtInterval.Text)),
                    () => Test(), cancellationTokenSource.Token);

            //Test();

            if (chkbxCat.Checked == false)
            {


                //    CheckDatabaseForNewReports();
                //}
                //else
                //{
                //    if (chbCatGraph.Checked == false)
                //    {

                //        //cancellationTokenSource = new CancellationTokenSource();
                //        //Task task = Repeat.Interval(
                //        //        TimeSpan.FromSeconds(30),
                //        //        () => CheckDatabaseForNewReports2(), cancellationTokenSource.Token);

                //        CheckDatabaseForNewReports2();
                //    }
                //    else
                //    {
                //        //cancellationTokenSource = new CancellationTokenSource();
                //        //Task task = Repeat.Interval(
                //        //        TimeSpan.FromSeconds(40),
                //        //        () => CheckDatabaseForNewReports3(), cancellationTokenSource.Token);

                //        CheckDatabaseForNewReports3();
                //    }
                //}
            }

        }
        private void Test()
        {
            DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 00, 00, 0);
            online_data = DataLoader.LoadOnlineData(dt,LoadingSetting);
            DateTime latestdailydate = market.MarketIndex.Ticks.Last().Date;
            //try
            //{
            foreach (Entry ent in Entries)
            {
                if (ent.mode == EntryMode.Stocks)
                {
                    StepBundle stk_onine_data = online_data.Find(x => x.tsetid == ent.stock.TSETID);
                    if (stk_onine_data != null && stk_onine_data.Steps.Count() > 1)
                    {
                        //if (ent.stock.IsActive != true)
                        //{
                        //    ent.desc = "CLOSED";
                        //    continue;
                        //}

                        Step stp = stk_onine_data.Steps.Last();
                        double buy_sum = Convert.ToDouble(stp.b1volume) + Convert.ToDouble(stp.b2volume) + Convert.ToDouble(stp.b3volume);
                        double sell_sum = Convert.ToDouble(stp.s1volume) + Convert.ToDouble(stp.s2volume) + Convert.ToDouble(stp.s3volume);
                        double queue = buy_sum - sell_sum;

                        Step frststp = stk_onine_data.Steps[0];

                        Step lststp = stk_onine_data.Steps[stk_onine_data.Steps.IndexOf(stp) - 1];
                        double last_buy_sum = Convert.ToDouble(lststp.b1volume) + Convert.ToDouble(lststp.b2volume) + Convert.ToDouble(lststp.b3volume);
                        double last_sell_sum = Convert.ToDouble(lststp.s1volume) + Convert.ToDouble(lststp.s2volume) + Convert.ToDouble(lststp.s3volume);
                        double last_queue = last_buy_sum - last_sell_sum;

                        double queue_change = GetPerformance(last_queue, queue, 4);

                        ent.AbsoluteFluctuation = Math.Round((Convert.ToDouble(stk_onine_data.Steps.Max(x => x.close)) - Convert.ToDouble(stk_onine_data.Steps.Min(x => x.close))) / Convert.ToDouble(stp.yesterday_price) * 100, 2);

                        if (stk_onine_data.Steps.Count() > 0)
                        {
                            ent.vol_change = GetPerformance(ent.stock.Volume.GetValueAt(latestdailydate), Convert.ToDouble(stp.vol));
                            ent.vt_change = GetPerformance(ent.stock.ValueOfTrades.GetValueAt(latestdailydate), Convert.ToDouble(stp.value_of_trades));
                            ent.ind_chart = GetDoubleChartBar(stk_onine_data.Steps, OnlineDataCategories.Individual_buy, OnlineDataCategories.Individual_sell);
                            ent.ins_chart = GetDoubleChartBar(stk_onine_data.Steps, OnlineDataCategories.Institution_buy, OnlineDataCategories.Institution_sell);
                            ent.ask_bid_chart = GetDoubleChart(stk_onine_data.Steps, OnlineDataCategories.buy_queue, OnlineDataCategories.sell_queue);
                            ent.performance_chart = GetChart(stk_onine_data.Steps, OnlineDataCategories.performance);
                            ent.today_streak = GetPerformance(Convert.ToDouble(stp.yesterday_price), Convert.ToDouble(stp.close), 2);
                            ent.queue = queue;

                            string lststrk = GetLastStreak(ent.stock.Close, 50, 0);
                            ent.last_streak_sum = Convert.ToDouble(lststrk.Split(',')[0]);
                            ent.last_streak_days = Convert.ToInt32(lststrk.Split(',')[1]);
                            ent.last_streak_friendly = ent.last_streak_sum.ToString() + " in " + ent.last_streak_days.ToString() + " days";

                            string scndlststrk = GetSecondLastStreak(ent.stock.Close, 50, 0);
                            ent.snd_last_streak_sum = Convert.ToDouble(scndlststrk.Split(',')[0]);
                            ent.snd_last_streak_days = Convert.ToInt32(scndlststrk.Split(',')[1]);
                            ent.snd_last_streak_friendly = ent.snd_last_streak_sum.ToString() + " in " + ent.snd_last_streak_days.ToString() + " days";


                            string bb = "";

                            if (ent.stock.BaseVolume == 1)
                                bb += "No BaseVol" + Environment.NewLine;
                            else if (Convert.ToDouble(stp.vol) > ent.stock.BaseVolume)
                                bb += "Vol > BaseVol" + Environment.NewLine;
                            else if (Convert.ToDouble(stp.vol) < ent.stock.BaseVolume)
                                bb += "Vol < BaseVol" + Environment.NewLine;

                            if (Convert.ToDouble(stp.vol) > ent.stock.Volume.Average)
                                bb += "Vol > Avg" + Environment.NewLine;

                            if (Convert.ToDouble(stp.value_of_trades) > ent.stock.ValueOfTrades.Average)
                                bb += "VT > Avg" + Environment.NewLine;

                            if (Convert.ToDouble(stp.number_of_trades) > ent.stock.NumberOfTrades.Average)
                                bb += "NT > Avg" + Environment.NewLine;

                            if (Convert.ToDouble(stp.vol) >= (ent.stock.Volume.RecordHigh * 4 / 5))
                                bb += "Vol > 4/5Max" + Environment.NewLine;
                            else if (Convert.ToDouble(stp.vol) >= (ent.stock.Volume.RecordHigh * 3 / 5))
                                bb += "Vol > 3/5Max" + Environment.NewLine;
                            else if (Convert.ToDouble(stp.vol) >= (ent.stock.Volume.RecordHigh * 2 / 5))
                                bb += "Vol > 2/5Max" + Environment.NewLine;
                            else if (Convert.ToDouble(stp.vol) >= (ent.stock.Volume.RecordHigh / 5))
                                bb += "Vol > 1/5Max" + Environment.NewLine;

                            if (Convert.ToDouble(stp.value_of_trades) >= (ent.stock.ValueOfTrades.RecordHigh * 4 / 5))
                                bb += "VT > 4/5Max" + Environment.NewLine;
                            else if (Convert.ToDouble(stp.value_of_trades) >= (ent.stock.ValueOfTrades.RecordHigh * 3 / 5))
                                bb += "VT > 3/5Max" + Environment.NewLine;
                            else if (Convert.ToDouble(stp.value_of_trades) >= (ent.stock.ValueOfTrades.RecordHigh * 2 / 5))
                                bb += "VT > 2/5Max" + Environment.NewLine;
                            else if (Convert.ToDouble(stp.value_of_trades) >= (ent.stock.ValueOfTrades.RecordHigh / 5))
                                bb += "VT > 1/5Max" + Environment.NewLine;

                            if (Convert.ToDouble(stp.number_of_trades) >= (ent.stock.NumberOfTrades.RecordHigh * 4 / 5))
                                bb += "NT > 4/5Max" + Environment.NewLine;
                            else if (Convert.ToDouble(stp.number_of_trades) >= (ent.stock.NumberOfTrades.RecordHigh * 3 / 5))
                                bb += "NT > 3/5Max" + Environment.NewLine;
                            else if (Convert.ToDouble(stp.number_of_trades) >= (ent.stock.NumberOfTrades.RecordHigh * 2 / 5))
                                bb += "NT > 2/5Max" + Environment.NewLine;
                            else if (Convert.ToDouble(stp.number_of_trades) >= (ent.stock.NumberOfTrades.RecordHigh / 5))
                                bb += "NT > 1/5Max" + Environment.NewLine;

                            bb += "BaseVol cvrg: " + ent.fiftyday_basevol_surpass_percentage + Environment.NewLine;

                            bb += "fluctuation: " + ent.AbsoluteFluctuation;

                            ent.desc = bb;

                        }

                    }
                    Entries = Entries.OrderByDescending(x => x.AbsoluteFluctuation).ToList();
                    //OrderizeStocks();
                }
                else if (ent.mode == EntryMode.Categories)
                {
                    double performances_avg = 0;
                    double queue_sum = 0;
                    double counter = 0;
                    foreach (Stock stk in ent.cat.Stocks)
                    {
                        StepBundle stk_onine_data = online_data.Find(x => x.tsetid == stk.TSETID);
                        if (stk_onine_data != null && stk_onine_data.Steps.Count() > 1)
                        {
                            {
                                Step stp = stk_onine_data.Steps.Last();
                                double buy_sum = Convert.ToDouble(stp.b1volume) + Convert.ToDouble(stp.b2volume) + Convert.ToDouble(stp.b3volume);
                                double sell_sum = Convert.ToDouble(stp.s1volume) + Convert.ToDouble(stp.s2volume) + Convert.ToDouble(stp.s3volume);
                                queue_sum += buy_sum - sell_sum;

                                performances_avg += GetPerformance(Convert.ToDouble(stp.yesterday_price), Convert.ToDouble(stp.close));

                                counter++;
                            }

                        }
                    }
                    ent.ask_bid_chart = GetDoubleChartCat(ent.cat, OnlineDataCategories.buy_queue, OnlineDataCategories.sell_queue);
                    ent.ind_chart = GetDoubleChartCat(ent.cat, OnlineDataCategories.Individual_buy, OnlineDataCategories.Individual_sell);
                    ent.ins_chart = GetDoubleChartCat(ent.cat, OnlineDataCategories.Institution_buy, OnlineDataCategories.Institution_sell);
                    ent.today_streak = Math.Round((performances_avg / counter), 2);
                    ent.queue = queue_sum;

                    OrderizeCats();
                }



            }

            if (Entries.First().mode == EntryMode.Stocks)
            {
                this.dgvProspects.BeginInvoke((MethodInvoker)delegate ()
                {
                    dgvProspects.Rows.Clear();
                    dgvProspects.Columns.Clear();
                    dgvProspects.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "سهم", Name = "instrument_detailed" });
                    dgvProspects.Columns[0].Width = 60;
                    dgvProspects.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "حجم%", Name = "vol_change" });
                    dgvProspects.Columns[1].Width = 50;
                    dgvProspects.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "ارزش معامله%", Name = "vt_change" });
                    dgvProspects.Columns[2].Width = 50;
                    dgvProspects.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "روند ماقبل", Name = "2last_streak" });
                    dgvProspects.Columns[3].Width = 120;
                    dgvProspects.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "روند قبل", Name = "1last_streak" });
                    dgvProspects.Columns[4].Width = 120;
                    dgvProspects.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "روند امروز", Name = "today_streak" });
                    dgvProspects.Columns[5].Width = 60;
                    dgvProspects.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "صف", Name = "queue" });
                    dgvProspects.Columns[6].Width = 70;
                    dgvProspects.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "ملاحظات", Name = "desc" });
                    dgvProspects.Columns[7].Width = 80;
                    dgvProspects.Columns.Add(new ChartColumn() { HeaderText = "نمودار سهم", Name = "instrument_chart" });
                    dgvProspects.Columns[8].Width = Constants.ChartCellWidth;
                    dgvProspects.Columns.Add(new ChartColumn() { HeaderText = "معاملات مردمی", Name = "ind_chart" });
                    dgvProspects.Columns[9].Width = Constants.ChartCellWidth;
                    dgvProspects.Columns.Add(new ChartColumn() { HeaderText = "معاملات شرکتی", Name = "ins_chart" });
                    dgvProspects.Columns[10].Width = Constants.ChartCellWidth;
                    dgvProspects.Columns.Add(new ChartColumn() { HeaderText = "نمودار عرضه/تقاضا", Name = "ask_bid_chart" });
                    dgvProspects.Columns[11].Width = Constants.ChartCellWidth;
                    dgvProspects.Columns.Add(new ChartColumn() { HeaderText = "نمودار عملکرد", Name = "performance_chart" });
                    dgvProspects.Columns[12].Width = Constants.ChartCellWidth;
                    dgvProspects.RowHeadersVisible = false;
                    dgvProspects.VirtualMode = true;
                    this.dgvProspects.CellValueNeeded += new DataGridViewCellValueEventHandler(dgvProspects_CellValueNeeded);
                    this.dgvProspects.RowCount = Entries.Count;


                });
            }
            else if (Entries.First().mode == EntryMode.Categories)
            {
                this.dgvProspects.BeginInvoke((MethodInvoker)delegate ()
                {
                    dgvProspects.Rows.Clear();
                    dgvProspects.Columns.Clear();
                    dgvProspects.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "گروه", Name = "instrument" });
                    dgvProspects.Columns[0].Width = 90;
                    dgvProspects.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "عملکرد", Name = "close_change" });
                    dgvProspects.Columns[1].Width = 60;
                    dgvProspects.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "حجم%", Name = "vol_change" });
                    dgvProspects.Columns[2].Width = 50;
                    dgvProspects.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "ارزش معامله%", Name = "vt_change" });
                    dgvProspects.Columns[3].Width = 50;
                    dgvProspects.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "تعداد معامله%", Name = "nt_change" });
                    dgvProspects.Columns[4].Width = 60;
                    dgvProspects.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "روند ماقبل", Name = "2last_streak" });
                    dgvProspects.Columns[5].Width = 120;
                    dgvProspects.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "روند قبل", Name = "1last_streak" });
                    dgvProspects.Columns[6].Width = 120;
                    dgvProspects.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "روند امروز", Name = "today_streak" });
                    dgvProspects.Columns[7].Width = 120;
                    dgvProspects.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "صف", Name = "queue" });
                    dgvProspects.Columns[8].Width = 120;
                    dgvProspects.Columns.Add(new ChartColumn() { HeaderText = "شاخص گروه", Name = "instrument_chart" });
                    dgvProspects.Columns[9].Width = Constants.ChartCellWidth;
                    dgvProspects.Columns.Add(new ChartColumn() { HeaderText = "نمودار عرضه/تقاضا", Name = "ask_bid_chart" });
                    dgvProspects.Columns[10].Width = Constants.ChartCellWidth;
                    dgvProspects.Columns.Add(new ChartColumn() { HeaderText = "معاملات مردمی", Name = "ind_chart" });
                    dgvProspects.Columns[11].Width = Constants.ChartCellWidth;
                    dgvProspects.Columns.Add(new ChartColumn() { HeaderText = "معاملات شرکتی", Name = "ins_chart" });
                    dgvProspects.Columns[12].Width = Constants.ChartCellWidth;
                    dgvProspects.RowHeadersVisible = false;
                    dgvProspects.VirtualMode = true;
                    this.dgvProspects.CellValueNeeded += new DataGridViewCellValueEventHandler(dgvProspects_CellValueNeeded);
                    this.dgvProspects.RowCount = Entries.Count;


                });

                this.BeginInvoke((MethodInvoker)delegate () {
                    Refresh();
                });
            }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
        }

        private void CheckDatabaseForNewReports()
        {

            DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 00, 00, 0);
            online_data = DataLoader.LoadOnlineData(dt,LoadingSetting);
            int counter = 0;
            DateTime latestdailydate = market.MarketIndex.Ticks.Last().Date;
            foreach (Stock stk in CompareBundle)
            {

                try
                {
                    //DataRow row = dt.NewRow();

                    StepBundle stk_onine_data = online_data.Find(x => x.tsetid == stk.TSETID);

                    Step stp = stk_onine_data.Steps.Last();
                    double buy_sum = Convert.ToDouble(stp.b1volume) + Convert.ToDouble(stp.b2volume) + Convert.ToDouble(stp.b3volume);
                    double sell_sum = Convert.ToDouble(stp.s1volume) + Convert.ToDouble(stp.s2volume) + Convert.ToDouble(stp.s3volume);
                    double queue = buy_sum - sell_sum;

                    Step frststp = stk_onine_data.Steps[0];

                    Step lststp = stk_onine_data.Steps[stk_onine_data.Steps.IndexOf(stp) - 1];
                    double last_buy_sum = Convert.ToDouble(lststp.b1volume) + Convert.ToDouble(lststp.b2volume) + Convert.ToDouble(lststp.b3volume);
                    double last_sell_sum = Convert.ToDouble(lststp.s1volume) + Convert.ToDouble(lststp.s2volume) + Convert.ToDouble(lststp.s3volume);
                    double last_queue = last_buy_sum - last_sell_sum;

                    double queue_change = GetPerformance(last_queue, queue, 4);


                    if (stk_onine_data.Steps.Count() > 0)
                    {
                        counter++;

                        Entry ent = new Entry();
                        ent.name = stk.Name + Environment.NewLine + new DateTime(Convert.ToInt64(stp.tick)).ToString("HH:mm") + Environment.NewLine + new DateTime(Convert.ToInt64(stp.tick)).ToString("MMM-dd") + Environment.NewLine + stk.ParentCategory.Name;
                        ent.vol_change = GetPerformance(stk.Volume.GetValueAt(latestdailydate), Convert.ToDouble(stp.vol));
                        ent.vt_change = GetPerformance(stk.ValueOfTrades.GetValueAt(latestdailydate), Convert.ToDouble(stp.value_of_trades));
                        //ent.snd_last_streak = GetSecondLastStreak(stk.Close, 50);
                        ent.stock = stk;
                        ent.index_chart = GetChart(stk.Close);
                        Entries.Add(ent);

                        //this.dgvProspects.BeginInvoke((MethodInvoker)delegate ()
                        //{

                        //    DataGridViewRow dgvr = new DataGridViewRow();
                        //    dgvr.Height = 100;
                        //    dgvProspects.Rows.Add(dgvr);


                        //    //[0]
                        //    DataGridViewTextBoxCell c1 = new DataGridViewTextBoxCell();
                        //    c1.Value = stk.Name + Environment.NewLine + new DateTime(Convert.ToInt64(stp.tick)).ToString("HH:mm") + Environment.NewLine + new DateTime(Convert.ToInt64(stp.tick)).ToString("MMM-dd") + Environment.NewLine + stk.ParentCategory.Name;
                        //    c1.Style.WrapMode = DataGridViewTriState.True;
                        //    c1.Style.Font = new Font("Tahoma", 9F, GraphicsUnit.Pixel);
                        //    dgvr.Cells[0] = c1;

                        //    //[1]
                        //    c1 = new DataGridViewTextBoxCell();
                        //    double volprf = GetPerformance(stk.Volume.GetValueAt(latestdailydate), Convert.ToDouble(stp.vol));

                        //    if (volprf > 0)
                        //        c1.Style = new DataGridViewCellStyle { ForeColor = Color.ForestGreen };
                        //    else
                        //        c1.Style = new DataGridViewCellStyle { ForeColor = Color.Red };
                        //    //c1.Value = string.Format("{0:n0}", volprf);
                        //    c1.Value = volprf;
                        //    c1.Style.Font = new Font("Tahoma", 12F, GraphicsUnit.Pixel);
                        //    dgvr.Cells[1] = c1;

                        //    //[2]
                        //    c1 = new DataGridViewTextBoxCell();

                        //    volprf = GetPerformance(stk.ValueOfTrades.GetValueAt(latestdailydate), Convert.ToDouble(stp.value_of_trades));
                        //    c1.Value = stp.lateset_trade_price;
                        //    if (volprf > 0)
                        //        c1.Style = new DataGridViewCellStyle { ForeColor = Color.ForestGreen };
                        //    else
                        //        c1.Style = new DataGridViewCellStyle { ForeColor = Color.Red };
                        //    //c1.Value = string.Format("{0:n0}", volprf);
                        //    c1.Style.Font = new Font("Tahoma", 12F, GraphicsUnit.Pixel);
                        //    dgvr.Cells[2] = c1;



                        //    c1 = new DataGridViewTextBoxCell();
                        //    //double diff = 0;
                        //    //int num = 0;
                        //    //int cnt = 0;
                        //    //if (stk.Close.List.Count() > 100)
                        //    //{
                        //    //    cnt = 100;
                        //    //}
                        //    //else
                        //    //{
                        //    //    cnt = stk.Close.List.Count();
                        //    //}
                        //    //List<double> result = market.GetSubListD(stk.Close, dt, cnt);
                        //    //if (result[cnt-1] > 0)
                        //    //{
                        //    //    while (result[cnt-1] > 0 && cnt-1 > 0)
                        //    //    {
                        //    //        num++;
                        //    //        cnt--;
                        //    //    }
                        //    //    c1.Value = num;
                        //    //    c1.Style = new DataGridViewCellStyle { ForeColor = Color.ForestGreen };
                        //    //}
                        //    //else
                        //    //{
                        //    //    while (result[cnt-1] <=0 && cnt-1 > 0)
                        //    //    {
                        //    //        num++;
                        //    //        cnt--;
                        //    //    }

                        //    //    c1.Value = num;
                        //    //    c1.Style = new DataGridViewCellStyle { ForeColor = Color.Red };
                        //    //}
                        //    //diff = GetPerformance(stk.Close.List[stk.Close.List.Count() - num -1], stk.Close.List[stk.Close.List.Count()-1], 2);
                        //    c1.Style.Font = new Font("Tahoma", 12F, GraphicsUnit.Pixel);

                        //    dgvr.Cells[3] = GetSecondLastStreak(stk.Close, 50);

                        //    DataGridViewTextBoxCell c2 = new DataGridViewTextBoxCell();
                        //    double ww = Convert.ToDouble(GetLastStreak(stk.Close, 50).Value);
                        //    c2 = GetLastStreak(stk.Close, 50);
                        //    dgvr.Cells[4] = c2;


                        //    c1 = new DataGridViewTextBoxCell();
                        //    double prf = 0;
                        //    //if (stk.BaseVolume == 1 || stk.BaseVolume <= Convert.ToDouble(stp.vol))
                        //    prf = GetPerformance(Convert.ToDouble(stp.yesterday_price), Convert.ToDouble(stp.close), 2);
                        //    //else
                        //    // prf = GetPerformance(Convert.ToDouble(stp.yesterday_price), Convert.ToDouble(stp.close), 2)*  Convert.ToDouble(stp.vol) / stk.BaseVolume;
                        //    c1.Value = prf;
                        //    if (prf > 0)
                        //        c1.Style = new DataGridViewCellStyle { ForeColor = Color.ForestGreen };
                        //    else
                        //        c1.Style = new DataGridViewCellStyle { ForeColor = Color.Red };
                        //    c1.Style.Font = new Font("Tahoma", 12F, GraphicsUnit.Pixel);
                        //    dgvr.Cells[5] = c1;


                        //    c1 = new DataGridViewTextBoxCell();
                        //    c1.Value = queue;//.ToString();
                        //    if (buy_sum / sell_sum >= 2)
                        //        c1.Style = new DataGridViewCellStyle { ForeColor = Color.ForestGreen };
                        //    else if (sell_sum / buy_sum >= 2)
                        //        c1.Style = new DataGridViewCellStyle { ForeColor = Color.Red };
                        //    //c1.Value = string.Format("{0:n0}", queue);
                        //    c1.Style.Font = new Font("Tahoma", 12F, GraphicsUnit.Pixel);
                        //    dgvr.Cells[6] = c1;





                        //    c1 = new DataGridViewTextBoxCell();
                        //    string bb = "";

                        //    if (stk.BaseVolume == 1)
                        //        bb += "No BaseVol" + Environment.NewLine;
                        //    else if (Convert.ToDouble(stp.vol) > stk.BaseVolume)
                        //        bb += "Vol > BaseVol" + Environment.NewLine;
                        //    else if (Convert.ToDouble(stp.vol) < stk.BaseVolume)
                        //        bb += "Vol < BaseVol" + Environment.NewLine;

                        //    if (Convert.ToDouble(stp.vol) > stk.Volume.Average)
                        //        bb += "Vol > Avg" + Environment.NewLine;

                        //    if (Convert.ToDouble(stp.value_of_trades) > stk.ValueOfTrades.Average)
                        //        bb += "VT > Avg" + Environment.NewLine;

                        //    if (Convert.ToDouble(stp.number_of_trades) > stk.NumberOfTrades.Average)
                        //        bb += "NT > Avg" + Environment.NewLine;

                        //    if (Convert.ToDouble(stp.vol) >= (stk.Volume.RecordHigh * 4 / 5))
                        //        bb += "Vol > 4/5Max" + Environment.NewLine;
                        //    else if (Convert.ToDouble(stp.vol) >= (stk.Volume.RecordHigh * 3 / 5))
                        //        bb += "Vol > 3/5Max" + Environment.NewLine;
                        //    else if (Convert.ToDouble(stp.vol) >= (stk.Volume.RecordHigh * 2 / 5))
                        //        bb += "Vol > 2/5Max" + Environment.NewLine;
                        //    else if (Convert.ToDouble(stp.vol) >= (stk.Volume.RecordHigh / 5))
                        //        bb += "Vol > 1/5Max" + Environment.NewLine;

                        //    if (Convert.ToDouble(stp.value_of_trades) >= (stk.ValueOfTrades.RecordHigh * 4 / 5))
                        //        bb += "VT > 4/5Max" + Environment.NewLine;
                        //    else if (Convert.ToDouble(stp.value_of_trades) >= (stk.ValueOfTrades.RecordHigh * 3 / 5))
                        //        bb += "VT > 3/5Max" + Environment.NewLine;
                        //    else if (Convert.ToDouble(stp.value_of_trades) >= (stk.ValueOfTrades.RecordHigh * 2 / 5))
                        //        bb += "VT > 2/5Max" + Environment.NewLine;
                        //    else if (Convert.ToDouble(stp.value_of_trades) >= (stk.ValueOfTrades.RecordHigh / 5))
                        //        bb += "VT > 1/5Max" + Environment.NewLine;

                        //    if (Convert.ToDouble(stp.number_of_trades) >= (stk.NumberOfTrades.RecordHigh * 4 / 5))
                        //        bb += "NT > 4/5Max" + Environment.NewLine;
                        //    else if (Convert.ToDouble(stp.number_of_trades) >= (stk.NumberOfTrades.RecordHigh * 3 / 5))
                        //        bb += "NT > 3/5Max" + Environment.NewLine;
                        //    else if (Convert.ToDouble(stp.number_of_trades) >= (stk.NumberOfTrades.RecordHigh * 2 / 5))
                        //        bb += "NT > 2/5Max" + Environment.NewLine;
                        //    else if (Convert.ToDouble(stp.number_of_trades) >= (stk.NumberOfTrades.RecordHigh / 5))
                        //        bb += "NT > 1/5Max" + Environment.NewLine;

                        //    c1.Value = bb;
                        //    c1.Style.WrapMode = DataGridViewTriState.True;
                        //    c1.Style.Font = new Font("Tahoma", 9F, GraphicsUnit.Pixel);
                        //    dgvr.Cells[7] = c1;


                        //    ChartCell d1 = new ChartCell();
                        //    d1.Value = GetChart(stk.Close);
                        //    dgvr.Cells[8] = d1;

                        //    d1 = new ChartCell();
                        //    d1.Value = GetDoubleChartBar(stk_onine_data.Steps, OnlineDataCategories.Individual_buy, OnlineDataCategories.Individual_sell);
                        //    dgvr.Cells[9] = d1;


                        //    d1 = new ChartCell();
                        //    d1.Value = GetDoubleChartBar(stk_onine_data.Steps, OnlineDataCategories.Institution_buy, OnlineDataCategories.Institution_sell);
                        //    dgvr.Cells[10] = d1;

                        //    d1 = new ChartCell();
                        //    d1.Value = GetDoubleChart(stk_onine_data.Steps, OnlineDataCategories.buy_queue, OnlineDataCategories.sell_queue);// GetChart(stk_onine_data.Steps, OnlineDataCategories.queue);
                        //    dgvr.Cells[11] = d1;

                        //    d1 = new ChartCell();
                        //    d1.Value = GetChart(stk_onine_data.Steps, OnlineDataCategories.performance);// GetChart(stk_onine_data.Steps, OnlineDataCategories.queue);
                        //    dgvr.Cells[12] = d1;


                        //    c1 = new DataGridViewTextBoxCell();
                        //    c1.Value = prf * ww;
                        //    dgvr.Cells[13] = c1;


                        //    c1 = new DataGridViewTextBoxCell();
                        //    c1.Value = queue_change;
                        //    dgvr.Cells[14] = c1;

                        //    c1 = new DataGridViewTextBoxCell();
                        //    c1.Value = GetPerformance(Convert.ToDouble(frststp.close), Convert.ToDouble(stp.close), 2);
                        //    dgvr.Cells[15] = c1;


                        //});

                    }
                }
                catch (Exception ex)
                {
                    //Console.WriteLine("Sikim " + ex.Message);
                }
            }

            this.dgvProspects.BeginInvoke((MethodInvoker)delegate ()
            {
                dgvProspects.Rows.Clear();
                dgvProspects.Columns.Clear();
                dgvProspects.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "سهام", Name = "instrument_detailed" });
                dgvProspects.Columns[0].Width = 45;
                dgvProspects.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "حجم%", Name = "vol_change" });
                dgvProspects.Columns[1].Width = 50;
                dgvProspects.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "ارزش معامله%", Name = "vt_change" });
                dgvProspects.Columns[2].Width = 50;
                dgvProspects.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "روند ماقبل", Name = "2last_streak" });
                dgvProspects.Columns[3].Width = 70;
                dgvProspects.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "روند قبل", Name = "1last_streak" });
                dgvProspects.Columns[4].Width = 70;
                dgvProspects.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "روند امروز", Name = "today_streak" });
                dgvProspects.Columns[5].Width = 70;
                dgvProspects.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "صف", Name = "queue" });
                dgvProspects.Columns[6].Width = 70;
                dgvProspects.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "ملاحظات", Name = "desc" });
                dgvProspects.Columns[7].Width = 80;
                dgvProspects.Columns.Add(new ChartColumn() { HeaderText = "نمودار سهم", Name = "instrument_chart" });
                dgvProspects.Columns[8].Width = Constants.ChartCellWidth;
                dgvProspects.Columns.Add(new ChartColumn() { HeaderText = "معاملات مردمی", Name = "ind_chart" });
                dgvProspects.Columns[9].Width = Constants.ChartCellWidth;
                dgvProspects.Columns.Add(new ChartColumn() { HeaderText = "معاملات شرکتی", Name = "ins_chart" });
                dgvProspects.Columns[10].Width = Constants.ChartCellWidth;
                dgvProspects.Columns.Add(new ChartColumn() { HeaderText = "نمودار عرضه/تقاضا", Name = "ask_bid_chart" });
                dgvProspects.Columns[11].Width = Constants.ChartCellWidth;
                dgvProspects.Columns.Add(new ChartColumn() { HeaderText = "نمودار عملکرد", Name = "perf_chart" });
                dgvProspects.Columns[12].Width = Constants.ChartCellWidth;
                dgvProspects.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "حاصلضرب روندقبل در روند امروز" });
                dgvProspects.Columns[13].Width = 60;
                dgvProspects.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "تغییر صف" });
                dgvProspects.Columns[13].Width = 60;
                dgvProspects.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "تغییر عملکرد" });
                dgvProspects.Columns[14].Width = 60;
                dgvProspects.RowHeadersVisible = false;
                dgvProspects.VirtualMode = true;
                this.dgvProspects.CellValueNeeded += new DataGridViewCellValueEventHandler(dgvProspects_CellValueNeeded);
                this.dgvProspects.RowCount = Entries.Count;
                
            });
            //this.dgvProspects.BeginInvoke((MethodInvoker)delegate ()
            //{
            //    if (ckbxSortOnPotential.Checked)
            //        dgvProspects.Sort(this.dgvProspects.Columns[13], ListSortDirection.Ascending);
            //    else if (ckbxSortOnQueueGrowth.Checked)
            //        dgvProspects.Sort(this.dgvProspects.Columns[14], ListSortDirection.Descending);
            //});
            //this.dgvProspects.RowCount = 4;
            online_data = null;
        }

        private void dgvProspects_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            try
            {
                // If this is the row for new records, no values are needed.
                //DataGridViewCell cell = (DataGridViewCell)sender;
                //if (e.RowIndex == this.dgvProspects.RowCount - 1) return;

                Entry customerTmp = null;


                customerTmp = (Entry)this.Entries[e.RowIndex];

                if (e.RowIndex % 2 == 0)
                    dgvProspects[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.LightGray;

                // Set the cell value to paint using the Customer object retrieved.
                switch (this.dgvProspects.Columns[e.ColumnIndex].Name)
                {
                    case "instrument":
                        {
                            e.Value = customerTmp.stock.Name;
                            dgvProspects[e.ColumnIndex, e.RowIndex].Style.WrapMode = DataGridViewTriState.True;
                            dgvProspects[e.ColumnIndex, e.RowIndex].Style.Font = new Font("Tahoma", 12F, GraphicsUnit.Pixel);
                            break;
                        }
                    case "instrument_detailed":
                        {
                            e.Value = customerTmp.name;
                            dgvProspects[e.ColumnIndex, e.RowIndex].Style.WrapMode = DataGridViewTriState.True;
                            dgvProspects[e.ColumnIndex, e.RowIndex].Style.Font = new Font("Tahoma", 12F, GraphicsUnit.Pixel);
                            break;
                        }
                    case "close_change":
                        {
                            e.Value = customerTmp.close_change;
                            if (customerTmp.close_change > 0)
                                dgvProspects[e.ColumnIndex, e.RowIndex].Style = new DataGridViewCellStyle { ForeColor = Color.ForestGreen };
                            else
                                dgvProspects[e.ColumnIndex, e.RowIndex].Style = new DataGridViewCellStyle { ForeColor = Color.Crimson };
                            break;
                        }
                    case "vol_change":
                        {
                            e.Value = customerTmp.vol_change;
                            if (customerTmp.vol_change > 0)
                                dgvProspects[e.ColumnIndex, e.RowIndex].Style = new DataGridViewCellStyle { ForeColor = Color.ForestGreen };
                            else
                                dgvProspects[e.ColumnIndex, e.RowIndex].Style = new DataGridViewCellStyle { ForeColor = Color.Crimson };
                            break;
                        }
                    case "vt_change":
                        {
                            e.Value = customerTmp.vt_change;
                            if (customerTmp.close_change > 0)
                                dgvProspects[e.ColumnIndex, e.RowIndex].Style = new DataGridViewCellStyle { ForeColor = Color.ForestGreen };
                            else
                                dgvProspects[e.ColumnIndex, e.RowIndex].Style = new DataGridViewCellStyle { ForeColor = Color.Crimson };
                            break;
                        }
                    case "nt_change":
                        {
                            e.Value = customerTmp.nt_change;
                            if (customerTmp.close_change > 0)
                                dgvProspects[e.ColumnIndex, e.RowIndex].Style = new DataGridViewCellStyle { ForeColor = Color.ForestGreen };
                            else
                                dgvProspects[e.ColumnIndex, e.RowIndex].Style = new DataGridViewCellStyle { ForeColor = Color.Crimson };
                            break;
                        }
                    case "queue":
                        e.Value = FormatDouble(customerTmp.queue);
                        if (customerTmp.queue > 0)
                            dgvProspects[e.ColumnIndex, e.RowIndex].Style = new DataGridViewCellStyle { ForeColor = Color.ForestGreen };
                        else
                            dgvProspects[e.ColumnIndex, e.RowIndex].Style = new DataGridViewCellStyle { ForeColor = Color.Crimson };
                        break;
                    case "desc":
                        e.Value = customerTmp.desc;
                        dgvProspects[e.ColumnIndex, e.RowIndex].Style.WrapMode = DataGridViewTriState.True;
                        dgvProspects[e.ColumnIndex, e.RowIndex].Style.Font = new Font("Tahoma", 9F, GraphicsUnit.Pixel);
                        break;
                    case "instrument_chart":
                        e.Value = customerTmp.index_chart;
                        break;
                    case "ind_chart":
                        e.Value = customerTmp.ind_chart;
                        break;
                    case "ins_chart":
                        e.Value = customerTmp.ins_chart;
                        break;
                    case "ask_bid_chart":
                        e.Value = customerTmp.ask_bid_chart;
                        break;
                    case "green_prc_chart":
                        if (customerTmp.green_chart != null)
                            e.Value = customerTmp.green_chart;
                        break;
                    case "performance_chart":
                        if (customerTmp.performance_chart != null)
                            e.Value = customerTmp.performance_chart;
                        break;
                    case "2last_streak":
                        {
                            e.Value = customerTmp.snd_last_streak_friendly;
                            if (customerTmp.snd_last_streak_sum > 0)
                                dgvProspects[e.ColumnIndex, e.RowIndex].Style = new DataGridViewCellStyle { ForeColor = Color.ForestGreen };
                            else
                                dgvProspects[e.ColumnIndex, e.RowIndex].Style = new DataGridViewCellStyle { ForeColor = Color.Crimson };
                            break;
                        }
                    case "1last_streak":
                        {
                            e.Value = customerTmp.last_streak_friendly;
                            if (customerTmp.last_streak_sum > 0)
                                dgvProspects[e.ColumnIndex, e.RowIndex].Style = new DataGridViewCellStyle { ForeColor = Color.ForestGreen };
                            else
                                dgvProspects[e.ColumnIndex, e.RowIndex].Style = new DataGridViewCellStyle { ForeColor = Color.Crimson };
                            break;
                        }
                    case "today_streak":
                        {
                            e.Value = customerTmp.today_streak;
                            if (customerTmp.today_streak > 0)
                                dgvProspects[e.ColumnIndex, e.RowIndex].Style = new DataGridViewCellStyle { ForeColor = Color.ForestGreen };
                            else
                                dgvProspects[e.ColumnIndex, e.RowIndex].Style = new DataGridViewCellStyle { ForeColor = Color.Crimson };
                            break;
                        }
                    case "final_prcnt_1dayago":
                        {
                            e.Value = customerTmp.final_prcnt_1dayago;
                            if (customerTmp.final_prcnt_1dayago > 0)
                                dgvProspects[e.ColumnIndex, e.RowIndex].Style = new DataGridViewCellStyle { ForeColor = Color.ForestGreen };
                            else
                                dgvProspects[e.ColumnIndex, e.RowIndex].Style = new DataGridViewCellStyle { ForeColor = Color.Crimson };
                            break;
                        }
                    case "final_prcnt_2daysago":
                        {
                            e.Value = customerTmp.final_prcnt_2daysago;
                            if (customerTmp.final_prcnt_2daysago > 0)
                                dgvProspects[e.ColumnIndex, e.RowIndex].Style = new DataGridViewCellStyle { ForeColor = Color.ForestGreen };
                            else
                                dgvProspects[e.ColumnIndex, e.RowIndex].Style = new DataGridViewCellStyle { ForeColor = Color.Crimson };
                            break;
                        }
                    case "final_prcnt_3daysago":
                        {
                            e.Value = customerTmp.final_prcnt_3daysago;
                            if (customerTmp.final_prcnt_3daysago > 0)
                                dgvProspects[e.ColumnIndex, e.RowIndex].Style = new DataGridViewCellStyle { ForeColor = Color.ForestGreen };
                            else
                                dgvProspects[e.ColumnIndex, e.RowIndex].Style = new DataGridViewCellStyle { ForeColor = Color.Crimson };
                            break;
                        }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void OrderizeStocks()
        {
            if (rbStrategy1.Checked)
            {
                Entries = Entries.OrderBy(x => x.last_streak_sum).ToList();
            }
            else if (rbStrategy2.Checked)
            {
                Entries = Entries.OrderBy(x => x.snd_last_streak_sum).ThenBy(y => y.last_streak_sum).ToList();
            }
            else
            {
                Entries = Entries.Where(y => y.stock.ValueOfTrades.List[y.stock.ValueOfTrades.List.Count-1] > 100000000000).OrderByDescending(x => x.final_prcnt_1dayago).ToList();
            }
        }

        private void OrderizeCats()
        {
            if (rbStrategy1.Checked)
            {
                Entries = Entries.OrderByDescending(x => x.queue).ToList();
            }
            else if (rbStrategy2.Checked)
            {
                Entries = Entries.OrderBy(x => x.last_streak_sum).ThenBy(y => y.today_streak).ToList();
            }
        }

        private void CheckDatabaseForNewReports2()
        {
            //try
            //{
            DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 00, 00, 0);
            online_data = DataLoader.LoadOnlineData(dt,LoadingSetting);
            int counter = 0;
            DateTime latestdailydate = market.MarketIndex.Ticks.Last().Date;

            Entries.Clear();
            foreach (Category cat in market.Categories)
            {
                if (cat.HasIndex)
                {
                    if (cat.CategoryIndex.Close.List.Count > 0)
                    {
                        Entry ent = new Entry();
                        ent.stock = cat.CategoryIndex;
                        ent.vol_change = market.GetSubListD(cat.CategoryIndex.Volume, market.MarketIndex.Close.Dates.Last(), 1).Last() * 100;
                        ent.vt_change = market.GetSubListD(cat.CategoryIndex.ValueOfTrades, market.MarketIndex.Close.Dates.Last(), 1).Last() * 100;
                        ent.close_change = market.GetSubListD(cat.CategoryIndex.Close, market.MarketIndex.Close.Dates.Last(), 1).Last() * 100;
                        ent.nt_change = market.GetSubListD(cat.CategoryIndex.NumberOfTrades, market.MarketIndex.Close.Dates.Last(), 1).Last() * 100;

                        ent.index_chart = GetChart(cat.CategoryIndex.Close);


                        string lststrk = GetLastStreak(cat.CategoryIndex.Close, 50,0);
                        ent.last_streak_sum = Convert.ToDouble(lststrk.Split(',')[0]);
                        ent.last_streak_days = Convert.ToInt32(lststrk.Split(',')[1]);
                        ent.last_streak_friendly = ent.last_streak_sum.ToString() + " in " + ent.last_streak_days.ToString() + " days";

                        string scndlststrk = GetSecondLastStreak(cat.CategoryIndex.Close, 50,0);
                        ent.snd_last_streak_sum = Convert.ToDouble(scndlststrk.Split(',')[0]);
                        ent.snd_last_streak_days = Convert.ToInt32(scndlststrk.Split(',')[1]);
                        ent.snd_last_streak_friendly = ent.snd_last_streak_sum.ToString() + " in " + ent.snd_last_streak_days.ToString() + " days";


                        int greencounter = 0;
                        double avg = 0;

                        foreach (Stock stk in cat.Stocks)
                        {

                            StepBundle stk_onine_data = online_data.Find(x => x.tsetid == stk.TSETID);
                            double prf = 0;
                            if (stk_onine_data.Steps.Count > 0)
                            {
                                prf = GetPerformance(Convert.ToDouble(stk_onine_data.Steps.Last().yesterday_price), Convert.ToDouble(stk_onine_data.Steps.Last().close), 2);
                                avg += prf;
                                if (prf > 0)
                                {
                                    greencounter++;

                                }
                            }



                            ent.today_streak = Math.Round((prf / (double)cat.Stocks.Count), 2);


                        }

                        List<double> greens = new List<double>();
                        List<double> perfor = new List<double>();
                        if (cat.Stocks.Count > 3)
                        {
                            for (int i = 0; i <= 400; i++)
                            {
                                int grns = 0;
                                double avgs = 0;
                                foreach (Stock stk in cat.Stocks)
                                {
                                    StepBundle stk_onine_data = online_data.Find(x => x.tsetid == stk.TSETID);
                                    if (stk_onine_data.Steps.Count > i)
                                    {
                                        double prf = GetPerformance(Convert.ToDouble(stk_onine_data.Steps[i].yesterday_price), Convert.ToDouble(stk_onine_data.Steps[i].close), 2);
                                        avgs += prf;
                                        if (prf > 0)
                                        {
                                            grns++;
                                        }
                                    }
                                }
                                greens.Add(Math.Round((grns / (double)cat.Stocks.Count), 2) * 100);
                                perfor.Add(Math.Round((avgs / (double)cat.Stocks.Count), 2) * 100);
                            }
                            ent.green_chart = GetChart(greens);
                            ent.performance_chart = GetChart(perfor);
                        }
                        Entries.Add(ent);
                    }

                }
            }

            this.dgvProspects.BeginInvoke((MethodInvoker)delegate ()
            {
                //Entries = Entries.OrderByDescending(x => x.last_streak_sum).ThenBy(y => y.last_streak_days).ToList();
                Entries = Entries.OrderBy(x => x.last_streak_sum).ThenBy(y => y.today_streak).ToList();

                dgvProspects.Rows.Clear();
                dgvProspects.Columns.Clear();
                dgvProspects.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "گروه", Name = "instrument" });
                dgvProspects.Columns[0].Width = 90;
                dgvProspects.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "عملکرد", Name = "close_change" });
                dgvProspects.Columns[1].Width = 60;
                dgvProspects.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "حجم%", Name = "vol_change" });
                dgvProspects.Columns[2].Width = 60;
                dgvProspects.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "ارزش معامله%", Name = "vt_change" });
                dgvProspects.Columns[3].Width = 60;
                dgvProspects.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "تعداد معامله%", Name = "nt_change" });
                dgvProspects.Columns[4].Width = 60;
                dgvProspects.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "روند قبلی", Name = "2last_streak" });
                dgvProspects.Columns[5].Width = 70;
                dgvProspects.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "آخرین روند", Name = "1last_streak" });
                dgvProspects.Columns[6].Width = 70;
                dgvProspects.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "روند امروز", Name = "today_streak" });
                dgvProspects.Columns[7].Width = 80;
                dgvProspects.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "ملاحظات", Name = "desc" });
                dgvProspects.Columns[8].Width = 80;
                dgvProspects.Columns.Add(new ChartColumn() { HeaderText = "نمودار گروه", Name = "instrument_chart" });
                dgvProspects.Columns[9].Width = Constants.ChartCellWidth;
                dgvProspects.Columns.Add(new ChartColumn() { HeaderText = "نمودار عملکرد", Name = "performance_chart" });
                dgvProspects.Columns[10].Width = Constants.ChartCellWidth;
                dgvProspects.Columns.Add(new ChartColumn() { HeaderText = "درصد سبزها", Name = "green_prc_chart" });
                dgvProspects.Columns[11].Width = Constants.ChartCellWidth;

                dgvProspects.RowHeadersVisible = false;
                dgvProspects.VirtualMode = true;
                this.dgvProspects.CellValueNeeded += new DataGridViewCellValueEventHandler(dgvProspects_CellValueNeeded);
                this.dgvProspects.RowCount = Entries.Count;

            });

            online_data = null;
            //}
            //catch(Exception ex)
            //{
            //    Console.WriteLine(ex.Message.ToString());
            //}
        }

        private void CheckDatabaseForNewReports3()
        {




            DateTime dt = new DateTime(dtpProspects.Value.Year, dtpProspects.Value.Month, dtpProspects.Value.Day, 00, 00, 0);
            online_data = DataLoader.LoadOnlineData(dt,LoadingSetting);
            CatStocksChart.Series.Clear();
            CatStocksChart.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;
            CatStocksChart.ChartAreas[0].AxisY.MajorGrid.LineWidth = 0;
            CatStocksChart.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            CatStocksChart.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;

            CatStocksChart.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            CatStocksChart.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
            CatStocksChart.MouseWheel += new MouseEventHandler(chData_MouseWheel);
            //CatStocksChart.ChartAreas.Add(chartArea);

            foreach (Category cat in market.Categories)
            {
                if (cat.Stocks.Count < 4)
                    continue;


                var series = new Series();

                series.Name = cat.Name;
                series.ChartType = SeriesChartType.Line;
                series.XValueType = ChartValueType.Int32;
                CatStocksChart.Series.Add(series);

                for (int g = 0; g < 300; g++)
                {
                    int greencounter = 0;

                    foreach (Stock stk in cat.Stocks)
                    {
                        StepBundle stk_data = online_data.Find(x => x.tsetid == stk.TSETID);
                        if (stk_data == null)
                            continue;
                        double prf = 0;
                        if (stk_data.Steps.Count > 0 && stk_data.Steps.Count > g)
                            prf = GetPerformance(Convert.ToDouble(stk_data.Steps[g].yesterday_price), Convert.ToDouble(stk_data.Steps[g].close), 2);
                        if (prf > 0)
                            greencounter++;
                    }
                    double res = Math.Round(((double)greencounter / (double)cat.Stocks.Count) * 100, 2);

                    if (res > 50)
                    {

                        DataPoint dp = new DataPoint();
                        dp.XValue = g;
                        dp.YValues = new double[] { res };
                        dp.ToolTip = cat.Name;
                        series.Points.Add(dp);
                    }
                    /// xvals++;
                }

            }
            CatStocksChart.Invalidate();

            tabControl1.SelectedIndex = 4;
            //});



            online_data = null;
        }


        private double GetPerformance(double d1, double d2, int precision = 0)
        {
            double d = ((d2 - d1) / Math.Abs(d1)) * 100;
            if (Double.IsNaN(d) || Double.IsPositiveInfinity(d) || Double.IsNegativeInfinity(d))
                return 0;
            else
                return Math.Round(d, precision);
        }

        private Chart GetDoubleChart(List<Step> steps, OnlineDataCategories cat1, OnlineDataCategories cat2)
        {

            DataBundle db1 = new DataBundle(DataNames.Volume);
            DataBundle db2 = new DataBundle(DataNames.Volume);

            foreach (Step stp in steps)
            {
                db1.Dates.Add(new DateTime(Convert.ToInt64(stp.tick)));
                db2.Dates.Add(new DateTime(Convert.ToInt64(stp.tick)));
                double buy_sum = Convert.ToDouble(stp.b1volume) + Convert.ToDouble(stp.b2volume) + Convert.ToDouble(stp.b3volume);
                double sell_sum = Convert.ToDouble(stp.s1volume) + Convert.ToDouble(stp.s2volume) + Convert.ToDouble(stp.s3volume);

                if (cat1 == OnlineDataCategories.Individual_buy)
                    db1.List.Add(Convert.ToDouble(stp.b_ind_vol));
                else if (cat1 == OnlineDataCategories.Institution_buy)
                    db1.List.Add(Convert.ToDouble(stp.b_ins_vol));
                else if (cat1 == OnlineDataCategories.Individual_sell)
                    db1.List.Add(Convert.ToDouble(stp.s_ind_vol));
                else if (cat1 == OnlineDataCategories.Institution_sell)
                    db1.List.Add(Convert.ToDouble(stp.s_ins_vol));
                else if (cat1 == OnlineDataCategories.buy_queue)
                    db1.List.Add(buy_sum);
                else if (cat1 == OnlineDataCategories.sell_queue)
                    db1.List.Add(sell_sum);
                else if (cat1 == OnlineDataCategories.queue)
                    db1.List.Add(Convert.ToDouble(buy_sum) - Convert.ToDouble(sell_sum));
                else if (cat1 == OnlineDataCategories.performance)
                    db1.List.Add(GetPerformance(Convert.ToDouble(stp.yesterday_price), Convert.ToDouble(stp.lateset_trade_price)));

                if (cat2 == OnlineDataCategories.Individual_buy)
                    db2.List.Add(Convert.ToDouble(stp.b_ind_vol));
                else if (cat2 == OnlineDataCategories.Institution_buy)
                    db2.List.Add(Convert.ToDouble(stp.b_ins_vol));
                else if (cat2 == OnlineDataCategories.Individual_sell)
                    db2.List.Add(Convert.ToDouble(stp.s_ind_vol));
                else if (cat2 == OnlineDataCategories.Institution_sell)
                    db2.List.Add(Convert.ToDouble(stp.s_ins_vol));
                else if (cat2 == OnlineDataCategories.buy_queue)
                    db2.List.Add(buy_sum);
                else if (cat2 == OnlineDataCategories.sell_queue)
                    db2.List.Add(sell_sum);
                else if (cat2 == OnlineDataCategories.queue)
                    db2.List.Add(Convert.ToDouble(buy_sum) - Convert.ToDouble(sell_sum));
                else if (cat2 == OnlineDataCategories.performance)
                    db2.List.Add(GetPerformance(Convert.ToDouble(stp.yesterday_price), Convert.ToDouble(stp.lateset_trade_price)));
            }


            //__________________________________________________________________________________
            Chart zz = new Chart();
            zz.Width = Constants.ChartCellWidth;
            zz.Height = 100;

            var chartArea = new ChartArea();
            chartArea.AxisX.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisY.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisX.LabelStyle.Font = new Font("Consolas", 8);
            chartArea.AxisY.LabelStyle.Font = new Font("Consolas", 8);
            chartArea.AxisY.LabelStyle.Format = "{0:0}";
            chartArea.AxisX.MajorGrid.LineWidth = 1;
            chartArea.AxisY.MajorGrid.LineWidth = 1;
            chartArea.AxisX.LabelStyle.Enabled = false;
            chartArea.AxisY.LabelStyle.Enabled = false;


            zz.ChartAreas.Add(chartArea);

            var series = new Series();
            //series.Name = "شاخص گروه " + stk.Name;
            series.ChartType = SeriesChartType.Line;
            series.XValueType = ChartValueType.Int32;
            series.Color = Color.ForestGreen;
            zz.Series.Add(series);

            int coun = 0;
            foreach (double val in db1.List)
            {
                DataPoint dp = new DataPoint();
                dp.XValue = coun;
                dp.YValues = new double[] { val };
                zz.Series[0].Points.Add(dp);
                coun++;
            }

            var series2 = new Series();
            //series.Name = "شاخص گروه " + stk.Name;
            series2.ChartType = SeriesChartType.Line;
            series2.XValueType = ChartValueType.Int32;
            series2.Color = Color.Crimson;
            zz.Series.Add(series2);

            coun = 0;
            foreach (double val in db2.List)
            {
                DataPoint dp = new DataPoint();
                dp.XValue = coun;
                dp.YValues = new double[] { val };
                zz.Series[1].Points.Add(dp);
                coun++;
            }


            zz.Invalidate();

            return zz;
        }

        private Chart GetDoubleChartCat(Category cat, OnlineDataCategories cat1, OnlineDataCategories cat2)
        {

            DataBundle db1 = new DataBundle(DataNames.Volume);
            DataBundle db2 = new DataBundle(DataNames.Volume);
            double b_ind_vol__sum = 0;
            double b_ins_vol__sum = 0;
            double s_ind_vol__sum = 0;
            double s_ins_vol__sum = 0;

            double buy_sum__sum = 0;
            double sell_sum__sum = 0;

            int zw = 0;
            foreach (Stock stk in cat.Stocks)
            {
                StepBundle stk_onine_data = online_data.Find(x => x.tsetid == stk.TSETID);
                if (stk_onine_data.Steps.Count() > zw)
                    zw = stk_onine_data.Steps.Count();
            }

            try
            {
                for (int w = 0; w < zw; w++)
                {
                    db1.Dates.Add(new DateTime(w));
                    db2.Dates.Add(new DateTime(w));

                    foreach (Stock stk in cat.Stocks)
                    {
                        StepBundle stk_onine_data = online_data.Find(x => x.tsetid == stk.TSETID);
                        b_ind_vol__sum = 0;
                        b_ins_vol__sum = 0;
                        s_ind_vol__sum = 0;
                        s_ins_vol__sum = 0;

                        buy_sum__sum = 0;
                        sell_sum__sum = 0;


                        if (stk_onine_data.Steps.Count()-1 >= w)
                        {
                            double buy_sum = Convert.ToDouble(stk_onine_data.Steps[w].b1volume) + Convert.ToDouble(stk_onine_data.Steps[w].b2volume) + Convert.ToDouble(stk_onine_data.Steps[w].b3volume);
                            double sell_sum = Convert.ToDouble(stk_onine_data.Steps[w].s1volume) + Convert.ToDouble(stk_onine_data.Steps[w].s2volume) + Convert.ToDouble(stk_onine_data.Steps[w].s3volume);

                            b_ind_vol__sum += Convert.ToDouble(stk_onine_data.Steps[w].b_ind_vol);
                            b_ins_vol__sum += Convert.ToDouble(stk_onine_data.Steps[w].b_ins_vol);
                            s_ind_vol__sum += Convert.ToDouble(stk_onine_data.Steps[w].s_ind_vol);
                            s_ins_vol__sum += Convert.ToDouble(stk_onine_data.Steps[w].s_ins_vol);

                            buy_sum__sum += buy_sum;
                            sell_sum__sum += sell_sum;
                        }
                    }

                    if (cat1 == OnlineDataCategories.Individual_buy)
                        db1.List.Add(b_ind_vol__sum);
                    else if (cat1 == OnlineDataCategories.Institution_buy)
                        db1.List.Add(b_ins_vol__sum);
                    else if (cat1 == OnlineDataCategories.Individual_sell)
                        db1.List.Add(s_ind_vol__sum);
                    else if (cat1 == OnlineDataCategories.Institution_sell)
                        db1.List.Add(s_ins_vol__sum);
                    else if (cat1 == OnlineDataCategories.buy_queue)
                        db1.List.Add(buy_sum__sum);
                    else if (cat1 == OnlineDataCategories.sell_queue)
                        db1.List.Add(sell_sum__sum);
                    else if (cat1 == OnlineDataCategories.queue)
                        db1.List.Add(Convert.ToDouble(buy_sum__sum) - Convert.ToDouble(sell_sum__sum));

                    if (cat2 == OnlineDataCategories.Individual_buy)
                        db2.List.Add(b_ind_vol__sum);
                    else if (cat2 == OnlineDataCategories.Institution_buy)
                        db2.List.Add(b_ins_vol__sum);
                    else if (cat2 == OnlineDataCategories.Individual_sell)
                        db2.List.Add(s_ind_vol__sum);
                    else if (cat2 == OnlineDataCategories.Institution_sell)
                        db2.List.Add(s_ins_vol__sum);
                    else if (cat2 == OnlineDataCategories.buy_queue)
                        db2.List.Add(buy_sum__sum);
                    else if (cat2 == OnlineDataCategories.sell_queue)
                        db2.List.Add(sell_sum__sum);
                    else if (cat2 == OnlineDataCategories.queue)
                        db2.List.Add(Convert.ToDouble(buy_sum__sum) - Convert.ToDouble(sell_sum__sum));
                    
                }

                //__________________________________________________________________________________
                Chart zz = new Chart();
                zz.Width = Constants.ChartCellWidth;
                zz.Height = 100;

                var chartArea = new ChartArea();
                chartArea.AxisX.MajorGrid.LineColor = Color.LightGray;
                chartArea.AxisY.MajorGrid.LineColor = Color.LightGray;
                chartArea.AxisX.LabelStyle.Font = new Font("Consolas", 8);
                chartArea.AxisY.LabelStyle.Font = new Font("Consolas", 8);
                chartArea.AxisY.LabelStyle.Format = "{0:0}";
                chartArea.AxisX.MajorGrid.LineWidth = 1;
                chartArea.AxisY.MajorGrid.LineWidth = 1;
                chartArea.AxisX.LabelStyle.Enabled = false;
                chartArea.AxisY.LabelStyle.Enabled = false;


                zz.ChartAreas.Add(chartArea);

                var series = new Series();
                //series.Name = "شاخص گروه " + stk.Name;
                series.ChartType = SeriesChartType.Line;
                series.XValueType = ChartValueType.Int32;
                series.Color = Color.ForestGreen;
                zz.Series.Add(series);

                int coun = 0;
                foreach (double val in db1.List)
                {
                    DataPoint dp = new DataPoint();
                    dp.XValue = coun;
                    dp.YValues = new double[] { val };
                    zz.Series[0].Points.Add(dp);
                    coun++;
                }

                var series2 = new Series();
                //series.Name = "شاخص گروه " + stk.Name;
                series2.ChartType = SeriesChartType.Line;
                series2.XValueType = ChartValueType.Int32;
                series2.Color = Color.Crimson;
                zz.Series.Add(series2);

                coun = 0;
                foreach (double val in db2.List)
                {
                    DataPoint dp = new DataPoint();
                    dp.XValue = coun;
                    dp.YValues = new double[] { val };
                    zz.Series[1].Points.Add(dp);
                    coun++;
                }


                zz.Invalidate();

                return zz;
            }
            catch(Exception ex)
            {
                return new Chart();
            }
        }

        private Chart GetDoubleChartCatBar(Category cat, OnlineDataCategories cat1, OnlineDataCategories cat2)
        {

            DataBundle db1 = new DataBundle(DataNames.Volume);
            DataBundle db2 = new DataBundle(DataNames.Volume);
            double b_ind_vol__sum = 0;
            double b_ins_vol__sum = 0;
            double s_ind_vol__sum = 0;
            double s_ins_vol__sum = 0;

            double buy_sum__sum = 0;
            double sell_sum__sum = 0;

            double b_ind_vol__last_step1 = 0;
            double b_ins_vol__last_step1 = 0;
            double s_ind_vol__last_step1 = 0;
            double s_ins_vol__last_step1 = 0;

            double b_ind_vol__last_step2 = 0;
            double b_ins_vol__last_step2 = 0;
            double s_ind_vol__last_step2 = 0;
            double s_ins_vol__last_step2 = 0;


            try
            {
                for (int w = 0; w < 50; w++)
                {
                    db1.Dates.Add(new DateTime(w));
                    db2.Dates.Add(new DateTime(w));

                    foreach (Stock stk in cat.Stocks)
                    {
                        StepBundle stk_onine_data = online_data.Find(x => x.tsetid == stk.TSETID);
                        b_ind_vol__sum = 0;
                        b_ins_vol__sum = 0;
                        s_ind_vol__sum = 0;
                        s_ins_vol__sum = 0;

                        buy_sum__sum = 0;
                        sell_sum__sum = 0;


                        if (stk_onine_data.Steps.Count() - 1 >= w)
                        {
                            double buy_sum = Convert.ToDouble(stk_onine_data.Steps[w].b1volume) + Convert.ToDouble(stk_onine_data.Steps[w].b2volume) + Convert.ToDouble(stk_onine_data.Steps[w].b3volume);
                            double sell_sum = Convert.ToDouble(stk_onine_data.Steps[w].s1volume) + Convert.ToDouble(stk_onine_data.Steps[w].s2volume) + Convert.ToDouble(stk_onine_data.Steps[w].s3volume);

                            b_ind_vol__sum += Convert.ToDouble(stk_onine_data.Steps[w].b_ind_vol);
                            b_ins_vol__sum += Convert.ToDouble(stk_onine_data.Steps[w].b_ins_vol);
                            s_ind_vol__sum += Convert.ToDouble(stk_onine_data.Steps[w].s_ind_vol);
                            s_ins_vol__sum += Convert.ToDouble(stk_onine_data.Steps[w].s_ins_vol);

                            buy_sum__sum += buy_sum;
                            sell_sum__sum += sell_sum;
                        }
                    }

                    if (cat1 == OnlineDataCategories.Individual_buy)
                    {
                        db1.List.Add(b_ind_vol__sum - b_ind_vol__last_step1);
                        b_ind_vol__last_step1 = b_ind_vol__sum;
                    }
                    else if (cat1 == OnlineDataCategories.Institution_buy)
                    {
                        db1.List.Add(b_ins_vol__sum - b_ins_vol__last_step1);
                        b_ins_vol__last_step1 = b_ins_vol__sum;
                    }
                    else if (cat1 == OnlineDataCategories.Individual_sell)
                    {
                        db1.List.Add(s_ind_vol__sum - s_ind_vol__last_step1);
                        s_ind_vol__last_step1 = s_ind_vol__sum;
                    }
                    else if (cat1 == OnlineDataCategories.Institution_sell)
                    {
                        db1.List.Add(s_ins_vol__sum - s_ins_vol__last_step1);
                        s_ins_vol__last_step1 = s_ins_vol__sum;
                    }
                    else if (cat1 == OnlineDataCategories.buy_queue)
                        db1.List.Add(buy_sum__sum);
                    else if (cat1 == OnlineDataCategories.sell_queue)
                        db1.List.Add(sell_sum__sum);
                    else if (cat1 == OnlineDataCategories.queue)
                        db1.List.Add(Convert.ToDouble(buy_sum__sum) - Convert.ToDouble(sell_sum__sum));

                    if (cat2 == OnlineDataCategories.Individual_buy)
                    {
                        db2.List.Add(b_ind_vol__sum - b_ind_vol__last_step2);
                        b_ind_vol__last_step2 = b_ind_vol__sum;
                    }
                    else if (cat2 == OnlineDataCategories.Institution_buy)
                    {
                        db2.List.Add(b_ins_vol__sum - b_ins_vol__last_step2);
                        b_ins_vol__last_step2 = b_ins_vol__sum;
                    }
                    else if (cat2 == OnlineDataCategories.Individual_sell)
                    {
                        db2.List.Add(s_ind_vol__sum - s_ind_vol__last_step2);
                        s_ind_vol__last_step2 = s_ind_vol__sum;
                    }
                    else if (cat2 == OnlineDataCategories.Institution_sell)
                    {
                        db2.List.Add(s_ins_vol__sum - s_ins_vol__last_step2);
                        s_ins_vol__last_step2 = s_ins_vol__sum;
                    }
                    else if (cat2 == OnlineDataCategories.buy_queue)
                        db2.List.Add(buy_sum__sum);
                    else if (cat2 == OnlineDataCategories.sell_queue)
                        db2.List.Add(sell_sum__sum);
                    else if (cat2 == OnlineDataCategories.queue)
                        db2.List.Add(Convert.ToDouble(buy_sum__sum) - Convert.ToDouble(sell_sum__sum));

                }

                //__________________________________________________________________________________
                Chart zz = new Chart();
                zz.Width = Constants.ChartCellWidth;
                zz.Height = 100;

                var chartArea = new ChartArea();
                chartArea.AxisX.MajorGrid.LineColor = Color.LightGray;
                chartArea.AxisY.MajorGrid.LineColor = Color.LightGray;
                chartArea.AxisX.LabelStyle.Font = new Font("Consolas", 8);
                chartArea.AxisY.LabelStyle.Font = new Font("Consolas", 8);
                chartArea.AxisY.LabelStyle.Format = "{0:0}";
                chartArea.AxisX.MajorGrid.LineWidth = 1;
                chartArea.AxisY.MajorGrid.LineWidth = 1;
                chartArea.AxisX.LabelStyle.Enabled = false;
                chartArea.AxisY.LabelStyle.Enabled = false;


                zz.ChartAreas.Add(chartArea);

                var series = new Series();
                //series.Name = "شاخص گروه " + stk.Name;
                series.ChartType = SeriesChartType.Line;
                series.XValueType = ChartValueType.Int32;
                series.Color = Color.ForestGreen;
                zz.Series.Add(series);

                int coun = 0;
                foreach (double val in db1.List)
                {
                    DataPoint dp = new DataPoint();
                    dp.XValue = coun;
                    dp.YValues = new double[] { val };
                    zz.Series[0].Points.Add(dp);
                    coun++;
                }

                var series2 = new Series();
                //series.Name = "شاخص گروه " + stk.Name;
                series2.ChartType = SeriesChartType.Line;
                series2.XValueType = ChartValueType.Int32;
                series2.Color = Color.Crimson;
                zz.Series.Add(series2);

                coun = 0;
                foreach (double val in db2.List)
                {
                    DataPoint dp = new DataPoint();
                    dp.XValue = coun;
                    dp.YValues = new double[] { val };
                    zz.Series[1].Points.Add(dp);
                    coun++;
                }


                zz.Invalidate();

                return zz;
            }
            catch (Exception ex)
            {
                return new Chart();
            }
        }

        private Chart GetDoubleChartBar(List<Step> steps, OnlineDataCategories cat1, OnlineDataCategories cat2)
        {

            DataBundle db1 = new DataBundle(DataNames.Volume);
            DataBundle db2 = new DataBundle(DataNames.Volume);

            double last_stp1 = 0;
            double last_stp2 = 0;
            foreach (Step stp in steps)
            {
                db1.Dates.Add(new DateTime(Convert.ToInt64(stp.tick)));
                db2.Dates.Add(new DateTime(Convert.ToInt64(stp.tick)));
                double buy_sum = Convert.ToDouble(stp.b1volume) + Convert.ToDouble(stp.b2volume) + Convert.ToDouble(stp.b3volume);
                double sell_sum = Convert.ToDouble(stp.s1volume) + Convert.ToDouble(stp.s2volume) + Convert.ToDouble(stp.s3volume);

                if (cat1 == OnlineDataCategories.Individual_buy)
                {
                    db1.List.Add(Convert.ToDouble(stp.b_ind_vol) - last_stp1);
                    last_stp1 = Convert.ToDouble(stp.b_ind_vol);
                }
                else if (cat1 == OnlineDataCategories.Institution_buy)
                {
                    db1.List.Add(Convert.ToDouble(stp.b_ins_vol) - last_stp1);
                    last_stp1 = Convert.ToDouble(stp.b_ins_vol);

                }
                else if (cat1 == OnlineDataCategories.Individual_sell)
                {
                    db1.List.Add(Convert.ToDouble(stp.s_ind_vol) - last_stp1);
                    last_stp1 = Convert.ToDouble(stp.s_ind_vol);
                }
                else if (cat1 == OnlineDataCategories.Institution_sell)
                {
                    db1.List.Add(Convert.ToDouble(stp.s_ins_vol) - last_stp1);
                    last_stp1 = Convert.ToDouble(stp.s_ins_vol);
                }
                else if (cat1 == OnlineDataCategories.buy_queue)
                    db1.List.Add(buy_sum);
                else if (cat1 == OnlineDataCategories.sell_queue)
                    db1.List.Add(sell_sum);
                else if (cat1 == OnlineDataCategories.queue)
                    db1.List.Add(Convert.ToDouble(buy_sum) - Convert.ToDouble(sell_sum));
                else if (cat1 == OnlineDataCategories.performance)
                    db1.List.Add(GetPerformance(Convert.ToDouble(stp.yesterday_price), Convert.ToDouble(stp.lateset_trade_price)));

                if (cat2 == OnlineDataCategories.Individual_buy)
                {
                    db2.List.Add(Convert.ToDouble(stp.b_ind_vol) - last_stp2);
                    last_stp2 = Convert.ToDouble(stp.b_ind_vol);
                }
                else if (cat2 == OnlineDataCategories.Institution_buy)
                {
                    db2.List.Add(Convert.ToDouble(stp.b_ins_vol) - last_stp2);
                    last_stp2 = Convert.ToDouble(stp.b_ins_vol);
                }
                else if (cat2 == OnlineDataCategories.Individual_sell)
                {
                    db2.List.Add(Convert.ToDouble(stp.s_ind_vol) - last_stp2);
                    last_stp2 = Convert.ToDouble(stp.s_ind_vol);
                }
                else if (cat2 == OnlineDataCategories.Institution_sell)
                {
                    db2.List.Add(Convert.ToDouble(stp.s_ins_vol) - last_stp2);
                    last_stp2 = Convert.ToDouble(stp.s_ins_vol);
                }
                else if (cat2 == OnlineDataCategories.buy_queue)
                    db2.List.Add(buy_sum);
                else if (cat2 == OnlineDataCategories.sell_queue)
                    db2.List.Add(sell_sum);
                else if (cat2 == OnlineDataCategories.queue)
                    db2.List.Add(Convert.ToDouble(buy_sum) - Convert.ToDouble(sell_sum));
                else if (cat2 == OnlineDataCategories.performance)
                    db2.List.Add(GetPerformance(Convert.ToDouble(stp.yesterday_price), Convert.ToDouble(stp.lateset_trade_price)));
            }


            //__________________________________________________________________________________
            Chart zz = new Chart();
            zz.Width = Constants.ChartCellWidth;
            zz.Height = 100;

            var chartArea = new ChartArea();
            chartArea.AxisX.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisY.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisX.LabelStyle.Font = new Font("Consolas", 8);
            chartArea.AxisY.LabelStyle.Font = new Font("Consolas", 8);
            chartArea.AxisY.LabelStyle.Format = "{0:0}";
            chartArea.AxisX.MajorGrid.LineWidth = 1;
            chartArea.AxisY.MajorGrid.LineWidth = 1;
            chartArea.AxisX.LabelStyle.Enabled = false;
            chartArea.AxisY.LabelStyle.Enabled = false;


            zz.ChartAreas.Add(chartArea);

            var series = new Series();
            //series.Name = "شاخص گروه " + stk.Name;
            series.ChartType = SeriesChartType.Line;
            series.XValueType = ChartValueType.Int32;
            series.Color = Color.ForestGreen;
            zz.Series.Add(series);

            int coun = 0;
            foreach (double val in db1.List)
            {
                DataPoint dp = new DataPoint();
                dp.XValue = coun;
                dp.YValues = new double[] { val };
                zz.Series[0].Points.Add(dp);
                coun++;
            }

            var series2 = new Series();
            //series.Name = "شاخص گروه " + stk.Name;
            series2.ChartType = SeriesChartType.Line;
            series2.XValueType = ChartValueType.Int32;
            series2.Color = Color.Crimson;
            zz.Series.Add(series2);

            coun = 0;
            foreach (double val in db2.List)
            {
                DataPoint dp = new DataPoint();
                dp.XValue = coun;
                dp.YValues = new double[] { val };
                zz.Series[1].Points.Add(dp);
                coun++;
            }


            zz.Invalidate();

            return zz;
        }

        private Chart GetChart(List<Step> steps, OnlineDataCategories cat)
        {

            DataBundle db = new DataBundle(DataNames.Volume);

            foreach (Step stp in steps)
            {
                double buy_sum = Convert.ToDouble(stp.b1volume) + Convert.ToDouble(stp.b2volume) + Convert.ToDouble(stp.b3volume);
                double sell_sum = Convert.ToDouble(stp.s1volume) + Convert.ToDouble(stp.s2volume) + Convert.ToDouble(stp.s3volume);

                db.Dates.Add(new DateTime(Convert.ToInt64(stp.tick)));
                if (cat == OnlineDataCategories.Individual_buy)
                    db.List.Add(Convert.ToDouble(stp.b_ind_vol));
                else if (cat == OnlineDataCategories.Institution_buy)
                    db.List.Add(Convert.ToDouble(stp.b_ins_vol));
                else if (cat == OnlineDataCategories.Individual_sell)
                    db.List.Add(Convert.ToDouble(stp.s_ind_vol));
                else if (cat == OnlineDataCategories.Institution_sell)
                    db.List.Add(Convert.ToDouble(stp.s_ins_vol));
                else if (cat == OnlineDataCategories.buy_queue)
                    db.List.Add(buy_sum);
                else if (cat == OnlineDataCategories.sell_queue)
                    db.List.Add(sell_sum);
                else if (cat == OnlineDataCategories.queue)
                    db.List.Add(Convert.ToDouble(buy_sum) - Convert.ToDouble(sell_sum));
                else if (cat == OnlineDataCategories.performance)
                    db.List.Add(GetPerformance(Convert.ToDouble(stp.yesterday_price), Convert.ToDouble(stp.lateset_trade_price)));

            }

            //__________________________________________________________________________________
            Chart zz = new Chart();
            zz.Width = Constants.ChartCellWidth;
            zz.Height = 100;

            var chartArea = new ChartArea();
            chartArea.AxisX.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisY.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisX.LabelStyle.Font = new Font("Consolas", 8);
            chartArea.AxisY.LabelStyle.Font = new Font("Consolas", 8);
            chartArea.AxisY.LabelStyle.Format = "{0:0}";
            chartArea.AxisX.MajorGrid.LineWidth = 1;
            chartArea.AxisY.MajorGrid.LineWidth = 1;
            chartArea.AxisX.LabelStyle.Enabled = false;
            chartArea.AxisY.LabelStyle.Enabled = false;


            zz.Invalidate();
            zz.ChartAreas.Add(chartArea);

            var series = new Series();
            //series.Name = "شاخص گروه " + stk.Name;
            series.ChartType = SeriesChartType.Line;
            series.XValueType = ChartValueType.Int32;
            zz.Series.Add(series);

            int coun = 0;
            foreach (double val in db.List)
            {
                DataPoint dp = new DataPoint();
                dp.XValue = coun;
                if (val >= 0)
                {
                    dp.Color = Color.ForestGreen;
                }
                else
                {
                    dp.Color = Color.Crimson;
                }
                dp.YValues = new double[] { val };
                zz.Series[0].Points.Add(dp);
                coun++;
            }
            return zz;
        }

        private Chart GetChart(DataBundle db)
        {

            //__________________________________________________________________________________
            Chart zz = new Chart();
            zz.Width = Constants.ChartCellWidth;
            zz.Height = 100;

            var chartArea = new ChartArea();
            chartArea.AxisX.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisY.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisX.LabelStyle.Font = new Font("Consolas", 8);
            chartArea.AxisY.LabelStyle.Font = new Font("Consolas", 8);
            chartArea.AxisY.LabelStyle.Format = "{0:0}";
            chartArea.AxisX.MajorGrid.LineWidth = 1;
            chartArea.AxisY.MajorGrid.LineWidth = 1;
            chartArea.AxisX.LabelStyle.Enabled = false;
            chartArea.AxisY.LabelStyle.Enabled = false;

            zz.Invalidate();
            zz.ChartAreas.Add(chartArea);

            var series = new Series();
            series.ChartType = SeriesChartType.Line;
            series.XValueType = ChartValueType.Int32;
            zz.Series.Add(series);

            int coun = 0;
            int num = db.List.Count();
            double last_move = GetPerformance(db.List[num - 2], db.List[num - 1], 4);
            double min = double.MaxValue;
            double max = double.MinValue;
            if (num - 400 > 0)
            {
                foreach (double val in db.List.GetRange(db.List.Count - 400, 400))
                {

                    DataPoint dp = new DataPoint();
                    dp.XValue = coun;
                    if (last_move > 0)
                        dp.Color = Color.ForestGreen;
                    else
                        dp.Color = Color.Red;
                    dp.YValues = new double[] { val };
                    zz.Series[0].Points.Add(dp);
                    coun++;
                    min = Math.Min(min, dp.YValues[0]);
                    max = Math.Max(max, dp.YValues[0]);
                }
            }
            else
            {
                foreach (double val in db.List)
                {

                    DataPoint dp = new DataPoint();
                    dp.XValue = coun;
                    if (last_move > 0)
                        dp.Color = Color.ForestGreen;
                    else
                        dp.Color = Color.Red;
                    dp.YValues = new double[] { val };
                    zz.Series[0].Points.Add(dp);
                    coun++;
                    min = Math.Min(min, dp.YValues[0]);
                    max = Math.Max(max, dp.YValues[0]);
                }
            }
            zz.ChartAreas[0].AxisY.Maximum = max + min / 50;
            zz.ChartAreas[0].AxisY.Minimum = min - min / 50;
            return zz;
        }

        private Chart GetChart(List<double> db)
        {

            //__________________________________________________________________________________
            Chart zz = new Chart();
            zz.Width = Constants.ChartCellWidth;
            zz.Height = 100;

            var chartArea = new ChartArea();
            chartArea.AxisX.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisY.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisX.LabelStyle.Font = new Font("Consolas", 8);
            chartArea.AxisY.LabelStyle.Font = new Font("Consolas", 8);
            chartArea.AxisY.LabelStyle.Format = "{0:0}";
            chartArea.AxisX.MajorGrid.LineWidth = 1;
            chartArea.AxisY.MajorGrid.LineWidth = 1;
            chartArea.AxisX.LabelStyle.Enabled = false;
            chartArea.AxisY.LabelStyle.Enabled = false;

            zz.Invalidate();
            zz.ChartAreas.Add(chartArea);

            var series = new Series();
            series.ChartType = SeriesChartType.Line;
            series.XValueType = ChartValueType.Int32;

            zz.Series.Add(series);

            int coun = 0;
            int num = db.Count();
            double last_move = GetPerformance(db[num - 2], db[num - 1], 4);
            double min = double.MaxValue;
            double max = double.MinValue;
            if (num - 400 > 0)
            {
                foreach (double val in db.GetRange(db.Count - 400, 400))
                {

                    DataPoint dp = new DataPoint();
                    dp.XValue = coun;
                    if (val > 0)
                        dp.Color = Color.ForestGreen;
                    else
                        dp.Color = Color.Red;
                    dp.YValues = new double[] { val };
                    zz.Series[0].Points.Add(dp);
                    coun++;
                    min = Math.Min(min, dp.YValues[0]);
                    max = Math.Max(max, dp.YValues[0]);
                }
            }
            else
            {
                foreach (double val in db)
                {

                    DataPoint dp = new DataPoint();
                    dp.XValue = coun;
                    if (val > 0)
                        dp.Color = Color.ForestGreen;
                    else
                        dp.Color = Color.Red;
                    dp.YValues = new double[] { val };
                    zz.Series[0].Points.Add(dp);
                    coun++;
                    min = Math.Min(min, dp.YValues[0]);
                    max = Math.Max(max, dp.YValues[0]);
                }
            }
            //zz.ChartAreas[0].AxisY.Maximum = max + min / 50;
            //zz.ChartAreas[0].AxisY.Minimum = min - min / 50;
            return zz;
        }

        private void btnStopLiveMonitoring_Click(object sender, EventArgs e)
        {
            btnLiveStat.Enabled = true;
            btnStopLiveMonitoring.Enabled = false;
            if (cancellationTokenSource != null)
                cancellationTokenSource.Cancel();
        }

        private void dgvProspects_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {

        }

        private void dgvProspects_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            //Console.WriteLine(e.Exception.Message);
        }

        private void dgvProspects_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            //if (dgvProspects.Rows[e.RowIndex].Cells[5].GetType() != typeof(Chart))
            //    dgvProspects.Rows[e.RowIndex].ErrorText = "Concisely describe the error and how to fix it";
            //e.Cancel = false;
        }

        private void dgvProspects_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {

            try
            {

                //if (e.ColumnIndex == 1 || e.ColumnIndex == 2 || e.ColumnIndex == 6) // Check for the column you want
                //{

                //    if (Convert.ToDouble(e.Value) < 1000 && Convert.ToDouble(e.Value) > -1000)
                //    {
                //        // no formating
                //        e.CellStyle.Format = "#";
                //    }
                //    else if (Convert.ToDouble(e.Value) < 1000000 && Convert.ToDouble(e.Value) > -1000000)
                //    {
                //        e.CellStyle.Format = "#";
                //        e.Value = (Convert.ToDouble(e.Value) / 1000).ToString("0.##") + "K";
                //        e.FormattingApplied = true;
                //    }
                //    else if (Convert.ToDouble(e.Value) < 1000000000 && Convert.ToDouble(e.Value) > -1000000000)
                //    {
                //        e.CellStyle.Format = "#";
                //        e.Value = (Convert.ToDouble(e.Value) / 1000000).ToString("0.##") + "M";
                //        e.FormattingApplied = true;
                //    }
                //    else if (Convert.ToDouble(e.Value) < 1000000000000 && Convert.ToDouble(e.Value) > -1000000000000)
                //    {
                //        e.CellStyle.Format = "#";
                //        e.Value = (Convert.ToDouble(e.Value) / 1000000000).ToString("0.##") + "B";
                //        e.FormattingApplied = true;
                //    }

                //}
            }
            catch (Exception ex)
            {

            }
        }

        public string FormatDouble(double num)
        {
            if (num < 1000 && num > -1000)
            {
                return (num/10).ToString();
            }
            else if (num < 1000000 && num > -1000000)
            {
                return (num / 10000).ToString("0.##") + "KT";
            }
            else if (num < 1000000000 && num > -1000000000)
            {
                return (num / 10000000).ToString("0.##") + "MT";
            }
            else if (num > -1000000000000)
            {
                return (num / 10000000000).ToString("0.##") + "BT";
            }
            else
                return num.ToString();
        }

        private void btnDrawCatStocks_Click(object sender, EventArgs e)
        {
            CompareBundle.Clear();
            if (cbhCategories.SelectedItem == null)
                return;
            else
                Console.WriteLine(cbhCategories.SelectedItem);

            foreach (Category cat in market.Categories)
            {

                if (cbhCategories.SelectedItem.ToString() == cat.Name)
                {
                    foreach (Stock stk in cat.Stocks)
                    {
                        if (stk.IsActive && stk.IsCompetent)
                            CompareBundle.Add(stk);
                    }
                }
            }



            var chartArea = new ChartArea();
            chartArea.AxisX.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisY.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisX.LabelStyle.Font = new Font("Consolas", 8);
            chartArea.AxisY.LabelStyle.Font = new Font("Consolas", 8);
            chartArea.AxisY.LabelStyle.Format = "{0:0}";
            chartArea.AxisX.MajorGrid.LineWidth = 1;
            chartArea.AxisY.MajorGrid.LineWidth = 1;
            chartArea.AxisX.LabelStyle.Enabled = false;
            chartArea.AxisY.LabelStyle.Enabled = false;



            CatStocksChart.ChartAreas.Clear();
            CatStocksChart.Series.Clear();
            CatStocksChart.ChartAreas.Add(chartArea);

            foreach (Stock stk in CompareBundle)
            {
                var series = new Series();
                series.Name = stk.Name;
                series.ChartType = SeriesChartType.Line;
                series.XValueType = ChartValueType.DateTime;
                CatStocksChart.Series.Add(series);
                int coun = 0;
                double lstval = 1;
                foreach (double val in stk.Close.List)
                {

                    if (stk.Close.Dates[coun] > new DateTime(2016, 9, 1))
                    {
                        DataPoint dp = new DataPoint();
                        dp.XValue = stk.Close.Dates[coun].ToOADate();
                        dp.YValues = new double[] { val };
                        dp.ToolTip = stk.Name;
                        if (val > lstval)
                            dp.Color = Color.ForestGreen;
                        else
                            dp.Color = Color.Red;
                        lstval = val;
                        series.Points.Add(dp);
                    }
                    coun++;
                }
            }

            chartArea.AxisX.ScaleView.Zoomable = true;
            chartArea.AxisY.ScaleView.Zoomable = true;
            chartArea.CursorX.IsUserSelectionEnabled = true;
            chartArea.CursorY.IsUserSelectionEnabled = true;
            CatStocksChart.MouseWheel += new MouseEventHandler(chData_MouseWheel);

            CatStocksChart.Invalidate();


            tabControl1.SelectedIndex = 4;
        }

        private void btnClickerEngage_Click(object sender, EventArgs e)
        {

        }

        private void chkbxCat_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbxCat.Checked)
            {
                AnalyseCats();
            }
            else
            {
                dgvProspects.Columns.Clear();
                dgvProspects.Rows.Clear();
            }

        }

        private void AnalyseCats()
        {
            Entries.Clear();
            foreach (Category cat in market.Categories)
            {
                if (cat.HasIndex)
                {
                    if (cat.CategoryIndex.Close.List.Count > 0)
                    {
                        Entry ent = new Entry();
                        ent.stock = cat.CategoryIndex;
                        ent.vol_change = market.GetSubListD(cat.CategoryIndex.Volume, market.MarketIndex.Close.Dates.Last(), 1).Last() * 100;
                        ent.vt_change = market.GetSubListD(cat.CategoryIndex.ValueOfTrades, market.MarketIndex.Close.Dates.Last(), 1).Last() * 100;
                        ent.close_change = market.GetSubListD(cat.CategoryIndex.Close, market.MarketIndex.Close.Dates.Last(), 1).Last() * 100;
                        ent.nt_change = market.GetSubListD(cat.CategoryIndex.NumberOfTrades, market.MarketIndex.Close.Dates.Last(), 1).Last() * 100;

                        ent.index_chart = GetChart(cat.CategoryIndex.Close);

                        string lststrk = GetLastStreak(cat.CategoryIndex.Close, 50,0);
                        ent.last_streak_sum = Convert.ToDouble(lststrk.Split(',')[0]);
                        ent.last_streak_days = Convert.ToInt32(lststrk.Split(',')[1]);
                        ent.last_streak_friendly = ent.last_streak_sum.ToString() + " in " + ent.last_streak_days.ToString() + " days";

                        string scndlststrk = GetSecondLastStreak(cat.CategoryIndex.Close, 50,0);
                        ent.snd_last_streak_sum = Convert.ToDouble(scndlststrk.Split(',')[0]);
                        ent.snd_last_streak_days = Convert.ToInt32(scndlststrk.Split(',')[1]);
                        ent.snd_last_streak_friendly = ent.snd_last_streak_sum.ToString() + " in " + ent.snd_last_streak_days.ToString() + " days";
                        //ent.snd_last_streak = 
                        Entries.Add(ent);
                    }

                }
            }

            //Entries = Entries.OrderByDescending(x => x.last_streak_sum).ThenBy(y => y.last_streak_days).ToList();
            Entries = Entries.OrderByDescending(x => x.last_streak_sum / x.last_streak_days).ToList();

            dgvProspects.Rows.Clear();
            dgvProspects.Columns.Clear();
            dgvProspects.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "گروه", Name = "instrument" });
            dgvProspects.Columns[0].Width = 90;
            dgvProspects.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "عملکرد", Name = "close_change" });
            dgvProspects.Columns[1].Width = 60;
            dgvProspects.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "حجم%", Name = "vol_change" });
            dgvProspects.Columns[2].Width = 60;
            dgvProspects.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "ارزش معامله%", Name = "vt_change" });
            dgvProspects.Columns[3].Width = 60;
            dgvProspects.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "تعداد معامله%", Name = "nt_change" });
            dgvProspects.Columns[4].Width = 60;
            dgvProspects.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "روند قبلی", Name = "2last_streak" });
            dgvProspects.Columns[5].Width = 70;
            dgvProspects.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "آخرین روند", Name = "1last_streak" });
            dgvProspects.Columns[6].Width = 70;
            dgvProspects.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "ملاحظات", Name = "desc" });
            dgvProspects.Columns[7].Width = 80;
            dgvProspects.Columns.Add(new ChartColumn() { HeaderText = "نمودار سهم", Name = "instrument_chart" });
            dgvProspects.Columns[8].Width = Constants.ChartCellWidth;

            dgvProspects.RowHeadersVisible = false;
            dgvProspects.VirtualMode = true;
            this.dgvProspects.CellValueNeeded += new DataGridViewCellValueEventHandler(dgvProspects_CellValueNeeded);
            this.dgvProspects.RowCount = Entries.Count;
        }

        private string GetLastStreak(DataBundle db, int days, int backtrack)
        {

            try
            {
                List<double> dList = market.GetSubListD(db, dtpProspects.Value, days);


                int counter = dList.Count - 1;
                counter = counter - backtrack;
                double lstelemDaysNum = 0;
                int ds = 0;
                if (dList.ElementAt(counter) > 0)
                {
                    while (dList.ElementAt(counter) >= 0 && counter >= 1)
                    {
                        lstelemDaysNum += dList.ElementAt(counter) * 100;
                        ds++;
                        counter--;
                    }


                }
                else if (dList.ElementAt(counter) < 0)
                {
                    while (dList.ElementAt(counter) <= 0 && counter >= 1)
                    {
                        lstelemDaysNum += dList.ElementAt(counter) * 100;
                        ds++;
                        counter--;
                    }

                }
                else if (dList.ElementAt(counter) == 0 && backtrack < 10)
                {
                    return GetLastStreak(db, days, backtrack + 1);
                }
                else
                    return "0,0";


                return lstelemDaysNum.ToString() + "," + ds.ToString();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private string GetSecondLastStreak(DataBundle db, int days,int backtrack)
        {

            try
            {


                List<double> dList = market.GetSubListD(db, dtpProspects.Value, days);

                int counter = dList.Count - 1;
                counter = counter - backtrack;
                int ds = 0;
                double lstelemDaysNum = 0;
                double sndlstelemDaysNum = 0;
                if (dList.ElementAt(counter) > 0)
                {
                    while (dList.ElementAt(counter) >= 0 && counter >= 1)
                    {
                        lstelemDaysNum += dList.ElementAt(counter) * 100;
                        counter--;
                    }

                    while (dList.ElementAt(counter) <= 0 && counter >= 1)
                    {
                        sndlstelemDaysNum += dList.ElementAt(counter) * 100;
                        ds++;
                        counter--;
                    }

                }
                else if (dList.ElementAt(counter) < 0)
                {
                    while (dList.ElementAt(counter) <= 0 && counter >= 1)
                    {
                        lstelemDaysNum += dList.ElementAt(counter) * 100;
                        counter--;
                    }

                    while (dList.ElementAt(counter) >= 0 && counter >= 1)
                    {
                        sndlstelemDaysNum += dList.ElementAt(counter) * 100;
                        ds++;
                        counter--;
                    }
                }
                else if (dList.ElementAt(counter) == 0 && backtrack < 10)
                {
                    return GetSecondLastStreak(db, days, backtrack + 1);
                }

                return sndlstelemDaysNum.ToString() + "," + ds.ToString();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private void dgvProspects_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvProspects_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            try
            {
                if (e.RowIndex == -1) return; //check if row index is not selected
                if (this.dgvProspects.Columns[e.ColumnIndex].CellType.Equals(typeof(ChartCell)))
                {
                    Chart chart1 = (Chart)this.dgvProspects[e.ColumnIndex, e.RowIndex].Value;
                    //Enter your chart building code here
                    System.IO.MemoryStream myStream = new System.IO.MemoryStream();
                    chart1.Serializer.Save(myStream);
                    CatStocksChart.Serializer.Load(myStream);
                    CatStocksChart.Width = 1882;
                    CatStocksChart.Height = 872;
                    tabControl1.SelectedIndex = 4;
                }
                else if (this.dgvProspects.Columns[e.ColumnIndex].HeaderText == "گروه")
                {
                    LoadStocks(this.dgvProspects[e.ColumnIndex, e.RowIndex].Value.ToString());
                }
            }
            catch (Exception ex)
            {

            }
        }


        private void btnLoadCategories_Click(object sender, EventArgs e)
        {



            CatStocksChart.Series.Clear();
            CatStocksChart.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;
            CatStocksChart.ChartAreas[0].AxisY.MajorGrid.LineWidth = 0;
            CatStocksChart.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            CatStocksChart.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;

            CatStocksChart.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            CatStocksChart.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
            CatStocksChart.MouseWheel += new MouseEventHandler(chData_MouseWheel);
            //CatStocksChart.ChartAreas.Add(chartArea);

            if (Entries.Count() == 0)
            {

                foreach (Category cat in market.Categories)
                {
                    var series = new Series();

                    series.Name = cat.Name;
                    series.ChartType = SeriesChartType.Line;
                    series.XValueType = ChartValueType.Int32;
                    CatStocksChart.Series.Add(series);

                    if (cat.CategoryIndex.ValueOfTrades.Dates.Count > 0)
                    {
                        //if (cat.CategoryIndex.ValueOfTrades.Dates.Last().DayOfYear == DateTime.Now.AddDays(-4).DayOfYear)
                        List<double> dlist = market.GetSubListD(cat.CategoryIndex.Close, DateTime.Now, 100);
                        for (int w = 100; w >= 0; w--)
                        {
                            DataPoint dp = new DataPoint();
                            dp.XValue = 100 - w;
                            dp.YValues = new double[] { dlist.Skip(dlist.Count - (w + 1)).Take(1).FirstOrDefault() };
                            dp.ToolTip = cat.Name;
                            series.Points.Add(dp);
                        }

                    }

                }
            }
            else
            {
                foreach (Entry ent in Entries)
                {
                    var series = new Series();

                    series.Name = ent.stock.Name;
                    series.ChartType = SeriesChartType.Line;
                    series.XValueType = ChartValueType.Int32;
                    CatStocksChart.Series.Add(series);

                    if (ent.stock.ValueOfTrades.Dates.Count > 0)
                    {
                        //if (cat.CategoryIndex.ValueOfTrades.Dates.Last().DayOfYear == DateTime.Now.AddDays(-4).DayOfYear)
                        List<double> dlist = market.GetSubListD(ent.stock.Close, DateTime.Now, 100);
                        for (int w = 100; w >= 0; w--)
                        {
                            DataPoint dp = new DataPoint();
                            dp.XValue = 100 - w;
                            dp.YValues = new double[] { dlist.Skip(dlist.Count - (w + 1)).Take(1).FirstOrDefault() };
                            dp.ToolTip = ent.stock.Name;
                            series.Points.Add(dp);
                        }

                    }

                }
            }
            CatStocksChart.Invalidate();

            //tabControl1.SelectedIndex = 4;


            var list = new List<KeyValuePair<string, double>>();

            foreach (Category cat in market.Categories)
            {
                double sum = 0;
                //if (cat.Name.Contains("سرمایه"))
                foreach (Stock stk in cat.Stocks)
                {
                    int periodd = 960;
                    if (cat.CategoryIndex.ValueOfTrades.List.Count() > 0 && stk.ValueOfTrades.List.Count > periodd && stk.IsActive)
                        list.Add(new KeyValuePair<string, double>(stk.Name, stk.ValueOfTrades.List.Skip(stk.ValueOfTrades.List.Count() - periodd).Take(periodd).Average()));
                        //list.Add(new KeyValuePair<string, double>(stk.Name, (stk.High.List.Skip(stk.High.List.Count() - 30).Take(30).Max() - stk.Low.List.Skip(stk.Low.List.Count() - 30).Take(30).Min())));// * stk.ValueOfTrades.List.Skip(stk.ValueOfTrades.List.Count() - 300).Take(300).Average()));
                    //list.Add(new KeyValuePair<string, double>(stk.Name, stk.Volume.List.Skip(stk.Volume.List.Count() - 100).Take(100).Average()));
                    //list.Add(new KeyValuePair<string, double>(stk.Name, stk.ValueOfTrades.List.Last()));
                    //list.Add(new KeyValuePair<string, double>(stk.Name, stk.TotalShares * stk.Close.List.Last()));

                }

            }


            list.OrderBy(o => o.Value);

            foreach (KeyValuePair<string, double> kv in list.OrderByDescending(o => o.Value))
            {
                Console.WriteLine(kv.Key + " : " + FormatDouble(kv.Value));
                //Console.WriteLine(kv.Key + " : " + kv.Value);
            }

            LoadCats();

            //*************************************************************8
        }

        private void LoadCats()
        {
            Entries.Clear();
            DateTime latest_date = market.MarketIndex.Close.Dates.Last();
            foreach (Category cat in market.Categories)
            {
                if (cat.HasIndex)
                {
                    if (cat.CategoryIndex.Close.List.Count > 0)
                    {
                        Entry ent = new Entry();
                        ent.cat = cat;
                        ent.stock = cat.CategoryIndex;
                        ent.vol_change = market.GetSubListD(cat.CategoryIndex.Volume, latest_date, 1).Last() * 100;
                        ent.vt_change = market.GetSubListD(cat.CategoryIndex.ValueOfTrades, latest_date, 1).Last() * 100;
                        ent.close_change = market.GetSubListD(cat.CategoryIndex.Close, latest_date, 1).Last() * 100;
                        ent.nt_change = market.GetSubListD(cat.CategoryIndex.NumberOfTrades, latest_date, 1).Last() * 100;

                        ent.index_chart = GetChart(cat.CategoryIndex.Close);

                        string lststrk = GetLastStreak(cat.CategoryIndex.Close, 50,0);
                        ent.last_streak_sum = Convert.ToDouble(lststrk.Split(',')[0]);
                        ent.last_streak_days = Convert.ToInt32(lststrk.Split(',')[1]);
                        ent.last_streak_friendly = ent.last_streak_sum.ToString() + " in " + ent.last_streak_days.ToString() + " days";

                        string scndlststrk = GetSecondLastStreak(cat.CategoryIndex.Close, 50,0);
                        ent.snd_last_streak_sum = Convert.ToDouble(scndlststrk.Split(',')[0]);
                        ent.snd_last_streak_days = Convert.ToInt32(scndlststrk.Split(',')[1]);
                        ent.snd_last_streak_friendly = ent.snd_last_streak_sum.ToString() + " in " + ent.snd_last_streak_days.ToString() + " days";
                        ent.mode = EntryMode.Categories;
                        Entries.Add(ent);
                    }

                }
            }

            OrderizeCats();


            dgvProspects.Rows.Clear();
            dgvProspects.Columns.Clear();
            dgvProspects.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "گروه", Name = "instrument" });
            dgvProspects.Columns[0].Width = 90;
            dgvProspects.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "عملکرد", Name = "close_change" });
            dgvProspects.Columns[1].Width = 60;
            dgvProspects.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "حجم%", Name = "vol_change" });
            dgvProspects.Columns[2].Width = 60;
            dgvProspects.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "ارزش معامله%", Name = "vt_change" });
            dgvProspects.Columns[3].Width = 60;
            dgvProspects.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "تعداد معامله%", Name = "nt_change" });
            dgvProspects.Columns[4].Width = 60;
            dgvProspects.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "روند قبلی", Name = "2last_streak" });
            dgvProspects.Columns[5].Width = 120;
            dgvProspects.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "آخرین روند", Name = "1last_streak" });
            dgvProspects.Columns[6].Width = 120;
            dgvProspects.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "ملاحظات", Name = "desc" });
            dgvProspects.Columns[7].Width = 80;
            dgvProspects.Columns.Add(new ChartColumn() { HeaderText = "نمودار سهم", Name = "instrument_chart" });
            dgvProspects.Columns[8].Width = Constants.ChartCellWidth;

            dgvProspects.RowHeadersVisible = false;
            dgvProspects.VirtualMode = true;
            this.dgvProspects.CellValueNeeded += new DataGridViewCellValueEventHandler(dgvProspects_CellValueNeeded);
            this.dgvProspects.RowCount = Entries.Count;
        }

        private void btnLoadStocks_Click(object sender, EventArgs e)
        {
            LoadStocks();
        }

        private void LoadStocks()
        {
            Entries.Clear();
            DateTime latest_date = market.MarketIndex.Close.Dates.Last();
            foreach (Category cat in market.Categories)
            {
                foreach (Stock stk in cat.Stocks)
                {
                    if (stk.IsActive && stk.IsCompetent)
                    if (stk.Close.List.Count > 50)
                    {
                        Entry ent = new Entry();
                        ent.stock = stk;
                        ent.name = stk.Name + Environment.NewLine + Environment.NewLine + stk.ParentCategory.Name;
                        ent.vol_change = market.GetSubListD(stk.Volume, latest_date, 1).Last() * 100;
                        ent.vt_change = market.GetSubListD(stk.ValueOfTrades, latest_date, 1).Last() * 100;
                        ent.close_change = market.GetSubListD(stk.Close, latest_date, 1).Last() * 100;
                        ent.nt_change = market.GetSubListD(stk.NumberOfTrades, latest_date, 1).Last() * 100;
                        ent.fiftyday_basevol_surpass_percentage = Math.Round(((double)stk.Volume.List.Skip(Math.Max(0, stk.Volume.List.Count() - 50)).Count(x => x > stk.BaseVolume) / 50) * 100, 2);
                        ent.index_chart = GetChart(stk.Close);
                        ent.final_prcnt_1dayago = market.GetSubListD(stk.Close, latest_date, 5).ElementAt(4) * 100;
                        ent.final_prcnt_2daysago = market.GetSubListD(stk.Close, latest_date, 5).ElementAt(3) * 100;
                        ent.final_prcnt_3daysago = market.GetSubListD(stk.Close, latest_date, 5).ElementAt(2) * 100;

                        string lststrk = GetLastStreak(stk.Close, 50,0);
                        ent.last_streak_sum = Convert.ToDouble(lststrk.Split(',')[0]);
                        ent.last_streak_days = Convert.ToInt32(lststrk.Split(',')[1]);
                        ent.last_streak_friendly = ent.last_streak_sum.ToString() + " in " + ent.last_streak_days.ToString() + " days";

                        string scndlststrk = GetSecondLastStreak(stk.Close, 50,0);
                        ent.snd_last_streak_sum = Convert.ToDouble(scndlststrk.Split(',')[0]);
                        ent.snd_last_streak_days = Convert.ToInt32(scndlststrk.Split(',')[1]);
                        ent.snd_last_streak_friendly = ent.snd_last_streak_sum.ToString() + " in " + ent.snd_last_streak_days.ToString() + " days";
                        ent.mode = EntryMode.Stocks;
                        if (!stk.IsActive)
                            ent.desc = "CLOSED";

                        double last_thirth_days_min_price = stk.Close.List.Skip(stk.Close.List.Count() - 30).Take(30).Min();
                        if (((stk.Close.List.Last()- last_thirth_days_min_price)/ last_thirth_days_min_price) <= 0.2)
                                ent.desc = "Goooood Choice!!";
                            Entries.Add(ent);
                    }

                }
            }

            OrderizeStocks();

            dgvProspects.Rows.Clear();
            dgvProspects.Columns.Clear();
            dgvProspects.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "سهم", Name = "instrument_detailed" });
            dgvProspects.Columns[0].Width = 90;
            dgvProspects.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "عملکرد", Name = "close_change" });
            dgvProspects.Columns[1].Width = 60;
            dgvProspects.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "حجم%", Name = "vol_change" });
            dgvProspects.Columns[2].Width = 60;
            dgvProspects.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "ارزش معامله%", Name = "vt_change" });
            dgvProspects.Columns[3].Width = 60;
            dgvProspects.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "تعداد معامله%", Name = "nt_change" });
            dgvProspects.Columns[4].Width = 60;
            dgvProspects.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "روند قبلی", Name = "2last_streak" });
            dgvProspects.Columns[5].Width = 120;
            dgvProspects.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "آخرین روند", Name = "1last_streak" });
            dgvProspects.Columns[6].Width = 120;
            dgvProspects.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "ملاحظات", Name = "desc" });
            dgvProspects.Columns[7].Width = 80;
            dgvProspects.Columns.Add(new ChartColumn() { HeaderText = "نمودار سهم", Name = "instrument_chart" });
            dgvProspects.Columns[8].Width = Constants.ChartCellWidth;
            dgvProspects.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "سه روزپیش", Name = "final_prcnt_3daysago" });
            dgvProspects.Columns[9].Width = 60;
            dgvProspects.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "دو روز پیش", Name = "final_prcnt_2daysago" });
            dgvProspects.Columns[10].Width = 60;
            dgvProspects.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "یه روز پیش", Name = "final_prcnt_1dayago" });
            dgvProspects.Columns[11].Width = 60;

            dgvProspects.RowHeadersVisible = false;
            dgvProspects.VirtualMode = true;
            this.dgvProspects.CellValueNeeded += new DataGridViewCellValueEventHandler(dgvProspects_CellValueNeeded);
            this.dgvProspects.RowCount = Entries.Count;
        }

        private void LoadStocks(string cat_name)
        {
            Entries.Clear();
            DateTime latest_date = market.MarketIndex.Close.Dates.Last();

            foreach (Stock stk in market.Categories.Find(x => x.Name == cat_name).Stocks)
            {
                if (stk.IsActive && stk.IsCompetent)
                if (stk.Close.List.Count > 0)
                {
                    Entry ent = new Entry();
                    ent.stock = stk;
                    ent.name = stk.Name + Environment.NewLine + Environment.NewLine + stk.ParentCategory.Name;
                    ent.vol_change = market.GetSubListD(stk.Volume, latest_date, 1).Last() * 100;
                    ent.vt_change = market.GetSubListD(stk.ValueOfTrades, latest_date, 1).Last() * 100;
                    ent.close_change = market.GetSubListD(stk.Close, latest_date, 1).Last() * 100;
                    ent.nt_change = market.GetSubListD(stk.NumberOfTrades, latest_date, 1).Last() * 100;
                    ent.fiftyday_basevol_surpass_percentage = Math.Round(((double)stk.Volume.List.Skip(Math.Max(0, stk.Volume.List.Count() - 50)).Count(x => x > stk.BaseVolume) / 50), 2);
                    ent.index_chart = GetChart(stk.Close);

                    string lststrk = GetLastStreak(stk.Close, 50,0);
                    ent.last_streak_sum = Convert.ToDouble(lststrk.Split(',')[0]);
                    ent.last_streak_days = Convert.ToInt32(lststrk.Split(',')[1]);
                    ent.last_streak_friendly = ent.last_streak_sum.ToString() + " in " + ent.last_streak_days.ToString() + " days";

                    string scndlststrk = GetSecondLastStreak(stk.Close, 50,0);
                    ent.snd_last_streak_sum = Convert.ToDouble(scndlststrk.Split(',')[0]);
                    ent.snd_last_streak_days = Convert.ToInt32(scndlststrk.Split(',')[1]);
                    ent.snd_last_streak_friendly = ent.snd_last_streak_sum.ToString() + " in " + ent.snd_last_streak_days.ToString() + " days";
                    ent.mode = EntryMode.Stocks;
                    if (!stk.IsActive)
                        ent.desc = "CLOSED";
                    Entries.Add(ent);
                }

            }


            OrderizeStocks();

            dgvProspects.Rows.Clear();
            dgvProspects.Columns.Clear();
            dgvProspects.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "سهم", Name = "instrument_detailed" });
            dgvProspects.Columns[0].Width = 90;
            dgvProspects.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "عملکرد", Name = "close_change" });
            dgvProspects.Columns[1].Width = 60;
            dgvProspects.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "حجم%", Name = "vol_change" });
            dgvProspects.Columns[2].Width = 60;
            dgvProspects.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "ارزش معامله%", Name = "vt_change" });
            dgvProspects.Columns[3].Width = 60;
            dgvProspects.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "تعداد معامله%", Name = "nt_change" });
            dgvProspects.Columns[4].Width = 60;
            dgvProspects.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "روند قبلی", Name = "2last_streak" });
            dgvProspects.Columns[5].Width = 120;
            dgvProspects.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "آخرین روند", Name = "1last_streak" });
            dgvProspects.Columns[6].Width = 120;
            dgvProspects.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "ملاحظات", Name = "desc" });
            dgvProspects.Columns[7].Width = 80;
            dgvProspects.Columns.Add(new ChartColumn() { HeaderText = "نمودار سهم", Name = "instrument_chart" });
            dgvProspects.Columns[8].Width = Constants.ChartCellWidth;

            dgvProspects.RowHeadersVisible = false;
            dgvProspects.VirtualMode = true;
            this.dgvProspects.CellValueNeeded += new DataGridViewCellValueEventHandler(dgvProspects_CellValueNeeded);
            this.dgvProspects.RowCount = Entries.Count;
        }

        #region DataCollector


        int counter = 1;
        List<Stock> Stocks = new List<Stock>();
        private void btnStartDataCollector_Click(object sender, EventArgs e)
        {
            foreach (Category cat in market.Categories)
            {
                foreach (Stock stk in cat.Stocks)
                    Stocks.Add(stk);
            }
            btnStartDataCollector.Enabled = false;
            TimeSpan now = DateTime.Now.TimeOfDay;

            if (MarketIsOpen())
            {
                f1();
            }
            else
            {
                Task task = Repeat.Interval(
                    TimeSpan.FromSeconds(30),
                    () => f2(), cancellationTokenSource2.Token);
            }
        }

        private void f1()
        {
            CheckIfFileExists();

            counter = 0;

            Task task = Repeat.Interval(
                    TimeSpan.FromSeconds(30),
                    () => FetchData(), cancellationTokenSource1.Token);

            //FetchData();
        }

        private void f2()
        {
            TimeSpan now = DateTime.Now.TimeOfDay;

            if (MarketIsOpen())
            {
                cancellationTokenSource2.Cancel();
                f1();
            }
            else
            {
                this.lblCounter.BeginInvoke((MethodInvoker)delegate () { this.lblCounter.Text = "Markets are closed at this time, waiting..." + Environment.NewLine + DateTime.Now.ToShortTimeString(); });
            }
        }

        void DownloadDataCompleted(Stock stk, DownloadDataCompletedEventArgs e)
        {
            try
            {
                Encoding encoding = Encoding.GetEncoding("windows-1251");
                GZipStream gzip = new GZipStream(new MemoryStream(e.Result), CompressionMode.Decompress);
                MemoryStream decompressed = new MemoryStream();
                gzip.CopyTo(decompressed);
                string str = encoding.GetString(decompressed.GetBuffer(), 0, (int)decompressed.Length);

                Step stp = new Step();
                stp.tick = DateTime.Now.Ticks.ToString();
                stp.tsetid = stk.TSETID;

                string[] mainparts = str.Split(';');
                // parse part 1
                if (mainparts.Count() > 0)
                {
                    string[] firstpartelements = mainparts[0].Split(',');

                    //firstpartelements[0] // time of last trade
                    //firstpartelements[1] // status of stock availability

                    if (firstpartelements[1].ToString().Trim().ToLower() != "a")
                    {
                        stk.IsActive = false;
                        return;
                    }

                    stp.lateset_trade_price = firstpartelements[2];
                    stp.close = firstpartelements[3];
                    stp.open = firstpartelements[4];

                    stp.yesterday_price = firstpartelements[5];
                    stp.high = firstpartelements[6];
                    stp.low = firstpartelements[7];
                    stp.number_of_trades = firstpartelements[8];
                    stp.vol = firstpartelements[9];
                    stp.value_of_trades = firstpartelements[10];
                    stp.date = firstpartelements[12];


                }

                // haghighi hoghooghi 
                if (mainparts.Count() >= 4)
                {
                    string[] firstpartelements = mainparts[4].Split(',');

                    stp.b_ind_vol = firstpartelements[0];
                    stp.b_ins_vol = firstpartelements[1];
                    stp.s_ind_vol = firstpartelements[3];
                    stp.s_ins_vol = firstpartelements[4];

                    stp.b_ind_number = firstpartelements[5];
                    stp.b_ins_number = firstpartelements[6];
                    stp.s_ind_number = firstpartelements[8];
                    stp.s_ins_number = firstpartelements[9];

                }

                if (mainparts.Count() >= 3)
                {
                    string[] firstpartelements = mainparts[2].Split(',');

                    string[] r1 = firstpartelements[0].Split('@');

                    stp.b1number = r1[0];
                    stp.b1volume = r1[1];
                    stp.b1price = r1[2];
                    stp.s1price = r1[3];
                    stp.s1volume = r1[4];
                    stp.s1number = r1[5];

                    string[] r2 = firstpartelements[1].Split('@');

                    stp.b2number = r2[0];
                    stp.b2volume = r2[1];
                    stp.b2price = r2[2];
                    stp.s2price = r2[3];
                    stp.s2volume = r2[4];
                    stp.s2number = r2[5];

                    string[] r3 = firstpartelements[2].Split('@');

                    stp.b3number = r3[0];
                    stp.b3volume = r3[1];
                    stp.b3price = r3[2];
                    stp.s3price = r3[3];
                    stp.s3volume = r3[4];
                    stp.s3number = r3[5];

                }


                //var regex = new Regex(@"ZTitad=[0-9]*");
                //Match m = regex.Match(task.Result);
                //nd["totalshares"].InnerText = m.Value.Split('=')[1];
                stk.Steps.Add(stp);


                //Console.WriteLine(stk.Name);
            }
            catch (Exception ex)
            {

            }
        }

        public void FetchData()
        {
            try
            {
                if (MarketIsOpen())
                {
                    foreach (Stock stk in Stocks)
                    {
                        string s = string.Format(string.Format(@"http://www.tsetmc.com/tsev2/data/instinfofast.aspx?i={0}&c=34+", stk.TSETID));
                        WebClient wc = new WebClient();
                        wc.DownloadDataAsync(new Uri(s));
                        wc.DownloadDataCompleted += (sender, args) => DownloadDataCompleted(stk, args);
                    }

                    List<StepBundle> output = new List<StepBundle>();

                    foreach (Stock stk in Stocks)
                    {
                        StepBundle bun = new StepBundle();
                        bun.tsetid = stk.TSETID;
                        bun.Steps = stk.Steps;
                        output.Add(bun);
                    }

                    //// serialize results so far
                    BinarySerializeObject(output, Constants.OnlineDataLocalPath + string.Format(Constants.OnlineDataFileName, DateTime.Now.ToString("dd_MM_yyyy")));


                    this.lblCounter.BeginInvoke((MethodInvoker)delegate () { this.lblCounter.Text = DateTime.Now.ToString("dd_MM_yyyy") + ".zip -- " + "step: " + (counter++).ToString(); });
                }
                else
                {
                    // cancel the streak
                    cancellationTokenSource1.Cancel();
                    cancellationTokenSource2 = new CancellationTokenSource();
                    Task task = Repeat.Interval(
                        TimeSpan.FromSeconds(30),
                        () => f2(), cancellationTokenSource2.Token);
                }
            }
            catch(Exception ex)
            {
                this.lblCounter.BeginInvoke((MethodInvoker)delegate () { this.lblCounter.Text = ex.Message; });
            }
        }

        private bool MarketIsOpen()
        {
            TimeSpan now = DateTime.Now.TimeOfDay;
            if ((now > Constants.MarketOpen) && (now < Constants.MarketClose))
            {
                if (DateTime.Now.DayOfWeek != DayOfWeek.Friday && DateTime.Now.DayOfWeek != DayOfWeek.Thursday)
                {
                    return true;
                }
            }
            return false;
        }

        private void CheckIfFileExists()
        {
            try
            {
                if (File.Exists(Constants.OnlineDataLocalPath + string.Format(Constants.OnlineDataFileName, DateTime.Now.ToString("dd_MM_yyyy"))))
                {

                    List<StepBundle> tmp = BinaryDeSerializeObject<List<StepBundle>>(Constants.OnlineDataLocalPath + string.Format(Constants.OnlineDataFileName, DateTime.Now.ToString("dd_MM_yyyy")));

                    foreach (Stock stk in Stocks)
                    {
                        stk.Steps = tmp.Find(x => x.tsetid == stk.TSETID).Steps;
                    }

                    counter = tmp.Last().Steps.Count();
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
            }
        }

        private static T BinaryDeSerializeObject<T>(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) { return default(T); }

            T objectOut = default(T);

            try
            {

                using (Stream stream = new MemoryStream())
                {
                    using (ZipFile zip1 = ZipFile.Read(fileName))
                    {
                        ZipEntry entry = zip1["data"];
                        entry.Extract(stream);  // extract uncompressed content into a memorystream 
                    }
                    stream.Position = 0;
                    var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    objectOut = (T)binaryFormatter.Deserialize(stream);
                }

            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
            }

            return objectOut;
        }

        public void BinarySerializeObject<T>(T serializableObject, string fileName)
        {
            if (serializableObject == null) { return; }

            try
            {
                using (Stream stream = new MemoryStream())// File.Open(fileName, FileMode.OpenOrCreate))
                {
                    var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    binaryFormatter.Serialize(stream, serializableObject);
                    stream.Position = 0;
                    using (ZipFile zipFile = new ZipFile())
                    {
                        zipFile.AddEntry("data", stream);
                        zipFile.Save(fileName);
                    }
                    
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                
            }
        }

        #endregion

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Refresh();
            this.Invalidate();
        }

        private void btnNewProspects_Click(object sender, EventArgs e)
        {
            double toll = 0;
            while (Entries.Count() == 0 || toll <= 0.5)
            {
                toll = toll + 0.01;
                foreach (Category cat in market.Categories)
                {
                    foreach (Stock stk in cat.Stocks)
                    {
                        if (!stk.IsActive)
                            continue;

                        string desc = "";
                        double ninetydaytradevalueaverage = stk.ValueOfTrades.List.Skip(stk.ValueOfTrades.List.Count() - 90).Take(90).Average();
                        //double tentydaytradevalueaverage = stk.ValueOfTrades.List.Skip(stk.ValueOfTrades.List.Count() - 10).Take(10).Average();
                        if (ninetydaytradevalueaverage < 5000000000)
                            continue;
                        //if (tentydaytradevalueaverage < 3000000000)
                        //    continue;

                        Criterion cri = new Criterion();
                        cri.Name = "custom criterion";
                        cri.id = "h3";
                        cri.HeadPeriod = 1;
                        cri.HeadSlope = Slope.Flat;
                        cri.HeadRangeMin = 0;
                        cri.HeadRangeMax = 999;
                        cri.HeadAverageAmount = Amount.N;
                        cri.TailPeriod = 10;
                        cri.TailSlope = Slope.Flat;
                        cri.TailRangeMin = -999;
                        cri.TailRangeMax = 0;
                        cri.TailPullbackDistribution = 0.5;
                        cri.TailPullBackTolerance = toll;
                        cri.TailAverageAmount = Amount.N;

                        if (!CriterionAnalyser.AnalyseDerivative(market.GetSubListD(stk.Close, DateTime.Now, 11), cri, stk.Close))
                            continue;

                        desc += "90-day VT avg: " + FormatDouble(ninetydaytradevalueaverage) + Environment.NewLine;
                        //desc += "trade avg: " + FormatDouble(stk.ValueOfTrades.List.Skip(stk.ValueOfTrades.List.Count() - 10).Take(10).Average()/ stk.NumberOfTrades.List.Skip(stk.NumberOfTrades.List.Count() - 10).Take(10).Average()) + Environment.NewLine;
                        desc += "trade avg: " + FormatDouble(stk.ValueOfTrades.List.Last() / stk.NumberOfTrades.List.Last()) + Environment.NewLine;
                        desc += "tollerance: " + toll;

                        if (Entries.Count(x => x.stock == stk) == 0)
                        {
                            Entry ent = new Entry();
                            ent.name = stk.Name + Environment.NewLine + stk.ParentCategory.Name;
                            ent.stock = stk;
                            ent.index_chart = GetChart(stk.Close);
                            ent.mode = EntryMode.Stocks;
                            ent.desc = desc;
                            Entries.Add(ent);
                        }
                    }
                }
            }

            dgvProspects.Rows.Clear();
            dgvProspects.Columns.Clear();
            dgvProspects.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "گروه", Name = "instrument" });
            dgvProspects.Columns[0].Width = 90;
            dgvProspects.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "ملاحظات", Name = "desc" });
            dgvProspects.Columns[1].Width = 300;
            dgvProspects.Columns.Add(new ChartColumn() { HeaderText = "نمودار سهم", Name = "instrument_chart" });
            dgvProspects.Columns[2].Width = Constants.ChartCellWidth;

            dgvProspects.RowHeadersVisible = false;
            dgvProspects.VirtualMode = true;
            this.dgvProspects.CellValueNeeded += new DataGridViewCellValueEventHandler(dgvProspects_CellValueNeeded);
            this.dgvProspects.RowCount = Entries.Count;
        }


        private void btnPossessionHistory_Click_1(object sender, EventArgs e)
        {
            string s = string.Format(string.Format(@"http://www.tsetmc.com/tsev2/data/clienttype.aspx?i={0}", Entries.FirstOrDefault().stock.TSETID));
            WebClient wc = new WebClient();
            wc.DownloadDataAsync(new Uri(s));
            wc.DownloadDataCompleted += (senderz, args) => DownloadPossessionDataCompleted(Entries.FirstOrDefault().stock, args);
        }

        void DownloadPossessionDataCompleted(Stock stk, DownloadDataCompletedEventArgs e)
        {
            try
            {
                Encoding encoding = Encoding.GetEncoding("windows-1251");
                GZipStream gzip = new GZipStream(new MemoryStream(e.Result), CompressionMode.Decompress);
                MemoryStream decompressed = new MemoryStream();
                gzip.CopyTo(decompressed);
                string str = encoding.GetString(decompressed.GetBuffer(), 0, (int)decompressed.Length);

                string[] mainparts = str.Split(';');

                CatStocksChart.Series.Clear();
                CatStocksChart.ChartAreas[0].AxisX.MajorGrid.LineWidth = 1;
                CatStocksChart.ChartAreas[0].AxisY.MajorGrid.LineWidth = 1;
                CatStocksChart.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
                CatStocksChart.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;
                
                CatStocksChart.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
                CatStocksChart.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
                CatStocksChart.MouseWheel += new MouseEventHandler(chData_MouseWheel);

                //var series1 = new Series();

                //series1.Name = "حقیقی-خرید";
                //series1.ChartType = SeriesChartType.Line;
                //series1.XValueType = ChartValueType.Date;
                //CatStocksChart.Series.Add(series1);

                //var series2 = new Series();

                //series2.Name = "حقوقی-خرید";
                //series2.ChartType = SeriesChartType.Line;
                //series2.XValueType = ChartValueType.Date;
                //CatStocksChart.Series.Add(series2);

                //var series3 = new Series();

                //series3.Name = "حقیقی-فروش";
                //series3.ChartType = SeriesChartType.Line;
                //series3.XValueType = ChartValueType.Date;
                //CatStocksChart.Series.Add(series3);

                //var series4 = new Series();

                //series4.Name = "حقوقی-فروش";
                //series4.ChartType = SeriesChartType.Line;
                //series4.XValueType = ChartValueType.Date;
                //CatStocksChart.Series.Add(series4);

                var series5 = new Series();

                series5.Name = "تبدیل حقوقی به حقیقی";
                series5.ChartType = SeriesChartType.Line;
                series5.XValueType = ChartValueType.Date;
                CatStocksChart.Series.Add(series5);

                // parse part 1
                if (mainparts.Count() > 0)
                {
                    foreach (string tick in mainparts)
                    {
                        string[] elements = tick.Split(',');

                        string year = elements[0].Substring(0, 4);
                        string month = elements[0].Substring(4, 2);
                        string day = elements[0].Substring(6, 2);

                        DateTime dt = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), Convert.ToInt32(day));

                        if (dt.Year < 2016)
                            continue;

                        //DataPoint dp1 = new DataPoint();
                        //dp1.XValue = dt.ToOADate();
                        //dp1.YValues = new double[] { Convert.ToDouble(elements[9])  };
                        //dp1.ToolTip = series1.Name;
                        //series1.Points.Add(dp1);

                        //DataPoint dp2 = new DataPoint();
                        //dp2.XValue = dt.ToOADate();
                        //dp2.YValues = new double[] { Convert.ToDouble(elements[10]) };
                        //dp2.ToolTip = series2.Name;
                        //series2.Points.Add(dp2);

                        //DataPoint dp3 = new DataPoint();
                        //dp3.XValue = dt.ToOADate();
                        //dp3.YValues = new double[] { Convert.ToDouble(elements[11])*(-1) };
                        //dp3.ToolTip = series3.Name;
                        //series3.Points.Add(dp3);

                        //DataPoint dp4 = new DataPoint();
                        //dp4.XValue = dt.ToOADate();
                        //dp4.YValues = new double[] { Convert.ToDouble(elements[12])*(-1) };
                        //dp4.ToolTip = series4.Name;
                        //series4.Points.Add(dp4);

                        DataPoint dp5 = new DataPoint();
                        dp5.XValue = dt.ToOADate();
                        dp5.YValues = new double[] { Convert.ToDouble(elements[9]) - Convert.ToDouble(elements[11]) };
                        dp5.ToolTip = series5.Name;
                        series5.Points.Add(dp5);


                    }
                    



                }


                //Console.WriteLine(stk.Name);
            }
            catch (Exception ex)
            {

            }
        }

        private void btnCalcMeanStrategy_Click(object sender, EventArgs e)
        {
            DateTime dtItirator = new DateTime(2016, 7, 3, 00, 00, 0);
            Entry ent = Entries.First();
            Console.WriteLine("Stock Name: " + ent.stock.Name);
            double gov_cut = 0.0048;
            int cum_stocks = 0;
            double cum_cost;
            //double begin_price = Convert.ToDouble(txtMeanPrice.Text);
            double steps_height = Convert.ToDouble(txtMeanSteps.Text);

            double high = ent.stock.High.GetValueAt(dtItirator);
            double low = ent.stock.Low.GetValueAt(dtItirator);
            Random rnd = new Random();
            
            int rnd_price_to_trade_at_first_step = rnd.Next(Convert.ToInt32(low), Convert.ToInt32(high));
            int number_of_stocks = 10000000 / rnd_price_to_trade_at_first_step;
            cum_stocks += number_of_stocks;
            cum_cost = (number_of_stocks * rnd_price_to_trade_at_first_step) + ((number_of_stocks * rnd_price_to_trade_at_first_step) * gov_cut);
            //ent.st

            dtItirator = dtItirator.AddDays(1);
            while (ent.stock.High.Dates.Last() > dtItirator)
            {



            }

            for (int i = 1;i<=8;i++)
            {

                Console.WriteLine("step: " + i);
                ent.stock.High.GetValueAt(dtItirator);




                Console.WriteLine("*****************************");

            }
        }
    }

    internal static class Repeat
    {
        public static Task Interval(
            TimeSpan pollInterval,
            Action action,
            CancellationToken token)
        {
            // We don't use Observable.Interval:
            // If we block, the values start bunching up behind each other.
            return Task.Factory.StartNew(
                () =>
                {
                    for (;;)
                    {
                        if (token.WaitCancellationRequested(pollInterval))
                            break;

                        action();
                    }
                }, token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }
    }

    static class CancellationTokenExtensions
    {
        public static bool WaitCancellationRequested(
            this CancellationToken token,
            TimeSpan timeout)
        {
            return token.WaitHandle.WaitOne(timeout);
        }
    }

    class CustomComparer : IComparer
    {
        private static int SortOrder = 1;

        public CustomComparer(SortOrder sortOrder)
        {
            SortOrder = sortOrder == System.Windows.Forms.SortOrder.Ascending ? 1 : -1;
        }

        public int Compare(object x, object y)
        {
            double result1 = 0;
            double result2 = 0;
            DataGridViewRow row1 = (DataGridViewRow)x;
            DataGridViewRow row2 = (DataGridViewRow)y;


            result1 = Convert.ToDouble(row1.Cells[4].Value) * (Convert.ToDouble(row1.Cells[5].Value));
            result2 = Convert.ToDouble(row2.Cells[4].Value) * (Convert.ToDouble(row2.Cells[5].Value));


            return result1.CompareTo(result2);// * SortOrder;
        }
    }
}



