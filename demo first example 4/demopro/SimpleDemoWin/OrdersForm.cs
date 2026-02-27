using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DemoLib;
using DemoLib.Models.Clients;

namespace SimpleDemoWin
{
    public partial class OrdersForm : Form
    {
        private readonly Client client;
        private readonly MySQLClientsModel model;

        public OrdersForm(Client client, MySQLClientsModel model)
        {
            InitializeComponent();
            this.client = client;
            this.model = model;
            lblClientName.Text = $"Заказы клиента: {client.Name}";
        }

        private void OrdersForm_Load(object sender, EventArgs e)
        {
            RefreshOrdersGrid();
        }

        private void RefreshOrdersGrid()
        {
            // Получаем актуальные заказы из объекта client (они уже загружены моделью)
            var orders = client.order.Records;

            // Создаём DataTable для удобного отображения
            var table = new System.Data.DataTable();
            table.Columns.Add("ID", typeof(int));
            table.Columns.Add("Товар", typeof(string));
            table.Columns.Add("Количество", typeof(int));
            table.Columns.Add("Цена", typeof(double));
            table.Columns.Add("Дата", typeof(DateTime));

            foreach (var record in orders)
            {
                table.Rows.Add(record.Id, record.NameProduct, record.Count, record.Price, record.SaleDate);
            }

            dgvOrders.DataSource = table;

            // Скрываем колонку ID, если не нужна
            if (dgvOrders.Columns["ID"] != null)
                dgvOrders.Columns["ID"].Visible = false;

            // Настройка ширины колонок
            dgvOrders.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (var addForm = new AddOrderForm())
            {
                if (addForm.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        // Добавляем заказ в БД
                        model.AddOrder(client.ID, addForm.OrderRecord);

                        // Добавляем запись в локальный список клиента
                        client.order.AddRecord(addForm.OrderRecord);

                        // Обновляем грид
                        RefreshOrdersGrid();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при добавлении заказа: {ex.Message}", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvOrders.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите заказ для удаления", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Получаем ID заказа из скрытой колонки
            int orderId = Convert.ToInt32(dgvOrders.SelectedRows[0].Cells["ID"].Value);

            // Находим соответствующую запись в списке клиента
            OrderRecord recordToDelete = null;
            foreach (var rec in client.order.Records)
            {
                if (rec.Id == orderId)
                {
                    recordToDelete = rec;
                    break;
                }
            }

            if (recordToDelete == null) return;

            DialogResult result = MessageBox.Show("Удалить выбранный заказ?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    model.DeleteOrder(orderId);
                    client.order.RemoveRecord(recordToDelete);
                    RefreshOrdersGrid();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении заказа: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}