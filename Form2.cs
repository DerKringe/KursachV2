using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KursachV2
{
    public partial class Form2 : Form
    {
        private NpgsqlConnection conn = new NpgsqlConnection("Server=172.20.7.9;Port=5432;Database=kp1095_01;User Id=st1095;Password=pwd1095;");

        public Form2()
        {
            InitializeComponent();
        }

        private void btnAuth_load_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textAuthLogin.Text)) { MessageBox.Show("Логин не указан"); return; }
            if (string.IsNullOrEmpty(textAuthPass.Text)) { MessageBox.Show("Пароль не указан"); return; }

            conn.Open();
            NpgsqlCommand cmd = new NpgsqlCommand("SELECT authentification(@login, @pass)", conn);
            cmd.Parameters.AddWithValue("login", NpgsqlTypes.NpgsqlDbType.Varchar, 20, textAuthLogin.Text);
            cmd.Parameters.AddWithValue("pass", NpgsqlTypes.NpgsqlDbType.Varchar, 120, textAuthPass.Text);

            object Timed = cmd.ExecuteScalar();
            int id_rank = Int32.Parse(Timed.ToString());
            if (id_rank == 0)
            {
                MessageBox.Show("Пользователя нет или неверный пароль" + id_rank);
            }
            else if (id_rank == 3)
            {
                Form1 MainForm = new Form1();
                MainForm.Show();
                this.Hide();
            }
            else 
            {
                Form1 MainForm = new Form1();
                MainForm.Show();
                this.Hide();
            }                
            conn.Close();
            SetValue(id_rank);
        }

        public void SetValue(int newValue)
        {
            DataTrans.Value = newValue;
        }

    }
}
