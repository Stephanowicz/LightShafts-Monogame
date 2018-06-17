namespace LightShafts
{
    partial class Controls
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
            this.trackBarLightPosX = new System.Windows.Forms.TrackBar();
            this.trackBarLightPosY = new System.Windows.Forms.TrackBar();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.trackBarLightShaftExposure = new System.Windows.Forms.TrackBar();
            this.trackBarLightShaftDecay = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.trackBarLightShaftDensity = new System.Windows.Forms.TrackBar();
            this.label4 = new System.Windows.Forms.Label();
            this.trackBarLightShaftWeight = new System.Windows.Forms.TrackBar();
            this.trackBarGearExposure = new System.Windows.Forms.TrackBar();
            this.label5 = new System.Windows.Forms.Label();
            this.trackBarLuminanceThreshold = new System.Windows.Forms.TrackBar();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.trackBarLuminanceScaleFactor = new System.Windows.Forms.TrackBar();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.trackBarLightMapOffsetY = new System.Windows.Forms.TrackBar();
            this.trackBarLightMapOffsetX = new System.Windows.Forms.TrackBar();
            this.label12 = new System.Windows.Forms.Label();
            this.trackBarFlareTexDivisor = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLightPosX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLightPosY)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLightShaftExposure)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLightShaftDecay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLightShaftDensity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLightShaftWeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarGearExposure)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLuminanceThreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLuminanceScaleFactor)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLightMapOffsetY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLightMapOffsetX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarFlareTexDivisor)).BeginInit();
            this.SuspendLayout();
            // 
            // trackBarLightPosX
            // 
            this.trackBarLightPosX.AutoSize = false;
            this.trackBarLightPosX.Location = new System.Drawing.Point(3, 21);
            this.trackBarLightPosX.Margin = new System.Windows.Forms.Padding(0);
            this.trackBarLightPosX.Maximum = 500;
            this.trackBarLightPosX.Name = "trackBarLightPosX";
            this.trackBarLightPosX.Size = new System.Drawing.Size(90, 25);
            this.trackBarLightPosX.TabIndex = 0;
            this.trackBarLightPosX.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarLightPosX.Scroll += new System.EventHandler(this.trackBarLightPos_Scroll);
            // 
            // trackBarLightPosY
            // 
            this.trackBarLightPosY.AutoSize = false;
            this.trackBarLightPosY.Location = new System.Drawing.Point(3, 46);
            this.trackBarLightPosY.Margin = new System.Windows.Forms.Padding(0);
            this.trackBarLightPosY.Maximum = 500;
            this.trackBarLightPosY.Name = "trackBarLightPosY";
            this.trackBarLightPosY.Size = new System.Drawing.Size(90, 25);
            this.trackBarLightPosY.TabIndex = 0;
            this.trackBarLightPosY.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarLightPosY.Scroll += new System.EventHandler(this.trackBarLightPos_Scroll);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.trackBarLightPosY);
            this.groupBox1.Controls.Add(this.trackBarLightPosX);
            this.groupBox1.Location = new System.Drawing.Point(5, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(141, 77);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Light position";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(89, 48);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(14, 13);
            this.label9.TabIndex = 12;
            this.label9.Text = "Y";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(89, 24);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(14, 13);
            this.label8.TabIndex = 11;
            this.label8.Text = "X";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 214);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "LightShaft Exposure";
            // 
            // trackBarLightShaftExposure
            // 
            this.trackBarLightShaftExposure.AutoSize = false;
            this.trackBarLightShaftExposure.Location = new System.Drawing.Point(8, 230);
            this.trackBarLightShaftExposure.Maximum = 1500;
            this.trackBarLightShaftExposure.Name = "trackBarLightShaftExposure";
            this.trackBarLightShaftExposure.Size = new System.Drawing.Size(116, 23);
            this.trackBarLightShaftExposure.TabIndex = 3;
            this.trackBarLightShaftExposure.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarLightShaftExposure.Scroll += new System.EventHandler(this.trackBarLightShaftExposure_Scroll);
            // 
            // trackBarLightShaftDecay
            // 
            this.trackBarLightShaftDecay.AutoSize = false;
            this.trackBarLightShaftDecay.Location = new System.Drawing.Point(8, 270);
            this.trackBarLightShaftDecay.Maximum = 1100;
            this.trackBarLightShaftDecay.Name = "trackBarLightShaftDecay";
            this.trackBarLightShaftDecay.Size = new System.Drawing.Size(116, 23);
            this.trackBarLightShaftDecay.TabIndex = 5;
            this.trackBarLightShaftDecay.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarLightShaftDecay.Scroll += new System.EventHandler(this.trackBarLightShaftDecay_Scroll);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 254);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "LightShaft Decay";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 293);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "LightShaft Density";
            // 
            // trackBarLightShaftDensity
            // 
            this.trackBarLightShaftDensity.AutoSize = false;
            this.trackBarLightShaftDensity.Location = new System.Drawing.Point(8, 309);
            this.trackBarLightShaftDensity.Maximum = 2000;
            this.trackBarLightShaftDensity.Name = "trackBarLightShaftDensity";
            this.trackBarLightShaftDensity.Size = new System.Drawing.Size(116, 23);
            this.trackBarLightShaftDensity.TabIndex = 5;
            this.trackBarLightShaftDensity.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarLightShaftDensity.Scroll += new System.EventHandler(this.trackBarLightShaftDensity_Scroll);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 333);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "LightShaft Weight";
            // 
            // trackBarLightShaftWeight
            // 
            this.trackBarLightShaftWeight.AutoSize = false;
            this.trackBarLightShaftWeight.Location = new System.Drawing.Point(8, 349);
            this.trackBarLightShaftWeight.Maximum = 1500;
            this.trackBarLightShaftWeight.Name = "trackBarLightShaftWeight";
            this.trackBarLightShaftWeight.Size = new System.Drawing.Size(116, 23);
            this.trackBarLightShaftWeight.TabIndex = 5;
            this.trackBarLightShaftWeight.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarLightShaftWeight.Scroll += new System.EventHandler(this.trackBarLightShaftWeight_Scroll);
            // 
            // trackBarGearExposure
            // 
            this.trackBarGearExposure.AutoSize = false;
            this.trackBarGearExposure.Location = new System.Drawing.Point(8, 399);
            this.trackBarGearExposure.Maximum = 1000;
            this.trackBarGearExposure.Name = "trackBarGearExposure";
            this.trackBarGearExposure.Size = new System.Drawing.Size(116, 23);
            this.trackBarGearExposure.TabIndex = 7;
            this.trackBarGearExposure.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarGearExposure.Scroll += new System.EventHandler(this.trackBarGearExposure_Scroll);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 383);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(83, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Model Exposure";
            // 
            // trackBarLuminanceThreshold
            // 
            this.trackBarLuminanceThreshold.AutoSize = false;
            this.trackBarLuminanceThreshold.Location = new System.Drawing.Point(5, 460);
            this.trackBarLuminanceThreshold.Maximum = 1000;
            this.trackBarLuminanceThreshold.Name = "trackBarLuminanceThreshold";
            this.trackBarLuminanceThreshold.Size = new System.Drawing.Size(104, 23);
            this.trackBarLuminanceThreshold.TabIndex = 9;
            this.trackBarLuminanceThreshold.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarLuminanceThreshold.Scroll += new System.EventHandler(this.trackBarLuminanceThreshold_Scroll);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(5, 444);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(106, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "LuminanceThreshold";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(5, 483);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(116, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "LuminanceScaleFactor";
            // 
            // trackBarLuminanceScaleFactor
            // 
            this.trackBarLuminanceScaleFactor.AutoSize = false;
            this.trackBarLuminanceScaleFactor.Location = new System.Drawing.Point(5, 499);
            this.trackBarLuminanceScaleFactor.Maximum = 5000;
            this.trackBarLuminanceScaleFactor.Name = "trackBarLuminanceScaleFactor";
            this.trackBarLuminanceScaleFactor.Size = new System.Drawing.Size(104, 23);
            this.trackBarLuminanceScaleFactor.TabIndex = 9;
            this.trackBarLuminanceScaleFactor.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarLuminanceScaleFactor.Scroll += new System.EventHandler(this.trackBarLuminanceScaleFactor_Scroll);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.trackBarLightMapOffsetY);
            this.groupBox2.Controls.Add(this.trackBarLightMapOffsetX);
            this.groupBox2.Location = new System.Drawing.Point(5, 86);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(141, 74);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Texture/Light offset";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(89, 50);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(14, 13);
            this.label10.TabIndex = 14;
            this.label10.Text = "Y";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(89, 26);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(14, 13);
            this.label11.TabIndex = 13;
            this.label11.Text = "X";
            // 
            // trackBarLightMapOffsetY
            // 
            this.trackBarLightMapOffsetY.AutoSize = false;
            this.trackBarLightMapOffsetY.Location = new System.Drawing.Point(3, 46);
            this.trackBarLightMapOffsetY.Margin = new System.Windows.Forms.Padding(0);
            this.trackBarLightMapOffsetY.Maximum = 100;
            this.trackBarLightMapOffsetY.Minimum = -100;
            this.trackBarLightMapOffsetY.Name = "trackBarLightMapOffsetY";
            this.trackBarLightMapOffsetY.Size = new System.Drawing.Size(90, 25);
            this.trackBarLightMapOffsetY.TabIndex = 0;
            this.trackBarLightMapOffsetY.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarLightMapOffsetY.Scroll += new System.EventHandler(this.trackBarLightMapOffset_Scroll);
            // 
            // trackBarLightMapOffsetX
            // 
            this.trackBarLightMapOffsetX.AutoSize = false;
            this.trackBarLightMapOffsetX.Location = new System.Drawing.Point(3, 21);
            this.trackBarLightMapOffsetX.Margin = new System.Windows.Forms.Padding(0);
            this.trackBarLightMapOffsetX.Maximum = 100;
            this.trackBarLightMapOffsetX.Minimum = -100;
            this.trackBarLightMapOffsetX.Name = "trackBarLightMapOffsetX";
            this.trackBarLightMapOffsetX.Size = new System.Drawing.Size(90, 25);
            this.trackBarLightMapOffsetX.TabIndex = 0;
            this.trackBarLightMapOffsetX.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarLightMapOffsetX.Scroll += new System.EventHandler(this.trackBarLightMapOffset_Scroll);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(8, 167);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(87, 13);
            this.label12.TabIndex = 12;
            this.label12.Text = "FlareTexture size";
            // 
            // trackBarFlareTexDivisor
            // 
            this.trackBarFlareTexDivisor.AutoSize = false;
            this.trackBarFlareTexDivisor.LargeChange = 1;
            this.trackBarFlareTexDivisor.Location = new System.Drawing.Point(11, 183);
            this.trackBarFlareTexDivisor.Maximum = 800;
            this.trackBarFlareTexDivisor.Minimum = 1;
            this.trackBarFlareTexDivisor.Name = "trackBarFlareTexDivisor";
            this.trackBarFlareTexDivisor.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.trackBarFlareTexDivisor.Size = new System.Drawing.Size(116, 23);
            this.trackBarFlareTexDivisor.TabIndex = 13;
            this.trackBarFlareTexDivisor.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarFlareTexDivisor.Value = 1;
            this.trackBarFlareTexDivisor.Scroll += new System.EventHandler(this.trackBarFlareTexDivisor_Scroll);
            // 
            // Controls
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(151, 524);
            this.Controls.Add(this.trackBarFlareTexDivisor);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.trackBarLuminanceScaleFactor);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.trackBarLuminanceThreshold);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.trackBarGearExposure);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.trackBarLightShaftWeight);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.trackBarLightShaftDensity);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.trackBarLightShaftDecay);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.trackBarLightShaftExposure);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Controls";
            this.Text = "Controls";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLightPosX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLightPosY)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLightShaftExposure)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLightShaftDecay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLightShaftDensity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLightShaftWeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarGearExposure)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLuminanceThreshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLuminanceScaleFactor)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLightMapOffsetY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLightMapOffsetX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarFlareTexDivisor)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar trackBarLightPosX;
        private System.Windows.Forms.TrackBar trackBarLightPosY;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar trackBarLightShaftExposure;
        private System.Windows.Forms.TrackBar trackBarLightShaftDecay;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TrackBar trackBarLightShaftDensity;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TrackBar trackBarLightShaftWeight;
        private System.Windows.Forms.TrackBar trackBarGearExposure;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TrackBar trackBarLuminanceThreshold;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TrackBar trackBarLuminanceScaleFactor;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TrackBar trackBarLightMapOffsetY;
        private System.Windows.Forms.TrackBar trackBarLightMapOffsetX;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TrackBar trackBarFlareTexDivisor;
    }
}