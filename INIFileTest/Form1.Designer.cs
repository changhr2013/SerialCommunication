namespace INIFileTest
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.nameLab = new System.Windows.Forms.Label();
            this.sexLab = new System.Windows.Forms.Label();
            this.ageLab = new System.Windows.Forms.Label();
            this.addressLab = new System.Windows.Forms.Label();
            this.nameTxt = new System.Windows.Forms.TextBox();
            this.sexTxt = new System.Windows.Forms.TextBox();
            this.ageTxt = new System.Windows.Forms.TextBox();
            this.addressTxt = new System.Windows.Forms.TextBox();
            this.writeBtn = new System.Windows.Forms.Button();
            this.readBtn = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.addressTxt);
            this.groupBox1.Controls.Add(this.ageTxt);
            this.groupBox1.Controls.Add(this.sexTxt);
            this.groupBox1.Controls.Add(this.nameTxt);
            this.groupBox1.Controls.Add(this.addressLab);
            this.groupBox1.Controls.Add(this.ageLab);
            this.groupBox1.Controls.Add(this.sexLab);
            this.groupBox1.Controls.Add(this.nameLab);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(367, 235);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "ini示例";
            // 
            // nameLab
            // 
            this.nameLab.AutoSize = true;
            this.nameLab.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.nameLab.Location = new System.Drawing.Point(7, 42);
            this.nameLab.Name = "nameLab";
            this.nameLab.Size = new System.Drawing.Size(69, 20);
            this.nameLab.TabIndex = 0;
            this.nameLab.Text = "姓名：";
            // 
            // sexLab
            // 
            this.sexLab.AutoSize = true;
            this.sexLab.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.sexLab.Location = new System.Drawing.Point(7, 89);
            this.sexLab.Name = "sexLab";
            this.sexLab.Size = new System.Drawing.Size(69, 20);
            this.sexLab.TabIndex = 1;
            this.sexLab.Text = "性别：";
            // 
            // ageLab
            // 
            this.ageLab.AutoSize = true;
            this.ageLab.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ageLab.Location = new System.Drawing.Point(7, 134);
            this.ageLab.Name = "ageLab";
            this.ageLab.Size = new System.Drawing.Size(69, 20);
            this.ageLab.TabIndex = 2;
            this.ageLab.Text = "年龄：";
            // 
            // addressLab
            // 
            this.addressLab.AutoSize = true;
            this.addressLab.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.addressLab.Location = new System.Drawing.Point(7, 181);
            this.addressLab.Name = "addressLab";
            this.addressLab.Size = new System.Drawing.Size(69, 20);
            this.addressLab.TabIndex = 3;
            this.addressLab.Text = "地址：";
            // 
            // nameTxt
            // 
            this.nameTxt.Location = new System.Drawing.Point(82, 43);
            this.nameTxt.Name = "nameTxt";
            this.nameTxt.Size = new System.Drawing.Size(225, 25);
            this.nameTxt.TabIndex = 4;
            // 
            // sexTxt
            // 
            this.sexTxt.Location = new System.Drawing.Point(82, 90);
            this.sexTxt.Name = "sexTxt";
            this.sexTxt.Size = new System.Drawing.Size(225, 25);
            this.sexTxt.TabIndex = 5;
            // 
            // ageTxt
            // 
            this.ageTxt.Location = new System.Drawing.Point(82, 135);
            this.ageTxt.Name = "ageTxt";
            this.ageTxt.Size = new System.Drawing.Size(225, 25);
            this.ageTxt.TabIndex = 6;
            // 
            // addressTxt
            // 
            this.addressTxt.Location = new System.Drawing.Point(82, 182);
            this.addressTxt.Name = "addressTxt";
            this.addressTxt.Size = new System.Drawing.Size(225, 25);
            this.addressTxt.TabIndex = 7;
            // 
            // writeBtn
            // 
            this.writeBtn.Location = new System.Drawing.Point(24, 254);
            this.writeBtn.Name = "writeBtn";
            this.writeBtn.Size = new System.Drawing.Size(128, 37);
            this.writeBtn.TabIndex = 1;
            this.writeBtn.Text = "写入";
            this.writeBtn.UseVisualStyleBackColor = true;
            this.writeBtn.Click += new System.EventHandler(this.writeBtn_Click);
            // 
            // readBtn
            // 
            this.readBtn.Location = new System.Drawing.Point(192, 254);
            this.readBtn.Name = "readBtn";
            this.readBtn.Size = new System.Drawing.Size(128, 37);
            this.readBtn.TabIndex = 2;
            this.readBtn.Text = "读取";
            this.readBtn.UseVisualStyleBackColor = true;
            this.readBtn.Click += new System.EventHandler(this.readBtn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(359, 369);
            this.Controls.Add(this.readBtn);
            this.Controls.Add(this.writeBtn);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "ini测试";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox addressTxt;
        private System.Windows.Forms.TextBox ageTxt;
        private System.Windows.Forms.TextBox sexTxt;
        private System.Windows.Forms.TextBox nameTxt;
        private System.Windows.Forms.Label addressLab;
        private System.Windows.Forms.Label ageLab;
        private System.Windows.Forms.Label sexLab;
        private System.Windows.Forms.Label nameLab;
        private System.Windows.Forms.Button writeBtn;
        private System.Windows.Forms.Button readBtn;
    }
}

