using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SocialNetwork
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Data.DataContext dataContext;
        public ObservableCollection<Pair> Pairs { get; set; }
        public ObservableCollection<Data.Entity.User> UsersView { get; set; }
        Func<Data.Entity.User, bool> predicate;
        public MainWindow()
        {
            InitializeComponent();
            dataContext = new Data.DataContext();
            Pairs = new ObservableCollection<Pair>();
            UsersView = new ObservableCollection<Data.Entity.User>();
            this.DataContext = this;
            predicate = manager => manager.DeleteDt == null;
        }
        public class Pair
        {
            public String Key { get; set; } = null!;
            public String? Value { get; set; }
        }
        private void GenderButton_Click(object sender, RoutedEventArgs e)
        {
            var query = dataContext
                .Users
                .Select(m => new Pair() { Key = $"{m.Surname} {m.Name[0]}.", Value = m.Gender.Name });
            Pairs.Clear();
            foreach (var pair in query)
            {
                Pairs.Add(pair);
            }
        }

        private void AgeButton_Click(object sender, RoutedEventArgs e)
        {
            var today = DateTime.Today;
            var Query = dataContext.Users.Join(dataContext.Users, m1 => m1.Id, m2 => m2.Id,
                (m1, m2) => new Pair()
                {
                    Key = $"{m1.Surname} {m1.Name[0]}.",
                    Value = (today.Year - m2.Birthday.Year).ToString() // calculate age based on current date
                })
                .ToList();
            Pairs.Clear();
            foreach (var pair in Query)
            {
                Pairs.Add(pair);
            }
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem item)
            {
                if (item.Content is Data.Entity.User user)
                {
                    View.CrudUser dialog = new() { User = user };
                    if (dialog.ShowDialog() ?? false)
                    {
                        if (dialog.User != null)
                        {
                            if (dialog.IsDeleted)
                            {
                                var dep = dataContext.Users.Find(user.Id);
                                user.DeleteDt = DateTime.Now;
                                dataContext.SaveChanges();
                                Update_Objects(predicate);
                            }
                            else
                            {
                                var dep = dataContext.Users.Find(user.Id);
                                if (dep != null)
                                {
                                    dep.Name = user.Name;
                                }
                                dataContext.SaveChanges();
                                int index = UsersView.IndexOf(user);
                                UsersView.Remove(user);
                                UsersView.Insert(index, user);
                            }
                        }
                        else // dialog.Department == null deleted
                        {
                            dataContext.Users.Remove(user);
                            dataContext.SaveChanges();
                            Update_Objects(predicate);
                        }
                    }
                }
            }
        }

        private void ShowDeleted_Checked(object sender, RoutedEventArgs e)
        {
            var query = dataContext.Users.Where(manager => manager.DeleteDt != null);
            foreach (var pair in query)
            {
                UsersView.Add(pair);
            }
        }

        private void ShowDeleted_Unchecked(object sender, RoutedEventArgs e)
        {
            Update_Objects(predicate);
        }

        private void AddGroupButton_Click(object sender, RoutedEventArgs e)
        {
            Data.Entity.User newUser = new() { Id = new Guid(), Name = " ", Surname = "",
                Birthday = DateTime.Now, CreateDt = DateTime.Now, Status = null, 
                Avatar = null, Gender = null, DeleteDt = null, IdGender = new Guid("d3c376e4-bce3-4d85-aba4-e3cf49612c94") };
            View.CrudUser dialog = new() { User = newUser };
            if (dialog.ShowDialog() ?? false)
            {
                dataContext.Users.Add(newUser);
                dataContext.SaveChanges();
                Update_Objects(predicate);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Update_Objects(predicate);
            usersList.ItemsSource = UsersView;
        }

        public void Update_Objects(Func<Data.Entity.User, bool> predicate)
        {
            UsersView.Clear();
            var query = dataContext.Users.Where(predicate);
            foreach (var pair in query)
            {
                UsersView.Add(pair);
            }
        }
    }
}
