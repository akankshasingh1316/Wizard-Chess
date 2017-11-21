namespace ChessClient
{
    partial class MainForm
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
            this.picBoard = new System.Windows.Forms.PictureBox();
            this.btnListen = new System.Windows.Forms.Button();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblState = new System.Windows.Forms.Label();
            this.txtResponse = new System.Windows.Forms.TextBox();
            this.pbPicstatus = new System.Windows.Forms.ProgressBar();
            this.lblTurn = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picBoard)).BeginInit();
            this.SuspendLayout();
            // 
            // picBoard
            // 
            this.picBoard.BackColor = System.Drawing.SystemColors.ControlDark;
            this.picBoard.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picBoard.Location = new System.Drawing.Point(12, 12);
            this.picBoard.Name = "picBoard";
            this.picBoard.Size = new System.Drawing.Size(400, 400);
            this.picBoard.TabIndex = 0;
            this.picBoard.TabStop = false;
            this.picBoard.LoadCompleted += new System.ComponentModel.AsyncCompletedEventHandler(this.picBoard_LoadCompleted);
            // 
            // btnListen
            // 
            this.btnListen.Location = new System.Drawing.Point(418, 12);
            this.btnListen.Name = "btnListen";
            this.btnListen.Size = new System.Drawing.Size(224, 53);
            this.btnListen.TabIndex = 1;
            this.btnListen.Text = "Start listening";
            this.btnListen.UseVisualStyleBackColor = true;
            this.btnListen.Click += new System.EventHandler(this.btnListen_Click);
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(418, 84);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.txtLog.Size = new System.Drawing.Size(224, 328);
            this.txtLog.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(418, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Status: ";
            // 
            // lblState
            // 
            this.lblState.AutoSize = true;
            this.lblState.Location = new System.Drawing.Point(467, 68);
            this.lblState.Name = "lblState";
            this.lblState.Size = new System.Drawing.Size(78, 13);
            this.lblState.TabIndex = 6;
            this.lblState.Text = "waiting for user";
            // 
            // txtResponse
            // 
            this.txtResponse.Location = new System.Drawing.Point(179, 418);
            this.txtResponse.Multiline = true;
            this.txtResponse.Name = "txtResponse";
            this.txtResponse.ReadOnly = true;
            this.txtResponse.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResponse.Size = new System.Drawing.Size(463, 59);
            this.txtResponse.TabIndex = 7;
            // 
            // pbPicstatus
            // 
            this.pbPicstatus.Location = new System.Drawing.Point(12, 454);
            this.pbPicstatus.MarqueeAnimationSpeed = 50;
            this.pbPicstatus.Name = "pbPicstatus";
            this.pbPicstatus.Size = new System.Drawing.Size(161, 23);
            this.pbPicstatus.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.pbPicstatus.TabIndex = 8;
            // 
            // lblTurn
            // 
            this.lblTurn.AutoSize = true;
            this.lblTurn.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTurn.Location = new System.Drawing.Point(12, 421);
            this.lblTurn.Name = "lblTurn";
            this.lblTurn.Size = new System.Drawing.Size(120, 24);
            this.lblTurn.TabIndex = 9;
            this.lblTurn.Text = "White\'s turn";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(654, 489);
            this.Controls.Add(this.lblTurn);
            this.Controls.Add(this.pbPicstatus);
            this.Controls.Add(this.txtResponse);
            this.Controls.Add(this.lblState);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.btnListen);
            this.Controls.Add(this.picBoard);
            this.Name = "MainForm";
            this.Text = "Wizard\'s Chess";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picBoard)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picBoard;
        private System.Windows.Forms.Button btnListen;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblState;
        private System.Windows.Forms.TextBox txtResponse;
        private System.Windows.Forms.ProgressBar pbPicstatus;
        private System.Windows.Forms.Label lblTurn;
    }
}

