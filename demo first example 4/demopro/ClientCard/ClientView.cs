using System.Windows.Forms;
using DemoLib.Views;
using DemoLib;
using System.Drawing;
using System;

namespace ClientCard
{
    public partial class ClientView : UserControl, IClientView
    {
        private Color defaultColor = Color.FromArgb(255, 192, 128);
        private Color enteringColor = Color.FromArgb(255, 140, 0);
        private Client client_;

        // Событие для одинарного клика (используется в текущем коде)
        public event Action<Client> SelectedClient;

        // Событие для двойного клика (для открытия заказов)
        public event Action<Client> ClientDoubleClick;

        public ClientView()
        {
            InitializeComponent();

            foreach (Control control in this.Controls)
            {
                control.MouseEnter += ClientView_MouseEnter;
                control.MouseLeave += ClientView_MouseLeave;
                control.MouseClick += ClientView_MouseClick;
                control.DoubleClick += ClientView_DoubleClick; // подписка на двойной клик
            }
            // Подписка самого контрола
            this.DoubleClick += ClientView_DoubleClick;
        }

        public void ShowClientInfo(Client client)
        {
            client_ = client;

            TitleLabel.Text = client.Name;
            DescriptionLabel.Text = client.Description;
            PhoneLabel.Text = client.Phone;
            MailLabel.Text = client.Mail;
            AvatarBox.Load(client.ImagePath);
        }

        public Client GetClientInfo()
        {
            return client_;
        }

        public void ShowView()
        {
            Visible = true;
        }

        public void HideView()
        {
            Visible = false;
        }

        private void ClientView_MouseEnter(object sender, EventArgs e)
        {
            this.BackColor = enteringColor;
        }

        private void ClientView_MouseLeave(object sender, EventArgs e)
        {
            this.BackColor = defaultColor;
        }

        private void ClientView_MouseClick(object sender, MouseEventArgs e)
        {
            SelectedClient?.Invoke(client_);
        }

        private void ClientView_DoubleClick(object sender, EventArgs e)
        {
            ClientDoubleClick?.Invoke(client_);
        }

        public void ClearClientInfo()
        {
            TitleLabel.Text = "Клиенты отсутствуют";
            DescriptionLabel.Text = "";
            PhoneLabel.Text = "";
            MailLabel.Text = "";
            AvatarBox.Image?.Dispose();
            AvatarBox.Image = null;
        }

        public void SetAvatarProperties(Size size, Point location)
        {
            AvatarBox.Size = size;
            AvatarBox.Location = location;
        }
    }
}