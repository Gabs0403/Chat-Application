namespace Chat_Engine_Project
{
    partial class ConversationScreen
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
            txtBoxMessage = new TextBox();
            label1 = new Label();
            fontDialog1 = new FontDialog();
            FLPChat = new FlowLayoutPanel();
            btnSend = new Button();
            comboBoxUsers = new ComboBox();
            btnChat = new Button();
            FLPChatPanel = new FlowLayoutPanel();
            lblChatWith = new Label();
            SuspendLayout();
            // 
            // txtBoxMessage
            // 
            txtBoxMessage.Location = new Point(325, 647);
            txtBoxMessage.Name = "txtBoxMessage";
            txtBoxMessage.Size = new Size(742, 23);
            txtBoxMessage.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(105, 24);
            label1.Name = "label1";
            label1.Size = new Size(38, 15);
            label1.TabIndex = 1;
            label1.Text = "label1";
            // 
            // FLPChat
            // 
            FLPChat.AutoScroll = true;
            FLPChat.FlowDirection = FlowDirection.TopDown;
            FLPChat.Location = new Point(325, 71);
            FLPChat.Name = "FLPChat";
            FLPChat.Size = new Size(856, 570);
            FLPChat.TabIndex = 9;
            // 
            // btnSend
            // 
            btnSend.Location = new Point(1083, 647);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(94, 23);
            btnSend.TabIndex = 10;
            btnSend.Text = "Send";
            btnSend.UseVisualStyleBackColor = true;
            btnSend.Click += btnSend_Click;
            // 
            // comboBoxUsers
            // 
            comboBoxUsers.FormattingEnabled = true;
            comboBoxUsers.Location = new Point(22, 71);
            comboBoxUsers.Name = "comboBoxUsers";
            comboBoxUsers.Size = new Size(187, 23);
            comboBoxUsers.TabIndex = 11;
            comboBoxUsers.Text = "Who do you want to chat with?";
            // 
            // btnChat
            // 
            btnChat.Location = new Point(225, 71);
            btnChat.Name = "btnChat";
            btnChat.Size = new Size(75, 23);
            btnChat.TabIndex = 12;
            btnChat.Text = "Chat";
            btnChat.UseVisualStyleBackColor = true;
            btnChat.Click += btnChat_Click;
            // 
            // FLPChatPanel
            // 
            FLPChatPanel.FlowDirection = FlowDirection.TopDown;
            FLPChatPanel.Location = new Point(22, 261);
            FLPChatPanel.Name = "FLPChatPanel";
            FLPChatPanel.Size = new Size(261, 409);
            FLPChatPanel.TabIndex = 13;
            FLPChatPanel.WrapContents = false;
            // 
            // lblChatWith
            // 
            lblChatWith.AutoSize = true;
            lblChatWith.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblChatWith.Location = new Point(335, 33);
            lblChatWith.Name = "lblChatWith";
            lblChatWith.Size = new Size(20, 25);
            lblChatWith.TabIndex = 14;
            lblChatWith.Text = "-";
            // 
            // ConversationScreen
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1189, 721);
            Controls.Add(lblChatWith);
            Controls.Add(FLPChatPanel);
            Controls.Add(btnChat);
            Controls.Add(comboBoxUsers);
            Controls.Add(btnSend);
            Controls.Add(FLPChat);
            Controls.Add(label1);
            Controls.Add(txtBoxMessage);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "ConversationScreen";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ConversationScreen";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtBoxMessage;
        private Label label1;
        private FontDialog fontDialog1;
        private FlowLayoutPanel FLPChat;
        private Button btnSend;
        private ComboBox comboBoxUsers;
        private Button btnChat;
        private FlowLayoutPanel FLPChatPanel;
        private Label lblChatWith;
    }
}