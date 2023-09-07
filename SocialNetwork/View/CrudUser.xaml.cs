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
using System.Windows.Shapes;

namespace SocialNetwork.View
{
    /// <summary>
    /// Логика взаимодействия для CrudUser.xaml
    /// </summary>
    public partial class CrudUser : Window
    {
        public Data.Entity.User? User { get; set; }
        public bool IsDeleted { get; set; }
        public CrudUser()
        {
            InitializeComponent();
        }

        private void SoftDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            IsDeleted = true;
            DialogResult = true;
        }

        private void HardDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            User = null;
            DialogResult = true;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            User.Name = NameBox.Text;
            User.Surname = SurnameBox.Text;
            User.Avatar = AvatarBox.Text;
            User.Birthday = DateTime.Parse(BirthdayBox.Text);
            DialogResult = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IdBox.Text = User.Id.ToString() ?? "";
            NameBox.Text = User.Name?.ToString() ?? "";
            SurnameBox.Text = User.Surname?.ToString() ?? "";
            AvatarBox.Text = User.Avatar?.ToString() ?? "";
            BirthdayBox.Text = User.Birthday.ToString() ?? "";
        }
    }
}
