using Backup.Logic;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;

namespace Backup
{
    public partial class ConfigureForm : Form
    {
        FileListForm fileListForm;
        DestinationForm destForm;

        public ConfigureForm()
        {
            InitializeComponent();
            PopulateControls();
            this.FormClosing += ConfigureForm_FormClosing;
        }

        private void ConfigureForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettings();
        }

        public void PopulateControls()
        {
            Sources.BeginUpdate();
            Sources.Items.Clear();
            var sources = FileManager.GetSources();
            foreach (var source in sources)
            {
                Sources.Items.Add(
                    new ListViewItem(new string[] {
                        source.Directory,
                        source.Pattern,
                        source.ModifiedOnly,
                        source.LastBackupText }));
            }
            Sources.EndUpdate();

            var settings = FileManager.GetSettings();
            //this.CreateOnStart.Checked = settings.CreateBackupOnStart;
            this.IsBackupOnLogoff.Checked = settings.IsBackupOnLogoff;
            //this.LaunchOnLogon.Checked = settings.IsLaunchOnLogon;
        }

        private void Edit_Click(object sender, EventArgs e)
        {
            var item = this.Sources.SelectedItems[0];
            var editSource = new EditSource(item);
            var result = editSource.ShowDialog();
            if (result == DialogResult.OK)
            {
                var vals = editSource.GetValues();
                var idx = Sources.Items.IndexOf(item);
                //Sources.Items[idx].Text = vals.Item1;
                Sources.Items[idx].SubItems[0].Text = vals.Item1;
                Sources.Items[idx].SubItems[1].Text = vals.Item2;
                Sources.Items[idx].SubItems[2].Text = vals.Item3;
                // Not setting date here

                SaveSources();
                PopulateControls();
            }
        }

        private void Remove_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.Sources.SelectedItems.Count > 0)
                {
                    var confirmResult = MessageBox.Show("Are you sure to delete this item?", "Confirm Delete", MessageBoxButtons.YesNo);
                    if (confirmResult == DialogResult.Yes)
                    {
                        ListView.SelectedIndexCollection selectedItems = Sources.SelectedIndices;
                        (from int s in selectedItems orderby s descending select s)
                            .ToList()
                            .ForEach(i => Sources.Items.RemoveAt(i));

                        SaveSources();
                        PopulateControls();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "An Error Occurred");
            }
        }

        private void Sources_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Sources.SelectedItems != null && Sources.SelectedItems.Count > 0)
            {
                this.Edit.Enabled = true;
                this.Delete.Enabled = true;
            }
            else
            {
                this.Edit.Enabled = false;
                this.Delete.Enabled = false;
            }
        }

        private void SaveSources()
        {
            var toSave = new List<Source>();
            foreach (ListViewItem item in this.Sources.Items)
                toSave.Add(new Source(
                        item.SubItems[0].Text, item.SubItems[1].Text, 
                        item.SubItems[2].Text, item.SubItems[3].Text));

            FileManager.SaveSources(toSave);
        }

        private void Discover_Click(object sender, EventArgs e)
        {
            if (this.fileListForm == null)
            {
                this.fileListForm = new FileListForm();
                this.fileListForm.FormClosed += FileList_FormClosed;
                this.fileListForm.ShowDialog();
            }
        }

        private void FileList_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.fileListForm = null;
            this.PopulateControls();
        }

        private void Close_Click(object sender, EventArgs e)
        {
            SaveSettings();

            this.Close();
        }

        private void SaveSettings()
        {
            var settings = FileManager.GetSettings();
            settings.IsBackupOnLogoff = this.IsBackupOnLogoff.Checked;
            //settings.CreateBackupOnStart = this.CreateOnStart.Checked;
            //settings.IsLaunchOnLogon = this.LaunchOnLogon.Checked;
            FileManager.SaveSettings(settings);
        }

        private void BackupDestination_Click(object sender, EventArgs e)
        {
            if (this.destForm == null)
            {
                this.destForm = new DestinationForm();
                this.destForm.FormClosed += DestForm_FormClosed;
                this.destForm.ShowDialog();
            }
        }

        private void DestForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.destForm = null;
        }

        private void AddDirectory_Click(object sender, EventArgs e)
        {
            var editSource = new EditSource();
            var result = editSource.ShowDialog();
            if (result == DialogResult.OK)
            {
                var vals = editSource.GetValues();
                Sources.Items.Add(
                    new ListViewItem(new string[] { vals.Item1, vals.Item2, vals.Item3, Source.NeverText }));
            }

            SaveSources();
        }

        private void LaunchOnLogon_Click(object sender, EventArgs e)
        {
            // NB Windows 10 allows addition to Startup items through the package manifest
            // Universal Windows platform has the following to enable programmatic modification
            //var startupTask = Windows.ApplicationModel.StartupTask.GetAsync("MyStartupId");

            //#region This is adding an entry to the TaskScheduler for pre-Windows 10 
            //try
            //{
            //    var path = "\\tiz.digital\\Backup";
            //    if (this.LaunchOnLogon.Checked)
            //    {
            //        var ts = new TaskScheduler.TaskScheduler();
            //        ts.Connect();
            //        var task = ts.NewTask(0);
            //        task.Settings.MultipleInstances = TaskScheduler._TASK_INSTANCES_POLICY.TASK_INSTANCES_IGNORE_NEW;
            //        task.RegistrationInfo.Description = "A scheduled task to launch the tiz.digital Backup utility";
            //        task.RegistrationInfo.Author = "tiz.digital";
            //        task.Principal.LogonType = TaskScheduler._TASK_LOGON_TYPE.TASK_LOGON_INTERACTIVE_TOKEN;
            //        task.Settings.Hidden = false;

            //        var trigger = task.Triggers.Create(TaskScheduler._TASK_TRIGGER_TYPE2.TASK_TRIGGER_LOGON);
            //        trigger.Enabled = true;

            //        var action = task.Actions.Create(TaskScheduler._TASK_ACTION_TYPE.TASK_ACTION_EXEC) as TaskScheduler.IExecAction;
            //        var asm = System.Reflection.Assembly.GetExecutingAssembly();
            //        action.Path = asm.Location;
            //        action.Arguments = "";

            //        var root = ts.GetFolder("\\");
            //        //root.CreateFolder(path);
            //        //var tt = root.GetTask(path);
            //        var registeredTask = root.RegisterTaskDefinition(
            //            path,
            //            task,
            //            (int)TaskScheduler._TASK_CREATION.TASK_CREATE_OR_UPDATE,
            //            Environment.UserName, null,
            //            TaskScheduler._TASK_LOGON_TYPE.TASK_LOGON_INTERACTIVE_TOKEN);

            //        SaveSettings();
            //    }
            //    else
            //    {
            //        var ts = new TaskScheduler.TaskScheduler();
            //        ts.Connect();
            //        var root = ts.GetFolder("\\");
            //        var task = root.GetTask(path);
            //        task.Enabled = false;

            //        SaveSettings();
            //    }
            //}
            //catch (UnauthorizedAccessException ex)
            //{
            //    this.LaunchOnLogon.Checked = false;
            //    SaveSettings();
            //    MessageBox.Show($"This function requires running this program with adminstrator privileges. \nPlease Exit and find the location of {Program.ProgramName}.exe and enable \"Run this as program as an administrator\", (open the Properties dialog and select Compatibility tab).",
            //        "Backup - Task Scheduler", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //}
            //#endregion
        }
    }
}
