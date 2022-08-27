namespace PortTester
{
    partial class Form1
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.LabelPort = new System.Windows.Forms.Label();
            this.LabelProtocol = new System.Windows.Forms.Label();
            this.Port = new System.Windows.Forms.TextBox();
            this.Protocol = new System.Windows.Forms.ComboBox();
            this.Test = new System.Windows.Forms.Button();
            this.Status = new System.Windows.Forms.Label();
            this.TCPServer = new System.ComponentModel.BackgroundWorker();
            this.UDPServer = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // LabelPort
            // 
            this.LabelPort.AutoSize = true;
            this.LabelPort.Location = new System.Drawing.Point(69, 32);
            this.LabelPort.Name = "LabelPort";
            this.LabelPort.Size = new System.Drawing.Size(26, 13);
            this.LabelPort.TabIndex = 0;
            this.LabelPort.Text = "Port";
            // 
            // LabelProtocol
            // 
            this.LabelProtocol.AutoSize = true;
            this.LabelProtocol.Location = new System.Drawing.Point(202, 32);
            this.LabelProtocol.Name = "LabelProtocol";
            this.LabelProtocol.Size = new System.Drawing.Size(46, 13);
            this.LabelProtocol.TabIndex = 1;
            this.LabelProtocol.Text = "Protocol";
            // 
            // Port
            // 
            this.Port.Location = new System.Drawing.Point(35, 48);
            this.Port.Name = "Port";
            this.Port.Size = new System.Drawing.Size(100, 20);
            this.Port.TabIndex = 2;
            this.Port.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Protocol
            // 
            this.Protocol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Protocol.FormattingEnabled = true;
            this.Protocol.Items.AddRange(new object[] {
            "TCP",
            "UDP"});
            this.Protocol.Location = new System.Drawing.Point(174, 47);
            this.Protocol.Name = "Protocol";
            this.Protocol.Size = new System.Drawing.Size(104, 21);
            this.Protocol.TabIndex = 3;
            // 
            // Test
            // 
            this.Test.Location = new System.Drawing.Point(116, 128);
            this.Test.Name = "Test";
            this.Test.Size = new System.Drawing.Size(75, 23);
            this.Test.TabIndex = 4;
            this.Test.Text = "Test";
            this.Test.UseVisualStyleBackColor = true;
            this.Test.Click += new System.EventHandler(this.Test_Click);
            // 
            // Status
            // 
            this.Status.AutoSize = true;
            this.Status.Location = new System.Drawing.Point(134, 94);
            this.Status.Name = "Status";
            this.Status.Size = new System.Drawing.Size(37, 13);
            this.Status.TabIndex = 5;
            this.Status.Text = "Status";
            // 
            // TCPServer
            // 
            this.TCPServer.WorkerSupportsCancellation = true;
            this.TCPServer.DoWork += new System.ComponentModel.DoWorkEventHandler(this.TCPServer_DoWork);
            this.TCPServer.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.TCPServer_RunWorkerCompleted);
            // 
            // UDPServer
            // 
            this.UDPServer.WorkerSupportsCancellation = true;
            this.UDPServer.DoWork += new System.ComponentModel.DoWorkEventHandler(this.UDPServer_DoWork);
            this.UDPServer.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.UDPServer_RunWorkerCompleted);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(310, 178);
            this.Controls.Add(this.Status);
            this.Controls.Add(this.Test);
            this.Controls.Add(this.Protocol);
            this.Controls.Add(this.Port);
            this.Controls.Add(this.LabelProtocol);
            this.Controls.Add(this.LabelPort);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PortTester";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LabelPort;
        private System.Windows.Forms.Label LabelProtocol;
        private System.Windows.Forms.TextBox Port;
        private System.Windows.Forms.ComboBox Protocol;
        private System.Windows.Forms.Button Test;
        private System.Windows.Forms.Label Status;
        private System.ComponentModel.BackgroundWorker TCPServer;
        private System.ComponentModel.BackgroundWorker UDPServer;
    }
}

