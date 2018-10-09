using ManageQRCode;
using smartManage.Model;
using smartManage.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Timers;
using System.Windows.Forms;

namespace smartManage.Desktop
{
    public partial class frmOrdinateur : Form, ICRUDGeneral, ICallMainForm
    {
        BindingSource bdsrc = new BindingSource();
        bool blnModifie = false;
        private clsmateriel materiel = new clsmateriel();
        int? newID = null;

        //Delegate utilisation des threads
        private delegate void LoadSomeData(string threadName);

        //Timer for automatically unload thread for update comboBox on DropDown event
        System.Timers.Timer tempsActualiseCombo = null;

        //Timer for automatically unload thread for generate QrCode
        System.Timers.Timer tempsGenerateQrCode = null;

        //Timer for automatically unload thread for SelectionChange DataGridView event
        System.Timers.Timer tempsSelectionChangeDataGrid = null;

        //Timer for automatically unload thread for RefreshData method
        System.Timers.Timer tempsRefreshData = null;

        //Timer for automatically unload thread when FormActivated event occurs in form
        System.Timers.Timer tempsActivateForm = null;

        //Timer for automatically set default cursor to form
        System.Timers.Timer tempsStopWaitCursor = null;

        //All thread for loading values
        Thread tDataGrid = null;
        Thread tSelectionChangeDataGrid = null;
        Thread tLeftCombo = null, tMiddleCombo = null, tRightCombo = null;
        Thread tActualiseComb = null;
        Thread tGenerateQrCode = null;
        Thread tStopWaitCursor = null;

        //Boolean variables for photo
        bool blnPhoto1 = false;
        bool blnPhoto2 = false;
        bool blnPhoto3 = false;

        bool firstLoad = false;

        public frmPrincipal Principal
        {
            get;
            set;
        }

        public frmOrdinateur()
        {
            InitializeComponent();
        }

        #region Methods For THREAD
        private void UnloadThreadRessource(Thread thread)
        {
            if (thread != null)
            {
                thread.Abort();
                thread = null;
            }
        }

        private void LoadLeftCombo(string threadName)
        {
            this.Cursor = Cursors.WaitCursor;

            cboCatMateriel.DataSource = clsMetier.GetInstance().getAllClscategorie_materiel();
            this.setMembersallcbo(cboCatMateriel, "Designation", "Id");
            cboNumCompte.DataSource = clsMetier.GetInstance().getAllClscompte();
            this.setMembersallcbo(cboNumCompte, "Numero", "Id");
            cboGarantie.DataSource = clsMetier.GetInstance().getAllClsgarantie();
            this.setMembersallcbo(cboGarantie, "Valeur", "Id");
            cboMarque.DataSource = clsMetier.GetInstance().getAllClsmarque();
            this.setMembersallcbo(cboMarque, "Designation", "Id");
            cboModele.DataSource = clsMetier.GetInstance().getAllClsmodele();
            this.setMembersallcbo(cboModele, "Designation", "Id");
            cboCouleur.DataSource = clsMetier.GetInstance().getAllClscouleur();
            this.setMembersallcbo(cboCouleur, "Designation", "Id");
            cboPoids.DataSource = clsMetier.GetInstance().getAllClspoids();
            this.setMembersallcbo(cboPoids, "Valeur", "Id");
            cboEtat.DataSource = clsMetier.GetInstance().getAllClsetat_materiel();
            this.setMembersallcbo(cboEtat, "Designation", "Id");

            List<ComboBox> lstCombo = new List<ComboBox>() { cboCatMateriel, cboNumCompte, cboGarantie, cboMarque, cboModele, cboCouleur, cboPoids, cboEtat};

            SetSelectedIndexComboBox(lstCombo);
        }

        private void ExecuteLeftCombo()
        {
            try
            {
                LoadSomeData leftCbo = new LoadSomeData(LoadLeftCombo);

                this.Invoke(leftCbo, "tLeftCombo");
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Erreur lors du chargement des listes déroulantes left, {0}", ex.Message), "Chargement listes déroulantes", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void LoadMiddleCombo(string threadName)
        {
            this.Cursor = Cursors.WaitCursor;

            cboTypeOrdi.DataSource = clsMetier.GetInstance().getAllClstype_ordinateur();
            this.setMembersallcbo(cboTypeOrdi, "Designation", "Id");
            cboTypeClavier.DataSource = clsMetier.GetInstance().getAllClstype_clavier();
            this.setMembersallcbo(cboTypeClavier, "Designation", "Id");
            cboTypeOS.DataSource = clsMetier.GetInstance().getAllClstype_OS();
            this.setMembersallcbo(cboTypeOS, "Designation", "Id");
            cboRAM.DataSource = clsMetier.GetInstance().getAllClsram();
            this.setMembersallcbo(cboRAM, "Valeur", "Id");
            cboProcesseur.DataSource = clsMetier.GetInstance().getAllClsprocesseur();
            this.setMembersallcbo(cboProcesseur, "Valeur", "Id");
            cboNbrCoeurProcesseur.DataSource = clsMetier.GetInstance().getAllClsnombre_coeur_processeur();
            this.setMembersallcbo(cboNbrCoeurProcesseur, "Valeur", "Id");
            cboTypeHDD.DataSource = clsMetier.GetInstance().getAllClstype_hdd();
            this.setMembersallcbo(cboTypeHDD, "Designation", "Id");
            cboCapaciteHDD.DataSource = clsMetier.GetInstance().getAllClscapacite_hdd();
            this.setMembersallcbo(cboCapaciteHDD, "Valeur", "Id");
            cboNbrHDD.DataSource = clsMetier.GetInstance().getAllClsnombre_hdd();
            this.setMembersallcbo(cboNbrHDD, "Valeur", "Id");
            cboTailleEcran.DataSource = clsMetier.GetInstance().getAllClstaille_ecran();
            this.setMembersallcbo(cboTailleEcran, "Valeur", "Id");
            cboUSB2.DataSource = clsMetier.GetInstance().getAllClsusb2();
            this.setMembersallcbo(cboUSB2, "Valeur", "Id");
            cboUSB3.DataSource = clsMetier.GetInstance().getAllClsusb3();
            this.setMembersallcbo(cboUSB3, "Valeur", "Id");

            List<ComboBox> lstCombo = new List<ComboBox>() { cboTypeOrdi, cboTypeClavier, cboTypeOS, cboRAM, cboProcesseur, cboNbrCoeurProcesseur,
                cboTypeHDD, cboCapaciteHDD, cboNbrHDD, cboTailleEcran, cboUSB2, cboUSB3 };

            SetSelectedIndexComboBox(lstCombo);
        }

        private void ExecuteMiddleCombo()
        {
            try
            {
                LoadSomeData middleCbo = new LoadSomeData(LoadMiddleCombo);

                this.Invoke(middleCbo, "tMiddleCombo");
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Erreur lors du chargement des listes déroulantes middle, {0}", ex.Message), "Chargement listes déroulantes", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void LoadRightCombo(string threadName)
        {
            this.Cursor = Cursors.WaitCursor;

            cboNbrHDMI.DataSource = clsMetier.GetInstance().getAllClshdmi();
            this.setMembersallcbo(cboNbrHDMI, "Valeur", "Id");
            cboNbrVGA.DataSource = clsMetier.GetInstance().getAllClsvga();
            this.setMembersallcbo(cboNbrVGA, "Valeur", "Id");
            cboTensionBatt.DataSource = clsMetier.GetInstance().getAllClstension_batterie();
            this.setMembersallcbo(cboTensionBatt, "Valeur", "Id");
            cboTensionAdap.DataSource = clsMetier.GetInstance().getAllClstension_adaptateur();
            this.setMembersallcbo(cboTensionAdap, "Valeur", "Id");
            cboPuissanceAdap.DataSource = clsMetier.GetInstance().getAllClspuissance_adaptateur();
            this.setMembersallcbo(cboPuissanceAdap, "Valeur", "Id");
            cboIntensiteAdap.DataSource = clsMetier.GetInstance().getAllClsintensite_adaptateur();
            this.setMembersallcbo(cboIntensiteAdap, "Valeur", "Id");

            List<ComboBox> lstCombo = new List<ComboBox>() { cboNbrHDMI, cboNbrVGA, cboTensionBatt, cboTensionAdap, cboPuissanceAdap, cboIntensiteAdap };

            SetSelectedIndexComboBox(lstCombo);
        }

        private void ExecuteRightCombo()
        {
            try
            {
                LoadSomeData rightCbo = new LoadSomeData(LoadRightCombo);

                this.Invoke(rightCbo, "tRightCombo");
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Erreur lors du chargement des listes déroulantes right, {0}", ex.Message), "Chargement listes déroulantes", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void LoadDataGrid(string threadName)
        {
            this.Cursor = Cursors.WaitCursor;

            bdsrc.DataSource = clsMetier.GetInstance().getAllClsmateriel();
            Principal.SetDataSource(bdsrc);

            dgv.DataSource = bdsrc;
        }

        private void ExecuteDataGrid()
        {
            try
            {
                LoadSomeData datagrid = new LoadSomeData(LoadDataGrid);

                this.Invoke(datagrid, "tDataGrid");
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Erreur lors du chargement de la zone d'affichage, {0}", ex.Message), "Chargement zone d'affichage", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void GenerateQrCode(string threadName)
        {
            this.Cursor = Cursors.WaitCursor;

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(string.Format("{0}-{1}", txtId.Text, ((clscategorie_materiel)cboCatMateriel.SelectedItem).Id.ToString()));
            txtIdentidiant.Text = sb.ToString();

            //Creating QrCode 
            System.Drawing.Image img = QRCodeImage.GetGenerateQRCode(txtIdentidiant.Text, "L", "", 0);//L, M ou Q
            pbQRCode.Image = img;

            //Convert PictureBox image to Base64 text
            //Save a temp image file
            string fileName = clsTools.Instance.SaveTempImage(pbQRCode);
            txtQRCode.Text = clsTools.Instance.ImageToString64_(clsTools.Instance.GetImageFromByte(fileName));

            //Remove the temp image created
            clsTools.Instance.RemoveTempImage(fileName);
        }

        private void ExecuteGenerateQrCode()
        {
            try
            {
                LoadSomeData codeQr = new LoadSomeData(GenerateQrCode);

                this.Invoke(codeQr, "tQrCode");
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Erreur lors de la génération du QrCode, {0}", ex.Message), "Génération QrCode", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void DoExecuteSelectionDataGrid(string threadName)
        {
            this.Cursor = Cursors.WaitCursor;

            BindingList();
            blnModifie = true;
            Principal.ActivateOnNewSelectionChangeDgvCommandButtons(true);

            try
            {
                //Affectation de la duree restante par rapport a la garantie de l'equipement
                int? duree = null;

                if(cboGarantie.SelectedValue != null)
                    duree = int.Parse(cboGarantie.SelectedValue.ToString());

                lblStatusGuaraty.Text = clsMetier.GetInstance().CalculateEndGuarany(duree, DateTime.Parse(txtDateAcquisition.Text));
            }
            catch
            {
                lblStatusGuaraty.Text = "Erreur de génération alerte";
            }

            //Affiche QrCode from rtfTextBox
            pbQRCode.Image = null;
            pbQRCode.Image = clsTools.Instance.LoadImage(txtQRCode.Text);
        }

        private void ExecuteSelectionDataGrid()
        {
            LoadSomeData selectDataGrid = new LoadSomeData(DoExecuteSelectionDataGrid);
            
            try
            {
                this.Invoke(selectDataGrid, "tSelectedItemDataGrid");
            }
            catch 
            {
                blnModifie = false;
                Principal.ActivateOnSelectionChangeDgvExceptionCommandButtons(false);
            }
        }

        private void DoSetDefaultCursor(string threadName)
        {
            this.Cursor = Cursors.Default;
        }

        private void ExecuteDefaultCursor()
        {
            LoadSomeData defaultCurs = new LoadSomeData(DoSetDefaultCursor);

            try
            {
                this.Invoke(defaultCurs, "DoSetDefaultCursor");
            }
            catch(Exception ex)
            {
                MessageBox.Show(string.Format("Error occur when change cursor, {0}", ex.Message), "Defaut Cursor", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        #region Actualise value in ComboBox
        private void CallActualiseComboBoxModification(string threadName)
        {
            //Actualisation des combobox si modification

            if (!string.IsNullOrEmpty(smartManage.Desktop.Properties.Settings.Default.strFormModifie))
            {
                if (smartManage.Desktop.Properties.Settings.Default.strFormModifie.Equals(FormActualisation.frmCategorieMateriel.ToString()))
                {
                    cboCatMateriel.DataSource = clsMetier.GetInstance().getAllClscategorie_materiel();
                    this.setMembersallcbo(cboCatMateriel, "Designation", "Id");
                }
                else if (smartManage.Desktop.Properties.Settings.Default.strFormModifie.Equals(FormActualisation.frmNumeroCompte.ToString()))
                {
                    cboNumCompte.DataSource = clsMetier.GetInstance().getAllClscompte();
                    this.setMembersallcbo(cboNumCompte, "Numero", "Id");
                }
                else if (smartManage.Desktop.Properties.Settings.Default.strFormModifie.Equals(FormActualisation.frmMarque.ToString()))
                {
                    cboMarque.DataSource = clsMetier.GetInstance().getAllClsmarque();
                    this.setMembersallcbo(cboMarque, "Designation", "Id");
                }
                else if (smartManage.Desktop.Properties.Settings.Default.strFormModifie.Equals(FormActualisation.frmModele.ToString()))
                {
                    cboModele.DataSource = clsMetier.GetInstance().getAllClsmodele();
                    this.setMembersallcbo(cboModele, "Designation", "Id");
                }
                else if (smartManage.Desktop.Properties.Settings.Default.strFormModifie.Equals(FormActualisation.frmCouleur.ToString()))
                {
                    cboCouleur.DataSource = clsMetier.GetInstance().getAllClscouleur();
                    this.setMembersallcbo(cboCouleur, "Designation", "Id");
                }
                else if (smartManage.Desktop.Properties.Settings.Default.strFormModifie.Equals(FormActualisation.frmPoids.ToString()))
                {
                    cboPoids.DataSource = clsMetier.GetInstance().getAllClspoids();
                    this.setMembersallcbo(cboPoids, "Valeur", "Id");
                }
                else if (smartManage.Desktop.Properties.Settings.Default.strFormModifie.Equals(FormActualisation.frmEtatMateriel.ToString()))
                {
                    cboEtat.DataSource = clsMetier.GetInstance().getAllClsetat_materiel();
                    this.setMembersallcbo(cboEtat, "Designation", "Id");
                }
                else if (smartManage.Desktop.Properties.Settings.Default.strFormModifie.Equals(FormActualisation.frmTypeOrdinateur.ToString()))
                {
                    cboTypeOrdi.DataSource = clsMetier.GetInstance().getAllClstype_ordinateur();
                    this.setMembersallcbo(cboTypeOrdi, "Designation", "Id");
                }
                else if (smartManage.Desktop.Properties.Settings.Default.strFormModifie.Equals(FormActualisation.frmTypeClavier.ToString()))
                {
                    cboTypeClavier.DataSource = clsMetier.GetInstance().getAllClstype_clavier();
                    this.setMembersallcbo(cboTypeClavier, "Designation", "Id");
                }
                else if (smartManage.Desktop.Properties.Settings.Default.strFormModifie.Equals(FormActualisation.frmTypeOS.ToString()))
                {
                    cboTypeOS.DataSource = clsMetier.GetInstance().getAllClstype_OS();
                    this.setMembersallcbo(cboTypeOS, "Designation", "Id");
                }
                else if (smartManage.Desktop.Properties.Settings.Default.strFormModifie.Equals(FormActualisation.frmGarantie.ToString()))
                {
                    cboGarantie.DataSource = clsMetier.GetInstance().getAllClsgarantie();
                    this.setMembersallcbo(cboGarantie, "Valeur", "Id");
                }
                else if (smartManage.Desktop.Properties.Settings.Default.strFormModifie.Equals(FormActualisation.frmRAM.ToString()))
                {
                    cboRAM.DataSource = clsMetier.GetInstance().getAllClsram();
                    this.setMembersallcbo(cboRAM, "Valeur", "Id");
                }
                else if (smartManage.Desktop.Properties.Settings.Default.strFormModifie.Equals(FormActualisation.frmRAM.ToString()))
                {
                    cboProcesseur.DataSource = clsMetier.GetInstance().getAllClsprocesseur();
                    this.setMembersallcbo(cboProcesseur, "Valeur", "Id");
                }
                else if (smartManage.Desktop.Properties.Settings.Default.strFormModifie.Equals(FormActualisation.frmNbrCoeurProcesseur.ToString()))
                {
                    cboNbrCoeurProcesseur.DataSource = clsMetier.GetInstance().getAllClsnombre_coeur_processeur();
                    this.setMembersallcbo(cboNbrCoeurProcesseur, "Valeur", "Id");
                }
                else if (smartManage.Desktop.Properties.Settings.Default.strFormModifie.Equals(FormActualisation.frmTypeHDD.ToString()))
                {
                    cboTypeHDD.DataSource = clsMetier.GetInstance().getAllClstype_hdd();
                    this.setMembersallcbo(cboTypeHDD, "Designation", "Id");
                }
                else if (smartManage.Desktop.Properties.Settings.Default.strFormModifie.Equals(FormActualisation.frmCapaciteHDD.ToString()))
                {
                    cboCapaciteHDD.DataSource = clsMetier.GetInstance().getAllClscapacite_hdd();
                    this.setMembersallcbo(cboCapaciteHDD, "valeur", "Id");
                }
                else if (smartManage.Desktop.Properties.Settings.Default.strFormModifie.Equals(FormActualisation.frmNbrHDD.ToString()))
                {
                    cboNbrHDD.DataSource = clsMetier.GetInstance().getAllClsnombre_hdd();
                    this.setMembersallcbo(cboNbrHDD, "valeur", "Id");
                }
                else if (smartManage.Desktop.Properties.Settings.Default.strFormModifie.Equals(FormActualisation.frmTailleEcran.ToString()))
                {
                    cboTailleEcran.DataSource = clsMetier.GetInstance().getAllClstaille_ecran();
                    this.setMembersallcbo(cboTailleEcran, "valeur", "Id");
                }
                else if (smartManage.Desktop.Properties.Settings.Default.strFormModifie.Equals(FormActualisation.frmNbrUSB2.ToString()))
                {
                    cboUSB2.DataSource = clsMetier.GetInstance().getAllClsusb2();
                    this.setMembersallcbo(cboUSB2, "valeur", "Id");
                }
                else if (smartManage.Desktop.Properties.Settings.Default.strFormModifie.Equals(FormActualisation.frmNbrUSB3.ToString()))
                {
                    cboUSB3.DataSource = clsMetier.GetInstance().getAllClsusb2();
                    this.setMembersallcbo(cboUSB3, "valeur", "Id");
                }
                else if (smartManage.Desktop.Properties.Settings.Default.strFormModifie.Equals(FormActualisation.frmNbrHDMI.ToString()))
                {
                    cboNbrHDMI.DataSource = clsMetier.GetInstance().getAllClshdmi();
                    this.setMembersallcbo(cboNbrHDMI, "valeur", "Id");
                }
                else if (smartManage.Desktop.Properties.Settings.Default.strFormModifie.Equals(FormActualisation.frmNbrVGA.ToString()))
                {
                    cboNbrVGA.DataSource = clsMetier.GetInstance().getAllClsvga();
                    this.setMembersallcbo(cboNbrVGA, "valeur", "Id");
                }
                else if (smartManage.Desktop.Properties.Settings.Default.strFormModifie.Equals(FormActualisation.frmTensionBatterie.ToString()))
                {
                    cboTensionBatt.DataSource = clsMetier.GetInstance().getAllClstension_batterie();
                    this.setMembersallcbo(cboTensionBatt, "valeur", "Id");
                }
                else if (smartManage.Desktop.Properties.Settings.Default.strFormModifie.Equals(FormActualisation.frmTensionAdaptateur.ToString()))
                {
                    cboTensionAdap.DataSource = clsMetier.GetInstance().getAllClstension_adaptateur();
                    this.setMembersallcbo(cboTensionAdap, "valeur", "Id");
                }
                else if (smartManage.Desktop.Properties.Settings.Default.strFormModifie.Equals(FormActualisation.frmPuissanceAdaptateur.ToString()))
                {
                    cboPuissanceAdap.DataSource = clsMetier.GetInstance().getAllClspuissance_adaptateur();
                    this.setMembersallcbo(cboPuissanceAdap, "valeur", "Id");
                }
                else if (smartManage.Desktop.Properties.Settings.Default.strFormModifie.Equals(FormActualisation.frmIntensiteAdaptateur.ToString()))
                {
                    cboIntensiteAdap.DataSource = clsMetier.GetInstance().getAllClsintensite_adaptateur();
                    this.setMembersallcbo(cboIntensiteAdap, "valeur", "Id");
                }
            }

            smartManage.Desktop.Properties.Settings.Default.strFormModifie = "";
        }

        private void ActualiseComboBoxModification()
        {
            try
            {
                LoadSomeData actualiseCboModifie = new LoadSomeData(CallActualiseComboBoxModification);

                this.Invoke(actualiseCboModifie, "tActualiseComb");
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Erreur lors de l'actuaisation de la liste deroulante, {0}", ex.Message), "Actuelisation liste deroulante", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void TempsActualiseCombo_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (tActualiseComb != null)
            {
                if (!tActualiseComb.IsAlive)
                {
                    tempsActualiseCombo.Enabled = false;
                    tActualiseComb.Abort();
                    tActualiseComb = null;

                    try
                    { 
                        clsTools.SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);
                    }
                    catch { }

                    ExecuteThreadStopWaitCursor();
                }
            }
        }

        private void TempsGenerateQrCode_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (tGenerateQrCode != null)
            {
                if (!tGenerateQrCode.IsAlive)
                {
                    tempsGenerateQrCode.Enabled = false;
                    tGenerateQrCode.Abort();
                    tGenerateQrCode = null;

                    try
                    {
                        clsTools.SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);
                    }
                    catch { }

                    ExecuteThreadStopWaitCursor();
                }
            }
        }

        private void TempsSelectionChangeDataGrid_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (tSelectionChangeDataGrid != null)
            {
                if (!tSelectionChangeDataGrid.IsAlive)
                {
                    tempsSelectionChangeDataGrid.Enabled = false;
                    tSelectionChangeDataGrid.Abort();
                    tSelectionChangeDataGrid = null;

                    try
                    {
                        clsTools.SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);
                    }
                    catch { }

                    ExecuteThreadStopWaitCursor();
                }
            }
        }

        private void TempsActivateForm_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (tDataGrid != null)
            {
                if (!tDataGrid.IsAlive)
                {
                    tempsActivateForm.Enabled = false;
                    tDataGrid.Abort();
                    tDataGrid = null;

                    try
                    {
                        clsTools.SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);
                    }
                    catch { }

                    ExecuteThreadStopWaitCursor();
                }
            }
        }

        private void TempsStopWaitCursor_Elapsed(object sender, ElapsedEventArgs e)
        {
            List<Thread> lstThread = new List<Thread>();

            if (tStopWaitCursor != null)
            {
                if (!tStopWaitCursor.IsAlive)
                {
                    if (tDataGrid != null)
                        lstThread.Add(tDataGrid);
                    else if (tSelectionChangeDataGrid != null)
                        lstThread.Add(tSelectionChangeDataGrid);
                    else if (tGenerateQrCode != null)
                        lstThread.Add(tGenerateQrCode);
                    else if (tLeftCombo != null)
                        lstThread.Add(tLeftCombo);
                    else if (tRightCombo != null)
                        lstThread.Add(tRightCombo);
                    else if (tMiddleCombo != null)
                        lstThread.Add(tMiddleCombo);
                    else if (tActualiseComb != null)
                        lstThread.Add(tActualiseComb);

                    bool[] tb = { };
                    int count = 1;

                    foreach (Thread t in lstThread)
                    {
                        if (!t.IsAlive)
                            count++;
                    }

                    if (count == tb.Length)
                    {
                        tempsStopWaitCursor.Enabled = false;
                        tStopWaitCursor.Abort();
                        tStopWaitCursor = null;
                    }

                    try
                    {
                        clsTools.SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);
                    }
                    catch { }
                }
            }
        }

        private void ExecuteThreadStopWaitCursor()
        {
            tempsStopWaitCursor.Enabled = true;
            tempsStopWaitCursor.Elapsed += TempsStopWaitCursor_Elapsed;

            if (tStopWaitCursor == null)
            {
                tStopWaitCursor = new Thread(new ThreadStart(ExecuteDefaultCursor));
                tStopWaitCursor.Start();
            }
            else
            {
                tStopWaitCursor.Abort();
                tStopWaitCursor = null;

                tStopWaitCursor = new Thread(new ThreadStart(ExecuteDefaultCursor));
                tStopWaitCursor.Start();
            }
        }

        private void TempsRefreshData_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (tLeftCombo != null || tMiddleCombo != null || tRightCombo != null || tDataGrid != null)
            {
                if (!tRightCombo.IsAlive)
                {
                    if (!tMiddleCombo.IsAlive)
                    {
                        if (!tDataGrid.IsAlive)
                        {
                            if (!tLeftCombo.IsAlive)
                            {
                                tempsRefreshData.Enabled = false;

                                tLeftCombo.Abort();
                                tLeftCombo = null;

                                tDataGrid.Abort();
                                tDataGrid = null;

                                tMiddleCombo.Abort();
                                tMiddleCombo = null;

                                tRightCombo.Abort();
                                tRightCombo = null;

                                try
                                {
                                    clsTools.SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);
                                }
                                catch { }

                                tempsStopWaitCursor.Enabled = true;
                                tempsStopWaitCursor.Elapsed += TempsStopWaitCursor_Elapsed;
                            }
                        }
                    }
                }
            }
        }

        private void DoActualiseDropDown()
        {
            tempsActualiseCombo.Enabled = true;
            tempsActualiseCombo.Elapsed += TempsActualiseCombo_Elapsed;

            if (tActualiseComb == null)
            {
                tActualiseComb = new Thread(new ThreadStart(ActualiseComboBoxModification));
                tActualiseComb.Start();
            }
        }
        #endregion  

        #endregion

        #region FOR GENERATE QRCode
        public string SaveTempImage(System.Windows.Forms.PictureBox pbox)
        {
            string filename = Environment.GetEnvironmentVariables()["TEMP"].ToString() + @"\" + "fTmp" + DateTime.Now.Millisecond.ToString() + ".png";

            using (System.IO.FileStream fs = new System.IO.FileStream(filename, System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite))
            {
                System.Drawing.Imaging.ImageFormat imageFormat = System.Drawing.Imaging.ImageFormat.Png;
                pbox.Image.Save(fs, imageFormat);
                fs.Close();
            }

            return filename;
        }

        public void RemoveTempImage(string fileName)
        {
            if (System.IO.File.Exists(fileName))
            {
                System.IO.File.Delete(fileName);
            }
        }
        #endregion

        #region FOR BINDING
        private void setMembersallcbo(ComboBox cbo, string displayMember, string valueMember)
        {
            cbo.DisplayMember = displayMember;
            cbo.ValueMember = valueMember;
        }

        private void SetBindingControls(Control ctr, string ctr_prop, object objsrce, string obj_prop)
        {
            ctr.DataBindings.Clear();
            ctr.DataBindings.Add(ctr_prop, objsrce, obj_prop, true, DataSourceUpdateMode.OnPropertyChanged);
        }

        private void Bs_Parse1(object sender, ConvertEventArgs e)
        {
            try
            {
                if (e.Value == null) e = null;
                else e.Value = (clsTools.Instance.ImageToString64(pbPhoto1.Image));
            }
            catch { }
        }

        private void Bs_Parse2(object sender, ConvertEventArgs e)
        {
            try
            {
                if (e.Value == null) e = null;
                else e.Value = (clsTools.Instance.ImageToString64(pbPhoto2.Image));
            }
            catch { }
        }

        private void Bs_Parse3(object sender, ConvertEventArgs e)
        {
            try
            {
                if (e.Value == null) e = null;
                else e.Value = (clsTools.Instance.ImageToString64(pbPhoto3.Image));
            }
            catch { }
        }
        void binding_Format1(object sender, ConvertEventArgs e)
        {
            try
            {
                pbPhoto1.Tag = null;
                if (e.DesiredType != typeof(System.Drawing.Image) || e.Value.ToString() == e.DesiredType.FullName || e.Value.ToString() == e.DesiredType.Name) return;
                if (e.Value.ToString() == "System.Drawing.Bitmap") return;
                if (e.Value == null || e.Value.ToString() == "")
                {
                    pbPhoto1.Tag = "1";
                    pbPhoto1.Image = null;
                }
                else
                {
                    string imagestr = e.Value.ToString();
                    e.Value = (clsTools.Instance.LoadImage(e.Value.ToString()));
                }
            }
            catch { }
        }

        void binding_Format2(object sender, ConvertEventArgs e)
        {
            try
            {
                pbPhoto2.Tag = null;
                if (e.DesiredType != typeof(System.Drawing.Image) || e.Value.ToString() == e.DesiredType.FullName || e.Value.ToString() == e.DesiredType.Name) return;
                if (e.Value.ToString() == "System.Drawing.Bitmap") return;
                if (e.Value == null || e.Value.ToString() == "")
                {
                    pbPhoto2.Tag = "1";
                    pbPhoto2.Image = null;
                }
                else
                {
                    string imagestr = e.Value.ToString();
                    e.Value = (clsTools.Instance.LoadImage(e.Value.ToString()));
                }
            }
            catch { }
        }

        void binding_Format3(object sender, ConvertEventArgs e)
        {
            try
            {
                pbPhoto3.Tag = null;
                if (e.DesiredType != typeof(System.Drawing.Image) || e.Value.ToString() == e.DesiredType.FullName || e.Value.ToString() == e.DesiredType.Name) return;
                if (e.Value.ToString() == "System.Drawing.Bitmap") return;
                if (e.Value == null || e.Value.ToString() == "")
                {
                    pbPhoto3.Tag = "1";
                    pbPhoto3.Image = null;
                }
                else
                {
                    string imagestr = e.Value.ToString();
                    e.Value = (clsTools.Instance.LoadImage(e.Value.ToString()));
                }
            }
            catch { }
        }

        private void bingImg1(PictureBox pb, Object src, string ctrProp, string obj)
        {
            pb.DataBindings.Clear();
            Binding binding = new Binding(ctrProp, src, obj, true, DataSourceUpdateMode.OnPropertyChanged);
            binding.Parse += new ConvertEventHandler(Bs_Parse1);
            binding.Format += new ConvertEventHandler(binding_Format1);
            pb.DataBindings.Add(binding);
        }

        private void bingImg2(PictureBox pb, Object src, string ctrProp, string obj)
        {
            pb.DataBindings.Clear();
            Binding binding = new Binding(ctrProp, src, obj, true, DataSourceUpdateMode.OnPropertyChanged);
            binding.Parse += new ConvertEventHandler(Bs_Parse2);
            binding.Format += new ConvertEventHandler(binding_Format2);
            pb.DataBindings.Add(binding);
        }

        private void bingImg3(PictureBox pb, Object src, string ctrProp, string obj)
        {
            pb.DataBindings.Clear();
            Binding binding = new Binding(ctrProp, src, obj, true, DataSourceUpdateMode.OnPropertyChanged);
            binding.Parse += new ConvertEventHandler(Bs_Parse3);
            binding.Format += new ConvertEventHandler(binding_Format3);
            pb.DataBindings.Add(binding);
        }

        private void BindingCls()
        {
            SetBindingControls(txtId, "Text", materiel, "Id");
            SetBindingControls(txtIdentidiant, "Text", materiel, "Code_str");
            SetBindingControls(cboCatMateriel, "SelectedValue", materiel, "Id_categorie_materiel");
            SetBindingControls(cboNumCompte, "SelectedValue", materiel, "Id_compte");
            SetBindingControls(txtQRCode, "Text", materiel, "Qrcode");
            SetBindingControls(txtDateAcquisition, "Text", materiel, "Date_acquisition");
            SetBindingControls(cboGarantie, "SelectedValue", materiel, "Id_garantie");
            SetBindingControls(cboMarque, "SelectedValue", materiel, "Id_marque");
            SetBindingControls(cboModele, "SelectedValue", materiel, "Id_modele");
            SetBindingControls(cboCouleur, "SelectedValue", materiel, "Id_couleur");
            SetBindingControls(cboPoids, "SelectedValue", materiel, "Id_poids");
            SetBindingControls(cboEtat, "SelectedValue", materiel, "Id_etat_materiel");
            bingImg1(pbPhoto1, materiel, "Image", "Photo1");
            bingImg2(pbPhoto2, materiel, "Image", "Photo2");
            bingImg3(pbPhoto3, materiel, "Image", "Photo3");
            SetBindingControls(txtMAC1, "Text", materiel, "Mac_adresse1");
            SetBindingControls(txtMAC2, "Text", materiel, "Mac_adresse2");
            SetBindingControls(txtCommentaire, "Text", materiel, "Commentaire");
            SetBindingControls(txtCreateBy, "Text", materiel, "User_created");
            SetBindingControls(txtDateCreate, "Text", materiel, "Date_created");
            SetBindingControls(txtModifieBy, "Text", materiel, "User_modified");
            SetBindingControls(txtDateModifie, "Text", materiel, "Date_modified");
            
            //Partie pour ordinateur
            SetBindingControls(cboTypeOrdi, "SelectedValue", materiel, "Id_type_ordinateur");
            SetBindingControls(cboTypeClavier, "SelectedValue", materiel, "Id_type_clavier");
            SetBindingControls(cboTypeOS, "SelectedValue", materiel, "Id_os");
            SetBindingControls(cboRAM, "SelectedValue", materiel, "Id_ram");
            SetBindingControls(cboProcesseur, "SelectedValue", materiel, "Id_processeur");
            SetBindingControls(cboNbrCoeurProcesseur, "SelectedValue", materiel, "Id_nombre_coeur_processeur");
            SetBindingControls(cboTypeHDD, "SelectedValue", materiel, "Id_type_hdd");
            SetBindingControls(cboCapaciteHDD, "SelectedValue", materiel, "Id_capacite_hdd");
            SetBindingControls(cboNbrHDD, "SelectedValue", materiel, "Id_nombre_hdd");
            SetBindingControls(cboTailleEcran, "SelectedValue", materiel, "Id_taille_ecran");
            SetBindingControls(cboUSB2, "SelectedValue", materiel, "Id_usb2");
            SetBindingControls(cboUSB3, "SelectedValue", materiel, "Id_usb3");
            SetBindingControls(cboNbrHDMI, "SelectedValue", materiel, "Id_hdmi");
            SetBindingControls(cboNbrVGA, "SelectedValue", materiel, "Id_vga");
            SetBindingControls(cboTensionBatt, "SelectedValue", materiel, "Id_tension_batterie");
            SetBindingControls(cboTensionAdap, "SelectedValue", materiel, "Id_tension_adaptateur");
            SetBindingControls(cboPuissanceAdap, "SelectedValue", materiel, "Id_puissance_adaptateur");
            SetBindingControls(cboIntensiteAdap, "SelectedValue", materiel, "Id_intensite_adaptateur");
            SetBindingControls(txtNumeroCle, "Text", materiel, "Numero_cle");
        }

        private void BindingList()
        {
            SetBindingControls(txtId, "Text", bdsrc, "Id");
            SetBindingControls(txtIdentidiant, "Text", bdsrc, "Code_str");
            SetBindingControls(cboCatMateriel, "SelectedValue", bdsrc, "Id_categorie_materiel");
            SetBindingControls(cboNumCompte, "SelectedValue", bdsrc, "Id_compte");
            SetBindingControls(txtQRCode, "Text", bdsrc, "Qrcode");
            SetBindingControls(txtDateAcquisition, "Text", bdsrc, "Date_acquisition");
            SetBindingControls(cboGarantie, "SelectedValue", bdsrc, "Id_garantie");
            SetBindingControls(cboMarque, "SelectedValue", bdsrc, "Id_marque");
            SetBindingControls(cboModele, "SelectedValue", bdsrc, "Id_modele");
            SetBindingControls(cboCouleur, "SelectedValue", bdsrc, "Id_couleur");
            SetBindingControls(cboPoids, "SelectedValue", bdsrc, "Id_poids");
            SetBindingControls(cboEtat, "SelectedValue", bdsrc, "Id_etat_materiel");
            bingImg1(pbPhoto1, bdsrc, "Image", "Photo1");
            bingImg2(pbPhoto2, bdsrc, "Image", "Photo2");
            bingImg3(pbPhoto3, bdsrc, "Image", "Photo3");
            SetBindingControls(txtMAC1, "Text", bdsrc, "Mac_adresse1");
            SetBindingControls(txtMAC2, "Text", bdsrc, "Mac_adresse2");
            SetBindingControls(txtCommentaire, "Text", bdsrc, "Commentaire");
            SetBindingControls(txtCreateBy, "Text", bdsrc, "User_created");
            SetBindingControls(txtDateCreate, "Text", bdsrc, "Date_created");
            SetBindingControls(txtModifieBy, "Text", bdsrc, "User_modified");
            SetBindingControls(txtDateModifie, "Text", bdsrc, "Date_modified");

            //Partie pour ordinateur
            SetBindingControls(cboTypeOrdi, "SelectedValue", bdsrc, "Id_type_ordinateur");
            SetBindingControls(cboTypeClavier, "SelectedValue", bdsrc, "Id_type_clavier");
            SetBindingControls(cboTypeOS, "SelectedValue", bdsrc, "Id_os");
            SetBindingControls(cboRAM, "SelectedValue", bdsrc, "Id_ram");
            SetBindingControls(cboProcesseur, "SelectedValue", bdsrc, "Id_processeur");
            SetBindingControls(cboNbrCoeurProcesseur, "SelectedValue", bdsrc, "Id_nombre_coeur_processeur");
            SetBindingControls(cboTypeHDD, "SelectedValue", bdsrc, "Id_type_hdd");
            SetBindingControls(cboCapaciteHDD, "SelectedValue", bdsrc, "Id_capacite_hdd");
            SetBindingControls(cboNbrHDD, "SelectedValue", bdsrc, "Id_nombre_hdd");
            SetBindingControls(cboTailleEcran, "SelectedValue", bdsrc, "Id_taille_ecran");
            SetBindingControls(cboUSB2, "SelectedValue", bdsrc, "Id_usb2");
            SetBindingControls(cboUSB3, "SelectedValue", bdsrc, "Id_usb3");
            SetBindingControls(cboNbrHDMI, "SelectedValue", bdsrc, "Id_hdmi");
            SetBindingControls(cboNbrVGA, "SelectedValue", bdsrc, "Id_vga");
            SetBindingControls(cboTensionBatt, "SelectedValue", bdsrc, "Id_tension_batterie");
            SetBindingControls(cboTensionAdap, "SelectedValue", bdsrc, "Id_tension_adaptateur");
            SetBindingControls(cboPuissanceAdap, "SelectedValue", bdsrc, "Id_puissance_adaptateur");
            SetBindingControls(cboIntensiteAdap, "SelectedValue", bdsrc, "Id_intensite_adaptateur");
            SetBindingControls(txtNumeroCle, "Text", bdsrc, "Numero_cle");
        }
        #endregion

        private void frmOrdinateur_Load(object sender, EventArgs e)
        {
            //Initialise timers
            tempsActualiseCombo = new System.Timers.Timer();
            tempsActualiseCombo.Interval = 100;

            tempsGenerateQrCode = new System.Timers.Timer();
            tempsGenerateQrCode.Interval = 100;

            tempsSelectionChangeDataGrid = new System.Timers.Timer();
            tempsSelectionChangeDataGrid.Interval = 100;

            tempsRefreshData = new System.Timers.Timer();
            tempsRefreshData.Interval = 100;

            tempsActivateForm = new System.Timers.Timer();
            tempsActivateForm.Interval = 100;

            tempsStopWaitCursor = new System.Timers.Timer();
            tempsStopWaitCursor.Interval = 50;

            //Affecte MenuStrip
            pbPhoto1.ContextMenuStrip = ctxMenuPhoto1;
            pbPhoto2.ContextMenuStrip = ctxMenuPhoto2;
            pbPhoto3.ContextMenuStrip = ctxMenuPhoto3;

            //executed in many independant thread
            RefreshData();
        }

        private void SetSelectedIndexComboBox(List<ComboBox> cbo)
        {
            foreach (ComboBox cmb in cbo)
                if (cmb.Items.Count > 0)
                    cmb.SelectedIndex = 0;
        }

        private void RefreshData()
        {
            tempsRefreshData.Enabled = true;
            tempsRefreshData.Elapsed += TempsRefreshData_Elapsed;

            if (tLeftCombo == null)
            {
                tLeftCombo = new Thread(new ThreadStart(ExecuteLeftCombo));
                tLeftCombo.Start();

                tMiddleCombo = new Thread(new ThreadStart(ExecuteMiddleCombo));
                tMiddleCombo.Start();

                tRightCombo = new Thread(new ThreadStart(ExecuteRightCombo));
                tRightCombo.Start();

                tDataGrid = new Thread(new ThreadStart(ExecuteDataGrid));
                tDataGrid.Start();
            }
            else
            {
                tLeftCombo.Abort();
                tLeftCombo = null;
            }

            if (bdsrc.Count == 0)
            {
                Principal.ActivateOnLoadCommandButtons(false);
            }
        }

        private void frmOrdinateur_Activated(object sender, EventArgs e)
        {
            Principal.SetValuesLabel(Properties.Settings.Default.UserConnected, "Gestion des ordinateurs");
            Principal.SetCurrentICRUDChildForm(this);

            if (firstLoad)
            {
                tempsActivateForm.Enabled = true;
                tempsActivateForm.Elapsed += TempsActivateForm_Elapsed;

                if (tDataGrid == null)
                {
                    tDataGrid = new Thread(new ThreadStart(ExecuteDataGrid));
                    tDataGrid.Start();
                }
            }

            firstLoad = true;
        }

        private void frmOrdinateur_FormClosed(object sender, FormClosedEventArgs e)
        {
            Principal.SetValuesLabel(Properties.Settings.Default.UserConnected, "Attente d'une action de l'utilisateur");

            //Reinitialise all Thread
            try
            {
                this.UnloadThreadRessource(tDataGrid);
                this.UnloadThreadRessource(tSelectionChangeDataGrid);
                this.UnloadThreadRessource(tGenerateQrCode);
                this.UnloadThreadRessource(tLeftCombo);
                this.UnloadThreadRessource(tRightCombo);
                this.UnloadThreadRessource(tMiddleCombo);
                this.UnloadThreadRessource(tActualiseComb);
                this.UnloadThreadRessource(tStopWaitCursor);
            }
            catch { }

            Principal.ApplyDefaultStatusBar(Principal, Properties.Settings.Default.UserConnected);
        }

        public void New()
        {
            try
            {
                //Initialise object class
                materiel = new clsmateriel();

                Principal.ActivateOnNewCommandButtons(true);
                blnModifie = false;

                BindingCls();

                //Set the new ID
                if (newID == null)
                    newID = clsMetier.GetInstance().GenerateLastID("materiel");
                txtId.Text = newID.ToString();
                txtCreateBy.Text = smartManage.Desktop.Properties.Settings.Default.UserConnected;
                txtDateCreate.Text = DateTime.Now.ToString();
                lblStatusGuaraty.Text = "";
            }
            catch (Exception)
            {
                Principal.ActivateOnNewCommandButtons(false);
            }
        }

        public void Search(string criteria)
        {
            try
            {
                if (string.IsNullOrEmpty(criteria))
                {
                    this.RefreshRec();
                    return;
                }
                else
                {
                    List<clsmateriel> lstItemSearch = new List<clsmateriel>();
                    lstItemSearch = clsMetier.GetInstance().getAllClsmateriel(criteria);

                    dgv.DataSource = lstItemSearch;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la recherche, " + ex.Message, "Recherche élément", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public void Save()
        {
            try
            {
                if (!blnModifie)
                {
                    int record = materiel.inserts();
                    MessageBox.Show("Enregistrement éffectué : " + record + " Affecté", "Enregistrement", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    UpdateRec();
                }

                newID = null;
                RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Echec de la mise à jour, " + ex.Message, "Mise à jour", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public void UpdateRec()
        {
            ((clsmateriel)bdsrc.Current).User_modified = smartManage.Desktop.Properties.Settings.Default.UserConnected;
            ((clsmateriel)bdsrc.Current).Date_modified = DateTime.Now;

            clsmateriel mat = new clsmateriel();
            mat = ((clsmateriel)bdsrc.Current);
            if (blnPhoto1)
            {
                mat.Photo1 = materiel.Photo1;
                blnPhoto1 = false;
            }
            if (blnPhoto2)
            {
                mat.Photo2 = materiel.Photo2;
                blnPhoto2 = false;
            }
            if (blnPhoto3)
            {
                mat.Photo3 = materiel.Photo3;
                blnPhoto3 = false;
            }

            int record = materiel.update(mat);
            MessageBox.Show("Modification éffectuée : " + record + " Modifié", "Modification", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void Delete()
        {
            try
            {
                if (blnModifie)
                {
                    DialogResult dr = MessageBox.Show("Voulez-vous supprimer cet enrgistrement ?", "Suppression enregistrement", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    int record = 0;

                    if (dr == DialogResult.Yes)
                    {
                        record = materiel.delete(((clsmateriel)bdsrc.Current));
                        MessageBox.Show("Suppression éffectuée : " + record + " Supprimé", "Suppression enregistrement", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        newID = null;
                    }
                    else
                        MessageBox.Show("Aucune suppression éffectuée : " + record + " Supprimé", "Suppression enregistrement", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

                RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Echec de la suppression, " + ex.Message, "Suppression enregistrement", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public void Preview()
        {
            try
            {
                frmReport frm = new frmReport();
                frm.Text = "Rapports pour ordinateurs";
                frm.MdiParent = Principal;
                //frm.setData(factory.getAllSexe_Dt(), @"D:\appStockMS\appStock.Desktop\reports\rptListSexe.rdlc");
                frm.Icon = this.Icon;
                frm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Echec de la mise à jour, " + ex.Message, "Mise à jour", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public void RefreshRec()
        {
            try
            {
                RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de l'actualisation, " + ex.Message, "Actualisation des données", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void dgv_SelectionChanged(object sender, EventArgs e)
        {
            tempsSelectionChangeDataGrid.Enabled = true;
            tempsSelectionChangeDataGrid.Elapsed += TempsSelectionChangeDataGrid_Elapsed;

            //Executed in thread
            if(tSelectionChangeDataGrid == null)
            {
                tSelectionChangeDataGrid = new Thread(new ThreadStart(ExecuteSelectionDataGrid));
                tSelectionChangeDataGrid.Start();
            }
        }

        private void lblAddCategorieMat_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmCategorieMateriel frm = new frmCategorieMateriel();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void lblAddNumCompte_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmNumeroCompte frm = new frmNumeroCompte();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void cboCatMateriel_DropDown(object sender, EventArgs e)
        {
            DoActualiseDropDown();
        }

        private void cboNumCompte_DropDown(object sender, EventArgs e)
        {
            DoActualiseDropDown();
        }

        private void lblAddMarque_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmMarque frm = new frmMarque();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void lblAddModele_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmModele frm = new frmModele();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void cboModele_DropDown(object sender, EventArgs e)
        {
            DoActualiseDropDown();
        }

        private void cboCouleur_DropDown(object sender, EventArgs e)
        {
            DoActualiseDropDown();
        }

        private void cboPoids_DropDown(object sender, EventArgs e)
        {
            DoActualiseDropDown();
        }

        private void cboEtat_DropDown(object sender, EventArgs e)
        {
            DoActualiseDropDown();
        }

        private void cboTypeOrdi_DropDown(object sender, EventArgs e)
        {
            DoActualiseDropDown();
        }

        private void cboTypeClavier_DropDown(object sender, EventArgs e)
        {
            DoActualiseDropDown();
        }

        private void cboTypeOS_DropDown(object sender, EventArgs e)
        {
            DoActualiseDropDown();
        }

        private void cboNbrCoeurProcesseur_DropDown(object sender, EventArgs e)
        {
            DoActualiseDropDown();
        }

        private void cboNbrHDD_DropDown(object sender, EventArgs e)
        {
            DoActualiseDropDown();
        }

        private void cboCapaciteHDD_DropDown(object sender, EventArgs e)
        {
            DoActualiseDropDown();
        }

        private void cboIndicePC_DropDown(object sender, EventArgs e)
        {
            DoActualiseDropDown();
        }

        private void cboUSB2_DropDown(object sender, EventArgs e)
        {
            DoActualiseDropDown();
        }

        private void cboUSB3_DropDown(object sender, EventArgs e)
        {
            DoActualiseDropDown();
        }

        private void cboNbrHDMI_DropDown(object sender, EventArgs e)
        {
            DoActualiseDropDown();
        }

        private void cboNbrVGA_DropDown(object sender, EventArgs e)
        {
            DoActualiseDropDown();
        }

        private void cboTensionBatt_DropDown(object sender, EventArgs e)
        {
            DoActualiseDropDown();
        }

        private void cboTensionAdap_DropDown(object sender, EventArgs e)
        {
            DoActualiseDropDown();
        }

        private void cboPuissanceAdap_DropDown(object sender, EventArgs e)
        {
            DoActualiseDropDown();
        }

        private void cboIntensiteAdap_DropDown(object sender, EventArgs e)
        {
            DoActualiseDropDown();
        }

        private void lblAddCouleur_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmCouleur frm = new frmCouleur();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void lblAddPoids_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmPoids frm = new frmPoids();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void lblEtatMatriel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmEtatMateriel frm = new frmEtatMateriel();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void lblAddPC_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmModele frm = new frmModele();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void lblAddTypeClavier_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmModele frm = new frmModele();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void lblAddOS_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmModele frm = new frmModele();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void cboCatMateriel_Leave(object sender, EventArgs e)
        {
            tempsGenerateQrCode.Enabled = true;
            tempsGenerateQrCode.Elapsed += TempsGenerateQrCode_Elapsed;

            //Executed in new thread
            if(tGenerateQrCode == null)
            {
                tGenerateQrCode = new Thread(new ThreadStart(ExecuteGenerateQrCode));
                tGenerateQrCode.Start();
            }
        }

        private void cboMarque_DropDown(object sender, EventArgs e)
        {
            DoActualiseDropDown();
        }

        private void lblPhoto1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                LoadPicture1();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Echec de chargement de la photo, " + ex.Message, "Chargement de la photo1", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void LoadPicture1()
        {
            OpenFileDialog open;
            DialogResult result;
            LoadPicture(out open, out result);

            if (result == DialogResult.OK)
            {
                if (clsTools.Instance.LimiteImageSize(open.FileName, 3000000, 150, 150))
                {
                    pbPhoto1.Load(open.FileName);
                    //string img1 = clsTools.Instance.ImageToString64(open.FileName);
                    //string img2 = clsTools.Instance.ImageToString64_(open.FileName);
                    //int compare = string.Compare(img1, img2);
                    //if(compare == 0)
                    //    MessageBox.Show("Equal, tail = {0}", img1.Length.ToString());
                    //else if(compare > 0)
                    //    MessageBox.Show(string.Format("img1 > img2, taille1 = {0} et taille2 = {1}", img1.Length.ToString(), img2.Length.ToString()));
                    //else
                    //    MessageBox.Show(string.Format("img2 > img1, taille2 = {0} et taille1 = {1}", img2.Length.ToString(), img1.Length.ToString()));

                    blnPhoto1 = true;
                    materiel.Photo1 = clsTools.Instance.ImageToString64(open.FileName);
                }
            }
        }

        private static void LoadPicture(out OpenFileDialog open, out DialogResult result)
        {
            open = new OpenFileDialog();
            open.Title = "Sélection d'une photo";
            open.Filter = "PNG Files(*.png)|*.png|JPG Files(*.jpg)|*.jpg|JPEG Files(*.jpeg)|*.jpeg|TIFF Files(*.tiff)|*.tiff";
            result = open.ShowDialog();
        }

        private void lblPhoto2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                LoadPicture2();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Echec de chargement de la photo, " + ex.Message, "Chargement de la photo2", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void LoadPicture2()
        {
            OpenFileDialog open;
            DialogResult result;
            LoadPicture(out open, out result);

            if (result == DialogResult.OK)
            {
                pbPhoto2.Load(open.FileName);
                blnPhoto2 = true;
                materiel.Photo2 = clsTools.Instance.ImageToString64(open.FileName);
            }
        }

        private void lblPhoto3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                LoadPicture3();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Echec de chargement de la photo, " + ex.Message, "Chargement de la photo3", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void LoadPicture3()
        {
            OpenFileDialog open;
            DialogResult result;
            LoadPicture(out open, out result);

            if (result == DialogResult.OK)
            {
                pbPhoto3.Load(open.FileName);
                blnPhoto3 = true;
                materiel.Photo3 = clsTools.Instance.ImageToString64(open.FileName);
            }
        }

        private void lblAddGuaratie_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmGarantie frm = new frmGarantie();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void cboGarantie_DropDown(object sender, EventArgs e)
        {
            DoActualiseDropDown();
        }

        private void cboRAM_DropDown(object sender, EventArgs e)
        {
            DoActualiseDropDown();
        }

        private void cboProcesseur_DropDown(object sender, EventArgs e)
        {
            DoActualiseDropDown();
        }

        private void cboTailleEcran_DropDown(object sender, EventArgs e)
        {
            DoActualiseDropDown();
        }

        private void lblAddRAM_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmRAM frm = new frmRAM();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void lblAddProcessor_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmProcesseur frm = new frmProcesseur();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void lblAddCorProcessor_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmNbrCoeurProcesseur frm = new frmNbrCoeurProcesseur();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void lblAddTypeHDD_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmTypeHDD frm = new frmTypeHDD();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void lblAddCapacityHDD_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmCapaciteHDD frm = new frmCapaciteHDD();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void lblAddNbrHDD_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmNbrHDD frm = new frmNbrHDD();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void lblAddScreen_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmTailleEcran frm = new frmTailleEcran();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void lblAddUSB2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmNbrUSB2 frm = new frmNbrUSB2();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void lblAddUSB3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmNbrUSB3 frm = new frmNbrUSB3();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void lblAddHDMI_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmNbrHDMI frm = new frmNbrHDMI();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void lblAddVGA_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmNbrVGA frm = new frmNbrVGA();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void lblAddUBatterie_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmTensionBatterie frm = new frmTensionBatterie();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void lblAddUAdapt_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmTensionAdaptateur frm = new frmTensionAdaptateur();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void lblAddPAdapt_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmPuissanceAdaptateur frm = new frmPuissanceAdaptateur();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void lblAddIAdapt_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmIntensiteAdaptateur frm = new frmIntensiteAdaptateur();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void smnCtxPhoto1_Click(object sender, EventArgs e)
        {
            try
            {
                LoadPicture1();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Echec de chargement de la photo, " + ex.Message, "Chargement de la photo1", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void smnCtxPhoto2_Click(object sender, EventArgs e)
        {
            try
            {
                LoadPicture2();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Echec de chargement de la photo, " + ex.Message, "Chargement de la photo2", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
