using System;
using System.Windows.Forms;
using DemoLib;

namespace SimpleDemoWin
{
    public partial class AddOrderForm : Form
    {
        public OrderRecord OrderRecord { get; private set; }

        public AddOrderForm()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            // Валидация
            if (string.IsNullOrWhiteSpace(txtProductName.Text))
            {
                MessageBox.Show("Введите наименование товара", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!double.TryParse(txtPrice.Text, out double price) || price <= 0)
            {
                MessageBox.Show("Введите корректную цену (положительное число)", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            OrderRecord = new OrderRecord
            {
                NameProduct = txtProductName.Text.Trim(),
                Count = (int)nudCount.Value,
                Price = price,
                SaleDate = dtpSaleDate.Value
            };

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}