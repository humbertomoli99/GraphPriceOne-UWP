using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace GraphPriceOne.Models
{
    public class ProductsModel : PropertyChangedNotification
    {
        public Visibility IsCheckedAllVisibility
        {
            get { return GetValue(() => IsCheckedAllVisibility); }
            set { SetValue(() => IsCheckedAllVisibility, value); }
        }
        public Visibility DeleteStoreVisibility
        {
            get { return GetValue(() => DeleteStoreVisibility); }
            set { SetValue(() => DeleteStoreVisibility, value); }
        }
        public Visibility FirstProductVisibility
        {
            get { return GetValue(() => FirstProductVisibility); }
            set { SetValue(() => FirstProductVisibility, value); }
        }
        public Visibility CommandBarVisibility
        {
            get { return GetValue(() => CommandBarVisibility); }
            set { SetValue(() => CommandBarVisibility, value); }
        }
        public Visibility ListProductsVisibility { get; set; }

        public ICommand SelectMultiple { get; set; }
    }
}
