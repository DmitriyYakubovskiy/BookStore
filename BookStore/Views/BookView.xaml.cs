using System.Windows;

namespace BookStore.Views
{
    /// <summary>
    /// Логика взаимодействия для BookView.xaml
    /// </summary>
    public partial class BookView : Window
    {
        public BookView(Window owner)
        {
            InitializeComponent();
            Owner = owner;
        }
    }
}
