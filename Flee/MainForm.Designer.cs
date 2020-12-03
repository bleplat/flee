using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Microsoft.VisualBasic.CompilerServices;

namespace Flee {
	[DesignerGenerated()]
	public partial class MainForm : Form {

		// Form remplace la méthode Dispose pour nettoyer la liste des composants.
		[DebuggerNonUserCode()]
		protected override void Dispose(bool disposing) {
			try {
				if (disposing && components is object) components.Dispose();
			} finally {
				base.Dispose(disposing);
			}
		}

		// Requise par le Concepteur Windows Form
		private System.ComponentModel.IContainer components;

		// REMARQUE : la procédure suivante est requise par le Concepteur Windows Form
		// Elle peut être modifiée à l'aide du Concepteur Windows Form.  
		// Ne la modifiez pas à l'aide de l'éditeur de code.
		[DebuggerStepThrough()]
		private void InitializeComponent() {
			components = new System.ComponentModel.Container();
			var resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			MainPanel = new Panel();
			_MiniBox = new PictureBox();
			_MiniBox.MouseDown += new MouseEventHandler(MiniBox_MouseDown);
			_MiniBox.MouseUp += new MouseEventHandler(MiniBox_MouseUp);
			_MiniBox.MouseMove += new MouseEventHandler(MiniBox_MouseMove);
			MenuPanel = new Panel();
			Label4 = new Label();
			_RandomizeButton = new Button();
			_RandomizeButton.Click += new EventHandler(RandomizeButton_Click);
			Label3 = new Label();
			SeedTextBox = new TextBox();
			Label2 = new Label();
			_StartPlayingButton = new Button();
			_StartPlayingButton.Click += new EventHandler(BeginButton_Click);
			UpgradeDetails = new Panel();
			PriceA = new Label();
			PriceU = new Label();
			PriceC = new Label();
			PriceM = new Label();
			UpDesc = new Label();
			UpName = new Label();
			PriceCIcon = new PictureBox();
			PriceAIcon = new PictureBox();
			PriceUIcon = new PictureBox();
			PriceMIcon = new PictureBox();
			SShipPanel = new Panel();
			AllowMiningBox = new PictureBox();
			SShipUpsMax = new Label();
			_UpgradesBox = new PictureBox();
			_UpgradesBox.MouseMove += new MouseEventHandler(UpgradesBox_MouseMove);
			_UpgradesBox.Click += new EventHandler(UpgradesBox_Click);
			_UpgradesBox.MouseLeave += new EventHandler(UpgradesBox_MouseLeave);
			SShipImageBox = new PictureBox();
			Label1 = new Label();
			SShipTypeBox = new Label();
			PanelRes = new Panel();
			WarCriminalLabel = new Label();
			AntimatterTextBox = new Label();
			UraniumTextBox = new Label();
			CristalTextBox = new Label();
			MetalTextBox = new Label();
			_PictureBox5 = new PictureBox();
			_PictureBox5.Click += new EventHandler(PictureBox2_Click);
			_PictureBox6 = new PictureBox();
			_PictureBox6.Click += new EventHandler(PictureBox2_Click);
			_PictureBox2 = new PictureBox();
			_PictureBox2.Click += new EventHandler(PictureBox2_Click);
			PictureBox1 = new PictureBox();
			_DrawBox = new PictureBox();
			_DrawBox.MouseDown += new MouseEventHandler(DrawBox_MouseDown);
			_DrawBox.MouseMove += new MouseEventHandler(DrawBox_MouseMove);
			_DrawBox.MouseUp += new MouseEventHandler(DrawBox_MouseUp);
			_DrawBox.SizeChanged += new EventHandler(DrawBox_SizeChanged);
			_Ticker = new Timer(components);
			_Ticker.Tick += new EventHandler(Ticker_Tick);
			MainPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)_MiniBox).BeginInit();
			MenuPanel.SuspendLayout();
			UpgradeDetails.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)PriceCIcon).BeginInit();
			((System.ComponentModel.ISupportInitialize)PriceAIcon).BeginInit();
			((System.ComponentModel.ISupportInitialize)PriceUIcon).BeginInit();
			((System.ComponentModel.ISupportInitialize)PriceMIcon).BeginInit();
			SShipPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)AllowMiningBox).BeginInit();
			((System.ComponentModel.ISupportInitialize)_UpgradesBox).BeginInit();
			((System.ComponentModel.ISupportInitialize)SShipImageBox).BeginInit();
			PanelRes.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)_PictureBox5).BeginInit();
			((System.ComponentModel.ISupportInitialize)_PictureBox6).BeginInit();
			((System.ComponentModel.ISupportInitialize)_PictureBox2).BeginInit();
			((System.ComponentModel.ISupportInitialize)PictureBox1).BeginInit();
			((System.ComponentModel.ISupportInitialize)_DrawBox).BeginInit();
			SuspendLayout();
			// 
			// MainPanel
			// 
			MainPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

			MainPanel.Controls.Add(_MiniBox);
			MainPanel.Controls.Add(MenuPanel);
			MainPanel.Controls.Add(UpgradeDetails);
			MainPanel.Controls.Add(SShipPanel);
			MainPanel.Controls.Add(PanelRes);
			MainPanel.Controls.Add(_DrawBox);
			MainPanel.Location = new Point(0, 0);
			MainPanel.Name = "MainPanel";
			MainPanel.Size = new Size(800, 600);
			MainPanel.TabIndex = 0;
			// 
			// MiniBox
			// 
			_MiniBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			_MiniBox.BorderStyle = BorderStyle.FixedSingle;
			_MiniBox.Location = new Point(599, 399);
			_MiniBox.Name = "_MiniBox";
			_MiniBox.Size = new Size(202, 202);
			_MiniBox.SizeMode = PictureBoxSizeMode.StretchImage;
			_MiniBox.TabIndex = 1;
			_MiniBox.TabStop = false;
			// 
			// MenuPanel
			// 
			MenuPanel.Anchor = AnchorStyles.Left;
			MenuPanel.Controls.Add(Label4);
			MenuPanel.Controls.Add(_RandomizeButton);
			MenuPanel.Controls.Add(Label3);
			MenuPanel.Controls.Add(SeedTextBox);
			MenuPanel.Controls.Add(Label2);
			MenuPanel.Controls.Add(_StartPlayingButton);
			MenuPanel.Font = new Font("Constantia", 15.75f, FontStyle.Bold, GraphicsUnit.Point, Conversions.ToByte(0));
			MenuPanel.Location = new Point(155, 162);
			MenuPanel.Name = "MenuPanel";
			MenuPanel.Size = new Size(312, 249);
			MenuPanel.TabIndex = 5;
			// 
			// Label4
			// 
			Label4.Font = new Font("Constantia", 8.25f, FontStyle.Regular, GraphicsUnit.Point, Conversions.ToByte(0));
			Label4.Location = new Point(5, 144);
			Label4.Name = "Label4";
			Label4.Size = new Size(302, 57);
			Label4.TabIndex = 8;
			Label4.Text = "" + '\r' + '\n' + "Some sprites are from MillionthVector." + '\r' + '\n' + "The music is from PhilippWeigl.";
			// 
			// RandomizeButton
			// 
			_RandomizeButton.FlatStyle = FlatStyle.Flat;
			_RandomizeButton.Font = new Font("Corbel", 15.75f, FontStyle.Bold, GraphicsUnit.Point, Conversions.ToByte(0));
			_RandomizeButton.Location = new Point(79, 89);
			_RandomizeButton.Name = "_RandomizeButton";
			_RandomizeButton.Size = new Size(230, 41);
			_RandomizeButton.TabIndex = 7;
			_RandomizeButton.Text = "Randomize";
			_RandomizeButton.UseVisualStyleBackColor = true;
			// 
			// Label3
			// 
			Label3.AutoSize = true;
			Label3.Font = new Font("Corbel", 24.0f, FontStyle.Bold, GraphicsUnit.Point, Conversions.ToByte(0));
			Label3.Location = new Point(3, 0);
			Label3.Name = "Label3";
			Label3.Size = new Size(87, 39);
			Label3.TabIndex = 6;
			Label3.Text = "FLEE";
			// 
			// SeedTextBox
			// 
			SeedTextBox.BackColor = Color.FromArgb(Conversions.ToInteger(Conversions.ToByte(64)), Conversions.ToInteger(Conversions.ToByte(64)), Conversions.ToInteger(Conversions.ToByte(64)));
			SeedTextBox.BorderStyle = BorderStyle.FixedSingle;
			SeedTextBox.Font = new Font("Corbel", 15.75f, FontStyle.Bold, GraphicsUnit.Point, Conversions.ToByte(0));
			SeedTextBox.ForeColor = Color.White;
			SeedTextBox.Location = new Point(79, 50);
			SeedTextBox.Name = "SeedTextBox";
			SeedTextBox.Size = new Size(230, 33);
			SeedTextBox.TabIndex = 6;
			SeedTextBox.Text = "0";
			SeedTextBox.TextAlign = HorizontalAlignment.Center;
			// 
			// Label2
			// 
			Label2.AutoSize = true;
			Label2.Font = new Font("Corbel", 12.0f, FontStyle.Regular, GraphicsUnit.Point, Conversions.ToByte(0));
			Label2.Location = new Point(3, 57);
			Label2.Name = "Label2";
			Label2.Size = new Size(47, 19);
			Label2.TabIndex = 1;
			Label2.Text = "SEED";
			// 
			// StartPlayingButton
			// 
			_StartPlayingButton.Dock = DockStyle.Bottom;
			_StartPlayingButton.FlatStyle = FlatStyle.Flat;
			_StartPlayingButton.Font = new Font("Corbel", 14.25f, FontStyle.Bold, GraphicsUnit.Point, Conversions.ToByte(0));
			_StartPlayingButton.Location = new Point(0, 211);
			_StartPlayingButton.Name = "_StartPlayingButton";
			_StartPlayingButton.Size = new Size(312, 38);
			_StartPlayingButton.TabIndex = 0;
			_StartPlayingButton.Text = "Play";
			_StartPlayingButton.UseVisualStyleBackColor = true;
			// 
			// UpgradeDetails
			// 
			UpgradeDetails.Anchor = AnchorStyles.Top;
			UpgradeDetails.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			UpgradeDetails.BorderStyle = BorderStyle.FixedSingle;
			UpgradeDetails.Controls.Add(PriceA);
			UpgradeDetails.Controls.Add(PriceU);
			UpgradeDetails.Controls.Add(PriceC);
			UpgradeDetails.Controls.Add(PriceM);
			UpgradeDetails.Controls.Add(UpDesc);
			UpgradeDetails.Controls.Add(UpName);
			UpgradeDetails.Controls.Add(PriceCIcon);
			UpgradeDetails.Controls.Add(PriceAIcon);
			UpgradeDetails.Controls.Add(PriceUIcon);
			UpgradeDetails.Controls.Add(PriceMIcon);
			UpgradeDetails.Location = new Point(188, 12);
			UpgradeDetails.Name = "UpgradeDetails";
			UpgradeDetails.Size = new Size(412, 94);
			UpgradeDetails.TabIndex = 4;
			UpgradeDetails.Visible = false;
			// 
			// PriceA
			// 
			PriceA.Font = new Font("Consolas", 8.25f, FontStyle.Regular, GraphicsUnit.Point, Conversions.ToByte(0));
			PriceA.ForeColor = Color.Yellow;
			PriceA.Location = new Point(236, 19);
			PriceA.Name = "PriceA";
			PriceA.Size = new Size(40, 20);
			PriceA.TabIndex = 1;
			PriceA.Text = "99999";
			PriceA.TextAlign = ContentAlignment.MiddleRight;
			// 
			// PriceU
			// 
			PriceU.Font = new Font("Consolas", 8.25f, FontStyle.Regular, GraphicsUnit.Point, Conversions.ToByte(0));
			PriceU.ForeColor = Color.Lime;
			PriceU.Location = new Point(189, 19);
			PriceU.Name = "PriceU";
			PriceU.Size = new Size(25, 20);
			PriceU.TabIndex = 1;
			PriceU.Text = "99";
			PriceU.TextAlign = ContentAlignment.MiddleRight;
			// 
			// PriceC
			// 
			PriceC.Font = new Font("Consolas", 8.25f, FontStyle.Regular, GraphicsUnit.Point, Conversions.ToByte(0));
			PriceC.ForeColor = Color.Violet;
			PriceC.Location = new Point(296, 19);
			PriceC.Name = "PriceC";
			PriceC.Size = new Size(28, 20);
			PriceC.TabIndex = 1;
			PriceC.Text = "999";
			PriceC.TextAlign = ContentAlignment.MiddleRight;
			// 
			// PriceM
			// 
			PriceM.Font = new Font("Consolas", 8.25f, FontStyle.Regular, GraphicsUnit.Point, Conversions.ToByte(0));
			PriceM.ForeColor = Color.FromArgb(Conversions.ToInteger(Conversions.ToByte(255)), Conversions.ToInteger(Conversions.ToByte(128)), Conversions.ToInteger(Conversions.ToByte(0)));
			PriceM.Location = new Point(346, 19);
			PriceM.Name = "PriceM";
			PriceM.Size = new Size(40, 20);
			PriceM.TabIndex = 1;
			PriceM.Text = "99999";
			PriceM.TextAlign = ContentAlignment.MiddleRight;
			// 
			// UpDesc
			// 
			UpDesc.Dock = DockStyle.Bottom;
			UpDesc.Font = new Font("Consolas", 8.25f, FontStyle.Regular, GraphicsUnit.Point, Conversions.ToByte(0));
			UpDesc.Location = new Point(0, 42);
			UpDesc.Name = "UpDesc";
			UpDesc.Size = new Size(410, 50);
			UpDesc.TabIndex = 0;
			UpDesc.Text = "Description";
			// 
			// UpName
			// 
			UpName.Dock = DockStyle.Top;
			UpName.Font = new Font("Consolas", 11.25f, FontStyle.Bold, GraphicsUnit.Point, Conversions.ToByte(0));
			UpName.ForeColor = Color.White;
			UpName.Location = new Point(0, 0);
			UpName.Name = "UpName";
			UpName.Size = new Size(410, 18);
			UpName.TabIndex = 0;
			UpName.Text = "Upgrade name.";
			// 
			// PriceCIcon
			// 
			PriceCIcon.Image = My.Resources.Resources.Crystal;
			PriceCIcon.Location = new Point(325, 19);
			PriceCIcon.Name = "PriceCIcon";
			PriceCIcon.Size = new Size(20, 20);
			PriceCIcon.SizeMode = PictureBoxSizeMode.StretchImage;
			PriceCIcon.TabIndex = 0;
			PriceCIcon.TabStop = false;
			// 
			// PriceAIcon
			// 
			PriceAIcon.Image = My.Resources.Resources.Antimatter;
			PriceAIcon.Location = new Point(277, 19);
			PriceAIcon.Name = "PriceAIcon";
			PriceAIcon.Size = new Size(20, 20);
			PriceAIcon.SizeMode = PictureBoxSizeMode.StretchImage;
			PriceAIcon.TabIndex = 0;
			PriceAIcon.TabStop = false;
			// 
			// PriceUIcon
			// 
			PriceUIcon.Image = My.Resources.Resources.Fissile;
			PriceUIcon.Location = new Point(215, 19);
			PriceUIcon.Name = "PriceUIcon";
			PriceUIcon.Size = new Size(20, 20);
			PriceUIcon.SizeMode = PictureBoxSizeMode.StretchImage;
			PriceUIcon.TabIndex = 0;
			PriceUIcon.TabStop = false;
			// 
			// PriceMIcon
			// 
			PriceMIcon.Image = My.Resources.Resources.Metal;
			PriceMIcon.Location = new Point(387, 19);
			PriceMIcon.Name = "PriceMIcon";
			PriceMIcon.Size = new Size(20, 20);
			PriceMIcon.SizeMode = PictureBoxSizeMode.Zoom;
			PriceMIcon.TabIndex = 0;
			PriceMIcon.TabStop = false;
			// 
			// SShipPanel
			// 
			SShipPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
			SShipPanel.BorderStyle = BorderStyle.FixedSingle;
			SShipPanel.Controls.Add(AllowMiningBox);
			SShipPanel.Controls.Add(SShipUpsMax);
			SShipPanel.Controls.Add(_UpgradesBox);
			SShipPanel.Controls.Add(SShipImageBox);
			SShipPanel.Controls.Add(Label1);
			SShipPanel.Controls.Add(SShipTypeBox);
			SShipPanel.Location = new Point(600, 67);
			SShipPanel.Name = "SShipPanel";
			SShipPanel.Size = new Size(200, 332);
			SShipPanel.TabIndex = 3;
			// 
			// AllowMiningBox
			// 
			AllowMiningBox.Image = My.Resources.Resources.DeadSkull;
			AllowMiningBox.Location = new Point(55, 39);
			AllowMiningBox.Name = "AllowMiningBox";
			AllowMiningBox.Size = new Size(16, 16);
			AllowMiningBox.SizeMode = PictureBoxSizeMode.StretchImage;
			AllowMiningBox.TabIndex = 4;
			AllowMiningBox.TabStop = false;
			// 
			// SShipUpsMax
			// 
			SShipUpsMax.Font = new Font("Consolas", 8.25f, FontStyle.Bold, GraphicsUnit.Point, Conversions.ToByte(0));
			SShipUpsMax.ForeColor = Color.White;
			SShipUpsMax.Location = new Point(59, 23);
			SShipUpsMax.Name = "SShipUpsMax";
			SShipUpsMax.Size = new Size(50, 16);
			SShipUpsMax.TabIndex = 3;
			SShipUpsMax.Text = "00 / XX";
			SShipUpsMax.TextAlign = ContentAlignment.MiddleCenter;
			// 
			// UpgradesBox
			// 
			_UpgradesBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

			_UpgradesBox.Location = new Point(0, 55);
			_UpgradesBox.Name = "_UpgradesBox";
			_UpgradesBox.Size = new Size(200, 272);
			_UpgradesBox.TabIndex = 2;
			_UpgradesBox.TabStop = false;
			// 
			// SShipImageBox
			// 
			SShipImageBox.BorderStyle = BorderStyle.FixedSingle;
			SShipImageBox.Location = new Point(2, 2);
			SShipImageBox.Name = "SShipImageBox";
			SShipImageBox.Size = new Size(50, 50);
			SShipImageBox.SizeMode = PictureBoxSizeMode.StretchImage;
			SShipImageBox.TabIndex = 1;
			SShipImageBox.TabStop = false;
			// 
			// Label1
			// 
			Label1.AutoSize = true;
			Label1.Font = new Font("Consolas", 9.75f, FontStyle.Regular, GraphicsUnit.Point, Conversions.ToByte(0));
			Label1.ForeColor = Color.White;
			Label1.Location = new Point(97, 37);
			Label1.Name = "Label1";
			Label1.Size = new Size(63, 15);
			Label1.TabIndex = 0;
			Label1.Text = "Upgrades" + '\r' + '\n';
			// 
			// SShipTypeBox
			// 
			SShipTypeBox.Font = new Font("Consolas", 9.75f, FontStyle.Regular, GraphicsUnit.Point, Conversions.ToByte(0));
			SShipTypeBox.Location = new Point(57, 3);
			SShipTypeBox.Name = "SShipTypeBox";
			SShipTypeBox.Size = new Size(126, 20);
			SShipTypeBox.TabIndex = 0;
			SShipTypeBox.Text = "Type";
			// 
			// PanelRes
			// 
			PanelRes.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			PanelRes.BorderStyle = BorderStyle.FixedSingle;
			PanelRes.Controls.Add(WarCriminalLabel);
			PanelRes.Controls.Add(AntimatterTextBox);
			PanelRes.Controls.Add(UraniumTextBox);
			PanelRes.Controls.Add(CristalTextBox);
			PanelRes.Controls.Add(MetalTextBox);
			PanelRes.Controls.Add(_PictureBox5);
			PanelRes.Controls.Add(_PictureBox6);
			PanelRes.Controls.Add(_PictureBox2);
			PanelRes.Controls.Add(PictureBox1);
			PanelRes.Location = new Point(600, 0);
			PanelRes.Name = "PanelRes";
			PanelRes.Size = new Size(200, 68);
			PanelRes.TabIndex = 2;
			// 
			// WarCriminalLabel
			// 
			WarCriminalLabel.AutoSize = true;
			WarCriminalLabel.Font = new Font("Consolas", 6.75f, FontStyle.Regular, GraphicsUnit.Point, Conversions.ToByte(0));
			WarCriminalLabel.ForeColor = Color.Red;
			WarCriminalLabel.Location = new Point(34, 54);
			WarCriminalLabel.Name = "WarCriminalLabel";
			WarCriminalLabel.Size = new Size(120, 10);
			WarCriminalLabel.TabIndex = 4;
			WarCriminalLabel.Text = "W A R   C R I M I N A L";
			WarCriminalLabel.Visible = false;
			// 
			// AntimatterTextBox
			// 
			AntimatterTextBox.Font = new Font("Consolas", 9.75f, FontStyle.Regular, GraphicsUnit.Point, Conversions.ToByte(0));
			AntimatterTextBox.ForeColor = Color.Yellow;
			AntimatterTextBox.Location = new Point(108, 3);
			AntimatterTextBox.Name = "AntimatterTextBox";
			AntimatterTextBox.Size = new Size(56, 25);
			AntimatterTextBox.TabIndex = 3;
			AntimatterTextBox.Text = "0";
			AntimatterTextBox.TextAlign = ContentAlignment.MiddleRight;
			// 
			// UraniumTextBox
			// 
			UraniumTextBox.Font = new Font("Consolas", 9.75f, FontStyle.Regular, GraphicsUnit.Point, Conversions.ToByte(0));
			UraniumTextBox.ForeColor = Color.Lime;
			UraniumTextBox.Location = new Point(108, 34);
			UraniumTextBox.Name = "UraniumTextBox";
			UraniumTextBox.Size = new Size(56, 25);
			UraniumTextBox.TabIndex = 3;
			UraniumTextBox.Text = "0";
			UraniumTextBox.TextAlign = ContentAlignment.MiddleRight;
			// 
			// CristalTextBox
			// 
			CristalTextBox.Font = new Font("Consolas", 9.75f, FontStyle.Regular, GraphicsUnit.Point, Conversions.ToByte(0));
			CristalTextBox.ForeColor = Color.Violet;
			CristalTextBox.Location = new Point(36, 34);
			CristalTextBox.Name = "CristalTextBox";
			CristalTextBox.Size = new Size(56, 25);
			CristalTextBox.TabIndex = 3;
			CristalTextBox.Text = "0";
			CristalTextBox.TextAlign = ContentAlignment.MiddleLeft;
			// 
			// MetalTextBox
			// 
			MetalTextBox.Font = new Font("Consolas", 9.75f, FontStyle.Regular, GraphicsUnit.Point, Conversions.ToByte(0));
			MetalTextBox.ForeColor = Color.FromArgb(Conversions.ToInteger(Conversions.ToByte(255)), Conversions.ToInteger(Conversions.ToByte(128)), Conversions.ToInteger(Conversions.ToByte(0)));
			MetalTextBox.Location = new Point(36, 3);
			MetalTextBox.Name = "MetalTextBox";
			MetalTextBox.Size = new Size(71, 25);
			MetalTextBox.TabIndex = 3;
			MetalTextBox.Text = "0";
			MetalTextBox.TextAlign = ContentAlignment.MiddleLeft;
			// 
			// PictureBox5
			// 
			_PictureBox5.Image = My.Resources.Resources.Fissile;
			_PictureBox5.Location = new Point(170, 34);
			_PictureBox5.Name = "_PictureBox5";
			_PictureBox5.Size = new Size(25, 25);
			_PictureBox5.SizeMode = PictureBoxSizeMode.StretchImage;
			_PictureBox5.TabIndex = 0;
			_PictureBox5.TabStop = false;
			// 
			// PictureBox6
			// 
			_PictureBox6.Image = My.Resources.Resources.Antimatter;
			_PictureBox6.Location = new Point(170, 3);
			_PictureBox6.Name = "_PictureBox6";
			_PictureBox6.Size = new Size(25, 25);
			_PictureBox6.SizeMode = PictureBoxSizeMode.StretchImage;
			_PictureBox6.TabIndex = 0;
			_PictureBox6.TabStop = false;
			// 
			// PictureBox2
			// 
			_PictureBox2.Image = My.Resources.Resources.Crystal;
			_PictureBox2.Location = new Point(5, 34);
			_PictureBox2.Name = "_PictureBox2";
			_PictureBox2.Size = new Size(25, 25);
			_PictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
			_PictureBox2.TabIndex = 0;
			_PictureBox2.TabStop = false;
			// 
			// PictureBox1
			// 
			PictureBox1.Image = My.Resources.Resources.Metal;
			PictureBox1.Location = new Point(5, 3);
			PictureBox1.Name = "PictureBox1";
			PictureBox1.Size = new Size(25, 25);
			PictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
			PictureBox1.TabIndex = 0;
			PictureBox1.TabStop = false;
			// 
			// DrawBox
			// 
			_DrawBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
			_DrawBox.BorderStyle = BorderStyle.FixedSingle;
			_DrawBox.Location = new Point(0, 0);
			_DrawBox.Name = "_DrawBox";
			_DrawBox.Size = new Size(600, 600);
			_DrawBox.SizeMode = PictureBoxSizeMode.StretchImage;
			_DrawBox.TabIndex = 0;
			_DrawBox.TabStop = false;
			// 
			// Ticker
			// 
			_Ticker.Interval = 33;
			// 
			// MainForm
			// 
			AutoScaleMode = AutoScaleMode.None;
			BackColor = Color.Black;
			ClientSize = new Size(800, 600);
			Controls.Add(MainPanel);
			Cursor = Cursors.Cross;
			DoubleBuffered = true;
			Font = new Font("Consolas", 6.75f, FontStyle.Regular, GraphicsUnit.Point, Conversions.ToByte(0));
			ForeColor = Color.White;
			Icon = (Icon)resources.GetObject("$this.Icon");
			KeyPreview = true;
			Name = "MainForm";
			StartPosition = FormStartPosition.CenterScreen;
			Text = "Flee";
			MainPanel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)_MiniBox).EndInit();
			MenuPanel.ResumeLayout(false);
			MenuPanel.PerformLayout();
			UpgradeDetails.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)PriceCIcon).EndInit();
			((System.ComponentModel.ISupportInitialize)PriceAIcon).EndInit();
			((System.ComponentModel.ISupportInitialize)PriceUIcon).EndInit();
			((System.ComponentModel.ISupportInitialize)PriceMIcon).EndInit();
			SShipPanel.ResumeLayout(false);
			SShipPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)AllowMiningBox).EndInit();
			((System.ComponentModel.ISupportInitialize)_UpgradesBox).EndInit();
			((System.ComponentModel.ISupportInitialize)SShipImageBox).EndInit();
			PanelRes.ResumeLayout(false);
			PanelRes.PerformLayout();
			((System.ComponentModel.ISupportInitialize)_PictureBox5).EndInit();
			((System.ComponentModel.ISupportInitialize)_PictureBox6).EndInit();
			((System.ComponentModel.ISupportInitialize)_PictureBox2).EndInit();
			((System.ComponentModel.ISupportInitialize)PictureBox1).EndInit();
			((System.ComponentModel.ISupportInitialize)_DrawBox).EndInit();
			Load += new EventHandler(MainForm_Load);
			KeyDown += new KeyEventHandler(MainForm_KeyDown);
			KeyUp += new KeyEventHandler(MainForm_KeyUp);
			Resize += new EventHandler(MainForm_Resize);
			ResumeLayout(false);
		}

		internal Panel MainPanel;
		private PictureBox _DrawBox;

		internal PictureBox DrawBox {
			[MethodImpl(MethodImplOptions.Synchronized)]
			get {
				return _DrawBox;
			}

			[MethodImpl(MethodImplOptions.Synchronized)]
			set {
				if (_DrawBox != null) {
					_DrawBox.MouseDown -= DrawBox_MouseDown;
					_DrawBox.MouseMove -= DrawBox_MouseMove;
					_DrawBox.MouseUp -= DrawBox_MouseUp;
					_DrawBox.SizeChanged -= DrawBox_SizeChanged;
				}

				_DrawBox = value;
				if (_DrawBox != null) {
					_DrawBox.MouseDown += DrawBox_MouseDown;
					_DrawBox.MouseMove += DrawBox_MouseMove;
					_DrawBox.MouseUp += DrawBox_MouseUp;
					_DrawBox.SizeChanged += DrawBox_SizeChanged;
				}
			}
		}

		private PictureBox _MiniBox;

		internal PictureBox MiniBox {
			[MethodImpl(MethodImplOptions.Synchronized)]
			get {
				return _MiniBox;
			}

			[MethodImpl(MethodImplOptions.Synchronized)]
			set {
				if (_MiniBox != null) {
					_MiniBox.MouseDown -= MiniBox_MouseDown;
					_MiniBox.MouseUp -= MiniBox_MouseUp;
					_MiniBox.MouseMove -= MiniBox_MouseMove;
				}

				_MiniBox = value;
				if (_MiniBox != null) {
					_MiniBox.MouseDown += MiniBox_MouseDown;
					_MiniBox.MouseUp += MiniBox_MouseUp;
					_MiniBox.MouseMove += MiniBox_MouseMove;
				}
			}
		}

		private Timer _Ticker;

		internal Timer Ticker {
			[MethodImpl(MethodImplOptions.Synchronized)]
			get {
				return _Ticker;
			}

			[MethodImpl(MethodImplOptions.Synchronized)]
			set {
				if (_Ticker != null) _Ticker.Tick -= Ticker_Tick;

				_Ticker = value;
				if (_Ticker != null) _Ticker.Tick += Ticker_Tick;
			}
		}

		internal Panel PanelRes;
		internal PictureBox PictureBox1;
		internal Label MetalTextBox;
		internal Panel SShipPanel;
		internal PictureBox SShipImageBox;
		internal Label SShipTypeBox;
		internal Label CristalTextBox;
		private PictureBox _PictureBox2;

		internal PictureBox PictureBox2 {
			[MethodImpl(MethodImplOptions.Synchronized)]
			get {
				return _PictureBox2;
			}

			[MethodImpl(MethodImplOptions.Synchronized)]
			set {
				if (_PictureBox2 != null) _PictureBox2.Click -= PictureBox2_Click;

				_PictureBox2 = value;
				if (_PictureBox2 != null) _PictureBox2.Click += PictureBox2_Click;
			}
		}

		private PictureBox _UpgradesBox;

		internal PictureBox UpgradesBox {
			[MethodImpl(MethodImplOptions.Synchronized)]
			get {
				return _UpgradesBox;
			}

			[MethodImpl(MethodImplOptions.Synchronized)]
			set {
				if (_UpgradesBox != null) {
					_UpgradesBox.MouseMove -= UpgradesBox_MouseMove;
					_UpgradesBox.Click -= UpgradesBox_Click;
					_UpgradesBox.MouseLeave -= UpgradesBox_MouseLeave;
				}

				_UpgradesBox = value;
				if (_UpgradesBox != null) {
					_UpgradesBox.MouseMove += UpgradesBox_MouseMove;
					_UpgradesBox.Click += UpgradesBox_Click;
					_UpgradesBox.MouseLeave += UpgradesBox_MouseLeave;
				}
			}
		}

		internal Label SShipUpsMax;
		internal Label Label1;
		internal Panel UpgradeDetails;
		internal Label UpDesc;
		internal Label UpName;
		internal Label PriceC;
		internal Label PriceM;
		internal PictureBox PriceCIcon;
		internal PictureBox PriceMIcon;
		internal PictureBox AllowMiningBox;
		internal Label UraniumTextBox;
		private PictureBox _PictureBox5;

		internal PictureBox PictureBox5 {
			[MethodImpl(MethodImplOptions.Synchronized)]
			get {
				return _PictureBox5;
			}

			[MethodImpl(MethodImplOptions.Synchronized)]
			set {
				if (_PictureBox5 != null) _PictureBox5.Click -= PictureBox2_Click;

				_PictureBox5 = value;
				if (_PictureBox5 != null) _PictureBox5.Click += PictureBox2_Click;
			}
		}

		internal Label PriceU;
		internal PictureBox PriceUIcon;
		internal Label WarCriminalLabel;
		internal Panel MenuPanel;
		private Button _StartPlayingButton;

		internal Button StartPlayingButton {
			[MethodImpl(MethodImplOptions.Synchronized)]
			get {
				return _StartPlayingButton;
			}

			[MethodImpl(MethodImplOptions.Synchronized)]
			set {
				if (_StartPlayingButton != null) _StartPlayingButton.Click -= BeginButton_Click;

				_StartPlayingButton = value;
				if (_StartPlayingButton != null) _StartPlayingButton.Click += BeginButton_Click;
			}
		}

		internal TextBox SeedTextBox;
		internal Label Label2;
		internal Label Label3;
		private Button _RandomizeButton;

		internal Button RandomizeButton {
			[MethodImpl(MethodImplOptions.Synchronized)]
			get {
				return _RandomizeButton;
			}

			[MethodImpl(MethodImplOptions.Synchronized)]
			set {
				if (_RandomizeButton != null) _RandomizeButton.Click -= RandomizeButton_Click;

				_RandomizeButton = value;
				if (_RandomizeButton != null) _RandomizeButton.Click += RandomizeButton_Click;
			}
		}

		internal Label AntimatterTextBox;
		private PictureBox _PictureBox6;

		internal PictureBox PictureBox6 {
			[MethodImpl(MethodImplOptions.Synchronized)]
			get {
				return _PictureBox6;
			}

			[MethodImpl(MethodImplOptions.Synchronized)]
			set {
				if (_PictureBox6 != null) _PictureBox6.Click -= PictureBox2_Click;

				_PictureBox6 = value;
				if (_PictureBox6 != null) _PictureBox6.Click += PictureBox2_Click;
			}
		}

		internal Label PriceA;
		internal PictureBox PriceAIcon;
		internal Label Label4;
	}
}