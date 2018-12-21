namespace OpenBreed.Editor.UI.WinForms.Views
{
    partial class LevelSettingsView
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
            this.Tabs = new System.Windows.Forms.TabControl();
            this.tabPageGeneral = new System.Windows.Forms.TabPage();
            this.grpMonster1 = new System.Windows.Forms.GroupBox();
            this.lblMonster1Type = new System.Windows.Forms.Label();
            this.lblMonster1Speed = new System.Windows.Forms.Label();
            this.lblMonster1Health = new System.Windows.Forms.Label();
            this.grpMonster2 = new System.Windows.Forms.GroupBox();
            this.lblMonster2Type = new System.Windows.Forms.Label();
            this.lblMonster2Speed = new System.Windows.Forms.Label();
            this.lblMonster2Health = new System.Windows.Forms.Label();
            this.grpTimer = new System.Windows.Forms.GroupBox();
            this.grpExitCodes = new System.Windows.Forms.GroupBox();
            this.lblEXC4 = new System.Windows.Forms.Label();
            this.lblEXC3 = new System.Windows.Forms.Label();
            this.lblEXC2 = new System.Windows.Forms.Label();
            this.lblEXC1 = new System.Windows.Forms.Label();
            this.tabPageTexts = new System.Windows.Forms.TabPage();
            this.grpNOT3 = new System.Windows.Forms.GroupBox();
            this.grpNOT2 = new System.Windows.Forms.GroupBox();
            this.grpNOT1 = new System.Windows.Forms.GroupBox();
            this.grpLCTX = new System.Windows.Forms.GroupBox();
            this.grpMTXT = new System.Windows.Forms.GroupBox();
            this.tbxM1HE = new OpenBreed.Common.UI.WinForms.Controls.TextBoxEx();
            this.tbxM1SP = new OpenBreed.Common.UI.WinForms.Controls.TextBoxEx();
            this.tbxM1TY = new OpenBreed.Common.UI.WinForms.Controls.TextBoxEx();
            this.tbxM2TY = new OpenBreed.Common.UI.WinForms.Controls.TextBoxEx();
            this.tbxM2HE = new OpenBreed.Common.UI.WinForms.Controls.TextBoxEx();
            this.tbxM2SP = new OpenBreed.Common.UI.WinForms.Controls.TextBoxEx();
            this.tbxUNKN16 = new OpenBreed.Common.UI.WinForms.Controls.TextBoxEx();
            this.tbxTIME = new OpenBreed.Common.UI.WinForms.Controls.TextBoxEx();
            this.tbxUNKN22 = new OpenBreed.Common.UI.WinForms.Controls.TextBoxEx();
            this.tbxEXC4 = new OpenBreed.Common.UI.WinForms.Controls.TextBoxEx();
            this.tbxEXC3 = new OpenBreed.Common.UI.WinForms.Controls.TextBoxEx();
            this.tbxEXC2 = new OpenBreed.Common.UI.WinForms.Controls.TextBoxEx();
            this.tbxEXC1 = new OpenBreed.Common.UI.WinForms.Controls.TextBoxEx();
            this.tbxUNKN21 = new OpenBreed.Common.UI.WinForms.Controls.TextBoxEx();
            this.tbxUNKN17 = new OpenBreed.Common.UI.WinForms.Controls.TextBoxEx();
            this.tbxUNKN1 = new OpenBreed.Common.UI.WinForms.Controls.TextBoxEx();
            this.tbxUNKN2 = new OpenBreed.Common.UI.WinForms.Controls.TextBoxEx();
            this.tbxUNKN3 = new OpenBreed.Common.UI.WinForms.Controls.TextBoxEx();
            this.tbxUNKN8 = new OpenBreed.Common.UI.WinForms.Controls.TextBoxEx();
            this.tbxUNKN4 = new OpenBreed.Common.UI.WinForms.Controls.TextBoxEx();
            this.tbxUNKN7 = new OpenBreed.Common.UI.WinForms.Controls.TextBoxEx();
            this.tbxUNKN6 = new OpenBreed.Common.UI.WinForms.Controls.TextBoxEx();
            this.tbxNOT3 = new OpenBreed.Common.UI.WinForms.Controls.TextBoxEx();
            this.tbxNOT2 = new OpenBreed.Common.UI.WinForms.Controls.TextBoxEx();
            this.tbxNOT1 = new OpenBreed.Common.UI.WinForms.Controls.TextBoxEx();
            this.tbxLCTX = new OpenBreed.Common.UI.WinForms.Controls.TextBoxEx();
            this.tbxMTXT = new OpenBreed.Common.UI.WinForms.Controls.TextBoxEx();
            this.PropertiesCtrl = new OpenBreed.Editor.UI.WinForms.Controls.Levels.LevelSettingsCtrl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.Tabs.SuspendLayout();
            this.tabPageGeneral.SuspendLayout();
            this.grpMonster1.SuspendLayout();
            this.grpMonster2.SuspendLayout();
            this.grpTimer.SuspendLayout();
            this.grpExitCodes.SuspendLayout();
            this.tabPageTexts.SuspendLayout();
            this.grpNOT3.SuspendLayout();
            this.grpNOT2.SuspendLayout();
            this.grpNOT1.SuspendLayout();
            this.grpLCTX.SuspendLayout();
            this.grpMTXT.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Tabs
            // 
            this.Tabs.Controls.Add(this.tabPageGeneral);
            this.Tabs.Controls.Add(this.tabPageTexts);
            this.Tabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Tabs.Location = new System.Drawing.Point(0, 0);
            this.Tabs.Name = "Tabs";
            this.Tabs.SelectedIndex = 0;
            this.Tabs.Size = new System.Drawing.Size(244, 629);
            this.Tabs.TabIndex = 20;
            // 
            // tabPageGeneral
            // 
            this.tabPageGeneral.AutoScroll = true;
            this.tabPageGeneral.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageGeneral.Controls.Add(this.panel1);
            this.tabPageGeneral.Controls.Add(this.PropertiesCtrl);
            this.tabPageGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabPageGeneral.Name = "tabPageGeneral";
            this.tabPageGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGeneral.Size = new System.Drawing.Size(236, 603);
            this.tabPageGeneral.TabIndex = 0;
            this.tabPageGeneral.Text = "General";
            // 
            // grpMonster1
            // 
            this.grpMonster1.Controls.Add(this.lblMonster1Type);
            this.grpMonster1.Controls.Add(this.lblMonster1Speed);
            this.grpMonster1.Controls.Add(this.tbxM1HE);
            this.grpMonster1.Controls.Add(this.lblMonster1Health);
            this.grpMonster1.Controls.Add(this.tbxM1SP);
            this.grpMonster1.Controls.Add(this.tbxM1TY);
            this.grpMonster1.Location = new System.Drawing.Point(-2, 205);
            this.grpMonster1.Name = "grpMonster1";
            this.grpMonster1.Size = new System.Drawing.Size(171, 62);
            this.grpMonster1.TabIndex = 24;
            this.grpMonster1.TabStop = false;
            this.grpMonster1.Text = "Monster 1";
            // 
            // lblMonster1Type
            // 
            this.lblMonster1Type.AutoSize = true;
            this.lblMonster1Type.Location = new System.Drawing.Point(7, 20);
            this.lblMonster1Type.Name = "lblMonster1Type";
            this.lblMonster1Type.Size = new System.Drawing.Size(31, 13);
            this.lblMonster1Type.TabIndex = 25;
            this.lblMonster1Type.Text = "Type";
            // 
            // lblMonster1Speed
            // 
            this.lblMonster1Speed.AutoSize = true;
            this.lblMonster1Speed.Location = new System.Drawing.Point(126, 20);
            this.lblMonster1Speed.Name = "lblMonster1Speed";
            this.lblMonster1Speed.Size = new System.Drawing.Size(38, 13);
            this.lblMonster1Speed.TabIndex = 24;
            this.lblMonster1Speed.Text = "Speed";
            // 
            // lblMonster1Health
            // 
            this.lblMonster1Health.AutoSize = true;
            this.lblMonster1Health.Location = new System.Drawing.Point(66, 20);
            this.lblMonster1Health.Name = "lblMonster1Health";
            this.lblMonster1Health.Size = new System.Drawing.Size(38, 13);
            this.lblMonster1Health.TabIndex = 23;
            this.lblMonster1Health.Text = "Health";
            // 
            // grpMonster2
            // 
            this.grpMonster2.Controls.Add(this.lblMonster2Type);
            this.grpMonster2.Controls.Add(this.lblMonster2Speed);
            this.grpMonster2.Controls.Add(this.lblMonster2Health);
            this.grpMonster2.Controls.Add(this.tbxM2TY);
            this.grpMonster2.Controls.Add(this.tbxM2HE);
            this.grpMonster2.Controls.Add(this.tbxM2SP);
            this.grpMonster2.Location = new System.Drawing.Point(-2, 332);
            this.grpMonster2.Name = "grpMonster2";
            this.grpMonster2.Size = new System.Drawing.Size(171, 62);
            this.grpMonster2.TabIndex = 23;
            this.grpMonster2.TabStop = false;
            this.grpMonster2.Text = "Monster 2";
            // 
            // lblMonster2Type
            // 
            this.lblMonster2Type.AutoSize = true;
            this.lblMonster2Type.Location = new System.Drawing.Point(7, 20);
            this.lblMonster2Type.Name = "lblMonster2Type";
            this.lblMonster2Type.Size = new System.Drawing.Size(31, 13);
            this.lblMonster2Type.TabIndex = 22;
            this.lblMonster2Type.Text = "Type";
            // 
            // lblMonster2Speed
            // 
            this.lblMonster2Speed.AutoSize = true;
            this.lblMonster2Speed.Location = new System.Drawing.Point(126, 20);
            this.lblMonster2Speed.Name = "lblMonster2Speed";
            this.lblMonster2Speed.Size = new System.Drawing.Size(38, 13);
            this.lblMonster2Speed.TabIndex = 21;
            this.lblMonster2Speed.Text = "Speed";
            // 
            // lblMonster2Health
            // 
            this.lblMonster2Health.AutoSize = true;
            this.lblMonster2Health.Location = new System.Drawing.Point(66, 20);
            this.lblMonster2Health.Name = "lblMonster2Health";
            this.lblMonster2Health.Size = new System.Drawing.Size(38, 13);
            this.lblMonster2Health.TabIndex = 20;
            this.lblMonster2Health.Text = "Health";
            // 
            // grpTimer
            // 
            this.grpTimer.Controls.Add(this.tbxTIME);
            this.grpTimer.Location = new System.Drawing.Point(-2, 51);
            this.grpTimer.Name = "grpTimer";
            this.grpTimer.Size = new System.Drawing.Size(49, 48);
            this.grpTimer.TabIndex = 22;
            this.grpTimer.TabStop = false;
            this.grpTimer.Text = "Timer";
            // 
            // grpExitCodes
            // 
            this.grpExitCodes.Controls.Add(this.lblEXC4);
            this.grpExitCodes.Controls.Add(this.lblEXC3);
            this.grpExitCodes.Controls.Add(this.lblEXC2);
            this.grpExitCodes.Controls.Add(this.lblEXC1);
            this.grpExitCodes.Controls.Add(this.tbxEXC4);
            this.grpExitCodes.Controls.Add(this.tbxEXC3);
            this.grpExitCodes.Controls.Add(this.tbxEXC2);
            this.grpExitCodes.Controls.Add(this.tbxEXC1);
            this.grpExitCodes.Location = new System.Drawing.Point(-2, 105);
            this.grpExitCodes.Name = "grpExitCodes";
            this.grpExitCodes.Size = new System.Drawing.Size(171, 79);
            this.grpExitCodes.TabIndex = 18;
            this.grpExitCodes.TabStop = false;
            this.grpExitCodes.Text = "Exit Codes";
            // 
            // lblEXC4
            // 
            this.lblEXC4.AutoSize = true;
            this.lblEXC4.Location = new System.Drawing.Point(84, 48);
            this.lblEXC4.Name = "lblEXC4";
            this.lblEXC4.Size = new System.Drawing.Size(16, 13);
            this.lblEXC4.TabIndex = 15;
            this.lblEXC4.Text = "4:";
            // 
            // lblEXC3
            // 
            this.lblEXC3.AutoSize = true;
            this.lblEXC3.Location = new System.Drawing.Point(9, 48);
            this.lblEXC3.Name = "lblEXC3";
            this.lblEXC3.Size = new System.Drawing.Size(16, 13);
            this.lblEXC3.TabIndex = 14;
            this.lblEXC3.Text = "3:";
            // 
            // lblEXC2
            // 
            this.lblEXC2.AutoSize = true;
            this.lblEXC2.Location = new System.Drawing.Point(84, 22);
            this.lblEXC2.Name = "lblEXC2";
            this.lblEXC2.Size = new System.Drawing.Size(16, 13);
            this.lblEXC2.TabIndex = 13;
            this.lblEXC2.Text = "2:";
            // 
            // lblEXC1
            // 
            this.lblEXC1.AutoSize = true;
            this.lblEXC1.Location = new System.Drawing.Point(9, 22);
            this.lblEXC1.Name = "lblEXC1";
            this.lblEXC1.Size = new System.Drawing.Size(16, 13);
            this.lblEXC1.TabIndex = 12;
            this.lblEXC1.Text = "1:";
            // 
            // tabPageTexts
            // 
            this.tabPageTexts.AutoScroll = true;
            this.tabPageTexts.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageTexts.Controls.Add(this.grpNOT3);
            this.tabPageTexts.Controls.Add(this.grpNOT2);
            this.tabPageTexts.Controls.Add(this.grpNOT1);
            this.tabPageTexts.Controls.Add(this.grpLCTX);
            this.tabPageTexts.Controls.Add(this.grpMTXT);
            this.tabPageTexts.Location = new System.Drawing.Point(4, 22);
            this.tabPageTexts.Name = "tabPageTexts";
            this.tabPageTexts.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTexts.Size = new System.Drawing.Size(268, 431);
            this.tabPageTexts.TabIndex = 1;
            this.tabPageTexts.Text = "Texts";
            // 
            // grpNOT3
            // 
            this.grpNOT3.Controls.Add(this.tbxNOT3);
            this.grpNOT3.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpNOT3.Location = new System.Drawing.Point(3, 803);
            this.grpNOT3.Name = "grpNOT3";
            this.grpNOT3.Size = new System.Drawing.Size(245, 200);
            this.grpNOT3.TabIndex = 4;
            this.grpNOT3.TabStop = false;
            this.grpNOT3.Text = "Note Text 3";
            // 
            // grpNOT2
            // 
            this.grpNOT2.Controls.Add(this.tbxNOT2);
            this.grpNOT2.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpNOT2.Location = new System.Drawing.Point(3, 603);
            this.grpNOT2.Name = "grpNOT2";
            this.grpNOT2.Size = new System.Drawing.Size(245, 200);
            this.grpNOT2.TabIndex = 3;
            this.grpNOT2.TabStop = false;
            this.grpNOT2.Text = "Note Text 2";
            // 
            // grpNOT1
            // 
            this.grpNOT1.Controls.Add(this.tbxNOT1);
            this.grpNOT1.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpNOT1.Location = new System.Drawing.Point(3, 403);
            this.grpNOT1.Name = "grpNOT1";
            this.grpNOT1.Size = new System.Drawing.Size(245, 200);
            this.grpNOT1.TabIndex = 2;
            this.grpNOT1.TabStop = false;
            this.grpNOT1.Text = "Note Text 1";
            // 
            // grpLCTX
            // 
            this.grpLCTX.Controls.Add(this.tbxLCTX);
            this.grpLCTX.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpLCTX.Location = new System.Drawing.Point(3, 203);
            this.grpLCTX.Name = "grpLCTX";
            this.grpLCTX.Size = new System.Drawing.Size(245, 200);
            this.grpLCTX.TabIndex = 5;
            this.grpLCTX.TabStop = false;
            this.grpLCTX.Text = "Terminal/Corridor Text";
            // 
            // grpMTXT
            // 
            this.grpMTXT.Controls.Add(this.tbxMTXT);
            this.grpMTXT.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpMTXT.Location = new System.Drawing.Point(3, 3);
            this.grpMTXT.Name = "grpMTXT";
            this.grpMTXT.Size = new System.Drawing.Size(245, 200);
            this.grpMTXT.TabIndex = 1;
            this.grpMTXT.TabStop = false;
            this.grpMTXT.Text = "Mission Text";
            // 
            // tbxM1HE
            // 
            this.tbxM1HE.Location = new System.Drawing.Point(68, 36);
            this.tbxM1HE.Name = "tbxM1HE";
            this.tbxM1HE.Size = new System.Drawing.Size(34, 20);
            this.tbxM1HE.TabIndex = 13;
            // 
            // tbxM1SP
            // 
            this.tbxM1SP.Location = new System.Drawing.Point(129, 36);
            this.tbxM1SP.Name = "tbxM1SP";
            this.tbxM1SP.Size = new System.Drawing.Size(34, 20);
            this.tbxM1SP.TabIndex = 14;
            // 
            // tbxM1TY
            // 
            this.tbxM1TY.Location = new System.Drawing.Point(6, 36);
            this.tbxM1TY.Name = "tbxM1TY";
            this.tbxM1TY.Size = new System.Drawing.Size(34, 20);
            this.tbxM1TY.TabIndex = 12;
            // 
            // tbxM2TY
            // 
            this.tbxM2TY.Location = new System.Drawing.Point(6, 36);
            this.tbxM2TY.Name = "tbxM2TY";
            this.tbxM2TY.Size = new System.Drawing.Size(34, 20);
            this.tbxM2TY.TabIndex = 17;
            // 
            // tbxM2HE
            // 
            this.tbxM2HE.Location = new System.Drawing.Point(68, 36);
            this.tbxM2HE.Name = "tbxM2HE";
            this.tbxM2HE.Size = new System.Drawing.Size(34, 20);
            this.tbxM2HE.TabIndex = 18;
            // 
            // tbxM2SP
            // 
            this.tbxM2SP.Location = new System.Drawing.Point(129, 36);
            this.tbxM2SP.Name = "tbxM2SP";
            this.tbxM2SP.Size = new System.Drawing.Size(34, 20);
            this.tbxM2SP.TabIndex = 19;
            // 
            // tbxUNKN16
            // 
            this.tbxUNKN16.Location = new System.Drawing.Point(4, 280);
            this.tbxUNKN16.Name = "tbxUNKN16";
            this.tbxUNKN16.Size = new System.Drawing.Size(34, 20);
            this.tbxUNKN16.TabIndex = 15;
            // 
            // tbxTIME
            // 
            this.tbxTIME.Location = new System.Drawing.Point(6, 19);
            this.tbxTIME.Name = "tbxTIME";
            this.tbxTIME.Size = new System.Drawing.Size(34, 20);
            this.tbxTIME.TabIndex = 4;
            // 
            // tbxUNKN22
            // 
            this.tbxUNKN22.Location = new System.Drawing.Point(4, 430);
            this.tbxUNKN22.Name = "tbxUNKN22";
            this.tbxUNKN22.Size = new System.Drawing.Size(34, 20);
            this.tbxUNKN22.TabIndex = 21;
            // 
            // tbxEXC4
            // 
            this.tbxEXC4.Location = new System.Drawing.Point(106, 45);
            this.tbxEXC4.Name = "tbxEXC4";
            this.tbxEXC4.Size = new System.Drawing.Size(46, 20);
            this.tbxEXC4.TabIndex = 11;
            // 
            // tbxEXC3
            // 
            this.tbxEXC3.Location = new System.Drawing.Point(32, 45);
            this.tbxEXC3.Name = "tbxEXC3";
            this.tbxEXC3.Size = new System.Drawing.Size(46, 20);
            this.tbxEXC3.TabIndex = 10;
            // 
            // tbxEXC2
            // 
            this.tbxEXC2.Location = new System.Drawing.Point(106, 19);
            this.tbxEXC2.Name = "tbxEXC2";
            this.tbxEXC2.Size = new System.Drawing.Size(46, 20);
            this.tbxEXC2.TabIndex = 9;
            // 
            // tbxEXC1
            // 
            this.tbxEXC1.Location = new System.Drawing.Point(32, 19);
            this.tbxEXC1.Name = "tbxEXC1";
            this.tbxEXC1.Size = new System.Drawing.Size(46, 20);
            this.tbxEXC1.TabIndex = 8;
            // 
            // tbxUNKN21
            // 
            this.tbxUNKN21.Location = new System.Drawing.Point(4, 400);
            this.tbxUNKN21.Name = "tbxUNKN21";
            this.tbxUNKN21.Size = new System.Drawing.Size(34, 20);
            this.tbxUNKN21.TabIndex = 20;
            // 
            // tbxUNKN17
            // 
            this.tbxUNKN17.Location = new System.Drawing.Point(4, 306);
            this.tbxUNKN17.Name = "tbxUNKN17";
            this.tbxUNKN17.Size = new System.Drawing.Size(34, 20);
            this.tbxUNKN17.TabIndex = 16;
            // 
            // tbxUNKN1
            // 
            this.tbxUNKN1.Location = new System.Drawing.Point(5, 6);
            this.tbxUNKN1.Name = "tbxUNKN1";
            this.tbxUNKN1.Size = new System.Drawing.Size(34, 20);
            this.tbxUNKN1.TabIndex = 0;
            // 
            // tbxUNKN2
            // 
            this.tbxUNKN2.Location = new System.Drawing.Point(45, 6);
            this.tbxUNKN2.Name = "tbxUNKN2";
            this.tbxUNKN2.Size = new System.Drawing.Size(34, 20);
            this.tbxUNKN2.TabIndex = 1;
            // 
            // tbxUNKN3
            // 
            this.tbxUNKN3.Location = new System.Drawing.Point(85, 6);
            this.tbxUNKN3.Name = "tbxUNKN3";
            this.tbxUNKN3.Size = new System.Drawing.Size(34, 20);
            this.tbxUNKN3.TabIndex = 2;
            // 
            // tbxUNKN8
            // 
            this.tbxUNKN8.Location = new System.Drawing.Point(138, 70);
            this.tbxUNKN8.Name = "tbxUNKN8";
            this.tbxUNKN8.Size = new System.Drawing.Size(34, 20);
            this.tbxUNKN8.TabIndex = 7;
            // 
            // tbxUNKN4
            // 
            this.tbxUNKN4.Location = new System.Drawing.Point(125, 6);
            this.tbxUNKN4.Name = "tbxUNKN4";
            this.tbxUNKN4.Size = new System.Drawing.Size(34, 20);
            this.tbxUNKN4.TabIndex = 3;
            // 
            // tbxUNKN7
            // 
            this.tbxUNKN7.Location = new System.Drawing.Point(98, 70);
            this.tbxUNKN7.Name = "tbxUNKN7";
            this.tbxUNKN7.Size = new System.Drawing.Size(34, 20);
            this.tbxUNKN7.TabIndex = 6;
            // 
            // tbxUNKN6
            // 
            this.tbxUNKN6.Location = new System.Drawing.Point(58, 70);
            this.tbxUNKN6.Name = "tbxUNKN6";
            this.tbxUNKN6.Size = new System.Drawing.Size(34, 20);
            this.tbxUNKN6.TabIndex = 5;
            // 
            // tbxNOT3
            // 
            this.tbxNOT3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbxNOT3.Location = new System.Drawing.Point(3, 16);
            this.tbxNOT3.Multiline = true;
            this.tbxNOT3.Name = "tbxNOT3";
            this.tbxNOT3.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbxNOT3.Size = new System.Drawing.Size(239, 181);
            this.tbxNOT3.TabIndex = 0;
            // 
            // tbxNOT2
            // 
            this.tbxNOT2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbxNOT2.Location = new System.Drawing.Point(3, 16);
            this.tbxNOT2.Multiline = true;
            this.tbxNOT2.Name = "tbxNOT2";
            this.tbxNOT2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbxNOT2.Size = new System.Drawing.Size(239, 181);
            this.tbxNOT2.TabIndex = 0;
            // 
            // tbxNOT1
            // 
            this.tbxNOT1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbxNOT1.Location = new System.Drawing.Point(3, 16);
            this.tbxNOT1.Multiline = true;
            this.tbxNOT1.Name = "tbxNOT1";
            this.tbxNOT1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbxNOT1.Size = new System.Drawing.Size(239, 181);
            this.tbxNOT1.TabIndex = 0;
            // 
            // tbxLCTX
            // 
            this.tbxLCTX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbxLCTX.Location = new System.Drawing.Point(3, 16);
            this.tbxLCTX.Multiline = true;
            this.tbxLCTX.Name = "tbxLCTX";
            this.tbxLCTX.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbxLCTX.Size = new System.Drawing.Size(239, 181);
            this.tbxLCTX.TabIndex = 0;
            // 
            // tbxMTXT
            // 
            this.tbxMTXT.AcceptsReturn = true;
            this.tbxMTXT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbxMTXT.Location = new System.Drawing.Point(3, 16);
            this.tbxMTXT.Multiline = true;
            this.tbxMTXT.Name = "tbxMTXT";
            this.tbxMTXT.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbxMTXT.Size = new System.Drawing.Size(239, 181);
            this.tbxMTXT.TabIndex = 0;
            // 
            // PropertiesCtrl
            // 
            this.PropertiesCtrl.Dock = System.Windows.Forms.DockStyle.Top;
            this.PropertiesCtrl.Location = new System.Drawing.Point(3, 3);
            this.PropertiesCtrl.Name = "PropertiesCtrl";
            this.PropertiesCtrl.Size = new System.Drawing.Size(230, 171);
            this.PropertiesCtrl.TabIndex = 25;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tbxUNKN2);
            this.panel1.Controls.Add(this.tbxUNKN6);
            this.panel1.Controls.Add(this.grpMonster1);
            this.panel1.Controls.Add(this.tbxUNKN7);
            this.panel1.Controls.Add(this.grpMonster2);
            this.panel1.Controls.Add(this.tbxUNKN4);
            this.panel1.Controls.Add(this.tbxUNKN16);
            this.panel1.Controls.Add(this.tbxUNKN8);
            this.panel1.Controls.Add(this.grpTimer);
            this.panel1.Controls.Add(this.tbxUNKN3);
            this.panel1.Controls.Add(this.tbxUNKN22);
            this.panel1.Controls.Add(this.tbxUNKN1);
            this.panel1.Controls.Add(this.grpExitCodes);
            this.panel1.Controls.Add(this.tbxUNKN17);
            this.panel1.Controls.Add(this.tbxUNKN21);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 174);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(230, 426);
            this.panel1.TabIndex = 26;
            // 
            // MapPropertiesView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(244, 629);
            this.Controls.Add(this.Tabs);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.HideOnClose = true;
            this.Name = "MapPropertiesView";
            this.Text = "Properties";
            this.Tabs.ResumeLayout(false);
            this.tabPageGeneral.ResumeLayout(false);
            this.grpMonster1.ResumeLayout(false);
            this.grpMonster1.PerformLayout();
            this.grpMonster2.ResumeLayout(false);
            this.grpMonster2.PerformLayout();
            this.grpTimer.ResumeLayout(false);
            this.grpTimer.PerformLayout();
            this.grpExitCodes.ResumeLayout(false);
            this.grpExitCodes.PerformLayout();
            this.tabPageTexts.ResumeLayout(false);
            this.grpNOT3.ResumeLayout(false);
            this.grpNOT3.PerformLayout();
            this.grpNOT2.ResumeLayout(false);
            this.grpNOT2.PerformLayout();
            this.grpNOT1.ResumeLayout(false);
            this.grpNOT1.PerformLayout();
            this.grpLCTX.ResumeLayout(false);
            this.grpLCTX.PerformLayout();
            this.grpMTXT.ResumeLayout(false);
            this.grpMTXT.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl Tabs;
        private System.Windows.Forms.TabPage tabPageGeneral;
        private System.Windows.Forms.GroupBox grpMonster1;
        private System.Windows.Forms.Label lblMonster1Type;
        private System.Windows.Forms.Label lblMonster1Speed;
        private OpenBreed.Common.UI.WinForms.Controls.TextBoxEx tbxM1HE;
        private System.Windows.Forms.Label lblMonster1Health;
        private OpenBreed.Common.UI.WinForms.Controls.TextBoxEx tbxM1SP;
        private OpenBreed.Common.UI.WinForms.Controls.TextBoxEx tbxM1TY;
        private System.Windows.Forms.GroupBox grpMonster2;
        private System.Windows.Forms.Label lblMonster2Type;
        private System.Windows.Forms.Label lblMonster2Speed;
        private System.Windows.Forms.Label lblMonster2Health;
        private OpenBreed.Common.UI.WinForms.Controls.TextBoxEx tbxM2TY;
        private OpenBreed.Common.UI.WinForms.Controls.TextBoxEx tbxM2HE;
        private OpenBreed.Common.UI.WinForms.Controls.TextBoxEx tbxM2SP;
        private OpenBreed.Common.UI.WinForms.Controls.TextBoxEx tbxUNKN16;
        private System.Windows.Forms.GroupBox grpTimer;
        private OpenBreed.Common.UI.WinForms.Controls.TextBoxEx tbxTIME;
        private OpenBreed.Common.UI.WinForms.Controls.TextBoxEx tbxUNKN22;
        private System.Windows.Forms.GroupBox grpExitCodes;
        private System.Windows.Forms.Label lblEXC4;
        private System.Windows.Forms.Label lblEXC3;
        private System.Windows.Forms.Label lblEXC2;
        private System.Windows.Forms.Label lblEXC1;
        private OpenBreed.Common.UI.WinForms.Controls.TextBoxEx tbxEXC4;
        private OpenBreed.Common.UI.WinForms.Controls.TextBoxEx tbxEXC3;
        private OpenBreed.Common.UI.WinForms.Controls.TextBoxEx tbxEXC2;
        private OpenBreed.Common.UI.WinForms.Controls.TextBoxEx tbxEXC1;
        private OpenBreed.Common.UI.WinForms.Controls.TextBoxEx tbxUNKN21;
        private OpenBreed.Common.UI.WinForms.Controls.TextBoxEx tbxUNKN17;
        private OpenBreed.Common.UI.WinForms.Controls.TextBoxEx tbxUNKN1;
        private OpenBreed.Common.UI.WinForms.Controls.TextBoxEx tbxUNKN2;
        private OpenBreed.Common.UI.WinForms.Controls.TextBoxEx tbxUNKN3;
        private OpenBreed.Common.UI.WinForms.Controls.TextBoxEx tbxUNKN8;
        private OpenBreed.Common.UI.WinForms.Controls.TextBoxEx tbxUNKN4;
        private OpenBreed.Common.UI.WinForms.Controls.TextBoxEx tbxUNKN7;
        private OpenBreed.Common.UI.WinForms.Controls.TextBoxEx tbxUNKN6;
        private System.Windows.Forms.TabPage tabPageTexts;
        private System.Windows.Forms.GroupBox grpNOT3;
        private OpenBreed.Common.UI.WinForms.Controls.TextBoxEx tbxNOT3;
        private System.Windows.Forms.GroupBox grpNOT2;
        private OpenBreed.Common.UI.WinForms.Controls.TextBoxEx tbxNOT2;
        private System.Windows.Forms.GroupBox grpNOT1;
        private OpenBreed.Common.UI.WinForms.Controls.TextBoxEx tbxNOT1;
        private System.Windows.Forms.GroupBox grpLCTX;
        private OpenBreed.Common.UI.WinForms.Controls.TextBoxEx tbxLCTX;
        private System.Windows.Forms.GroupBox grpMTXT;
        private OpenBreed.Common.UI.WinForms.Controls.TextBoxEx tbxMTXT;
        private System.Windows.Forms.Panel panel1;
        private Controls.Levels.LevelSettingsCtrl PropertiesCtrl;
    }
}