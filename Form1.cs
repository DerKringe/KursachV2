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
    public partial class Form1 : Form
    {

        //private string connstring = "Server=172.20.7.9;Port=5432;Database=kp1095_01;User Id=st1095;Password=pwd1095;";
        //private NpgsqlConnection conn = new NpgsqlConnection(connstring);

        private NpgsqlConnection conn = new NpgsqlConnection("Server=172.20.7.9;Port=5432;Database=kp1095_01;User Id=st1095;Password=pwd1095;");


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_load(object sender, EventArgs e)
        {
            //conn = new NpgsqlConnection(connstring);

            //conn.Open();
        }

        //NEZROBIT
        private void button1_Click(object sender, EventArgs e)
        {
            conn.Open();
            NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter("SELECT * FROM categories", conn);

            DataSet db = new DataSet();
            dataAdapter.Fill(db);
            dataGridView2.DataSource = db.Tables[0];

            conn.Close();
            //MessageBox.Show("OK");

        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            conn.Open();

            if (true)
            {
                textCategories1.Text = dataGridView2.Rows[e.RowIndex].Cells["n_category"].Value.ToString();
                textCategories2.Text = dataGridView2.Rows[e.RowIndex].Cells["desc_category"].Value.ToString();
            }
            else
            {
                //MessageBox.Show("Что то не так");
            }
            conn.Close();
        }

        private void dataGridProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            conn.Open();

            if (e.RowIndex >= 0)
            {
                textProducts1.Text = dataGridProducts.Rows[e.RowIndex].Cells["id_category"].Value.ToString();
                textProducts2.Text = dataGridProducts.Rows[e.RowIndex].Cells["n_product"].Value.ToString();
                textProducts3.Text = dataGridProducts.Rows[e.RowIndex].Cells["price"].Value.ToString();
                textProducts4.Text = dataGridProducts.Rows[e.RowIndex].Cells["desc_product"].Value.ToString();
            }
            else
            {
                // MessageBox.Show("Что то не так");
            }
            conn.Close();
        }

        // ------------------------------------------------------------------------------------------ PRODUCT_BTN

        private void btnProducts_load_Click(object sender, EventArgs e)
        {

            conn.Open();
            NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter("SELECT * FROM products", conn);

            DataSet db = new DataSet();
            dataAdapter.Fill(db);
            dataGridProducts.DataSource = db.Tables[0];
            conn.Close();
            //MessageBox.Show("OK");

        }

        private void btnProduct_Insert_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textProducts3.Text)) { MessageBox.Show("Цена не указана"); return; }
            conn.Open();

            try
            {
                int Timed_int = Int32.Parse(textProducts1.Text);
                decimal price = Decimal.Parse(textProducts3.Text);

                NpgsqlCommand cmd = new NpgsqlCommand("SELECT products_insert(@id_category, @n_product, @price, @desc_product)", conn);
                cmd.Parameters.AddWithValue("id_category", Timed_int);
                cmd.Parameters.AddWithValue("n_product", NpgsqlTypes.NpgsqlDbType.Varchar, 20, textProducts2.Text);
                cmd.Parameters.AddWithValue("price", price);
                cmd.Parameters.AddWithValue("desc_product", NpgsqlTypes.NpgsqlDbType.Varchar, 120, textProducts4.Text);


                cmd.ExecuteNonQuery();


                conn.Close();
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show("Произошла ошибка SQL: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Произошла ошибка при работе с базой данных: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
            }
        
        }

        private void btnProduct_Update_Click(object sender, EventArgs e)
        {
            conn.Open();

            try
            {
                int Timed_int = Int32.Parse(textProducts1.Text);        
                decimal price = Decimal.Parse(textProducts3.Text);

                DataGridViewRow row = dataGridProducts.Rows[dataGridProducts.CurrentCell.RowIndex];

                NpgsqlCommand cmd = new NpgsqlCommand("SELECT products_update(@id_product, @id_category, @n_product, @price, @desc_product)", conn);
                cmd.Parameters.AddWithValue("id_product", Int32.Parse(row.Cells["id_product"].Value.ToString()));
                cmd.Parameters.AddWithValue("id_category", Timed_int);
                cmd.Parameters.AddWithValue("n_product", NpgsqlTypes.NpgsqlDbType.Varchar, 20, textProducts2.Text);
                cmd.Parameters.AddWithValue("price", price);
                cmd.Parameters.AddWithValue("desc_product", NpgsqlTypes.NpgsqlDbType.Varchar, 120, textProducts4.Text);

                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show("Произошла ошибка SQL: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Произошла ошибка при работе с базой данных: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
            }


          
        }

        private void btnProduct_Delete_Click(object sender, EventArgs e)
        {
            conn.Open();
            DataGridViewRow row = dataGridProducts.Rows[dataGridProducts.CurrentCell.RowIndex];
            NpgsqlCommand cmd = new NpgsqlCommand("SELECT products_delete(@id_product)", conn);
            cmd.Parameters.AddWithValue("id_product", Int32.Parse(row.Cells["id_product"].Value.ToString()));

            cmd.ExecuteNonQuery();
            conn.Close();
        }

        // ------------------------------------------------------------------------------------------ LOAD_BTN

        private void btnStorages_load_Click(object sender, EventArgs e)
        {
            conn.Open();
            //NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter("SELECT * FROM storages", conn);
            NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter("SELECT* FROM product_storage_view;", conn);

            DataSet db = new DataSet();
            dataAdapter.Fill(db);
            dataGridStorages.DataSource = db.Tables[0];

            conn.Close();
            //MessageBox.Show("OK");
        }

        private void btnStoragesInfo_load_Click(object sender, EventArgs e)
        {
            conn.Open();
            NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter("SELECT * FROM storagesinfo", conn);

            DataSet db = new DataSet();
            dataAdapter.Fill(db);
            dataGridStoragesInfo.DataSource = db.Tables[0];

            conn.Close();
            //MessageBox.Show("OK");
        }

        private void btnProviders_load_Click(object sender, EventArgs e)
        {
            conn.Open();
            NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter("SELECT * FROM providers", conn);

            DataSet db = new DataSet();
            dataAdapter.Fill(db);
            dataGridProviders.DataSource = db.Tables[0];

            conn.Close();
            //MessageBox.Show("OK");
        }

        private void btnCustomers_load_Click(object sender, EventArgs e)
        {
            conn.Open();
            NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter("SELECT * FROM customers", conn);

            DataSet db = new DataSet();
            dataAdapter.Fill(db);
            dataGridCustomers.DataSource = db.Tables[0];

            conn.Close();
            //MessageBox.Show("OK");
        }

        private void btnEmployee_load_Click(object sender, EventArgs e)
        {
            conn.Open();
            NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter("SELECT * FROM employees", conn);

            DataSet db = new DataSet();
            dataAdapter.Fill(db);
            dataGridEmployees.DataSource = db.Tables[0];

            conn.Close();
            //MessageBox.Show("OK");
        }

        private void btnUsers_load_Click(object sender, EventArgs e)
        {
            conn.Open();
            NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter("SELECT * FROM users", conn);

            DataSet db = new DataSet();
            dataAdapter.Fill(db);
            dataGridUsers.DataSource = db.Tables[0];

            conn.Close();
            //MessageBox.Show("OK");
        }

        private void btnRanks_load_Click(object sender, EventArgs e)
        {
            conn.Open();
            NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter("SELECT * FROM ranks", conn);

            DataSet db = new DataSet();
            dataAdapter.Fill(db);
            dataGridRanks.DataSource = db.Tables[0];

            conn.Close();
            //MessageBox.Show("OK");
        }

        // ---------------------------------------------------------------------------------------- OPER_BTN

        private void btnPostavka_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textPostavka21.Text))
            { if (string.IsNullOrEmpty(textPostavka22.Text)) { MessageBox.Show("Количество не заполнено"); return; } }
            if (!string.IsNullOrEmpty(textPostavka31.Text))
            { if (string.IsNullOrEmpty(textPostavka32.Text)) { MessageBox.Show("Количество не заполнено"); return; } }
            if (!string.IsNullOrEmpty(textPostavka41.Text))
            { if (string.IsNullOrEmpty(textPostavka42.Text)) { MessageBox.Show("Количество не заполнено"); return; } }

            if (!string.IsNullOrEmpty(textPostavka22.Text))
            { if (string.IsNullOrEmpty(textPostavka21.Text)) { MessageBox.Show("Товар не заполнен"); return; } }
            if (!string.IsNullOrEmpty(textPostavka32.Text))
            { if (string.IsNullOrEmpty(textPostavka31.Text)) { MessageBox.Show("Товар не заполнен"); return; } }
            if (!string.IsNullOrEmpty(textPostavka42.Text))
            { if (string.IsNullOrEmpty(textPostavka41.Text)) { MessageBox.Show("Товар не заполнен"); return; } }

            conn.Open();

            int counter = 0;
            int Timed_int5 = Int32.Parse(textPostavka5.Text);
            //NpgsqlCommand cmd = new NpgsqlCommand("SELECT postavka(@n_provider, @id_product, @quantity, @id_storage, @counter)", conn);

            if (!string.IsNullOrEmpty(textPostavka21.Text))
            {
                int Timed_int = Int32.Parse(textPostavka21.Text);
                int Timed_int2 = Int32.Parse(textPostavka22.Text);
                object result;
                counter += 1;


                NpgsqlCommand cmd = new NpgsqlCommand("SELECT postavka(@n_provider, @id_product, @quantity, @id_storage, @counter)", conn);

                cmd.Parameters.AddWithValue("n_provider", NpgsqlTypes.NpgsqlDbType.Varchar, 120, textPostavka1.Text);
                cmd.Parameters.AddWithValue("id_product", Timed_int);
                cmd.Parameters.AddWithValue("quantity", Timed_int2);
                cmd.Parameters.AddWithValue("id_storage", Timed_int5);
                cmd.Parameters.AddWithValue("counter", counter);

                result = cmd.ExecuteScalar();
                //MessageBox.Show(result.ToString());
                //MessageBox.Show("1");
            };


            if (!string.IsNullOrEmpty(textPostavka31.Text))
            {
                int Timed_int = Int32.Parse(textPostavka31.Text);
                int Timed_int2 = Int32.Parse(textPostavka32.Text);
                object result;
                counter += 1;

                NpgsqlCommand cmd = new NpgsqlCommand("SELECT postavka(@n_provider, @id_product, @quantity, @id_storage, @counter)", conn);

                cmd.Parameters.AddWithValue("n_provider", NpgsqlTypes.NpgsqlDbType.Varchar, 120, textPostavka1.Text);
                cmd.Parameters.AddWithValue("id_product", Timed_int);
                cmd.Parameters.AddWithValue("quantity", Timed_int2);
                cmd.Parameters.AddWithValue("id_storage", Timed_int5);
                cmd.Parameters.AddWithValue("counter", counter);

                result = cmd.ExecuteScalar();
                //MessageBox.Show(result.ToString());
                //MessageBox.Show("2");
            };


            if (!string.IsNullOrEmpty(textPostavka41.Text))
            {
                int Timed_int = Int32.Parse(textPostavka41.Text);
                int Timed_int2 = Int32.Parse(textPostavka42.Text);
                object result;
                counter += 1;

                NpgsqlCommand cmd = new NpgsqlCommand("SELECT postavka(@n_provider, @id_product, @quantity, @id_storage, @counter)", conn);

                cmd.Parameters.AddWithValue("n_provider", NpgsqlTypes.NpgsqlDbType.Varchar, 120, textPostavka1.Text);
                cmd.Parameters.AddWithValue("id_product", Timed_int);
                cmd.Parameters.AddWithValue("quantity", Timed_int2);
                cmd.Parameters.AddWithValue("id_storage", Timed_int5);
                cmd.Parameters.AddWithValue("counter", counter);

                result = cmd.ExecuteScalar();
                //MessageBox.Show(result.ToString());
                //MessageBox.Show("3");
            };

            textPostavka1.Clear(); textPostavka5.Clear();
            textPostavka21.Clear(); textPostavka22.Clear();
            textPostavka31.Clear(); textPostavka32.Clear();
            textPostavka41.Clear(); textPostavka42.Clear();
            conn.Close();
        }

        private void btnTransport_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textTransport21.Text))
            { if (string.IsNullOrEmpty(textTransport22.Text)) { MessageBox.Show("Количество не заполнено"); return; } }
            if (!string.IsNullOrEmpty(textTransport22.Text))
            { if (string.IsNullOrEmpty(textTransport21.Text)) { MessageBox.Show("Товар не указан"); return; } }

            if (string.IsNullOrEmpty(textTransport1.Text))
            {
                MessageBox.Show("Не указан склад (from)"); return;
            }
            if (string.IsNullOrEmpty(textTransport3.Text))
            {
                MessageBox.Show("Не указан склад (to)"); return;
            }

            conn.Open();
            int Timed_int = Int32.Parse(textTransport1.Text);
            int Timed_int2 = Int32.Parse(textTransport3.Text);
            int Timed_int3 = Int32.Parse(textTransport21.Text);
            int Timed_int4 = Int32.Parse(textTransport22.Text);

            NpgsqlCommand cmd = new NpgsqlCommand("SELECT transport(@from_id_storage, @to_id_storage, @id_product, @quantity)", conn);

            cmd.Parameters.AddWithValue("from_id_storage", Timed_int);
            cmd.Parameters.AddWithValue("to_id_storage", Timed_int2);
            cmd.Parameters.AddWithValue("id_product", Timed_int3);
            cmd.Parameters.AddWithValue("quantity", Timed_int4);

            //cmd.ExecuteNonQuery();
            object result = cmd.ExecuteScalar();
            MessageBox.Show(result.ToString());
            conn.Close();
        }

        private void btnTrade_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textTrade31.Text))
            { if (string.IsNullOrEmpty(textTrade32.Text)) { MessageBox.Show("Количество не заполнено"); return; } }
            if (!string.IsNullOrEmpty(textTrade41.Text))
            { if (string.IsNullOrEmpty(textTrade42.Text)) { MessageBox.Show("Количество не заполнено"); return; } }
            if (!string.IsNullOrEmpty(textTrade51.Text))
            { if (string.IsNullOrEmpty(textTrade52.Text)) { MessageBox.Show("Количество не заполнено"); return; } }

            if (!string.IsNullOrEmpty(textTrade32.Text))
            { if (string.IsNullOrEmpty(textTrade31.Text)) { MessageBox.Show("Товар не заполнено"); return; } }
            if (!string.IsNullOrEmpty(textTrade42.Text))
            { if (string.IsNullOrEmpty(textTrade41.Text)) { MessageBox.Show("Товар не заполнено"); return; } }
            if (!string.IsNullOrEmpty(textTrade52.Text))
            { if (string.IsNullOrEmpty(textTrade51.Text)) { MessageBox.Show("Товар не заполнено"); return; } }

            if (string.IsNullOrEmpty(textTrade1.Text)) { MessageBox.Show("Имя не заполнено"); return; }
            if (string.IsNullOrEmpty(textTrade2.Text)) { MessageBox.Show("Фамилия не заполнено"); return; }
            if (string.IsNullOrEmpty(textTrade6.Text)) { MessageBox.Show("ID работника не заполнен"); return; }


            conn.Open();
            int Timed_int3 = Int32.Parse(textTrade6.Text);
            int counter = 0;

            if (!string.IsNullOrEmpty(textTrade41.Text))
            {
                int Timed_int = Int32.Parse(textTrade41.Text);
                int Timed_int2 = Int32.Parse(textTrade42.Text);

                counter += 1;

                NpgsqlCommand cmd = new NpgsqlCommand("SELECT trade(@fn_customer, @sn_customer, @id_employee, @id_product, @quantity, @counter)", conn);

                cmd.Parameters.AddWithValue("fn_customer", NpgsqlTypes.NpgsqlDbType.Varchar, 20, textTrade1.Text);
                cmd.Parameters.AddWithValue("sn_customer", NpgsqlTypes.NpgsqlDbType.Varchar, 20, textTrade2.Text);
                cmd.Parameters.AddWithValue("id_employee", Timed_int3);
                cmd.Parameters.AddWithValue("id_product", Timed_int);
                cmd.Parameters.AddWithValue("quantity", Timed_int2);
                cmd.Parameters.AddWithValue("counter", counter);

                //cmd.ExecuteNonQuery();
                object result = cmd.ExecuteScalar();
                MessageBox.Show(result.ToString());
            }

            if (!string.IsNullOrEmpty(textTrade31.Text))
            {
                int Timed_int = Int32.Parse(textTrade31.Text);
                int Timed_int2 = Int32.Parse(textTrade32.Text);

                counter += 1;

                NpgsqlCommand cmd = new NpgsqlCommand("SELECT trade(@fn_customer, @sn_customer, @id_employee, @id_product, @quantity, @counter)", conn);

                cmd.Parameters.AddWithValue("fn_customer", NpgsqlTypes.NpgsqlDbType.Varchar, 20, textTrade1.Text);
                cmd.Parameters.AddWithValue("sn_customer", NpgsqlTypes.NpgsqlDbType.Varchar, 20, textTrade2.Text);
                cmd.Parameters.AddWithValue("id_employee", Timed_int3);
                cmd.Parameters.AddWithValue("id_product", Timed_int);
                cmd.Parameters.AddWithValue("quantity", Timed_int2);
                cmd.Parameters.AddWithValue("counter", counter);

                //cmd.ExecuteNonQuery();
                object result = cmd.ExecuteScalar();
                MessageBox.Show(result.ToString());
            }

            if (!string.IsNullOrEmpty(textTrade51.Text))
            {
                int Timed_int = Int32.Parse(textTrade51.Text);
                int Timed_int2 = Int32.Parse(textTrade52.Text);

                counter += 1;

                NpgsqlCommand cmd = new NpgsqlCommand("SELECT trade(@fn_customer, @sn_customer, @id_employee, @id_product, @quantity, @counter)", conn);

                cmd.Parameters.AddWithValue("fn_customer", NpgsqlTypes.NpgsqlDbType.Varchar, 20, textTrade1.Text);
                cmd.Parameters.AddWithValue("sn_customer", NpgsqlTypes.NpgsqlDbType.Varchar, 20, textTrade2.Text);
                cmd.Parameters.AddWithValue("id_employee", Timed_int3);
                cmd.Parameters.AddWithValue("id_product", Timed_int);
                cmd.Parameters.AddWithValue("quantity", Timed_int2);
                cmd.Parameters.AddWithValue("counter", counter);

                //cmd.ExecuteNonQuery();
                object result = cmd.ExecuteScalar();
                MessageBox.Show(result.ToString());
            }
            conn.Close();
        }

        // -----------------------------------------------------------------

        private void button1_Click_1(object sender, EventArgs e)
        {
            conn.Open();
            NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter("SELECT * FROM categories", conn);

            DataSet db = new DataSet();
            dataAdapter.Fill(db);
            dataGridView2.DataSource = db.Tables[0];

            conn.Close();
            //MessageBox.Show("OK");
        }

        // ------------------------------------------------------------------------------------- UNIVERSAL_LOAD_BTN

        private void btnMovings_load_Click(object sender, EventArgs e)
        {
            conn.Open();
            NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter("SELECT * FROM movings", conn);

            DataSet db = new DataSet();
            dataAdapter.Fill(db);
            dataGridUniversal.DataSource = db.Tables[0];

            conn.Close();
            //MessageBox.Show("OK");
        }

        private void btnMovingCart_load_Click(object sender, EventArgs e)
        {
            conn.Open();
            NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter("SELECT * FROM movingcart", conn);

            DataSet db = new DataSet();
            dataAdapter.Fill(db);
            dataGridUniversal.DataSource = db.Tables[0];

            conn.Close();
            //MessageBox.Show("OK");
        }

        private void btnSupplies_load_Click(object sender, EventArgs e)
        {
            conn.Open();
            NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter("SELECT * FROM supplies", conn);

            DataSet db = new DataSet();
            dataAdapter.Fill(db);
            dataGridUniversal.DataSource = db.Tables[0];

            conn.Close();
            //MessageBox.Show("OK");
        }

        private void btnSupplyCart_load_Click(object sender, EventArgs e)
        {
            conn.Open();
            NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter("SELECT * FROM supplycart", conn);

            DataSet db = new DataSet();
            dataAdapter.Fill(db);
            dataGridUniversal.DataSource = db.Tables[0];

            conn.Close();
            //MessageBox.Show("OK");
        }

        private void btnOrders_load_Click(object sender, EventArgs e)
        {
            conn.Open();
            NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter("SELECT * FROM orders", conn);

            DataSet db = new DataSet();
            dataAdapter.Fill(db);
            dataGridUniversal.DataSource = db.Tables[0];

            conn.Close();
            //MessageBox.Show("OK");
        }

        private void btnOrderCart_load_Click(object sender, EventArgs e)
        {
            conn.Open();
            NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter("SELECT * FROM ordercart", conn);

            DataSet db = new DataSet();
            dataAdapter.Fill(db);
            dataGridUniversal.DataSource = db.Tables[0];

            conn.Close();
            //MessageBox.Show("OK");
        }

        // ---------------------------------------------------------------------------------------------- OTCHET_BTN

        private void btnSumm_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(textSumm1.Text)) { MessageBox.Show("Дата ОТ не заполнена"); return; }
            if (string.IsNullOrEmpty(textSumm2.Text)) { MessageBox.Show("Дата ДО не заполнена"); return; }

            DateTime dDate;
            if (DateTime.TryParse(textSumm1.Text, out dDate))
            {
                String.Format("{0:yyyy-MM-d}", dDate);
            }
            else
            {
                MessageBox.Show("Неверный формат");
                return;
            }

            if (DateTime.TryParse(textSumm2.Text, out dDate))
            {
                String.Format("{0:yyyy-MM-d}", dDate);
            }
            else
            {
                MessageBox.Show("Неверный формат");
                return;
            }

            conn.Open();

            NpgsqlCommand cmd = new NpgsqlCommand("SELECT get_sales_sum(@start_date, @end_date)", conn);

            DateTime startDate = DateTime.Parse(textSumm1.Text);
            DateTime endDate = DateTime.Parse(textSumm2.Text);

            cmd.Parameters.AddWithValue("start_date", NpgsqlTypes.NpgsqlDbType.Date, startDate);
            cmd.Parameters.AddWithValue("end_date", NpgsqlTypes.NpgsqlDbType.Date, endDate);

            object summ = cmd.ExecuteScalar();
            Summ.Text = Convert.ToString(summ);
            Summ.Text += " рублей";
            conn.Close();

        }

        private void btnNalichie_Click(object sender, EventArgs e)
        {
            NalichieStorage1.Text = "0";
            NalichieStorage2.Text = "0";
            NalichieStorage3.Text = "0";
            NalichieStorageSumm.Text = "0";
            NalichieName.Text = " ";

            if (string.IsNullOrEmpty(textNalichie1.Text)) { MessageBox.Show("ID товара не заполнен"); return; }

            conn.Open();

            int Timed_int = Int32.Parse(textNalichie1.Text);
            var totalQuantity = 0;

            NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM check_product_quantity(@id_product)", conn);
            cmd.Parameters.AddWithValue("id_product", Timed_int);

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {

                    if (!reader.HasRows)
                    {
                        MessageBox.Show("Такого товара ");
                        return;
                    }

                    var storageId = reader.GetFieldValue<int>(0);
                    var quantity = reader.GetFieldValue<int>(1);
                    totalQuantity = reader.GetFieldValue<int>(2);

                    if (storageId == 3)
                    {
                        NalichieStorage1.Text = Convert.ToString(quantity);
                    }
                    else if (storageId == 4)
                    {
                        NalichieStorage2.Text = Convert.ToString(quantity);
                    }
                    else if (storageId == 5)
                    {
                        NalichieStorage3.Text = Convert.ToString(quantity);
                    }
                    //MessageBox.Show("Номер склада:" + storageId.ToString() + "  ; Наличие: "+ quantity.ToString() );
                }

                //MessageBox.Show(totalQuantity.ToString());
                NalichieStorageSumm.Text = Convert.ToString(totalQuantity);

            }
            NpgsqlCommand cmd2 = new NpgsqlCommand("SELECT n_product FROM Products WHERE id_product = @product_id;", conn);
            cmd2.Parameters.AddWithValue("product_id", Timed_int);

            object name = cmd2.ExecuteScalar();
            NalichieName.Text = Convert.ToString(name);

            conn.Close();

        }

        private void btnTradeChek_load1_Click(object sender, EventArgs e)
        {
            TradeChekSumm1.Text = " ";
            TradeChekDate1.Text = " ";
            TradeChekCustomer1.Text = " ";
            TradeChekEmployee1.Text = " ";

            conn.Open();
            NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter("SELECT * FROM get_product_totals_by_last_order_id()", conn);

            DataSet db = new DataSet();
            dataAdapter.Fill(db);
            dataGridTradeChek1.DataSource = db.Tables[0];

            NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM get_last_order_totals()", conn);
            var id_employee = 0;
            var id_customer = 0;

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    if (!reader.HasRows)
                    {
                        MessageBox.Show("Такого товара ");
                        return;
                    }
                    var data = reader.GetFieldValue<DateTime>(0);
                    var totalprice = reader.GetFieldValue<float>(1);
                    id_employee = reader.GetFieldValue<int>(2);
                    id_customer = reader.GetFieldValue<int>(3);

                    TradeChekSumm1.Text = "Всего: ";
                    TradeChekSumm1.Text += Convert.ToString(totalprice);
                    TradeChekSumm1.Text += " рублей";

                    TradeChekDate1.Text = Convert.ToString(data);
                    //MessageBox.Show("Номер склада:" + storageId.ToString() + "  ; Наличие: "+ quantity.ToString() );
                }
                conn.Close();

                conn.Open();


                NpgsqlCommand cmd2 = new NpgsqlCommand("SELECT concat(fn_employee, ' ', sn_employee) as full_name FROM employees WHERE id_employee = id_employee;", conn);
                cmd2.Parameters.AddWithValue("id_employee", id_employee);

                object name2 = cmd2.ExecuteScalar();
                TradeChekEmployee1.Text = "Продавец: ";
                TradeChekEmployee1.Text += Convert.ToString(name2);

                NpgsqlCommand cmd3 = new NpgsqlCommand("SELECT concat(fn_customer, ' ', sn_customer) as full_name FROM customers WHERE id_customer = @id_customer;", conn);
                cmd3.Parameters.AddWithValue("id_customer", id_customer);

                object name3 = cmd3.ExecuteScalar();
                TradeChekCustomer1.Text = "Покупатель: ";
                TradeChekCustomer1.Text += Convert.ToString(name3);

                conn.Close();
                //MessageBox.Show("OK");
            }


        }

        private void btnTradeChek_load2_Click(object sender, EventArgs e)
        {
            TradeChekSumm2.Text = " ";
            TradeChekDate2.Text = " ";
            TradeChekCustomer2.Text = " ";
            TradeChekEmployee2.Text = " ";
            var id_employee = 0;
            var id_customer = 0;

            if (string.IsNullOrEmpty(textTradeChek1.Text)) { MessageBox.Show("ID заказа не заполнен"); return; }

            conn.Open();

            int id_order = Int32.Parse(textTradeChek1.Text);

            NpgsqlCommand cmd0 = new NpgsqlCommand("SELECT * FROM get_product_totals_by_order_id(@id_order)", conn);
            cmd0.Parameters.AddWithValue("id_order", id_order);

            NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(cmd0);
            DataSet db = new DataSet();
            dataAdapter.Fill(db);
            dataGridTradeChek2.DataSource = db.Tables[0];

            
            NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM get_order_totals_by_id(@id_order)", conn);
            cmd.Parameters.AddWithValue("id_order", id_order);


            using (var reader2 = cmd.ExecuteReader())
            {
                while (reader2.Read())
                {

                    if (!reader2.HasRows)
                    {
                        MessageBox.Show("Такого товара ");
                        return;
                    }

                    var data = reader2.GetFieldValue<DateTime>(0);
                    var totalprice = reader2.GetFieldValue<float>(1);
                    id_employee = reader2.GetFieldValue<int>(2);
                    id_customer = reader2.GetFieldValue<int>(3);
                    TradeChekSumm2.Text = "Всего: ";
                    TradeChekSumm2.Text += Convert.ToString(totalprice);
                    TradeChekSumm2.Text += " рублей";

                    TradeChekDate2.Text = Convert.ToString(data);

                    //MessageBox.Show("Номер склада:" + storageId.ToString() + "  ; Наличие: "+ quantity.ToString() );
                }
                conn.Close();

                conn.Open();


                NpgsqlCommand cmd2 = new NpgsqlCommand("SELECT concat(fn_employee, ' ', sn_employee) as full_name FROM employees WHERE id_employee = @id_employee;", conn);
                cmd2.Parameters.AddWithValue("id_employee", id_employee);

                object name2 = cmd2.ExecuteScalar();
                TradeChekEmployee2.Text = "Продавец: ";
                TradeChekEmployee2.Text += Convert.ToString(name2);

                NpgsqlCommand cmd3 = new NpgsqlCommand("SELECT concat(fn_customer, ' ', sn_customer) as full_name FROM customers WHERE id_customer = @id_customer;", conn);
                cmd3.Parameters.AddWithValue("id_customer", id_customer);

                object name3 = cmd3.ExecuteScalar();
                TradeChekCustomer2.Text = "Покупатель: ";
                TradeChekCustomer2.Text += Convert.ToString(name3);

                conn.Close();
                //MessageBox.Show("OK");
            
            }
            
        }

        private void btnNaklad_load1_Click(object sender, EventArgs e)
        {
            TradeChekSumm1.Text = " ";
            TradeChekDate1.Text = " ";
            TradeChekCustomer1.Text = " ";
            TradeChekEmployee1.Text = " ";

            conn.Open();
            NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter("SELECT * FROM get_product_totals_by_last_supply_id()", conn);

            DataSet db = new DataSet();
            dataAdapter.Fill(db);
            dataGridNaklad1.DataSource = db.Tables[0];


            NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM get_last_supply_totals()", conn);
            var id_provider = 0;


            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {

                    if (!reader.HasRows)
                    {
                        MessageBox.Show("Такого товара ");
                        return;
                    }

                    var data = reader.GetFieldValue<DateTime>(0);
                    var totalprice = reader.GetFieldValue<float>(1);
                    id_provider = reader.GetFieldValue<int>(2);


                    NakladSumm1.Text = "Всего: ";
                    NakladSumm1.Text += Convert.ToString(totalprice);
                    NakladSumm1.Text += " рублей";

                    NakladDate1.Text = Convert.ToString(data);

                    //MessageBox.Show("Номер склада:" + storageId.ToString() + "  ; Наличие: "+ quantity.ToString() );
                }
                conn.Close();

                conn.Open();


                NpgsqlCommand cmd2 = new NpgsqlCommand("SELECT n_provider FROM providers WHERE id_provider = @id_provider;", conn);
                cmd2.Parameters.AddWithValue("id_provider", id_provider);

                object name2 = cmd2.ExecuteScalar();
                NakladProvider1.Text = "Поставщик: ";
                NakladProvider1.Text += Convert.ToString(name2);

                conn.Close();
                //MessageBox.Show("OK");
            }

        }

        private void btnNaklad_load2_Click(object sender, EventArgs e)
        {
            TradeChekSumm2.Text = " ";
            TradeChekDate2.Text = " ";
            TradeChekCustomer2.Text = " ";
            TradeChekEmployee2.Text = " ";


            if (string.IsNullOrEmpty(textNaklad1.Text)) { MessageBox.Show("ID поставки не заполнен"); return; }

            conn.Open();

            var id_provider = Int32.Parse(textNaklad1.Text);

            NpgsqlCommand cmd0 = new NpgsqlCommand("SELECT * FROM get_product_totals_by_supply_id(@id_provider)", conn);
            cmd0.Parameters.AddWithValue("id_provider", id_provider);

            NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(cmd0);
            DataSet db = new DataSet();
            dataAdapter.Fill(db);
            dataGridNaklad2.DataSource = db.Tables[0];

            NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM get_supply_totals_by_id(@id_provider)", conn);
            cmd.Parameters.AddWithValue("id_provider", id_provider);



            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {

                    if (!reader.HasRows)
                    {
                        MessageBox.Show("Такого товара ");
                        return;
                    }

                    var data = reader.GetFieldValue<DateTime>(0);
                    var totalprice = reader.GetFieldValue<float>(1);
                    id_provider = reader.GetFieldValue<int>(2);


                    NakladSumm2.Text = "Всего: ";
                    NakladSumm2.Text += Convert.ToString(totalprice);
                    NakladSumm2.Text += " рублей";

                    NakladDate2.Text = Convert.ToString(data);

                    //MessageBox.Show("Номер склада:" + storageId.ToString() + "  ; Наличие: "+ quantity.ToString() );
                }
                conn.Close();

                conn.Open();


                NpgsqlCommand cmd2 = new NpgsqlCommand("SELECT n_provider FROM providers WHERE id_provider = @id_provider;", conn);
                cmd2.Parameters.AddWithValue("id_provider", id_provider);

                object name2 = cmd2.ExecuteScalar();
                NakladProvider2.Text = "Поставщик: ";
                NakladProvider2.Text += Convert.ToString(name2);

                conn.Close();
                //MessageBox.Show("OK");
            }
        }

        // -------------------------------------------------------------------------------------------------- INSERT_BTN

        private void btnStoragesInfo_insert_Click(object sender, EventArgs e)
        {
            conn.Open();

            NpgsqlCommand cmd = new NpgsqlCommand("SELECT storagesinfo_insert(@adress)", conn);
            cmd.Parameters.AddWithValue("adress", NpgsqlTypes.NpgsqlDbType.Varchar, 120, textStoragesInfo1.Text);

            cmd.ExecuteNonQuery();

            conn.Close();
        }

        private void btnProviders_insert_Click(object sender, EventArgs e)
        {
            conn.Open();

            NpgsqlCommand cmd = new NpgsqlCommand("SELECT providers_insert(@n_provider)", conn);
            cmd.Parameters.AddWithValue("n_provider", NpgsqlTypes.NpgsqlDbType.Varchar, 120, textProviders1.Text);

            cmd.ExecuteNonQuery();

            conn.Close();
        }

        private void btnCustomers_insert_Click(object sender, EventArgs e)
        {
            conn.Open();

            NpgsqlCommand cmd = new NpgsqlCommand("SELECT customers_insert(@fn_customer, @sn_customer)", conn);
            cmd.Parameters.AddWithValue("fn_customer", NpgsqlTypes.NpgsqlDbType.Varchar, 20, textCustomers1.Text);
            cmd.Parameters.AddWithValue("sn_customer", NpgsqlTypes.NpgsqlDbType.Varchar, 20, textCustomers2.Text);

            cmd.ExecuteNonQuery();

            conn.Close();
        }

        private void btnCategories_insert_Click(object sender, EventArgs e)
        {
            conn.Open();

            NpgsqlCommand cmd = new NpgsqlCommand("SELECT categories_insert(@n_category, @desc_category)", conn);
            cmd.Parameters.AddWithValue("n_category", NpgsqlTypes.NpgsqlDbType.Varchar, 20, textCategories1.Text);
            cmd.Parameters.AddWithValue("desc_category", NpgsqlTypes.NpgsqlDbType.Varchar, 120, textCategories2.Text);

            cmd.ExecuteNonQuery();

            conn.Close();
        }

        private void btnEmployee_insert_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textEmployee1.Text)) { MessageBox.Show("Ранг не указан"); return; }
            conn.Open();

            try
            {
                int Timed_int = Int32.Parse(textEmployee1.Text);

                NpgsqlCommand cmd = new NpgsqlCommand("SELECT employees_insert(@id_rank, @fn_employee, @sn_employee)", conn);
                cmd.Parameters.AddWithValue("id_rank", Timed_int);
                cmd.Parameters.AddWithValue("fn_employee", NpgsqlTypes.NpgsqlDbType.Varchar, 20, textEmployee2.Text);
                cmd.Parameters.AddWithValue("sn_employee", NpgsqlTypes.NpgsqlDbType.Varchar, 20, textEmployee3.Text);

                cmd.ExecuteNonQuery();

                conn.Close();
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show("Произошла ошибка SQL: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Произошла ошибка при работе с базой данных: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
            }


        }

        private void btnUsers_insert_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textUsers3.Text)) { MessageBox.Show("Ранг не указан"); return; }
            object result;
            conn.Open();

            try
            {
                int Timed_int = Int32.Parse(textUsers3.Text);

                NpgsqlCommand cmd = new NpgsqlCommand("SELECT users_insert(@id_rank, @login, @pass", conn);

                cmd.Parameters.AddWithValue("login", NpgsqlTypes.NpgsqlDbType.Varchar, 20, textUsers1.Text);
                cmd.Parameters.AddWithValue("pass", NpgsqlTypes.NpgsqlDbType.Varchar, 120, textUsers2.Text);
                cmd.Parameters.AddWithValue("id_rank", Timed_int);

                result = cmd.ExecuteScalar();


                conn.Close();
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show("Произошла ошибка SQL: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Произошла ошибка при работе с базой данных: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
            }

        }

        private void btnRanks_insert_Click(object sender, EventArgs e)
        {
            conn.Open();

            NpgsqlCommand cmd = new NpgsqlCommand("SELECT ranks_insert(@n_rank)", conn);
            cmd.Parameters.AddWithValue("n_rank", NpgsqlTypes.NpgsqlDbType.Varchar, 20, textRanks1.Text);

            cmd.ExecuteNonQuery();

            conn.Close();
        }

        // ---------------------------------------------------------------------------------------------------- DELETE_BTN
        // 	DELETE FROM Products WHERE id_product = _id_product;

        private void btnStoragesInfo_delete_Click(object sender, EventArgs e)
        {
            conn.Open();
            try
            {
                DataGridViewRow row = dataGridStoragesInfo.Rows[dataGridStoragesInfo.CurrentCell.RowIndex];
                NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM StoragesInfo WHERE id_storage = @id_storage;", conn);
                cmd.Parameters.AddWithValue("id_storage", Int32.Parse(row.Cells["id_storage"].Value.ToString()));

                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show("Произошла ошибка SQL: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Произошла ошибка при работе с базой данных: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
            }
 
        }

        private void btnProviders_delete_Click(object sender, EventArgs e)
        {
            conn.Open();
            try
            {
                DataGridViewRow row = dataGridProviders.Rows[dataGridProviders.CurrentCell.RowIndex];
                NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM Providers WHERE id_provider = @id_provider;", conn);
                cmd.Parameters.AddWithValue("id_provider", Int32.Parse(row.Cells["id_provider"].Value.ToString()));

                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show("Произошла ошибка SQL: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Произошла ошибка при работе с базой данных: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
            }

        }

        private void btnCustomers_delete_Click(object sender, EventArgs e)
        {
            conn.Open();
            try
            {
                DataGridViewRow row = dataGridCustomers.Rows[dataGridCustomers.CurrentCell.RowIndex];
                NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM Customers WHERE id_customer = @id_customer;", conn);
                cmd.Parameters.AddWithValue("id_customer", Int32.Parse(row.Cells["id_customer"].Value.ToString()));

                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show("Произошла ошибка SQL: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Произошла ошибка при работе с базой данных: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
            }

        }

        private void btnCategories_delete_Click(object sender, EventArgs e)
        {
            conn.Open();
            try
            {
                DataGridViewRow row = dataGridView2.Rows[dataGridView2.CurrentCell.RowIndex];
                NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM Categories WHERE id_category = @id_category;", conn);
                cmd.Parameters.AddWithValue("id_category", Int32.Parse(row.Cells["id_category"].Value.ToString()));

                cmd.ExecuteNonQuery();
                conn.Close();

            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show("Произошла ошибка SQL: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
            }
            catch (Exception ex)
            {
 
                MessageBox.Show("Произошла ошибка при работе с базой данных: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
            }
        }

        private void btnEmployee_delete_Click(object sender, EventArgs e)
        {
            conn.Open();
            try
            {
                DataGridViewRow row = dataGridEmployees.Rows[dataGridEmployees.CurrentCell.RowIndex];
                NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM Employees WHERE id_employee = @id_employee;", conn);
                cmd.Parameters.AddWithValue("id_employee", Int32.Parse(row.Cells["id_employee"].Value.ToString()));

                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show("Произошла ошибка SQL: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Произошла ошибка при работе с базой данных: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
            }

        }

        private void btnUsers_delete_Click(object sender, EventArgs e)
        {
            conn.Open();
            try
            {
                DataGridViewRow row = dataGridUsers.Rows[dataGridUsers.CurrentCell.RowIndex];
                NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM Users WHERE id_user = @id_user;", conn);
                cmd.Parameters.AddWithValue("id_user", Int32.Parse(row.Cells["id_user"].Value.ToString()));

                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show("Произошла ошибка SQL: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка при работе с базой данных: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
            }

        }

        private void btnRanks_delete_Click(object sender, EventArgs e)
        {
            conn.Open();
            try
            {
                DataGridViewRow row = dataGridRanks.Rows[dataGridRanks.CurrentCell.RowIndex];
                NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM Ranks WHERE id_rank = @id_rank;", conn);
                cmd.Parameters.AddWithValue("id_rank", Int32.Parse(row.Cells["id_rank"].Value.ToString()));

                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show("Произошла ошибка SQL: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Произошла ошибка при работе с базой данных: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
            }
 
        }

        // ---------------------------------------------------------------------------------------------- UPDATE_BTN

        private void btnStoragesInfo_update_Click(object sender, EventArgs e)
        {
            conn.Open();

            DataGridViewRow row = dataGridStoragesInfo.Rows[dataGridStoragesInfo.CurrentCell.RowIndex];

            NpgsqlCommand cmd = new NpgsqlCommand("SELECT storagesinfo_update(@id_storage, @adress)", conn);
            cmd.Parameters.AddWithValue("id_storage", Int32.Parse(row.Cells["id_storage"].Value.ToString()));
            cmd.Parameters.AddWithValue("adress", NpgsqlTypes.NpgsqlDbType.Varchar, 120, textStoragesInfo1.Text);

            cmd.ExecuteNonQuery();
            conn.Close();
        }

        private void btnProviders_update_Click(object sender, EventArgs e)
        {
            conn.Open();

            DataGridViewRow row = dataGridProviders.Rows[dataGridProviders.CurrentCell.RowIndex];

            NpgsqlCommand cmd = new NpgsqlCommand("SELECT providers_update(@id_provider, @n_provider)", conn);
            cmd.Parameters.AddWithValue("id_provider", Int32.Parse(row.Cells["id_provider"].Value.ToString()));
            cmd.Parameters.AddWithValue("n_provider", NpgsqlTypes.NpgsqlDbType.Varchar, 120, textProviders1.Text);

            cmd.ExecuteNonQuery();
            conn.Close();
        }

        private void btnCustomers_update_Click(object sender, EventArgs e)
        {
            conn.Open();

            DataGridViewRow row = dataGridCustomers.Rows[dataGridCustomers.CurrentCell.RowIndex];

            NpgsqlCommand cmd = new NpgsqlCommand("SELECT customers_update(@id_customer, @fn_customer, @sn_customer)", conn);
            cmd.Parameters.AddWithValue("id_customer", Int32.Parse(row.Cells["id_customer"].Value.ToString()));
            cmd.Parameters.AddWithValue("fn_customer", NpgsqlTypes.NpgsqlDbType.Varchar, 20, textCustomers1.Text);
            cmd.Parameters.AddWithValue("sn_customer", NpgsqlTypes.NpgsqlDbType.Varchar, 20, textCustomers2.Text);

            cmd.ExecuteNonQuery();
            conn.Close();
        }

        private void btnCategories_update_Click(object sender, EventArgs e)
        {
            conn.Open();

            DataGridViewRow row = dataGridView2.Rows[dataGridView2.CurrentCell.RowIndex];

            NpgsqlCommand cmd = new NpgsqlCommand("SELECT categories_update(@id_category, @n_category, @desc_category)", conn);
            cmd.Parameters.AddWithValue("id_category", Int32.Parse(row.Cells["id_category"].Value.ToString()));
            cmd.Parameters.AddWithValue("n_category", NpgsqlTypes.NpgsqlDbType.Varchar, 20, textCategories1.Text);
            cmd.Parameters.AddWithValue("desc_category", NpgsqlTypes.NpgsqlDbType.Varchar, 120, textCategories2.Text);

            cmd.ExecuteNonQuery();
            conn.Close();
        }

        private void btnEmployee_update_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textEmployee1.Text)) { MessageBox.Show("Ранг не указан"); return; }
            conn.Open();

            try
            {
                DataGridViewRow row = dataGridEmployees.Rows[dataGridEmployees.CurrentCell.RowIndex];
                int Timed_int = Int32.Parse(textEmployee1.Text);

                NpgsqlCommand cmd = new NpgsqlCommand("SELECT employees_update(@id_employee, @id_rank, @fn_employee, @sn_employee)", conn);
                cmd.Parameters.AddWithValue("id_employee", Int32.Parse(row.Cells["id_employee"].Value.ToString()));
                cmd.Parameters.AddWithValue("fn_employee", NpgsqlTypes.NpgsqlDbType.Varchar, 20, textEmployee2.Text);
                cmd.Parameters.AddWithValue("sn_employee", NpgsqlTypes.NpgsqlDbType.Varchar, 20, textEmployee3.Text);
                cmd.Parameters.AddWithValue("id_rank", Timed_int);

                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show("Произошла ошибка SQL: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Произошла ошибка при работе с базой данных: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
            }

        }

        private void btnUsers_update_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textUsers3.Text)) { MessageBox.Show("Ранг не указан"); return; }
            conn.Open();

            try
            {
                DataGridViewRow row = dataGridUsers.Rows[dataGridUsers.CurrentCell.RowIndex];
                int Timed_int = Int32.Parse(textUsers3.Text);

                NpgsqlCommand cmd = new NpgsqlCommand("SELECT users_update(@id_user, @id_rank, @login, @pass)", conn);
                cmd.Parameters.AddWithValue("id_user", Int32.Parse(row.Cells["id_user"].Value.ToString()));
                cmd.Parameters.AddWithValue("login", NpgsqlTypes.NpgsqlDbType.Varchar, 20, textUsers1.Text);
                cmd.Parameters.AddWithValue("pass", NpgsqlTypes.NpgsqlDbType.Varchar, 120, textUsers2.Text);
                cmd.Parameters.AddWithValue("id_rank", Timed_int);

                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show("Произошла ошибка SQL: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Произошла ошибка при работе с базой данных: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
            }

        }

        private void btnRanks_update_Click(object sender, EventArgs e)
        {
            conn.Open();

            DataGridViewRow row = dataGridRanks.Rows[dataGridRanks.CurrentCell.RowIndex];

            NpgsqlCommand cmd = new NpgsqlCommand("SELECT ranks_update(@id_rank, @n_rank)", conn);

            cmd.Parameters.AddWithValue("id_rank", Int32.Parse(row.Cells["id_rank"].Value.ToString()));
            cmd.Parameters.AddWithValue("n_rank", NpgsqlTypes.NpgsqlDbType.Varchar, 20, textRanks1.Text);

            cmd.ExecuteNonQuery();
            conn.Close();
        }

        // ---------------------------------------------------------------------------------------------- DATAGRID_CELL_CLITCK_BTN

        private void dataGridStoragesInfo_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            conn.Open();
            if (e.RowIndex >= 0)
            {
                textStoragesInfo1.Text = dataGridStoragesInfo.Rows[e.RowIndex].Cells["adress"].Value.ToString();
            }
            else
            {
                // MessageBox.Show("Что то не так");
            }
            conn.Close();
        }

        private void dataGridProviders_CelltClick(object sender, DataGridViewCellEventArgs e)
        {
            conn.Open();
            if (e.RowIndex >= 0)
            {
                textProviders1.Text = dataGridProviders.Rows[e.RowIndex].Cells["n_provider"].Value.ToString();
            }
            else
            {
                // MessageBox.Show("Что то не так");
            }
            conn.Close();
        }

        private void dataGridCustomers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            conn.Open();
            if (e.RowIndex >= 0)
            {
                textCustomers1.Text = dataGridCustomers.Rows[e.RowIndex].Cells["fn_customer"].Value.ToString();
                textCustomers2.Text = dataGridCustomers.Rows[e.RowIndex].Cells["sn_customer"].Value.ToString();
            }
            else
            {
                // MessageBox.Show("Что то не так");
            }
            conn.Close();
        }

        private void dataGridEmployees_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            conn.Open();
            if (e.RowIndex >= 0)
            {
                textEmployee1.Text = dataGridEmployees.Rows[e.RowIndex].Cells["id_rank"].Value.ToString();
                textEmployee2.Text = dataGridEmployees.Rows[e.RowIndex].Cells["fn_employee"].Value.ToString();
                textEmployee3.Text = dataGridEmployees.Rows[e.RowIndex].Cells["sn_employee"].Value.ToString();
            }
            else
            {
                // MessageBox.Show("Что то не так");
            }
            conn.Close();
        }

        private void dataGridUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            conn.Open();
            if (e.RowIndex >= 0)
            {
                textUsers1.Text = dataGridUsers.Rows[e.RowIndex].Cells["login"].Value.ToString();
                textUsers2.Text = dataGridUsers.Rows[e.RowIndex].Cells["pass"].Value.ToString();
                textUsers3.Text = dataGridUsers.Rows[e.RowIndex].Cells["id_rank"].Value.ToString();
            }
            else
            {
                // MessageBox.Show("Что то не так");
            }
            conn.Close();
        }

        private void dataGridRanks_CelltClick(object sender, DataGridViewCellEventArgs e)
        {
            conn.Open();
            if (e.RowIndex >= 0)
            {
                textRanks1.Text = dataGridRanks.Rows[e.RowIndex].Cells["n_rank"].Value.ToString();
            }
            else
            {
                // MessageBox.Show("Что то не так");
            }
            conn.Close();
        }

        // ------------------------------------------------------------------------------------------- CUSTOM_SQL_BTNS

        private void btnSQL_load_Click(object sender, EventArgs e)
        {
            conn.Open();

            NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(textSQL1.Text, conn);
            DataSet dataset = new DataSet();

            dataAdapter.Fill(dataset);
            dataGridSQL.DataSource = dataset.Tables[0];

            conn.Close();
        }

        private void btnMain_logout_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Выход из программы");
            Application.Exit();

            //Form2 AuthForm = new Form2();
            //AuthForm.Show();
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            int Timed = DataTrans.Value;
            //RightLevel.Text = "Уровень прав: ";
            RightLevel.Text += Convert.ToString(DataTrans.Value.ToString());

            //MessageBox.Show(DataTrans.Value.ToString() + " От формы 1");
        }

        // ------------------------------------------------------------------------------------------- ADMIN_ACSESS

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //здесь обработка изменения выбранной вкладки например
            if (tabControl1.SelectedTab == tabPage5)
            {
                if (DataTrans.Value == 3)
                {
                    MessageBox.Show("Доступ разрешён");
                }
                else
                {
                    MessageBox.Show("У вас нет прав");
                    tabControl1.SelectedTab = tabPage1;
                }
            }
        }
    }
}
