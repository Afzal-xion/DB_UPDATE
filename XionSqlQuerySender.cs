using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XionFix
{
    public partial class Form1 : Form
    {
        SqlConnection connection;
        public static string Connection1 = "";
        public static string connectionString = "";
        private string msgHeader = ("XION (" + Application.ProductVersion + ")");
        public Form1()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dataSources = SqlDataSourceEnumerator.Instance.GetDataSources();
                foreach (DataRow row in dataSources.Rows)
                {
                    //cmbServerName.Text = row[dataSources.Columns["ServerName"]].ToString() + "\\" + row[dataSources.Columns["InstanceName"]].ToString();
                    cmbServerName.Items.Add(row[dataSources.Columns["ServerName"]].ToString() + "\\" + row[dataSources.Columns["InstanceName"]].ToString());
                }
                if (string.IsNullOrEmpty(cmbServerName.Text))
                {
                    cmbServerName.Items.Add(@".\sqlexpress");
                }
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message);
            }
            //try
            //{
            //    if (string.IsNullOrEmpty(cmbServerName.Text))
            //    {
            //        cmbServerName.Items.Add(@".\sqlexpress");
            //    }
            //}
            //catch (Exception exception1)
            //{
            //    MessageBox.Show(exception1.Message);
            //}
        }
        public void get_alldb()
        {
            try
            {
                string connectionString = "";
                if (cmbServerName.Text != string.Empty)
                {
                    if (txtUsername.Text != string.Empty)
                    {
                        if (txtPassword.Text != string.Empty)
                        {
                            string[] strArray = new string[] { "Data Source='", cmbServerName.Text, "'; User ID='", txtUsername.Text, "';Password='", txtPassword.Text, "'" };
                            connectionString = string.Concat(strArray);
                        }
                        else
                        {
                            MessageBox.Show("Please Enter Password!");
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please Enter Username!");
                        return;
                    }

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        using (SqlCommand command = new SqlCommand("SELECT name from sys.databases where name not in ('master', 'model', 'tempdb', 'msdb') order by name", connection))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    cmbDatabase.Items.Add(reader[0].ToString());
                                }
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please Enete Hostname!");
                }
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbServerName.Text != "")
                {
                    if (cmbDatabase.Text != "")
                    {
                        if (txtPassword.Text != "")
                        {
                            if (txtUsername.Text != "")
                            {
                                string[] strArray2 = new string[] { "Data Source=", cmbServerName.Text, ";Initial Catalog='", cmbDatabase.Text, "';User ID='", txtUsername.Text, "';Password='", txtPassword.Text, "'" };
                                string connectionString = string.Concat(strArray2);
                                connection = new SqlConnection(connectionString);
                                connection.Open();
                                MessageBox.Show("Database Connection Successfully !!!");
                                tabControl1.Enabled = true;
                                connection.Close();
                                button2.Enabled = true;
                                txtquery.Enabled = true;
                            }
                            else
                            {
                                MessageBox.Show("please enter username!");
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show("please enter password!");
                            return;
                        }
                    }
                    else
                    {
                        get_alldb();
                        MessageBox.Show("Please Enter Databasename Name!");
                    }
                }
                else
                {
                    MessageBox.Show("Please Enter Host Name!");
                }
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
            SqlCommand command = new SqlCommand(txtquery.Text, connection);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
            MessageBox.Show("SQL QUIRY EXCUTED !!!");
        }

        public void executequery(string query, string Message1)
        {
            try
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                SqlCommand sqlCmd = new SqlCommand(query, connection);
                connection.Open();
                sqlCmd.ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Error!" + exception.Message, msgHeader, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            finally
            {
                connection.Close();
                MessageBox.Show(Message1);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                string query ="IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AccessRoleMaster]') AND type in (N'U')) BEGIN SET ANSI_NULLS ON SET QUOTED_IDENTIFIER ON CREATE TABLE [dbo].[AccessRoleMaster]([RoleID][int] IDENTITY(1, 1) NOT NULL,[RoleName][Varchar](150) NULL,[company_id][int] NULL,[BranchID][int] NULL,[CreateBy][int] NULL,[CreateDate][datetime] NULL,[UpdateBy][int] NULL,[UpdateDate][datetime] NULL,[ForOnlyTill][bit] NULL, CONSTRAINT[PK_AccessRoleMaster] PRIMARY KEY CLUSTERED([RoleID] ASC)) ON[PRIMARY]END IF NOT EXISTS(SELECT* FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[systemuser]') AND type in (N'U')) BEGIN SET ANSI_NULLS ON SET QUOTED_IDENTIFIER ON CREATE TABLE[dbo].[systemuser]([systemuser_id][int] IDENTITY(1, 1) NOT NULL,[systemuser_code] [varchar](25) NULL,[systemuser_name] [varchar](150) NULL,[systemuser_designation] [varchar](150) NULL,[systemuser_mobile] [varchar](12) NULL,[systemuser_email] [varchar](150) NULL,[systemuser_address] [varchar](255) NULL,[systemuser_otherid] [varchar](35) NULL,[systemuser_username] [varchar](35) NULL,[systemuser_password] [varchar](25) NULL,[systemuser_dob] [date] NULL,[systemuser_doj] [date] NULL,[systemuser_till] [varchar](255) NULL,[systemuser_role] [varchar](50) NULL,[systemuser_image] [varchar](max)NULL,[systemuser_lock] [smallint] NULL,[price_selection] [varchar](255) NULL,[debtor] [int] NULL,[systemuser_reprint] [int] NULL,[month_end_id] [int] NULL,[allow_negative_qty_sell] [int] NULL,[select_selling_price] [int] NULL,[print_balance_on_invoice] [int] NULL,[change_selling_price] [int] NULL,[desc_change] [int] NULL,[capture_barcode] [int] NULL,[sell_below_cost] [int] NULL,[activate_rounding] [int] NULL,[sales_return] [int] NULL,[line_discount] [int] NULL,[Item_Statastic] [int] NULL,[stock_browse] [int] NULL,[display_pole] [int] NULL,[view_cost] [int] NULL,[input_qty] [int] NULL,[end_discount] [int] NULL,[status] [int] NULL,[allow_print] [int] NULL,[warehouse_order] [varchar](100) NULL,[amend_cost] [int] NULL,[sale_zero] [int] NULL,[multi_warehouse] [int] NULL,[balance_age] [int] NULL,[supr_rule] [int] NULL,[view_profit] [int] NULL,[machine_id] [int] NULL,[QPrint] [int] NULL,[LBViewExpiry] [int] NULL,[systemuser_roleid] [int] NULL,[Showlistdatalimits] [int] NULL,[AcSalesA4DirPrint] [bit] NULL,[CshOpenDrawer] [bit] NULL,[CrdOpenDrawer] [bit] NULL,[VouOpenDrawer] [bit] NULL,[EFTOpenDrawer] [bit] NULL,[AddQtyOnExisting] [bit] NULL,[SearchByCodeDesc] [bit] NULL,[AddDirectFromSearch] [bit] NULL,[SetDefaultSearch] [bit] NULL,[AllowDisplayQty] [bit] DEFAULT(0) NOT NULL,[AllowDisplayCost] [bit] DEFAULT(0) NOT NULL, CONSTRAINT[PK_systemuser_id] PRIMARY KEY CLUSTERED([systemuser_id] ASC)) ON[PRIMARY]END";
                executequery(query, "Database Updated successfully!");
                checkBox1.Checked = false;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                string query = "IF NOT EXISTS(SELECT* FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[delivery_address]') AND type in (N'U')) BEGIN CREATE TABLE [dbo].[delivery_address]([id] [int] IDENTITY(1,1) NOT NULL,[name] [nvarchar](255) NULL,[contact_name] [nvarchar](255) NULL,[contact_no] [nvarchar](20) NULL,[address] [nvarchar](max) NULL,PRIMARY KEY CLUSTERED([id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY] END";
                executequery(query, "Database Updated successfully!");
                checkBox3.Checked = false;
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
            {
                string query = "IF NOT EXISTS(SELECT* FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[stock_end_of_month_new]') AND type in (N'U')) BEGIN CREATE TABLE [dbo].[stock_end_of_month_new]([id] [int] IDENTITY(1,1) NOT NULL,[stock_id][int] NULL,[end_of_month_id] [int] NULL,[yearmonth] [varchar](10) NULL,[period_close] [varchar](1) NULL,[on_hand_stock] [numeric](18, 3) NULL,[txtn] [varchar](10) NULL,[WarehouseID] [int] NULL,CONSTRAINT[PK_stock_end_of_month_new] PRIMARY KEY CLUSTERED([id] ASC)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON[PRIMARY]) ON[PRIMARY] END";
                executequery(query, "Database Updated successfully!");
                checkBox4.Checked = false;
            }
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked)
            {
                string query = "IF NOT EXISTS(SELECT* FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sales_item_temp]') AND type in (N'U')) BEGIN CREATE TABLE [dbo].[sales_item_temp]([sales_item_id] [int] IDENTITY(28,1) NOT NULL,[sales_id][int] NOT NULL,[stock_id] [int] NOT NULL,[detail] [varchar](255) NOT NULL,[qty] [float] NOT NULL,[return_qty] [float] NULL,[sell_price] [float] NOT NULL,[price_code] [varchar](255) NOT NULL,[pack_size] [varchar](255) NOT NULL,[rep] [int] NOT NULL,[weight] [varchar](255) NOT NULL,[warehouse] [int] NOT NULL,[vat] [varchar](255) NOT NULL,[discount_value] [float] NOT NULL,[discount_type] [varchar](255) NOT NULL,[cost_inclusive] [numeric](10, 2) NOT NULL,[item_return_or_not] [smallint] NOT NULL,[return_discount] [float] NULL,[debtor_deal] [tinyint] NULL,[deal_having] [tinyint] NULL,[deal_giving] [tinyint] NULL,[promo_applied] [tinyint] NULL,[cost_exclusive] [numeric](10, 2) NULL,[promoid] [int] NULL,[onhand] [float] NULL,[AVGInc] [numeric](10, 2) NULL,[AVGExc] [numeric](10, 2) NULL,[InvoiceInEx] [varchar](10) NULL,[company_id] [int] NULL,[systemuser_id] [int] NULL,[FYID] [int] NULL,[BranchID] [int] NULL,[RepID] [int] NULL,[RepCType] [varchar](50) NULL,[RepCPer] [float] NULL,[StkCFlag] [bit] NULL,[StkCPer] [float] NULL,CONSTRAINT[PK_sales_item_temp_sales_item_id] PRIMARY KEY CLUSTERED([sales_item_id] ASC)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON[PRIMARY]) ON[PRIMARY] END";
                executequery(query, "Database Updated successfully!");
                checkBox5.Checked = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                string query = "IF NOT EXISTS(SELECT* FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sales_item_void]') AND type in (N'U')) BEGIN CREATE TABLE [dbo].[sales_item_void]([sales_item_id] [int] IDENTITY(28,1) NOT NULL,[sales_id][int] NOT NULL,[stock_id] [int] NOT NULL,[detail] [varchar](255) NOT NULL,[qty] [float] NOT NULL,[return_qty] [float] NULL,[sell_price] [float] NOT NULL,[price_code] [varchar](255) NOT NULL,[pack_size] [varchar](255) NOT NULL,[rep] [int] NOT NULL,[weight] [varchar](255) NOT NULL,[warehouse] [int] NOT NULL,[vat] [varchar](255) NOT NULL,[discount_value] [float] NOT NULL,[discount_type] [varchar](255) NOT NULL,[cost_inclusive] [numeric](10, 2) NOT NULL,[item_return_or_not] [smallint] NOT NULL,[return_discount] [float] NULL,[debtor_deal] [tinyint] NULL,[deal_having] [tinyint] NULL,[deal_giving] [tinyint] NULL,[promo_applied] [tinyint] NULL,[cost_exclusive] [numeric](10, 2) NULL,[promoid] [int] NULL,[onhand] [float] NULL,[AVGInc] [numeric](10, 2) NULL,[AVGExc] [numeric](10, 2) NULL,[InvoiceInEx] [varchar](10) NULL,[company_id] [int] NULL,[systemuser_id] [int] NULL,[FYID] [int] NULL,[BranchID] [int] NULL,[RepID] [int] NULL,[RepCType] [varchar](50) NULL,[RepCPer] [float] NULL,[StkCFlag] [bit] NULL,[StkCPer] [float] NULL,CONSTRAINT[PK_sales_item_void_id] PRIMARY KEY CLUSTERED([sales_item_id] ASC)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON[PRIMARY]) ON[PRIMARY] END";
                executequery(query, "Database Updated successfully!");
                checkBox2.Checked = false;
            }
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox6.Checked)
            {
                string query = "IF NOT EXISTS(SELECT* FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sales_temp]') AND type in (N'U')) BEGIN CREATE TABLE [dbo].[sales_temp]([sales_id] [int] IDENTITY(20,1) NOT NULL,[debtor_id] [int] NOT NULL,[sales_date] [datetime] NULL,[sales_total] [float] NOT NULL,[total_amount_paid] [float] NOT NULL,[sales_parksales] [smallint] NOT NULL,[sales_till_no] [int] NOT NULL,[card] [float] NOT NULL,[cash] [float] NOT NULL,[other] [float] NOT NULL,[changed] [float] NOT NULL,[voucher] [float] NOT NULL,[invoice_no] [varchar](255) NOT NULL,[cash_account] [int] NOT NULL,[created_by] [int] NOT NULL,[parksales_number] [varchar](255) NOT NULL,[sales_order_or_not] [smallint] NOT NULL,[sales_order_number] [varchar](255) NOT NULL,[invoice_status] [varchar](50) NULL,[slip_type] [varchar](25) NULL,[till_summary_id] [int] NULL,[sell_final] [int] NOT NULL,[month_end_id] [int] NOT NULL,[total_discount] [float] NULL,[total_vat] [float] NULL,[rounding] [numeric](10, 2) NULL,[qty_minus] [int] NULL,[total_cost_incl] [numeric](10, 2) NULL,[total_cost_excl] [numeric](10, 2) NULL,[db_vat_indi] [varchar](10) NULL,[dispatch] [int] NULL,[time] [varchar](30) NULL,[dbr_name] [varchar](100) NULL,[dbr_address] [varchar](500) NULL,[dbr_contact] [varchar](20) NULL,[dbr_vat_no] [varchar](50) NULL,[db_owing] [numeric](20, 3) NULL,[TotalAVGInc] [numeric](10, 2) NULL,[TotalAVGExc] [numeric](10, 2) NULL,[DDFlag] [bit] NULL,[quotation_id] [int] NULL,[company_id] [int] NULL,[FYID] [int] NULL,[BranchID] [int] NULL,CONSTRAINT[PK_sales_temp_sales_id] PRIMARY KEY CLUSTERED([sales_id] ASC)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON[PRIMARY]) ON[PRIMARY] END";
                executequery(query, "Database Updated successfully!");
                checkBox6.Checked = false;
            }
        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox10.Checked)
            {
                string query = "IF NOT EXISTS(SELECT* FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sales_void]') AND type in (N'U')) BEGIN  CREATE TABLE [dbo].[sales_void]([sales_id] [int] IDENTITY(20,1) NOT NULL,[debtor_id] [int] NOT NULL,[sales_date] [datetime] NULL,[sales_total] [float] NOT NULL,[total_amount_paid] [float] NOT NULL,[sales_parksales] [smallint] NOT NULL,[sales_till_no] [int] NOT NULL,[card] [float] NOT NULL,[cash] [float] NOT NULL,[other] [float] NOT NULL,[changed] [float] NOT NULL,[voucher] [float] NOT NULL,[invoice_no] [varchar](255) NOT NULL,[cash_account] [int] NOT NULL,[created_by] [int] NOT NULL,[parksales_number] [varchar](255) NOT NULL,[sales_order_or_not] [smallint] NOT NULL,[sales_order_number] [varchar](255) NOT NULL,[invoice_status] [varchar](50) NULL,[slip_type] [varchar](25) NULL,[till_summary_id] [int] NULL,[sell_final] [int] NOT NULL,[month_end_id] [int] NOT NULL,[total_discount] [float] NULL,[total_vat] [float] NULL,[rounding] [numeric](10, 2) NULL,[qty_minus] [int] NULL,[total_cost_incl] [numeric](10, 2) NULL,[total_cost_excl] [numeric](10, 2) NULL,[db_vat_indi] [varchar](10) NULL,[dispatch] [int] NULL,[time] [varchar](30) NULL,[dbr_name] [varchar](100) NULL,[dbr_address] [varchar](500) NULL,[dbr_contact] [varchar](20) NULL,[dbr_vat_no] [varchar](50) NULL,[db_owing] [numeric](20, 3) NULL,[TotalAVGInc] [numeric](10, 2) NULL,[TotalAVGExc] [numeric](10, 2) NULL,[DDFlag] [bit] NULL,[quotation_id] [int] NULL,[company_id] [int] NULL,[FYID] [int] NULL,[BranchID] [int] NULL,[FinalPay] [int] NOT NULL,CONSTRAINT [PK_sales_void_id] PRIMARY KEY CLUSTERED ([sales_id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]) ON [PRIMARY] END";
                executequery(query, "Database Updated successfully!");
                checkBox10.Checked = false;
            }
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {

            if (checkBox9.Checked)
            {
                string query = "if not exists (select column_name from INFORMATION_SCHEMA.columns where table_name = 'systemuser' and column_name = 'AllowDisplayQty') ALTER TABLE [dbo].[systemuser] ADD AllowDisplayQty BIT DEFAULT(0) NOT NULL;if not exists (select column_name from INFORMATION_SCHEMA.columns where table_name = 'systemuser' and column_name = 'AllowDisplayCost') ALTER TABLE [dbo].[systemuser] ADD AllowDisplayCost BIT DEFAULT(0) NOT NULL;";
                 executequery(query, "Database Updated successfully!");
                string query1 = "ALTER Proc[dbo].[SPsystemuserInsert](@systemuser_code varchar(25), @systemuser_name varchar(150), @systemuser_designation varchar(150), @systemuser_mobile varchar(12), @systemuser_email varchar(150), @systemuser_address varchar(255), @systemuser_otherid varchar(35), @systemuser_username varchar(35), @systemuser_password varchar(25), @systemuser_dob date, @systemuser_doj date, @systemuser_till varchar(255), @systemuser_role varchar(50), @systemuser_image varchar(max), @systemuser_lock smallint, @warhouse varchar(255),@sales_rep varchar(255), @payment_type varchar(255), @price_selection varchar(255), @printer varchar(255), @debtor int, @systemuser_reprint int, @month_end_id int, @allow_negative_qty_sell int, @select_selling_price int, @print_balance_on_invoice int, @change_selling_price int, @desc_change int, @capture_barcode int, @sell_below_cost int, @activate_rounding int, @sales_return int, @line_discount int, @Item_Statastic int, @stock_browse int, @display_pole int, @view_cost int, @input_qty int, @end_discount int, @status int, @allow_print int, @warehouse_order varchar(100), @amend_cost int, @sale_zero int, @multi_warehouse int, @balance_age int, @supr_rule int, @view_profit int, @scr varchar(30), @machine_id int, @QPrint int, @LBViewExpiry int, @systemuser_roleid int, @Showlistdatalimits int, @AcSalesA4DirPrint bit, @CshOpenDrawer bit, @CrdOpenDrawer bit, @VouOpenDrawer bit, @EFTOpenDrawer bit, @AddQtyOnExisting bit, @SearchByCodeDesc bit, @AddDirectFromSearch bit, @SetDefaultSearch bit,@AllowDisplayQty bit, @AllowDisplayCost bit, @ID int OUTPUT) As INSERT INTO[dbo].[systemuser]([systemuser_code],[systemuser_name],[systemuser_designation],[systemuser_mobile],[systemuser_email],[systemuser_address],[systemuser_otherid],[systemuser_username],[systemuser_password],[systemuser_dob],[systemuser_doj],[systemuser_till],[systemuser_role],[systemuser_image],[systemuser_lock],[warhouse],[sales_rep],[payment_type],[price_selection],[printer],[debtor],[systemuser_reprint],[month_end_id],[allow_negative_qty_sell],[select_selling_price],[print_balance_on_invoice],[change_selling_price],[desc_change],[capture_barcode],[sell_below_cost],[activate_rounding],[sales_return],[line_discount],[Item_Statastic],[stock_browse],[display_pole],[view_cost],[input_qty],[end_discount],[status],[allow_print],[warehouse_order],[amend_cost],[sale_zero],[multi_warehouse],[balance_age],[supr_rule],[view_profit],[scr],[machine_id],[QPrint],[LBViewExpiry],[systemuser_roleid],[Showlistdatalimits],[AcSalesA4DirPrint], [CshOpenDrawer] , [CrdOpenDrawer] , [VouOpenDrawer] , [EFTOpenDrawer], [AddQtyOnExisting],[SearchByCodeDesc],[AddDirectFromSearch], [SetDefaultSearch],[AllowDisplayQty],[AllowDisplayCost]) VALUES(@systemuser_code, @systemuser_name, @systemuser_designation, @systemuser_mobile, @systemuser_email, @systemuser_address, @systemuser_otherid, @systemuser_username, @systemuser_password, @systemuser_dob, @systemuser_doj, @systemuser_till, @systemuser_role, @systemuser_image, @systemuser_lock, @warhouse, @sales_rep, @payment_type, @price_selection, @printer, @debtor, @systemuser_reprint, @month_end_id, @allow_negative_qty_sell, @select_selling_price, @print_balance_on_invoice, @change_selling_price, @desc_change, @capture_barcode, @sell_below_cost, @activate_rounding, @sales_return, @line_discount, @Item_Statastic, @stock_browse, @display_pole, @view_cost, @input_qty, @end_discount, @status, @allow_print, @warehouse_order, @amend_cost, @sale_zero, @multi_warehouse, @balance_age, @supr_rule, @view_profit, @scr, @machine_id, @QPrint, @LBViewExpiry, @systemuser_roleid, @Showlistdatalimits, @AcSalesA4DirPrint, @CshOpenDrawer, @CrdOpenDrawer, @VouOpenDrawer, @EFTOpenDrawer, @AddQtyOnExisting, @SearchByCodeDesc, @AddDirectFromSearch, @SetDefaultSearch, @AllowDisplayQty, @AllowDisplayCost) SET @ID = SCOPE_IDENTITY() Return @ID ;";
                executequery(query1, "Database Updated successfully!");
                string query2 = "ALTER Proc[dbo].[SPsystemuserUpdate](@systemuser_id int, @systemuser_code varchar(25), @systemuser_name varchar(150), @systemuser_designation varchar(150), @systemuser_mobile varchar(12), @systemuser_email varchar(150), @systemuser_address varchar(255), @systemuser_otherid varchar(35), @systemuser_username varchar(35), @systemuser_password varchar(25), @systemuser_dob date, @systemuser_doj date, @systemuser_till varchar(255), @systemuser_role varchar(50), @systemuser_image varchar(max), @warhouse varchar(255), @payment_type varchar(255), @price_selection varchar(255), @printer varchar(255), @debtor int, @systemuser_reprint int, @allow_negative_qty_sell int, @select_selling_price int, @print_balance_on_invoice int, @change_selling_price int, @desc_change int, @capture_barcode int, @sell_below_cost int, @activate_rounding int, @sales_return int, @line_discount int, @Item_Statastic int, @stock_browse int, @display_pole int, @view_cost int, @input_qty int, @end_discount int, @allow_print int, @amend_cost int, @multi_warehouse int, @balance_age int, @supr_rule int, @view_profit int, @scr varchar(30), @machine_id int, @QPrint int, @sales_rep varchar(255), @sale_zero int, @LBViewExpiry int, @systemuser_roleid int, @Showlistdatalimits int, @AcSalesA4DirPrint bit, @CshOpenDrawer bit, @CrdOpenDrawer bit, @VouOpenDrawer bit, @EFTOpenDrawer bit, @AddQtyOnExisting bit, @SearchByCodeDesc bit, @AddDirectFromSearch bit, @SetDefaultSearch bit, @AllowDisplayQty bit, @AllowDisplayCost bit) As UPDATE[dbo].[systemuser] SET[systemuser_code] = @systemuser_code,[systemuser_name] = @systemuser_name,[systemuser_designation] = @systemuser_designation,[systemuser_mobile] = @systemuser_mobile,[systemuser_email] = @systemuser_email,[systemuser_address] = @systemuser_address,[systemuser_otherid] = @systemuser_otherid,[systemuser_username] = @systemuser_username,[systemuser_password] = @systemuser_password,[systemuser_dob] = @systemuser_dob,[systemuser_doj] = @systemuser_doj,[systemuser_till] = @systemuser_till,[systemuser_role] = @systemuser_role,[systemuser_image] = @systemuser_image,[warhouse] = @warhouse,[payment_type] = @payment_type,[price_selection] = @price_selection,[printer] = @printer,[debtor] = @debtor,[systemuser_reprint] = @systemuser_reprint,[allow_negative_qty_sell] = @allow_negative_qty_sell,[select_selling_price] = @select_selling_price,[print_balance_on_invoice] = @print_balance_on_invoice,[change_selling_price] = @change_selling_price,[desc_change] = @desc_change,[capture_barcode] = @capture_barcode,[sell_below_cost] = @sell_below_cost,[activate_rounding] = @activate_rounding,[sales_return] = @sales_return,[line_discount] = @line_discount,[Item_Statastic] = @Item_Statastic,[stock_browse] = @stock_browse,[display_pole] = @display_pole,[view_cost] = @view_cost,[input_qty] = @input_qty,[end_discount] = @end_discount,[allow_print] = @allow_print,[amend_cost] = @amend_cost,[multi_warehouse] = @multi_warehouse,[balance_age] = @balance_age,[supr_rule] = @supr_rule,[view_profit] = @view_profit,[scr] = @scr,[machine_id] = @machine_id,[QPrint] = @QPrint,[sales_rep] = @sales_rep,[sale_zero] = @sale_zero,[LBViewExpiry] = @LBViewExpiry, [systemuser_roleid] = @systemuser_roleid, [Showlistdatalimits] = @Showlistdatalimits, [AcSalesA4DirPrint] = @AcSalesA4DirPrint, [CshOpenDrawer] = @CshOpenDrawer, [CrdOpenDrawer] = @CrdOpenDrawer, [VouOpenDrawer] = @VouOpenDrawer, [EFTOpenDrawer] = @EFTOpenDrawer, [AddQtyOnExisting] = @AddQtyOnExisting, [SearchByCodeDesc] = @SearchByCodeDesc, [AddDirectFromSearch] = @AddDirectFromSearch, [SetDefaultSearch] = @SetDefaultSearch, [AllowDisplayQty] = @AllowDisplayQty, [AllowDisplayCost] = @AllowDisplayCost  WHERE systemuser_id = @systemuser_id;";
                executequery(query2, "Database Updated successfully!");
                checkBox9.Checked = false;
            }
        }

        private void txtquery_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
