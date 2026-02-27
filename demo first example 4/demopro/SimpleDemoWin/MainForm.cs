using System;
using System.Collections.Generic;
using System.Reflection;
using DemoLib.Views;
using System.Windows.Forms;
using DemoLib;
using DemoLib.Models.Clients;
using System.Data;
using System.Drawing;
using DemoLib.Models;

namespace SimpleDemoWin
{
    public partial class MainForm : Form
    {
        private List<Client> allClients_ = new List<Client>();
        private MySQLClientsModel model = new MySQLClientsModel();
        public MainForm()
        {
            InitializeComponent();
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            MySQLClientsModel model = new MySQLClientsModel();
            
            allClients_ = model.ReadAllClients();
            ShowClients(allClients_);
            Card.SetAvatarProperties(new Size(260,195), new Point(220,20));
        }

        private void ClientsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = ClientsListBox.SelectedItem;
            if (item == null)
            {
                return;
            }

            Client client = item as Client;
            if (client == null)
            {

                return;
            }

            Card.ShowClientInfo(client);
        }

        private void ShowClients(List<Client> clients)
        {
            ClientsListBox.DataSource = null;
            ClientsListBox.DataSource = clients;
            ClientsListBox.DisplayMember = "Name";
        }

        private void FilterAndSearch()
        {
            if (IsNotSettedFilterAndSearch())
            {
                ShowClients(allClients_);
                return;
            }

            string searchingText = SearchByNameTextBox.Text; // это условие поиска
            string alphabetText = AlphabetComboBox.Text; // это условие фильтра


            List<Client> resultClients = new List<Client>();
            foreach (Client client in allClients_)
            {
                if ((String.IsNullOrEmpty(searchingText)
                     || client.Name.ToLower().Contains(searchingText.ToLower()))
                    && (String.IsNullOrEmpty(alphabetText) 
                     || client.Name[0] == alphabetText[0]))
                {
                    resultClients.Add(client);
                }
            }

            ShowClients(resultClients);
        }

        private void SearchByNameTextBox_TextChanged(object sender, EventArgs e)
        {
            FilterAndSearch();
        }

        private bool IsNotSettedFilterAndSearch()
        {
            return string.IsNullOrEmpty(AlphabetComboBox.Text)
                        && string.IsNullOrEmpty(SearchByNameTextBox.Text);
        }

        private void AlphabetComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterAndSearch();
        }

        private void AlphabetComboBox_TextChanged(object sender, EventArgs e)
        {
            FilterAndSearch();
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            if (ClientsListBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите клиента для удаления", "Информация",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Client selectedClient = ClientsListBox.SelectedItem as Client;
            if (selectedClient == null)
                return;

            DialogResult result = MessageBox.Show(
                $"Вы уверены, что хотите удалить клиента '{selectedClient.Name}'?",
                "Подтверждение удаления",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    model.DeleteClient(selectedClient.ID);

                    // Удаляем клиента из локального списка
                    updateClients();

                        MessageBox.Show("Клиент успешно удален", "Успех",
                                      MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении клиента: {ex.Message}", "Ошибка",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            AddNewClientForm addNewClientForm = new AddNewClientForm(model);
            if (addNewClientForm.ShowDialog() == DialogResult.OK)
            {
                Client client = addNewClientForm.addingClient;
                model.AddClient(client);
                updateClients();
            }
        }

        public void updateClients()
        {
            allClients_ = model.ReadAllClients();
            ShowClients(allClients_);
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            // Проверяем, выбран ли клиент
            if (ClientsListBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите клиента для редактирования", "Информация",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Client selectedClient = ClientsListBox.SelectedItem as Client;
            if (selectedClient == null)
                return;

            // Открываем форму редактирования с выбранным клиентом
            AddNewClientForm editForm = new AddNewClientForm(model, selectedClient);
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                // Получаем обновлённого клиента из формы
                Client updatedClient = editForm.addingClient;

                // Сохраняем изменения в базу данных
                try
                {
                    model.UpdateClient(updatedClient);
                    // Обновляем список клиентов на форме
                    updateClients();
                    // Если нужно, сразу показываем обновлённую информацию в карточке
                    Card.ShowClientInfo(updatedClient);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении изменений: {ex.Message}", "Ошибка",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
