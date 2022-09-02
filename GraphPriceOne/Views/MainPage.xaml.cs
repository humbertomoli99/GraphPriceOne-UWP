using GraphPriceOne.Models;
using GraphPriceOne.Services;
using GraphPriceOne.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;

namespace GraphPriceOne.Views
{
    public sealed partial class MainPage : Page
    {
        private ProductDetailsViewModel selectors;

        public MainPage()
        {
            InitializeComponent();
            DataContext = new MainViewModel(ListProducts);

            selectors = new ProductDetailsViewModel();
        }
        private void productView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int itemsSelected = ListProducts.SelectedItems.Count;
            int AllItems = ListProducts.Items.Count;
            if (ListProducts.SelectionMode == ListViewSelectionMode.Multiple || ListProducts.SelectionMode == ListViewSelectionMode.Extended)
            {
                if (itemsSelected == AllItems)
                {
                    CheckBox1.IsChecked = true;
                    CheckBox1Icon.Glyph = "\ue73a";
                }
                else if (itemsSelected == 0)
                {
                    CheckBox1.IsChecked = false;
                    CheckBox1Icon.Glyph = "\ue739";
                }
                else
                {
                    CheckBox1.IsChecked = false;
                    CheckBox1Icon.Glyph = "\uf16e";
                }
            }
            if (ListProducts.SelectionMode == ListViewSelectionMode.Single && ListProducts.SelectedItem != null)
            {
                ProductsModel obj = (ProductsModel)ListProducts.SelectedItem;
                selectors.SelectedProduct = obj.ID_PRODUCT;
                NavigationService.Navigate(typeof(ProductDetailsPage));
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (ListProducts.SelectionMode == ListViewSelectionMode.Multiple || ListProducts.SelectionMode == ListViewSelectionMode.Extended)
            {
                CheckBox1.IsChecked = true;
                CheckBox1Icon.Glyph = "\ue73a";
                ListProducts.SelectAll();
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (ListProducts.SelectionMode == ListViewSelectionMode.Multiple || ListProducts.SelectionMode == ListViewSelectionMode.Extended)
            {
                CheckBox1.IsChecked = false;
                CheckBox1Icon.Glyph = "\ue739";
                ListProducts.DeselectRange(new ItemIndexRange(0, (uint)ListProducts.Items.Count));
            }
        }
        private void ListViewStores_RefreshRequested(Microsoft.UI.Xaml.Controls.RefreshContainer sender, Microsoft.UI.Xaml.Controls.RefreshRequestedEventArgs args)
        {
            new MainViewModel().GetProductsAsync().Wait();
        }
    }
}
