using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UpdateDatabase
{
    public partial class Form1 : Form
    {

        // link 
        string strcon = @"Data Source=DESKTOP-M4UKRGH\SQLEXPRESS01;Initial Catalog=QLNH;Integrated Security=True";
        SqlConnection sqlcon = null;

        private void Form1_Load(object sender, EventArgs e)
        {

            if (sqlcon == null)
            {
                sqlcon = new SqlConnection(strcon);
            }
            if (sqlcon.State != ConnectionState.Open)
            {
                sqlcon.Open();
                MessageBox.Show("Thanh cong");

            }
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.CommandType = CommandType.Text;
            sqlcmd.CommandText = "select * from NhomMonAn1";

            sqlcmd.Connection = sqlcon;

            SqlDataReader reader = sqlcmd.ExecuteReader();
            while (reader.Read())
            {
                string IdNhomMonAn = reader.GetString(0);
                string TenNhomMonAn = reader.GetString(1);

                string line = IdNhomMonAn.Trim() + "/" + TenNhomMonAn;
                comboBox1.Items.Add(line);


            }
            reader.Close();
        }



        public Form1()
        {
            InitializeComponent();


        }



        // khong dung parameter 

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // nhom1/khaivi 
            if (comboBox1.SelectedIndex == -1)
            {
                return;
            }
            string aa = comboBox1.SelectedItem.ToString();
            string[] mang = aa.Split('/');
            string dau = mang[0].Trim();
            // nhom01
            // muon hien thi trogn listview;
            hienthithongtin(dau);
        }
        private void hienthithongtin(string manhom)
        {

            if (sqlcon == null)
            {
                sqlcon = new SqlConnection(strcon);
            }
            if (sqlcon.State != ConnectionState.Open)
            {
                sqlcon.Open();
                MessageBox.Show("Thanh cong");

            }
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.CommandType = CommandType.Text;
            sqlcmd.CommandText = "select * from MonAn1 Where IdNhomMonAn = '"+manhom+"'";

            sqlcmd.Connection = sqlcon;
            listView1.Items.Clear();
            SqlDataReader reader = sqlcmd.ExecuteReader();
            while (reader.Read())
            {
                string idmonan = reader.GetString(0);
                string idnhommonan = reader.GetString(1);
                string namemonan = reader.GetString(2);
                string donvitinh = reader.GetString(3);
                float dongia = reader.GetFloat(4);
                // tao ra  hang 
                ListViewItem hang = new ListViewItem(idmonan);
                hang.SubItems.Add(idnhommonan);
                hang.SubItems.Add(namemonan);
                hang.SubItems.Add(donvitinh);
                hang.SubItems.Add(dongia.ToString());
                listView1.Items.Add(hang);

            }
            reader.Close();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // lay ra hàng chỉ định 
            if(listView1.SelectedItems.Count > 0)
            {
                // lay ra hang do 
                ListViewItem hang = listView1.SelectedItems[0];
                textBox2.Text = hang.SubItems[0].Text.Trim();
                textBox3.Text = hang.SubItems[1].Text.Trim();
                textBox4.Text = hang.SubItems[2].Text.Trim();
                textBox5.Text = hang.SubItems[4].Text.Trim();
                textDVT.Text = hang.SubItems[3].Text;


            }
        }

        // dung parameter 

        private void button1_Click(object sender, EventArgs e)
        {
            if (sqlcon == null)
            {
                sqlcon = new SqlConnection(strcon);

            }
            if (sqlcon.State != ConnectionState.Open)
            {
                sqlcon.Open();

            }
            //
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.CommandType = CommandType.Text;
            sqlcmd.CommandText = "update MonAn1 set IdMonAn=@monan,IdNhomMonAn=@nhommonan,NameMonAn =@tenmon,DonViTinh=@donvi,DonGia=@dongia where IdMonAn=@monan";

            // dinh nghia parameter 
            SqlParameter MA= new SqlParameter("monan", SqlDbType.VarChar);
            SqlParameter NhomMA = new SqlParameter("nhommonan", SqlDbType.VarChar);
            SqlParameter NameM = new SqlParameter("tenmon", SqlDbType.VarChar);
            SqlParameter DonVi1 = new SqlParameter("donvi", SqlDbType.VarChar);
            SqlParameter DonGia1 = new SqlParameter("dongia", SqlDbType.Real);

            MA.Value = textBox2.Text.Trim();
            NhomMA.Value = textBox3.Text.Trim();
            NameM.Value = textBox4.Text.Trim();
            DonVi1.Value = textDVT.Text.Trim();
            DonGia1.Value = textBox5.Text.Trim();

            // dua vao sql 
            sqlcmd.Parameters.Add(MA);
            sqlcmd.Parameters.Add(NhomMA);
            sqlcmd.Parameters.Add(NameM);
            sqlcmd.Parameters.Add(DonVi1);
            sqlcmd.Parameters.Add(DonGia1);

            //ket noi
            sqlcmd.Connection = sqlcon;
            int kq = sqlcmd.ExecuteNonQuery();

            if (kq > 0)
            {
                hienthithongtin(textBox3.Text.Trim());
            }
            else
            {
                MessageBox.Show("ko đc ");
            }

        }

    }
}
