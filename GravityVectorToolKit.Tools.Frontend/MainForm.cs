using GravityVectorToolkit.Tools.DatabaseImport;
using NHibernate.Mapping.ByCode;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Threading;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.IO;

namespace GravityVectorToolKit.Tools.Frontend
{

	

	public partial class MainForm : Form
	{
		private const string SettingsFilePath = "settings.json";

		/// <summary>
		/// File dialog for the normal route file
		/// </summary>
		protected OpenFileDialog dlgNormalRouteFile = new OpenFileDialog();

		/// <summary>
		/// File dialog for the gravity vector file
		/// </summary>
		protected OpenFileDialog dlgGravityVectorFile = new OpenFileDialog();

		/// <summary>
		/// File dialog for the deviation map file
		/// </summary>
		protected OpenFileDialog dlgDeviationMapFile = new OpenFileDialog();

		/// <summary>
		/// File dialog for the near-miss map file
		/// </summary>
		protected OpenFileDialog dlgNearMissMapFile = new OpenFileDialog();


		public MainForm()
		{
			InitializeComponent();

			// Configure file dialogs
			dlgNormalRouteFile.Title = "Select normal route file";
			dlgGravityVectorFile.Title = "Select gravity vector file";
			dlgDeviationMapFile.Title = "Select deviation map file";
			dlgNearMissMapFile.Title = "Select near-miss map file";

			ConfigureCommonProperties(dlgNormalRouteFile);
			ConfigureCommonProperties(dlgGravityVectorFile);
			ConfigureCommonProperties(dlgDeviationMapFile);
			ConfigureCommonProperties(dlgNearMissMapFile);
		}

		/// <summary>
		/// Configure properties on the file dialogs that they all have in common
		/// </summary>
		/// <param name="dialog"></param>
		private void ConfigureCommonProperties(OpenFileDialog dialog)
		{
			dialog.Filter = "CSV files|*.csv";
			dialog.InitialDirectory = Environment.CurrentDirectory;
			dialog.RestoreDirectory = true;
			dialog.CheckFileExists = true;
			dialog.AutoUpgradeEnabled = true;
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			if (File.Exists(SettingsFilePath))
			{
				var settingsStr = File.ReadAllText(SettingsFilePath);
				var settings = JsonSerializer.Deserialize<DatabaseImportParameters>(settingsStr);
				SetParameters(settings);
			}
		}

		private void btnFindNormalRouteFile_Click(object sender, EventArgs e)
		{
			dlgNormalRouteFile.ShowDialog();
			txtNormalRoutePath.Text = dlgNormalRouteFile.FileName;
		}

		private void btnFindGravityVectorFile_Click(object sender, EventArgs e)
		{
			dlgGravityVectorFile.ShowDialog();
			txtGravityVectorPath.Text = dlgGravityVectorFile.FileName;
		}

		private void btnFindDeviationMapFile_Click(object sender, EventArgs e)
		{
			dlgDeviationMapFile.ShowDialog();
			txtDeviationMapPath.Text = dlgDeviationMapFile.FileName;
		}

		private void btnFindNearMissMapFile_Click(object sender, EventArgs e)
		{
			dlgNearMissMapFile.ShowDialog();
			txtNearMiss.Text = dlgNearMissMapFile.FileName;
		}

		private void btnImport_Click(object sender, EventArgs e)
		{
			if (ValidateInput())
			{
				ExecuteImport();
			}
		}

		private void ExecuteImport()
		{
			if (!chkDropAndRecreate.Checked)
			{
				var result = MessageBox.Show("This action will append data to existing tables and data duplication is likely to occur. Are you sure you wish to continue?", "Confirm action",
					MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
				if (result == DialogResult.Cancel)
				{
					return;
				}
			}
			btnImport.Enabled = btnValidate.Enabled = false;
			pictureBoxSpinner.Visible = true;
			var task = Task.Run(() =>
			{
				try
				{
					DatabaseImport.Import(GetParameters(), (s) => lblStatus.SetPropertyThreadSafe(() => lblStatus.Text, s));
				}
				catch(Exception e)
				{
					var result = MessageBox.Show("The database import failed because: " + e.Message, "Database import failed",
						MessageBoxButtons.OK, MessageBoxIcon.Error);

				}
			}).ContinueWith((o) =>
			{
				btnImport.Enabled = btnValidate.Enabled = true;
				pictureBoxSpinner.Visible = false;
				lblStatus.Text = "Ready!";
			});

		}

		private bool ValidateInput()
		{
			var validationResult = DatabaseImport.Validate(GetParameters());
			if (validationResult.Count > 0)
			{
				MessageBox.Show(string.Join("\r\n", validationResult.Select(v => v.ErrorMessage).ToArray()), "Validation error",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}

			return true;
		}

		private void btnValidate_Click(object sender, EventArgs e)
		{
			if (ValidateInput())
			{
				MessageBox.Show("Validation successful!", "Validation status",
					MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		private DatabaseImportParameters GetParameters()
		{
			var parameters = new DatabaseImportParameters();
			
			parameters.NormalRoutePath = txtNormalRoutePath.Text;
			parameters.GravityVectorPath = txtGravityVectorPath.Text;
			parameters.DeviationMapPath = txtDeviationMapPath.Text;
			parameters.NearMissMapPath = txtNearMiss.Text;
			parameters.ConnectionDetails.HostName = txtHostname.Text;
			parameters.ConnectionDetails.Port = txtPort.Text;
			parameters.ConnectionDetails.DatabaseName = txtDatabase.Text;
			parameters.ConnectionDetails.Username = txtUsername.Text;
			parameters.ConnectionDetails.Password = txtPassword.Text;
			parameters.DropAndCreate = chkDropAndRecreate.Checked;

			return parameters;
		}

		private void SetParameters(DatabaseImportParameters parameters)
		{
			// Database connection details
			txtHostname.Text = parameters.ConnectionDetails.HostName;
			txtDatabase.Text = parameters.ConnectionDetails.DatabaseName;
			txtPort.Text = parameters.ConnectionDetails.Port;
			txtUsername.Text = parameters.ConnectionDetails.Username;
			txtPassword.Text = parameters.ConnectionDetails.Password;

			// Input files
			txtNormalRoutePath.Text = parameters.NormalRoutePath;
			txtGravityVectorPath.Text = parameters.GravityVectorPath;
			txtDeviationMapPath.Text = parameters.DeviationMapPath;
			txtNearMiss.Text = parameters.NearMissMapPath; 
			chkDropAndRecreate.Checked = parameters.DropAndCreate;
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			var settings = GetParameters();
			var str = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
			File.WriteAllText(SettingsFilePath, str);
		}


	}
}