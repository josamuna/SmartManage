namespace smartManage.Desktop
{
    partial class frmPrincipal
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPrincipal));
            this.nmStripMenu = new System.Windows.Forms.MenuStrip();
            this.fichierToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.smConnection = new System.Windows.Forms.ToolStripMenuItem();
            this.smDeconnection = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.smCloseAllForms = new System.Windows.Forms.ToolStripMenuItem();
            this.smQuit = new System.Windows.Forms.ToolStripMenuItem();
            this.donnéesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.matérielToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ssmAmplifie = new System.Windows.Forms.ToolStripMenuItem();
            this.ssmAP = new System.Windows.Forms.ToolStripMenuItem();
            this.ssmEmetteur = new System.Windows.Forms.ToolStripMenuItem();
            this.ssmPrinter = new System.Windows.Forms.ToolStripMenuItem();
            this.ssmComputer = new System.Windows.Forms.ToolStripMenuItem();
            this.ssmRouter = new System.Windows.Forms.ToolStripMenuItem();
            this.ssmProjector = new System.Windows.Forms.ToolStripMenuItem();
            this.ssmSwitch = new System.Windows.Forms.ToolStripMenuItem();
            this.smPerson = new System.Windows.Forms.ToolStripMenuItem();
            this.smSignataire = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.affectationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ssmAffectationPerson = new System.Windows.Forms.ToolStripMenuItem();
            this.ssmAffectationMaterials = new System.Windows.Forms.ToolStripMenuItem();
            this.rapportsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.smRptMateriels = new System.Windows.Forms.ToolStripMenuItem();
            this.smRptPerson = new System.Windows.Forms.ToolStripMenuItem();
            this.situationAffectationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ssmRptAffectationPerson = new System.Windows.Forms.ToolStripMenuItem();
            this.ssmRptAffectationMateriels = new System.Windows.Forms.ToolStripMenuItem();
            this.outilsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.smManageUsers = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.smRadiusServer = new System.Windows.Forms.ToolStripMenuItem();
            this.ssmRadiusServerAdmin = new System.Windows.Forms.ToolStripMenuItem();
            this.sssmVueDataAdministration = new System.Windows.Forms.ToolStripMenuItem();
            this.ssmRadiusServerStudent = new System.Windows.Forms.ToolStripMenuItem();
            this.sssmVueDataStudent = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.ssmParamServer = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.smRestoreBackup = new System.Windows.Forms.ToolStripMenuItem();
            this.aideToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.smContent = new System.Windows.Forms.ToolStripMenuItem();
            this.smAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.lblUserConnected = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblStatusApplication = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblDate = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.bdNav = new System.Windows.Forms.BindingNavigator(this.components);
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
            this.bdNew = new System.Windows.Forms.ToolStripButton();
            this.bdSave = new System.Windows.Forms.ToolStripButton();
            this.bdRefresh = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.txtSearch = new System.Windows.Forms.ToolStripTextBox();
            this.bdSearch = new System.Windows.Forms.ToolStripButton();
            this.bdPreview = new System.Windows.Forms.ToolStripButton();
            this.nmStripMenu.SuspendLayout();
            this.statusBar.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bdNav)).BeginInit();
            this.bdNav.SuspendLayout();
            this.SuspendLayout();
            // 
            // nmStripMenu
            // 
            this.nmStripMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fichierToolStripMenuItem,
            this.donnéesToolStripMenuItem,
            this.rapportsToolStripMenuItem,
            this.outilsToolStripMenuItem,
            this.aideToolStripMenuItem});
            this.nmStripMenu.Location = new System.Drawing.Point(0, 0);
            this.nmStripMenu.Name = "nmStripMenu";
            this.nmStripMenu.Size = new System.Drawing.Size(918, 24);
            this.nmStripMenu.TabIndex = 1;
            this.nmStripMenu.Text = "menuStrip1";
            // 
            // fichierToolStripMenuItem
            // 
            this.fichierToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.smConnection,
            this.smDeconnection,
            this.toolStripSeparator3,
            this.smCloseAllForms,
            this.smQuit});
            this.fichierToolStripMenuItem.Name = "fichierToolStripMenuItem";
            this.fichierToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.fichierToolStripMenuItem.Text = "&Fichier";
            // 
            // smConnection
            // 
            this.smConnection.Name = "smConnection";
            this.smConnection.Size = new System.Drawing.Size(143, 22);
            this.smConnection.Text = "Conne&xion";
            this.smConnection.Click += new System.EventHandler(this.smConnection_Click);
            // 
            // smDeconnection
            // 
            this.smDeconnection.Name = "smDeconnection";
            this.smDeconnection.Size = new System.Drawing.Size(143, 22);
            this.smDeconnection.Text = "Déconnexion";
            this.smDeconnection.Click += new System.EventHandler(this.smDeconnection_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(140, 6);
            // 
            // smCloseAllForms
            // 
            this.smCloseAllForms.Name = "smCloseAllForms";
            this.smCloseAllForms.Size = new System.Drawing.Size(143, 22);
            this.smCloseAllForms.Text = "&Fermer tous";
            this.smCloseAllForms.Click += new System.EventHandler(this.smCloseAllForms_Click);
            // 
            // smQuit
            // 
            this.smQuit.Name = "smQuit";
            this.smQuit.Size = new System.Drawing.Size(143, 22);
            this.smQuit.Text = "&Quitter";
            this.smQuit.Click += new System.EventHandler(this.smQuit_Click);
            // 
            // donnéesToolStripMenuItem
            // 
            this.donnéesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.matérielToolStripMenuItem,
            this.smPerson,
            this.smSignataire,
            this.toolStripSeparator4,
            this.affectationToolStripMenuItem});
            this.donnéesToolStripMenuItem.Name = "donnéesToolStripMenuItem";
            this.donnéesToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.donnéesToolStripMenuItem.Text = "D&onnées";
            // 
            // matérielToolStripMenuItem
            // 
            this.matérielToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ssmAmplifie,
            this.ssmAP,
            this.ssmEmetteur,
            this.ssmPrinter,
            this.ssmComputer,
            this.ssmRouter,
            this.ssmProjector,
            this.ssmSwitch});
            this.matérielToolStripMenuItem.Name = "matérielToolStripMenuItem";
            this.matérielToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.matérielToolStripMenuItem.Text = "Matériel";
            // 
            // ssmAmplifie
            // 
            this.ssmAmplifie.Name = "ssmAmplifie";
            this.ssmAmplifie.Size = new System.Drawing.Size(189, 22);
            this.ssmAmplifie.Text = "Amplificateur";
            // 
            // ssmAP
            // 
            this.ssmAP.Name = "ssmAP";
            this.ssmAP.Size = new System.Drawing.Size(189, 22);
            this.ssmAP.Text = "Accès Point";
            // 
            // ssmEmetteur
            // 
            this.ssmEmetteur.Name = "ssmEmetteur";
            this.ssmEmetteur.Size = new System.Drawing.Size(189, 22);
            this.ssmEmetteur.Text = "Emetteur";
            this.ssmEmetteur.Click += new System.EventHandler(this.ssmEmetteur_Click);
            // 
            // ssmPrinter
            // 
            this.ssmPrinter.Name = "ssmPrinter";
            this.ssmPrinter.Size = new System.Drawing.Size(189, 22);
            this.ssmPrinter.Text = "Imprimante";
            this.ssmPrinter.Click += new System.EventHandler(this.ssmPrinter_Click);
            // 
            // ssmComputer
            // 
            this.ssmComputer.Name = "ssmComputer";
            this.ssmComputer.Size = new System.Drawing.Size(189, 22);
            this.ssmComputer.Text = "Ordinateur";
            this.ssmComputer.Click += new System.EventHandler(this.ssmComputer_Click);
            // 
            // ssmRouter
            // 
            this.ssmRouter.Name = "ssmRouter";
            this.ssmRouter.Size = new System.Drawing.Size(189, 22);
            this.ssmRouter.Text = "Routeur-Acces Point";
            // 
            // ssmProjector
            // 
            this.ssmProjector.Name = "ssmProjector";
            this.ssmProjector.Size = new System.Drawing.Size(189, 22);
            this.ssmProjector.Text = "Retroprojecteurs";
            // 
            // ssmSwitch
            // 
            this.ssmSwitch.Name = "ssmSwitch";
            this.ssmSwitch.Size = new System.Drawing.Size(189, 22);
            this.ssmSwitch.Text = "Switch-Commutateur";
            // 
            // smPerson
            // 
            this.smPerson.Name = "smPerson";
            this.smPerson.Size = new System.Drawing.Size(152, 22);
            this.smPerson.Text = "Personne";
            // 
            // smSignataire
            // 
            this.smSignataire.Name = "smSignataire";
            this.smSignataire.Size = new System.Drawing.Size(152, 22);
            this.smSignataire.Text = "Signataire";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(149, 6);
            // 
            // affectationToolStripMenuItem
            // 
            this.affectationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ssmAffectationPerson,
            this.ssmAffectationMaterials});
            this.affectationToolStripMenuItem.Name = "affectationToolStripMenuItem";
            this.affectationToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.affectationToolStripMenuItem.Text = "Affectation";
            // 
            // ssmAffectationPerson
            // 
            this.ssmAffectationPerson.Name = "ssmAffectationPerson";
            this.ssmAffectationPerson.Size = new System.Drawing.Size(185, 22);
            this.ssmAffectationPerson.Text = "Affectation personne";
            // 
            // ssmAffectationMaterials
            // 
            this.ssmAffectationMaterials.Name = "ssmAffectationMaterials";
            this.ssmAffectationMaterials.Size = new System.Drawing.Size(185, 22);
            this.ssmAffectationMaterials.Text = "Affectation matériel";
            // 
            // rapportsToolStripMenuItem
            // 
            this.rapportsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.smRptMateriels,
            this.smRptPerson,
            this.situationAffectationToolStripMenuItem});
            this.rapportsToolStripMenuItem.Name = "rapportsToolStripMenuItem";
            this.rapportsToolStripMenuItem.Size = new System.Drawing.Size(66, 20);
            this.rapportsToolStripMenuItem.Text = "&Rapports";
            // 
            // smRptMateriels
            // 
            this.smRptMateriels.Name = "smRptMateriels";
            this.smRptMateriels.Size = new System.Drawing.Size(181, 22);
            this.smRptMateriels.Text = "Matériels";
            // 
            // smRptPerson
            // 
            this.smRptPerson.Name = "smRptPerson";
            this.smRptPerson.Size = new System.Drawing.Size(181, 22);
            this.smRptPerson.Text = "Personne";
            // 
            // situationAffectationToolStripMenuItem
            // 
            this.situationAffectationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ssmRptAffectationPerson,
            this.ssmRptAffectationMateriels});
            this.situationAffectationToolStripMenuItem.Name = "situationAffectationToolStripMenuItem";
            this.situationAffectationToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.situationAffectationToolStripMenuItem.Text = "Situation affectation";
            // 
            // ssmRptAffectationPerson
            // 
            this.ssmRptAffectationPerson.Name = "ssmRptAffectationPerson";
            this.ssmRptAffectationPerson.Size = new System.Drawing.Size(126, 22);
            this.ssmRptAffectationPerson.Text = "Personnel";
            // 
            // ssmRptAffectationMateriels
            // 
            this.ssmRptAffectationMateriels.Name = "ssmRptAffectationMateriels";
            this.ssmRptAffectationMateriels.Size = new System.Drawing.Size(126, 22);
            this.ssmRptAffectationMateriels.Text = "Matériels";
            // 
            // outilsToolStripMenuItem
            // 
            this.outilsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.smManageUsers,
            this.toolStripSeparator1,
            this.smRadiusServer,
            this.toolStripSeparator2,
            this.smRestoreBackup});
            this.outilsToolStripMenuItem.Name = "outilsToolStripMenuItem";
            this.outilsToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.outilsToolStripMenuItem.Text = "O&utils";
            // 
            // smManageUsers
            // 
            this.smManageUsers.Name = "smManageUsers";
            this.smManageUsers.Size = new System.Drawing.Size(336, 22);
            this.smManageUsers.Text = "Gestion des utilisateurs";
            this.smManageUsers.Click += new System.EventHandler(this.smManageUsers_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(333, 6);
            // 
            // smRadiusServer
            // 
            this.smRadiusServer.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ssmRadiusServerAdmin,
            this.ssmRadiusServerStudent,
            this.toolStripSeparator5,
            this.ssmParamServer});
            this.smRadiusServer.Name = "smRadiusServer";
            this.smRadiusServer.Size = new System.Drawing.Size(336, 22);
            this.smRadiusServer.Text = "Accès aux serveurs Radius";
            // 
            // ssmRadiusServerAdmin
            // 
            this.ssmRadiusServerAdmin.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sssmVueDataAdministration});
            this.ssmRadiusServerAdmin.Name = "ssmRadiusServerAdmin";
            this.ssmRadiusServerAdmin.Size = new System.Drawing.Size(230, 22);
            this.ssmRadiusServerAdmin.Text = "Administration";
            // 
            // sssmVueDataAdministration
            // 
            this.sssmVueDataAdministration.Name = "sssmVueDataAdministration";
            this.sssmVueDataAdministration.Size = new System.Drawing.Size(163, 22);
            this.sssmVueDataAdministration.Text = "Vue des données";
            // 
            // ssmRadiusServerStudent
            // 
            this.ssmRadiusServerStudent.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sssmVueDataStudent});
            this.ssmRadiusServerStudent.Name = "ssmRadiusServerStudent";
            this.ssmRadiusServerStudent.Size = new System.Drawing.Size(230, 22);
            this.ssmRadiusServerStudent.Text = "Etudiant";
            // 
            // sssmVueDataStudent
            // 
            this.sssmVueDataStudent.Name = "sssmVueDataStudent";
            this.sssmVueDataStudent.Size = new System.Drawing.Size(163, 22);
            this.sssmVueDataStudent.Text = "Vue des données";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(227, 6);
            // 
            // ssmParamServer
            // 
            this.ssmParamServer.Name = "ssmParamServer";
            this.ssmParamServer.Size = new System.Drawing.Size(230, 22);
            this.ssmParamServer.Text = "Parametrage accès au serveur";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(333, 6);
            // 
            // smRestoreBackup
            // 
            this.smRestoreBackup.Name = "smRestoreBackup";
            this.smRestoreBackup.Size = new System.Drawing.Size(336, 22);
            this.smRestoreBackup.Text = "Sauvegarde et Restauration de la Base de données";
            // 
            // aideToolStripMenuItem
            // 
            this.aideToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.smContent,
            this.smAbout});
            this.aideToolStripMenuItem.Name = "aideToolStripMenuItem";
            this.aideToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.aideToolStripMenuItem.Text = "&Aide";
            // 
            // smContent
            // 
            this.smContent.Name = "smContent";
            this.smContent.Size = new System.Drawing.Size(131, 22);
            this.smContent.Text = "Contenu";
            // 
            // smAbout
            // 
            this.smAbout.Name = "smAbout";
            this.smAbout.Size = new System.Drawing.Size(131, 22);
            this.smAbout.Text = "A Propos...";
            // 
            // statusBar
            // 
            this.statusBar.BackColor = System.Drawing.Color.LemonChiffon;
            this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblUserConnected,
            this.lblStatusApplication,
            this.lblDate});
            this.statusBar.Location = new System.Drawing.Point(0, 332);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(918, 22);
            this.statusBar.TabIndex = 2;
            this.statusBar.Text = "statusStrip1";
            // 
            // lblUserConnected
            // 
            this.lblUserConnected.Name = "lblUserConnected";
            this.lblUserConnected.Size = new System.Drawing.Size(392, 17);
            this.lblUserConnected.Spring = true;
            this.lblUserConnected.Text = "toolStripStatusLabel1";
            this.lblUserConnected.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblUserConnected.ToolTipText = "Information de l\'utilisateur";
            // 
            // lblStatusApplication
            // 
            this.lblStatusApplication.Name = "lblStatusApplication";
            this.lblStatusApplication.Size = new System.Drawing.Size(392, 17);
            this.lblStatusApplication.Spring = true;
            this.lblStatusApplication.Text = "toolStripStatusLabel1";
            this.lblStatusApplication.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblStatusApplication.ToolTipText = "Status de l\'application";
            // 
            // lblDate
            // 
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(118, 17);
            this.lblDate.Text = "toolStripStatusLabel1";
            this.lblDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblDate.ToolTipText = "Date du jour";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.bdNav);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(918, 36);
            this.panel1.TabIndex = 4;
            // 
            // bdNav
            // 
            this.bdNav.AddNewItem = null;
            this.bdNav.CountItem = this.bindingNavigatorCountItem;
            this.bdNav.DeleteItem = this.bdDelete;
            this.bdNav.Dock = System.Windows.Forms.DockStyle.Fill;
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
            this.bdNav.Size = new System.Drawing.Size(918, 36);
            this.bdNav.TabIndex = 0;
            this.bdNav.Text = "bindingNavigator1";
            // 
            // bindingNavigatorCountItem
            // 
            this.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
            this.bindingNavigatorCountItem.Size = new System.Drawing.Size(35, 33);
            this.bindingNavigatorCountItem.Text = "of {0}";
            this.bindingNavigatorCountItem.ToolTipText = "Total number of items";
            // 
            // bdDelete
            // 
            this.bdDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bdDelete.Image = ((System.Drawing.Image)(resources.GetObject("bdDelete.Image")));
            this.bdDelete.Name = "bdDelete";
            this.bdDelete.RightToLeftAutoMirrorImage = true;
            this.bdDelete.Size = new System.Drawing.Size(23, 33);
            this.bdDelete.Text = "Supprimer enregistrement";
            this.bdDelete.Click += new System.EventHandler(this.bdDelete_Click);
            // 
            // bindingNavigatorMoveFirstItem
            // 
            this.bindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveFirstItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveFirstItem.Image")));
            this.bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
            this.bindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveFirstItem.Size = new System.Drawing.Size(23, 33);
            this.bindingNavigatorMoveFirstItem.Text = "Premier enregistrement";
            // 
            // bindingNavigatorMovePreviousItem
            // 
            this.bindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMovePreviousItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMovePreviousItem.Image")));
            this.bindingNavigatorMovePreviousItem.Name = "bindingNavigatorMovePreviousItem";
            this.bindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMovePreviousItem.Size = new System.Drawing.Size(23, 33);
            this.bindingNavigatorMovePreviousItem.Text = "Enregistrement précédent";
            // 
            // bindingNavigatorSeparator
            // 
            this.bindingNavigatorSeparator.Name = "bindingNavigatorSeparator";
            this.bindingNavigatorSeparator.Size = new System.Drawing.Size(6, 36);
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
            this.bindingNavigatorSeparator1.Size = new System.Drawing.Size(6, 36);
            // 
            // bindingNavigatorMoveNextItem
            // 
            this.bindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveNextItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveNextItem.Image")));
            this.bindingNavigatorMoveNextItem.Name = "bindingNavigatorMoveNextItem";
            this.bindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveNextItem.Size = new System.Drawing.Size(23, 33);
            this.bindingNavigatorMoveNextItem.Text = "Enregistrement suivant";
            // 
            // bindingNavigatorMoveLastItem
            // 
            this.bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveLastItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveLastItem.Image")));
            this.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
            this.bindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveLastItem.Size = new System.Drawing.Size(23, 33);
            this.bindingNavigatorMoveLastItem.Text = "Dernier enregistrement";
            // 
            // bindingNavigatorSeparator2
            // 
            this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
            this.bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 36);
            // 
            // bdNew
            // 
            this.bdNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bdNew.Image = ((System.Drawing.Image)(resources.GetObject("bdNew.Image")));
            this.bdNew.Name = "bdNew";
            this.bdNew.RightToLeftAutoMirrorImage = true;
            this.bdNew.Size = new System.Drawing.Size(23, 33);
            this.bdNew.Text = "Nouvel enregistrement";
            this.bdNew.Click += new System.EventHandler(this.bdNew_Click);
            // 
            // bdSave
            // 
            this.bdSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bdSave.Image = global::smartManage.Desktop.Properties.Resources.Save;
            this.bdSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bdSave.Name = "bdSave";
            this.bdSave.Size = new System.Drawing.Size(23, 33);
            this.bdSave.Text = "Mise à jour enregistrement";
            this.bdSave.Click += new System.EventHandler(this.bdSave_Click);
            // 
            // bdRefresh
            // 
            this.bdRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bdRefresh.Image = global::smartManage.Desktop.Properties.Resources.Refresh;
            this.bdRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bdRefresh.Name = "bdRefresh";
            this.bdRefresh.Size = new System.Drawing.Size(23, 33);
            this.bdRefresh.Text = "Actualiser";
            this.bdRefresh.Click += new System.EventHandler(this.bdRefresh_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 36);
            // 
            // txtSearch
            // 
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(250, 36);
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // bdSearch
            // 
            this.bdSearch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bdSearch.Image = global::smartManage.Desktop.Properties.Resources.Search;
            this.bdSearch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bdSearch.Name = "bdSearch";
            this.bdSearch.Size = new System.Drawing.Size(23, 33);
            this.bdSearch.Text = "Rechercher";
            this.bdSearch.Click += new System.EventHandler(this.bdSearch_Click);
            // 
            // bdPreview
            // 
            this.bdPreview.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bdPreview.Image = global::smartManage.Desktop.Properties.Resources.Preview;
            this.bdPreview.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bdPreview.Name = "bdPreview";
            this.bdPreview.Size = new System.Drawing.Size(23, 33);
            this.bdPreview.Text = "Afficher rapport";
            this.bdPreview.Click += new System.EventHandler(this.bdPreview_Click);
            // 
            // frmPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(918, 354);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.nmStripMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.nmStripMenu;
            this.Name = "frmPrincipal";
            this.Text = "Smart Manage votre appui à la bonne gestion";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmPrincipal_FormClosed);
            this.Load += new System.EventHandler(this.frmPrincipal_Load);
            this.MdiChildActivate += new System.EventHandler(this.frmPrincipal_MdiChildActivate);
            this.nmStripMenu.ResumeLayout(false);
            this.nmStripMenu.PerformLayout();
            this.statusBar.ResumeLayout(false);
            this.statusBar.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bdNav)).EndInit();
            this.bdNav.ResumeLayout(false);
            this.bdNav.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip nmStripMenu;
        private System.Windows.Forms.ToolStripMenuItem fichierToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem smConnection;
        private System.Windows.Forms.ToolStripMenuItem smDeconnection;
        private System.Windows.Forms.ToolStripMenuItem smCloseAllForms;
        private System.Windows.Forms.ToolStripMenuItem donnéesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem matérielToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ssmAmplifie;
        private System.Windows.Forms.ToolStripMenuItem ssmAP;
        private System.Windows.Forms.ToolStripMenuItem ssmEmetteur;
        private System.Windows.Forms.ToolStripMenuItem ssmPrinter;
        private System.Windows.Forms.ToolStripMenuItem ssmComputer;
        private System.Windows.Forms.ToolStripMenuItem ssmRouter;
        private System.Windows.Forms.ToolStripMenuItem ssmProjector;
        private System.Windows.Forms.ToolStripMenuItem ssmSwitch;
        private System.Windows.Forms.ToolStripMenuItem smPerson;
        private System.Windows.Forms.ToolStripMenuItem smSignataire;
        private System.Windows.Forms.ToolStripMenuItem affectationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ssmAffectationPerson;
        private System.Windows.Forms.ToolStripMenuItem ssmAffectationMaterials;
        private System.Windows.Forms.ToolStripMenuItem rapportsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem smRptMateriels;
        private System.Windows.Forms.ToolStripMenuItem smRptPerson;
        private System.Windows.Forms.ToolStripMenuItem situationAffectationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ssmRptAffectationPerson;
        private System.Windows.Forms.ToolStripMenuItem ssmRptAffectationMateriels;
        private System.Windows.Forms.ToolStripMenuItem outilsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem smManageUsers;
        private System.Windows.Forms.ToolStripMenuItem smRadiusServer;
        private System.Windows.Forms.ToolStripMenuItem ssmRadiusServerAdmin;
        private System.Windows.Forms.ToolStripMenuItem sssmVueDataAdministration;
        private System.Windows.Forms.ToolStripMenuItem ssmRadiusServerStudent;
        private System.Windows.Forms.ToolStripMenuItem sssmVueDataStudent;
        private System.Windows.Forms.ToolStripMenuItem smRestoreBackup;
        private System.Windows.Forms.ToolStripMenuItem aideToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem smContent;
        private System.Windows.Forms.ToolStripMenuItem smAbout;
        private System.Windows.Forms.StatusStrip statusBar;
        private System.Windows.Forms.ToolStripStatusLabel lblUserConnected;
        private System.Windows.Forms.ToolStripStatusLabel lblDate;
        private System.Windows.Forms.ToolStripStatusLabel lblStatusApplication;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem ssmParamServer;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.BindingNavigator bdNav;
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
        private System.Windows.Forms.ToolStripTextBox txtSearch;
        private System.Windows.Forms.ToolStripButton bdSave;
        private System.Windows.Forms.ToolStripButton bdRefresh;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripButton bdSearch;
        private System.Windows.Forms.ToolStripButton bdPreview;
        private System.Windows.Forms.ToolStripMenuItem smQuit;
        private System.Windows.Forms.ToolStripButton bdNew;
    }
}

