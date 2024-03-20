using System.Windows;

namespace BookStore.Views
{
    /// <summary>
    /// Логика взаимодействия для ShowBookInfoView.xaml
    /// </summary>
    public partial class ShowBookInfoView : Window
    {
        public ShowBookInfoView(Window owner)
        {
            InitializeComponent();
            Owner = owner;
        }
    }
}
