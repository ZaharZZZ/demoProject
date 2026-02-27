using DemoLib;
using DemoLib.Models;
using DemoLib.Models.Clients;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleDemoWin
{
    public partial class AddNewClientForm : Form
    {
        MySQLClientsModel mySQLClientsModel = new MySQLClientsModel();
        private string imagePath = "";
        private bool isEditMode = false;          // флаг режима редактирования
        private Client editingClient;              // клиент для редактирования

        public Client addingClient { get; set; }

        // Конструктор для добавления нового клиента
        public AddNewClientForm(MySQLClientsModel model)
        {
            InitializeComponent();
            mySQLClientsModel = model;
            isEditMode = false;
            this.Text = "Добавление нового клиента";
        }

        // Конструктор для редактирования существующего клиента
        public AddNewClientForm(MySQLClientsModel model, Client clientToEdit)
        {
            InitializeComponent();
            mySQLClientsModel = model;
            isEditMode = true;
            editingClient = clientToEdit;
            this.Text = "Редактирование клиента";

            // Заполняем поля данными выбранного клиента
            Name_textBox.Text = clientToEdit.Name;
            Description_textBox.Text = clientToEdit.Description;
            Phone_textBox.Text = clientToEdit.Phone;
            Mail_textBox.Text = clientToEdit.Mail;
            imagePath = clientToEdit.ImagePath; // текущий путь к аватару
        }

        private void Cancel_button_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void AddClient_button_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Name_textBox.Text))
            {
                MessageBox.Show("Введите имя клиента", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (isEditMode)
            {
                // Режим редактирования — обновляем существующего клиента
                editingClient.Name = Name_textBox.Text;
                editingClient.Description = Description_textBox.Text;
                editingClient.Phone = Phone_textBox.Text;
                editingClient.Mail = Mail_textBox.Text;
                editingClient.ImagePath = imagePath;

                addingClient = editingClient; // возвращаем отредактированного клиента
            }
            else
            {
                // Режим добавления — создаём нового клиента
                addingClient = new Client(mySQLClientsModel.GetClientsCount() + 1)
                {
                    Name = Name_textBox.Text,
                    Description = Description_textBox.Text,
                    Phone = Phone_textBox.Text,
                    Mail = Mail_textBox.Text,
                    ImagePath = imagePath
                };
            }

            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void chooseAvatar_button_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                imagePath = openFileDialog.FileName;
            }
        }
    }
}
