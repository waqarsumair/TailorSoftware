using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
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
            LanguageCombo.SelectedValueChanged -= LanguageCombo_SelectedValueChanged;
            GetSettings();
            ListUpdateBtn.Hide();
            ConnectionString = "Data Source=DESKTOP-966OP3S;Initial Catalog=EasyTailor;Integrated Security=True";
            FieldGridView.DataSource = GetAllFields();
            GetFieldsInPanel();
            pictureBox1.ImageLocation = @"" + ShopNameTB.Text + ".jpg";
            GetInvoiceNo();
            LangauageChanger();
            LanguageCombo.SelectedValueChanged += LanguageCombo_SelectedValueChanged;
        }
        private void LangauageChanger()
        {
            if (LanguageCombo.Text == "ENGLISH")
            {
                RightLeftFunction(ContentAlignment.MiddleLeft, HorizontalAlignment.Left);
            }
            else
            {
                RightLeftFunction(ContentAlignment.MiddleRight, HorizontalAlignment.Right);
            }

            GetLanguages();
            SetLangauage();
        }

        private void SetLangauage()
        {
     
            ClothDetailLabel.Text = lan[0]; CustomerNameLabel.Text = lan[1]; CustomerNoLabel.Text = lan[2];
            NewInvoiceLabel.Text = lan[3]; DateLabel.Text = lan[4]; DeliveryDateLabel.Text = lan[5];
            InvoiceNoLabel.Text = lan[6]; InvoiceCustomerLabel.Text = lan[7]; InvoiceCustomerNoLabel.Text = lan[8];
            DressQtyLabel.Text = lan[9]; TotalAmountLabel.Text = lan[10]; AdvanceLabel.Text = lan[11];
            BalanceLabel.Text = lan[12]; DescriptionLabel.Text = lan[13];

            metroButton2.Text = lan[14]; AddBtn.Text = lan[15]; HomeTab.Text = lan[16]; NewTab.Text = lan[17];
            FindTab.Text = lan[18]; AddCloth.Text = lan[19]; SettingTab.Text = lan[20];
            FindCustomerLabel.Text = lan[18]; FindSearchLabel.Text = lan[21]; FindSearchBtn.Text = lan[21]; 
            FieldNameLabel.Text = lan[22]; SearchFieldLabel.Text = lan[23]; AddFieldBtn.Text = lan[15];
            FieldResetBtn.Text = lan[14]; SearchBtn.Text = lan[21];


            ShopNameLabel.Text = lan[24]; ShopAddressLabel.Text = lan[25]; LanguageLabel.Text = lan[26];
            PrintPageLabel.Text = lan[27]; ShopQuoteLabel.Text = lan[28]; PrintNoteLabel.Text = lan[29];
            metroLabel16.Text = lan[30]; metroLabel18.Text = lan[31]; metroLabel17.Text = lan[32];
            metroButton1.Text = lan[33];
        }

        string[] lan = { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
        private DataTable GetLanguages()
        {
            DataTable dodb = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Select " + LanguageCombo.Text + " From Languages", conn))
                {
                    int i = 0;
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); while (reader.Read())
                    {
                        lan[i] = reader[LanguageCombo.Text].ToString();
                        i++;
                    }
                    dodb.Load(reader);
                }
                return dodb;
            }
        }

        private void RightLeftFunction(ContentAlignment co, HorizontalAlignment ha)
        {
            //New Customer Label
            ClothDetailLabel.TextAlign = co;
            CustomerNameLabel.TextAlign = co;
            CustomerNoLabel.TextAlign = co;
            NewInvoiceLabel.TextAlign = co;
            DateLabel.TextAlign = co;
            DeliveryDateLabel.TextAlign = co;
            InvoiceNoLabel.TextAlign = co;
            InvoiceCustomerLabel.TextAlign = co;
            InvoiceCustomerNoLabel.TextAlign = co;
            DressQtyLabel.TextAlign = co;
            TotalAmountLabel.TextAlign = co;
            AdvanceLabel.TextAlign = co;
            BalanceLabel.TextAlign = co;
            DescriptionLabel.TextAlign = co;
            //New Customer Text Boxes
            CustomerNameTB.TextAlign = ha;
            CustomerNoTB.TextAlign = ha;
            InvoiceCustomerNameTB.TextAlign = ha;
            InvoiceCustomerNoTB.TextAlign = ha;
            DescriptionTB.TextAlign = ha;

            //Find Customer Label
            FindSearchLabel.TextAlign = co;

            //Find Customer Text Boxes
            FindSearchTB.TextAlign = ha;

            //Add Cloth Field Label
            FieldNameLabel.TextAlign = co;
            SearchFieldLabel.TextAlign = co;

            //Add Cloth Field Text Boxes
            FieldNameTB.TextAlign = ha;
            SearchFieldTB.TextAlign = ha;

            //Settings Label
            ShopNameLabel.TextAlign = co;
            ShopAddressLabel.TextAlign = co;
            LanguageLabel.TextAlign = co;
            PrintPageLabel.TextAlign = co;
            ShopQuoteLabel.TextAlign = co;
            PrintNoteLabel.TextAlign = co;

            //Settings Text Boxes
            ShopNameTB.TextAlign = ha;
            ShopAddressTB.TextAlign = ha;
            ShopQuoteTB.TextAlign = ha;
            NoteTB.TextAlign = ha;
        }

        private DataTable GetInvoiceNo()
        {
            DataTable dodb = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("select isnull(InvoiceNo,0) as 'InvoiceNo' from Invoices order by InvoiceNo asc", conn))
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); while (reader.Read())
                    {
                        InvoiceNoTB.Text = Convert.ToDecimal((reader["InvoiceNo"].ToString()) + 1).ToString();
                    }
                    dodb.Load(reader);
                }
                return dodb;
            }
        }


        private void GetSettings()
        {
            ShopNameTB.Text = Properties.Settings.Default.ShopName;
            ShopAddressTB.Text = Properties.Settings.Default.ShopAddress;
            LanguageCombo.Text = Properties.Settings.Default.Language;
            PageCombo.Text = Properties.Settings.Default.PrintPage;
            ShopQuoteTB.Text = Properties.Settings.Default.ShopQuote;
            NoteTB.Text = Properties.Settings.Default.PrintNote;
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
                if (AddFieldBtn.Text == "UPDATE" || AddFieldBtn.Text == "اپڈیٹ")
                {
                    RenameColumn();
                    UpdateRecord();
                    
                }
                else
                {
                    AddColumn();
                    SaveRecord();
                }
                ClearScreen();
                FieldGridView.DataSource = GetAllFields();
                GetFieldsInPanel();
            }
        }

        private void AddColumn()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("ALTER TABLE CustomerDetails ADD " + FieldNameTB.Text + " nvarchar(max);", conn))
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void DeleteColumn()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("ALTER TABLE CustomerDetails DROP COLUMN " + oldname + ";", conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@ColumnName", FieldNameTB.Text);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        string oldname = "";
        private void RenameColumn()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("EXEC sp_RENAME 'CustomerDetails." + oldname + "', '" + FieldNameTB.Text + "', 'COLUMN'", conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@ColumnName", FieldNameTB.Text);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private bool IsValidates()
        {
            if(FieldNameTB.Text.Trim() == "")
            {
                MessageBox.Show(lan[37], "EASY", MessageBoxButtons.OK, MessageBoxIcon.Information);
                FieldNameTB.Clear();
                FieldNameTB.Focus();
                return false;
            }
            return true;
        }

        private void ClearScreen()
        {
            FieldNameTB.Clear();
            AddFieldBtn.Text = lan[15];
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
            if(MessageBox.Show(lan[38],"EASY",MessageBoxButtons.YesNo,MessageBoxIcon.Information) == DialogResult.Yes)
            {
                CheckFieldUsedOrNot();
                if (fieldUsed != "")
                {
                    MessageBox.Show(lan[39], "EASY", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    DeleteColumn();
                    DeleteRecord();
                    ClearScreen();
                    FieldGridView.DataSource = GetAllFields();
                    GetFieldsInPanel();
                }
            }
        }
        string fieldUsed = "";
        private DataTable CheckFieldUsedOrNot()
        {
            fieldUsed = "";
            DataTable dodb = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Select " + oldname + " from CustomerDetails where " + oldname + " != ''", conn))
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); while (reader.Read())
                    {
                        fieldUsed = reader[oldname].ToString();
                    }
                    dodb.Load(reader);
                }
                return dodb;
            }
        }

        private void FieldGridView_Click(object sender, EventArgs e)
        {
            if (FieldGridView.SelectedRows.Count > 0)
            {
                AddFieldBtn.Text = lan[33];
                DataGridViewRow item = FieldGridView.SelectedRows[0];
                FieldIdTB.Text = item.Cells[0].Value.ToString();
                oldname = item.Cells[1].Value.ToString();
                FieldNameTB.Text = item.Cells[1].Value.ToString();
            }
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            if (CustomerNameTB.Text != "")
            {
                CheckCustomerDuplicate();
                if (CustomerNameTB.Text == Name)
                {
                    UpdateClothDetail();
                }
                else
                {
                    SaveCustomerRecord();
                }
                if (AddBtn.Text == "UPDATE" || AddBtn.Text == "اپڈیٹ")
                {
                    InvoiceUpdateRecord(invoiceid);
                }
                else
                {
                    if (DressQtyTB.Text == "" || DressQtyTB.Text == "0")
                    {
                        MessageBox.Show(lan[40], "EASY", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        InvoiceSaveRecord();
                    }
                }
                if (MessageBox.Show(lan[41], "EASY", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    if (PageCombo.Text == "A4")
                    {
                        TailorA4Print.Print();
                    }
                    else
                    {
                        Tailor8MMPrint.Print();
                    }
                }
                ClearCustomerScreen();
            }
            else
            {
                MessageBox.Show(lan[44], "EASY", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void InvoiceUpdateRecord(string id)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Update Invoices SET Date = @Date,DeliveryDate = @DeliveryDate,CustomerName = @CustomerName,CustomerNo = @CustomerNo,DressQty = @DressQty,TotalAmount = @TotalAmount,Advance = @Advance,Balance = @Balance,Description = @Description where id = @Id", conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@Date", StandardDatePicker.Text);
                    cmd.Parameters.AddWithValue("@DeliveryDate", DeliveryDatePicker.Text);
                    cmd.Parameters.AddWithValue("@CustomerName", CustomerNameTB.Text);
                    cmd.Parameters.AddWithValue("@CustomerNo", CustomerNoTB.Text);
                    cmd.Parameters.AddWithValue("@DressQty", DressDecimal);
                    cmd.Parameters.AddWithValue("@TotalAmount", TotalDecimal);
                    cmd.Parameters.AddWithValue("@Advance", AdvanceDecimal);
                    cmd.Parameters.AddWithValue("@Balance", BalanceDecimal);
                    cmd.Parameters.AddWithValue("@Description", DescriptionTB.Text);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void InvoiceSaveRecord()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Insert Into Invoices(Date,DeliveryDate,CustomerName,CustomerNo,DressQty,TotalAmount,Advance,Balance,Description) VALUES(@Date,@DeliveryDate,@CustomerName,@CustomerNo,@DressQty,@TotalAmount,@Advance,@Balance,@Description)", conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@Date", StandardDatePicker.Text);
                    cmd.Parameters.AddWithValue("@DeliveryDate", DeliveryDatePicker.Text);
                    cmd.Parameters.AddWithValue("@CustomerName", CustomerNameTB.Text);
                    cmd.Parameters.AddWithValue("@CustomerNo", CustomerNoTB.Text);
                    cmd.Parameters.AddWithValue("@DressQty", DressDecimal);
                    cmd.Parameters.AddWithValue("@TotalAmount", TotalDecimal);
                    cmd.Parameters.AddWithValue("@Advance", AdvanceDecimal);
                    cmd.Parameters.AddWithValue("@Balance", BalanceDecimal);
                    cmd.Parameters.AddWithValue("@Description", DescriptionTB.Text);
                    cmd.ExecuteNonQuery();
                }
            }
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
            AddBtn.Text = lan[15];
            ListUpdateBtn.Hide();
            GetInvoiceNo();
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
                if (LanguageCombo.Text != "ENGLISH")
                {
                    Name.TextAlign = ContentAlignment.MiddleRight;
                }
                p.Controls.Add(Name);
                Name.Location = new Point(4, 18);

                MetroFramework.Controls.MetroLabel NameLabel;
                NameLabel = new MetroFramework.Controls.MetroLabel();
                if (LanguageCombo.Text != "ENGLISH")
                {
                    NameLabel.AutoSize = false;
                    NameLabel.Size = new Size(234, 23);
                    NameLabel.TextAlign = ContentAlignment.MiddleRight;
                }
                NameLabel.Text = lan[34];
                NameLabel.FontSize = MetroFramework.MetroLabelSize.Small;
                p.Controls.Add(NameLabel);
                NameLabel.Location = new Point(4, 0);

                MetroFramework.Controls.MetroLabel Number;
                Number = new MetroFramework.Controls.MetroLabel();
                Number.Text = row["CustomerNo"].ToString();
                Number.AutoSize = false;
                Number.Size = new Size(234, 23);
                if (LanguageCombo.Text != "ENGLISH")
                {
                    Number.TextAlign = ContentAlignment.MiddleRight;
                }
                p.Controls.Add(Number);
                Number.Location = new Point(4, 61);

                MetroFramework.Controls.MetroLabel NumberLabel;
                NumberLabel = new MetroFramework.Controls.MetroLabel();
                if (LanguageCombo.Text != "ENGLISH")
                {
                    NumberLabel.AutoSize = false;
                    NumberLabel.Size = new Size(234, 23);
                    NumberLabel.TextAlign = ContentAlignment.MiddleRight;
                }
                NumberLabel.Text = lan[35];
                NumberLabel.FontSize = MetroFramework.MetroLabelSize.Small;
                p.Controls.Add(NumberLabel);
                NumberLabel.Location = new Point(4, 42);

                MetroFramework.Controls.MetroButton Invoice;
                Invoice = new MetroFramework.Controls.MetroButton();
                Invoice.Name = row["CustomerName"].ToString();
                Invoice.Click += Invoice_Button;
                Invoice.Text = lan[36];
                Invoice.Highlight = true;
                Invoice.Size = new Size(120, 30);
                p.Controls.Add(Invoice);
                Invoice.Location = new Point(4, 89);

                MetroFramework.Controls.MetroButton Edit;
                Edit = new MetroFramework.Controls.MetroButton();
                Edit.Name = row["ID"].ToString();
                Edit.Tag = row["CustomerName"].ToString();
                Edit.Click += Edit_Button;
                Edit.BackgroundImage = Properties.Resources.edit;
                Edit.BackgroundImageLayout = ImageLayout.Stretch;
                Edit.Highlight = true;
                Edit.Size = new Size(30, 30);
                p.Controls.Add(Edit);
                Edit.Location = new Point(130, 89);

                MetroFramework.Controls.MetroButton Delete;
                Delete = new MetroFramework.Controls.MetroButton();
                Delete.Tag = row["CustomerName"].ToString();
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
                Open.Tag = row["CustomerName"].ToString();
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
                DateLabel.Text = lan[4];
                p.Controls.Add(DateLabel);
                DateLabel.Location = new Point(3, 5);

                MetroFramework.Controls.MetroLabel DDateLabel;
                DDateLabel = new MetroFramework.Controls.MetroLabel();
                DDateLabel.FontSize = MetroFramework.MetroLabelSize.Small;
                DDateLabel.Text = lan[5];
                p.Controls.Add(DDateLabel);
                DDateLabel.Location = new Point(127, 5);

                //Panel pl = new Panel();
                //pl.Size = new Size(246, 1);
                //p.Controls.Add(pl);
                //pl.Location = new Point(3, 40);

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
                NameLabel.Text = lan[34];
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

                MetroFramework.Controls.MetroLabel Balance;
                Balance = new MetroFramework.Controls.MetroLabel();
                Balance.AutoSize = false;
                Balance.Size = new Size(41, 15);
                Balance.Text = row["Balance"].ToString();
                p.Controls.Add(Balance);
                Balance.Location = new Point(201, 97);

                MetroFramework.Controls.MetroLabel BalanceLabel;
                BalanceLabel = new MetroFramework.Controls.MetroLabel();
                BalanceLabel.FontSize = MetroFramework.MetroLabelSize.Small;
                BalanceLabel.Text = lan[12];
                p.Controls.Add(BalanceLabel);
                BalanceLabel.Location = new Point(197, 82);

                MetroFramework.Controls.MetroLabel AdvanceLabel;
                AdvanceLabel = new MetroFramework.Controls.MetroLabel();
                AdvanceLabel.FontSize = MetroFramework.MetroLabelSize.Small;
                AdvanceLabel.Text = lan[11];
                p.Controls.Add(AdvanceLabel);
                AdvanceLabel.Location = new Point(141, 82);

               

                MetroFramework.Controls.MetroLabel TotalLabel;
                TotalLabel = new MetroFramework.Controls.MetroLabel();
                TotalLabel.FontSize = MetroFramework.MetroLabelSize.Small;
                TotalLabel.Text = lan[10];
                p.Controls.Add(TotalLabel);
                TotalLabel.Location = new Point(63, 82);

                MetroFramework.Controls.MetroLabel QtyLabel;
                QtyLabel = new MetroFramework.Controls.MetroLabel();
                QtyLabel.FontSize = MetroFramework.MetroLabelSize.Small;
                QtyLabel.Text = lan[9];
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
                DescriptionLabel.Text = lan[13];
                p.Controls.Add(DescriptionLabel);
                DescriptionLabel.Location = new Point(3, 118);

                MetroFramework.Controls.MetroButton Edit;
                Edit = new MetroFramework.Controls.MetroButton();
                Edit.Name = row["ID"].ToString();
                Edit.Tag = row["CustomerName"].ToString();
                Edit.Click += Invoice_Edit_Button;
                Edit.BackgroundImage = Properties.Resources.edit;
                Edit.BackgroundImageLayout = ImageLayout.Stretch;
                Edit.Highlight = true;
                Edit.Size = new Size(30, 30);
                p.Controls.Add(Edit);
                Edit.Location = new Point(183, 128);

                MetroFramework.Controls.MetroButton Delete;
                Delete = new MetroFramework.Controls.MetroButton();
                Delete.Name = row["ID"].ToString();
                Delete.Tag = row["CustomerName"].ToString();
                Delete.Click += Invoice_Delete_Button;
                Delete.BackgroundImage = Properties.Resources.delete;
                Delete.BackgroundImageLayout = ImageLayout.Stretch;
                Delete.Highlight = true;
                Delete.Size = new Size(30, 30);
                p.Controls.Add(Delete);
                Delete.Location = new Point(219, 128);

                CustomersPanel.Controls.Add(p);
            }
        }

        private void Invoice_Delete_Button(object sender, EventArgs e)
        {
            MetroFramework.Controls.MetroButton btn = (MetroFramework.Controls.MetroButton)sender;
            if (MessageBox.Show(lan[38], "EASY", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                DeleteInvoiceRecord(btn.Name);
                GetInvoices(btn.Tag.ToString());
                GetInvoicesInPanel();
            }
        }

        private void DeleteInvoiceRecord(string Id)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Delete From Invoices where ID = @ID", conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@ID", Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        string invoiceid = "";
        private void Invoice_Edit_Button(object sender, EventArgs e)
        {
            MetroFramework.Controls.MetroButton btn = (MetroFramework.Controls.MetroButton)sender;
            eu = "Update";
            AddBtn.Text = lan[33];
            ClothId = btn.Tag.ToString();
            GetClothDetail(btn.Tag.ToString());
            GetInvoiceDetail(btn.Name);
            invoiceid = btn.Name;
            metroTabControl1.SelectedTab = NewTab;
            DressQtyTB.Focus();
        }

        private DataTable GetInvoiceDetail(string ID)
        {
            DataTable dodb = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Select * from Invoices where Id = @Id", conn))
                {
                    cmd.Parameters.AddWithValue("@Id", ID);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); while (reader.Read())
                    {
                        StandardDatePicker.Text = reader["Date"].ToString();
                        DeliveryDatePicker.Text = reader["DeliveryDate"].ToString();
                        InvoiceCustomerNameTB.Text = reader["CustomerName"].ToString();
                        InvoiceCustomerNoTB.Text = reader["CustomerNo"].ToString();
                        DressQtyTB.Text = reader["DressQty"].ToString();
                        TotalAmountTB.Text = reader["TotalAmount"].ToString();
                        AdvanceTB.Text = reader["Advance"].ToString();
                        BalanceTB.Text = reader["Balance"].ToString();
                        DescriptionTB.Text = reader["Description"].ToString();
                    }
                    dodb.Load(reader);
                }
                return dodb;
            }
        }


        private DataTable GetClothDetail(string Name)
        {
            DataTable dodb = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Select * from CustomerDetails where CustomerName = @CustomerName", conn))
                {
                    cmd.Parameters.AddWithValue("@CustomerName", Name);
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
            ClothId = btn.Tag.ToString();
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
            ClothId = btn.Tag.ToString();
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
            CheckInvoicesCreatedorNot(btn.Tag.ToString());
            if (HaveInvoice != "")
            {
                if (MessageBox.Show(lan[42], "EASY", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    DeleteInvoicesRecord(btn.Tag.ToString());
                    DeleteCustomerRecord(btn.Name);
                    FindCustomer();
                    GetCustomersinPanel();
                }
            }
            else
            {
                if (MessageBox.Show(lan[38], "EASY", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    DeleteCustomerRecord(btn.Name);
                    FindCustomer();
                    GetCustomersinPanel();
                }
            }
        }
        private void DeleteInvoicesRecord(string CustomerName)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Delete From Invoices where CustomerName = '" + CustomerName + "'", conn))
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        string HaveInvoice = "";
        private DataTable CheckInvoicesCreatedorNot(string customer)
        {
            HaveInvoice = "";
            DataTable dodb = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Select CustomerName from Invoices Where CustomerName = '" + customer + "'", conn))
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader(); while (reader.Read())
                    {
                        HaveInvoice = reader["CustomerName"].ToString();
                    }
                    dodb.Load(reader);
                }
                return dodb;
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
                query = query + " Where CustomerName = @CustomerNames";
                i = 0;
            }
            else { query = query + " Where CustomerName = @CustomerNames"; }
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@CustomerNames", ClothId);
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
            e.Graphics.DrawString(InvoiceNoTB.Text, new Font("Microsft Sans Sarif", 12, FontStyle.Regular), Brushes.Black, new Point(275, 195));

            e.Graphics.DrawString("Date: _______________", new Font("Microsft Sans Sarif", 10, FontStyle.Regular), Brushes.Black, new Point(400, 200));
            e.Graphics.DrawString(DateTime.Parse(StandardDatePicker.Text).ToString("dd-MMM-yyyy"), new Font("Microsft Sans Sarif", 12, FontStyle.Regular), Brushes.Black, new Point(440, 195));

            e.Graphics.DrawString("Delivery Date: _______________", new Font("Microsft Sans Sarif", 10, FontStyle.Regular), Brushes.Black, new Point(600, 200));
            e.Graphics.DrawString(DateTime.Parse(DeliveryDatePicker.Text).ToString("dd-MMM-yyyy"), new Font("Microsft Sans Sarif", 12, FontStyle.Regular), Brushes.Black, new Point(695, 195));

            e.Graphics.DrawString("Name: ___________________________________ نام", new Font("Microsft Sans Sarif", 10, FontStyle.Regular), Brushes.Black, new Point(240, 230));
            e.Graphics.DrawString("MUHAMMAD ARSLAN ABBASI", new Font("Microsft Sans Sarif", 10, FontStyle.Regular), Brushes.Black, new Point(288, 230));

            e.Graphics.DrawString("Qty: ___________________________________ نام", new Font("Microsft Sans Sarif", 10, FontStyle.Regular), Brushes.Black, new Point(240, 260));
            e.Graphics.DrawString("15", new Font("Microsft Sans Sarif", 10, FontStyle.Regular), Brushes.Black, new Point(288, 260));

        }

        private void metroLabel17_Click(object sender, EventArgs e)
        {
            Image File;
            OpenFileDialog f = new OpenFileDialog();
            f.Filter = "Image files (*.jpg, *.png) | *.jpg; *.png";

            if (f.ShowDialog() == DialogResult.OK)
            {
                File = Image.FromFile(f.FileName);
                pictureBox1.Image = File;
            }
        }

        private void metroButton1_Click_1(object sender, EventArgs e)
        {
            Properties.Settings.Default.ShopName = ShopNameTB.Text;
            Properties.Settings.Default.ShopAddress = ShopAddressTB.Text;
            Properties.Settings.Default.Language = LanguageCombo.Text;
            Properties.Settings.Default.PrintPage = PageCombo.Text;
            Properties.Settings.Default.ShopQuote = ShopQuoteTB.Text;
            Properties.Settings.Default.PrintNote = NoteTB.Text;
            Properties.Settings.Default.PX = this.Location.X;
            Properties.Settings.Default.PY = this.Location.Y;
            pictureBox1.Image.Save(@"" + ShopNameTB.Text + ".jpg");
            Properties.Settings.Default.Save();

            MessageBox.Show(lan[43], "EASY", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void metroLabel18_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
            if (File.Exists(@"" + ShopNameTB.Text + ".jpg"))
            {
                try
                {
                    File.Delete(@"" + ShopNameTB.Text + ".jpg");
                }
                catch (Exception ex)
                {
                    //Do something
                }
            }
        }
        string rl = "Left";
        private void Tailor8MMPrint_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            if (rl == "Right")
            {
                e.Graphics.DrawImage(pictureBox1.Image, 5, 0, 50, 50);

                Rectangle rect1 = new Rectangle(0, 0, 285, 40);
                Rectangle rect2 = new Rectangle(0, 42, 285, 25);
                Rectangle rect3 = new Rectangle(0, 65, 285, 30);
                Rectangle rect4 = new Rectangle(0, 100, 285, 20);
                Rectangle rect5 = new Rectangle(2, 280, 279, 50);

                //e.Graphics.FillRectangle(Brushes.DarkGreen, rect2);
                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;

                e.Graphics.DrawString(ShopNameTB.Text, new Font("Arial Rounded MT", 13, FontStyle.Bold), Brushes.Black, rect1, stringFormat);
                e.Graphics.DrawString(ShopQuoteTB.Text, new Font("Arial Rounded MT", 10, FontStyle.Bold), Brushes.Black, rect2, stringFormat);

                e.Graphics.DrawString(ShopAddressTB.Text, new Font("Jameel Noori Nastaleeq", 12, FontStyle.Bold), Brushes.Black, rect3, stringFormat);

                e.Graphics.DrawString("Call : 0301-14499025", new Font("Arial Rounded MT", 10, FontStyle.Bold), Brushes.Black, rect4);

                Pen blackPen4 = new Pen(Color.DarkGreen, 1);
                PointF point4 = new PointF(0, 95);
                PointF point5 = new PointF(285, 95);
                e.Graphics.DrawLine(blackPen4, point4, point5);

                PointF point1 = new PointF(0, 120);
                PointF point2 = new PointF(285, 120);
                e.Graphics.DrawLine(blackPen4, point1, point2);

                Pen blackPen = new Pen(Color.DarkGreen, 1);
                e.Graphics.DrawRectangle(blackPen, rect5);

                e.Graphics.DrawString("Invoice No._________", new Font("Microsft Sans Sarif", 9, FontStyle.Regular), Brushes.Black, new Point(0, 130));
                e.Graphics.DrawString(InvoiceNoTB.Text, new Font("Microsft Sans Sarif", 10, FontStyle.Regular), Brushes.Black, new Point(90, 128));

                e.Graphics.DrawString("Date: _____________", new Font("Microsft Sans Sarif", 9, FontStyle.Regular), Brushes.Black, new Point(150, 130));
                e.Graphics.DrawString(DateTime.Parse(StandardDatePicker.Text).ToString("dd-MMM-yyyy"), new Font("Microsft Sans Sarif", 10, FontStyle.Regular), Brushes.Black, new Point(196, 128));

                e.Graphics.DrawString("D.Date: ___________", new Font("Microsft Sans Sarif", 9, FontStyle.Regular), Brushes.Black, new Point(150, 150));
                e.Graphics.DrawString(DateTime.Parse(DeliveryDatePicker.Text).ToString("dd-MMM-yyyy"), new Font("Microsft Sans Sarif", 10, FontStyle.Regular), Brushes.Black, new Point(196, 148));

                e.Graphics.DrawString("Name: _______________________________", new Font("Microsft Sans Sarif", 10, FontStyle.Regular), Brushes.Black, new Point(0, 180));
                e.Graphics.DrawString(CustomerNameTB.Text, new Font("Microsft Sans Sarif", 10, FontStyle.Regular), Brushes.Black, new Point(50, 178));

                e.Graphics.DrawString("Dress Qty: ______", new Font("Microsft Sans Sarif", 10, FontStyle.Regular), Brushes.Black, new Point(0, 210));
                e.Graphics.DrawString(DressQtyTB.Text, new Font("Microsft Sans Sarif", 10, FontStyle.Regular), Brushes.Black, new Point(70, 208));

                e.Graphics.DrawString("Total Amount: _________", new Font("Microsft Sans Sarif", 10, FontStyle.Regular), Brushes.Black, new Point(120, 210));
                e.Graphics.DrawString(TotalAmountTB.Text, new Font("Microsft Sans Sarif", 10, FontStyle.Regular), Brushes.Black, new Point(210, 208));

                e.Graphics.DrawString("Advance: ________", new Font("Microsft Sans Sarif", 10, FontStyle.Regular), Brushes.Black, new Point(0, 240));
                e.Graphics.DrawString(AdvanceTB.Text, new Font("Microsft Sans Sarif", 10, FontStyle.Regular), Brushes.Black, new Point(60, 238));

                e.Graphics.DrawString("Balance: _________", new Font("Microsft Sans Sarif", 10, FontStyle.Bold), Brushes.Black, new Point(137, 240));
                e.Graphics.DrawString(BalanceTB.Text, new Font("Microsft Sans Sarif", 10, FontStyle.Bold), Brushes.Black, new Point(203, 238));

                e.Graphics.DrawString("Note:", new Font("Microsft Sans Sarif", 10, FontStyle.Regular), Brushes.Black, new Point(0, 260));
                e.Graphics.DrawString(NoteTB.Text, new Font("Microsft Sans Sarif", 10, FontStyle.Regular), Brushes.Black, rect5);

                e.Graphics.DrawString("Software Developed By www.Easysoft.pk", new Font("Microsft Sans Sarif", 10, FontStyle.Regular), Brushes.Black, new Point(15, 330));
                e.Graphics.DrawString("Call : 0301-1499025 | 0318-0137714", new Font("Microsft Sans Sarif", 10, FontStyle.Regular), Brushes.Black, new Point(28, 345));
            }

            if (rl == "Left")
            {
                e.Graphics.DrawImage(pictureBox1.Image, 5, 0, 50, 50);

                Rectangle rect1 = new Rectangle(0, 0, 285, 40);
                Rectangle rect2 = new Rectangle(0, 42, 285, 25);
                Rectangle rect3 = new Rectangle(0, 65, 285, 30);
                Rectangle rect4 = new Rectangle(0, 100, 285, 20);
                Rectangle rect5 = new Rectangle(2, 280, 279, 50);

                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;

                e.Graphics.DrawString(ShopNameTB.Text, new Font("Arial Rounded MT", 13, FontStyle.Bold), Brushes.Black, rect1, stringFormat);
                e.Graphics.DrawString(ShopQuoteTB.Text, new Font("Arial Rounded MT", 10, FontStyle.Bold), Brushes.Black, rect2, stringFormat);

                e.Graphics.DrawString(ShopAddressTB.Text, new Font("Jameel Noori Nastaleeq", 12, FontStyle.Bold), Brushes.Black, rect3, stringFormat);
               
                e.Graphics.DrawString("Call : 0301-14499025", new Font("Arial Rounded MT", 10, FontStyle.Regular), Brushes.Black, rect4);

                Pen blackPen4 = new Pen(Color.DarkGreen, 1);
                PointF point4 = new PointF(0, 95);
                PointF point5 = new PointF(285, 95);
                e.Graphics.DrawLine(blackPen4, point4, point5);

                PointF point1 = new PointF(0, 120);
                PointF point2 = new PointF(285, 120);
                e.Graphics.DrawLine(blackPen4, point1, point2);

                Pen blackPen = new Pen(Color.DarkGreen, 1);
                e.Graphics.DrawRectangle(blackPen, rect5);

                StringFormat format = new StringFormat(StringFormatFlags.DirectionRightToLeft);
                e.Graphics.DrawString("بل نمبر ---------", new Font("Jameel Noori Nastaleeq", 9, FontStyle.Regular), Brushes.Black, new Point(260, 130), format);
                e.Graphics.DrawString(InvoiceNoTB.Text, new Font("Microsft Sans Sarif", 10, FontStyle.Regular), Brushes.Black, new Point(225, 126), format);

                e.Graphics.DrawString("تاریخ -----------------", new Font("Jameel Noori Nastaleeq", 9, FontStyle.Regular), Brushes.Black, new Point(130, 130), format);
                e.Graphics.DrawString(DateTime.Parse(StandardDatePicker.Text).ToString("dd-MMM-yyyy"), new Font("Microsft Sans Sarif", 10, FontStyle.Regular), Brushes.Black, new Point(20, 126));

                e.Graphics.DrawString("ادئیگی کی تاریخ -----------------", new Font("Jameel Noori Nastaleeq", 9, FontStyle.Regular), Brushes.Black, new Point(155, 150), format);
                e.Graphics.DrawString(DateTime.Parse(DeliveryDatePicker.Text).ToString("dd-MMM-yyyy"), new Font("Microsft Sans Sarif", 10, FontStyle.Regular), Brushes.Black, new Point(20, 146));

                e.Graphics.DrawString("نام -------------------------------------------------", new Font("Jameel Noori Nastaleeq", 10, FontStyle.Regular), Brushes.Black, new Point(280, 180), format);
                e.Graphics.DrawString(CustomerNameTB.Text, new Font("Microsft Sans Sarif", 10, FontStyle.Regular), Brushes.Black, new Point(260, 176), format);

                e.Graphics.DrawString("لباس کی تعداد ---------", new Font("Jameel Noori Nastaleeq", 10, FontStyle.Regular), Brushes.Black, new Point(260, 210), format);
                e.Graphics.DrawString(DressQtyTB.Text, new Font("Microsft Sans Sarif", 10, FontStyle.Regular), Brushes.Black, new Point(210, 208), format);

                e.Graphics.DrawString("کل رقم ------------", new Font("Jameel Noori Nastaleeq", 10, FontStyle.Regular), Brushes.Black, new Point(120, 210), format);
                e.Graphics.DrawString(TotalAmountTB.Text, new Font("Microsft Sans Sarif", 10, FontStyle.Regular), Brushes.Black, new Point(90, 208), format);

                e.Graphics.DrawString("پیشگی ------------", new Font("Jameel Noori Nastaleeq", 10, FontStyle.Regular), Brushes.Black, new Point(260, 240), format);
                e.Graphics.DrawString(AdvanceTB.Text, new Font("Microsft Sans Sarif", 10, FontStyle.Regular), Brushes.Black, new Point(240, 238), format);

                e.Graphics.DrawString("بقایہ رقم ------------", new Font("Jameel Noori Nastaleeq", 10, FontStyle.Regular), Brushes.Black, new Point(120, 240), format);
                e.Graphics.DrawString(BalanceTB.Text, new Font("Microsft Sans Sarif", 10, FontStyle.Regular), Brushes.Black, new Point(90, 238), format);

                e.Graphics.DrawString("نوٹ:", new Font("Jameel Noori Nastaleeq", 10, FontStyle.Regular), Brushes.Black, new Point(280, 260), format);
                e.Graphics.DrawString(NoteTB.Text, new Font("Microsft Sans Sarif", 10, FontStyle.Regular), Brushes.Black, rect5, format);

                e.Graphics.DrawString("Software Developed By www.Easysoft.pk", new Font("Microsft Sans Sarif", 10, FontStyle.Regular), Brushes.Black, new Point(15, 330));
                e.Graphics.DrawString("Call : 0301-1499025 | 0318-0137714", new Font("Microsft Sans Sarif", 10, FontStyle.Regular), Brushes.Black, new Point(28, 345));
            }
        }

        private void panel1_Paint_2(object sender, PaintEventArgs e)
        {

        }

        private void LanguageCombo_SelectedValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Language = LanguageCombo.Text;
            Properties.Settings.Default.PX = this.Location.X;
            Properties.Settings.Default.PY = this.Location.Y;
            Properties.Settings.Default.Save();

            this.Hide();
            MainForm mf = new MainForm();
            mf.ShowDialog();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.F2)
            {
                TailorPrintPreview.Document = Tailor8MMPrint;
                TailorPrintPreview.ShowDialog();
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
            AddFieldBtn.Text = lan[15];
            SearchFieldTB.Clear();
            FieldNameTB.Focus();
        }
    }
}
