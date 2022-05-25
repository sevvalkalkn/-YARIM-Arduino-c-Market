using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO.Ports;

namespace market
{
    public partial class Form1 : Form




    {
        const string connStr = "Server = DESKTOP-GV6VG8L;Database=market;Uid=sa;Pwd=SQL";
        SqlConnection conn = new SqlConnection(connStr);
        string query = "Select * from tbUrunler";
        DataTable dtToplam = new DataTable();
        
        public Form1()
        {
            InitializeComponent();
        }
        OleDbConnection conn;
        OleDbCommand cmd;
        OleDbDataAdapter da;
        //Ürünleri listelemek için komut oluşturcaz.

        double toplam = 0;
            

        void urunlistele()
        {
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(query, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            conn.Close();

            //baglanti = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=|DataDirectory|\\Database7.mdb");
            //baglanti.Open();
            //da = new OleDbDataAdapter("SELECT *FROM urunler", baglanti);
            //DataTable tablo = new DataTable();
            //da.Fill(tablo);
            //dataGridView1.DataSource = tablo;
            //baglanti.Close();
        }
        //OleDbConnection conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=|DataDirectory|\\Database7.mdb");


        private void Form1_Load(object sender, EventArgs e)
        {
            dtToplam.Columns.Add("Ürün Adı");
            dtToplam.Columns.Add("Ürün Barkod");
            dtToplam.Columns.Add("Fiyat");
            //form yüklendiğinde metod çsğırdım
            urunlistele();

            string[] ports = SerialPort.GetPortNames();
            foreach (var port in ports)
            {
                comboBox1.Items.Add(port);
            }


        }
        public DataSet Database6 = new DataSet();
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        DataTable tablo = new DataTable();
        private void button1_Click(object sender, EventArgs e)
        {
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter("Select * from tbUrunler where UrunBarkod like '%" + txtBarkod.Text + "%'", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                dtToplam.Rows.Add(dt.Rows[0]["UrunAdi"], dt.Rows[0]["UrunBarkod"], dt.Rows[0]["Fiyat"]);
                toplam += Convert.ToInt32(dt.Rows[0]["Fiyat"]);
                dataGridView1.DataSource = dtToplam;
                
            }
            else
                MessageBox.Show("BARKOD BULUNAMADI");
            conn.Close();

            //baglanti.Open();
            //OleDbCommand komut = new OleDbCommand("Select *from urunler where barkod like '%" + txtBarkod.Text + "%'", baglanti);
            //OleDbDataAdapter da = new OleDbDataAdapter(komut);
            //DataSet ds = new DataSet();
            //da.Fill(ds);
            //dataGridView1.DataSource = ds.Tables[0];
            ////baglanti.Close();
            //string sorgu = "INSERT INTO urunler(Kimlik,barkod,urun_adi,urun_fiyat) values (@Kimlik,@barkod,@urun_adi,@urun_fiyat)";
            //komut = new OleDbCommand(sorgu, baglanti);
            //komut.Parameters.AddWithValue("@barkod", txtBarkod.Text);
            //komut.Parameters.AddWithValue("@urun_adi", txturun_adi.Text);
            //komut.Parameters.AddWithValue("@urun_fiyat", txturun_fiyat.Text);
            //baglanti.Close();



            //for (int i = 0; i < dataGridView1.Rows.Count; i++)
            //{
            //    toplam += Convert.ToDouble(dataGridView1.Rows[i].Cells[3].Value);
            //}
            label5.Text = "TOPLAM: " + toplam;


        }

        private void button2_Click(object sender, EventArgs e)
        {
            //string sorgu = "INSERT INTO urunler(barkod,urun_adi,urun_fiyat) values (@barkod,@urun_adi,@urun_fiyat)";
            //komut = new OleDbCommand(sorgu, baglanti);
            //komut.Parameters.AddWithValue("@barkod", txtBarkod.Text);
            //komut.Parameters.AddWithValue("@urun_adi", txturun_adi.Text);
            //komut.Parameters.AddWithValue("@urun_fiyat", txturun_fiyat.Text);
            //baglanti.Open();
            //komut.ExecuteNonQuery();
            //baglanti.Close();
            //urunlistele();
            string sorgu = "INSERT INTO tbUrunler(UrunBarkod,UrunAdi,Silindi,Fiyat) values (@UrunBarkod,@UrunAdi,@Silindi,@Fiyat)";
            SqlCommand cmd = new SqlCommand(sorgu, conn);
            cmd.Parameters.AddWithValue("@UrunBarkod", txtBarkod.Text);
            cmd.Parameters.AddWithValue("@UrunAdi", txturun_adi.Text);
            cmd.Parameters.AddWithValue("@Silindi", false);
            cmd.Parameters.AddWithValue("@Fiyat", Convert.ToInt32(txturun_fiyat.Text));
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            urunlistele();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int sonIndex = dtToplam.Rows.Count-1;
            //serialPort2.Write(dtToplam.Rows[sonIndex]["Ürün Adı"].ToString() + " " + dtToplam.Rows[sonIndex]["Fiyat"].ToString());
            if(serialPort2.IsOpen == false)
            {
                serialPort2.Open();
                serialPort2.Write("");

            }
            else
            {
                serialPort2.Write("1");
            }
        }
        
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            serialPort2.PortName = comboBox1.SelectedItem.ToString();
            serialPort2.BaudRate = 9600;
            serialPort2.Open();
        }
    }
}
    
