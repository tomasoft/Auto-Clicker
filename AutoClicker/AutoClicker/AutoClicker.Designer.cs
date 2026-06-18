namespace AutoClicker
{
    partial class AutoClicker
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
            this.components = new System.ComponentModel.Container();
            this.minWait = new System.Windows.Forms.TextBox();
            this.maxWait = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.ctxMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuStart = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuStop = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuMode = new System.Windows.Forms.ToolStripMenuItem();
            this.slowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fastToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.keepPressedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clickAnywhereToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuQuit = new System.Windows.Forms.ToolStripMenuItem();
            this.chkInRobloxOnly = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnHide = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonToPress = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.chkAutoType = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.delayTime = new System.Windows.Forms.TextBox();
            this.pressedFor = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.chkKeepPressed = new System.Windows.Forms.CheckBox();
            this.ctxMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // minWait
            // 
            this.minWait.Location = new System.Drawing.Point(162, 37);
            this.minWait.Name = "minWait";
            this.minWait.Size = new System.Drawing.Size(100, 20);
            this.minWait.TabIndex = 3;
            // 
            // maxWait
            // 
            this.maxWait.Location = new System.Drawing.Point(162, 60);
            this.maxWait.Name = "maxWait";
            this.maxWait.Size = new System.Drawing.Size(100, 20);
            this.maxWait.TabIndex = 4;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(13, 37);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(71, 13);
            this.label8.TabIndex = 5;
            this.label8.Text = "Min Wait (ms)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Max Wait (ms)";
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.ctxMenuStrip;
            this.notifyIcon1.Text = "AutoClicker";
            this.notifyIcon1.Visible = true;
            // 
            // ctxMenuStrip
            // 
            this.ctxMenuStrip.AllowMerge = false;
            this.ctxMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuStart,
            this.mnuStop,
            this.mnuMode,
            this.mnuSettings,
            this.mnuQuit});
            this.ctxMenuStrip.Name = "contextMenuStrip1";
            this.ctxMenuStrip.ShowImageMargin = false;
            this.ctxMenuStrip.Size = new System.Drawing.Size(92, 114);
            // 
            // mnuStart
            // 
            this.mnuStart.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.mnuStart.Name = "mnuStart";
            this.mnuStart.Size = new System.Drawing.Size(91, 22);
            this.mnuStart.Text = "Start";
            this.mnuStart.Click += new System.EventHandler(this.mnuStart_Click);
            // 
            // mnuStop
            // 
            this.mnuStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.mnuStop.Name = "mnuStop";
            this.mnuStop.Size = new System.Drawing.Size(91, 22);
            this.mnuStop.Text = "Stop";
            this.mnuStop.Click += new System.EventHandler(this.mnuStop_Click);
            // 
            // mnuMode
            // 
            this.mnuMode.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.mnuMode.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.slowToolStripMenuItem,
            this.fastToolStripMenuItem,
            this.keepPressedToolStripMenuItem,
            this.clickAnywhereToolStripMenuItem});
            this.mnuMode.Name = "mnuMode";
            this.mnuMode.Size = new System.Drawing.Size(91, 22);
            this.mnuMode.Text = "Mode";
            // 
            // slowToolStripMenuItem
            // 
            this.slowToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.slowToolStripMenuItem.Name = "slowToolStripMenuItem";
            this.slowToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.slowToolStripMenuItem.Text = "Slow";
            this.slowToolStripMenuItem.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.slowToolStripMenuItem.Click += new System.EventHandler(this.slowToolStripMenuItem_Click);
            // 
            // fastToolStripMenuItem
            // 
            this.fastToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.fastToolStripMenuItem.Name = "fastToolStripMenuItem";
            this.fastToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.fastToolStripMenuItem.Text = "Fast";
            this.fastToolStripMenuItem.Click += new System.EventHandler(this.fastToolStripMenuItem_Click);
            // 
            // keepPressedToolStripMenuItem
            // 
            this.keepPressedToolStripMenuItem.Name = "keepPressedToolStripMenuItem";
            this.keepPressedToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.keepPressedToolStripMenuItem.Text = "Keep Pressed";
            this.keepPressedToolStripMenuItem.Click += new System.EventHandler(this.keepPressedToolStripMenuItem_Click);
            // 
            // clickAnywhereToolStripMenuItem
            // 
            this.clickAnywhereToolStripMenuItem.Name = "clickAnywhereToolStripMenuItem";
            this.clickAnywhereToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.clickAnywhereToolStripMenuItem.Text = "Click Anywhere";
            this.clickAnywhereToolStripMenuItem.Click += new System.EventHandler(this.clickAnywhereToolStripMenuItem_Click);
            // 
            // mnuSettings
            // 
            this.mnuSettings.Name = "mnuSettings";
            this.mnuSettings.Size = new System.Drawing.Size(91, 22);
            this.mnuSettings.Text = "Settings";
            this.mnuSettings.Click += new System.EventHandler(this.mnuSettings_Click);
            // 
            // mnuQuit
            // 
            this.mnuQuit.Name = "mnuQuit";
            this.mnuQuit.Size = new System.Drawing.Size(91, 22);
            this.mnuQuit.Text = "Quit";
            this.mnuQuit.Click += new System.EventHandler(this.mnuQuit_Click);
            // 
            // chkInRobloxOnly
            // 
            this.chkInRobloxOnly.AutoSize = true;
            this.chkInRobloxOnly.Location = new System.Drawing.Point(162, 184);
            this.chkInRobloxOnly.Name = "chkInRobloxOnly";
            this.chkInRobloxOnly.Size = new System.Drawing.Size(15, 14);
            this.chkInRobloxOnly.TabIndex = 7;
            this.chkInRobloxOnly.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 184);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Click Anywhere?";
            // 
            // btnHide
            // 
            this.btnHide.Location = new System.Drawing.Point(204, 203);
            this.btnHide.Name = "btnHide";
            this.btnHide.Size = new System.Drawing.Size(58, 32);
            this.btnHide.TabIndex = 9;
            this.btnHide.Text = "OK";
            this.btnHide.UseVisualStyleBackColor = true;
            this.btnHide.Click += new System.EventHandler(this.btnHide_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 109);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Button to press";
            // 
            // buttonToPress
            // 
            this.buttonToPress.Location = new System.Drawing.Point(162, 106);
            this.buttonToPress.Name = "buttonToPress";
            this.buttonToPress.Size = new System.Drawing.Size(100, 20);
            this.buttonToPress.TabIndex = 11;
            this.buttonToPress.Text = "E";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 86);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Auto Press Button";
            // 
            // chkAutoType
            // 
            this.chkAutoType.AutoSize = true;
            this.chkAutoType.Location = new System.Drawing.Point(162, 86);
            this.chkAutoType.Name = "chkAutoType";
            this.chkAutoType.Size = new System.Drawing.Size(15, 14);
            this.chkAutoType.TabIndex = 13;
            this.chkAutoType.UseVisualStyleBackColor = true;
            this.chkAutoType.CheckedChanged += new System.EventHandler(this.chkAutoType_CheckedChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 135);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Delay (ms)";
            // 
            // delayTime
            // 
            this.delayTime.Location = new System.Drawing.Point(162, 132);
            this.delayTime.Name = "delayTime";
            this.delayTime.Size = new System.Drawing.Size(100, 20);
            this.delayTime.TabIndex = 15;
            // 
            // pressedFor
            // 
            this.pressedFor.Location = new System.Drawing.Point(162, 158);
            this.pressedFor.Name = "pressedFor";
            this.pressedFor.Size = new System.Drawing.Size(100, 20);
            this.pressedFor.TabIndex = 17;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 161);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(82, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Pressed for (ms)";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 14);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(79, 13);
            this.label7.TabIndex = 19;
            this.label7.Text = "Keep Pressed?";
            // 
            // chkKeepPressed
            // 
            this.chkKeepPressed.AutoSize = true;
            this.chkKeepPressed.Location = new System.Drawing.Point(162, 14);
            this.chkKeepPressed.Name = "chkKeepPressed";
            this.chkKeepPressed.Size = new System.Drawing.Size(15, 14);
            this.chkKeepPressed.TabIndex = 18;
            this.chkKeepPressed.UseVisualStyleBackColor = true;
            this.chkKeepPressed.CheckedChanged += new System.EventHandler(this.chkKeepPressed_CheckedChanged);
            // 
            // AutoClicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(288, 280);
            this.ControlBox = false;
            this.Controls.Add(this.label7);
            this.Controls.Add(this.chkKeepPressed);
            this.Controls.Add(this.pressedFor);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.delayTime);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.chkAutoType);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.buttonToPress);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnHide);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkInRobloxOnly);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.maxWait);
            this.Controls.Add(this.minWait);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(290, 137);
            this.Name = "AutoClicker";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Auto Clicker";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.Load += new System.EventHandler(this.AutoClicker_Load);
            this.Resize += new System.EventHandler(this.AutoClicker_Resize);
            this.ctxMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox minWait;
        private System.Windows.Forms.TextBox maxWait;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip ctxMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem mnuStart;
        private System.Windows.Forms.ToolStripMenuItem mnuStop;
        private System.Windows.Forms.ToolStripMenuItem mnuQuit;
        private System.Windows.Forms.ToolStripMenuItem mnuSettings;
        private System.Windows.Forms.CheckBox chkInRobloxOnly;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnHide;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox buttonToPress;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkAutoType;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox delayTime;
        private System.Windows.Forms.TextBox pressedFor;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox chkKeepPressed;
        private System.Windows.Forms.ToolStripMenuItem mnuMode;
        private System.Windows.Forms.ToolStripMenuItem slowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fastToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clickAnywhereToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem keepPressedToolStripMenuItem;
    }
}

