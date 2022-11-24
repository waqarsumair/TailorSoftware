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
            ConnectionString = "Data Source=DESKTOP-966OP3S;Initial Catalog=EasyTailor;Integrated Security=True";
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
        }

        //private void GetCustomersinPanel()
        //{
        //    CustomersPanel.Controls.Clear();
        //    foreach (DataRow row in Customers.Rows)
        //    {
        //        Panel p = new Panel();
        //        p.Size = new Size(243, 125);
        //        p.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

        //        MetroFramework.Controls.MetroLabel Name;
        //        Name = new MetroFramework.Controls.MetroLabel();
        //        Name.Text = row["CustomerName"].ToString();
        //        Name.AutoSize = false;
        //        Name.Size = new Size(234, 23);
        //        p.Controls.Add(Name);
        //        Name.Location = new Point(4, 18);

        //        MetroFramework.Controls.MetroLabel NameLabel;
        //        NameLabel = new MetroFramework.Controls.MetroLabel();
        //        NameLabel.Text = "Name";
        //        NameLabel.FontSize = MetroFramework.MetroLabelSize.Small;
        //        p.Controls.Add(NameLabel);
        //        NameLabel.Location = new Point(4, 4);

        //        MetroFramework.Controls.MetroLabel Number;
        //        Number = new MetroFramework.Controls.MetroLabel();
        //        Number.Text = row["CustomerNo"].ToString();
        //        Number.AutoSize = false;
        //        Number.Size = new Size(234, 23);
        //        p.Controls.Add(Number);
        //        Number.Location = new Point(4, 61);

        //        MetroFramework.Controls.MetroLabel NumberLabel;
        //        NumberLabel = new MetroFramework.Controls.MetroLabel();
        //        NumberLabel.Text = "PhoneNo";
        //        NumberLabel.FontSize = MetroFramework.MetroLabelSize.Small;
        //        p.Controls.Add(NumberLabel);
        //        NumberLabel.Location = new Point(4, 47);

        //        MetroFramework.Controls.MetroButton Invoice;
        //        Invoice = new MetroFramework.Controls.MetroButton();
        //        Invoice.Name = row["CustomerName"].ToString();
        //        Invoice.Click += Invoice_Button;
        //        Invoice.Text = "SHOW INVOICES";
        //        Invoice.Highlight = true;
        //        Invoice.Size = new Size(120, 30);
        //        p.Controls.Add(Invoice);
        //        Invoice.Location = new Point(4, 89);

        //        MetroFramework.Controls.MetroButton Edit;
        //        Edit = new MetroFramework.Controls.MetroButton();
        //        Edit.Name = row["ID"].ToString();
        //        Edit.Click += Edit_Button;
        //        Edit.BackgroundImage = Properties.Resources.edit;
        //        Edit.BackgroundImageLayout = ImageLayout.Stretch;
        //        Edit.Highlight = true;
        //        Edit.Size = new Size(30, 30);
        //        p.Controls.Add(Edit);
        //        Edit.Location = new Point(130, 89);

        //        MetroFramework.Controls.MetroButton Delete;
        //        Delete = new MetroFramework.Controls.MetroButton();
        //        Delete.Name = row["ID"].ToString();
        //        Delete.Click += Delete_Button;
        //        Delete.BackgroundImage = Properties.Resources.delete;
        //        Delete.BackgroundImageLayout = ImageLayout.Stretch;
        //        Delete.Highlight = true;
        //        Delete.Size = new Size(30, 30);
        //        p.Controls.Add(Delete);
        //        Delete.Location = new Point(166, 89);

        //        MetroFramework.Controls.MetroButton Open;
        //        Open = new MetroFramework.Controls.MetroButton();
        //        Open.Name = row["ID"].ToString();
        //        Open.Click += Open_Button;
        //        Open.BackgroundImage = Properties.Resources.maximize;
        //        Open.BackgroundImageLayout = ImageLayout.Stretch;
        //        Open.Highlight = true;
        //        Open.Size = new Size(30, 30);
        //        p.Controls.Add(Open);
        //        Open.Location = new Point(203, 89);

        //        CustomersPanel.Controls.Add(p);
        //    }
        //}

        DataTable Invoices = new DataTable();
        private DataTable GetInvoices(string cname)
        {
            Invoices.Rows.Clear();
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(@"Select * from Invoices where CustomerName = @CustomerName", conn))
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
