﻿using System.ComponentModel.Composition;
using Caliburn.Micro;

namespace HearthCap.Shell.Dialogs
{
    [Export(typeof(IMessageBox))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class MessageBoxViewModel : Screen, IMessageBox
    {
        private MessageBoxOptions selection;

        public bool OkVisible
        {
            get { return IsVisible(MessageBoxOptions.Ok); }
        }

        public bool CancelVisible
        {
            get { return IsVisible(MessageBoxOptions.Cancel); }
        }

        public bool YesVisible
        {
            get { return IsVisible(MessageBoxOptions.Yes); }
        }

        public bool NoVisible
        {
            get { return IsVisible(MessageBoxOptions.No); }
        }

        public string Message { get; set; }

        public MessageBoxOptions Options { get; set; }

        public void Ok()
        {
            Select(MessageBoxOptions.Ok);
        }

        public void Cancel()
        {
            Select(MessageBoxOptions.Cancel);
        }

        public void Yes()
        {
            Select(MessageBoxOptions.Yes);
        }

        public void No()
        {
            Select(MessageBoxOptions.No);
        }

        public bool WasSelected(MessageBoxOptions option)
        {
            return (selection & option) == option;
        }

        private bool IsVisible(MessageBoxOptions option)
        {
            return (Options & option) == option;
        }

        private void Select(MessageBoxOptions option)
        {
            selection = option;
            TryClose();
        }
    }
}
