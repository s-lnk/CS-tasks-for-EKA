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



/*
 * DATABASE MANAGEMENT TOOL
 * There are 2 tables in database which are generated automatically:
 * - carmake - Makes of cars
 * - carmodel - Models of cars
 * Features:
 * - Seed test data
 * - Trubcate tables
 * - In cell edit
 * - Delete selected row
 * - Search for records
 * - Edited data is updated in DB pressing Save or if user closes connection
 * 
 * 
 * NB! Before use set correct connection string, which contanis existing database on selected server. Tables will be generated automatically during app startup.
 * */



namespace db
{
    public partial class Form1 : Form
    {
        public SqlConnection DBConnection;
        private DataTable DBTable = new DataTable();
        private SqlDataAdapter DBAdapter = new SqlDataAdapter();
        public Form1()
        {
            InitializeComponent();
            this.Text = "DB by VG";
            textBox1.Text = "Data Source=192.168.7.15;User Id=sa_uvs;Password=sDfeW45FGd5TeWTgS5;Initial Catalog=test;Integrated Security=False;";
        }
       

        public SqlConnection getSqlConnection(String ConnString)
        {
            SqlConnection resultConnection;
            resultConnection = new SqlConnection(ConnString);
            return resultConnection;
        }

        //Open connection to DB, generate tables if needed
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                DBConnection = getSqlConnection(textBox1.Text);
                DBConnection.Open();
                initTables(); //Generate tables in database if needed
                button1.Enabled = false;
                button2.Enabled = true;
                button3.Enabled = true;
                button4.Enabled = true;
                button5.Enabled = true;
                button6.Enabled = true;
                button7.Enabled = true;
                comboBox1.Enabled = true;
                comboBox1.Items.Add("carmake");
                comboBox1.Items.Add("carmodel");
            } catch (Exception ex)
            {
                System.Console.WriteLine("Exception " + ex.Message);
            }
        }

        // Save data and close connection
        private void button2_Click(object sender, EventArgs e)
        {
            updateDB();
            DBConnection.Close();
            button1.Enabled = true;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
            button7.Enabled = false;
            comboBox1.Enabled = false;
            comboBox1.Items.Clear();
        }


        private void initTables()
        {
            try
            {
                String DBQuery;
                SqlCommand DBCmd = DBConnection.CreateCommand();
                DBQuery = "IF OBJECT_ID('carmake', 'U') IS NULL CREATE TABLE carmake (id INTEGER NOT NULL IDENTITY(1,1) PRIMARY KEY, name VARCHAR(80))";
                DBCmd.CommandText = DBQuery;
                DBCmd.ExecuteNonQuery();
                DBQuery = "IF OBJECT_ID('carmodel', 'U') IS NULL CREATE TABLE carmodel (id INTEGER NOT NULL IDENTITY(1,1) PRIMARY KEY, name VARCHAR(80), make_id int FOREIGN KEY REFERENCES carmake(id))";
                DBCmd.CommandText = DBQuery;
                DBCmd.ExecuteNonQuery();
            } catch (Exception)
            {

            }
        }


        //Test data seeder for table carmake
        private void seedTableMake(String name)
        {
            String DBQuery;
            SqlCommand DBCmd = DBConnection.CreateCommand();
            DBQuery = "INSERT INTO carmake (name) VALUES (@pr);";
            //DBCmd.CommandText = DBQuery;
            DBCmd.Parameters.AddWithValue("@pr", name);
            DBCmd.CommandText = DBQuery;
            DBCmd.ExecuteNonQuery();

        }

        //Test data seeder for table carmodel
        private void seedTableModel(String name, int fid)
        {
            String DBQuery;
            SqlCommand DBCmd = DBConnection.CreateCommand();
            DBQuery = "INSERT INTO carmodel (name, make_id) VALUES (@pr, @fid);";
            DBCmd.CommandText = DBQuery;
            DBCmd.Parameters.AddWithValue("@pr", name);
            DBCmd.Parameters.AddWithValue("@fid", fid);
            DBCmd.CommandText = DBQuery;
            DBCmd.ExecuteNonQuery();
            
        }

        private void executeSQLQuery(String DBQuery)
        {
            SqlCommand DBCmd = DBConnection.CreateCommand();
            DBCmd.CommandText = DBQuery;
            DBCmd.ExecuteNonQuery();
        }

        //Seed test data
        private void button3_Click(object sender, EventArgs e)
        {
            seedTableMake("Audi");
            seedTableMake("BMW");
            seedTableMake("Mercedes");
            seedTableModel("A6",1);
            seedTableModel("A8", 1);
            seedTableModel("Q7", 1);
            seedTableModel("X1", 2);
            seedTableModel("X5", 2);
            seedTableModel("X6", 2);
            seedTableModel("A140", 3);
            seedTableModel("E320", 3);
            seedTableModel("S600", 3);
        }

        //Truncate tables
        private void button4_Click(object sender, EventArgs e)
        {
            executeSQLQuery("DROP TABLE carmodel");
            executeSQLQuery("DROP TABLE carmake");
            initTables();
            DBTable.Clear();
            dataGridView1.Refresh();
        }

        //Select table and fill datagrid
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.Console.WriteLine("Choosen:" + comboBox1.SelectedItem);
            dataGridView1.EditMode = DataGridViewEditMode.EditOnEnter;
            DBTable.Columns.Clear();
            DBAdapter.SelectCommand = new SqlCommand("SELECT * FROM " + comboBox1.SelectedItem, DBConnection);
            DBTable.Clear();
            DBAdapter.Fill(DBTable);
            dataGridView1.DataSource = DBTable;
            dataGridView1.Refresh();
            label1.Text = "Total records: " + DBTable.Rows.Count;

        }

        private void dataGridView1_CellContentClick(object sender)
        {

        }

        //Save changes to table
        private void button5_Click(object sender, EventArgs e)
        {
            updateDB();
        }

        private void updateDB()
        {
            try
            {
                DBAdapter.UpdateCommand = new SqlCommandBuilder(DBAdapter).GetUpdateCommand();
                int res = DBAdapter.Update(DBTable);
                System.Console.WriteLine("res " + res);
                DBTable.Clear();
                DBAdapter.Fill(DBTable);
            }
            catch (SqlException ex)
            {
                System.Console.WriteLine("Ex " + ex.Message);
            }
        }

        //Delete selected row
        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    dataGridView1.Rows.RemoveAt(this.dataGridView1.SelectedRows[0].Index);
                    updateDB();
                }
            } catch (Exception ex)
            {
                System.Console.WriteLine("Ex " + ex.Message + " " + dataGridView1.SelectedRows.Count);
            }
        }

        //Search
        private void button7_Click(object sender, EventArgs e)
        {
            DBAdapter.SelectCommand = new SqlCommand("SELECT * FROM " + comboBox1.SelectedItem + " WHERE name LIKE '%" + textBox2.Text + "%'", DBConnection);
            DBTable.Clear();
            DBAdapter.Fill(DBTable);
            dataGridView1.DataSource = DBTable;
            dataGridView1.Refresh();
            label1.Text = "Total records found: " + DBTable.Rows.Count;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


    }
}
