using System.Windows;

namespace BookStore.Views
{
    /// <summary>
    /// Логика взаимодействия для DiscountsView.xaml
    /// </summary>
    public partial class DiscountsView : Window
    {
        public DiscountsView(Window window)
        {
            InitializeComponent();
            Owner = window;
        }
    }
}
