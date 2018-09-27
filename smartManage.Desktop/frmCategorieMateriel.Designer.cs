﻿namespace smartManage.Desktop
{
    partial class frmCategorieMateriel
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCategorieMateriel));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.bdNav = new System.Windows.Forms.BindingNavigator(this.components);
            this.bdNew = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorCountItem = new System.Windows.Forms.ToolStripLabel();
            this.bdDelete = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveFirstItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMovePreviousItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorPositionItem = new System.Windows.Forms.ToolStripTextBox();
            this.bindingNavigatorSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorMoveNextItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveLastItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.bdSave = new System.Windows.Forms.ToolStripButton();
            this.bdRefresh = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.txtSearch = new System.Windows.Forms.ToolStripTextBox();
            this.bdSearch = new System.Windows.Forms.ToolStripButton();
            this.bdPreview = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.txtDateModifie = new System.Windows.Forms.DateTimePicker();
            this.label17 = new System.Windows.Forms.Label();
            this.txtModifieBy = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.txtDateCreate = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.txtCreateBy = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtId = new System.Windows.Forms.TextBox();
            this.txtDesignation = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.bdNav)).BeginInit();
            this.bdNav.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // bdNav
            // 
            this.bdNav.AddNewItem = this.bdNew;
            this.bdNav.CountItem = this.bindingNavigatorCountItem;
            this.bdNav.DeleteItem = this.bdDelete;
            this.bdNav.Enabled = false;
            this.bdNav.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorMoveFirstItem,
            this.bindingNavigatorMovePreviousItem,
            this.bindingNavigatorSeparator,
            this.bindingNavigatorPositionItem,
            this.bindingNavigatorCountItem,
            this.bindingNavigatorSeparator1,
            this.bindingNavigatorMoveNextItem,
            this.bindingNavigatorMoveLastItem,
            this.bindingNavigatorSeparator2,
            this.bdNew,
            this.bdDelete,
            this.bdSave,
            this.bdRefresh,
            this.toolStripSeparator6,
            this.txtSearch,
            this.bdSearch,
            this.bdPreview});
            this.bdNav.Location = new System.Drawing.Point(0, 0);
            this.bdNav.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
            this.bdNav.MoveLastItem = this.bindingNavigatorMoveLastItem;
            this.bdNav.MoveNextItem = this.bindingNavigatorMoveNextItem;
            this.bdNav.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
            this.bdNav.Name = "bdNav";
            this.bdNav.PositionItem = this.bindingNavigatorPositionItem;
            this.bdNav.Size = new System.Drawing.Size(484, 25);
            this.bdNav.TabIndex = 1;
            this.bdNav.Text = "bindingNavigator1";
            // 
            // bdNew
            // 
            this.bdNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bdNew.Image = ((System.Drawing.Image)(resources.GetObject("bdNew.Image")));
            this.bdNew.Name = "bdNew";
            this.bdNew.RightToLeftAutoMirrorImage = true;
            this.bdNew.Size = new System.Drawing.Size(23, 22);
            this.bdNew.Text = "Nouvel enregistrement";
            // 
            // bindingNavigatorCountItem
            // 
            this.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
            this.bindingNavigatorCountItem.Size = new System.Drawing.Size(35, 22);
            this.bindingNavigatorCountItem.Text = "of {0}";
            this.bindingNavigatorCountItem.ToolTipText = "Total number of items";
            // 
            // bdDelete
            // 
            this.bdDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bdDelete.Image = ((System.Drawing.Image)(resources.GetObject("bdDelete.Image")));
            this.bdDelete.Name = "bdDelete";
            this.bdDelete.RightToLeftAutoMirrorImage = true;
            this.bdDelete.Size = new System.Drawing.Size(23, 22);
            this.bdDelete.Text = "Supprimer enregistrement";
            // 
            // bindingNavigatorMoveFirstItem
            // 
            this.bindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveFirstItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveFirstItem.Image")));
            this.bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
            this.bindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveFirstItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveFirstItem.Text = "Premier enregistrement";
            // 
            // bindingNavigatorMovePreviousItem
            // 
            this.bindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMovePreviousItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMovePreviousItem.Image")));
            this.bindingNavigatorMovePreviousItem.Name = "bindingNavigatorMovePreviousItem";
            this.bindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMovePreviousItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMovePreviousItem.Text = "Enregistrement précédent";
            // 
            // bindingNavigatorSeparator
            // 
            this.bindingNavigatorSeparator.Name = "bindingNavigatorSeparator";
            this.bindingNavigatorSeparator.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorPositionItem
            // 
            this.bindingNavigatorPositionItem.AccessibleName = "Position";
            this.bindingNavigatorPositionItem.AutoSize = false;
            this.bindingNavigatorPositionItem.Name = "bindingNavigatorPositionItem";
            this.bindingNavigatorPositionItem.Size = new System.Drawing.Size(50, 23);
            this.bindingNavigatorPositionItem.Text = "0";
            this.bindingNavigatorPositionItem.ToolTipText = "Current position";
            // 
            // bindingNavigatorSeparator1
            // 
            this.bindingNavigatorSeparator1.Name = "bindingNavigatorSeparator1";
            this.bindingNavigatorSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorMoveNextItem
            // 
            this.bindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveNextItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveNextItem.Image")));
            this.bindingNavigatorMoveNextItem.Name = "bindingNavigatorMoveNextItem";
            this.bindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveNextItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveNextItem.Text = "Enregistrement suivant";
            // 
            // bindingNavigatorMoveLastItem
            // 
            this.bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveLastItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveLastItem.Image")));
            this.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
            this.bindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveLastItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveLastItem.Text = "Dernier enregistrement";
            // 
            // bindingNavigatorSeparator2
            // 
            this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
            this.bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // bdSave
            // 
            this.bdSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bdSave.Image = global::smartManage.Desktop.Properties.Resources.Save;
            this.bdSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bdSave.Name = "bdSave";
            this.bdSave.Size = new System.Drawing.Size(23, 22);
            this.bdSave.Text = "Mise à jour enregistrement";
            // 
            // bdRefresh
            // 
            this.bdRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bdRefresh.Image = global::smartManage.Desktop.Properties.Resources.Refresh;
            this.bdRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bdRefresh.Name = "bdRefresh";
            this.bdRefresh.Size = new System.Drawing.Size(23, 22);
            this.bdRefresh.Text = "Actualiser";
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            // 
            // txtSearch
            // 
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(120, 25);
            // 
            // bdSearch
            // 
            this.bdSearch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bdSearch.Image = global::smartManage.Desktop.Properties.Resources.Search;
            this.bdSearch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bdSearch.Name = "bdSearch";
            this.bdSearch.Size = new System.Drawing.Size(23, 22);
            this.bdSearch.Text = "Rechercher";
            // 
            // bdPreview
            // 
            this.bdPreview.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bdPreview.Image = global::smartManage.Desktop.Properties.Resources.Preview;
            this.bdPreview.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bdPreview.Name = "bdPreview";
            this.bdPreview.Size = new System.Drawing.Size(23, 22);
            this.bdPreview.Text = "Afficher rapport";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Beige;
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.dgv);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(484, 297);
            this.panel1.TabIndex = 2;
            // 
            // dgv
            // 
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToDeleteRows = false;
            this.dgv.BackgroundColor = System.Drawing.Color.White;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Thistle;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Blue;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgv.Location = new System.Drawing.Point(7, 111);
            this.dgv.MultiSelect = false;
            this.dgv.Name = "dgv";
            this.dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv.Size = new System.Drawing.Size(471, 179);
            this.dgv.TabIndex = 200;
            this.dgv.SelectionChanged += new System.EventHandler(this.dgv_SelectionChanged);
            // 
            // txtDateModifie
            // 
            this.txtDateModifie.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txtDateModifie.Location = new System.Drawing.Point(316, 72);
            this.txtDateModifie.Name = "txtDateModifie";
            this.txtDateModifie.Size = new System.Drawing.Size(147, 20);
            this.txtDateModifie.TabIndex = 1;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(313, 55);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(98, 13);
            this.label17.TabIndex = 56;
            this.label17.Text = "Date modification : ";
            // 
            // txtModifieBy
            // 
            this.txtModifieBy.Location = new System.Drawing.Point(316, 29);
            this.txtModifieBy.Name = "txtModifieBy";
            this.txtModifieBy.ReadOnly = true;
            this.txtModifieBy.Size = new System.Drawing.Size(147, 20);
            this.txtModifieBy.TabIndex = 55;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(313, 12);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(68, 13);
            this.label18.TabIndex = 54;
            this.label18.Text = "Modifier par :";
            // 
            // txtDateCreate
            // 
            this.txtDateCreate.Location = new System.Drawing.Point(162, 72);
            this.txtDateCreate.Name = "txtDateCreate";
            this.txtDateCreate.ReadOnly = true;
            this.txtDateCreate.Size = new System.Drawing.Size(147, 20);
            this.txtDateCreate.TabIndex = 53;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(161, 55);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(80, 13);
            this.label19.TabIndex = 52;
            this.label19.Text = "Date création : ";
            // 
            // txtCreateBy
            // 
            this.txtCreateBy.Location = new System.Drawing.Point(162, 29);
            this.txtCreateBy.Name = "txtCreateBy";
            this.txtCreateBy.ReadOnly = true;
            this.txtCreateBy.Size = new System.Drawing.Size(147, 20);
            this.txtCreateBy.TabIndex = 51;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(161, 12);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(53, 13);
            this.label20.TabIndex = 50;
            this.label20.Text = "Créé par :";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.txtDesignation);
            this.groupBox1.Controls.Add(this.txtDateModifie);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label17);
            this.groupBox1.Controls.Add(this.txtId);
            this.groupBox1.Controls.Add(this.txtModifieBy);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Controls.Add(this.label20);
            this.groupBox1.Controls.Add(this.txtDateCreate);
            this.groupBox1.Controls.Add(this.txtCreateBy);
            this.groupBox1.Controls.Add(this.label19);
            this.groupBox1.Location = new System.Drawing.Point(8, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(470, 100);
            this.groupBox1.TabIndex = 58;
            this.groupBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 59;
            this.label1.Text = "Code :";
            // 
            // txtId
            // 
            this.txtId.Location = new System.Drawing.Point(7, 29);
            this.txtId.Name = "txtId";
            this.txtId.ReadOnly = true;
            this.txtId.Size = new System.Drawing.Size(147, 20);
            this.txtId.TabIndex = 59;
            // 
            // txtDesignation
            // 
            this.txtDesignation.Location = new System.Drawing.Point(8, 72);
            this.txtDesignation.Name = "txtDesignation";
            this.txtDesignation.Size = new System.Drawing.Size(147, 20);
            this.txtDesignation.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 61;
            this.label2.Text = "Désignation :";
            // 
            // frmCategorieMateriel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 322);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.bdNav);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "frmCategorieMateriel";
            this.Text = "Categorie des matériels";
            this.Load += new System.EventHandler(this.frmCategorieMateriel_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bdNav)).EndInit();
            this.bdNav.ResumeLayout(false);
            this.bdNav.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.BindingNavigator bdNav;
        private System.Windows.Forms.ToolStripButton bdNew;
        private System.Windows.Forms.ToolStripLabel bindingNavigatorCountItem;
        private System.Windows.Forms.ToolStripButton bdDelete;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveFirstItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMovePreviousItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator;
        private System.Windows.Forms.ToolStripTextBox bindingNavigatorPositionItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator1;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveNextItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveLastItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator2;
        private System.Windows.Forms.ToolStripButton bdSave;
        private System.Windows.Forms.ToolStripButton bdRefresh;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripTextBox txtSearch;
        private System.Windows.Forms.ToolStripButton bdSearch;
        private System.Windows.Forms.ToolStripButton bdPreview;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtDesignation;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtId;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker txtDateModifie;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtModifieBy;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox txtDateCreate;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox txtCreateBy;
        private System.Windows.Forms.Label label20;
    }
}