namespace Final.View {
    partial class SetPositionBox {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_ax = new System.Windows.Forms.TextBox();
            this.textBox_ay = new System.Windows.Forms.TextBox();
            this.textBox_az = new System.Windows.Forms.TextBox();
            this.textBox_tx = new System.Windows.Forms.TextBox();
            this.textBox_ty = new System.Windows.Forms.TextBox();
            this.textBox_tz = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("新細明體", 14F);
            this.label1.Location = new System.Drawing.Point(30, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(165, 19);
            this.label1.TabIndex = 1;
            this.label1.Text = "x             y             z";
            // 
            // textBox_ax
            // 
            this.textBox_ax.Location = new System.Drawing.Point(6, 40);
            this.textBox_ax.Name = "textBox_ax";
            this.textBox_ax.Size = new System.Drawing.Size(67, 23);
            this.textBox_ax.TabIndex = 2;
            this.textBox_ax.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox_ax.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form2_KeyDown);
            // 
            // textBox_ay
            // 
            this.textBox_ay.Location = new System.Drawing.Point(79, 40);
            this.textBox_ay.Name = "textBox_ay";
            this.textBox_ay.Size = new System.Drawing.Size(67, 23);
            this.textBox_ay.TabIndex = 3;
            this.textBox_ay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox_ay.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form2_KeyDown);
            // 
            // textBox_az
            // 
            this.textBox_az.Location = new System.Drawing.Point(152, 40);
            this.textBox_az.Name = "textBox_az";
            this.textBox_az.Size = new System.Drawing.Size(67, 23);
            this.textBox_az.TabIndex = 4;
            this.textBox_az.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox_az.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form2_KeyDown);
            // 
            // textBox_tx
            // 
            this.textBox_tx.Location = new System.Drawing.Point(6, 41);
            this.textBox_tx.Name = "textBox_tx";
            this.textBox_tx.Size = new System.Drawing.Size(67, 23);
            this.textBox_tx.TabIndex = 2;
            this.textBox_tx.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox_tx.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form2_KeyDown);
            // 
            // textBox_ty
            // 
            this.textBox_ty.Location = new System.Drawing.Point(79, 41);
            this.textBox_ty.Name = "textBox_ty";
            this.textBox_ty.Size = new System.Drawing.Size(67, 23);
            this.textBox_ty.TabIndex = 3;
            this.textBox_ty.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox_ty.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form2_KeyDown);
            // 
            // textBox_tz
            // 
            this.textBox_tz.Location = new System.Drawing.Point(152, 41);
            this.textBox_tz.Name = "textBox_tz";
            this.textBox_tz.Size = new System.Drawing.Size(67, 23);
            this.textBox_tz.TabIndex = 4;
            this.textBox_tz.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox_tz.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form2_KeyDown);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("新細明體", 14F);
            this.label4.Location = new System.Drawing.Point(30, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(165, 19);
            this.label4.TabIndex = 1;
            this.label4.Text = "x             y             z";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(46, 177);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "Save";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(132, 177);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 9;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox_ay);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBox_ax);
            this.groupBox1.Controls.Add(this.textBox_az);
            this.groupBox1.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(226, 78);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Rotation";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBox_tz);
            this.groupBox2.Controls.Add(this.textBox_tx);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.textBox_ty);
            this.groupBox2.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(12, 94);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(226, 77);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Translation";
            // 
            // SetPositionBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(250, 210);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "SetPositionBox";
            this.Text = "Set Position";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form2_KeyDown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_ax;
        private System.Windows.Forms.TextBox textBox_ay;
        private System.Windows.Forms.TextBox textBox_az;
        private System.Windows.Forms.TextBox textBox_tx;
        private System.Windows.Forms.TextBox textBox_ty;
        private System.Windows.Forms.TextBox textBox_tz;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}