namespace Server
{
    partial class ServerHome
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            TB_Address = new TextBox();
            BTN_Start = new Button();
            label1 = new Label();
            TB_Log = new TextBox();
            SuspendLayout();
            // 
            // TB_Address
            // 
            TB_Address.Location = new Point(56, 77);
            TB_Address.Name = "TB_Address";
            TB_Address.Size = new Size(220, 23);
            TB_Address.TabIndex = 0;
            // 
            // BTN_Start
            // 
            BTN_Start.FlatStyle = FlatStyle.Popup;
            BTN_Start.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            BTN_Start.Location = new Point(118, 122);
            BTN_Start.Name = "BTN_Start";
            BTN_Start.Size = new Size(94, 36);
            BTN_Start.TabIndex = 1;
            BTN_Start.Text = "START";
            BTN_Start.UseVisualStyleBackColor = true;
            BTN_Start.Click += BTN_Start_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("OCRB", 20F, FontStyle.Regular, GraphicsUnit.Point);
            label1.ForeColor = Color.Maroon;
            label1.Location = new Point(80, 26);
            label1.Name = "label1";
            label1.Size = new Size(170, 23);
            label1.TabIndex = 2;
            label1.Text = "IP Address";
            // 
            // TB_Log
            // 
            TB_Log.Location = new Point(12, 183);
            TB_Log.Multiline = true;
            TB_Log.Name = "TB_Log";
            TB_Log.ReadOnly = true;
            TB_Log.Size = new Size(321, 244);
            TB_Log.TabIndex = 3;
            // 
            // ServerHome
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(345, 453);
            Controls.Add(TB_Log);
            Controls.Add(label1);
            Controls.Add(BTN_Start);
            Controls.Add(TB_Address);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MaximumSize = new Size(512, 512);
            Name = "ServerHome";
            Text = "Server";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox TB_Address;
        private Button BTN_Start;
        private Label label1;
        private TextBox TB_Log;
    }
}