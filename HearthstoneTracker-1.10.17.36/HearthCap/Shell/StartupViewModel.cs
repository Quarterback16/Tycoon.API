﻿using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using Caliburn.Micro;
using HearthCap.Shell.TrayIcon;

namespace HearthCap.Shell
{
    [Export(typeof(StartupViewModel))]
    public class StartupViewModel : Screen,
        IHandle<TrayIconLeftClick>,
        IHandle<TrayIconDoubleClick>
    {
        private readonly IEventAggregator events;

        private readonly CustomWindowManager windowManager;

        private readonly UserPreferences.UserPreferences userPreferences;

        private double oldHeight;

        private double oldWidth;

        private static readonly object initLock = new object();

        private double oldLeft;
        private double oldTop;

        private Window shellWindow;

        private TrayIconViewModel trayIcon;

        [ImportingConstructor]
        public StartupViewModel(
            IEventAggregator events,
            IShell shell,
            TrayIconViewModel trayIcon,
            CustomWindowManager windowManager,
            UserPreferences.UserPreferences userPreferences)
        {
            this.events = events;
            this.windowManager = windowManager;
            this.userPreferences = userPreferences;
            Shell = shell;
            this.trayIcon = trayIcon;
            events.Subscribe(this);
            Shell.Deactivated += ShellOnDeactivated;
        }

        private void ShellOnDeactivated(object sender, DeactivationEventArgs deactivationEventArgs)
        {
            TryClose();
        }

        public IShell Shell { get; set; }

        public TrayIconViewModel TrayIcon
        {
            get { return trayIcon; }
            set
            {
                if (Equals(value, trayIcon))
                {
                    return;
                }
                trayIcon = value;
                NotifyOfPropertyChange(() => TrayIcon);
            }
        }

        /// <summary>
        ///     Called when initializing.
        /// </summary>
        protected override void OnInitialize()
        {
            lock (initLock)
            {
                ((IActivate)TrayIcon).Activate();

                oldHeight = userPreferences.WindowHeight;
                oldWidth = userPreferences.WindowWidth;
                oldLeft = userPreferences.WindowLeft;
                oldTop = userPreferences.WindowTop;

                shellWindow = windowManager.EnsureWindow<IShell>();
                var args = Environment.GetCommandLineArgs();
                var isLogon = args.Any(x => x.Contains("-logon"));
                if (isLogon || userPreferences.StartMinimized)
                {
                    if (userPreferences.MinimizeToTray)
                    {
                        TrayIcon.IsVisible = true;
                        shellWindow.ShowInTaskbar = false;
                        shellWindow.ShowActivated = false;
                        shellWindow.Height = 0;
                        shellWindow.Width = 0;
                        shellWindow.Show();
                        shellWindow.Hide();
                        shellWindow.ShowInTaskbar = true;
                        shellWindow.ShowActivated = true;
                    }
                    else
                    {
                        shellWindow.WindowState = WindowState.Minimized;
                        shellWindow.Show();
                    }
                }
                else
                {
                    Shell.Show();
                }
                shellWindow.Height = oldHeight;
                shellWindow.Width = oldWidth;
                shellWindow.Left = oldLeft;
                shellWindow.Top = oldTop;
                userPreferences.Initialize();
            }
        }

        /// <summary>
        ///     Handles the message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Handle(TrayIconLeftClick message)
        {
            lock (initLock)
            {
                Shell.Show();
            }
        }

        /// <summary>
        ///     Handles the message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Handle(TrayIconDoubleClick message)
        {
            lock (initLock)
            {
                Shell.Show();
            }
        }
    }
}
