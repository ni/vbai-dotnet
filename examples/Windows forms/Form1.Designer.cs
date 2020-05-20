namespace dotNET_API_Example
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
            this.launch = new System.Windows.Forms.Button();
            this.open = new System.Windows.Forms.Button();
            this.inspect = new System.Windows.Forms.Button();
            this.quit = new System.Windows.Forms.Button();
            this.InspectionName = new System.Windows.Forms.Label();
            this.Path = new System.Windows.Forms.TextBox();
            this.PassFailIndicator = new System.Windows.Forms.RadioButton();
            this.imageViewer1 = new NationalInstruments.Vision.WindowsForms.ImageViewer();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            //
            // launch
            //
            this.launch.Location = new System.Drawing.Point(12, 81);
            this.launch.Name = "launch";
            this.launch.Size = new System.Drawing.Size(131, 36);
            this.launch.TabIndex = 8;
            this.launch.Text = "Launch Engine";
            this.launch.UseVisualStyleBackColor = true;
            this.launch.Click += new System.EventHandler(this.launch_Click);
            //
            // open
            //
            this.open.Enabled = false;
            this.open.Location = new System.Drawing.Point(12, 123);
            this.open.Name = "open";
            this.open.Size = new System.Drawing.Size(131, 36);
            this.open.TabIndex = 9;
            this.open.Text = "Open Inspection";
            this.open.UseVisualStyleBackColor = true;
            this.open.Click += new System.EventHandler(this.open_Click);
            //
            // inspect
            //
            this.inspect.Enabled = false;
            this.inspect.Location = new System.Drawing.Point(12, 165);
            this.inspect.Name = "inspect";
            this.inspect.Size = new System.Drawing.Size(131, 36);
            this.inspect.TabIndex = 10;
            this.inspect.Text = "Inspect";
            this.inspect.UseVisualStyleBackColor = true;
            this.inspect.Click += new System.EventHandler(this.inspect_Click);
            //
            // quit
            //
            this.quit.Location = new System.Drawing.Point(12, 207);
            this.quit.Name = "quit";
            this.quit.Size = new System.Drawing.Size(131, 36);
            this.quit.TabIndex = 11;
            this.quit.Text = "Quit";
            this.quit.UseVisualStyleBackColor = true;
            this.quit.Click += new System.EventHandler(this.quit_Click);
            //
            // InspectionName
            //
            this.InspectionName.AutoSize = true;
            this.InspectionName.Location = new System.Drawing.Point(149, 111);
            this.InspectionName.Name = "InspectionName";
            this.InspectionName.Size = new System.Drawing.Size(87, 13);
            this.InspectionName.TabIndex = 14;
            this.InspectionName.Text = "Inspection Name";
            //
            // Path
            //
            this.Path.Location = new System.Drawing.Point(149, 127);
            this.Path.Multiline = true;
            this.Path.Name = "Path";
            this.Path.ReadOnly = true;
            this.Path.Size = new System.Drawing.Size(204, 32);
            this.Path.TabIndex = 13;
            //
            // PassFailIndicator
            //
            this.PassFailIndicator.AutoSize = true;
            this.PassFailIndicator.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.PassFailIndicator.Location = new System.Drawing.Point(152, 175);
            this.PassFailIndicator.MaximumSize = new System.Drawing.Size(200, 200);
            this.PassFailIndicator.Name = "PassFailIndicator";
            this.PassFailIndicator.Size = new System.Drawing.Size(100, 17);
            this.PassFailIndicator.TabIndex = 12;
            this.PassFailIndicator.TabStop = true;
            this.PassFailIndicator.Text = "Inspection Pass";
            this.PassFailIndicator.UseVisualStyleBackColor = true;
            //
            // imageViewer1
            //
            this.imageViewer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imageViewer1.Location = new System.Drawing.Point(359, 12);
            this.imageViewer1.Name = "imageViewer1";
            this.imageViewer1.Size = new System.Drawing.Size(481, 354);
            this.imageViewer1.TabIndex = 15;
            this.imageViewer1.ZoomToFit = true;
            //
            // openFileDialog1
            //
            this.openFileDialog1.DefaultExt = "vbai";
            this.openFileDialog1.Filter = "Inspections|*.vbai|All Files|*.*";
            this.openFileDialog1.Title = "Select Inspection to Open";
            //
            // textBox1
            //
            this.textBox1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Location = new System.Drawing.Point(12, 12);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(341, 63);
            this.textBox1.TabIndex = 16;
            this.textBox1.Text = "1. Click \"Launch Engine\"\r\n2. Click \"Open Inspection\" and select the inspection yo" +
                "u want to run.\r\n3. Click \"Inspect\" to run the inspection.";
            //
            // Form1
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(852, 378);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.imageViewer1);
            this.Controls.Add(this.InspectionName);
            this.Controls.Add(this.Path);
            this.Controls.Add(this.PassFailIndicator);
            this.Controls.Add(this.quit);
            this.Controls.Add(this.inspect);
            this.Controls.Add(this.open);
            this.Controls.Add(this.launch);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button launch;
        private System.Windows.Forms.Button open;
        private System.Windows.Forms.Button inspect;
        private System.Windows.Forms.Button quit;
        private System.Windows.Forms.Label InspectionName;
        private System.Windows.Forms.TextBox Path;
        private System.Windows.Forms.RadioButton PassFailIndicator;
        private NationalInstruments.Vision.WindowsForms.ImageViewer imageViewer1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox textBox1;

    }
}

