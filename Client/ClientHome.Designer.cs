namespace Client
{
    partial class ClientHome
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
            BTN_Connect = new Button();
            label1 = new Label();
            TB_Username = new TextBox();
            GP_Chat = new GroupBox();
            TB_Message = new TextBox();
            TB_ChatBox = new TextBox();
            BTN_Send = new Button();
            label2 = new Label();
            TB_Address = new TextBox();
            GP_Chat.SuspendLayout();
            SuspendLayout();
            // 
            // BTN_Connect
            // 
            BTN_Connect.FlatStyle = FlatStyle.Popup;
            BTN_Connect.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            BTN_Connect.ForeColor = Color.Maroon;
            BTN_Connect.Location = new Point(693, 13);
            BTN_Connect.Name = "BTN_Connect";
            BTN_Connect.Size = new Size(95, 36);
            BTN_Connect.TabIndex = 0;
            BTN_Connect.Text = "CONNECT";
            BTN_Connect.UseVisualStyleBackColor = true;
            BTN_Connect.Click += BTN_Connect_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("OCRB", 15F, FontStyle.Regular, GraphicsUnit.Point);
            label1.ForeColor = Color.Maroon;
            label1.Location = new Point(40, 23);
            label1.Name = "label1";
            label1.Size = new Size(104, 17);
            label1.TabIndex = 1;
            label1.Text = "Username";
            // 
            // TB_Username
            // 
            TB_Username.Location = new Point(159, 22);
            TB_Username.Name = "TB_Username";
            TB_Username.Size = new Size(146, 23);
            TB_Username.TabIndex = 2;
            // 
            // GP_Chat
            // 
            GP_Chat.Controls.Add(TB_Message);
            GP_Chat.Controls.Add(TB_ChatBox);
            GP_Chat.Controls.Add(BTN_Send);
            GP_Chat.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            GP_Chat.ForeColor = Color.Maroon;
            GP_Chat.Location = new Point(221, 114);
            GP_Chat.Name = "GP_Chat";
            GP_Chat.Size = new Size(580, 334);
            GP_Chat.TabIndex = 3;
            GP_Chat.TabStop = false;
            GP_Chat.Text = "Chat";
            // 
            // TB_Message
            // 
            TB_Message.Font = new Font("OCRB", 10F, FontStyle.Regular, GraphicsUnit.Point);
            TB_Message.Location = new Point(0, 267);
            TB_Message.Multiline = true;
            TB_Message.Name = "TB_Message";
            TB_Message.Size = new Size(480, 67);
            TB_Message.TabIndex = 7;
            // 
            // TB_ChatBox
            // 
            TB_ChatBox.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            TB_ChatBox.Location = new Point(0, 22);
            TB_ChatBox.Multiline = true;
            TB_ChatBox.Name = "TB_ChatBox";
            TB_ChatBox.Size = new Size(574, 219);
            TB_ChatBox.TabIndex = 6;
            // 
            // BTN_Send
            // 
            BTN_Send.FlatStyle = FlatStyle.Popup;
            BTN_Send.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            BTN_Send.ForeColor = Color.Maroon;
            BTN_Send.Location = new Point(499, 277);
            BTN_Send.Name = "BTN_Send";
            BTN_Send.Size = new Size(75, 41);
            BTN_Send.TabIndex = 6;
            BTN_Send.Text = "SEND";
            BTN_Send.UseVisualStyleBackColor = true;
            BTN_Send.Click += BTN_Send_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("OCRB", 15F, FontStyle.Regular, GraphicsUnit.Point);
            label2.ForeColor = Color.Maroon;
            label2.Location = new Point(323, 24);
            label2.Name = "label2";
            label2.Size = new Size(176, 17);
            label2.TabIndex = 4;
            label2.Text = "Server Address";
            // 
            // TB_Address
            // 
            TB_Address.Location = new Point(505, 23);
            TB_Address.Name = "TB_Address";
            TB_Address.Size = new Size(151, 23);
            TB_Address.TabIndex = 5;
            // 
            // ClientHome
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(TB_Address);
            Controls.Add(label2);
            Controls.Add(GP_Chat);
            Controls.Add(TB_Username);
            Controls.Add(label1);
            Controls.Add(BTN_Connect);
            Name = "ClientHome";
            Text = "Client";
            FormClosing += ClientHome_FormClosing;
            GP_Chat.ResumeLayout(false);
            GP_Chat.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion


        private Button BTN_Connect;
        private Label label1;
        private TextBox TB_Username;
        private GroupBox GP_Chat;
        private TextBox TB_Message;
        private TextBox TB_ChatBox;
        private Button BTN_Send;
        private Label label2;
        private TextBox TB_Address;
    }
}