using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Microsoft.VisualBasic.CompilerServices;

namespace Flee {
	[DesignerGenerated()]
	public partial class GameForm : Form {

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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameForm));
			this.MainPanel = new System.Windows.Forms.Panel();
			this.menuMain = new System.Windows.Forms.Panel();
			this.buttonSettings = new System.Windows.Forms.Button();
			this.buttonMultiplayer = new System.Windows.Forms.Button();
			this.buttonHost = new System.Windows.Forms.Button();
			this.Label3 = new System.Windows.Forms.Label();
			this.Label4 = new System.Windows.Forms.Label();
			this.menuHost = new System.Windows.Forms.Panel();
			this.SeedTextBox = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.checkBoxLAN = new System.Windows.Forms.CheckBox();
			this.checkBoxArmagedon = new System.Windows.Forms.CheckBox();
			this.label5 = new System.Windows.Forms.Label();
			this.Label2 = new System.Windows.Forms.Label();
			this._StartPlayingButton = new System.Windows.Forms.Button();
			this._RandomizeButton = new System.Windows.Forms.Button();
			this.menuSettings = new System.Windows.Forms.Panel();
			this.buttonSettingsOk = new System.Windows.Forms.Button();
			this.label8 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.checkBoxEnableBackground = new System.Windows.Forms.CheckBox();
			this._MiniBox = new System.Windows.Forms.PictureBox();
			this.UpgradeDetails = new System.Windows.Forms.Panel();
			this.PriceA = new System.Windows.Forms.Label();
			this.PriceSlots = new System.Windows.Forms.Label();
			this.PriceU = new System.Windows.Forms.Label();
			this.PriceC = new System.Windows.Forms.Label();
			this.PriceM = new System.Windows.Forms.Label();
			this.UpDesc = new System.Windows.Forms.Label();
			this.UpName = new System.Windows.Forms.Label();
			this.PriceCIcon = new System.Windows.Forms.PictureBox();
			this.PriceAIcon = new System.Windows.Forms.PictureBox();
			this.PriceSlotsIcon = new System.Windows.Forms.PictureBox();
			this.PriceUIcon = new System.Windows.Forms.PictureBox();
			this.PriceMIcon = new System.Windows.Forms.PictureBox();
			this.SShipPanel = new System.Windows.Forms.Panel();
			this.AllowMiningBox = new System.Windows.Forms.PictureBox();
			this.SShipUpsMax = new System.Windows.Forms.Label();
			this._UpgradesBox = new System.Windows.Forms.PictureBox();
			this.SShipImageBox = new System.Windows.Forms.PictureBox();
			this.Label1 = new System.Windows.Forms.Label();
			this.SShipTypeBox = new System.Windows.Forms.Label();
			this.PanelRes = new System.Windows.Forms.Panel();
			this.WarCriminalLabel = new System.Windows.Forms.Label();
			this.AntimatterTextBox = new System.Windows.Forms.Label();
			this.UraniumTextBox = new System.Windows.Forms.Label();
			this.CristalTextBox = new System.Windows.Forms.Label();
			this.MetalTextBox = new System.Windows.Forms.Label();
			this.PictureBoxAvailableFissile = new System.Windows.Forms.PictureBox();
			this.PictureBoxAvailableStarfuel = new System.Windows.Forms.PictureBox();
			this.PictureBoxAvailableCrystal = new System.Windows.Forms.PictureBox();
			this.PictureBoxAvailableMetal = new System.Windows.Forms.PictureBox();
			this._DrawBox = new System.Windows.Forms.PictureBox();
			this._Ticker = new System.Windows.Forms.Timer(this.components);
			this.trackBarDifficulty = new System.Windows.Forms.TrackBar();
			this.label9 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.MainPanel.SuspendLayout();
			this.menuMain.SuspendLayout();
			this.menuHost.SuspendLayout();
			this.menuSettings.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._MiniBox)).BeginInit();
			this.UpgradeDetails.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.PriceCIcon)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.PriceAIcon)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.PriceSlotsIcon)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.PriceUIcon)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.PriceMIcon)).BeginInit();
			this.SShipPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.AllowMiningBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._UpgradesBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.SShipImageBox)).BeginInit();
			this.PanelRes.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.PictureBoxAvailableFissile)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.PictureBoxAvailableStarfuel)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.PictureBoxAvailableCrystal)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.PictureBoxAvailableMetal)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._DrawBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.trackBarDifficulty)).BeginInit();
			this.SuspendLayout();
			// 
			// MainPanel
			// 
			this.MainPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.MainPanel.Controls.Add(this.menuHost);
			this.MainPanel.Controls.Add(this.menuMain);
			this.MainPanel.Controls.Add(this.menuSettings);
			this.MainPanel.Controls.Add(this._MiniBox);
			this.MainPanel.Controls.Add(this.UpgradeDetails);
			this.MainPanel.Controls.Add(this.SShipPanel);
			this.MainPanel.Controls.Add(this.PanelRes);
			this.MainPanel.Controls.Add(this._DrawBox);
			this.MainPanel.Location = new System.Drawing.Point(0, 0);
			this.MainPanel.Name = "MainPanel";
			this.MainPanel.Size = new System.Drawing.Size(800, 600);
			this.MainPanel.TabIndex = 0;
			// 
			// menuMain
			// 
			this.menuMain.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.menuMain.Controls.Add(this.buttonSettings);
			this.menuMain.Controls.Add(this.buttonMultiplayer);
			this.menuMain.Controls.Add(this.buttonHost);
			this.menuMain.Controls.Add(this.Label3);
			this.menuMain.Controls.Add(this.Label4);
			this.menuMain.Location = new System.Drawing.Point(196, 124);
			this.menuMain.Name = "menuMain";
			this.menuMain.Size = new System.Drawing.Size(403, 347);
			this.menuMain.TabIndex = 6;
			// 
			// buttonSettings
			// 
			this.buttonSettings.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.buttonSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonSettings.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.buttonSettings.Location = new System.Drawing.Point(87, 201);
			this.buttonSettings.Name = "buttonSettings";
			this.buttonSettings.Size = new System.Drawing.Size(230, 34);
			this.buttonSettings.TabIndex = 7;
			this.buttonSettings.Text = "Settings";
			this.buttonSettings.UseVisualStyleBackColor = true;
			this.buttonSettings.Click += new System.EventHandler(this.buttonSettings_Click);
			// 
			// buttonMultiplayer
			// 
			this.buttonMultiplayer.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.buttonMultiplayer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonMultiplayer.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.buttonMultiplayer.Location = new System.Drawing.Point(87, 161);
			this.buttonMultiplayer.Name = "buttonMultiplayer";
			this.buttonMultiplayer.Size = new System.Drawing.Size(230, 34);
			this.buttonMultiplayer.TabIndex = 7;
			this.buttonMultiplayer.Text = "Multiplayer";
			this.buttonMultiplayer.UseVisualStyleBackColor = true;
			this.buttonMultiplayer.Click += new System.EventHandler(this.buttonMultiplayer_Click);
			// 
			// buttonHost
			// 
			this.buttonHost.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.buttonHost.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonHost.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.buttonHost.Location = new System.Drawing.Point(87, 121);
			this.buttonHost.Name = "buttonHost";
			this.buttonHost.Size = new System.Drawing.Size(230, 34);
			this.buttonHost.TabIndex = 7;
			this.buttonHost.Text = "Singleplayer";
			this.buttonHost.UseVisualStyleBackColor = true;
			this.buttonHost.Click += new System.EventHandler(this.buttonHost_Click);
			// 
			// Label3
			// 
			this.Label3.AutoSize = true;
			this.Label3.Dock = System.Windows.Forms.DockStyle.Top;
			this.Label3.Font = new System.Drawing.Font("Corbel", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Label3.Location = new System.Drawing.Point(0, 0);
			this.Label3.Name = "Label3";
			this.Label3.Size = new System.Drawing.Size(87, 39);
			this.Label3.TabIndex = 6;
			this.Label3.Text = "FLEE";
			// 
			// Label4
			// 
			this.Label4.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.Label4.Font = new System.Drawing.Font("Constantia", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Label4.ForeColor = System.Drawing.Color.Silver;
			this.Label4.Location = new System.Drawing.Point(0, 306);
			this.Label4.Name = "Label4";
			this.Label4.Size = new System.Drawing.Size(403, 41);
			this.Label4.TabIndex = 8;
			this.Label4.Text = "Some sprites are from MillionthVector.\r\nThe music is from PhilippWeigl.";
			this.Label4.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// menuHost
			// 
			this.menuHost.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.menuHost.Controls.Add(this.trackBarDifficulty);
			this.menuHost.Controls.Add(this.label12);
			this.menuHost.Controls.Add(this.label11);
			this.menuHost.Controls.Add(this.label10);
			this.menuHost.Controls.Add(this.label9);
			this.menuHost.Controls.Add(this.SeedTextBox);
			this.menuHost.Controls.Add(this.label6);
			this.menuHost.Controls.Add(this.checkBoxLAN);
			this.menuHost.Controls.Add(this.checkBoxArmagedon);
			this.menuHost.Controls.Add(this.label5);
			this.menuHost.Controls.Add(this.Label2);
			this.menuHost.Controls.Add(this._StartPlayingButton);
			this.menuHost.Controls.Add(this._RandomizeButton);
			this.menuHost.Enabled = false;
			this.menuHost.Font = new System.Drawing.Font("Corbel", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.menuHost.Location = new System.Drawing.Point(345, 121);
			this.menuHost.Name = "menuHost";
			this.menuHost.Size = new System.Drawing.Size(385, 353);
			this.menuHost.TabIndex = 5;
			this.menuHost.Visible = false;
			// 
			// SeedTextBox
			// 
			this.SeedTextBox.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.SeedTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.SeedTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.SeedTextBox.Font = new System.Drawing.Font("Corbel", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.SeedTextBox.ForeColor = System.Drawing.Color.White;
			this.SeedTextBox.Location = new System.Drawing.Point(151, 85);
			this.SeedTextBox.Name = "SeedTextBox";
			this.SeedTextBox.Size = new System.Drawing.Size(230, 33);
			this.SeedTextBox.TabIndex = 6;
			this.SeedTextBox.Text = "876303952";
			this.SeedTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Dock = System.Windows.Forms.DockStyle.Top;
			this.label6.Font = new System.Drawing.Font("Corbel", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label6.Location = new System.Drawing.Point(0, 0);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(282, 39);
			this.label6.TabIndex = 6;
			this.label6.Text = "FLEE - Singleplayer";
			// 
			// checkBoxLAN
			// 
			this.checkBoxLAN.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.checkBoxLAN.AutoCheck = false;
			this.checkBoxLAN.AutoSize = true;
			this.checkBoxLAN.Font = new System.Drawing.Font("Constantia", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.checkBoxLAN.Location = new System.Drawing.Point(151, 169);
			this.checkBoxLAN.Name = "checkBoxLAN";
			this.checkBoxLAN.Size = new System.Drawing.Size(79, 17);
			this.checkBoxLAN.TabIndex = 9;
			this.checkBoxLAN.Text = "open to lan";
			this.checkBoxLAN.UseVisualStyleBackColor = true;
			// 
			// checkBoxArmagedon
			// 
			this.checkBoxArmagedon.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.checkBoxArmagedon.AutoSize = true;
			this.checkBoxArmagedon.Font = new System.Drawing.Font("Constantia", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.checkBoxArmagedon.Location = new System.Drawing.Point(151, 192);
			this.checkBoxArmagedon.Name = "checkBoxArmagedon";
			this.checkBoxArmagedon.Size = new System.Drawing.Size(111, 17);
			this.checkBoxArmagedon.TabIndex = 9;
			this.checkBoxArmagedon.Text = "Armagedon mode";
			this.checkBoxArmagedon.UseVisualStyleBackColor = true;
			// 
			// label5
			// 
			this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("Corbel", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.Location = new System.Drawing.Point(5, 165);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(63, 19);
			this.label5.TabIndex = 1;
			this.label5.Text = "Options";
			// 
			// Label2
			// 
			this.Label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.Label2.AutoSize = true;
			this.Label2.Font = new System.Drawing.Font("Corbel", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Label2.Location = new System.Drawing.Point(5, 85);
			this.Label2.Name = "Label2";
			this.Label2.Size = new System.Drawing.Size(47, 19);
			this.Label2.TabIndex = 1;
			this.Label2.Text = "SEED";
			// 
			// _StartPlayingButton
			// 
			this._StartPlayingButton.Dock = System.Windows.Forms.DockStyle.Bottom;
			this._StartPlayingButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this._StartPlayingButton.Font = new System.Drawing.Font("Corbel", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._StartPlayingButton.Location = new System.Drawing.Point(0, 315);
			this._StartPlayingButton.Name = "_StartPlayingButton";
			this._StartPlayingButton.Size = new System.Drawing.Size(385, 38);
			this._StartPlayingButton.TabIndex = 0;
			this._StartPlayingButton.Text = "Play";
			this._StartPlayingButton.UseVisualStyleBackColor = true;
			this._StartPlayingButton.Click += new System.EventHandler(this.BeginButton_Click);
			// 
			// _RandomizeButton
			// 
			this._RandomizeButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this._RandomizeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this._RandomizeButton.Font = new System.Drawing.Font("Corbel", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._RandomizeButton.Location = new System.Drawing.Point(156, 117);
			this._RandomizeButton.Name = "_RandomizeButton";
			this._RandomizeButton.Size = new System.Drawing.Size(221, 31);
			this._RandomizeButton.TabIndex = 7;
			this._RandomizeButton.Text = "Randomize";
			this._RandomizeButton.UseVisualStyleBackColor = true;
			this._RandomizeButton.Click += new System.EventHandler(this.RandomizeButton_Click);
			// 
			// menuSettings
			// 
			this.menuSettings.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.menuSettings.Controls.Add(this.buttonSettingsOk);
			this.menuSettings.Controls.Add(this.label8);
			this.menuSettings.Controls.Add(this.label7);
			this.menuSettings.Controls.Add(this.checkBoxEnableBackground);
			this.menuSettings.Enabled = false;
			this.menuSettings.Location = new System.Drawing.Point(23, 16);
			this.menuSettings.Name = "menuSettings";
			this.menuSettings.Size = new System.Drawing.Size(344, 203);
			this.menuSettings.TabIndex = 7;
			this.menuSettings.Visible = false;
			// 
			// buttonSettingsOk
			// 
			this.buttonSettingsOk.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.buttonSettingsOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonSettingsOk.Font = new System.Drawing.Font("Corbel", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.buttonSettingsOk.Location = new System.Drawing.Point(0, 165);
			this.buttonSettingsOk.Name = "buttonSettingsOk";
			this.buttonSettingsOk.Size = new System.Drawing.Size(344, 38);
			this.buttonSettingsOk.TabIndex = 10;
			this.buttonSettingsOk.Text = "OK";
			this.buttonSettingsOk.UseVisualStyleBackColor = true;
			this.buttonSettingsOk.Click += new System.EventHandler(this.buttonSettingsOk_Click);
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Dock = System.Windows.Forms.DockStyle.Top;
			this.label8.Font = new System.Drawing.Font("Corbel", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label8.Location = new System.Drawing.Point(0, 0);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(227, 39);
			this.label8.TabIndex = 7;
			this.label8.Text = "FLEE - Settings";
			// 
			// label7
			// 
			this.label7.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label7.AutoSize = true;
			this.label7.Font = new System.Drawing.Font("Corbel", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label7.Location = new System.Drawing.Point(3, 46);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(81, 19);
			this.label7.TabIndex = 1;
			this.label7.Text = "GRAPHICS";
			// 
			// checkBoxEnableBackground
			// 
			this.checkBoxEnableBackground.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.checkBoxEnableBackground.AutoSize = true;
			this.checkBoxEnableBackground.Checked = true;
			this.checkBoxEnableBackground.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxEnableBackground.Font = new System.Drawing.Font("Constantia", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.checkBoxEnableBackground.Location = new System.Drawing.Point(129, 50);
			this.checkBoxEnableBackground.Name = "checkBoxEnableBackground";
			this.checkBoxEnableBackground.Size = new System.Drawing.Size(82, 17);
			this.checkBoxEnableBackground.TabIndex = 9;
			this.checkBoxEnableBackground.Text = "background";
			this.checkBoxEnableBackground.UseVisualStyleBackColor = true;
			// 
			// _MiniBox
			// 
			this._MiniBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._MiniBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._MiniBox.Location = new System.Drawing.Point(598, 398);
			this._MiniBox.Name = "_MiniBox";
			this._MiniBox.Size = new System.Drawing.Size(202, 202);
			this._MiniBox.TabIndex = 1;
			this._MiniBox.TabStop = false;
			this._MiniBox.Paint += new System.Windows.Forms.PaintEventHandler(this._MiniBox_Paint);
			this._MiniBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MiniBox_MouseDown);
			this._MiniBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MiniBox_MouseMove);
			this._MiniBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MiniBox_MouseUp);
			// 
			// UpgradeDetails
			// 
			this.UpgradeDetails.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.UpgradeDetails.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.UpgradeDetails.Controls.Add(this.PriceA);
			this.UpgradeDetails.Controls.Add(this.PriceSlots);
			this.UpgradeDetails.Controls.Add(this.PriceU);
			this.UpgradeDetails.Controls.Add(this.PriceC);
			this.UpgradeDetails.Controls.Add(this.PriceM);
			this.UpgradeDetails.Controls.Add(this.UpDesc);
			this.UpgradeDetails.Controls.Add(this.UpName);
			this.UpgradeDetails.Controls.Add(this.PriceCIcon);
			this.UpgradeDetails.Controls.Add(this.PriceAIcon);
			this.UpgradeDetails.Controls.Add(this.PriceSlotsIcon);
			this.UpgradeDetails.Controls.Add(this.PriceUIcon);
			this.UpgradeDetails.Controls.Add(this.PriceMIcon);
			this.UpgradeDetails.Location = new System.Drawing.Point(152, 12);
			this.UpgradeDetails.Name = "UpgradeDetails";
			this.UpgradeDetails.Size = new System.Drawing.Size(412, 94);
			this.UpgradeDetails.TabIndex = 4;
			this.UpgradeDetails.Visible = false;
			// 
			// PriceA
			// 
			this.PriceA.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.PriceA.ForeColor = System.Drawing.Color.Yellow;
			this.PriceA.Location = new System.Drawing.Point(236, 19);
			this.PriceA.Name = "PriceA";
			this.PriceA.Size = new System.Drawing.Size(40, 20);
			this.PriceA.TabIndex = 1;
			this.PriceA.Text = "99999";
			this.PriceA.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// PriceSlots
			// 
			this.PriceSlots.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.PriceSlots.ForeColor = System.Drawing.Color.Gray;
			this.PriceSlots.Location = new System.Drawing.Point(26, 19);
			this.PriceSlots.Name = "PriceSlots";
			this.PriceSlots.Size = new System.Drawing.Size(25, 20);
			this.PriceSlots.TabIndex = 1;
			this.PriceSlots.Text = "1";
			this.PriceSlots.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// PriceU
			// 
			this.PriceU.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.PriceU.ForeColor = System.Drawing.Color.Lime;
			this.PriceU.Location = new System.Drawing.Point(189, 19);
			this.PriceU.Name = "PriceU";
			this.PriceU.Size = new System.Drawing.Size(25, 20);
			this.PriceU.TabIndex = 1;
			this.PriceU.Text = "99";
			this.PriceU.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// PriceC
			// 
			this.PriceC.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.PriceC.ForeColor = System.Drawing.Color.Violet;
			this.PriceC.Location = new System.Drawing.Point(296, 19);
			this.PriceC.Name = "PriceC";
			this.PriceC.Size = new System.Drawing.Size(28, 20);
			this.PriceC.TabIndex = 1;
			this.PriceC.Text = "999";
			this.PriceC.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// PriceM
			// 
			this.PriceM.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.PriceM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.PriceM.Location = new System.Drawing.Point(346, 19);
			this.PriceM.Name = "PriceM";
			this.PriceM.Size = new System.Drawing.Size(40, 20);
			this.PriceM.TabIndex = 1;
			this.PriceM.Text = "99999";
			this.PriceM.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// UpDesc
			// 
			this.UpDesc.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.UpDesc.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.UpDesc.Location = new System.Drawing.Point(0, 42);
			this.UpDesc.Name = "UpDesc";
			this.UpDesc.Size = new System.Drawing.Size(410, 50);
			this.UpDesc.TabIndex = 0;
			this.UpDesc.Text = "Description";
			// 
			// UpName
			// 
			this.UpName.Dock = System.Windows.Forms.DockStyle.Top;
			this.UpName.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.UpName.ForeColor = System.Drawing.Color.White;
			this.UpName.Location = new System.Drawing.Point(0, 0);
			this.UpName.Name = "UpName";
			this.UpName.Size = new System.Drawing.Size(410, 18);
			this.UpName.TabIndex = 0;
			this.UpName.Text = "Upgrade name.";
			// 
			// PriceCIcon
			// 
			this.PriceCIcon.Image = global::Flee.My.Resources.Resources.Crystal;
			this.PriceCIcon.Location = new System.Drawing.Point(325, 19);
			this.PriceCIcon.Name = "PriceCIcon";
			this.PriceCIcon.Size = new System.Drawing.Size(20, 20);
			this.PriceCIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.PriceCIcon.TabIndex = 0;
			this.PriceCIcon.TabStop = false;
			// 
			// PriceAIcon
			// 
			this.PriceAIcon.Image = global::Flee.My.Resources.Resources.Antimatter;
			this.PriceAIcon.Location = new System.Drawing.Point(277, 19);
			this.PriceAIcon.Name = "PriceAIcon";
			this.PriceAIcon.Size = new System.Drawing.Size(20, 20);
			this.PriceAIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.PriceAIcon.TabIndex = 0;
			this.PriceAIcon.TabStop = false;
			// 
			// PriceSlotsIcon
			// 
			this.PriceSlotsIcon.Image = global::Flee.My.Resources.Resources.Slots;
			this.PriceSlotsIcon.Location = new System.Drawing.Point(5, 19);
			this.PriceSlotsIcon.Name = "PriceSlotsIcon";
			this.PriceSlotsIcon.Size = new System.Drawing.Size(20, 20);
			this.PriceSlotsIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.PriceSlotsIcon.TabIndex = 0;
			this.PriceSlotsIcon.TabStop = false;
			// 
			// PriceUIcon
			// 
			this.PriceUIcon.Image = global::Flee.My.Resources.Resources.Fissile;
			this.PriceUIcon.Location = new System.Drawing.Point(215, 19);
			this.PriceUIcon.Name = "PriceUIcon";
			this.PriceUIcon.Size = new System.Drawing.Size(20, 20);
			this.PriceUIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.PriceUIcon.TabIndex = 0;
			this.PriceUIcon.TabStop = false;
			// 
			// PriceMIcon
			// 
			this.PriceMIcon.Image = global::Flee.My.Resources.Resources.Metal;
			this.PriceMIcon.Location = new System.Drawing.Point(387, 19);
			this.PriceMIcon.Name = "PriceMIcon";
			this.PriceMIcon.Size = new System.Drawing.Size(20, 20);
			this.PriceMIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.PriceMIcon.TabIndex = 0;
			this.PriceMIcon.TabStop = false;
			// 
			// SShipPanel
			// 
			this.SShipPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.SShipPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.SShipPanel.Controls.Add(this.AllowMiningBox);
			this.SShipPanel.Controls.Add(this.SShipUpsMax);
			this.SShipPanel.Controls.Add(this._UpgradesBox);
			this.SShipPanel.Controls.Add(this.SShipImageBox);
			this.SShipPanel.Controls.Add(this.Label1);
			this.SShipPanel.Controls.Add(this.SShipTypeBox);
			this.SShipPanel.Location = new System.Drawing.Point(0, 32);
			this.SShipPanel.Name = "SShipPanel";
			this.SShipPanel.Size = new System.Drawing.Size(152, 538);
			this.SShipPanel.TabIndex = 3;
			this.SShipPanel.Visible = false;
			// 
			// AllowMiningBox
			// 
			this.AllowMiningBox.Image = global::Flee.My.Resources.Resources.DeadSkull;
			this.AllowMiningBox.Location = new System.Drawing.Point(55, 39);
			this.AllowMiningBox.Name = "AllowMiningBox";
			this.AllowMiningBox.Size = new System.Drawing.Size(16, 16);
			this.AllowMiningBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.AllowMiningBox.TabIndex = 4;
			this.AllowMiningBox.TabStop = false;
			// 
			// SShipUpsMax
			// 
			this.SShipUpsMax.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.SShipUpsMax.ForeColor = System.Drawing.Color.White;
			this.SShipUpsMax.Location = new System.Drawing.Point(99, 32);
			this.SShipUpsMax.Name = "SShipUpsMax";
			this.SShipUpsMax.Size = new System.Drawing.Size(50, 16);
			this.SShipUpsMax.TabIndex = 3;
			this.SShipUpsMax.Text = "00 / XX";
			this.SShipUpsMax.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// _UpgradesBox
			// 
			this._UpgradesBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._UpgradesBox.Location = new System.Drawing.Point(0, 61);
			this._UpgradesBox.Name = "_UpgradesBox";
			this._UpgradesBox.Size = new System.Drawing.Size(150, 475);
			this._UpgradesBox.TabIndex = 2;
			this._UpgradesBox.TabStop = false;
			this._UpgradesBox.Click += new System.EventHandler(this.UpgradesBox_Click);
			this._UpgradesBox.Paint += new System.Windows.Forms.PaintEventHandler(this._UpgradesBox_Paint);
			this._UpgradesBox.MouseLeave += new System.EventHandler(this.UpgradesBox_MouseLeave);
			this._UpgradesBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.UpgradesBox_MouseMove);
			// 
			// SShipImageBox
			// 
			this.SShipImageBox.Location = new System.Drawing.Point(2, 2);
			this.SShipImageBox.Name = "SShipImageBox";
			this.SShipImageBox.Size = new System.Drawing.Size(48, 48);
			this.SShipImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.SShipImageBox.TabIndex = 1;
			this.SShipImageBox.TabStop = false;
			// 
			// Label1
			// 
			this.Label1.AutoSize = true;
			this.Label1.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Label1.ForeColor = System.Drawing.Color.White;
			this.Label1.Location = new System.Drawing.Point(83, 44);
			this.Label1.Name = "Label1";
			this.Label1.Size = new System.Drawing.Size(63, 15);
			this.Label1.TabIndex = 0;
			this.Label1.Text = "Upgrades\r\n";
			// 
			// SShipTypeBox
			// 
			this.SShipTypeBox.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.SShipTypeBox.Location = new System.Drawing.Point(55, 2);
			this.SShipTypeBox.Name = "SShipTypeBox";
			this.SShipTypeBox.Size = new System.Drawing.Size(91, 33);
			this.SShipTypeBox.TabIndex = 0;
			this.SShipTypeBox.Text = "Type";
			// 
			// PanelRes
			// 
			this.PanelRes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.PanelRes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.PanelRes.Controls.Add(this.WarCriminalLabel);
			this.PanelRes.Controls.Add(this.AntimatterTextBox);
			this.PanelRes.Controls.Add(this.UraniumTextBox);
			this.PanelRes.Controls.Add(this.CristalTextBox);
			this.PanelRes.Controls.Add(this.MetalTextBox);
			this.PanelRes.Controls.Add(this.PictureBoxAvailableFissile);
			this.PanelRes.Controls.Add(this.PictureBoxAvailableStarfuel);
			this.PanelRes.Controls.Add(this.PictureBoxAvailableCrystal);
			this.PanelRes.Controls.Add(this.PictureBoxAvailableMetal);
			this.PanelRes.Location = new System.Drawing.Point(600, 0);
			this.PanelRes.Name = "PanelRes";
			this.PanelRes.Size = new System.Drawing.Size(200, 68);
			this.PanelRes.TabIndex = 2;
			// 
			// WarCriminalLabel
			// 
			this.WarCriminalLabel.AutoSize = true;
			this.WarCriminalLabel.Font = new System.Drawing.Font("Consolas", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.WarCriminalLabel.ForeColor = System.Drawing.Color.Red;
			this.WarCriminalLabel.Location = new System.Drawing.Point(34, 54);
			this.WarCriminalLabel.Name = "WarCriminalLabel";
			this.WarCriminalLabel.Size = new System.Drawing.Size(120, 10);
			this.WarCriminalLabel.TabIndex = 4;
			this.WarCriminalLabel.Text = "W A R   C R I M I N A L";
			this.WarCriminalLabel.Visible = false;
			// 
			// AntimatterTextBox
			// 
			this.AntimatterTextBox.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.AntimatterTextBox.ForeColor = System.Drawing.Color.Yellow;
			this.AntimatterTextBox.Location = new System.Drawing.Point(100, 3);
			this.AntimatterTextBox.Name = "AntimatterTextBox";
			this.AntimatterTextBox.Size = new System.Drawing.Size(64, 25);
			this.AntimatterTextBox.TabIndex = 3;
			this.AntimatterTextBox.Text = "starfuel";
			this.AntimatterTextBox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// UraniumTextBox
			// 
			this.UraniumTextBox.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.UraniumTextBox.ForeColor = System.Drawing.Color.Lime;
			this.UraniumTextBox.Location = new System.Drawing.Point(100, 34);
			this.UraniumTextBox.Name = "UraniumTextBox";
			this.UraniumTextBox.Size = new System.Drawing.Size(64, 25);
			this.UraniumTextBox.TabIndex = 3;
			this.UraniumTextBox.Text = "fissile";
			this.UraniumTextBox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// CristalTextBox
			// 
			this.CristalTextBox.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.CristalTextBox.ForeColor = System.Drawing.Color.Violet;
			this.CristalTextBox.Location = new System.Drawing.Point(36, 34);
			this.CristalTextBox.Name = "CristalTextBox";
			this.CristalTextBox.Size = new System.Drawing.Size(56, 25);
			this.CristalTextBox.TabIndex = 3;
			this.CristalTextBox.Text = "crystal";
			this.CristalTextBox.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// MetalTextBox
			// 
			this.MetalTextBox.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.MetalTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.MetalTextBox.Location = new System.Drawing.Point(36, 3);
			this.MetalTextBox.Name = "MetalTextBox";
			this.MetalTextBox.Size = new System.Drawing.Size(71, 25);
			this.MetalTextBox.TabIndex = 3;
			this.MetalTextBox.Text = "metal";
			this.MetalTextBox.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// PictureBoxAvailableFissile
			// 
			this.PictureBoxAvailableFissile.Image = global::Flee.My.Resources.Resources.Fissile;
			this.PictureBoxAvailableFissile.Location = new System.Drawing.Point(170, 34);
			this.PictureBoxAvailableFissile.Name = "PictureBoxAvailableFissile";
			this.PictureBoxAvailableFissile.Size = new System.Drawing.Size(25, 25);
			this.PictureBoxAvailableFissile.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.PictureBoxAvailableFissile.TabIndex = 0;
			this.PictureBoxAvailableFissile.TabStop = false;
			// 
			// PictureBoxAvailableStarfuel
			// 
			this.PictureBoxAvailableStarfuel.Image = global::Flee.My.Resources.Resources.Antimatter;
			this.PictureBoxAvailableStarfuel.Location = new System.Drawing.Point(170, 3);
			this.PictureBoxAvailableStarfuel.Name = "PictureBoxAvailableStarfuel";
			this.PictureBoxAvailableStarfuel.Size = new System.Drawing.Size(25, 25);
			this.PictureBoxAvailableStarfuel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.PictureBoxAvailableStarfuel.TabIndex = 0;
			this.PictureBoxAvailableStarfuel.TabStop = false;
			this.PictureBoxAvailableStarfuel.Click += new System.EventHandler(this.PictureBoxAvailableStarfuel_Click);
			// 
			// PictureBoxAvailableCrystal
			// 
			this.PictureBoxAvailableCrystal.Image = global::Flee.My.Resources.Resources.Crystal;
			this.PictureBoxAvailableCrystal.Location = new System.Drawing.Point(5, 34);
			this.PictureBoxAvailableCrystal.Name = "PictureBoxAvailableCrystal";
			this.PictureBoxAvailableCrystal.Size = new System.Drawing.Size(25, 25);
			this.PictureBoxAvailableCrystal.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.PictureBoxAvailableCrystal.TabIndex = 0;
			this.PictureBoxAvailableCrystal.TabStop = false;
			// 
			// PictureBoxAvailableMetal
			// 
			this.PictureBoxAvailableMetal.Image = global::Flee.My.Resources.Resources.Metal;
			this.PictureBoxAvailableMetal.Location = new System.Drawing.Point(5, 3);
			this.PictureBoxAvailableMetal.Name = "PictureBoxAvailableMetal";
			this.PictureBoxAvailableMetal.Size = new System.Drawing.Size(25, 25);
			this.PictureBoxAvailableMetal.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.PictureBoxAvailableMetal.TabIndex = 0;
			this.PictureBoxAvailableMetal.TabStop = false;
			// 
			// _DrawBox
			// 
			this._DrawBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._DrawBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this._DrawBox.Location = new System.Drawing.Point(0, 0);
			this._DrawBox.Name = "_DrawBox";
			this._DrawBox.Size = new System.Drawing.Size(800, 600);
			this._DrawBox.TabIndex = 0;
			this._DrawBox.TabStop = false;
			this._DrawBox.Paint += new System.Windows.Forms.PaintEventHandler(this._DrawBox_Paint);
			this._DrawBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DrawBox_MouseDown);
			this._DrawBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DrawBox_MouseMove);
			this._DrawBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DrawBox_MouseUp);
			// 
			// _Ticker
			// 
			this._Ticker.Interval = 25;
			this._Ticker.Tick += new System.EventHandler(this.Ticker_Tick);
			// 
			// trackBarDifficulty
			// 
			this.trackBarDifficulty.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.trackBarDifficulty.BackColor = System.Drawing.Color.Black;
			this.trackBarDifficulty.LargeChange = 2;
			this.trackBarDifficulty.Location = new System.Drawing.Point(151, 247);
			this.trackBarDifficulty.Name = "trackBarDifficulty";
			this.trackBarDifficulty.Size = new System.Drawing.Size(226, 45);
			this.trackBarDifficulty.TabIndex = 10;
			this.trackBarDifficulty.TickStyle = System.Windows.Forms.TickStyle.Both;
			// 
			// label9
			// 
			this.label9.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.label9.AutoSize = true;
			this.label9.Font = new System.Drawing.Font("Corbel", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label9.Location = new System.Drawing.Point(149, 225);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(70, 19);
			this.label9.TabIndex = 11;
			this.label9.Text = "Difficulty";
			// 
			// label10
			// 
			this.label10.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.label10.AutoSize = true;
			this.label10.Font = new System.Drawing.Font("Corbel", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
			this.label10.Location = new System.Drawing.Point(154, 288);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(45, 15);
			this.label10.TabIndex = 11;
			this.label10.Text = "normal";
			// 
			// label11
			// 
			this.label11.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.label11.AutoSize = true;
			this.label11.Font = new System.Drawing.Font("Corbel", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
			this.label11.Location = new System.Drawing.Point(250, 288);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(31, 15);
			this.label11.TabIndex = 11;
			this.label11.Text = "hard";
			// 
			// label12
			// 
			this.label12.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.label12.AutoSize = true;
			this.label12.Font = new System.Drawing.Font("Corbel", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
			this.label12.Location = new System.Drawing.Point(317, 288);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(63, 15);
			this.label12.TabIndex = 11;
			this.label12.Text = "nightmare";
			// 
			// GameForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.Black;
			this.ClientSize = new System.Drawing.Size(800, 600);
			this.Controls.Add(this.MainPanel);
			this.Cursor = System.Windows.Forms.Cursors.Cross;
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("Consolas", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ForeColor = System.Drawing.Color.White;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.Name = "GameForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Flee";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyUp);
			this.MainPanel.ResumeLayout(false);
			this.menuMain.ResumeLayout(false);
			this.menuMain.PerformLayout();
			this.menuHost.ResumeLayout(false);
			this.menuHost.PerformLayout();
			this.menuSettings.ResumeLayout(false);
			this.menuSettings.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this._MiniBox)).EndInit();
			this.UpgradeDetails.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.PriceCIcon)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.PriceAIcon)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.PriceSlotsIcon)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.PriceUIcon)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.PriceMIcon)).EndInit();
			this.SShipPanel.ResumeLayout(false);
			this.SShipPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.AllowMiningBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._UpgradesBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.SShipImageBox)).EndInit();
			this.PanelRes.ResumeLayout(false);
			this.PanelRes.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.PictureBoxAvailableFissile)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.PictureBoxAvailableStarfuel)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.PictureBoxAvailableCrystal)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.PictureBoxAvailableMetal)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._DrawBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.trackBarDifficulty)).EndInit();
			this.ResumeLayout(false);

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
				}

				_DrawBox = value;
				if (_DrawBox != null) {
					_DrawBox.MouseDown += DrawBox_MouseDown;
					_DrawBox.MouseMove += DrawBox_MouseMove;
					_DrawBox.MouseUp += DrawBox_MouseUp;
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
		internal PictureBox PictureBoxAvailableMetal;
		internal Label MetalTextBox;
		internal Panel SShipPanel;
		internal PictureBox SShipImageBox;
		internal Label SShipTypeBox;
		internal Label CristalTextBox;
		private PictureBox PictureBoxAvailableCrystal;

		internal PictureBox PictureBox2 {
			[MethodImpl(MethodImplOptions.Synchronized)]
			get {
				return PictureBoxAvailableCrystal;
			}

			[MethodImpl(MethodImplOptions.Synchronized)]
			set {
				if (PictureBoxAvailableCrystal != null) PictureBoxAvailableCrystal.Click -= PictureBoxAvailableStarfuel_Click;

				PictureBoxAvailableCrystal = value;
				if (PictureBoxAvailableCrystal != null) PictureBoxAvailableCrystal.Click += PictureBoxAvailableStarfuel_Click;
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
		private PictureBox PictureBoxAvailableFissile;

		internal PictureBox PictureBox5 {
			[MethodImpl(MethodImplOptions.Synchronized)]
			get {
				return PictureBoxAvailableFissile;
			}

			[MethodImpl(MethodImplOptions.Synchronized)]
			set {
				if (PictureBoxAvailableFissile != null) PictureBoxAvailableFissile.Click -= PictureBoxAvailableStarfuel_Click;

				PictureBoxAvailableFissile = value;
				if (PictureBoxAvailableFissile != null) PictureBoxAvailableFissile.Click += PictureBoxAvailableStarfuel_Click;
			}
		}

		internal Label PriceU;
		internal PictureBox PriceUIcon;
		internal Label WarCriminalLabel;
		internal Panel menuHost;
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
		private PictureBox PictureBoxAvailableStarfuel;

		internal PictureBox PictureBox6 {
			[MethodImpl(MethodImplOptions.Synchronized)]
			get {
				return PictureBoxAvailableStarfuel;
			}

			[MethodImpl(MethodImplOptions.Synchronized)]
			set {
				if (PictureBoxAvailableStarfuel != null) PictureBoxAvailableStarfuel.Click -= PictureBoxAvailableStarfuel_Click;

				PictureBoxAvailableStarfuel = value;
				if (PictureBoxAvailableStarfuel != null) PictureBoxAvailableStarfuel.Click += PictureBoxAvailableStarfuel_Click;
			}
		}

		internal Label PriceA;
		internal PictureBox PriceAIcon;
		internal Label Label4;
		private CheckBox checkBoxEnableBackground;
		private CheckBox checkBoxLAN;
		internal Label label5;
		private CheckBox checkBoxArmagedon;
		internal Label label7;
		internal Label PriceSlots;
		internal PictureBox PriceSlotsIcon;
		private Panel menuMain;
		private Button buttonHost;
		private Panel menuSettings;
		private Button buttonSettingsOk;
		internal Label label8;
		internal Label label6;
		private Button buttonSettings;
		private Button buttonMultiplayer;
		private Label label9;
		private TrackBar trackBarDifficulty;
		private Label label11;
		private Label label10;
		private Label label12;
	}
}