
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
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.btnValidate = new System.Windows.Forms.Button();
			this.btnImport = new System.Windows.Forms.Button();
			this.chkDropAndRecreate = new System.Windows.Forms.CheckBox();
			this.lblStatus = new System.Windows.Forms.Label();
			this.pictureBoxSpinner = new System.Windows.Forms.PictureBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.txtHostname = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.txtUsername = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.txtDatabase = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txtPort = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.txtPassword = new System.Windows.Forms.TextBox();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.btnFindDeviationMapFile = new System.Windows.Forms.Button();
			this.txtDeviationMapPath = new System.Windows.Forms.TextBox();
			this.btnFindGravityVectorFile = new System.Windows.Forms.Button();
			this.txtGravityVectorPath = new System.Windows.Forms.TextBox();
			this.btnFindNormalRouteFile = new System.Windows.Forms.Button();
			this.txtNormalRoutePath = new System.Windows.Forms.TextBox();
			this.lblDeviationMap = new System.Windows.Forms.Label();
			this.lblGravityVectors = new System.Windows.Forms.Label();
			this.lblNormalRoutes = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxSpinner)).BeginInit();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// progressBar1
			// 
			this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.progressBar1.Location = new System.Drawing.Point(12, 543);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(654, 29);
			this.progressBar1.TabIndex = 11;
			// 
			// btnValidate
			// 
			this.btnValidate.Location = new System.Drawing.Point(12, 447);
			this.btnValidate.Name = "btnValidate";
			this.btnValidate.Size = new System.Drawing.Size(112, 34);
			this.btnValidate.TabIndex = 12;
			this.btnValidate.Text = "Validate";
			this.btnValidate.UseVisualStyleBackColor = true;
			this.btnValidate.Click += new System.EventHandler(this.btnValidate_Click);
			// 
			// btnImport
			// 
			this.btnImport.Location = new System.Drawing.Point(130, 447);
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
			this.chkDropAndRecreate.Location = new System.Drawing.Point(13, 401);
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
			this.lblStatus.Location = new System.Drawing.Point(12, 502);
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size(65, 25);
			this.lblStatus.TabIndex = 17;
			this.lblStatus.Text = "Ready!";
			// 
			// pictureBoxSpinner
			// 
			this.pictureBoxSpinner.BackColor = System.Drawing.Color.Transparent;
			this.pictureBoxSpinner.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxSpinner.Image")));
			this.pictureBoxSpinner.Location = new System.Drawing.Point(249, 447);
			this.pictureBoxSpinner.Name = "pictureBoxSpinner";
			this.pictureBoxSpinner.Size = new System.Drawing.Size(53, 34);
			this.pictureBoxSpinner.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBoxSpinner.TabIndex = 18;
			this.pictureBoxSpinner.TabStop = false;
			this.pictureBoxSpinner.Visible = false;
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.txtHostname);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.txtUsername);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.txtDatabase);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.txtPort);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.txtPassword);
			this.groupBox1.Controls.Add(this.textBox1);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(651, 218);
			this.groupBox1.TabIndex = 21;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Connection Details";
			// 
			// txtHostname
			// 
			this.txtHostname.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtHostname.Location = new System.Drawing.Point(178, 24);
			this.txtHostname.Name = "txtHostname";
			this.txtHostname.Size = new System.Drawing.Size(463, 31);
			this.txtHostname.TabIndex = 32;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(68, 175);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(107, 25);
			this.label5.TabIndex = 30;
			this.label5.Text = "Password:";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// txtUsername
			// 
			this.txtUsername.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtUsername.Location = new System.Drawing.Point(178, 135);
			this.txtUsername.Name = "txtUsername";
			this.txtUsername.Size = new System.Drawing.Size(463, 31);
			this.txtUsername.TabIndex = 29;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(68, 138);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(107, 25);
			this.label4.TabIndex = 28;
			this.label4.Text = "Username:";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// txtDatabase
			// 
			this.txtDatabase.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtDatabase.Location = new System.Drawing.Point(178, 98);
			this.txtDatabase.Name = "txtDatabase";
			this.txtDatabase.Size = new System.Drawing.Size(463, 31);
			this.txtDatabase.TabIndex = 27;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(68, 101);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(107, 25);
			this.label3.TabIndex = 26;
			this.label3.Text = "Database:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// txtPort
			// 
			this.txtPort.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtPort.Location = new System.Drawing.Point(178, 61);
			this.txtPort.Name = "txtPort";
			this.txtPort.Size = new System.Drawing.Size(463, 31);
			this.txtPort.TabIndex = 25;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(68, 64);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(107, 25);
			this.label2.TabIndex = 24;
			this.label2.Text = "Port:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// txtPassword
			// 
			this.txtPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtPassword.Location = new System.Drawing.Point(178, 172);
			this.txtPassword.Name = "txtPassword";
			this.txtPassword.Size = new System.Drawing.Size(463, 31);
			this.txtPassword.TabIndex = 31;
			// 
			// textBox1
			// 
			this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBox1.Location = new System.Drawing.Point(-2631, 68);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(1351, 31);
			this.textBox1.TabIndex = 22;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(68, 27);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(107, 25);
			this.label1.TabIndex = 21;
			this.label1.Text = "Hostname:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.btnFindDeviationMapFile);
			this.groupBox2.Controls.Add(this.txtDeviationMapPath);
			this.groupBox2.Controls.Add(this.btnFindGravityVectorFile);
			this.groupBox2.Controls.Add(this.txtGravityVectorPath);
			this.groupBox2.Controls.Add(this.btnFindNormalRouteFile);
			this.groupBox2.Controls.Add(this.txtNormalRoutePath);
			this.groupBox2.Controls.Add(this.lblDeviationMap);
			this.groupBox2.Controls.Add(this.lblGravityVectors);
			this.groupBox2.Controls.Add(this.lblNormalRoutes);
			this.groupBox2.Location = new System.Drawing.Point(13, 237);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(650, 150);
			this.groupBox2.TabIndex = 22;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Input files";
			// 
			// btnFindDeviationMapFile
			// 
			this.btnFindDeviationMapFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnFindDeviationMapFile.Location = new System.Drawing.Point(597, 106);
			this.btnFindDeviationMapFile.Name = "btnFindDeviationMapFile";
			this.btnFindDeviationMapFile.Size = new System.Drawing.Size(43, 31);
			this.btnFindDeviationMapFile.TabIndex = 19;
			this.btnFindDeviationMapFile.Text = "...";
			this.btnFindDeviationMapFile.UseVisualStyleBackColor = true;
			this.btnFindDeviationMapFile.Click += new System.EventHandler(this.btnFindDeviationMapFile_Click);
			// 
			// txtDeviationMapPath
			// 
			this.txtDeviationMapPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtDeviationMapPath.Location = new System.Drawing.Point(177, 106);
			this.txtDeviationMapPath.Name = "txtDeviationMapPath";
			this.txtDeviationMapPath.Size = new System.Drawing.Size(409, 31);
			this.txtDeviationMapPath.TabIndex = 18;
			// 
			// btnFindGravityVectorFile
			// 
			this.btnFindGravityVectorFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnFindGravityVectorFile.Location = new System.Drawing.Point(597, 69);
			this.btnFindGravityVectorFile.Name = "btnFindGravityVectorFile";
			this.btnFindGravityVectorFile.Size = new System.Drawing.Size(43, 31);
			this.btnFindGravityVectorFile.TabIndex = 17;
			this.btnFindGravityVectorFile.Text = "...";
			this.btnFindGravityVectorFile.UseVisualStyleBackColor = true;
			this.btnFindGravityVectorFile.Click += new System.EventHandler(this.btnFindGravityVectorFile_Click);
			// 
			// txtGravityVectorPath
			// 
			this.txtGravityVectorPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtGravityVectorPath.Location = new System.Drawing.Point(177, 69);
			this.txtGravityVectorPath.Name = "txtGravityVectorPath";
			this.txtGravityVectorPath.Size = new System.Drawing.Size(409, 31);
			this.txtGravityVectorPath.TabIndex = 16;
			// 
			// btnFindNormalRouteFile
			// 
			this.btnFindNormalRouteFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnFindNormalRouteFile.Location = new System.Drawing.Point(597, 32);
			this.btnFindNormalRouteFile.Name = "btnFindNormalRouteFile";
			this.btnFindNormalRouteFile.Size = new System.Drawing.Size(43, 31);
			this.btnFindNormalRouteFile.TabIndex = 15;
			this.btnFindNormalRouteFile.Text = "...";
			this.btnFindNormalRouteFile.UseVisualStyleBackColor = true;
			this.btnFindNormalRouteFile.Click += new System.EventHandler(this.btnFindNormalRouteFile_Click);
			// 
			// txtNormalRoutePath
			// 
			this.txtNormalRoutePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtNormalRoutePath.Location = new System.Drawing.Point(177, 32);
			this.txtNormalRoutePath.Name = "txtNormalRoutePath";
			this.txtNormalRoutePath.Size = new System.Drawing.Size(409, 31);
			this.txtNormalRoutePath.TabIndex = 14;
			// 
			// lblDeviationMap
			// 
			this.lblDeviationMap.Location = new System.Drawing.Point(12, 109);
			this.lblDeviationMap.Name = "lblDeviationMap";
			this.lblDeviationMap.Size = new System.Drawing.Size(163, 25);
			this.lblDeviationMap.TabIndex = 13;
			this.lblDeviationMap.Text = "Deviation map file:";
			this.lblDeviationMap.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblGravityVectors
			// 
			this.lblGravityVectors.Location = new System.Drawing.Point(12, 72);
			this.lblGravityVectors.Name = "lblGravityVectors";
			this.lblGravityVectors.Size = new System.Drawing.Size(163, 25);
			this.lblGravityVectors.TabIndex = 12;
			this.lblGravityVectors.Text = "Gravity vector file:";
			this.lblGravityVectors.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblNormalRoutes
			// 
			this.lblNormalRoutes.Location = new System.Drawing.Point(12, 32);
			this.lblNormalRoutes.Name = "lblNormalRoutes";
			this.lblNormalRoutes.Size = new System.Drawing.Size(163, 25);
			this.lblNormalRoutes.TabIndex = 11;
			this.lblNormalRoutes.Text = "Normal route file:";
			this.lblNormalRoutes.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(678, 584);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.pictureBoxSpinner);
			this.Controls.Add(this.lblStatus);
			this.Controls.Add(this.chkDropAndRecreate);
			this.Controls.Add(this.btnImport);
			this.Controls.Add(this.btnValidate);
			this.Controls.Add(this.progressBar1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(700, 640);
			this.Name = "MainForm";
			this.Text = "MADART Database Import";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			this.Load += new System.EventHandler(this.MainForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxSpinner)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.Button btnValidate;
		private System.Windows.Forms.Button btnImport;
		private System.Windows.Forms.CheckBox chkDropAndRecreate;
		private System.Windows.Forms.Label lblStatus;
		private System.Windows.Forms.PictureBox pictureBoxSpinner;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox txtUsername;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txtDatabase;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtPort;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtPassword;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Button btnFindDeviationMapFile;
		private System.Windows.Forms.TextBox txtDeviationMapPath;
		private System.Windows.Forms.Button btnFindGravityVectorFile;
		private System.Windows.Forms.TextBox txtGravityVectorPath;
		private System.Windows.Forms.Button btnFindNormalRouteFile;
		private System.Windows.Forms.TextBox txtNormalRoutePath;
		private System.Windows.Forms.Label lblDeviationMap;
		private System.Windows.Forms.Label lblGravityVectors;
		private System.Windows.Forms.Label lblNormalRoutes;
		private System.Windows.Forms.TextBox txtHostname;
	}
}