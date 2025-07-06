namespace ADC_Read_App
{
    partial class Form1
    {
        /// <summary>
        ///Gerekli tasarımcı değişkeni.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///Kullanılan tüm kaynakları temizleyin.
        /// </summary>
        ///<param name="disposing">yönetilen kaynaklar dispose edilmeliyse doğru; aksi halde yanlış.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer üretilen kod

        /// <summary>
        /// Tasarımcı desteği için gerekli metot - bu metodun 
        ///içeriğini kod düzenleyici ile değiştirmeyin.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btn_connection = new System.Windows.Forms.Button();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.cmb_com = new System.Windows.Forms.ComboBox();
            this.cmb_baud = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lbl_status = new System.Windows.Forms.Label();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.txt_ADCRaw = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txt_RealVoltage = new System.Windows.Forms.TextBox();
            this.txt_offset = new System.Windows.Forms.TextBox();
            this.txt_gain = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btn_update_param = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.btn_save_param = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.txt_cur_offset = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txt_cur_gain = new System.Windows.Forms.TextBox();
            this.timer3 = new System.Windows.Forms.Timer(this.components);
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.label10 = new System.Windows.Forms.Label();
            this.lbl_dac = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.txt_ADC2 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_connection
            // 
            this.btn_connection.Location = new System.Drawing.Point(15, 105);
            this.btn_connection.Name = "btn_connection";
            this.btn_connection.Size = new System.Drawing.Size(200, 34);
            this.btn_connection.TabIndex = 0;
            this.btn_connection.Text = "Connect";
            this.btn_connection.UseVisualStyleBackColor = true;
            this.btn_connection.Click += new System.EventHandler(this.btn_connection_Click);
            // 
            // cmb_com
            // 
            this.cmb_com.FormattingEnabled = true;
            this.cmb_com.Location = new System.Drawing.Point(94, 34);
            this.cmb_com.Name = "cmb_com";
            this.cmb_com.Size = new System.Drawing.Size(121, 21);
            this.cmb_com.TabIndex = 1;
            // 
            // cmb_baud
            // 
            this.cmb_baud.FormattingEnabled = true;
            this.cmb_baud.Location = new System.Drawing.Point(94, 72);
            this.cmb_baud.Name = "cmb_baud";
            this.cmb_baud.Size = new System.Drawing.Size(121, 21);
            this.cmb_baud.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.Location = new System.Drawing.Point(12, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Com Port : ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label2.Location = new System.Drawing.Point(12, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Baudrate : ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label3.Location = new System.Drawing.Point(12, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(119, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Connection Status: ";
            // 
            // lbl_status
            // 
            this.lbl_status.AutoSize = true;
            this.lbl_status.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lbl_status.ForeColor = System.Drawing.Color.DarkRed;
            this.lbl_status.Location = new System.Drawing.Point(137, 9);
            this.lbl_status.Name = "lbl_status";
            this.lbl_status.Size = new System.Drawing.Size(71, 13);
            this.lbl_status.TabIndex = 6;
            this.lbl_status.Text = "Disconnect";
            // 
            // txt_ADCRaw
            // 
            this.txt_ADCRaw.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txt_ADCRaw.Location = new System.Drawing.Point(251, 42);
            this.txt_ADCRaw.Name = "txt_ADCRaw";
            this.txt_ADCRaw.Size = new System.Drawing.Size(174, 31);
            this.txt_ADCRaw.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label4.Location = new System.Drawing.Point(246, 14);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(187, 25);
            this.label4.TabIndex = 8;
            this.label4.Text = "ADC 1 Raw Data";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label5.Location = new System.Drawing.Point(267, 80);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(146, 25);
            this.label5.TabIndex = 10;
            this.label5.Text = "Calc Voltage";
            // 
            // txt_RealVoltage
            // 
            this.txt_RealVoltage.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txt_RealVoltage.Location = new System.Drawing.Point(251, 108);
            this.txt_RealVoltage.Name = "txt_RealVoltage";
            this.txt_RealVoltage.Size = new System.Drawing.Size(174, 31);
            this.txt_RealVoltage.TabIndex = 9;
            // 
            // txt_offset
            // 
            this.txt_offset.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F);
            this.txt_offset.Location = new System.Drawing.Point(224, 309);
            this.txt_offset.Name = "txt_offset";
            this.txt_offset.Size = new System.Drawing.Size(98, 31);
            this.txt_offset.TabIndex = 11;
            this.txt_offset.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_offset_KeyPress);
            // 
            // txt_gain
            // 
            this.txt_gain.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F);
            this.txt_gain.Location = new System.Drawing.Point(224, 357);
            this.txt_gain.Name = "txt_gain";
            this.txt_gain.Size = new System.Drawing.Size(98, 31);
            this.txt_gain.TabIndex = 12;
            this.txt_gain.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_gain_KeyPress);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label6.Location = new System.Drawing.Point(21, 315);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(187, 25);
            this.label6.TabIndex = 13;
            this.label6.Text = "New Ofset Value";
            // 
            // btn_update_param
            // 
            this.btn_update_param.Location = new System.Drawing.Point(329, 306);
            this.btn_update_param.Name = "btn_update_param";
            this.btn_update_param.Size = new System.Drawing.Size(98, 34);
            this.btn_update_param.TabIndex = 14;
            this.btn_update_param.Text = "Update Value";
            this.btn_update_param.UseVisualStyleBackColor = true;
            this.btn_update_param.Click += new System.EventHandler(this.btn_update_param_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label7.Location = new System.Drawing.Point(28, 363);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(180, 25);
            this.label7.TabIndex = 15;
            this.label7.Text = "New Gain Value";
            // 
            // btn_save_param
            // 
            this.btn_save_param.Location = new System.Drawing.Point(329, 354);
            this.btn_save_param.Name = "btn_save_param";
            this.btn_save_param.Size = new System.Drawing.Size(98, 34);
            this.btn_save_param.TabIndex = 16;
            this.btn_save_param.Text = "Save Value";
            this.btn_save_param.UseVisualStyleBackColor = true;
            this.btn_save_param.Click += new System.EventHandler(this.btn_save_param_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label8.Location = new System.Drawing.Point(28, 156);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(160, 25);
            this.label8.TabIndex = 18;
            this.label8.Text = "Current Offset";
            // 
            // txt_cur_offset
            // 
            this.txt_cur_offset.Enabled = false;
            this.txt_cur_offset.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txt_cur_offset.Location = new System.Drawing.Point(18, 184);
            this.txt_cur_offset.Name = "txt_cur_offset";
            this.txt_cur_offset.Size = new System.Drawing.Size(174, 31);
            this.txt_cur_offset.TabIndex = 17;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label9.Location = new System.Drawing.Point(258, 156);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(146, 25);
            this.label9.TabIndex = 20;
            this.label9.Text = "Current Gain";
            // 
            // txt_cur_gain
            // 
            this.txt_cur_gain.Enabled = false;
            this.txt_cur_gain.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txt_cur_gain.Location = new System.Drawing.Point(251, 184);
            this.txt_cur_gain.Name = "txt_cur_gain";
            this.txt_cur_gain.Size = new System.Drawing.Size(174, 31);
            this.txt_cur_gain.TabIndex = 19;
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(18, 436);
            this.trackBar1.Maximum = 4095;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(409, 45);
            this.trackBar1.TabIndex = 21;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            this.trackBar1.MouseLeave += new System.EventHandler(this.trackBar1_MouseLeave);
            this.trackBar1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.trackBar1_MouseUp);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label10.Location = new System.Drawing.Point(120, 408);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(127, 25);
            this.label10.TabIndex = 22;
            this.label10.Text = "Dac Value:";
            // 
            // lbl_dac
            // 
            this.lbl_dac.AutoSize = true;
            this.lbl_dac.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lbl_dac.Location = new System.Drawing.Point(246, 408);
            this.lbl_dac.Name = "lbl_dac";
            this.lbl_dac.Size = new System.Drawing.Size(25, 25);
            this.lbl_dac.TabIndex = 23;
            this.lbl_dac.Text = "0";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label11.Location = new System.Drawing.Point(120, 230);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(187, 25);
            this.label11.TabIndex = 25;
            this.label11.Text = "ADC 2 Raw Data";
            // 
            // txt_ADC2
            // 
            this.txt_ADC2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txt_ADC2.Location = new System.Drawing.Point(125, 258);
            this.txt_ADC2.Name = "txt_ADC2";
            this.txt_ADC2.Size = new System.Drawing.Size(174, 31);
            this.txt_ADC2.TabIndex = 24;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(438, 485);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txt_ADC2);
            this.Controls.Add(this.lbl_dac);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txt_cur_gain);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txt_cur_offset);
            this.Controls.Add(this.btn_save_param);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.btn_update_param);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txt_gain);
            this.Controls.Add(this.txt_offset);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txt_RealVoltage);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txt_ADCRaw);
            this.Controls.Add(this.lbl_status);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmb_baud);
            this.Controls.Add(this.cmb_com);
            this.Controls.Add(this.btn_connection);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_connection;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ComboBox cmb_com;
        private System.Windows.Forms.ComboBox cmb_baud;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lbl_status;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.TextBox txt_ADCRaw;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txt_RealVoltage;
        private System.Windows.Forms.TextBox txt_offset;
        private System.Windows.Forms.TextBox txt_gain;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btn_update_param;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btn_save_param;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txt_cur_offset;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txt_cur_gain;
        private System.Windows.Forms.Timer timer3;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lbl_dac;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txt_ADC2;
    }
}

