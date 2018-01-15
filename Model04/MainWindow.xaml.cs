using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;

namespace Model04
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _connectionString = "";
        public MainWindow()
        {
            InitializeComponent();
            _connectionString = "Data Source=192.168.111.154; Initial Catalog=hMalServer; User ID=sa; Password=Mc123456;";
        }

        private void ConnectBtn_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = _connectionString;

            SqlDataAdapter da = new SqlDataAdapter("select * from AT_Users", con);

            DataSet ds = new DataSet();
            da.Fill(ds);

            var tt = "";
            foreach (DataTable table in ds.Tables)
            {
                dtColumns.ItemsSource = table.Columns;
                foreach (DataRow row in table.Rows)
                {
                    var rows = row.ItemArray;
                    foreach (object cell in rows)
                    {
                        tt = cell.ToString();
                    }
                }
            }
            con.Dispose();
        }

        private void CreateTableBtn_Click(object sender, RoutedEventArgs e)
        {
           
            DataSet ds = new DataSet();

            //Первый Вариант
            DataTable tbl = new DataTable("AT_Users_New");
           

            // 2-ой способ
            //ds.Tables.Add("AT_Users_New");

            //Создаем кононку в таблице с типом int
            DataColumn col = tbl.Columns.Add("UserID", typeof(int));
            col.AllowDBNull = false;//Not Null
            /*col.MaxLength = 5;//Maxимальная длинна только для строчный type*/
            col.Unique = true;

            tbl.PrimaryKey = new DataColumn[]
            {
                tbl.Columns["UserID"]
            };

            col.AutoIncrement = true;//Разрешаем автоинкримент
            col.AutoIncrementSeed = -1;// начало инкримента
            col.AutoIncrementStep = +1;// шаг инкримента
            col.ReadOnly = true;//


            //добавление новой строки - запись

            DataRow dr = ds.Tables["AT_Users"].NewRow();
            dr["UserLogin"] = "Test";
            dr["UserPassword"] = "123";
            dr["UserAge"] = DBNull.Value;

            //удаление строки - записи(!) - главный 

            DataRow ddr = ds.Tables["AT_Users"].Rows.Find(2);
            ds.Tables["AT_Users"].Rows.Remove(ddr);

            //удаление строки - записи ----- способ 2

               ds.Tables["AT_Users"].Rows.RemoveAt(2);

            ds.Tables.Add(tbl);

            foreach (DataTable  table in ds.Tables)
            {     
                foreach (DataRow row in table.Rows)
                {
                    var st = row.RowState();
                }
            }

            //подключение к бд

            SqlConnection con = new SqlConnection(_connectionString);

            con.Open();

            SqlDataAdapter da = new SqlDataAdapter("", con);

            da.Update(ds);

            con.Close();
            
        }
    }
}
