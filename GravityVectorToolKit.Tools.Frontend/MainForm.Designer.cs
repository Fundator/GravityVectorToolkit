
namespace GravityVectorToolKit.Tools.Frontend
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.lblConnectionString = new System.Windows.Forms.Label();
			this.lblNormalRoutes = new System.Windows.Forms.Label();
			this.lblGravityVectors = new System.Windows.Forms.Label();
			this.lblDeviationMap = new System.Windows.Forms.Label();
			this.txtConnectionString = new System.Windows.Forms.TextBox();
			this.txtNormalRoutePath = new System.Windows.Forms.TextBox();
			this.btnFindNormalRouteFile = new System.Windows.Forms.Button();
			this.btnFindGravityVectorFile = new System.Windows.Forms.Button();
			this.txtGravityVectorPath = new System.Windows.Forms.TextBox();
			this.btnFindDeviationMapFile = new System.Windows.Forms.Button();
			this.txtDeviationMapPath = new System.Windows.Forms.TextBox();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.btnValidate = new System.Windows.Forms.Button();
			this.btnImport = new System.Windows.Forms.Button();
			this.chkDropAndRecreate = new System.Windows.Forms.CheckBox();
			this.lblStatus = new System.Windows.Forms.Label();
			this.pictureBoxSpinner = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxSpinner)).BeginInit();
			this.SuspendLayout();
			// 
			// lblConnectionString
			// 
			this.lblConnectionString.Location = new System.Drawing.Point(12, 9);
			this.lblConnectionString.Name = "lblConnectionString";
			this.lblConnectionString.Size = new System.Drawing.Size(219, 25);
			this.lblConnectionString.TabIndex = 0;
			this.lblConnectionString.Text = "Connection string:";
			this.lblConnectionString.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblNormalRoutes
			// 
			this.lblNormalRoutes.Location = new System.Drawing.Point(12, 54);
			this.lblNormalRoutes.Name = "lblNormalRoutes";
			this.lblNormalRoutes.Size = new System.Drawing.Size(219, 25);
			this.lblNormalRoutes.TabIndex = 1;
			this.lblNormalRoutes.Text = "Normal route file:";
			this.lblNormalRoutes.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblGravityVectors
			// 
			this.lblGravityVectors.Location = new System.Drawing.Point(12, 100);
			this.lblGravityVectors.Name = "lblGravityVectors";
			this.lblGravityVectors.Size = new System.Drawing.Size(219, 25);
			this.lblGravityVectors.TabIndex = 2;
			this.lblGravityVectors.Text = "Gravity vector file:";
			this.lblGravityVectors.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblDeviationMap
			// 
			this.lblDeviationMap.Location = new System.Drawing.Point(9, 144);
			this.lblDeviationMap.Name = "lblDeviationMap";
			this.lblDeviationMap.Size = new System.Drawing.Size(222, 25);
			this.lblDeviationMap.TabIndex = 3;
			this.lblDeviationMap.Text = "Deviation map file:";
			this.lblDeviationMap.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// txtConnectionString
			// 
			this.txtConnectionString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtConnectionString.Location = new System.Drawing.Point(236, 9);
			this.txtConnectionString.Name = "txtConnectionString";
			this.txtConnectionString.Size = new System.Drawing.Size(530, 31);
			this.txtConnectionString.TabIndex = 4;
			// 
			// txtNormalRoutePath
			// 
			this.txtNormalRoutePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtNormalRoutePath.Location = new System.Drawing.Point(237, 54);
			this.txtNormalRoutePath.Name = "txtNormalRoutePath";
			this.txtNormalRoutePath.Size = new System.Drawing.Size(480, 31);
			this.txtNormalRoutePath.TabIndex = 5;
			// 
			// btnFindNormalRouteFile
			// 
			this.btnFindNormalRouteFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnFindNormalRouteFile.Location = new System.Drawing.Point(723, 54);
			this.btnFindNormalRouteFile.Name = "btnFindNormalRouteFile";
			this.btnFindNormalRouteFile.Size = new System.Drawing.Size(43, 31);
			this.btnFindNormalRouteFile.TabIndex = 6;
			this.btnFindNormalRouteFile.Text = "...";
			this.btnFindNormalRouteFile.UseVisualStyleBackColor = true;
			this.btnFindNormalRouteFile.Click += new System.EventHandler(this.btnFindNormalRouteFile_Click);
			// 
			// btnFindGravityVectorFile
			// 
			this.btnFindGravityVectorFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnFindGravityVectorFile.Location = new System.Drawing.Point(723, 100);
			this.btnFindGravityVectorFile.Name = "btnFindGravityVectorFile";
			this.btnFindGravityVectorFile.Size = new System.Drawing.Size(43, 31);
			this.btnFindGravityVectorFile.TabIndex = 8;
			this.btnFindGravityVectorFile.Text = "...";
			this.btnFindGravityVectorFile.UseVisualStyleBackColor = true;
			this.btnFindGravityVectorFile.Click += new System.EventHandler(this.btnFindGravityVectorFile_Click);
			// 
			// txtGravityVectorPath
			// 
			this.txtGravityVectorPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtGravityVectorPath.Location = new System.Drawing.Point(237, 100);
			this.txtGravityVectorPath.Name = "txtGravityVectorPath";
			this.txtGravityVectorPath.Size = new System.Drawing.Size(480, 31);
			this.txtGravityVectorPath.TabIndex = 7;
			// 
			// btnFindDeviationMapFile
			// 
			this.btnFindDeviationMapFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnFindDeviationMapFile.Location = new System.Drawing.Point(723, 144);
			this.btnFindDeviationMapFile.Name = "btnFindDeviationMapFile";
			this.btnFindDeviationMapFile.Size = new System.Drawing.Size(43, 31);
			this.btnFindDeviationMapFile.TabIndex = 10;
			this.btnFindDeviationMapFile.Text = "...";
			this.btnFindDeviationMapFile.UseVisualStyleBackColor = true;
			this.btnFindDeviationMapFile.Click += new System.EventHandler(this.btnFindDeviationMapFile_Click);
			// 
			// txtDeviationMapPath
			// 
			this.txtDeviationMapPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtDeviationMapPath.Location = new System.Drawing.Point(237, 144);
			this.txtDeviationMapPath.Name = "txtDeviationMapPath";
			this.txtDeviationMapPath.Size = new System.Drawing.Size(480, 31);
			this.txtDeviationMapPath.TabIndex = 9;
			// 
			// progressBar1
			// 
			this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.progressBar1.Location = new System.Drawing.Point(12, 323);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(754, 29);
			this.progressBar1.TabIndex = 11;
			// 
			// btnValidate
			// 
			this.btnValidate.Location = new System.Drawing.Point(236, 233);
			this.btnValidate.Name = "btnValidate";
			this.btnValidate.Size = new System.Drawing.Size(112, 34);
			this.btnValidate.TabIndex = 12;
			this.btnValidate.Text = "Validate";
			this.btnValidate.UseVisualStyleBackColor = true;
			this.btnValidate.Click += new System.EventHandler(this.btnValidate_Click);
			// 
			// btnImport
			// 
			this.btnImport.Location = new System.Drawing.Point(354, 233);
			this.btnImport.Name = "btnImport";
			this.btnImport.Size = new System.Drawing.Size(112, 34);
			this.btnImport.TabIndex = 13;
			this.btnImport.Text = "Import";
			this.btnImport.UseVisualStyleBackColor = true;
			this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
			// 
			// chkDropAndRecreate
			// 
			this.chkDropAndRecreate.AutoSize = true;
			this.chkDropAndRecreate.Checked = true;
			this.chkDropAndRecreate.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkDropAndRecreate.Location = new System.Drawing.Point(237, 187);
			this.chkDropAndRecreate.Name = "chkDropAndRecreate";
			this.chkDropAndRecreate.Size = new System.Drawing.Size(364, 29);
			this.chkDropAndRecreate.TabIndex = 14;
			this.chkDropAndRecreate.Text = "Drop and recreate tables (recommended)";
			this.chkDropAndRecreate.UseVisualStyleBackColor = true;
			// 
			// lblStatus
			// 
			this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblStatus.AutoSize = true;
			this.lblStatus.Location = new System.Drawing.Point(12, 282);
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size(65, 25);
			this.lblStatus.TabIndex = 17;
			this.lblStatus.Text = "Ready!";
			// 
			// pictureBoxSpinner
			// 
			this.pictureBoxSpinner.BackColor = System.Drawing.Color.Transparent;
			this.pictureBoxSpinner.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxSpinner.Image")));
			this.pictureBoxSpinner.Location = new System.Drawing.Point(473, 233);
			this.pictureBoxSpinner.Name = "pictureBoxSpinner";
			this.pictureBoxSpinner.Size = new System.Drawing.Size(53, 34);
			this.pictureBoxSpinner.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBoxSpinner.TabIndex = 18;
			this.pictureBoxSpinner.TabStop = false;
			this.pictureBoxSpinner.Visible = false;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(778, 364);
			this.Controls.Add(this.pictureBoxSpinner);
			this.Controls.Add(this.lblStatus);
			this.Controls.Add(this.chkDropAndRecreate);
			this.Controls.Add(this.btnImport);
			this.Controls.Add(this.btnValidate);
			this.Controls.Add(this.progressBar1);
			this.Controls.Add(this.btnFindDeviationMapFile);
			this.Controls.Add(this.txtDeviationMapPath);
			this.Controls.Add(this.btnFindGravityVectorFile);
			this.Controls.Add(this.txtGravityVectorPath);
			this.Controls.Add(this.btnFindNormalRouteFile);
			this.Controls.Add(this.txtNormalRoutePath);
			this.Controls.Add(this.txtConnectionString);
			this.Controls.Add(this.lblDeviationMap);
			this.Controls.Add(this.lblGravityVectors);
			this.Controls.Add(this.lblNormalRoutes);
			this.Controls.Add(this.lblConnectionString);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(800, 420);
			this.Name = "MainForm";
			this.Text = "MADART Database Import";
			this.Load += new System.EventHandler(this.MainForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxSpinner)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblConnectionString;
		private System.Windows.Forms.Label lblNormalRoutes;
		private System.Windows.Forms.Label lblGravityVectors;
		private System.Windows.Forms.Label lblDeviationMap;
		private System.Windows.Forms.TextBox txtConnectionString;
		private System.Windows.Forms.TextBox txtNormalRoutePath;
		private System.Windows.Forms.Button btnFindNormalRouteFile;
		private System.Windows.Forms.Button btnFindGravityVectorFile;
		private System.Windows.Forms.TextBox txtGravityVectorPath;
		private System.Windows.Forms.TextBox txtDeviationMapPath;
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.Button btnValidate;
		private System.Windows.Forms.Button btnImport;
		private System.Windows.Forms.Button btnFindDeviationMapFile;
		private System.Windows.Forms.CheckBox chkDropAndRecreate;
		private System.Windows.Forms.Label lblStatus;
		private System.Windows.Forms.PictureBox pictureBoxSpinner;
	}
}