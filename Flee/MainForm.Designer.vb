<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainForm
    Inherits System.Windows.Forms.Form

    'Form remplace la méthode Dispose pour nettoyer la liste des composants.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requise par le Concepteur Windows Form
    Private components As System.ComponentModel.IContainer

    'REMARQUE : la procédure suivante est requise par le Concepteur Windows Form
    'Elle peut être modifiée à l'aide du Concepteur Windows Form.  
    'Ne la modifiez pas à l'aide de l'éditeur de code.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
		Me.MainPanel = New System.Windows.Forms.Panel()
		Me.MiniBox = New System.Windows.Forms.PictureBox()
		Me.MenuPanel = New System.Windows.Forms.Panel()
		Me.Label4 = New System.Windows.Forms.Label()
		Me.RandomizeButton = New System.Windows.Forms.Button()
		Me.Label3 = New System.Windows.Forms.Label()
		Me.SeedTextBox = New System.Windows.Forms.TextBox()
		Me.Label2 = New System.Windows.Forms.Label()
		Me.StartPlayingButton = New System.Windows.Forms.Button()
		Me.UpgradeDetails = New System.Windows.Forms.Panel()
		Me.PriceA = New System.Windows.Forms.Label()
		Me.PriceU = New System.Windows.Forms.Label()
		Me.PriceC = New System.Windows.Forms.Label()
		Me.PriceM = New System.Windows.Forms.Label()
		Me.UpDesc = New System.Windows.Forms.Label()
		Me.UpName = New System.Windows.Forms.Label()
		Me.PriceCIcon = New System.Windows.Forms.PictureBox()
		Me.PriceAIcon = New System.Windows.Forms.PictureBox()
		Me.PriceUIcon = New System.Windows.Forms.PictureBox()
		Me.PriceMIcon = New System.Windows.Forms.PictureBox()
		Me.SShipPanel = New System.Windows.Forms.Panel()
		Me.AllowMiningBox = New System.Windows.Forms.PictureBox()
		Me.SShipUpsMax = New System.Windows.Forms.Label()
		Me.UpgradesBox = New System.Windows.Forms.PictureBox()
		Me.SShipImageBox = New System.Windows.Forms.PictureBox()
		Me.Label1 = New System.Windows.Forms.Label()
		Me.SShipTypeBox = New System.Windows.Forms.Label()
		Me.PanelRes = New System.Windows.Forms.Panel()
		Me.WarCriminalLabel = New System.Windows.Forms.Label()
		Me.AntimatterTextBox = New System.Windows.Forms.Label()
		Me.UraniumTextBox = New System.Windows.Forms.Label()
		Me.CristalTextBox = New System.Windows.Forms.Label()
		Me.MetalTextBox = New System.Windows.Forms.Label()
		Me.PictureBox5 = New System.Windows.Forms.PictureBox()
		Me.PictureBox6 = New System.Windows.Forms.PictureBox()
		Me.PictureBox2 = New System.Windows.Forms.PictureBox()
		Me.PictureBox1 = New System.Windows.Forms.PictureBox()
		Me.DrawBox = New System.Windows.Forms.PictureBox()
		Me.Ticker = New System.Windows.Forms.Timer(Me.components)
		Me.MainPanel.SuspendLayout()
		CType(Me.MiniBox, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.MenuPanel.SuspendLayout()
		Me.UpgradeDetails.SuspendLayout()
		CType(Me.PriceCIcon, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.PriceAIcon, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.PriceUIcon, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.PriceMIcon, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SShipPanel.SuspendLayout()
		CType(Me.AllowMiningBox, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.UpgradesBox, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.SShipImageBox, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.PanelRes.SuspendLayout()
		CType(Me.PictureBox5, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.PictureBox6, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.DrawBox, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.SuspendLayout()
		'
		'MainPanel
		'
		Me.MainPanel.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.MainPanel.Controls.Add(Me.MiniBox)
		Me.MainPanel.Controls.Add(Me.MenuPanel)
		Me.MainPanel.Controls.Add(Me.UpgradeDetails)
		Me.MainPanel.Controls.Add(Me.SShipPanel)
		Me.MainPanel.Controls.Add(Me.PanelRes)
		Me.MainPanel.Controls.Add(Me.DrawBox)
		Me.MainPanel.Location = New System.Drawing.Point(0, 0)
		Me.MainPanel.Name = "MainPanel"
		Me.MainPanel.Size = New System.Drawing.Size(800, 600)
		Me.MainPanel.TabIndex = 0
		'
		'MiniBox
		'
		Me.MiniBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.MiniBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.MiniBox.Location = New System.Drawing.Point(599, 399)
		Me.MiniBox.Name = "MiniBox"
		Me.MiniBox.Size = New System.Drawing.Size(202, 202)
		Me.MiniBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
		Me.MiniBox.TabIndex = 1
		Me.MiniBox.TabStop = False
		'
		'MenuPanel
		'
		Me.MenuPanel.Anchor = System.Windows.Forms.AnchorStyles.Left
		Me.MenuPanel.Controls.Add(Me.Label4)
		Me.MenuPanel.Controls.Add(Me.RandomizeButton)
		Me.MenuPanel.Controls.Add(Me.Label3)
		Me.MenuPanel.Controls.Add(Me.SeedTextBox)
		Me.MenuPanel.Controls.Add(Me.Label2)
		Me.MenuPanel.Controls.Add(Me.StartPlayingButton)
		Me.MenuPanel.Font = New System.Drawing.Font("Constantia", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.MenuPanel.Location = New System.Drawing.Point(155, 162)
		Me.MenuPanel.Name = "MenuPanel"
		Me.MenuPanel.Size = New System.Drawing.Size(312, 249)
		Me.MenuPanel.TabIndex = 5
		'
		'Label4
		'
		Me.Label4.Font = New System.Drawing.Font("Constantia", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label4.Location = New System.Drawing.Point(5, 144)
		Me.Label4.Name = "Label4"
		Me.Label4.Size = New System.Drawing.Size(302, 57)
		Me.Label4.TabIndex = 8
		Me.Label4.Text = "" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Some sprites are from MillionthVector." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "The music is from PhilippWeigl."
		'
		'RandomizeButton
		'
		Me.RandomizeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
		Me.RandomizeButton.Font = New System.Drawing.Font("Corbel", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.RandomizeButton.Location = New System.Drawing.Point(79, 89)
		Me.RandomizeButton.Name = "RandomizeButton"
		Me.RandomizeButton.Size = New System.Drawing.Size(230, 41)
		Me.RandomizeButton.TabIndex = 7
		Me.RandomizeButton.Text = "Randomize"
		Me.RandomizeButton.UseVisualStyleBackColor = True
		'
		'Label3
		'
		Me.Label3.AutoSize = True
		Me.Label3.Font = New System.Drawing.Font("Corbel", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label3.Location = New System.Drawing.Point(3, 0)
		Me.Label3.Name = "Label3"
		Me.Label3.Size = New System.Drawing.Size(87, 39)
		Me.Label3.TabIndex = 6
		Me.Label3.Text = "FLEE"
		'
		'SeedTextBox
		'
		Me.SeedTextBox.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
		Me.SeedTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.SeedTextBox.Font = New System.Drawing.Font("Corbel", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.SeedTextBox.ForeColor = System.Drawing.Color.White
		Me.SeedTextBox.Location = New System.Drawing.Point(79, 50)
		Me.SeedTextBox.Name = "SeedTextBox"
		Me.SeedTextBox.Size = New System.Drawing.Size(230, 33)
		Me.SeedTextBox.TabIndex = 6
		Me.SeedTextBox.Text = "0"
		Me.SeedTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
		'
		'Label2
		'
		Me.Label2.AutoSize = True
		Me.Label2.Font = New System.Drawing.Font("Corbel", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label2.Location = New System.Drawing.Point(3, 57)
		Me.Label2.Name = "Label2"
		Me.Label2.Size = New System.Drawing.Size(47, 19)
		Me.Label2.TabIndex = 1
		Me.Label2.Text = "SEED"
		'
		'StartPlayingButton
		'
		Me.StartPlayingButton.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.StartPlayingButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
		Me.StartPlayingButton.Font = New System.Drawing.Font("Corbel", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.StartPlayingButton.Location = New System.Drawing.Point(0, 211)
		Me.StartPlayingButton.Name = "StartPlayingButton"
		Me.StartPlayingButton.Size = New System.Drawing.Size(312, 38)
		Me.StartPlayingButton.TabIndex = 0
		Me.StartPlayingButton.Text = "Play"
		Me.StartPlayingButton.UseVisualStyleBackColor = True
		'
		'UpgradeDetails
		'
		Me.UpgradeDetails.Anchor = System.Windows.Forms.AnchorStyles.Top
		Me.UpgradeDetails.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
		Me.UpgradeDetails.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.UpgradeDetails.Controls.Add(Me.PriceA)
		Me.UpgradeDetails.Controls.Add(Me.PriceU)
		Me.UpgradeDetails.Controls.Add(Me.PriceC)
		Me.UpgradeDetails.Controls.Add(Me.PriceM)
		Me.UpgradeDetails.Controls.Add(Me.UpDesc)
		Me.UpgradeDetails.Controls.Add(Me.UpName)
		Me.UpgradeDetails.Controls.Add(Me.PriceCIcon)
		Me.UpgradeDetails.Controls.Add(Me.PriceAIcon)
		Me.UpgradeDetails.Controls.Add(Me.PriceUIcon)
		Me.UpgradeDetails.Controls.Add(Me.PriceMIcon)
		Me.UpgradeDetails.Location = New System.Drawing.Point(188, 12)
		Me.UpgradeDetails.Name = "UpgradeDetails"
		Me.UpgradeDetails.Size = New System.Drawing.Size(412, 94)
		Me.UpgradeDetails.TabIndex = 4
		Me.UpgradeDetails.Visible = False
		'
		'PriceA
		'
		Me.PriceA.Font = New System.Drawing.Font("Consolas", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.PriceA.ForeColor = System.Drawing.Color.Yellow
		Me.PriceA.Location = New System.Drawing.Point(236, 19)
		Me.PriceA.Name = "PriceA"
		Me.PriceA.Size = New System.Drawing.Size(40, 20)
		Me.PriceA.TabIndex = 1
		Me.PriceA.Text = "99999"
		Me.PriceA.TextAlign = System.Drawing.ContentAlignment.MiddleRight
		'
		'PriceU
		'
		Me.PriceU.Font = New System.Drawing.Font("Consolas", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.PriceU.ForeColor = System.Drawing.Color.Lime
		Me.PriceU.Location = New System.Drawing.Point(189, 19)
		Me.PriceU.Name = "PriceU"
		Me.PriceU.Size = New System.Drawing.Size(25, 20)
		Me.PriceU.TabIndex = 1
		Me.PriceU.Text = "99"
		Me.PriceU.TextAlign = System.Drawing.ContentAlignment.MiddleRight
		'
		'PriceC
		'
		Me.PriceC.Font = New System.Drawing.Font("Consolas", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.PriceC.ForeColor = System.Drawing.Color.Violet
		Me.PriceC.Location = New System.Drawing.Point(296, 19)
		Me.PriceC.Name = "PriceC"
		Me.PriceC.Size = New System.Drawing.Size(28, 20)
		Me.PriceC.TabIndex = 1
		Me.PriceC.Text = "999"
		Me.PriceC.TextAlign = System.Drawing.ContentAlignment.MiddleRight
		'
		'PriceM
		'
		Me.PriceM.Font = New System.Drawing.Font("Consolas", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.PriceM.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer))
		Me.PriceM.Location = New System.Drawing.Point(346, 19)
		Me.PriceM.Name = "PriceM"
		Me.PriceM.Size = New System.Drawing.Size(40, 20)
		Me.PriceM.TabIndex = 1
		Me.PriceM.Text = "99999"
		Me.PriceM.TextAlign = System.Drawing.ContentAlignment.MiddleRight
		'
		'UpDesc
		'
		Me.UpDesc.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.UpDesc.Font = New System.Drawing.Font("Consolas", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.UpDesc.Location = New System.Drawing.Point(0, 42)
		Me.UpDesc.Name = "UpDesc"
		Me.UpDesc.Size = New System.Drawing.Size(410, 50)
		Me.UpDesc.TabIndex = 0
		Me.UpDesc.Text = "Description"
		'
		'UpName
		'
		Me.UpName.Dock = System.Windows.Forms.DockStyle.Top
		Me.UpName.Font = New System.Drawing.Font("Consolas", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.UpName.ForeColor = System.Drawing.Color.White
		Me.UpName.Location = New System.Drawing.Point(0, 0)
		Me.UpName.Name = "UpName"
		Me.UpName.Size = New System.Drawing.Size(410, 18)
		Me.UpName.TabIndex = 0
		Me.UpName.Text = "Upgrade name."
		'
		'PriceCIcon
		'
		Me.PriceCIcon.Image = Global.Flee.My.Resources.Resources.Crystal
		Me.PriceCIcon.Location = New System.Drawing.Point(325, 19)
		Me.PriceCIcon.Name = "PriceCIcon"
		Me.PriceCIcon.Size = New System.Drawing.Size(20, 20)
		Me.PriceCIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
		Me.PriceCIcon.TabIndex = 0
		Me.PriceCIcon.TabStop = False
		'
		'PriceAIcon
		'
		Me.PriceAIcon.Image = Global.Flee.My.Resources.Resources.Antimatter
		Me.PriceAIcon.Location = New System.Drawing.Point(277, 19)
		Me.PriceAIcon.Name = "PriceAIcon"
		Me.PriceAIcon.Size = New System.Drawing.Size(20, 20)
		Me.PriceAIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
		Me.PriceAIcon.TabIndex = 0
		Me.PriceAIcon.TabStop = False
		'
		'PriceUIcon
		'
		Me.PriceUIcon.Image = Global.Flee.My.Resources.Resources.Fissile
		Me.PriceUIcon.Location = New System.Drawing.Point(215, 19)
		Me.PriceUIcon.Name = "PriceUIcon"
		Me.PriceUIcon.Size = New System.Drawing.Size(20, 20)
		Me.PriceUIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
		Me.PriceUIcon.TabIndex = 0
		Me.PriceUIcon.TabStop = False
		'
		'PriceMIcon
		'
		Me.PriceMIcon.Image = Global.Flee.My.Resources.Resources.Metal
		Me.PriceMIcon.Location = New System.Drawing.Point(387, 19)
		Me.PriceMIcon.Name = "PriceMIcon"
		Me.PriceMIcon.Size = New System.Drawing.Size(20, 20)
		Me.PriceMIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
		Me.PriceMIcon.TabIndex = 0
		Me.PriceMIcon.TabStop = False
		'
		'SShipPanel
		'
		Me.SShipPanel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.SShipPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.SShipPanel.Controls.Add(Me.AllowMiningBox)
		Me.SShipPanel.Controls.Add(Me.SShipUpsMax)
		Me.SShipPanel.Controls.Add(Me.UpgradesBox)
		Me.SShipPanel.Controls.Add(Me.SShipImageBox)
		Me.SShipPanel.Controls.Add(Me.Label1)
		Me.SShipPanel.Controls.Add(Me.SShipTypeBox)
		Me.SShipPanel.Location = New System.Drawing.Point(600, 67)
		Me.SShipPanel.Name = "SShipPanel"
		Me.SShipPanel.Size = New System.Drawing.Size(200, 332)
		Me.SShipPanel.TabIndex = 3
		'
		'AllowMiningBox
		'
		Me.AllowMiningBox.Image = Global.Flee.My.Resources.Resources.DeadSkull
		Me.AllowMiningBox.Location = New System.Drawing.Point(55, 39)
		Me.AllowMiningBox.Name = "AllowMiningBox"
		Me.AllowMiningBox.Size = New System.Drawing.Size(16, 16)
		Me.AllowMiningBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
		Me.AllowMiningBox.TabIndex = 4
		Me.AllowMiningBox.TabStop = False
		'
		'SShipUpsMax
		'
		Me.SShipUpsMax.Font = New System.Drawing.Font("Consolas", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.SShipUpsMax.ForeColor = System.Drawing.Color.White
		Me.SShipUpsMax.Location = New System.Drawing.Point(59, 23)
		Me.SShipUpsMax.Name = "SShipUpsMax"
		Me.SShipUpsMax.Size = New System.Drawing.Size(50, 16)
		Me.SShipUpsMax.TabIndex = 3
		Me.SShipUpsMax.Text = "00 / XX"
		Me.SShipUpsMax.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		'
		'UpgradesBox
		'
		Me.UpgradesBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.UpgradesBox.Location = New System.Drawing.Point(0, 55)
		Me.UpgradesBox.Name = "UpgradesBox"
		Me.UpgradesBox.Size = New System.Drawing.Size(200, 272)
		Me.UpgradesBox.TabIndex = 2
		Me.UpgradesBox.TabStop = False
		'
		'SShipImageBox
		'
		Me.SShipImageBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.SShipImageBox.Location = New System.Drawing.Point(2, 2)
		Me.SShipImageBox.Name = "SShipImageBox"
		Me.SShipImageBox.Size = New System.Drawing.Size(50, 50)
		Me.SShipImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
		Me.SShipImageBox.TabIndex = 1
		Me.SShipImageBox.TabStop = False
		'
		'Label1
		'
		Me.Label1.AutoSize = True
		Me.Label1.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label1.ForeColor = System.Drawing.Color.White
		Me.Label1.Location = New System.Drawing.Point(97, 37)
		Me.Label1.Name = "Label1"
		Me.Label1.Size = New System.Drawing.Size(63, 15)
		Me.Label1.TabIndex = 0
		Me.Label1.Text = "Upgrades" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
		'
		'SShipTypeBox
		'
		Me.SShipTypeBox.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.SShipTypeBox.Location = New System.Drawing.Point(57, 3)
		Me.SShipTypeBox.Name = "SShipTypeBox"
		Me.SShipTypeBox.Size = New System.Drawing.Size(126, 20)
		Me.SShipTypeBox.TabIndex = 0
		Me.SShipTypeBox.Text = "Type"
		'
		'PanelRes
		'
		Me.PanelRes.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.PanelRes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.PanelRes.Controls.Add(Me.WarCriminalLabel)
		Me.PanelRes.Controls.Add(Me.AntimatterTextBox)
		Me.PanelRes.Controls.Add(Me.UraniumTextBox)
		Me.PanelRes.Controls.Add(Me.CristalTextBox)
		Me.PanelRes.Controls.Add(Me.MetalTextBox)
		Me.PanelRes.Controls.Add(Me.PictureBox5)
		Me.PanelRes.Controls.Add(Me.PictureBox6)
		Me.PanelRes.Controls.Add(Me.PictureBox2)
		Me.PanelRes.Controls.Add(Me.PictureBox1)
		Me.PanelRes.Location = New System.Drawing.Point(600, 0)
		Me.PanelRes.Name = "PanelRes"
		Me.PanelRes.Size = New System.Drawing.Size(200, 68)
		Me.PanelRes.TabIndex = 2
		'
		'WarCriminalLabel
		'
		Me.WarCriminalLabel.AutoSize = True
		Me.WarCriminalLabel.Font = New System.Drawing.Font("Consolas", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.WarCriminalLabel.ForeColor = System.Drawing.Color.Red
		Me.WarCriminalLabel.Location = New System.Drawing.Point(34, 54)
		Me.WarCriminalLabel.Name = "WarCriminalLabel"
		Me.WarCriminalLabel.Size = New System.Drawing.Size(120, 10)
		Me.WarCriminalLabel.TabIndex = 4
		Me.WarCriminalLabel.Text = "W A R   C R I M I N A L"
		Me.WarCriminalLabel.Visible = False
		'
		'AntimatterTextBox
		'
		Me.AntimatterTextBox.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.AntimatterTextBox.ForeColor = System.Drawing.Color.Yellow
		Me.AntimatterTextBox.Location = New System.Drawing.Point(108, 3)
		Me.AntimatterTextBox.Name = "AntimatterTextBox"
		Me.AntimatterTextBox.Size = New System.Drawing.Size(56, 25)
		Me.AntimatterTextBox.TabIndex = 3
		Me.AntimatterTextBox.Text = "0"
		Me.AntimatterTextBox.TextAlign = System.Drawing.ContentAlignment.MiddleRight
		'
		'UraniumTextBox
		'
		Me.UraniumTextBox.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.UraniumTextBox.ForeColor = System.Drawing.Color.Lime
		Me.UraniumTextBox.Location = New System.Drawing.Point(108, 34)
		Me.UraniumTextBox.Name = "UraniumTextBox"
		Me.UraniumTextBox.Size = New System.Drawing.Size(56, 25)
		Me.UraniumTextBox.TabIndex = 3
		Me.UraniumTextBox.Text = "0"
		Me.UraniumTextBox.TextAlign = System.Drawing.ContentAlignment.MiddleRight
		'
		'CristalTextBox
		'
		Me.CristalTextBox.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.CristalTextBox.ForeColor = System.Drawing.Color.Violet
		Me.CristalTextBox.Location = New System.Drawing.Point(36, 34)
		Me.CristalTextBox.Name = "CristalTextBox"
		Me.CristalTextBox.Size = New System.Drawing.Size(56, 25)
		Me.CristalTextBox.TabIndex = 3
		Me.CristalTextBox.Text = "0"
		Me.CristalTextBox.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		'
		'MetalTextBox
		'
		Me.MetalTextBox.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.MetalTextBox.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer))
		Me.MetalTextBox.Location = New System.Drawing.Point(36, 3)
		Me.MetalTextBox.Name = "MetalTextBox"
		Me.MetalTextBox.Size = New System.Drawing.Size(71, 25)
		Me.MetalTextBox.TabIndex = 3
		Me.MetalTextBox.Text = "0"
		Me.MetalTextBox.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		'
		'PictureBox5
		'
		Me.PictureBox5.Image = Global.Flee.My.Resources.Resources.Fissile
		Me.PictureBox5.Location = New System.Drawing.Point(170, 34)
		Me.PictureBox5.Name = "PictureBox5"
		Me.PictureBox5.Size = New System.Drawing.Size(25, 25)
		Me.PictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
		Me.PictureBox5.TabIndex = 0
		Me.PictureBox5.TabStop = False
		'
		'PictureBox6
		'
		Me.PictureBox6.Image = Global.Flee.My.Resources.Resources.Antimatter
		Me.PictureBox6.Location = New System.Drawing.Point(170, 3)
		Me.PictureBox6.Name = "PictureBox6"
		Me.PictureBox6.Size = New System.Drawing.Size(25, 25)
		Me.PictureBox6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
		Me.PictureBox6.TabIndex = 0
		Me.PictureBox6.TabStop = False
		'
		'PictureBox2
		'
		Me.PictureBox2.Image = Global.Flee.My.Resources.Resources.Crystal
		Me.PictureBox2.Location = New System.Drawing.Point(5, 34)
		Me.PictureBox2.Name = "PictureBox2"
		Me.PictureBox2.Size = New System.Drawing.Size(25, 25)
		Me.PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
		Me.PictureBox2.TabIndex = 0
		Me.PictureBox2.TabStop = False
		'
		'PictureBox1
		'
		Me.PictureBox1.Image = Global.Flee.My.Resources.Resources.Metal
		Me.PictureBox1.Location = New System.Drawing.Point(5, 3)
		Me.PictureBox1.Name = "PictureBox1"
		Me.PictureBox1.Size = New System.Drawing.Size(25, 25)
		Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
		Me.PictureBox1.TabIndex = 0
		Me.PictureBox1.TabStop = False
		'
		'DrawBox
		'
		Me.DrawBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom), System.Windows.Forms.AnchorStyles)
		Me.DrawBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.DrawBox.Location = New System.Drawing.Point(0, 0)
		Me.DrawBox.Name = "DrawBox"
		Me.DrawBox.Size = New System.Drawing.Size(600, 600)
		Me.DrawBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
		Me.DrawBox.TabIndex = 0
		Me.DrawBox.TabStop = False
		'
		'Ticker
		'
		Me.Ticker.Interval = 33
		'
		'MainForm
		'
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
		Me.BackColor = System.Drawing.Color.Black
		Me.ClientSize = New System.Drawing.Size(800, 600)
		Me.Controls.Add(Me.MainPanel)
		Me.Cursor = System.Windows.Forms.Cursors.Cross
		Me.DoubleBuffered = True
		Me.Font = New System.Drawing.Font("Consolas", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.ForeColor = System.Drawing.Color.White
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.KeyPreview = True
		Me.Name = "MainForm"
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "Flee"
		Me.MainPanel.ResumeLayout(False)
		CType(Me.MiniBox, System.ComponentModel.ISupportInitialize).EndInit()
		Me.MenuPanel.ResumeLayout(False)
		Me.MenuPanel.PerformLayout()
		Me.UpgradeDetails.ResumeLayout(False)
		CType(Me.PriceCIcon, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.PriceAIcon, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.PriceUIcon, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.PriceMIcon, System.ComponentModel.ISupportInitialize).EndInit()
		Me.SShipPanel.ResumeLayout(False)
		Me.SShipPanel.PerformLayout()
		CType(Me.AllowMiningBox, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.UpgradesBox, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.SShipImageBox, System.ComponentModel.ISupportInitialize).EndInit()
		Me.PanelRes.ResumeLayout(False)
		Me.PanelRes.PerformLayout()
		CType(Me.PictureBox5, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.PictureBox6, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.DrawBox, System.ComponentModel.ISupportInitialize).EndInit()
		Me.ResumeLayout(False)

	End Sub
	Friend WithEvents MainPanel As System.Windows.Forms.Panel
	Friend WithEvents DrawBox As System.Windows.Forms.PictureBox
	Friend WithEvents MiniBox As System.Windows.Forms.PictureBox
	Friend WithEvents Ticker As System.Windows.Forms.Timer
	Friend WithEvents PanelRes As System.Windows.Forms.Panel
	Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
	Friend WithEvents MetalTextBox As System.Windows.Forms.Label
	Friend WithEvents SShipPanel As System.Windows.Forms.Panel
	Friend WithEvents SShipImageBox As System.Windows.Forms.PictureBox
	Friend WithEvents SShipTypeBox As System.Windows.Forms.Label
	Friend WithEvents CristalTextBox As System.Windows.Forms.Label
	Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
	Friend WithEvents UpgradesBox As System.Windows.Forms.PictureBox
	Friend WithEvents SShipUpsMax As System.Windows.Forms.Label
	Friend WithEvents Label1 As System.Windows.Forms.Label
	Friend WithEvents UpgradeDetails As System.Windows.Forms.Panel
	Friend WithEvents UpDesc As System.Windows.Forms.Label
	Friend WithEvents UpName As System.Windows.Forms.Label
	Friend WithEvents PriceC As System.Windows.Forms.Label
	Friend WithEvents PriceM As System.Windows.Forms.Label
	Friend WithEvents PriceCIcon As System.Windows.Forms.PictureBox
	Friend WithEvents PriceMIcon As System.Windows.Forms.PictureBox
	Friend WithEvents AllowMiningBox As PictureBox
	Friend WithEvents UraniumTextBox As Label
	Friend WithEvents PictureBox5 As PictureBox
	Friend WithEvents PriceU As Label
	Friend WithEvents PriceUIcon As PictureBox
	Friend WithEvents WarCriminalLabel As Label
	Friend WithEvents MenuPanel As Panel
	Friend WithEvents StartPlayingButton As Button
	Friend WithEvents SeedTextBox As TextBox
	Friend WithEvents Label2 As Label
	Friend WithEvents Label3 As Label
	Friend WithEvents RandomizeButton As Button
	Friend WithEvents AntimatterTextBox As Label
	Friend WithEvents PictureBox6 As PictureBox
	Friend WithEvents PriceA As Label
	Friend WithEvents PriceAIcon As PictureBox
	Friend WithEvents Label4 As Label
End Class
