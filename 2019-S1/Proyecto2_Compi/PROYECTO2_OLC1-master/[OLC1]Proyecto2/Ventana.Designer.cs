namespace _OLC1_Proyecto2
{
    partial class Ventana
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.directoryEntry1 = new System.DirectoryServices.DirectoryEntry();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.txtArchivo1 = new System.Windows.Forms.RichTextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.txtArchivo2 = new System.Windows.Forms.RichTextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.txtArchivo3 = new System.Windows.Forms.RichTextBox();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.richTextBox4 = new System.Windows.Forms.RichTextBox();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.archivoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.BtnCrear = new System.Windows.Forms.ToolStripMenuItem();
            this.BtnAbrir = new System.Windows.Forms.ToolStripMenuItem();
            this.BtnGuardar = new System.Windows.Forms.ToolStripMenuItem();
            this.BtnGuardarComo = new System.Windows.Forms.ToolStripMenuItem();
            this.BtnEliminar = new System.Windows.Forms.ToolStripMenuItem();
            this.erroresToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reportesErroresToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compilarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.iniciarCompilacionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reportesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reporteErroresToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generarASTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Dialogo = new System.Windows.Forms.OpenFileDialog();
            this.reporteTokensToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(91, 52);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(950, 343);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.txtArchivo1);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(942, 314);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Archivo1.OLC";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // txtArchivo1
            // 
            this.txtArchivo1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtArchivo1.Font = new System.Drawing.Font("Consolas", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtArchivo1.Location = new System.Drawing.Point(6, 6);
            this.txtArchivo1.Name = "txtArchivo1";
            this.txtArchivo1.Size = new System.Drawing.Size(930, 302);
            this.txtArchivo1.TabIndex = 1;
            this.txtArchivo1.Text = "";
            this.txtArchivo1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtArchivo1_MouseClick);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.txtArchivo2);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(942, 314);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Archivo2.OLC";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // txtArchivo2
            // 
            this.txtArchivo2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtArchivo2.Font = new System.Drawing.Font("Consolas", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtArchivo2.Location = new System.Drawing.Point(6, 6);
            this.txtArchivo2.Name = "txtArchivo2";
            this.txtArchivo2.Size = new System.Drawing.Size(930, 302);
            this.txtArchivo2.TabIndex = 1;
            this.txtArchivo2.Text = "";
            this.txtArchivo2.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtArchivo2_MouseClick);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.txtArchivo3);
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(942, 314);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Archivo3.OLC";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // txtArchivo3
            // 
            this.txtArchivo3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtArchivo3.Font = new System.Drawing.Font("Consolas", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtArchivo3.Location = new System.Drawing.Point(6, 3);
            this.txtArchivo3.Name = "txtArchivo3";
            this.txtArchivo3.Size = new System.Drawing.Size(930, 305);
            this.txtArchivo3.TabIndex = 0;
            this.txtArchivo3.Text = "";
            this.txtArchivo3.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtArchivo3_MouseClick);
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Controls.Add(this.tabPage5);
            this.tabControl2.Location = new System.Drawing.Point(95, 423);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(946, 268);
            this.tabControl2.TabIndex = 1;
            // 
            // tabPage4
            // 
            this.tabPage4.BackColor = System.Drawing.Color.Black;
            this.tabPage4.Controls.Add(this.richTextBox4);
            this.tabPage4.Font = new System.Drawing.Font("Consolas", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage4.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.tabPage4.Location = new System.Drawing.Point(4, 25);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(938, 239);
            this.tabPage4.TabIndex = 0;
            this.tabPage4.Text = "Consola";
            // 
            // richTextBox4
            // 
            this.richTextBox4.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.richTextBox4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox4.ForeColor = System.Drawing.SystemColors.Info;
            this.richTextBox4.Location = new System.Drawing.Point(12, 6);
            this.richTextBox4.Name = "richTextBox4";
            this.richTextBox4.Size = new System.Drawing.Size(907, 227);
            this.richTextBox4.TabIndex = 0;
            this.richTextBox4.Text = "";
            // 
            // tabPage5
            // 
            this.tabPage5.BackColor = System.Drawing.Color.DimGray;
            this.tabPage5.Location = new System.Drawing.Point(4, 25);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(938, 239);
            this.tabPage5.TabIndex = 1;
            this.tabPage5.Text = "Tabla Simbolos";
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.archivoToolStripMenuItem,
            this.erroresToolStripMenuItem,
            this.compilarToolStripMenuItem,
            this.reportesToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1123, 31);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // archivoToolStripMenuItem
            // 
            this.archivoToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(78)))), ((int)(((byte)(211)))));
            this.archivoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.BtnCrear,
            this.BtnAbrir,
            this.BtnGuardar,
            this.BtnGuardarComo,
            this.BtnEliminar});
            this.archivoToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.archivoToolStripMenuItem.Name = "archivoToolStripMenuItem";
            this.archivoToolStripMenuItem.Size = new System.Drawing.Size(79, 27);
            this.archivoToolStripMenuItem.Text = "Archivo";
            // 
            // BtnCrear
            // 
            this.BtnCrear.Name = "BtnCrear";
            this.BtnCrear.Size = new System.Drawing.Size(199, 28);
            this.BtnCrear.Text = "Crear";
            this.BtnCrear.Click += new System.EventHandler(this.BtnCrear_Click);
            // 
            // BtnAbrir
            // 
            this.BtnAbrir.Name = "BtnAbrir";
            this.BtnAbrir.Size = new System.Drawing.Size(199, 28);
            this.BtnAbrir.Text = "Abrir";
            this.BtnAbrir.Click += new System.EventHandler(this.BtnAbrir_Click);
            // 
            // BtnGuardar
            // 
            this.BtnGuardar.Name = "BtnGuardar";
            this.BtnGuardar.Size = new System.Drawing.Size(199, 28);
            this.BtnGuardar.Text = "Guardar";
            this.BtnGuardar.Click += new System.EventHandler(this.BtnGuardar_Click);
            // 
            // BtnGuardarComo
            // 
            this.BtnGuardarComo.Name = "BtnGuardarComo";
            this.BtnGuardarComo.Size = new System.Drawing.Size(199, 28);
            this.BtnGuardarComo.Text = "Guardar Como";
            this.BtnGuardarComo.Click += new System.EventHandler(this.BtnGuardarComo_Click);
            // 
            // BtnEliminar
            // 
            this.BtnEliminar.Name = "BtnEliminar";
            this.BtnEliminar.Size = new System.Drawing.Size(199, 28);
            this.BtnEliminar.Text = "Eliminar";
            // 
            // erroresToolStripMenuItem
            // 
            this.erroresToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.erroresToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.reportesErroresToolStripMenuItem});
            this.erroresToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.erroresToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.erroresToolStripMenuItem.Name = "erroresToolStripMenuItem";
            this.erroresToolStripMenuItem.Size = new System.Drawing.Size(75, 27);
            this.erroresToolStripMenuItem.Text = "Errores";
            this.erroresToolStripMenuItem.Click += new System.EventHandler(this.erroresToolStripMenuItem_Click);
            // 
            // reportesErroresToolStripMenuItem
            // 
            this.reportesErroresToolStripMenuItem.Name = "reportesErroresToolStripMenuItem";
            this.reportesErroresToolStripMenuItem.Size = new System.Drawing.Size(211, 28);
            this.reportesErroresToolStripMenuItem.Text = "Reportes Errores";
            // 
            // compilarToolStripMenuItem
            // 
            this.compilarToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.compilarToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.iniciarCompilacionToolStripMenuItem});
            this.compilarToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.compilarToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.compilarToolStripMenuItem.Name = "compilarToolStripMenuItem";
            this.compilarToolStripMenuItem.Size = new System.Drawing.Size(91, 27);
            this.compilarToolStripMenuItem.Text = "Compilar";
            this.compilarToolStripMenuItem.Click += new System.EventHandler(this.compilarToolStripMenuItem_Click);
            // 
            // iniciarCompilacionToolStripMenuItem
            // 
            this.iniciarCompilacionToolStripMenuItem.Name = "iniciarCompilacionToolStripMenuItem";
            this.iniciarCompilacionToolStripMenuItem.Size = new System.Drawing.Size(232, 28);
            this.iniciarCompilacionToolStripMenuItem.Text = "Iniciar Compilacion";
            this.iniciarCompilacionToolStripMenuItem.Click += new System.EventHandler(this.iniciarCompilacionToolStripMenuItem_Click);
            // 
            // reportesToolStripMenuItem
            // 
            this.reportesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.reporteErroresToolStripMenuItem,
            this.generarASTToolStripMenuItem,
            this.reporteTokensToolStripMenuItem});
            this.reportesToolStripMenuItem.Name = "reportesToolStripMenuItem";
            this.reportesToolStripMenuItem.Size = new System.Drawing.Size(80, 27);
            this.reportesToolStripMenuItem.Text = "Reportes";
            // 
            // reporteErroresToolStripMenuItem
            // 
            this.reporteErroresToolStripMenuItem.BackColor = System.Drawing.SystemColors.ControlText;
            this.reporteErroresToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.reporteErroresToolStripMenuItem.Name = "reporteErroresToolStripMenuItem";
            this.reporteErroresToolStripMenuItem.Size = new System.Drawing.Size(216, 26);
            this.reporteErroresToolStripMenuItem.Text = "Reporte Errores";
            this.reporteErroresToolStripMenuItem.Click += new System.EventHandler(this.reporteErroresToolStripMenuItem_Click);
            this.reporteErroresToolStripMenuItem.Paint += new System.Windows.Forms.PaintEventHandler(this.reporteErroresToolStripMenuItem_Paint);
            // 
            // generarASTToolStripMenuItem
            // 
            this.generarASTToolStripMenuItem.BackColor = System.Drawing.SystemColors.Desktop;
            this.generarASTToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.generarASTToolStripMenuItem.Name = "generarASTToolStripMenuItem";
            this.generarASTToolStripMenuItem.Size = new System.Drawing.Size(216, 26);
            this.generarASTToolStripMenuItem.Text = "Generar AST";
            this.generarASTToolStripMenuItem.Click += new System.EventHandler(this.generarASTToolStripMenuItem_Click);
            this.generarASTToolStripMenuItem.Paint += new System.Windows.Forms.PaintEventHandler(this.generarASTToolStripMenuItem_Paint);
            // 
            // Dialogo
            // 
            this.Dialogo.FileName = "openFileDialog1";
            // 
            // reporteTokensToolStripMenuItem
            // 
            this.reporteTokensToolStripMenuItem.BackColor = System.Drawing.SystemColors.ControlText;
            this.reporteTokensToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.reporteTokensToolStripMenuItem.Name = "reporteTokensToolStripMenuItem";
            this.reporteTokensToolStripMenuItem.Size = new System.Drawing.Size(216, 26);
            this.reporteTokensToolStripMenuItem.Text = "Reporte Tokens";
            this.reporteTokensToolStripMenuItem.Click += new System.EventHandler(this.reporteTokensToolStripMenuItem_Click);
            this.reporteTokensToolStripMenuItem.Paint += new System.Windows.Forms.PaintEventHandler(this.reporteTokensToolStripMenuItem_Paint);
            // 
            // Ventana
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1123, 703);
            this.Controls.Add(this.tabControl2);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Ventana";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OLC1 Proyecto2";
            this.Load += new System.EventHandler(this.Ventana_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.DirectoryServices.DirectoryEntry directoryEntry1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem archivoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem BtnCrear;
        private System.Windows.Forms.ToolStripMenuItem BtnAbrir;
        private System.Windows.Forms.ToolStripMenuItem BtnGuardar;
        private System.Windows.Forms.ToolStripMenuItem BtnGuardarComo;
        private System.Windows.Forms.ToolStripMenuItem BtnEliminar;
        private System.Windows.Forms.ToolStripMenuItem erroresToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem compilarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reportesErroresToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem iniciarCompilacionToolStripMenuItem;
        private System.Windows.Forms.RichTextBox txtArchivo3;
        private System.Windows.Forms.RichTextBox txtArchivo1;
        private System.Windows.Forms.RichTextBox txtArchivo2;
        private System.Windows.Forms.RichTextBox richTextBox4;
        private System.Windows.Forms.OpenFileDialog Dialogo;
        private System.Windows.Forms.ToolStripMenuItem reportesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reporteErroresToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generarASTToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reporteTokensToolStripMenuItem;
    }
}

