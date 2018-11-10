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
    public partial class frmAccessPoint : Form, ICRUDGeneral, ICallMainForm
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
        Thread tLeftCombo = null, tMiddleCombo = null;
        Thread tActualiseComb = null;
        Thread tGenerateQrCode = null;
        Thread tStopWaitCursor = null;

        //ariable that contain Byte image for QrCode
        Byte[] tmpQrCode = null;

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

        public frmAccessPoint()
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
            try
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

                List<ComboBox> lstCombo = new List<ComboBox>() { cboCatMateriel, cboNumCompte, cboGarantie, cboMarque, cboModele, cboCouleur, cboPoids, cboEtat };

                SetSelectedIndexComboBox(lstCombo);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Erreur lors du chargement des listes déroulantes de gauche, {0}", ex.Message), "Chargement des listes déroulantes", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
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
            try
            {
                this.Cursor = Cursors.WaitCursor;

                cboTypeAccessPoint.DataSource = clsMetier.GetInstance().getAllClstype_AP();
                this.setMembersallcbo(cboTypeAccessPoint, "Designation", "Id");
                cboTensionAlimentation.DataSource = clsMetier.GetInstance().getAllClstension_alimentation();
                this.setMembersallcbo(cboTensionAlimentation, "Valeur", "Id");
                cboPuissance.DataSource = clsMetier.GetInstance().getAllClspuissance();
                this.setMembersallcbo(cboPuissance, "Valeur", "Id");
                cboIntensite.DataSource = clsMetier.GetInstance().getAllClsintensite();
                this.setMembersallcbo(cboIntensite, "Valeur", "Id");
                cboPortee.DataSource = clsMetier.GetInstance().getAllClsportee();
                this.setMembersallcbo(cboPortee, "Valeur", "Id");

                List<ComboBox> lstCombo = new List<ComboBox>() { cboTypeAccessPoint, cboTensionAlimentation, cboPuissance, cboIntensite, cboPortee };

                SetSelectedIndexComboBox(lstCombo);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Erreur lors du chargement des listes déroulantes du milieu, {0}", ex.Message), "Chargement des listes déroulantes", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
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

        private void LoadDataGrid(string threadName)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                bdsrc.DataSource = clsMetier.GetInstance().getAllClsmateriel_AP();
                Principal.SetDataSource(bdsrc);

                dgv.DataSource = bdsrc;

                //Here we sotp waitCursor if there are not records in BindinSource
                if (bdsrc.Count == 0)
                {
                    ExecuteThreadStopWaitCursor();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Erreur lors du chargement des données, {0}", ex.Message), "Chargement des données", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
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
            try
            {
                this.Cursor = Cursors.WaitCursor;

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(string.Format("{0}-{1}", txtId.Text, ((clscategorie_materiel)cboCatMateriel.SelectedItem).Id.ToString()));
                txtIdentidiant.Text = sb.ToString();

                //Generate label Equipement
                txtLabel.Text = clsMetier.GetInstance().GenerateLabelMateriel(Convert.ToInt32(txtId.Text), "ILAP");

                //Creating QrCode 
                System.Drawing.Image img = QRCodeImage.GetGenerateQRCode(string.Format("{0}/{1}", txtIdentidiant.Text, txtLabel.Text), "L", "", 0);//L, M ou Q
                pbQRCode.Image = img;

                //Convert PictureBox image to Byte[]
                //Save a temp image file
                string fileName = clsTools.Instance.SaveTempImage(pbQRCode);
                tmpQrCode = clsTools.Instance.GetByteFromFile(fileName);
                //txtQRCode.Text = Convert.ToString(tmpQrCode);
                //Remove the temp image created
                clsTools.Instance.RemoveTempImage(fileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Erreur lors de la génération du QrCode, {0}", ex.Message), "Génération QrCode", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
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
            try
            {
                this.Cursor = Cursors.WaitCursor;

                BindingList();
                blnModifie = true;
                Principal.ActivateOnNewSelectionChangeDgvCommandButtons(true);

                if (bdsrc.Count > 0)
                {
                    if ((bool)((clsmateriel)bdsrc.Current).Archiver)
                        cmdArchiver.Enabled = false;
                    else
                        cmdArchiver.Enabled = true;
                }
                else
                    cmdArchiver.Enabled = false;

                //Affectation de la duree restante par rapport a la garantie de l'equipement
                int? duree = null;

                if (cboGarantie.SelectedValue != null)
                    duree = int.Parse(cboGarantie.SelectedValue.ToString());

                lblStatusGuaraty.Text = clsMetier.GetInstance().CalculateEndGuarany(duree, DateTime.Parse(txtDateAcquisition.Text));

                //Affiche QrCode from rtfTextBox
                pbQRCode.Image = null;
                pbQRCode.Image = clsTools.Instance.GetImageFromByte(((clsmateriel)bdsrc.Current).Qrcode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Erreur lors de la sélection d'un enregistrement, {0}", ex.Message), "Sélection enegistrement", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }            
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
            catch { }
        }

        #region Actualise value in ComboBox
        private void CallActualiseComboBoxModification(string threadName)
        {
            //Actualisation des combobox si modification
            try
            {
                if (!string.IsNullOrEmpty(smartManage.Desktop.Properties.Settings.Default.strFormModifieSubForm))
                {
                    if (smartManage.Desktop.Properties.Settings.Default.strFormModifieSubForm.Equals(FormActualisation.frmCategorieMateriel.ToString()))
                    {
                        cboCatMateriel.DataSource = clsMetier.GetInstance().getAllClscategorie_materiel();
                        this.setMembersallcbo(cboCatMateriel, "Designation", "Id");
                    }
                    else if (smartManage.Desktop.Properties.Settings.Default.strFormModifieSubForm.Equals(FormActualisation.frmNumeroCompte.ToString()))
                    {
                        cboNumCompte.DataSource = clsMetier.GetInstance().getAllClscompte();
                        this.setMembersallcbo(cboNumCompte, "Numero", "Id");
                    }
                    else if (smartManage.Desktop.Properties.Settings.Default.strFormModifieSubForm.Equals(FormActualisation.frmMarque.ToString()))
                    {
                        cboMarque.DataSource = clsMetier.GetInstance().getAllClsmarque();
                        this.setMembersallcbo(cboMarque, "Designation", "Id");
                    }
                    else if (smartManage.Desktop.Properties.Settings.Default.strFormModifieSubForm.Equals(FormActualisation.frmModele.ToString()))
                    {
                        cboModele.DataSource = clsMetier.GetInstance().getAllClsmodele();
                        this.setMembersallcbo(cboModele, "Designation", "Id");
                    }
                    else if (smartManage.Desktop.Properties.Settings.Default.strFormModifieSubForm.Equals(FormActualisation.frmCouleur.ToString()))
                    {
                        cboCouleur.DataSource = clsMetier.GetInstance().getAllClscouleur();
                        this.setMembersallcbo(cboCouleur, "Designation", "Id");
                    }
                    else if (smartManage.Desktop.Properties.Settings.Default.strFormModifieSubForm.Equals(FormActualisation.frmPoids.ToString()))
                    {
                        cboPoids.DataSource = clsMetier.GetInstance().getAllClspoids();
                        this.setMembersallcbo(cboPoids, "Valeur", "Id");
                    }
                    else if (smartManage.Desktop.Properties.Settings.Default.strFormModifieSubForm.Equals(FormActualisation.frmEtatMateriel.ToString()))
                    {
                        cboEtat.DataSource = clsMetier.GetInstance().getAllClsetat_materiel();
                        this.setMembersallcbo(cboEtat, "Designation", "Id");
                    }
                    else if (smartManage.Desktop.Properties.Settings.Default.strFormModifieSubForm.Equals(FormActualisation.frmTypeAccessPoint.ToString()))
                    {
                        cboTypeAccessPoint.DataSource = clsMetier.GetInstance().getAllClstype_AP();
                        this.setMembersallcbo(cboTypeAccessPoint, "Designation", "Id");
                    }
                    else if (smartManage.Desktop.Properties.Settings.Default.strFormModifieSubForm.Equals(FormActualisation.frmTensionAlimentation.ToString()))
                    {
                        cboTensionAlimentation.DataSource = clsMetier.GetInstance().getAllClstension_alimentation();
                        this.setMembersallcbo(cboTensionAlimentation, "Valeur", "Id");
                    }
                    else if (smartManage.Desktop.Properties.Settings.Default.strFormModifieSubForm.Equals(FormActualisation.frmPuissance.ToString()))
                    {
                        cboPuissance.DataSource = clsMetier.GetInstance().getAllClspuissance();
                        this.setMembersallcbo(cboPuissance, "Valeur", "Id");
                    }
                    else if (smartManage.Desktop.Properties.Settings.Default.strFormModifieSubForm.Equals(FormActualisation.frmIntensite.ToString()))
                    {
                        cboIntensite.DataSource = clsMetier.GetInstance().getAllClsintensite();
                        this.setMembersallcbo(cboIntensite, "Valeur", "Id");
                    }
                    else if (smartManage.Desktop.Properties.Settings.Default.strFormModifieSubForm.Equals(FormActualisation.frmGarantie.ToString()))
                    {
                        cboGarantie.DataSource = clsMetier.GetInstance().getAllClsgarantie();
                        this.setMembersallcbo(cboGarantie, "Valeur", "Id");
                    }
                    else if (smartManage.Desktop.Properties.Settings.Default.strFormModifieSubForm.Equals(FormActualisation.frmPortee.ToString()))
                    {
                        cboPortee.DataSource = clsMetier.GetInstance().getAllClsportee();
                        this.setMembersallcbo(cboPortee, "Valeur", "Id");
                    }
                }

                smartManage.Desktop.Properties.Settings.Default.strFormModifieSubForm = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Echec d'actualisation de la liste déroulante, {0}", ex.Message), "Actualisation liste déroulante", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }       
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

                    ExecuteThreadStopWaitCursor();

                    try
                    {
                        clsTools.SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);
                    }
                    catch { }
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

                    ExecuteThreadStopWaitCursor();
                }
            }
        }

        private void TempsRefreshData_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (tLeftCombo != null || tMiddleCombo != null || tDataGrid != null)
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

                            tempsStopWaitCursor.Enabled = true;
                            tempsStopWaitCursor.Elapsed += TempsStopWaitCursor_Elapsed;

                            try
                            {
                                clsTools.SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);
                            }
                            catch { }
                        }
                    }
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
                }
            }
        }

        private void DoActualiseDropDown()
        {
            try
            {
                tempsActualiseCombo.Enabled = true;
                tempsActualiseCombo.Elapsed += TempsActualiseCombo_Elapsed;

                if (tActualiseComb == null)
                {
                    tActualiseComb = new Thread(new ThreadStart(ActualiseComboBoxModification));
                    tActualiseComb.Start();
                }
            }
            catch { }
        }

        private void ExecuteThreadStopWaitCursor()
        {
            tempsStopWaitCursor.Enabled = true;
            tempsStopWaitCursor.Elapsed += TempsStopWaitCursor_Elapsed;

            try
            {
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
            catch { }
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
                else e.Value = (clsTools.Instance.GetBytesFromImage(pbPhoto1.Image));
            }
            catch { }
        }

        private void Bs_Parse2(object sender, ConvertEventArgs e)
        {
            try
            {
                if (e.Value == null) e = null;
                else e.Value = (clsTools.Instance.GetBytesFromImage(pbPhoto2.Image));
            }
            catch { }
        }

        private void Bs_Parse3(object sender, ConvertEventArgs e)
        {
            try
            {
                if (e.Value == null) e = null;
                else e.Value = (clsTools.Instance.GetBytesFromImage(pbPhoto3.Image));
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
                    e.Value = (clsTools.Instance.GetImageFromByte((Byte[])e.Value));
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
                    e.Value = (clsTools.Instance.GetImageFromByte((Byte[])e.Value));
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
                    e.Value = (clsTools.Instance.GetImageFromByte((Byte[])e.Value));
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
            SetBindingControls(txtLabel, "Text", materiel, "Label");
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
            SetBindingControls(chkArchiver, "Checked", materiel, "Archiver");

            //Partie pour Access Point
            SetBindingControls(cboTypeAccessPoint, "SelectedValue", materiel, "Id_type_ap");
            SetBindingControls(cboTensionAlimentation, "SelectedValue", materiel, "Id_tension_alimentation");
            SetBindingControls(cboPuissance, "SelectedValue", materiel, "Id_puissance");
            SetBindingControls(cboIntensite, "SelectedValue", materiel, "Id_intensite");
            SetBindingControls(cboPortee, "SelectedValue", materiel, "Id_portee");                   
        }

        private void BindingList()
        {
            SetBindingControls(txtId, "Text", bdsrc, "Id");
            SetBindingControls(txtIdentidiant, "Text", bdsrc, "Code_str");
            SetBindingControls(cboCatMateriel, "SelectedValue", bdsrc, "Id_categorie_materiel");
            SetBindingControls(cboNumCompte, "SelectedValue", bdsrc, "Id_compte");
            SetBindingControls(txtLabel, "Text", bdsrc, "Label");
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
            SetBindingControls(chkArchiver, "Checked", bdsrc, "Archiver");

            //Partie pour Access Point
            SetBindingControls(cboTypeAccessPoint, "SelectedValue", bdsrc, "Id_type_ap");
            SetBindingControls(cboTensionAlimentation, "SelectedValue", bdsrc, "Id_tension_alimentation");
            SetBindingControls(cboPuissance, "SelectedValue", bdsrc, "Id_puissance");
            SetBindingControls(cboIntensite, "SelectedValue", bdsrc, "Id_intensite");
            SetBindingControls(cboPortee, "SelectedValue", bdsrc, "Id_portee");
        }
        #endregion

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

            try
            {
                if (tLeftCombo == null)
                {
                    tLeftCombo = new Thread(new ThreadStart(ExecuteLeftCombo));
                    tLeftCombo.Start();

                    tMiddleCombo = new Thread(new ThreadStart(ExecuteMiddleCombo));
                    tMiddleCombo.Start();

                    tDataGrid = new Thread(new ThreadStart(ExecuteDataGrid));
                    tDataGrid.Start();
                }
                else
                {
                    tLeftCombo.Abort();
                    tLeftCombo = null;
                }
            }
            catch { }

            if (bdsrc.Count == 0)
            {
                Principal.ActivateOnLoadCommandButtons(false);
            }
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
                pbQRCode.Image = null;
                cmdArchiver.Enabled = false;

                //Selection automatique de l'item du combo
                cboCatMateriel.Text = clsMetier.GetInstance().getClscategorie_materiel("8").Designation.ToString();
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
                    if (chkArchiver.Checked)
                        throw new Exception("Vous ne pouvez archiver un enregistrement non encore sauvegardé !!! Réessayer svp !!!");

                    materiel.Qrcode = tmpQrCode;
                    int record = materiel.inserts();
                    tmpQrCode = null;
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
            frmReportImprimante frm = new frmReportImprimante();
            frm.MdiParent = Principal;
            //frm.setData(factory.getAllSexe_Dt(), @"D:\appStockMS\appStock.Desktop\reports\rptListSexe.rdlc");
            frm.Icon = this.Icon;
            frm.Show();
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

            try
            {
                //Executed in thread
                if (tSelectionChangeDataGrid == null)
                {
                    tSelectionChangeDataGrid = new Thread(new ThreadStart(ExecuteSelectionDataGrid));
                    tSelectionChangeDataGrid.Start();
                }
            }
            catch { }
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
                if (clsTools.Instance.LimiteImageSize(open.FileName, 1024000, 1000, 1000))
                {
                    pbPhoto1.Load(open.FileName);
                    blnPhoto1 = true;
                    materiel.Photo1 = clsTools.Instance.GetByteFromFile(open.FileName);
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
                if (clsTools.Instance.LimiteImageSize(open.FileName, 1024000, 1000, 1000))
                {
                    pbPhoto2.Load(open.FileName);
                    blnPhoto2 = true;
                    materiel.Photo2 = clsTools.Instance.GetByteFromFile(open.FileName);
                }
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
                if (clsTools.Instance.LimiteImageSize(open.FileName, 1024000, 1000, 1000))
                {
                    pbPhoto3.Load(open.FileName);
                    blnPhoto3 = true;
                    materiel.Photo3 = clsTools.Instance.GetByteFromFile(open.FileName);
                }
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

        private void smnCtxPhoto3_Click(object sender, EventArgs e)
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

        private void pbPhoto3_MouseHover(object sender, EventArgs e)
        {
            pbPhoto3.Cursor = Cursors.Hand;
        }

        private void pbPhoto2_MouseHover(object sender, EventArgs e)
        {
            pbPhoto2.Cursor = Cursors.Hand;
        }

        private void pbPhoto1_MouseHover(object sender, EventArgs e)
        {
            pbPhoto1.Cursor = Cursors.Hand;
        }

        private void pbPhoto1_MouseLeave(object sender, EventArgs e)
        {
            pbPhoto1.Cursor = Cursors.Default;
        }

        private void pbPhoto2_MouseLeave(object sender, EventArgs e)
        {
            pbPhoto2.Cursor = Cursors.Default;
        }

        private void pbPhoto3_MouseLeave(object sender, EventArgs e)
        {
            pbPhoto3.Cursor = Cursors.Default;
        }

        private void cmdArchiver_Click(object sender, EventArgs e)
        {
            try
            {
                clsmateriel mat = new clsmateriel();
                mat = ((clsmateriel)bdsrc.Current);
                int record = clsMetier.GetInstance().ArchiverMateriel(mat);

                MessageBox.Show("Archivage éffectué : " + record + " Affecté", "Archivage enregistrement", MessageBoxButtons.OK, MessageBoxIcon.Information);
                RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Echec d'archivage, " + ex.Message, "Archivage enregistrement", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void cboPuissance_DropDown(object sender, EventArgs e)
        {
            DoActualiseDropDown();
        }

        private void cboIntensite_DropDown(object sender, EventArgs e)
        {
            DoActualiseDropDown();
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

        private void cboNumCompte_Leave(object sender, EventArgs e)
        {
            try
            {
                tempsGenerateQrCode.Enabled = true;
                tempsGenerateQrCode.Elapsed += TempsGenerateQrCode_Elapsed;

                //Executed in new thread
                if (tGenerateQrCode == null)
                {
                    tGenerateQrCode = new Thread(new ThreadStart(ExecuteGenerateQrCode));
                    tGenerateQrCode.Start();
                }
            }
            catch { }
        }

        private void lblAddPuissance_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmPuissance frm = new frmPuissance();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void lblAddIntensite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmIntensite frm = new frmIntensite();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void lblAddTensionAlim_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmTensionAlimentation frm = new frmTensionAlimentation();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void cboTensionAlimentation_DropDown(object sender, EventArgs e)
        {
            DoActualiseDropDown();
        }

        private void cboTypeAccessPoint_DropDown(object sender, EventArgs e)
        {
            DoActualiseDropDown();
        }

        private void lblAddTypeAccessPoint_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmTypeAccessPoint frm = new frmTypeAccessPoint();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void cboPortee_DropDown(object sender, EventArgs e)
        {
            DoActualiseDropDown();
        }

        private void lblAddPortee_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmPortee frm = new frmPortee();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void frmAccessPoint_Load(object sender, EventArgs e)
        {
            cmdArchiver.Enabled = false;
            cboCatMateriel.Enabled = false;

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

        private void frmAccessPoint_FormClosed(object sender, FormClosedEventArgs e)
        {
            Principal.SetValuesLabel(Properties.Settings.Default.UserConnected, "Attente d'une action de l'utilisateur");

            //Reinitialise all Thread
            try
            {
                this.UnloadThreadRessource(tDataGrid);
                this.UnloadThreadRessource(tSelectionChangeDataGrid);
                this.UnloadThreadRessource(tGenerateQrCode);
                this.UnloadThreadRessource(tLeftCombo);
                this.UnloadThreadRessource(tMiddleCombo);
                this.UnloadThreadRessource(tActualiseComb);
                this.UnloadThreadRessource(tStopWaitCursor);
            }
            catch { }

            Principal.ApplyDefaultStatusBar(Principal, Properties.Settings.Default.UserConnected);
            //Affecte Activate BindingNavigator
            Principal.ActivateMainBindingSource(Principal);
        }

        private void frmAccessPoint_Activated(object sender, EventArgs e)
        {
            Principal.SetValuesLabel(Properties.Settings.Default.UserConnected, "Gestion des access point ou point d'accès");
            Principal.SetCurrentICRUDChildForm(this);

            try
            {
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
            }
            catch { }

            firstLoad = true;
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
