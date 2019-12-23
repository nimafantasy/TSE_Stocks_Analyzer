using HtmlAgilityPack;
using Ionic.Zip;
using StocksHelper.Data;
using StocksHelper.Enum;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace StocksHelper
{
    public static class DataLoader
    {
        // loads stocks data into objects
        
        public static Market LoadMarket()
        {
            Market market = new Market();
            bool first_line = true;
            DateTime Latest_Market_Date = new DateTime();
            // load market index

            var reader = new StreamReader(File.OpenRead(Constants.RawFilesPath + @"\" + Constants.MarketIndexFileName));
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (first_line)
                {
                    first_line = false;
                    continue;
                }
                Tick tck = new Tick();
                
                string[] parts = Splitem(line);

                tck.Date = new DateTime(Convert.ToInt32(parts[0], CultureInfo.InvariantCulture), Convert.ToInt32(parts[1], CultureInfo.InvariantCulture), Convert.ToInt32(parts[2], CultureInfo.InvariantCulture));
                tck.Open = Convert.ToDouble(parts[3], CultureInfo.InvariantCulture);
                tck.High = Convert.ToDouble(parts[4], CultureInfo.InvariantCulture);
                tck.Low = Convert.ToDouble(parts[5], CultureInfo.InvariantCulture);
                tck.Close = Convert.ToDouble(parts[6], CultureInfo.InvariantCulture);
                tck.Volume = Convert.ToDouble(parts[7], CultureInfo.InvariantCulture);
                tck.ValueOfTrades = Convert.ToDouble(parts[8], CultureInfo.InvariantCulture);
                tck.NumberOfTrades = Convert.ToDouble(parts[9], CultureInfo.InvariantCulture);
                tck.YesterdayPrice = Convert.ToDouble(parts[10], CultureInfo.InvariantCulture);

                Latest_Market_Date = tck.Date;

                market.MarketIndex.Ticks.Add(tck);
            }
            market.MarketIndex.Name = Constants.MarketIndexName;
            market.MarketIndex.Init();


            //// load industry index
            //first_line = true;
            //reader = new StreamReader(File.OpenRead(Constants.RawFilesPath + @"\" + Constants.IndustryIndexFileName));
            //while (!reader.EndOfStream)
            //{
            //    if (first_line)
            //    {
            //        first_line = false;
            //        continue;
            //    }
            //    Tick tck = new Tick();
            //    var line = reader.ReadLine();
            //    string[] parts = Splitem(line);

            //    tck.Date = new DateTime(Convert.ToInt32(parts[0]), Convert.ToInt32(parts[1]), Convert.ToInt32(parts[2]));
            //    tck.Open = Convert.ToDouble(parts[3]);
            //    tck.High = Convert.ToDouble(parts[4]);
            //    tck.Low = Convert.ToDouble(parts[5]);
            //    tck.Close = Convert.ToDouble(parts[6]);
            //    tck.Volume = Convert.ToDouble(parts[7]);
            //    tck.ValueOfTrades = Convert.ToDouble(parts[8]);
            //    tck.NumberOfTrades = Convert.ToDouble(parts[9]);
            //    tck.YesterdayPrice = Convert.ToDouble(parts[10]);


            //    market.IndustryIndex.Ticks.Add(tck);
            //}
            //market.IndustryIndex.Name = Constants.IndustryIndexName;
            //market.IndustryIndex.Init();


            //// load big30 index
            //first_line = true;
            //reader = new StreamReader(File.OpenRead(Constants.RawFilesPath + @"\" + Constants.Big30IndexFileName));
            //while (!reader.EndOfStream)
            //{
            //    if (first_line)
            //    {
            //        first_line = false;
            //        continue;
            //    }
            //    Tick tck = new Tick();
            //    var line = reader.ReadLine();
            //    string[] parts = Splitem(line);

            //    tck.Date = new DateTime(Convert.ToInt32(parts[0]), Convert.ToInt32(parts[1]), Convert.ToInt32(parts[2]));
            //    tck.Open = Convert.ToDouble(parts[3]);
            //    tck.High = Convert.ToDouble(parts[4]);
            //    tck.Low = Convert.ToDouble(parts[5]);
            //    tck.Close = Convert.ToDouble(parts[6]);
            //    tck.Volume = Convert.ToDouble(parts[7]);
            //    tck.ValueOfTrades = Convert.ToDouble(parts[8]);
            //    tck.NumberOfTrades = Convert.ToDouble(parts[9]);
            //    tck.YesterdayPrice = Convert.ToDouble(parts[10]);


            //    market.Big30Index.Ticks.Add(tck);
            //}
            //market.Big30Index.Name = Constants.Big30IndexName;
            //market.Big30Index.Init();



            //// load big50 index
            //first_line = true;
            //reader = new StreamReader(File.OpenRead(Constants.RawFilesPath + @"\" + Constants.Big50IndexFileName));
            //while (!reader.EndOfStream)
            //{
            //    if (first_line)
            //    {
            //        first_line = false;
            //        continue;
            //    }
            //    Tick tck = new Tick();
            //    var line = reader.ReadLine();
            //    string[] parts = Splitem(line);

            //    tck.Date = new DateTime(Convert.ToInt32(parts[0]), Convert.ToInt32(parts[1]), Convert.ToInt32(parts[2]));
            //    tck.Open = Convert.ToDouble(parts[3]);
            //    tck.High = Convert.ToDouble(parts[4]);
            //    tck.Low = Convert.ToDouble(parts[5]);
            //    tck.Close = Convert.ToDouble(parts[6]);
            //    tck.Volume = Convert.ToDouble(parts[7]);
            //    tck.ValueOfTrades = Convert.ToDouble(parts[8]);
            //    tck.NumberOfTrades = Convert.ToDouble(parts[9]);
            //    tck.YesterdayPrice = Convert.ToDouble(parts[10]);


            //    market.Big50Index.Ticks.Add(tck);
            //}
            //market.Big50Index.Name = Constants.Big50IndexName;
            //market.Big50Index.Init();


            //******************************************************************
            //                  load stocks in every category from XML
            //******************************************************************

            // read xml file into object
            XmlDocument doc = new XmlDocument();

            doc.Load(Constants.XmlMarketData);

            //doc.Load(Constants.XmlChosenData);

            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            {
                Category cat = new Category();

                cat.Name = node.Attributes["name"]?.InnerText;
                cat.FileName = node.Attributes["filename"]?.InnerText;
                cat.Stocks = new List<Stock>();
                market.Categories.Add(cat);

                foreach (XmlNode nd in node.ChildNodes)
                {
                    if (cat.Stocks.Exists(x => x.Name == nd.Attributes["name"]?.InnerText))
                    {
                        Stock stk = cat.Stocks.Find(x => x.Name == nd.Attributes["name"]?.InnerText);
                        stk.FileNames.Add(nd["filename"]?.InnerText);
                    }
                    else
                    {
                        Stock stk = new Stock();
                        stk.Name = nd.Attributes["name"]?.InnerText;
                        stk.FileNames.Add(nd["filename"]?.InnerText);
                        stk.TSETID = nd["tsetid"]?.InnerText;
                        stk.TotalShares = Convert.ToDouble(string.IsNullOrEmpty(nd["totalshares"]?.InnerText)? "0" : nd["totalshares"]?.InnerText);
                        stk.OutstandingShares = Convert.ToDouble(string.IsNullOrEmpty(nd["outstandingshares"]?.InnerText) ? "0" : nd["outstandingshares"]?.InnerText);
                        stk.BaseVolume = Convert.ToDouble(string.IsNullOrEmpty(nd["basevolume"]?.InnerText)? "0" : nd["basevolume"]?.InnerText);
                        stk.EPS = Convert.ToDouble(string.IsNullOrEmpty(nd["eps"]?.InnerText)? "0": nd["eps"]?.InnerText);
                        cat.Stocks.Add(stk);
                        stk.ParentCategory = cat;
                    }
                }

            }
            //******************************************************************
            //              END OF XML READING
            //******************************************************************




            foreach (Category cat in market.Categories)
            {
                if (!string.IsNullOrEmpty(cat.FileName))
                {
                    // first load cat index
                    first_line = true;
                    reader = new StreamReader(File.OpenRead(Constants.RawFilesPath + @"\" + cat.FileName));
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if (first_line)
                        {
                            first_line = false;
                            continue;
                        }

                        Tick tck = new Tick();

                        string[] parts = Splitem(line);

                        tck.Date = new DateTime(Convert.ToInt32(parts[0]), Convert.ToInt32(parts[1]), Convert.ToInt32(parts[2]));
                        tck.Open = Convert.ToDouble(parts[3]);
                        tck.High = Convert.ToDouble(parts[4]);
                        tck.Low = Convert.ToDouble(parts[5]);
                        tck.Close = Convert.ToDouble(parts[6]);
                        tck.Volume = Convert.ToDouble(parts[7]);
                        tck.ValueOfTrades = Convert.ToDouble(parts[8]);
                        tck.NumberOfTrades = Convert.ToDouble(parts[9]);
                        tck.YesterdayPrice = Convert.ToDouble(parts[10]);


                        cat.CategoryIndex.Ticks.Add(tck);
                    }
                    cat.CategoryIndex.Name = cat.Name;
                    cat.CategoryIndex.Init();
                    cat.HasIndex = true;
                }
                else
                {
                    cat.HasIndex = false;
                }
                // now load stocks within the category

                foreach (Stock stk in cat.Stocks)
                {
                    stk.IsActive = false;
                    stk.IsCompetent = true;
                    foreach (string filename in stk.FileNames)
                    {
                        first_line = true;
                        reader = new StreamReader(File.OpenRead(Constants.RawFilesPath + @"\" + filename));
                        while (!reader.EndOfStream)
                        {
                            var line = reader.ReadLine();
                            if (first_line)
                            {
                                first_line = false;
                                continue;
                            }

                            Tick tck = new Tick();

                            string[] parts = Splitem(line);

                            tck.Date = new DateTime(Convert.ToInt32(parts[0]), Convert.ToInt32(parts[1]), Convert.ToInt32(parts[2]));
                            tck.Open = Convert.ToDouble(parts[3]);
                            tck.High = Convert.ToDouble(parts[4]);
                            tck.Low = Convert.ToDouble(parts[5]);
                            tck.Close = Convert.ToDouble(parts[6]);
                            tck.Volume = Convert.ToDouble(parts[7]);
                            tck.ValueOfTrades = Convert.ToDouble(parts[8]);
                            tck.NumberOfTrades = Convert.ToDouble(parts[9]);
                            tck.YesterdayPrice = Convert.ToDouble(parts[10]);

                            if (tck.Date == Latest_Market_Date && tck.Volume != 0)
                                stk.IsActive = true;

                            stk.Ticks.Add(tck);
                        }
                    }


                    stk.Init();
                }

            }


            return market;
        }

        public static List<Criterion> LoadCriteria()
        {
            List<Criterion> cl = new List<Criterion>();
            try
            {


                XmlDocument doc = new XmlDocument();
                doc.Load(Constants.XmlCriteriaData);

                foreach (XmlNode node in doc.DocumentElement.ChildNodes)
                {
                    Criterion c = new Criterion();

                    c.Name = node["name"]?.InnerText;
                    c.id = node["id"]?.InnerText;
                    c.HeadPeriod = Convert.ToInt32(node["headperiod"]?.InnerText);
                    c.HeadSlope = (Slope)System.Enum.Parse(typeof(Slope), node["headslope"]?.InnerText, true);
                    c.HeadRangeMin = Convert.ToDouble(node["headrangemin"]?.InnerText);
                    c.HeadRangeMax = Convert.ToDouble(node["headrangemax"]?.InnerText);
                    c.HeadAverageAmount = (Amount)System.Enum.Parse(typeof(Amount), node["headavgamount"]?.InnerText, true);
                    c.HeadSRPosition = (SRPosition)System.Enum.Parse(typeof(SRPosition), node["headsrposition"]?.InnerText);

                    c.TailPeriod = Convert.ToInt32(node["tailperiod"]?.InnerText);
                    c.TailSlope = (Slope)System.Enum.Parse(typeof(Slope), node["tailslope"]?.InnerText, true);
                    c.TailRangeMin = Convert.ToDouble(node["tailrangemin"]?.InnerText);
                    c.TailRangeMax = Convert.ToDouble(node["tailrangemax"]?.InnerText);
                    c.TailPullbackDistribution = Convert.ToDouble(node["taildistribution"]?.InnerText);
                    c.TailPullBackTolerance = Convert.ToDouble(node["tailtolerance"]?.InnerText);
                    c.TailAverageAmount = (Amount)System.Enum.Parse(typeof(Amount), node["tailavgamount"]?.InnerText, true);

                    cl.Add(c);

                }


            }
            catch (Exception ex)
            { }
                return cl;
        }

        public static List<StepBundle> LoadOnlineData(DateTime dt, GroupBox gb)
        {
            string loading_path = "";
            var radioButtons = gb.Controls.OfType<RadioButton>();
            foreach (RadioButton rb in radioButtons)
            {
                if (rb.Checked)
                {
                    if (rb.Name == "rbRemoteLoading")
                        loading_path = string.Format(Constants.OnlineDataRemotePath, "new-msi");
                    else if (rb.Name == "rbLocalLoading")
                        loading_path = Constants.OnlineDataLocalPath;
                }
            }
            try
            {
                while (!File.Exists(loading_path+string.Format(Constants.OnlineDataFileName, dt.ToString("dd_MM_yyyy"))))
                {
                    dt = dt.AddDays(-1);
                }

                List<StepBundle> tmp = BinaryDeSerializeObject<List<StepBundle>>(loading_path + string.Format(Constants.OnlineDataFileName, dt.ToString("dd_MM_yyyy")));
                return tmp;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<StepBundle>();
            }
        }

        private static T XMLDeSerializeObject<T>(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) { return default(T); }

            T objectOut = default(T);

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(fileName);
                string xmlString = xmlDocument.OuterXml;

                using (StringReader read = new StringReader(xmlString))
                {
                    Type outType = typeof(T);

                    XmlSerializer serializer = new XmlSerializer(outType);
                    using (XmlReader reader = new XmlTextReader(read))
                    {
                        objectOut = (T)serializer.Deserialize(reader);
                        reader.Close();
                    }

                    read.Close();
                }
            }
            catch (Exception ex)
            {
                //Log exception here
            }

            return objectOut;
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
                Console.WriteLine(ex.Message);
            }

            return objectOut;
        }
        private static string[] Splitem(string rawline)
        {
            string[] output = new string[11];
            string[] input = rawline.Split(',');
            // order
            // <TICKER>,<DTYYYYMMDD>,<OPEN>,<HIGH>,<LOW>,<CLOSE>,<VOL>,<NET>,<DEALSNUMBER>,<YESTERDAYPRICE>

            // date
            string year = input[1].Substring(0, 4);
            string month = input[1].Substring(4, 2);
            string day = input[1].Substring(6, 2);
            output[0] = year;
            output[1] = month;
            output[2] = day;

            // open
            output[3] = input[2];

            // high
            output[4] = input[3];

            // low
            output[5] = input[4];

            // close
            output[6] = input[5];

            //vol
            output[7] = input[6];

            // net value
            output[8] = input[7];

            // deals number
            output[9] = input[8];

            // yesterday price
            output[10] = input[9];


            return output;
        }


        public static void TempDataCollector()
        {
            //load html document 
            //var hdoc = new HtmlAgilityPack.HtmlDocument();
            //hdoc.Load(@"C:\Users\Nima\Desktop\linkchecker-out4.html");
            //var query = from table in hdoc.DocumentNode.SelectNodes("//table").Cast<HtmlNode>()
            //            from row in table.SelectNodes("tr").Cast<HtmlNode>()
            //            from cell in row.SelectNodes("th|td").Cast<HtmlNode>()
            //            select new { Table = table.Id, CellText = cell.InnerText, Name = table.ChildNodes[1].InnerText };

            XmlDocument doc = new XmlDocument();
            doc.Load(Constants.XmlMarketData);

            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            {
                //Category cat = new Category();

                //cat.Name = node.Attributes["name"]?.InnerText;
                //cat.FileName = node.Attributes["filename"]?.InnerText;
                //cat.Stocks = new List<Stock>();
                //market.Categories.Add(cat);

                foreach (XmlNode nd in node.ChildNodes)
                {

                    if (nd["tsetid"] == null)
                    {
                        XmlNode newNode = doc.CreateElement("tsetid");
                        nd.AppendChild(newNode);
                    }
                    if (nd["basevolume"] == null)
                    {
                        XmlNode newNode = doc.CreateElement("basevolume");
                        nd.AppendChild(newNode);
                    }
                    if (nd["outstandingshares"] == null)
                    {
                        XmlNode newNode = doc.CreateElement("outstandingshares");
                        nd.AppendChild(newNode);
                    }
                    if (nd["totalshares"] == null)
                    {
                        XmlNode newNode = doc.CreateElement("totalshares");
                        nd.AppendChild(newNode);
                    }

                    if (nd["eps"] == null)
                    {
                        XmlNode newNode = doc.CreateElement("eps");
                        nd.AppendChild(newNode);
                    }



                    if(!string.IsNullOrEmpty(nd["tsetid"].InnerText))
                    {
                        Task<string> task = Task<string>.Factory.StartNew(() =>
                        {
                            string s = string.Format(string.Format("http://www.tsetmc.com/Loader.aspx?ParTree=151311&i={0}", nd["tsetid"].InnerText));
                            WebClient wc = new WebClient();
                            Encoding encoding = Encoding.GetEncoding("windows-1251");
                            byte[] data = wc.DownloadData(s);
                            GZipStream gzip = new GZipStream(new MemoryStream(data), CompressionMode.Decompress);
                            MemoryStream decompressed = new MemoryStream();
                            gzip.CopyTo(decompressed);
                            string str = encoding.GetString(decompressed.GetBuffer(), 0, (int)decompressed.Length);


                            return str;
                        });

                        // total shares percent
                        var regex = new Regex(@"ZTitad=[0-9]*");
                        Match m = regex.Match(task.Result);
                        nd["totalshares"].InnerText = m.Value.Split('=')[1];


                        //basevol
                         regex = new Regex(@"BaseVol=[0-9]*");
                        m = regex.Match(task.Result);
                        nd["basevolume"].InnerText = m.Value.Split('=')[1];

                        if (Convert.ToInt32(nd["basevolume"].InnerText) > 1)
                        {
                            // outstanding shares percent
                            regex = new Regex(@"KAjCapValCpsIdx='[0-9]*'");
                            m = regex.Match(task.Result);
                            if (m.Value.Split('=')[1].Replace("\'", "") != "")
                                nd["outstandingshares"].InnerText =Math.Round(Convert.ToDouble((Convert.ToDouble(m.Value.Split('=')[1].Replace("\'", "")) * Convert.ToDouble(nd["totalshares"].InnerText) / 100)),0).ToString();
                        }
                        else
                        {
                            nd["outstandingshares"].InnerText = nd["totalshares"].InnerText;
                        }



                        // eps
                        regex = new Regex(@"EstimatedEPS='[-]*[0-9]*'");
                        m = regex.Match(task.Result);
                        nd["eps"].InnerText = m.Value.Split('=')[1].Replace("\'", "");


                        //return regex.Matches(strInput);


                        //Console.WriteLine(task.Result);
                    }

                    //try
                    //{
                    //    string n = nd.Attributes["name"].InnerText.Substring(0, nd.Attributes["name"].InnerText.Count() - 2);
                    //    string ababized = n.Replace('ی', 'ي');
                    //    string n2 = ababized.Replace('ک', 'ك');
                    //    //Console.WriteLine(query.FirstOrDefault(x => x.CellText.Contains(n2)).CellText);

                    //    var pair = query.Select((Value, Index) => new { Value, Index }).First(p => p.Value.CellText.Contains(n2));
                    //    string linee = query.ElementAt(pair.Index + 4).CellText;
                    //    string[] ps = linee.Split('=');
                    //    //Console.WriteLine(ps[2]);


                    //    if (string.IsNullOrEmpty(nd["tsetid"].InnerText))
                    //    {
                            
                    //        nd["tsetid"].InnerText = ps[2];
                    //    }

                    //}
                    //catch (Exception ex)
                    //{
                    //    Console.WriteLine(ex.Message.ToString());
                    //}



                }
                

               
            }
            
            doc.Save(Constants.XmlMarketData);
        }


    }
}
