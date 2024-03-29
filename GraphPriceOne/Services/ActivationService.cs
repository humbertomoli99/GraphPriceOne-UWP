﻿using GraphPriceOne.Activation;
using GraphPriceOne.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace GraphPriceOne.Services
{
    // For more information on understanding and extending activation flow see
    // https://github.com/Microsoft/WindowsTemplateStudio/blob/release/docs/UWP/activation.md
    internal class ActivationService
    {
        private readonly App _app;
        private readonly Type _defaultNavItem;
        private Lazy<UIElement> _shell;

        private object _lastActivationArgs;

        public ActivationService(App app, Type defaultNavItem, Lazy<UIElement> shell = null)
        {
            _app = app;
            _shell = shell;
            _defaultNavItem = defaultNavItem;
        }

        public async Task ActivateAsync(object activationArgs)
        {
            if (IsInteractive(activationArgs))
            {
                // Initialize services that you need before app activation
                // take into account that the splash screen is shown while this code runs.
                await InitializeAsync();

                // Do not repeat app initialization when the Window already has content,
                // just ensure that the window is active
                if (Window.Current.Content == null)
                {
                    // Create a Shell or Frame to act as the navigation context
                    Window.Current.Content = _shell?.Value ?? new Frame();

                    // Add support for system back requests. 
                    SystemNavigationManager.GetForCurrentView().BackRequested += System_BackRequested;
                    // Add support for accelerator keys. 
                    // Listen to the window directly so the app responds
                    // to accelerator keys regardless of which element has focus.
                    Window.Current.CoreWindow.Dispatcher.AcceleratorKeyActivated += CoreDispatcher_AcceleratorKeyActivated;
                    // Add support for mouse navigation buttons. 
                    Window.Current.CoreWindow.PointerPressed += CoreWindow_PointerPressed;
                }
            }

            // Depending on activationArgs one of ActivationHandlers or DefaultActivationHandler
            // will navigate to the first page
            await HandleActivationAsync(activationArgs);
            _lastActivationArgs = activationArgs;

            if (IsInteractive(activationArgs))
            {
                // Ensure the current window is active
                Window.Current.Activate();

                // Tasks after activation
                await StartupAsync();
            }
        }
        // Handle system back requests.
        private void System_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (!e.Handled)
            {
                e.Handled = NavigationService.GoBack();
            }
        }

        private async Task InitializeAsync()
        {
            await Singleton<BackgroundTaskService>.Instance.RegisterBackgroundTasksAsync().ConfigureAwait(false);
            await ThemeSelectorService.InitializeAsync().ConfigureAwait(false);
        }

        private async Task HandleActivationAsync(object activationArgs)
        {
            var activationHandler = GetActivationHandlers()
                                                .FirstOrDefault(h => h.CanHandle(activationArgs));

            if (activationHandler != null)
            {
                await activationHandler.HandleAsync(activationArgs);
            }

            if (IsInteractive(activationArgs))
            {
                var defaultHandler = new DefaultActivationHandler(_defaultNavItem);
                if (defaultHandler.CanHandle(activationArgs))
                {
                    await defaultHandler.HandleAsync(activationArgs);
                }
            }
        }

        private async Task StartupAsync()
        {
            await ThemeSelectorService.SetRequestedThemeAsync();
            await Singleton<StoreNotificationsService>.Instance.InitializeAsync().ConfigureAwait(false);
            await FirstRunDisplayService.ShowIfAppropriateAsync();
            await WhatsNewDisplayService.ShowIfAppropriateAsync();
        }

        private IEnumerable<ActivationHandler> GetActivationHandlers()
        {
            yield return Singleton<BackgroundTaskService>.Instance;
            yield return Singleton<StoreNotificationsService>.Instance;
            yield return Singleton<ToastNotificationsService>.Instance;
        }

        private bool IsInteractive(object args)
        {
            return args is IActivatedEventArgs;
        }

        // Invoked on every keystroke, including system keys such as Alt key combinations.
        // Used to detect keyboard navigation between pages even when the page itself
        // doesn't have focus.
        private void CoreDispatcher_AcceleratorKeyActivated(CoreDispatcher sender, AcceleratorKeyEventArgs e)
        {
            // When Alt+Left are pressed navigate back.
            // When Alt+Right are pressed navigate forward.
            if (e.EventType == CoreAcceleratorKeyEventType.SystemKeyDown
                && (e.VirtualKey == VirtualKey.Left || e.VirtualKey == VirtualKey.Right)
                && e.KeyStatus.IsMenuKeyDown == true
                && !e.Handled)
            {
                if (e.VirtualKey == VirtualKey.Left)
                {
                    e.Handled = NavigationService.GoBack();
                }
                else if (e.VirtualKey == VirtualKey.Right)
                {
                    if (NavigationService.CanGoForward)
                    {
                        NavigationService.GoForward();
                    }
                }
            }
        }
        // Handle mouse back button.
        private void CoreWindow_PointerPressed(CoreWindow sender, PointerEventArgs e)
        {
            // For this event, e.Handled arrives as 'true'.
            if (e.CurrentPoint.Properties.IsXButton1Pressed)
            {
                e.Handled = !NavigationService.GoBack();
            }
            else if (e.CurrentPoint.Properties.IsXButton2Pressed)
            {
                if (NavigationService.CanGoForward)
                {
                    NavigationService.GoForward();
                }
            }
        }
    }
}
