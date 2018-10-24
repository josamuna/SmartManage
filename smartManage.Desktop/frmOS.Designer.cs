﻿namespace smartManage.Desktop
{
    partial class frmOS
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmOS));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.bdNav = new System.Windows.Forms.BindingNavigator(this.components);
            this.bdNew = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorCountItem = new System.Windows.Forms.ToolStripLabel();
            this.bindingNavigatorMoveFirstItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMovePreviousItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorPositionItem = new System.Windows.Forms.ToolStripTextBox();
            this.bindingNavigatorSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorMoveNextItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveLastItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.bdDelete = new System.Windows.Forms.ToolStripButton();
            this.bdSave = new System.Windows.Forms.ToolStripButton();
            this.bdRefresh = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.txtSearch = new System.Windows.Forms.ToolStripTextBox();
            this.bdSearch = new System.Windows.Forms.ToolStripButton();
            this.bdPreview = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblAddArchitectureOS = new System.Windows.Forms.LinkLabel();
            this.label4 = new System.Windows.Forms.Label();
            this.cboArchitectureOS = new System.Windows.Forms.ComboBox();
            this.lblAddTypeOS = new System.Windows.Forms.LinkLabel();
            this.label3 = new System.Windows.Forms.Label();
            this.cboTypeOS = new System.Windows.Forms.ComboBox();
            this.txtDateModifie = new System.Windows.Forms.TextBox();
            this.txtDesignation = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.txtId = new System.Windows.Forms.TextBox();
            this.txtModifieBy = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.txtDateCreate = new System.Windows.Forms.TextBox();
            this.txtCreateBy = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDesignation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colId_architecture_OS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colId_type_OS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUser_created = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDate_created = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUser_modified = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDate_modified = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.bdNav)).BeginInit();
            this.bdNav.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // bdNav
            // 
            this.bdNav.AddNewItem = this.bdNew;
            this.bdNav.CountItem = this.bindingNavigatorCountItem;
            this.bdNav.DeleteItem = null;
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
            this.bdNav.Size = new System.Drawing.Size(676, 25);
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
            this.bdNew.Click += new System.EventHandler(this.bdNew_Click);
            // 
            // bindingNavigatorCountItem
            // 
            this.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
            this.bindingNavigatorCountItem.Size = new System.Drawing.Size(35, 22);
            this.bindingNavigatorCountItem.Text = "of {0}";
            this.bindingNavigatorCountItem.ToolTipText = "Total number of items";
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
            // bdDelete
            // 
            this.bdDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bdDelete.Image = global::smartManage.Desktop.Properties.Resources.delete;
            this.bdDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bdDelete.Name = "bdDelete";
            this.bdDelete.Size = new System.Drawing.Size(23, 22);
            this.bdDelete.Text = "Supprimer enregistrement";
            this.bdDelete.Click += new System.EventHandler(this.bdDelete_Click);
            // 
            // bdSave
            // 
            this.bdSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bdSave.Image = global::smartManage.Desktop.Properties.Resources.Save;
            this.bdSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bdSave.Name = "bdSave";
            this.bdSave.Size = new System.Drawing.Size(23, 22);
            this.bdSave.Text = "Mise à jour enregistrement";
            this.bdSave.Click += new System.EventHandler(this.bdSave_Click);
            // 
            // bdRefresh
            // 
            this.bdRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bdRefresh.Image = global::smartManage.Desktop.Properties.Resources.Refresh;
            this.bdRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bdRefresh.Name = "bdRefresh";
            this.bdRefresh.Size = new System.Drawing.Size(23, 22);
            this.bdRefresh.Text = "Actualiser";
            this.bdRefresh.Click += new System.EventHandler(this.bdRefresh_Click);
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
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // bdSearch
            // 
            this.bdSearch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bdSearch.Image = global::smartManage.Desktop.Properties.Resources.Search;
            this.bdSearch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bdSearch.Name = "bdSearch";
            this.bdSearch.Size = new System.Drawing.Size(23, 22);
            this.bdSearch.Text = "Rechercher";
            this.bdSearch.Click += new System.EventHandler(this.bdSearch_Click);
            // 
            // bdPreview
            // 
            this.bdPreview.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bdPreview.Enabled = false;
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
            this.panel1.Size = new System.Drawing.Size(676, 347);
            this.panel1.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.lblAddArchitectureOS);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.cboArchitectureOS);
            this.groupBox1.Controls.Add(this.lblAddTypeOS);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cboTypeOS);
            this.groupBox1.Controls.Add(this.txtDateModifie);
            this.groupBox1.Controls.Add(this.txtDesignation);
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
            this.groupBox1.Size = new System.Drawing.Size(660, 104);
            this.groupBox1.TabIndex = 58;
            this.groupBox1.TabStop = false;
            // 
            // lblAddArchitectureOS
            // 
            this.lblAddArchitectureOS.AutoSize = true;
            this.lblAddArchitectureOS.Location = new System.Drawing.Point(622, 76);
            this.lblAddArchitectureOS.Name = "lblAddArchitectureOS";
            this.lblAddArchitectureOS.Size = new System.Drawing.Size(29, 13);
            this.lblAddArchitectureOS.TabIndex = 4;
            this.lblAddArchitectureOS.TabStop = true;
            this.lblAddArchitectureOS.Text = "New";
            this.lblAddArchitectureOS.VisitedLinkColor = System.Drawing.Color.Blue;
            this.lblAddArchitectureOS.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblAddArchitectureOS_LinkClicked);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(452, 55);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(122, 13);
            this.label4.TabIndex = 67;
            this.label4.Text = "Architecture Syst. Expl. :";
            // 
            // cboArchitectureOS
            // 
            this.cboArchitectureOS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboArchitectureOS.FormattingEnabled = true;
            this.cboArchitectureOS.Location = new System.Drawing.Point(453, 72);
            this.cboArchitectureOS.Name = "cboArchitectureOS";
            this.cboArchitectureOS.Size = new System.Drawing.Size(166, 21);
            this.cboArchitectureOS.TabIndex = 3;
            this.cboArchitectureOS.DropDown += new System.EventHandler(this.cboArchitectureOS_DropDown);
            // 
            // lblAddTypeOS
            // 
            this.lblAddTypeOS.AutoSize = true;
            this.lblAddTypeOS.Location = new System.Drawing.Point(422, 76);
            this.lblAddTypeOS.Name = "lblAddTypeOS";
            this.lblAddTypeOS.Size = new System.Drawing.Size(29, 13);
            this.lblAddTypeOS.TabIndex = 2;
            this.lblAddTypeOS.TabStop = true;
            this.lblAddTypeOS.Text = "New";
            this.lblAddTypeOS.VisitedLinkColor = System.Drawing.Color.Blue;
            this.lblAddTypeOS.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblAddTypeOS_LinkClicked);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(285, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 13);
            this.label3.TabIndex = 64;
            this.label3.Text = "Type Syst. Expl. :";
            // 
            // cboTypeOS
            // 
            this.cboTypeOS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTypeOS.FormattingEnabled = true;
            this.cboTypeOS.Location = new System.Drawing.Point(286, 72);
            this.cboTypeOS.Name = "cboTypeOS";
            this.cboTypeOS.Size = new System.Drawing.Size(132, 21);
            this.cboTypeOS.TabIndex = 1;
            this.cboTypeOS.DropDown += new System.EventHandler(this.cboTypeOS_DropDown);
            // 
            // txtDateModifie
            // 
            this.txtDateModifie.Location = new System.Drawing.Point(286, 29);
            this.txtDateModifie.Name = "txtDateModifie";
            this.txtDateModifie.ReadOnly = true;
            this.txtDateModifie.Size = new System.Drawing.Size(132, 20);
            this.txtDateModifie.TabIndex = 62;
            this.txtDateModifie.TabStop = false;
            // 
            // txtDesignation
            // 
            this.txtDesignation.Location = new System.Drawing.Point(455, 29);
            this.txtDesignation.Name = "txtDesignation";
            this.txtDesignation.Size = new System.Drawing.Size(196, 20);
            this.txtDesignation.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(454, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 61;
            this.label2.Text = "Désignation :";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(283, 12);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(98, 13);
            this.label17.TabIndex = 56;
            this.label17.Text = "Date modification : ";
            // 
            // txtId
            // 
            this.txtId.Location = new System.Drawing.Point(7, 29);
            this.txtId.Name = "txtId";
            this.txtId.ReadOnly = true;
            this.txtId.Size = new System.Drawing.Size(132, 20);
            this.txtId.TabIndex = 59;
            this.txtId.TabStop = false;
            // 
            // txtModifieBy
            // 
            this.txtModifieBy.Location = new System.Drawing.Point(147, 72);
            this.txtModifieBy.Name = "txtModifieBy";
            this.txtModifieBy.ReadOnly = true;
            this.txtModifieBy.Size = new System.Drawing.Size(132, 20);
            this.txtModifieBy.TabIndex = 55;
            this.txtModifieBy.TabStop = false;
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
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(147, 55);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(68, 13);
            this.label18.TabIndex = 54;
            this.label18.Text = "Modifier par :";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(146, 12);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(53, 13);
            this.label20.TabIndex = 50;
            this.label20.Text = "Créé par :";
            // 
            // txtDateCreate
            // 
            this.txtDateCreate.Location = new System.Drawing.Point(7, 72);
            this.txtDateCreate.Name = "txtDateCreate";
            this.txtDateCreate.ReadOnly = true;
            this.txtDateCreate.Size = new System.Drawing.Size(132, 20);
            this.txtDateCreate.TabIndex = 53;
            this.txtDateCreate.TabStop = false;
            // 
            // txtCreateBy
            // 
            this.txtCreateBy.Location = new System.Drawing.Point(147, 29);
            this.txtCreateBy.Name = "txtCreateBy";
            this.txtCreateBy.ReadOnly = true;
            this.txtCreateBy.Size = new System.Drawing.Size(132, 20);
            this.txtCreateBy.TabIndex = 51;
            this.txtCreateBy.TabStop = false;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(6, 55);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(80, 13);
            this.label19.TabIndex = 52;
            this.label19.Text = "Date création : ";
            // 
            // dgv
            // 
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToDeleteRows = false;
            this.dgv.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgv.BackgroundColor = System.Drawing.Color.White;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colId,
            this.colDesignation,
            this.colId_architecture_OS,
            this.colId_type_OS,
            this.colUser_created,
            this.colDate_created,
            this.colUser_modified,
            this.colDate_modified});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Thistle;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Blue;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgv.Location = new System.Drawing.Point(7, 115);
            this.dgv.MultiSelect = false;
            this.dgv.Name = "dgv";
            this.dgv.RowHeadersVisible = false;
            this.dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv.Size = new System.Drawing.Size(662, 224);
            this.dgv.TabIndex = 200;
            this.dgv.TabStop = false;
            this.dgv.SelectionChanged += new System.EventHandler(this.dgv_SelectionChanged);
            // 
            // colId
            // 
            this.colId.DataPropertyName = "Id";
            this.colId.HeaderText = "Id";
            this.colId.Name = "colId";
            this.colId.Visible = false;
            // 
            // colDesignation
            // 
            this.colDesignation.DataPropertyName = "Designation";
            this.colDesignation.HeaderText = "Désignation";
            this.colDesignation.Name = "colDesignation";
            this.colDesignation.Width = 240;
            // 
            // colId_architecture_OS
            // 
            this.colId_architecture_OS.DataPropertyName = "Id_architecture_OS";
            this.colId_architecture_OS.HeaderText = "Id_architecture_OS";
            this.colId_architecture_OS.Name = "colId_architecture_OS";
            this.colId_architecture_OS.Visible = false;
            // 
            // colId_type_OS
            // 
            this.colId_type_OS.DataPropertyName = "Id_type_OS";
            this.colId_type_OS.HeaderText = "Id_type_OS";
            this.colId_type_OS.Name = "colId_type_OS";
            this.colId_type_OS.Visible = false;
            // 
            // colUser_created
            // 
            this.colUser_created.DataPropertyName = "User_created";
            this.colUser_created.HeaderText = "User created";
            this.colUser_created.Name = "colUser_created";
            // 
            // colDate_created
            // 
            this.colDate_created.DataPropertyName = "Date_created";
            this.colDate_created.HeaderText = "Date created";
            this.colDate_created.Name = "colDate_created";
            // 
            // colUser_modified
            // 
            this.colUser_modified.DataPropertyName = "User_modified";
            this.colUser_modified.HeaderText = "User modified";
            this.colUser_modified.Name = "colUser_modified";
            // 
            // colDate_modified
            // 
            this.colDate_modified.DataPropertyName = "Date_modified";
            this.colDate_modified.HeaderText = "Date modified";
            this.colDate_modified.Name = "colDate_modified";
            // 
            // frmOS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(676, 372);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.bdNav);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmOS";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Système d\'Exploitation";
            this.Load += new System.EventHandler(this.frmOS_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bdNav)).EndInit();
            this.bdNav.ResumeLayout(false);
            this.bdNav.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.BindingNavigator bdNav;
        private System.Windows.Forms.ToolStripButton bdNew;
        private System.Windows.Forms.ToolStripLabel bindingNavigatorCountItem;
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
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtModifieBy;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox txtDateCreate;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox txtCreateBy;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox txtDateModifie;
        private System.Windows.Forms.ToolStripButton bdDelete;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboTypeOS;
        private System.Windows.Forms.LinkLabel lblAddTypeOS;
        private System.Windows.Forms.LinkLabel lblAddArchitectureOS;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboArchitectureOS;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDesignation;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId_architecture_OS;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId_type_OS;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUser_created;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDate_created;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUser_modified;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDate_modified;
    }
}