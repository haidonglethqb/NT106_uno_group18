
namespace CLIENT
{
    partial class ConnectMenu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConnectMenu));
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.textBoxIP = new System.Windows.Forms.TextBox();
            this.btnCreate = new System.Windows.Forms.Button();
            this.btnJoin = new System.Windows.Forms.Button();
            this.btnRules = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBoxName
            // 
            this.textBoxName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.textBoxName.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.textBoxName.Location = new System.Drawing.Point(249, 141);
            this.textBoxName.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(210, 23);
            this.textBoxName.TabIndex = 0;
            this.textBoxName.Text = "Type in player\'s name";
            this.textBoxName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxName.MouseClick += new System.Windows.Forms.MouseEventHandler(this.textBoxName_MouseClick);
            // 
            // textBoxIP
            // 
            this.textBoxIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.textBoxIP.Location = new System.Drawing.Point(249, 173);
            this.textBoxIP.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxIP.Name = "textBoxIP";
            this.textBoxIP.Size = new System.Drawing.Size(210, 23);
            this.textBoxIP.TabIndex = 1;
            this.textBoxIP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnCreate
            // 
            this.btnCreate.BackColor = System.Drawing.Color.Transparent;
            this.btnCreate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCreate.FlatAppearance.BorderSize = 0;
            this.btnCreate.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCreate.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Orange;
            this.btnCreate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCreate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnCreate.Location = new System.Drawing.Point(291, 212);
            this.btnCreate.Margin = new System.Windows.Forms.Padding(2);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(130, 49);
            this.btnCreate.TabIndex = 2;
            this.btnCreate.UseVisualStyleBackColor = false;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // btnJoin
            // 
            this.btnJoin.BackColor = System.Drawing.Color.Transparent;
            this.btnJoin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnJoin.FlatAppearance.BorderSize = 0;
            this.btnJoin.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnJoin.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Orange;
            this.btnJoin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnJoin.Location = new System.Drawing.Point(164, 283);
            this.btnJoin.Margin = new System.Windows.Forms.Padding(2);
            this.btnJoin.Name = "btnJoin";
            this.btnJoin.Size = new System.Drawing.Size(136, 41);
            this.btnJoin.TabIndex = 3;
            this.btnJoin.UseVisualStyleBackColor = false;
            this.btnJoin.Click += new System.EventHandler(this.btnJoin_Click);
            // 
            // btnRules
            // 
            this.btnRules.BackColor = System.Drawing.Color.Transparent;
            this.btnRules.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRules.FlatAppearance.BorderSize = 0;
            this.btnRules.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnRules.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Orange;
            this.btnRules.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRules.Location = new System.Drawing.Point(420, 280);
            this.btnRules.Margin = new System.Windows.Forms.Padding(2);
            this.btnRules.Name = "btnRules";
            this.btnRules.Size = new System.Drawing.Size(128, 46);
            this.btnRules.TabIndex = 4;
            this.btnRules.UseVisualStyleBackColor = false;
            this.btnRules.Click += new System.EventHandler(this.btnRules_Click);
            // 
            // ConnectMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(712, 433);
            this.Controls.Add(this.btnRules);
            this.Controls.Add(this.btnJoin);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.textBoxIP);
            this.Controls.Add(this.textBoxName);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.Name = "ConnectMenu";
            this.Text = "Welcome to UNO Game !";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.TextBox textBoxIP;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnJoin;
        private System.Windows.Forms.Button btnRules;
    }
}

