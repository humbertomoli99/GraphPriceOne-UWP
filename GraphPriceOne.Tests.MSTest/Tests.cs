using System;

using GraphPriceOne.ViewModels;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GraphPriceOne.Tests.MSTest
{
    // TODO WTS: Add appropriate tests
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void TestMethod1()
        {
        }

        // TODO WTS: Add tests for functionality you add to AddStoreViewModel.
        [TestMethod]
        public void TestAddStoreViewModelCreation()
        {
            // This test is trivial. Add your own tests for the logic you add to the ViewModel.
            //var vm = new AddStoreViewModel();
            //Assert.IsNotNull(vm);
        }

        // TODO WTS: Add tests for functionality you add to ExportViewModel.
        [TestMethod]
        public void TestExportViewModelCreation()
        {
            // This test is trivial. Add your own tests for the logic you add to the ViewModel.
            var vm = new ExportViewModel();
            Assert.IsNotNull(vm);
        }

        // TODO WTS: Add tests for functionality you add to MainViewModel.
        [TestMethod]
        public void TestMainViewModelCreation()
        {
            // This test is trivial. Add your own tests for the logic you add to the ViewModel.
            var vm = new MainViewModel();
            Assert.IsNotNull(vm);
        }

        // TODO WTS: Add tests for functionality you add to ProductDetailsViewModel.
        [TestMethod]
        public void TestProductDetailsViewModelCreation()
        {
            // This test is trivial. Add your own tests for the logic you add to the ViewModel.
            var vm = new ProductDetailsViewModel();
            Assert.IsNotNull(vm);
        }

        // TODO WTS: Add tests for functionality you add to ProductsViewModel.
        [TestMethod]
        public void TestProductsViewModelCreation()
        {
            // This test is trivial. Add your own tests for the logic you add to the ViewModel.
            var vm = new ProductsViewModel();
            Assert.IsNotNull(vm);
        }

        // TODO WTS: Add tests for functionality you add to SettingsViewModel.
        [TestMethod]
        public void TestSettingsViewModelCreation()
        {
            // This test is trivial. Add your own tests for the logic you add to the ViewModel.
            var vm = new SettingsViewModel();
            Assert.IsNotNull(vm);
        }

        // TODO WTS: Add tests for functionality you add to StoresViewModel.
        [TestMethod]
        public void TestStoresViewModelCreation()
        {
            // This test is trivial. Add your own tests for the logic you add to the ViewModel.
            var vm = new StoresViewModel();
            Assert.IsNotNull(vm);
        }
    }
}
