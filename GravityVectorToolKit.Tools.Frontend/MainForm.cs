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

namespace GravityVectorToolKit.Tools.Frontend
{

	

	public partial class MainForm : Form
	{

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

		public MainForm()
		{
			InitializeComponent();

			// Configure file dialogs
			dlgNormalRouteFile.Title = "Select normal route file";
			dlgGravityVectorFile.Title = "Select gravity vector file";
			dlgDeviationMapFile.Title = "Select deviation map file";
			ConfigureCommonProperties(dlgNormalRouteFile);
			ConfigureCommonProperties(dlgGravityVectorFile);
			ConfigureCommonProperties(dlgDeviationMapFile);

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
			parameters.ConnectionString = txtConnectionString.Text;
			parameters.DropAndCreate = chkDropAndRecreate.Checked;
			return parameters;
		}
	}

	static class ControlExtensions
	{

		private delegate void SetPropertyThreadSafeDelegate<TResult>(
			Control @this,
			Expression<Func<TResult>> property,
			TResult value);

		public static void SetPropertyThreadSafe<TResult>(
			this Control @this,
			Expression<Func<TResult>> property,
			TResult value)
		{
			var propertyInfo = (property.Body as MemberExpression).Member
				as PropertyInfo;

			if (propertyInfo == null ||
				!@this.GetType().IsSubclassOf(propertyInfo.ReflectedType) ||
				@this.GetType().GetProperty(
					propertyInfo.Name,
					propertyInfo.PropertyType) == null)
			{
				throw new ArgumentException("The lambda expression 'property' must reference a valid property on this Control.");
			}

			if (@this.InvokeRequired)
			{
				@this.Invoke(new SetPropertyThreadSafeDelegate<TResult>
				(SetPropertyThreadSafe),
				new object[] { @this, property, value });
			}
			else
			{
				@this.GetType().InvokeMember(
					propertyInfo.Name,
					BindingFlags.SetProperty,
					null,
					@this,
					new object[] { value });
			}
		}
	}
}
