namespace Bluetooth_tutorial
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tbOutput = new System.Windows.Forms.TextBox();
            this.tbText = new System.Windows.Forms.TextBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.gpConnectionType = new System.Windows.Forms.GroupBox();
            this.rbServer = new System.Windows.Forms.RadioButton();
            this.rbClient = new System.Windows.Forms.RadioButton();
            this.bGo = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.bt_ClearConsole = new System.Windows.Forms.Button();
            this.bt_Expand = new System.Windows.Forms.Button();
            this.bt_Stop = new System.Windows.Forms.Button();
            this.bt_Auto = new System.Windows.Forms.Button();
            this.gbControls = new System.Windows.Forms.GroupBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gpConnectionType.SuspendLayout();
            this.gbControls.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbOutput
            // 
            this.tbOutput.Location = new System.Drawing.Point(12, 51);
            this.tbOutput.Multiline = true;
            this.tbOutput.Name = "tbOutput";
            this.tbOutput.Size = new System.Drawing.Size(361, 321);
            this.tbOutput.TabIndex = 0;
            // 
            // tbText
            // 
            this.tbText.Location = new System.Drawing.Point(386, 445);
            this.tbText.Name = "tbText";
            this.tbText.Size = new System.Drawing.Size(192, 20);
            this.tbText.TabIndex = 1;
            this.tbText.Visible = false;
            this.tbText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbText_KeyPress);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(386, 238);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(192, 225);
            this.listBox1.TabIndex = 2;
            this.listBox1.DoubleClick += new System.EventHandler(this.listBox1_DoubleClick);
            // 
            // gpConnectionType
            // 
            this.gpConnectionType.Controls.Add(this.rbServer);
            this.gpConnectionType.Controls.Add(this.rbClient);
            this.gpConnectionType.Location = new System.Drawing.Point(386, 51);
            this.gpConnectionType.Name = "gpConnectionType";
            this.gpConnectionType.Size = new System.Drawing.Size(192, 80);
            this.gpConnectionType.TabIndex = 3;
            this.gpConnectionType.TabStop = false;
            this.gpConnectionType.Text = "Connection Type";
            // 
            // rbServer
            // 
            this.rbServer.AutoSize = true;
            this.rbServer.Location = new System.Drawing.Point(6, 57);
            this.rbServer.Name = "rbServer";
            this.rbServer.Size = new System.Drawing.Size(137, 17);
            this.rbServer.TabIndex = 1;
            this.rbServer.Text = "Server (Wait for device)";
            this.rbServer.UseVisualStyleBackColor = true;
            this.rbServer.CheckedChanged += new System.EventHandler(this.rbServer_CheckedChanged);
            // 
            // rbClient
            // 
            this.rbClient.AutoSize = true;
            this.rbClient.Checked = true;
            this.rbClient.Location = new System.Drawing.Point(6, 19);
            this.rbClient.Name = "rbClient";
            this.rbClient.Size = new System.Drawing.Size(140, 17);
            this.rbClient.TabIndex = 0;
            this.rbClient.TabStop = true;
            this.rbClient.Text = "Client (Scan for devices)";
            this.rbClient.UseVisualStyleBackColor = true;
            this.rbClient.Click += new System.EventHandler(this.rbClient_Click);
            // 
            // bGo
            // 
            this.bGo.Location = new System.Drawing.Point(386, 148);
            this.bGo.Name = "bGo";
            this.bGo.Size = new System.Drawing.Size(192, 55);
            this.bGo.TabIndex = 2;
            this.bGo.Text = "Go!";
            this.bGo.UseVisualStyleBackColor = true;
            this.bGo.Click += new System.EventHandler(this.bGo_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Console";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(383, 219);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Devices";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(390, 428);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(139, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Type here your text to send ";
            this.label3.Visible = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(95, 19);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 37);
            this.button1.TabIndex = 7;
            this.button1.Text = "Roll up";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // bt_ClearConsole
            // 
            this.bt_ClearConsole.Location = new System.Drawing.Point(298, 374);
            this.bt_ClearConsole.Name = "bt_ClearConsole";
            this.bt_ClearConsole.Size = new System.Drawing.Size(75, 23);
            this.bt_ClearConsole.TabIndex = 8;
            this.bt_ClearConsole.Text = "Clear";
            this.bt_ClearConsole.UseVisualStyleBackColor = true;
            this.bt_ClearConsole.Click += new System.EventHandler(this.bt_ClearConsole_Click);
            // 
            // bt_Expand
            // 
            this.bt_Expand.Location = new System.Drawing.Point(14, 19);
            this.bt_Expand.Name = "bt_Expand";
            this.bt_Expand.Size = new System.Drawing.Size(75, 37);
            this.bt_Expand.TabIndex = 9;
            this.bt_Expand.Text = "Expand";
            this.bt_Expand.UseVisualStyleBackColor = true;
            this.bt_Expand.Click += new System.EventHandler(this.bt_Expand_Click);
            // 
            // bt_Stop
            // 
            this.bt_Stop.Location = new System.Drawing.Point(188, 19);
            this.bt_Stop.Name = "bt_Stop";
            this.bt_Stop.Size = new System.Drawing.Size(75, 37);
            this.bt_Stop.TabIndex = 10;
            this.bt_Stop.Text = "Stop";
            this.bt_Stop.UseVisualStyleBackColor = true;
            this.bt_Stop.Click += new System.EventHandler(this.bt_Stop_Click);
            // 
            // bt_Auto
            // 
            this.bt_Auto.Location = new System.Drawing.Point(269, 19);
            this.bt_Auto.Name = "bt_Auto";
            this.bt_Auto.Size = new System.Drawing.Size(75, 37);
            this.bt_Auto.TabIndex = 11;
            this.bt_Auto.Text = "Auto";
            this.bt_Auto.UseVisualStyleBackColor = true;
            this.bt_Auto.Click += new System.EventHandler(this.bt_Auto_Click);
            // 
            // gbControls
            // 
            this.gbControls.Controls.Add(this.button1);
            this.gbControls.Controls.Add(this.bt_Auto);
            this.gbControls.Controls.Add(this.bt_Expand);
            this.gbControls.Controls.Add(this.bt_Stop);
            this.gbControls.Location = new System.Drawing.Point(12, 403);
            this.gbControls.Name = "gbControls";
            this.gbControls.Size = new System.Drawing.Size(361, 62);
            this.gbControls.TabIndex = 2;
            this.gbControls.TabStop = false;
            this.gbControls.Text = "Control buttons";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(590, 24);
            this.menuStrip1.TabIndex = 9;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(590, 482);
            this.Controls.Add(this.gbControls);
            this.Controls.Add(this.bt_ClearConsole);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.bGo);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.gpConnectionType);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.tbText);
            this.Controls.Add(this.tbOutput);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Control your blinds";
            this.gpConnectionType.ResumeLayout(false);
            this.gpConnectionType.PerformLayout();
            this.gbControls.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbOutput;
        private System.Windows.Forms.TextBox tbText;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.GroupBox gpConnectionType;
        private System.Windows.Forms.RadioButton rbServer;
        private System.Windows.Forms.RadioButton rbClient;
        private System.Windows.Forms.Button bGo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button bt_ClearConsole;
        private System.Windows.Forms.Button bt_Expand;
        private System.Windows.Forms.Button bt_Stop;
        private System.Windows.Forms.Button bt_Auto;
        private System.Windows.Forms.GroupBox gbControls;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
    }
}

