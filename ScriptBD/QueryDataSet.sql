--Materiels
select materiel.id,materiel.code_str as 'Code',categorie_materiel.designation as 'CatMat',compte.numero as 'Numero',CONVERT(varchar(10),date_acquisition,3) as 'DateAcq',garantie.valeur as 'Garantie',marque.designation as 'Marque',
modele.designation as 'Modele',couleur.designation as 'Couleur',poids.valeur as 'Poids',etat_materiel.designation as 'Etat',materiel.qrcode as 'QRCode',materiel.photo1 as 'Photo1',materiel.photo2 as 'Photo2',materiel.photo3 as 'Photo3',materiel.label as 'Etiquette',
materiel.mac_adresse1 as 'MacWifi', materiel.mac_adresse2 as 'MacLAN',type_ordinateur.designation as 'TypeOrdi',type_clavier.designation as 'Clavier',OS.designation 'SystemeExpl',ram.id as 'Memoire',processeur.valeur as 'Processeur',
nombre_coeur_processeur.valeur as 'CoeurProcesseur',type_hdd.designation as 'TypeHDD',nombre_hdd.valeur as 'NbrHDD',capacite_hdd.valeur as 'CapaciteHDD',taille_ecran.valeur as 'Ecran',usb2.valeur as 'USB20',usb3.valeur as 'USB30',hdmi.valeur as 'HDMI',
vga.valeur as 'VGA',tension_adaptateur.valeur as 'UBat',tension_adaptateur.valeur as 'UAdapt',puissance_adaptateur.valeur as 'PAdapt',materiel.numero_cle as 'Numerocle', intensite_adaptateur.valeur as 'IAdapt', 
materiel.commentaire as 'Commentaire',materiel.archiver as 'Archiver',

type_switch.designation as 'TypeSwitch',

puissance.valeur as 'PImp',intensite.valeur as 'IImp',page_par_minute.valeur as 'PPM',type_imprimante.designation as 'TypeImprimante',

tension_alimentation.valeur as 'UAlim',usb.valeur as 'NbrUSB',memoire.valeur as 'NbrMemoire',sorties_audio.valeur as 'NbrSortiesAud',microphone.valeur as 'NbrMicro',gain.valeur as 'Gain',type_amplificateur.designation as 'TypeAmplificateur',

gbe.valeur as 'NbrGbe',fe.valeur as 'NbrFe',fo.valeur as 'NbrFo',serial.valeur as 'NbrSerial',default_pwd.designation as 'DefaultPwd',default_ip.designation as 'DefaultIP',console.valeur as 'NbrConsole',auxiliaire.valeur as 'NbrAux',materiel.capable_usb as 'SupportUSB', type_routeur_AP.designation as 'TyperouteurAP', version_ios.designation as 'VersionIOS',

portee.valeur as 'Portee',type_AP.designation as 'TypeAP',

frequence.designation as 'Frequence',antenne.valeur as 'NbrAnt',

netette.designation as 'Netete',materiel.compatible_wifi as 'SupportWifi'

from materiel 
left outer join garantie on garantie.id=materiel.id_garantie
left outer join categorie_materiel on categorie_materiel.id=materiel.id_categorie_materiel
inner join compte on compte.id=materiel.id_compte
inner join marque on marque.id=materiel.id_marque
inner join modele on modele.id=materiel.id_modele
inner join couleur on couleur.id=materiel.id_couleur
inner join poids on poids.id=materiel.id_poids
inner join etat_materiel on etat_materiel.id=materiel.id_etat_materiel
left outer join type_ordinateur on type_ordinateur.id=materiel.id_type_ordinateur
left outer join type_clavier on type_clavier.id=materiel.id_type_clavier
left outer join OS on OS.id=materiel.id_OS
left outer join ram on ram.id=materiel.id_ram
left outer join processeur on processeur.id=materiel.id_processeur
left outer join nombre_coeur_processeur on nombre_coeur_processeur.id=materiel.id_nombre_coeur_processeur
left outer join type_hdd on type_hdd.id=materiel.id_type_hdd
left outer join nombre_hdd on nombre_hdd.id=materiel.id_nombre_hdd
left outer join capacite_hdd on capacite_hdd.id=materiel.id_capacite_hdd
left outer join taille_ecran on taille_ecran.id=materiel.id_taille_ecran
left outer join usb2 on usb2.id=materiel.id_usb2
left outer join usb3 on usb3.id=materiel.id_usb3
left outer join hdmi on hdmi.id=materiel.id_hdmi
left outer join vga on vga.id=materiel.id_vga
left outer join tension_batterie on tension_batterie.id=materiel.id_tension_batterie
left outer join tension_adaptateur on tension_adaptateur.id=materiel.id_tension_adaptateur
left outer join puissance_adaptateur on puissance_adaptateur.id=materiel.id_puissance_adaptateur
left outer join intensite_adaptateur on intensite_adaptateur.id=materiel.id_intensite_adaptateur

--Printer
left outer join type_imprimante on type_imprimante.id=materiel.id_type_imprimante
left outer join puissance on puissance.id=materiel.id_puissance
left outer join intensite on intensite.id=materiel.id_intensite
left outer join page_par_minute on page_par_minute.id=materiel.id_page_par_minute

--Amplificateur
left outer join type_amplificateur on type_amplificateur.id=materiel.id_type_amplificateur
left outer join tension_alimentation on tension_alimentation.id=materiel.id_tension_alimentation
left outer join usb on usb.id=materiel.id_usb
left outer join memoire on memoire.id=materiel.id_memoire
left outer join sorties_audio on sorties_audio.id=materiel.id_sorties_audio
left outer join microphone on microphone.id=materiel.id_microphone
left outer join gain on gain.id=materiel.id_gain

--Routeur_AP
left outer join type_routeur_AP on type_routeur_AP.id=materiel.id_type_routeur_AP
left outer join gbe on gbe.id=materiel.id_gbe
left outer join fe on fe.id=materiel.id_fe
left outer join fo on fo.id=materiel.id_fo
left outer join serial on serial.id=materiel.id_serial
left outer join default_pwd on default_pwd.id=materiel.id_default_pwd
left outer join default_ip on default_ip.id=materiel.id_default_ip
left outer join console on console.id=materiel.id_console
left outer join auxiliaire on auxiliaire.id=materiel.id_auxiliaire
left outer join version_ios on version_ios.id=materiel.id_version_ios
--capable_usb

--AccessPoint
left outer join type_AP on type_AP.id=materiel.id_type_AP
left outer join portee on portee.id=materiel.id_portee

--Switch
left outer join type_switch on type_switch.id=materiel.id_type_switch

--Emetteur
left outer join frequence on frequence.id=materiel.id_frequence
left outer join antenne on antenne.id=materiel.id_antenne

--Retroprojecteur
left outer join netette on netette.id=materiel.id_netette
where categorie_materiel.designation='Ordinateur' 
									
									
									--'Access Point'
									--'Amplificateur'
									--'Emetteur'
									--'Imprimante'
									--'Retroprojecteur'
									--'Routeur_Access Point'
									--'Switch'
                                
                                                          
                                
                                
                                --and etat_materiel.designation='Bon' and archiver=0
								
--Affectation materiels
select affectation_materiel.id as 'NumAff',affectation_materiel.date_affectation as 'Date',salle.designation as 'Salle',AC.code_str as 'AnneeAcademique',lieu_affectation.designation as 'LieuAffectation',type_lieu_affectation.designation as 'TypeLieuAffectation',
ISNULL(personne.nom,'') + ' ' + ISNULL(personne.postnom,'') + ' ' + ISNULL(personne.prenom,'') as 'Nom',fonction.designation as 'Fonction',categorie_materiel.designation as 'CatMat',materiel.code_str as 'CodeMat',materiel.label as 'LabelMat',materiel.qrcode as 'QrCode' 
from affectation_materiel
inner join materiel on materiel.id=affectation_materiel.id_materiel
inner join categorie_materiel on categorie_materiel.id=materiel.id_categorie_materiel
inner join AC on AC.code_str=affectation_materiel.code_AC
inner join salle on salle.id=affectation_materiel.id_salle
inner join lieu_affectation on lieu_affectation.id=affectation_materiel.id_lieu_affectation
inner join type_lieu_affectation on type_lieu_affectation.id=lieu_affectation.id_type_lieu_affectation
left outer join personne on personne.id=lieu_affectation.id_personne
left outer join fonction on fonction.id=lieu_affectation.id_fonction