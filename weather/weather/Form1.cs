using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace weather
{
    public partial class Form1 : Form
    {
        Resources.DatabaseConnection objConnect;
        string conString;

        DataSet ds;
        DataRow dRow;

        int MaxRows = 30;

        public Form1()
        {
            InitializeComponent();
        }

        static string[] list = new string[] { };
        Dictionary<string, Label> citiesLabel = new Dictionary<string, Label>();

        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (Control c in Controls)
            {
                if (c.GetType() == typeof(Label) && c.Name.StartsWith("cityname"))
                {
                    string[] info = c.Name.Split('_');
                    try
                    {
                        objConnect = new Resources.DatabaseConnection();

                        conString = Properties.Settings.Default.WeatherConnectionString;

                        objConnect.connection_string = conString;

                        objConnect.Sql = Properties.Settings.Default.SQL;

                        ds = objConnect.GetConnection;

                        if (info.Length > 1)
                        {
                            citiesLabel.Add(info[1], c as Label);
                            c.Text = LoadCity(info[1]);

                            ds.Clear();

                            DataRow row = ds.Tables[0].NewRow();
                            row[0] = info[1];
                            row[1] = c.Text;
                            ds.Tables[0].Rows.Add(row);
                            

                            try
                            {
                                objConnect.UpdateDatabase(ds);
                                //MessageBox.Show("Database updated");
                            }
                            catch (Exception err)
                            {
                                MessageBox.Show(err.Message);
                            }
                        }
                    }
                    catch (Exception err)
                    {
                        objConnect = new Resources.DatabaseConnection();

                        conString = Properties.Settings.Default.WeatherConnectionString;

                        objConnect.connection_string = conString;

                        objConnect.Sql = Properties.Settings.Default.SQL;

                        ds = objConnect.GetConnection;

                        //MessageBox.Show(err.Message);
                        for (int i = 0; i < MaxRows; i++)
                        {
                            dRow = ds.Tables[0].Rows[i];
                            if (info.Length > 1 && info[1] == dRow.ItemArray.GetValue(0).ToString())
                            {
                                c.Text = dRow.ItemArray.GetValue(1).ToString();
                            }
                        }
                    }
                }
            }
        }
        
        private string LoadCity(string cityName)
        {
            cityName = cityName.ToLower();
            string url = string.Format("http://www.kazhydromet.kz/rss-pogoda.php?id={0}", cityName);
            XmlReader reader = XmlReader.Create(url);

            SyndicationFeed feed = SyndicationFeed.Load(reader);

            reader.Close();
            SyndicationItem item = feed.Items.ElementAt(0);
            string s = item.Summary.Text;
            s = s.Replace("<br>", "");
            s = s.Remove(42, 41);
            
            return s;
        }        
    }
}
