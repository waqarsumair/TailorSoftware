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

namespace EasyTailor
{
    public partial class MainForm : MetroFramework.Forms.MetroForm
    {
        public MainForm()
        {
            InitializeComponent();
        }
        string ConnectionString = "";
        private void MainForm_Load(object sender, EventArgs e)
        {
            ListUpdateBtn.Hide();
            ConnectionString = "Data Source=DESKTOP-T497H6H;Initial Catalog=EasyTailor;Integrated Security=True";
            FieldGridView.DataSource = GetAllFields();
            GetFieldsInPanel();
        }
        int lx = 8, ly = 9;
        int tx = 10, ty = 29;
        int i = 0;
        private void GetFieldsInPanel()
        {
            FieldPanel.Controls.Clear();
            foreach (DataRow row in Fields.Rows)
            {
                MetroFramework.Controls.MetroLabel label;
                label = new MetroFramework.Controls.MetroLabel();
                
                label.Text = row["FieldName"].ToString();
                label.AutoSize = false;
                label.Size = new Size(132, 20);
                FieldPanel.Controls.Add(label);
                label.Location = new Point(lx, ly);

                MetroFramework.Controls.MetroTextBox tb;
                tb = new MetroFramework.Controls.MetroTextBox();
                tb.Name = row["FieldName"].ToString();
                tb.Size = new Size(130, 30);
                tb.FontSize = MetroFramework.MetroTextBoxSize.Medium;
                tb.TextChanged += Tb_TextChange;
                FieldPanel.Controls.Add(tb);
                tb.Location = new Point(tx, ty);

                if (lx != 8) { 
                    if (i != 0) { 
                        ly = ly + 50; }
                    lx = 8;  }
                else {
                    lx = 143; }

                if (tx != 10) { 
                    if (i != 0) { 
                        ty = ty + 50; }
                    tx = 10;  }
                else {
                    tx = 146; }
                i++;
            }
            lx = 8; ly = 9;
            tx = 10; ty = 29;
            i = 0;
        }

        private void Tb_TextChange(object sender, EventArgs e)
        {
           
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void metroTabControl1_Click(object sender, EventArgs e)
        {
           
        }
        DataTable Fields = new DataTable();
        private DataTable GetAllFields()
        {
            Fields.Rows.Clear();
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(@"Select ID,ColumnName as 'FieldName' from TailorColumns where ColumnName LIKE '%' + @Search + '%' Order By ID asc", conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@Search", SearchFieldTB.Text);
                    SqlDataReader reader = cmd.ExecuteReader();

                    Fields.Load(reader);
                }
                return Fields;
            }
        }

        private void AddFieldBtn_Click(object sender, EventArgs e)
        {
            if (IsValidates())
            {
                if (AddFieldBtn.Text == "ADD")
                {
                    SaveRecord();
                }
                else
                {
                    UpdateRecord();
                }
                ClearScreen();
                FieldGridView.DataSource = GetAllFields();
                GetFieldsInPanel();
            }
        }

        private bool IsValidates()
        {
            if(FieldNameTB.Text.Trim() == "")
            {
                MessageBox.Show("Field Name is Required....", "EASY", MessageBoxButtons.OK, MessageBoxIcon.Information);
                FieldNameTB.Clear();
                FieldNameTB.Focus();
                return false;
            }
            return true;
        }

        private void ClearScreen()
        {
            FieldNameTB.Clear();
            AddFieldBtn.Text = "ADD";
            FieldNameTB.Focus();
        }

        private void SaveRecord()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Insert Into TailorColumns(ColumnName) VALUES(@ColumnName)", conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@ColumnName", FieldNameTB.Text);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void UpdateRecord()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Update TailorColumns Set ColumnName = @ColumnName where iD = @ID", conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@ColumnName", FieldNameTB.Text);
                    cmd.Parameters.AddWithValue("@ID", FieldIdTB.Text);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void DeleteRecord()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Delete From TailorColumns where iD = @ID", conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@ID", FieldIdTB.Text);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void dELETEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure you want to delete this field...","EASY",MessageBoxButtons.YesNo,MessageBoxIcon.Information) == DialogResult.Yes)
            {
                DeleteRecord();
                ClearScreen();
                FieldGridView.DataSource = GetAllFields();
                GetFieldsInPanel();
            }
        }

        private void FieldGridView_Click(object sender, EventArgs e)
        {
            if (FieldGridView.SelectedRows.Count > 0)
            {
                AddFieldBtn.Text = "UPDATE";
                DataGridViewRow item = FieldGridView.SelectedRows[0];
                FieldIdTB.Text = item.Cells[0].Value.ToString();
                FieldNameTB.Text = item.Cells[1].Value.ToString();
            }
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            SaveCustomerRecord();
            ClearCustomerScreen();
        }

        private void SaveCustomerRecord()
        {
            int i = 0;
            string query = "insert into CustomerDetails(CustomerName,CustomerNo";
            if (Fields.Rows.Count > 0)
            {
                foreach (DataRow row in Fields.Rows)
                {
                    query = query + ",[" + row["FieldName"].ToString() + "]";
                }
                query = query + ") VALUES (@CustomerName,@CustomerNo";
                foreach (DataRow row in Fields.Rows)
                {
                    string Para = row["FieldName"].ToString();
                    i++;
                    query = query + ", @" + Para.Last() + i;
                }
                i = 0;
                query = query + ")";
            }
            else { query = query + ") VALUES (@CustomerName,@CustomerNo)"; }
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@CustomerName", CustomerNameTB.Text);
                    cmd.Parameters.AddWithValue("@CustomerNo", CustomerNoTB.Text);
                    foreach (DataRow row in Fields.Rows)
                    {
                        string Para = row["FieldName"].ToString();
                        i++;
                        cmd.Parameters.AddWithValue("" + Para.Last() + i, FieldPanel.Controls[row["FieldName"].ToString()].Text);
                    }
                    cmd.Parameters.AddWithValue("@Description", DescriptionTB.Text);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void DressQtyTB_TextChanged(object sender, EventArgs e)
        {
            CheckNumericAndTotal();
        }
        decimal DressDecimal = 0, TotalDecimal = 0, AdvanceDecimal = 0, BalanceDecimal = 0;
        private void CheckNumericAndTotal()
        {
            if (DressQtyTB.Text == "") { DressDecimal = 0; } else { DressDecimal = Convert.ToDecimal(DressQtyTB.Text); }
            if (TotalAmountTB.Text == "") { TotalDecimal = 0; } else { TotalDecimal = Convert.ToDecimal(TotalAmountTB.Text); }
            if (AdvanceTB.Text == "") { AdvanceDecimal = 0; } else { AdvanceDecimal = Convert.ToDecimal(AdvanceTB.Text); }
            if (BalanceTB.Text == "") { BalanceDecimal = 0; } else { BalanceDecimal = Convert.ToDecimal(BalanceTB.Text); }
            BalanceTB.Text = (TotalDecimal - AdvanceDecimal).ToString();
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            ClearCustomerScreen();
        }

        private void ClearCustomerScreen()
        {
            InvoiceCustomerNameTB.Clear();
            InvoiceCustomerNoTB.Clear();
            CustomerNameTB.Clear();
            CustomerNoTB.Clear();
            DressQtyTB.Clear();
            TotalAmountTB.Clear();
            AdvanceTB.Clear();
            BalanceTB.Clear();
            DescriptionTB.Clear();
            eu = "";
            ClothId = "";
            ListUpdateBtn.Hide(); 
            GetFieldsInPanel();
            CustomerNameTB.Focus();
        }

        private void AllKeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.')) { e.Handled = true; }
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1)) { e.Handled = true; }
            if ((e.KeyChar == '+')) { e.Handled = true; }
            if ((e.KeyChar == '*')) { e.Handled = true; }
        }

        private void DressQtyTB_Click(object sender, EventArgs e)
        {

        }

        private void FindSearchBtn_Click(object sender, EventArgs e)
        {
            FindCustomer();
            GetCustomersinPanel();
        }

        private void GetCustomersinPanel()
        {
            CustomersPanel.Controls.Clear();
            foreach (DataRow row in Customers.Rows)
            {
                Panel p = new Panel();
                p.Size = new Size(243, 125);
                p.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

                MetroFramework.Controls.MetroLabel Name;
                Name = new MetroFramework.Controls.MetroLabel();
                Name.Text = row["CustomerName"].ToString();
                Name.AutoSize = false;
                Name.Size = new Size(234, 23);
                p.Controls.Add(Name);
                Name.Location = new Point(4, 18);

                MetroFramework.Controls.MetroLabel NameLabel;
                NameLabel = new MetroFramework.Controls.MetroLabel();
                NameLabel.Text = "Name";
                NameLabel.FontSize = MetroFramework.MetroLabelSize.Small;
                p.Controls.Add(NameLabel);
                NameLabel.Location = new Point(4, 4);

                MetroFramework.Controls.MetroLabel Number;
                Number = new MetroFramework.Controls.MetroLabel();
                Number.Text = row["CustomerNo"].ToString();
                Number.AutoSize = false;
                Number.Size = new Size(234, 23);
                p.Controls.Add(Number);
                Number.Location = new Point(4, 61);

                MetroFramework.Controls.MetroLabel NumberLabel;
                NumberLabel = new MetroFramework.Controls.MetroLabel();
                NumberLabel.Text = "PhoneNo";
                NumberLabel.FontSize = MetroFramework.MetroLabelSize.Small;
                p.Controls.Add(NumberLabel);
                NumberLabel.Location = new Point(4, 47);

                MetroFramework.Controls.MetroButton Invoice;
                Invoice = new MetroFramework.Controls.MetroButton();
                Invoice.Name = row["CustomerName"].ToString();
                Invoice.Click += Invoice_Button;
                Invoice.Text = "SHOW INVOICES";
                Invoice.Highlight = true;
                Invoice.Size = new Size(120, 30);
                p.Controls.Add(Invoice);
                Invoice.Location = new Point(4, 89);

                MetroFramework.Controls.MetroButton Edit;
                Edit = new MetroFramework.Controls.MetroButton();
                Edit.Name = row["ID"].ToString();
                Edit.Click += Edit_Button;
                Edit.BackgroundImage = Properties.Resources.edit;
                Edit.BackgroundImageLayout = ImageLayout.Stretch;
                Edit.Highlight = true;
                Edit.Size = new Size(30, 30);
                p.Controls.Add(Edit);
                Edit.Location = new Point(130, 89);

                MetroFramework.Controls.MetroButton Delete;
                Delete = new MetroFramework.Controls.MetroButton();
                Delete.Name = row["ID"].ToString();
                Delete.Click += Delete_Button;
                Delete.BackgroundImage = Properties.Resources.delete;
                Delete.BackgroundImageLayout = ImageLayout.Stretch;
                Delete.Highlight = true;
                Delete.Size = new Size(30, 30);
                p.Controls.Add(Delete);
                Delete.Location = new Point(166, 89);

                MetroFramework.Controls.MetroButton Open;
                Open = new MetroFramework.Controls.MetroButton();
                Open.Name = row["ID"].ToString();
                Open.Click += Open_Button;
                Open.BackgroundImage = Properties.Resources.maximize;
                Open.BackgroundImageLayout = ImageLayout.Stretch;
                Open.Highlight = true;
                Open.Size = new Size(30, 30);
                p.Controls.Add(Open);
                Open.Location = new Point(203, 89);

                CustomersPanel.Controls.Add(p);
            }
        }

        private void Invoice_Button(object sender, EventArgs e)
        {
            MetroFramework.Controls.MetroButton btn = (MetroFramework.Controls.MetroButton)sender;
            GetInvoices(btn.Name);
            GetInvoicesInPanel();
        }

        private void GetInvoicesInPanel()
        {
            CustomersPanel.Controls.Clear();
            foreach (DataRow row in Invoices.Rows)
            {
                Panel p = new Panel();
                p.Size = new Size(255, 163);
                p.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

                MetroFramework.Controls.MetroLabel Date;
                Date = new MetroFramework.Controls.MetroLabel();
                Date.AutoSize = false;
                Date.Size = new Size(118, 23);
                Date.Text = row["Date"].ToString();
                p.Controls.Add(Date);
                Date.Location = new Point(3, 19);

                MetroFramework.Controls.MetroLabel DDate;
                DDate = new MetroFramework.Controls.MetroLabel();
                DDate.AutoSize = false;
                DDate.Size = new Size(122, 23);
                DDate.Text = row["DeliveryDate"].ToString();
                p.Controls.Add(DDate);
                DDate.Location = new Point(127, 19);

                MetroFramework.Controls.MetroLabel DateLabel;
                DateLabel = new MetroFramework.Controls.MetroLabel();
                DateLabel.FontSize = MetroFramework.MetroLabelSize.Small;
                DateLabel.Text = "Date";
                p.Controls.Add(DateLabel);
                DateLabel.Location = new Point(3, 5);

                MetroFramework.Controls.MetroLabel DDateLabel;
                DDateLabel = new MetroFramework.Controls.MetroLabel();
                DDateLabel.FontSize = MetroFramework.MetroLabelSize.Small;
                DDateLabel.Text = "Delivery Date";
                p.Controls.Add(DDateLabel);
                DDateLabel.Location = new Point(127, 5);

               

                Panel pl = new Panel();
                pl.Size = new Size(246, 1);
                p.Controls.Add(pl);
                pl.Location = new Point(3, 40);

                MetroFramework.Controls.MetroLabel Name;
                Name = new MetroFramework.Controls.MetroLabel();
                Name.AutoSize = false;
                Name.Size = new Size(234, 23);
                Name.Text = row["CustomerName"].ToString();
                p.Controls.Add(Name);
                Name.Location = new Point(3, 56);

                MetroFramework.Controls.MetroLabel NameLabel;
                NameLabel = new MetroFramework.Controls.MetroLabel();
                NameLabel.FontSize = MetroFramework.MetroLabelSize.Small;
                NameLabel.Text = "Name";
                p.Controls.Add(NameLabel);
                NameLabel.Location = new Point(3, 42);

                MetroFramework.Controls.MetroLabel Qty;
                Qty = new MetroFramework.Controls.MetroLabel();
                Qty.AutoSize = false;
                Qty.Size = new Size(56, 15);
                Qty.Text = row["DressQty"].ToString();
                p.Controls.Add(Qty);
                Qty.Location = new Point(3, 97);

                MetroFramework.Controls.MetroLabel Total;
                Total = new MetroFramework.Controls.MetroLabel();
                Total.AutoSize = false;
                Total.Size = new Size(70, 15);
                Total.Text = row["TotalAmount"].ToString();
                p.Controls.Add(Total);
                Total.Location = new Point(67, 97);

                MetroFramework.Controls.MetroLabel Advance;
                Advance = new MetroFramework.Controls.MetroLabel();
                Advance.AutoSize = false;
                Advance.Size = new Size(46, 15);
                Advance.Text = row["Advance"].ToString();
                p.Controls.Add(Advance);
                Advance.Location = new Point(145, 97);

                MetroFramework.Controls.MetroLabel AdvanceLabel;
                AdvanceLabel = new MetroFramework.Controls.MetroLabel();
                AdvanceLabel.FontSize = MetroFramework.MetroLabelSize.Small;
                AdvanceLabel.Text = "Advance";
                p.Controls.Add(AdvanceLabel);
                AdvanceLabel.Location = new Point(141, 82);

                MetroFramework.Controls.MetroLabel Balance;
                Balance = new MetroFramework.Controls.MetroLabel();
                Balance.AutoSize = false;
                Balance.Size = new Size(41, 15);
                Balance.Text = row["Balance"].ToString();
                p.Controls.Add(Balance);
                Balance.Location = new Point(201, 97);


                MetroFramework.Controls.MetroLabel TotalLabel;
                TotalLabel = new MetroFramework.Controls.MetroLabel();
                TotalLabel.FontSize = MetroFramework.MetroLabelSize.Small;
                TotalLabel.Text = "Total Amount";
                p.Controls.Add(TotalLabel);
                TotalLabel.Location = new Point(63, 82);


                MetroFramework.Controls.MetroLabel BalanceLabel;
                BalanceLabel = new MetroFramework.Controls.MetroLabel();
                BalanceLabel.FontSize = MetroFramework.MetroLabelSize.Small;
                BalanceLabel.Text = "Balance";
                p.Controls.Add(BalanceLabel);
                BalanceLabel.Location = new Point(197, 82);


               

                MetroFramework.Controls.MetroLabel QtyLabel;
                QtyLabel = new MetroFramework.Controls.MetroLabel();
                QtyLabel.FontSize = MetroFramework.MetroLabelSize.Small;
                QtyLabel.Text = "Dress Qty";
                p.Controls.Add(QtyLabel);
                QtyLabel.Location = new Point(3, 82);

                MetroFramework.Controls.MetroLabel Description;
                Description = new MetroFramework.Controls.MetroLabel();
                Description.AutoSize = false;
                Description.Size = new Size(162, 23);
                Description.Text = row["Description"].ToString();
                p.Controls.Add(Description);
                Description.Location = new Point(3, 133);

                MetroFramework.Controls.MetroLabel DescriptionLabel;
                DescriptionLabel = new MetroFramework.Controls.MetroLabel();
                DescriptionLabel.FontSize = MetroFramework.MetroLabelSize.Small;
                DescriptionLabel.Text = "Description";
                p.Controls.Add(DescriptionLabel);
                DescriptionLabel.Location = new Point(3, 118);

                MetroFramework.Controls.MetroButton Edit;
                Edit = new MetroFramework.Controls.MetroButton();
                Edit.Name = row["ID"].ToString();
                //Edit.Click += Edit_Button;
                Edit.BackgroundImage = Properties.Resources.edit;
                Edit.BackgroundImageLayout = ImageLayout.Stretch;
                Edit.Highlight = true;
                Edit.Size = new Size(30, 30);
                p.Controls.Add(Edit);
                Edit.Location = new Point(183, 128);

                MetroFramework.Controls.MetroButton Delete;
                Delete = new MetroFramework.Controls.MetroButton();
                Delete.Name = row["ID"].ToString();
                //Delete.Click += Delete_Button;
                Delete.BackgroundImage = Properties.Resources.delete;
                Delete.BackgroundImageLayout = ImageLayout.Stretch;
                Delete.Highlight = true;
                Delete.Size = new Size(30, 30);
                p.Controls.Add(Delete);
                Delete.Location = new Point(219, 128);

                CustomersPanel.Controls.Add(p);
            }
        }

        DataTable Invoices = new DataTable();
        private DataTable GetInvoices(string cname)
        {
            Invoices.Rows.Clear();
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(@"Select ID,format(Date,'dd-MMM-yyyy') as 'Date',format(DeliveryDate,'dd-MMM-yyyy') as 'DeliveryDate',CustomerName,CustomerNo,DressQty,TotalAmount,Advance,Balance,Description from Invoices where CustomerName = @CustomerName", conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@CustomerName", cname);
                    SqlDataReader reader = cmd.ExecuteReader();
                    Invoices.Load(reader);
                }
                return Invoices;
            }
        }

        private void Open_Button(object sender, EventArgs e)
        {
            MetroFramework.Controls.MetroButton btn = (MetroFramework.Controls.MetroButton)sender;
            eu = "Update";
            ClothId = btn.Name;
            CheckCustomerDuplicate(btn.Name);
            metroTabControl1.SelectedTab = NewTab;
            DressQtyTB.Focus();
        }

        string eu = "";
        string ClothId = "";
        private void Edit_Button(object sender, EventArgs e)
        {
            MetroFramework.Controls.MetroButton btn = (MetroFramework.Controls.MetroButton)sender;
            eu = "Update";
            ClothId = btn.Name;
            CheckCustomerDuplicate(btn.Name);
            metroTabControl1.SelectedTab = NewTab;
            ListUpdateBtn.Show();
            CustomerNameTB.Focus();
        }
        private DataTable CheckCustomerDuplicate(string ID)
        {
            DataTable dodb = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Select * from CustomerDetails where ID = @ID", conn))
                {
                    cmd.Parameters.AddWithValue("@ID", ID);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); while (reader.Read())
                    {
                        CustomerNameTB.Text = reader["CustomerName"].ToString();
                        CustomerNoTB.Text = reader["CustomerNo"].ToString();

                        foreach (DataRow row in Fields.Rows)
                        {
                            FieldPanel.Controls[row["FieldName"].ToString()].Text = reader[row["FieldName"].ToString()].ToString();
                        }
                    }
                    dodb.Load(reader);
                }
                return dodb;
            }
        }


        private void Delete_Button(object sender, EventArgs e)
        {
            MetroFramework.Controls.MetroButton btn = (MetroFramework.Controls.MetroButton)sender;
            if (MessageBox.Show("Are you sure you want to delete this field...", "EASY", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                DeleteCustomerRecord(btn.Name);
                FindCustomer();
                GetCustomersinPanel();
            }
        }

        private void DeleteCustomerRecord(string Id)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Delete From CustomerDetails where ID = @ID", conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@ID", Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        DataTable Customers = new DataTable();
        private DataTable FindCustomer()
        {
            Customers.Rows.Clear();
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(@"Select ID,CustomerName,CustomerNo from CustomerDetails where isnull(CustomerName,'') + ' ' + ISNULL(CustomerNo,'') LIKE '%' + @Search +'%'", conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@Search", FindSearchTB.Text);
                    SqlDataReader reader = cmd.ExecuteReader();

                    Customers.Load(reader);
                }
                return Customers;
            }
        }


        private void NewTab_Click(object sender, EventArgs e)
        {

        }

        private void Button9_Click(object sender, EventArgs e)
        {

        }

        private void MetroLabel8_Click(object sender, EventArgs e)
        {

        }

        private void HomeTab_Click(object sender, EventArgs e)
        {

        }

        private void metroTextBox2_TextChanged(object sender, EventArgs e)
        {
            if (eu == "")
            {
                CheckCustomerDuplicate();
                if (CustomerNameTB.Text == Name)
                {
                    errorProvider1.SetError(CustomerNameTB, "This Customer Is Already Used");
                }
                else
                {
                    errorProvider1.SetError(CustomerNameTB, null);
                }
                if (CustomerNameTB.Text == "")
                {
                    errorProvider1.SetError(CustomerNameTB, null);
                }
            }
            InvoiceCustomerNameTB.Text = CustomerNameTB.Text;
        }
        string Name = "";
        private DataTable CheckCustomerDuplicate()
        {
            Name = "";
            DataTable dodb = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("select CustomerName from CustomerDetails where CustomerName = @CustomerName", conn))
                {
                    cmd.Parameters.AddWithValue("@CustomerName", CustomerNameTB.Text);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); while (reader.Read())
                    {
                        Name = reader["CustomerName"].ToString();
                    }
                    dodb.Load(reader);
                }
                return dodb;
            }
        }

        private void InvoiceCustomerNoTB_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void CustomerNoTB_TextChanged(object sender, EventArgs e)
        {
            InvoiceCustomerNoTB.Text = CustomerNoTB.Text;
        }

        private void ListUpdateBtn_Click(object sender, EventArgs e)
        {
            ClearCustomerScreen();
            UpdateClothDetail();
        }

        private void UpdateClothDetail()
        {
            int i = 0;
            string query = "Update CustomerDetails Set CustomerName = @CustomerName,CustomerNo = @CustomerNo";
            if (Fields.Rows.Count > 0)
            {
                foreach (DataRow row in Fields.Rows)
                {
                    string Para = row["FieldName"].ToString();
                    i++;

                    query = query + ",[" + row["FieldName"].ToString() + "] = @" + Para.Last() + i;
                }
                query = query + " Where ID = @ID";
                i = 0;
            }
            else { query = query + " Where ID = @ID"; }
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@ID",ClothId);
                    cmd.Parameters.AddWithValue("@CustomerName", CustomerNameTB.Text);
                    cmd.Parameters.AddWithValue("@CustomerNo", CustomerNoTB.Text);
                    foreach (DataRow row in Fields.Rows)
                    {
                        string Para = row["FieldName"].ToString();
                        i++;
                        cmd.Parameters.AddWithValue("" + Para.Last() + i, FieldPanel.Controls[row["FieldName"].ToString()].Text);
                    }
                    cmd.Parameters.AddWithValue("@Description", DescriptionTB.Text);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void panel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            TailorPrintPreview.Document = TailorPrint;
            TailorPrintPreview.ShowDialog();
        }

        private void TailorPrint_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Rectangle rect1 = new Rectangle(0, 0, 850, 100);
            Rectangle rect2 = new Rectangle(200, 80, 450, 25);
            Rectangle rect3 = new Rectangle(0, 110, 850, 25);
            Rectangle rect4 = new Rectangle(0, 140, 850, 25);
            Rectangle rect5 = new Rectangle(10, 205, 200, 200);
           
            e.Graphics.FillRectangle(Brushes.DarkGreen,rect2);
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;

            e.Graphics.DrawString("CHIEF TAILORS", new Font("Arial Rounded MT", 36, FontStyle.Bold), Brushes.Red, rect1, stringFormat);
            e.Graphics.DrawString("GENTS SPECIALIST", new Font("Arial Rounded MT", 18, FontStyle.Bold), Brushes.White, rect2, stringFormat);

            e.Graphics.DrawString("ستوں روڈ ، ٹھٹہ (سندہ)۔", new Font("Jameel Noori Nastaleeq", 18, FontStyle.Bold), Brushes.DarkGreen, rect3, stringFormat);

            e.Graphics.DrawString("MOB: 0321-3270001", new Font("Arial Rounded MT", 18, FontStyle.Regular), Brushes.DarkGreen, rect4, stringFormat);

            Pen blackPen4 = new Pen(Color.DarkGreen, 1);
            PointF point4 = new PointF(15, 170);
            PointF point5 = new PointF(830, 170);
            e.Graphics.DrawLine(blackPen4, point4, point5);

            e.Graphics.DrawString("Proprietor:", new Font("Arial Rounded MT", 18, FontStyle.Regular), Brushes.DarkGreen, new Point(10, 170));
            e.Graphics.DrawString("Veeram & Harri Lal", new Font("Arial Rounded MT", 18, FontStyle.Bold), Brushes.DarkGreen, rect5);

            Pen blackPen = new Pen(Color.DarkGreen, 1);
            e.Graphics.DrawRectangle(blackPen, rect5);

            e.Graphics.DrawString("No.____________", new Font("Microsft Sans Sarif", 10, FontStyle.Regular), Brushes.Black, new Point(240, 200));
            e.Graphics.DrawString(SnoTB.Text, new Font("Microsft Sans Sarif", 12, FontStyle.Regular), Brushes.Black, new Point(275, 195));

            e.Graphics.DrawString("Date: _______________", new Font("Microsft Sans Sarif", 10, FontStyle.Regular), Brushes.Black, new Point(400, 200));
            e.Graphics.DrawString(DateTime.Parse(StandardDatePicker.Text).ToString("dd-MMM-yyyy"), new Font("Microsft Sans Sarif", 12, FontStyle.Regular), Brushes.Black, new Point(440, 195));

            e.Graphics.DrawString("Delivery Date: _______________", new Font("Microsft Sans Sarif", 10, FontStyle.Regular), Brushes.Black, new Point(600, 200));
            e.Graphics.DrawString(DateTime.Parse(DeliveryDatePicker.Text).ToString("dd-MMM-yyyy"), new Font("Microsft Sans Sarif", 12, FontStyle.Regular), Brushes.Black, new Point(695, 195));

            e.Graphics.DrawString("Name: ___________________________________ نام", new Font("Microsft Sans Sarif", 10, FontStyle.Regular), Brushes.Black, new Point(240, 230));
            e.Graphics.DrawString("MUHAMMAD ARSLAN ABBASI", new Font("Microsft Sans Sarif", 10, FontStyle.Regular), Brushes.Black, new Point(288, 230));

            e.Graphics.DrawString("Qty: ___________________________________ نام", new Font("Microsft Sans Sarif", 10, FontStyle.Regular), Brushes.Black, new Point(240, 260));
            e.Graphics.DrawString("15", new Font("Microsft Sans Sarif", 10, FontStyle.Regular), Brushes.Black, new Point(288, 260));

        }

        private void BalanceTB_TextChanged(object sender, EventArgs e)
        {
            CheckNumericAndTotal();
        }

        private void AdvanceTB_TextChanged(object sender, EventArgs e)
        {
            CheckNumericAndTotal();
        }

        private void TotalAmountTB_TextChanged(object sender, EventArgs e)
        {
            CheckNumericAndTotal();
        }

        private void SearchBtn_Click(object sender, EventArgs e)
        {
            FieldGridView.DataSource = GetAllFields();
        }

        private void FieldResetBtn_Click(object sender, EventArgs e)
        {
            FieldNameTB.Clear();
            AddFieldBtn.Text = "ADD";
            SearchFieldTB.Clear();
            FieldNameTB.Focus();
        }
    }
}
