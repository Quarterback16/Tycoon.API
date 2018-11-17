﻿using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Caliburn.Micro;
using HearthCap.Data;
using HearthCap.Features.Analytics;
using HearthCap.Shell.Flyouts;
using Microsoft.Win32;
using Omu.ValueInjecter;

namespace HearthCap.Features.TextFiles
{
    [Export(typeof(IFlyout))]
    public class TextFilesViewModel : FlyoutViewModel
    {
        private readonly IEventAggregator events;

        private readonly IRepository<TextFileTemplate> repository;

        private readonly TextFilesManager textFilesManager;

        private readonly BindableCollection<TextFileModel> templates = new BindableCollection<TextFileModel>();

        private TextFileModel selectedTemplate;

        private readonly BindableCollection<KeyValuePair<string, string>> variables = new BindableCollection<KeyValuePair<string, string>>();

        private KeyValuePair<string, string>? selectedVariable;

        [ImportingConstructor]
        public TextFilesViewModel(IEventAggregator events,
            IRepository<TextFileTemplate> repository,
            TextFilesManager textFilesManager)
        {
            Name = Flyouts.TextFiles;
            Header = DisplayName = "Auto generated text files:";

            this.events = events;
            this.repository = repository;
            this.textFilesManager = textFilesManager;

            events.Subscribe(this);
            templates = textFilesManager.Templates;
            InitializeVariables(textFilesManager.Listeners);
            PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName == "IsOpen" && IsOpen)
                    {
                        Tracker.TrackEventAsync(Tracker.FlyoutsCategory, "Open", Name, 1);
                    }
                };
        }

        private void InitializeVariables(IEnumerable<TextFilesEventsListener> filesEventsListeners)
        {
            foreach (var textFilesEventsListener in filesEventsListeners)
            {
                variables.AddRange(textFilesEventsListener.Variables);
            }
        }

        public BindableCollection<TextFileModel> Templates
        {
            get { return templates; }
        }

        public TextFileModel SelectedTemplate
        {
            get { return selectedTemplate; }
            set
            {
                if (Equals(value, selectedTemplate))
                {
                    return;
                }
                selectedTemplate = value;
                NotifyOfPropertyChange(() => SelectedTemplate);
            }
        }

        public IObservableCollection<KeyValuePair<string, string>> Variables
        {
            get { return variables; }
        }

        public KeyValuePair<string, string>? SelectedVariable
        {
            get { return selectedVariable; }
            set
            {
                if (value.Equals(selectedVariable))
                {
                    return;
                }
                selectedVariable = value;
                NotifyOfPropertyChange(() => SelectedVariable);
            }
        }

        public void ChooseFile()
        {
            string file;
            if (OpenSaveAsDialog(out file) == true)
            {
                SelectedTemplate.Filename = file;
            }
        }

        public void AddNew()
        {
            string file;
            if (OpenSaveAsDialog(out file) == true)
            {
                var model = new TextFileModel
                    {
                        Filename = file,
                        Template = "%carena_wins% - %carena_losses%"
                    };
                Templates.Add(model);
                SelectedTemplate = model;
                Save();
            }
        }

        public async Task Delete()
        {
            if (SelectedTemplate == null)
            {
                return;
            }
            await repository.Delete(SelectedTemplate.Id);
            Templates.Remove(SelectedTemplate);
            SelectedTemplate = null;
        }

        public void InsertVariable()
        {
            if (SelectedTemplate == null
                || SelectedVariable == null)
            {
                return;
            }

            SelectedTemplate.Template += SelectedVariable.Value.Key;
        }

        public async void Save()
        {
            var entity = new TextFileTemplate();
            entity.InjectFrom(SelectedTemplate);
            await repository.SaveOrUpdateAsync(entity, entity.Id);
            textFilesManager.Refresh();
        }

        private bool? OpenSaveAsDialog(out string file)
        {
            var dlg = new SaveFileDialog
                {
                    FileName = "filename",
                    DefaultExt = ".txt",
                    Filter = "Text documents |*.txt"
                };

            var result = dlg.ShowDialog();
            file = dlg.FileName;
            return result;
        }
    }
}
