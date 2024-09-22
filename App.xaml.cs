using PContactos.Model;
using System.Collections.ObjectModel;

namespace PContactos

{
    public partial class App : Application
    {
        public static ContactItem selectedContact;

        public static ObservableCollection<ContactItem> contacts;
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}
