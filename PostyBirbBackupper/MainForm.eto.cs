using System;
using Eto.Forms;
using Eto.Drawing;

namespace PostyBirbBackupper
{
	partial class MainForm : Form
	{
		private OpenFileDialog openFileDialog = new OpenFileDialog() {
			Filters = {
				new FileFilter("PostyBirb Backup File", ".pb"),
			},
			MultiSelect = false,
			Title = "Choose a backup file to restore from"
		};

		private SaveFileDialog saveFileDialog = new SaveFileDialog() {
			Filters = {
				new FileFilter("PostyBirb Backup File", ".pb"),
			},
			FileName = "PostyBirbBackup.pb",
			Title = "Choose a location to save the backup"
		};

		private Label statusLabel = new Label() {
			Text = "Ready"
		};

		/// <summary>
		/// Initializes the controls and layout of the main form.
		/// </summary>
		void InitializeComponent()
		{
			Title = "PostyBirb Backupper";
			MinimumSize = new Size(640, 320);
			Padding = 10;

			Content = new StackLayout()
			{
				Items = {
					new StackLayout()
					{
						Spacing = 5,
						Items = {
							"PostyBirb Backupper - 1.0.0 - by ReDark Technology",
							new StackLayout() {
								Spacing = 5,
								Orientation = Orientation.Horizontal,
								Items = {
									new Button() { Text = "Backup", Command = new Command(OnBackup) },
									new Button() { Text = "Restore", Command = new Command(OnRestore) }	
								}
							},
							statusLabel
						}
					}
				}
			};
		}

        void Example()
		{
			Title = "My Eto Form";
			MinimumSize = new Size(200, 200);
			Padding = 10;

			Content = new StackLayout
			{
				Items =
				{
					"Hello World!",
					// add more controls here
				}
			};

			// create a few commands that can be used for the menu and toolbar
			var clickMe = new Command { MenuText = "Click Me!", ToolBarText = "Click Me!" };
			clickMe.Executed += (sender, e) => MessageBox.Show(this, "I was clicked!");

			var quitCommand = new Command { MenuText = "Quit", Shortcut = Application.Instance.CommonModifier | Keys.Q };
			quitCommand.Executed += (sender, e) => Application.Instance.Quit();

			var aboutCommand = new Command { MenuText = "About..." };
			aboutCommand.Executed += (sender, e) => new AboutDialog().ShowDialog(this);

			// create menu
			Menu = new MenuBar
			{
				Items =
				{
					// File submenu
					new SubMenuItem { Text = "&File", Items = { clickMe } },
					// new SubMenuItem { Text = "&Edit", Items = { /* commands/items */ } },
					// new SubMenuItem { Text = "&View", Items = { /* commands/items */ } },
				},
				ApplicationItems =
				{
					// application (OS X) or file menu (others)
					new ButtonMenuItem { Text = "&Preferences..." },
				},
				QuitItem = quitCommand,
				AboutItem = aboutCommand
			};

			// create toolbar			
			ToolBar = new ToolBar { Items = { clickMe } };
		}
	}
}
