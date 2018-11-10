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
    public partial class frmPersonne : Form, ICRUDGeneral, ICallMainForm
    {
        BindingSource bdsrc = new BindingSource();
        bool blnModifie = false;
        private clspersonne materiel = new clspersonne();
        int? newID = null;

        //Delegate utilisation des threads
        private delegate void LoadSomeData(string threadName);

        //Timer for automatically unload thread for update comboBox on DropDown event
        System.Timers.Timer tempsActualiseCombo = null;

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
        Thread tLeftCombo = null;
        Thread tActualiseComb = null;
        Thread tStopWaitCursor = null;

        //Boolean variables for photo
        bool blnPhoto = false;

        bool firstLoad = false;

        public frmPrincipal Principal
        {
            get;
            set;
        }

        public frmPersonne()
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
                Dictionary<string, string> dicoSexe = new Dictionary<string, string>();
                Dictionary<string, string> dicoEtatCivil = new Dictionary<string, string>();

                dicoSexe.Add("M", "Masculin");
                dicoSexe.Add("F", "Féminin");

                dicoEtatCivil.Add("Marié(e)", "Marié(e)");
                dicoEtatCivil.Add("Divorcé(e)", "Divorcé(e)");
                dicoEtatCivil.Add("Veuf(ve)", "Veuf(ve)");
                dicoEtatCivil.Add("Célibataire", "Célibataire");

                cboSexe.DataSource = new BindingSource(dicoSexe, null);
                this.setMembersallcbo(cboSexe, "Value", "Key");
                cboEtatCivil.DataSource = new BindingSource(dicoEtatCivil, null);
                this.setMembersallcbo(cboEtatCivil, "Value", "Key");
                cboGrade.DataSource = clsMetier.GetInstance().getAllClsgrade();
                this.setMembersallcbo(cboGrade, "Designation", "Id");

                List<ComboBox> lstCombo = new List<ComboBox>() { cboSexe, cboEtatCivil, cboGrade };

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

        private void LoadDataGrid(string threadName)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                bdsrc.DataSource = clsMetier.GetInstance().getAllClspersonne();
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

        private void DoExecuteSelectionDataGrid(string threadName)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                BindingList();
                blnModifie = true;
                Principal.ActivateOnNewSelectionChangeDgvCommandButtons(true);
                int id_personne = ((clspersonne)bdsrc.Current).Id;
                smartManage.Desktop.Properties.Settings.Default.IdentifiantPersonne = id_personne;
                //Here Fill DataGrid for Phone number, email address and address
                
                dgvTelephone.DataSource = clsMetier.GetInstance().getAllClstelephone(id_personne);
                dgvEmail.DataSource = clsMetier.GetInstance().getAllClsemail(id_personne);
                dgvAdresse.DataSource = clsMetier.GetInstance().getAllClsadresse(id_personne);
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
                    if (smartManage.Desktop.Properties.Settings.Default.strFormModifieSubForm.Equals(FormActualisation.frmGrade.ToString()))
                    {
                        cboGrade.DataSource = clsMetier.GetInstance().getAllClsgrade();
                        this.setMembersallcbo(cboGrade, "Designation", "Id");
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
            try
            {
                if (tLeftCombo != null || tDataGrid != null)
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
            catch { }
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
                    else if (tLeftCombo != null)
                        lstThread.Add(tLeftCombo);
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

        private void Bs_Parse(object sender, ConvertEventArgs e)
        {
            try
            {
                if (e.Value == null) e = null;
                else e.Value = (clsTools.Instance.GetBytesFromImage(pbPhoto.Image));
            }
            catch { }
        }

        void binding_Format(object sender, ConvertEventArgs e)
        {
            try
            {
                pbPhoto.Tag = null;
                if (e.DesiredType != typeof(System.Drawing.Image) || e.Value.ToString() == e.DesiredType.FullName || e.Value.ToString() == e.DesiredType.Name) return;
                if (e.Value.ToString() == "System.Drawing.Bitmap") return;
                if (e.Value == null || e.Value.ToString() == "")
                {
                    pbPhoto.Tag = "1";
                    pbPhoto.Image = null;
                }
                else
                {
                    string imagestr = e.Value.ToString();
                    e.Value = (clsTools.Instance.GetImageFromByte((Byte[])e.Value));
                }
            }
            catch { }
        }

        private void bingImg(PictureBox pb, Object src, string ctrProp, string obj)
        {
            pb.DataBindings.Clear();
            Binding binding = new Binding(ctrProp, src, obj, true, DataSourceUpdateMode.OnPropertyChanged);
            binding.Parse += new ConvertEventHandler(Bs_Parse);
            binding.Format += new ConvertEventHandler(binding_Format);
            pb.DataBindings.Add(binding);
        }

        private void BindingCls()
        {
            SetBindingControls(txtId, "Text", materiel, "Id");
            SetBindingControls(txtNom, "Text", materiel, "Nom");
            SetBindingControls(txtPostNom, "Text", materiel, "Postnom");
            SetBindingControls(txtPrenom, "Text", materiel, "Prenom");
            SetBindingControls(txtDateNaissance, "Text", materiel, "Datenaissance");
            SetBindingControls(cboSexe, "SelectedValue", materiel, "Sexe");
            SetBindingControls(cboEtatCivil, "SelectedValue", materiel, "Etatcivil");
            SetBindingControls(cboGrade, "SelectedValue", materiel, "Id_grade");
            bingImg(pbPhoto, materiel, "Image", "Photo");
            SetBindingControls(chkEnseignant, "Checked", materiel, "Isenseignant");
            SetBindingControls(chkAgent, "Checked", materiel, "Isagent");
            SetBindingControls(chkEtudiant, "Checked", materiel, "Isetudiant");

            SetBindingControls(txtCreateBy, "Text", materiel, "User_created");
            SetBindingControls(txtDateCreate, "Text", materiel, "Date_created");
            SetBindingControls(txtModifieBy, "Text", materiel, "User_modified");
            SetBindingControls(txtDateModifie, "Text", materiel, "Date_modified");                 
        }

        private void BindingList()
        {
            SetBindingControls(txtId, "Text", bdsrc, "Id");
            SetBindingControls(txtNom, "Text", bdsrc, "Nom");
            SetBindingControls(txtPostNom, "Text", bdsrc, "Postnom");
            SetBindingControls(txtPrenom, "Text", bdsrc, "Prenom");
            SetBindingControls(txtDateNaissance, "Text", bdsrc, "Datenaissance");
            SetBindingControls(cboSexe, "SelectedValue", bdsrc, "Sexe");
            SetBindingControls(cboEtatCivil, "SelectedValue", bdsrc, "Etatcivil");
            SetBindingControls(cboGrade, "SelectedValue", bdsrc, "Id_grade");
            bingImg(pbPhoto, bdsrc, "Image", "Photo");
            SetBindingControls(chkEnseignant, "Checked", bdsrc, "Isenseignant");
            SetBindingControls(chkAgent, "Checked", bdsrc, "Isagent");
            SetBindingControls(chkEtudiant, "Checked", bdsrc, "Isetudiant");

            SetBindingControls(txtCreateBy, "Text", bdsrc, "User_created");
            SetBindingControls(txtDateCreate, "Text", bdsrc, "Date_created");
            SetBindingControls(txtModifieBy, "Text", bdsrc, "User_modified");
            SetBindingControls(txtDateModifie, "Text", bdsrc, "Date_modified");
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
                materiel = new clspersonne();

                Principal.ActivateOnNewCommandButtons(true);
                blnModifie = false;

                BindingCls();

                //Set the new ID
                if (newID == null)
                    newID = clsMetier.GetInstance().GenerateLastID("personne");
                txtId.Text = newID.ToString();
                txtCreateBy.Text = smartManage.Desktop.Properties.Settings.Default.UserConnected;
                txtDateCreate.Text = DateTime.Now.ToString();

                try
                {
                    dgvTelephone.DataSource = clsMetier.GetInstance().getAllClstelephone(-1);
                    dgvEmail.DataSource = clsMetier.GetInstance().getAllClsemail(-1);
                    dgvAdresse.DataSource = clsMetier.GetInstance().getAllClsadresse(-1);
                }
                catch { }
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
                    List<clspersonne> lstItemSearch = new List<clspersonne>();
                    lstItemSearch = clsMetier.GetInstance().getAllClspersonne(criteria);

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
            ((clspersonne)bdsrc.Current).User_modified = smartManage.Desktop.Properties.Settings.Default.UserConnected;
            ((clspersonne)bdsrc.Current).Date_modified = DateTime.Now;

            clspersonne mat = new clspersonne();
            mat = ((clspersonne)bdsrc.Current);
            if (blnPhoto)
            {
                mat.Photo = materiel.Photo;
                blnPhoto = false;
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
                        record = materiel.delete(((clspersonne)bdsrc.Current));
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
            frmReportPersonne frm = new frmReportPersonne();
            frm.MdiParent = Principal;
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

        private void LoadPicture()
        {
            OpenFileDialog open;
            DialogResult result;
            LoadPicture(out open, out result);

            if (result == DialogResult.OK)
            {
                if (clsTools.Instance.LimiteImageSize(open.FileName, 5120000, 6000, 6000))
                {
                    pbPhoto.Load(open.FileName);
                    blnPhoto = true;
                    materiel.Photo = clsTools.Instance.GetByteFromFile(open.FileName);
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

        private clspersonne ReformatDateNaissancePersonne(Control txtBox, clspersonne personne)
        {
            if(txtBox is MaskedTextBox)
            {
                if (!((MaskedTextBox)txtBox).Text.Trim().Equals("/  /"))
                {
                    personne.Datenaissance = Convert.ToDateTime(((MaskedTextBox)txtBox).Text);
                    return personne;
                }
                else
                    personne.Datenaissance = null;
            }
            return personne;
        }

        private void lblAddGrade_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmGrade frm = new frmGrade();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void frmPersonne_FormClosed(object sender, FormClosedEventArgs e)
        {
            Principal.SetValuesLabel(Properties.Settings.Default.UserConnected, "Attente d'une action de l'utilisateur");

            //Reinitialise all Thread
            try
            {
                this.UnloadThreadRessource(tDataGrid);
                this.UnloadThreadRessource(tSelectionChangeDataGrid);
                this.UnloadThreadRessource(tLeftCombo);
                this.UnloadThreadRessource(tActualiseComb);
                this.UnloadThreadRessource(tStopWaitCursor);
            }
            catch { }

            Principal.ApplyDefaultStatusBar(Principal, Properties.Settings.Default.UserConnected);
            //Affecte Activate BindingNavigator
            Principal.ActivateMainBindingSource(Principal);
        }

        private void frmPersonne_Load(object sender, EventArgs e)
        {
            //Initialise timers
            tempsActualiseCombo = new System.Timers.Timer();
            tempsActualiseCombo.Interval = 100;

            tempsSelectionChangeDataGrid = new System.Timers.Timer();
            tempsSelectionChangeDataGrid.Interval = 100;

            tempsRefreshData = new System.Timers.Timer();
            tempsRefreshData.Interval = 100;

            tempsActivateForm = new System.Timers.Timer();
            tempsActivateForm.Interval = 100;

            tempsStopWaitCursor = new System.Timers.Timer();
            tempsStopWaitCursor.Interval = 50;

            //Affecte MenuStrip
            pbPhoto.ContextMenuStrip = ctxMenuPhoto;
            //executed in many independant thread
            RefreshData();
        }

        private void lblPhoto_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                LoadPicture();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Echec de chargement de la photo, " + ex.Message, "Chargement de la photo1", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void pbPhoto_MouseHover(object sender, EventArgs e)
        {
            pbPhoto.Cursor = Cursors.Hand;
        }

        private void pbPhoto_MouseLeave(object sender, EventArgs e)
        {
            pbPhoto.Cursor = Cursors.Default;
        }

        private void frmPersonne_Activated(object sender, EventArgs e)
        {
            Principal.SetValuesLabel(Properties.Settings.Default.UserConnected, "Identification des personnes");
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
            //Affecte Activate BindingNavigator
            Principal.ActivateMainBindingSource(Principal);
        }

        private void smnCtxPhoto_Click(object sender, EventArgs e)
        {
            try
            {
                LoadPicture();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Echec de chargement de la photo, " + ex.Message, "Chargement de la photo1", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void chkEnseignant_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEnseignant.Checked || chkAgent.Checked)
            {
                chkEtudiant.Checked = false;
                chkEtudiant.Enabled = false;
            }
            else
                chkEtudiant.Enabled = true;
        }

        private void chkAgent_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAgent.Checked || chkEnseignant.Checked)
            {
                chkEtudiant.Checked = false;
                chkEtudiant.Enabled = false;
            }
            else
                chkEtudiant.Enabled = true;
        }

        private void chkEtudiant_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEtudiant.Checked)
            {
                chkEnseignant.Checked = chkAgent.Checked = false;
                chkEnseignant.Enabled = chkAgent.Enabled = false;
            }
            else
            {
                chkEnseignant.Enabled = chkAgent.Enabled = true;
            }
        }

        private void cmdNewPhone_Click(object sender, EventArgs e)
        {
            frmTelephone frm = new frmTelephone();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void cmdNewEmail_Click(object sender, EventArgs e)
        {
            frmEmail frm = new frmEmail();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void cmdNewAddress_Click(object sender, EventArgs e)
        {
            frmAdresse frm = new frmAdresse();
            frm.Icon = this.Icon;
            frm.ShowDialog();
        }

        private void lblRefreshAllSubDgv_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                int id_personne = ((clspersonne)bdsrc.Current).Id;
                dgvTelephone.DataSource = clsMetier.GetInstance().getAllClstelephone(id_personne);
                dgvEmail.DataSource = clsMetier.GetInstance().getAllClsemail(id_personne);
                dgvAdresse.DataSource = clsMetier.GetInstance().getAllClsadresse(id_personne);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Echec de chargement des informations complémentaires de la personne, " + ex.Message, "Chargement information de la personne", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void cboGrade_DropDown(object sender, EventArgs e)
        {
            DoActualiseDropDown();
        }
    }
}
