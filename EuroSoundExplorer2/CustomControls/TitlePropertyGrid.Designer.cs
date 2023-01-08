
namespace sb_explorer.CustomControls
{
    partial class TitlePropertyGrid
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

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelTitle = new System.Windows.Forms.Label();
            this.propsGrid = new System.Windows.Forms.PropertyGrid();
            this.SuspendLayout();
            // 
            // labelTitle
            // 
            this.labelTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTitle.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.labelTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitle.Location = new System.Drawing.Point(0, 0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(331, 20);
            this.labelTitle.TabIndex = 5;
            this.labelTitle.Text = "Properties";
            this.labelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // propsGrid
            // 
            this.propsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propsGrid.HelpVisible = false;
            this.propsGrid.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.propsGrid.Location = new System.Drawing.Point(0, 20);
            this.propsGrid.Name = "propsGrid";
            this.propsGrid.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.propsGrid.Size = new System.Drawing.Size(331, 294);
            this.propsGrid.TabIndex = 4;
            this.propsGrid.ToolbarVisible = false;
            // 
            // TitlePropertyGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelTitle);
            this.Controls.Add(this.propsGrid);
            this.Name = "TitlePropertyGrid";
            this.Size = new System.Drawing.Size(331, 314);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelTitle;
        protected internal System.Windows.Forms.PropertyGrid propsGrid;
    }
}
