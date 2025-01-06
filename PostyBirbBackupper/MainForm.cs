using System;
using System.IO;
using System.IO.Compression;
using Eto.Forms;
using Eto.Drawing;

namespace PostyBirbBackupper
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();

			if (!PostyBirb.IsPostyBirbDataAvailable())
			{
				if (MessageBox.Show(this, "No PostyBirb data found. Do you want to open this app anyway?", "PostyBirb Backupper", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					Close();
				}

				statusLabel.Text = "No PostyBirb data found...?";
			}
		}

        private void OnRestore(object sender, EventArgs e)
        {
			try
			{
				if(openFileDialog.ShowDialog(this) == DialogResult.Ok)
					PostyBirb.RestoreFromBackupFile(openFileDialog.FileName);

				statusLabel.Text = "Backup restored from: " + openFileDialog.FileName;
			}
			catch(Exception ex)
			{
				statusLabel.Text = "Failed to restore from backup file: " + ex.Message;
			}
        }

        private void OnBackup(object sender, EventArgs e)
        {
			try
			{
				saveFileDialog.FileName = "PostyBirbBackup-" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".pb";
				if(saveFileDialog.ShowDialog(this) == DialogResult.Ok)
				{
					PostyBirb.CreateBackupFile(saveFileDialog.FileName);
					statusLabel.Text = "Backup created at: " + saveFileDialog.FileName;
				}
			}
			catch(Exception ex)
			{
				statusLabel.Text = "Failed to create backup file: " + ex.Message;
			}
        }
	}
}
