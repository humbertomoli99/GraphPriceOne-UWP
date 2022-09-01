using System;
using GraphPriceOne.Models;
using GraphPriceOne.Services;
using GraphPriceOne.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Navigation;

namespace GraphPriceOne.Views
{
    public sealed partial class StoresPage : Page
    {
        private AddSelectorsViewModel selectors;

        // TODO WTS: Change the grid as appropriate to your app, adjust the column definitions on StoresPage.xaml.
        // For more details see the documentation at https://docs.microsoft.com/windows/communitytoolkit/controls/datagrid
        public StoresPage()
        {
            InitializeComponent();
            object[] objects = { ListStores };
            DataContext = new StoresViewModel(objects);

            selectors = new AddSelectorsViewModel();
        }
        private void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            NavigationService.Navigate(typeof(AddStorePage));
        }

        private void CheckBox_Checked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (ListStores.SelectionMode == ListViewSelectionMode.Multiple || ListStores.SelectionMode == ListViewSelectionMode.Extended)
            {
                CheckBox1.IsChecked = true;
                CheckBox1Icon.Glyph = "\ue73a";
                ListStores.SelectAll();
            }
        }

        private void CheckBox_Unchecked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (ListStores.SelectionMode == ListViewSelectionMode.Multiple || ListStores.SelectionMode == ListViewSelectionMode.Extended)
            {
                CheckBox1.IsChecked = false;
                CheckBox1Icon.Glyph = "\ue739";
                ListStores.DeselectRange(new ItemIndexRange(0, (uint)ListStores.Items.Count));
            }
        }

        private void ListViewStores_RefreshRequested(RefreshContainer sender, RefreshRequestedEventArgs args)
        {
            new StoresViewModel();
        }

        private void ListStores_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int itemsSelected = ListStores.SelectedItems.Count;
            int AllItems = ListStores.Items.Count;
            if (ListStores.SelectionMode == ListViewSelectionMode.Multiple || ListStores.SelectionMode == ListViewSelectionMode.Extended)
            {
                if (ListStores.SelectedItem != null)
                {
                    selectedItem.Text = "Selected item: " + ListStores.SelectedItem.ToString();
                }
                else
                {
                    selectedItem.Text = "Selected item: null";
                }
                selectedIndex.Text = "Selected index: " + ListStores.SelectedIndex.ToString();
                selectedItemCount.Text = "Items selected: " + ListStores.SelectedItems.Count.ToString();
                addedItems.Text = "Added: " + e.AddedItems.Count.ToString();
                removedItems.Text = "Removed: " + e.RemovedItems.Count.ToString();

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
            if (ListStores.SelectionMode == ListViewSelectionMode.Single && ListStores.SelectedItem != null)
            {
                StoresModel obj = (StoresModel)ListStores.SelectedItem;
                selectors.SelectedStore = obj;
                NavigationService.Navigate(typeof(AddSelectorsPage));
            }
        }
    }
}
