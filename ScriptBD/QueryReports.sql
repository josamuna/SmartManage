--select * from materiel
--Vue generale pour Ordinateur
create view vOrdinateur 
as 
select materiel.id,materiel.code_str,categorie_materiel.designation as 'Catégorie Mat.',compte.numero as 'Numéro Cpte.',CONVERT(varchar(10),date_acquisition,3) as 'Date Acq.',garantie.valeur as 'Garantie(Année)',marque.designation as 'Marque',
modele.designation as 'Modèle',couleur.designation as 'Couleur',poids.valeur as 'Poids',etat_materiel.designation as 'Etat',materiel.qrcode as 'QRCode',materiel.photo1 as 'Photo1',materiel.photo2 as 'Photo2',materiel.photo3 as 'Photo3',materiel.label as 'Etiquette',
materiel.mac_adresse1 as 'MAC Wifi', materiel.mac_adresse2 as 'MAC LAN',type_ordinateur.designation as 'Type Ordi.',type_clavier.designation as 'Clavier',OS.designation 'Désignation',ram.id as 'Mémoire(Gb)',processeur.valeur as 'Processeur(Ghz)',
nombre_coeur_processeur.valeur as 'Processor Core',type_hdd.designation as 'Type HDD',nombre_hdd.valeur as 'Nre HDD',capacite_hdd.valeur as 'HDD',taille_ecran.valeur as 'Ecran',usb2.valeur as 'USB2.0',usb3.valeur as 'USB3.0',hdmi.valeur as 'HDMI',
vga.valeur as 'VGA',tension_adaptateur.valeur as 'U.Bat.(V)',tension_adaptateur.valeur as 'U.Adapt.(V)',puissance_adaptateur.valeur as 'P.Adapt.(W)',materiel.numero_cle as 'Numéro cl2', intensite_adaptateur.valeur as 'I.Adapt(A)', 
materiel.commentaire as 'Commentaire',materiel.archiver as 'Archiver' from materiel 
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

--Ordinateur par identifiant
select materiel.id,materiel.code_str,categorie_materiel.designation as 'Catégorie Mat.',compte.numero as 'Numéro Cpte.',CONVERT(varchar(10),date_acquisition,3) as 'Date Acq.',garantie.valeur as 'Garantie(Année)',marque.designation as 'Marque',
modele.designation as 'Modèle',couleur.designation as 'Couleur',poids.valeur as 'Poids',etat_materiel.designation as 'Etat',materiel.qrcode as 'QRCode',materiel.photo1 as 'Photo1',materiel.photo2 as 'Photo2',materiel.photo3 as 'Photo3',materiel.label as 'Etiquette',
materiel.mac_adresse1 as 'MAC Wifi', materiel.mac_adresse2 as 'MAC LAN',type_ordinateur.designation as 'Type Ordi.',type_clavier.designation as 'Clavier',OS.designation 'Désignation',ram.id as 'Mémoire(Gb)',processeur.valeur as 'Processeur(Ghz)',
nombre_coeur_processeur.valeur as 'Processor Core',type_hdd.designation as 'Type HDD',nombre_hdd.valeur as 'Nre HDD',capacite_hdd.valeur as 'HDD',taille_ecran.valeur as 'Ecran',usb2.valeur as 'USB2.0',usb3.valeur as 'USB3.0',hdmi.valeur as 'HDMI',
vga.valeur as 'VGA',tension_adaptateur.valeur as 'U.Bat.(V)',tension_adaptateur.valeur as 'U.Adapt.(V)',puissance_adaptateur.valeur as 'P.Adapt.(W)',materiel.numero_cle as 'Numéro cl2', intensite_adaptateur.valeur as 'I.Adapt(A)', 
materiel.commentaire as 'Commentaire',materiel.archiver as 'Archiver' from materiel 
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
where materiel.code_str='2-1' and materiel.archiver=0

--Ordinateur par etat de l'equipement
select materiel.id,materiel.code_str,categorie_materiel.designation as 'Catégorie Mat.',compte.numero as 'Numéro Cpte.',CONVERT(varchar(10),date_acquisition,3) as 'Date Acq.',garantie.valeur as 'Garantie(Année)',marque.designation as 'Marque',
modele.designation as 'Modèle',couleur.designation as 'Couleur',poids.valeur as 'Poids',etat_materiel.designation as 'Etat',materiel.qrcode as 'QRCode',materiel.photo1 as 'Photo1',materiel.photo2 as 'Photo2',materiel.photo3 as 'Photo3',materiel.label as 'Etiquette',
materiel.mac_adresse1 as 'MAC Wifi', materiel.mac_adresse2 as 'MAC LAN',type_ordinateur.designation as 'Type Ordi.',type_clavier.designation as 'Clavier',OS.designation 'Désignation',ram.id as 'Mémoire(Gb)',processeur.valeur as 'Processeur(Ghz)',
nombre_coeur_processeur.valeur as 'Processor Core',type_hdd.designation as 'Type HDD',nombre_hdd.valeur as 'Nre HDD',capacite_hdd.valeur as 'HDD',taille_ecran.valeur as 'Ecran',usb2.valeur as 'USB2.0',usb3.valeur as 'USB3.0',hdmi.valeur as 'HDMI',
vga.valeur as 'VGA',tension_adaptateur.valeur as 'U.Bat.(V)',tension_adaptateur.valeur as 'U.Adapt.(V)',puissance_adaptateur.valeur as 'P.Adapt.(W)',materiel.numero_cle as 'Numéro cl2', intensite_adaptateur.valeur as 'I.Adapt(A)', 
materiel.commentaire as 'Commentaire',materiel.archiver as 'Archiver' from materiel 
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
where etat_materiel.designation='BON'

--par délais de garantie de l'equipement
select materiel.id,materiel.code_str,categorie_materiel.designation as 'Catégorie Mat.',compte.numero as 'Numéro Cpte.',CONVERT(varchar(10),date_acquisition,3) as 'Date Acq.',garantie.valeur as 'Garantie(Année)',marque.designation as 'Marque',
modele.designation as 'Modèle',couleur.designation as 'Couleur',poids.valeur as 'Poids',etat_materiel.designation as 'Etat',materiel.qrcode as 'QRCode',materiel.photo1 as 'Photo1',materiel.photo2 as 'Photo2',materiel.photo3 as 'Photo3',materiel.label as 'Etiquette',
materiel.mac_adresse1 as 'MAC Wifi', materiel.mac_adresse2 as 'MAC LAN',type_ordinateur.designation as 'Type Ordi.',type_clavier.designation as 'Clavier',OS.designation 'Désignation',ram.id as 'Mémoire(Gb)',processeur.valeur as 'Processeur(Ghz)',
nombre_coeur_processeur.valeur as 'Processor Core',type_hdd.designation as 'Type HDD',nombre_hdd.valeur as 'Nre HDD',capacite_hdd.valeur as 'HDD',taille_ecran.valeur as 'Ecran',usb2.valeur as 'USB2.0',usb3.valeur as 'USB3.0',hdmi.valeur as 'HDMI',
vga.valeur as 'VGA',tension_adaptateur.valeur as 'U.Bat.(V)',tension_adaptateur.valeur as 'U.Adapt.(V)',puissance_adaptateur.valeur as 'P.Adapt.(W)',materiel.numero_cle as 'Numéro cl2', intensite_adaptateur.valeur as 'I.Adapt(A)', 
materiel.commentaire as 'Commentaire',materiel.archiver as 'Archiver' from materiel 
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
where garantie.valeur=1

--par MAC Wifi de l'equipement
select materiel.id,materiel.code_str,categorie_materiel.designation as 'Catégorie Mat.',compte.numero as 'Numéro Cpte.',CONVERT(varchar(10),date_acquisition,3) as 'Date Acq.',garantie.valeur as 'Garantie(Année)',marque.designation as 'Marque',
modele.designation as 'Modèle',couleur.designation as 'Couleur',poids.valeur as 'Poids',etat_materiel.designation as 'Etat',materiel.qrcode as 'QRCode',materiel.photo1 as 'Photo1',materiel.photo2 as 'Photo2',materiel.photo3 as 'Photo3',materiel.label as 'Etiquette',
materiel.mac_adresse1 as 'MAC Wifi', materiel.mac_adresse2 as 'MAC LAN',type_ordinateur.designation as 'Type Ordi.',type_clavier.designation as 'Clavier',OS.designation 'Désignation',ram.id as 'Mémoire(Gb)',processeur.valeur as 'Processeur(Ghz)',
nombre_coeur_processeur.valeur as 'Processor Core',type_hdd.designation as 'Type HDD',nombre_hdd.valeur as 'Nre HDD',capacite_hdd.valeur as 'HDD',taille_ecran.valeur as 'Ecran',usb2.valeur as 'USB2.0',usb3.valeur as 'USB3.0',hdmi.valeur as 'HDMI',
vga.valeur as 'VGA',tension_adaptateur.valeur as 'U.Bat.(V)',tension_adaptateur.valeur as 'U.Adapt.(V)',puissance_adaptateur.valeur as 'P.Adapt.(W)',materiel.numero_cle as 'Numéro cl2', intensite_adaptateur.valeur as 'I.Adapt(A)', 
materiel.commentaire as 'Commentaire',materiel.archiver as 'Archiver' from materiel 
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
where materiel.mac_adresse1=''

--par date acquisition de l'equipement
select materiel.id,materiel.code_str,categorie_materiel.designation as 'Catégorie Mat.',compte.numero as 'Numéro Cpte.',CONVERT(varchar(10),date_acquisition,3) as 'Date Acq.',garantie.valeur as 'Garantie(Année)',marque.designation as 'Marque',
modele.designation as 'Modèle',couleur.designation as 'Couleur',poids.valeur as 'Poids',etat_materiel.designation as 'Etat',materiel.qrcode as 'QRCode',materiel.photo1 as 'Photo1',materiel.photo2 as 'Photo2',materiel.photo3 as 'Photo3',materiel.label as 'Etiquette',
materiel.mac_adresse1 as 'MAC Wifi', materiel.mac_adresse2 as 'MAC LAN',type_ordinateur.designation as 'Type Ordi.',type_clavier.designation as 'Clavier',OS.designation 'Désignation',ram.id as 'Mémoire(Gb)',processeur.valeur as 'Processeur(Ghz)',
nombre_coeur_processeur.valeur as 'Processor Core',type_hdd.designation as 'Type HDD',nombre_hdd.valeur as 'Nre HDD',capacite_hdd.valeur as 'HDD',taille_ecran.valeur as 'Ecran',usb2.valeur as 'USB2.0',usb3.valeur as 'USB3.0',hdmi.valeur as 'HDMI',
vga.valeur as 'VGA',tension_adaptateur.valeur as 'U.Bat.(V)',tension_adaptateur.valeur as 'U.Adapt.(V)',puissance_adaptateur.valeur as 'P.Adapt.(W)',materiel.numero_cle as 'Numéro cl2', intensite_adaptateur.valeur as 'I.Adapt(A)', 
materiel.commentaire as 'Commentaire',materiel.archiver as 'Archiver' from materiel 
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
where materiel.date_acquisition between '23/01/2015' and '23/10/2018'




--=============
select materiel.id,materiel.code_str,categorie_materiel.designation as 'Catégorie Mat.',compte.numero as 'Numéro Cpte.',CONVERT(varchar(10),date_acquisition,3) as 'Date Acq.',garantie.valeur as 'Garantie(Année)',marque.designation as 'Marque',
etat_materiel.designation as 'Etat',materiel.qrcode as 'QRCode',materiel.photo1 as 'Photo1',materiel.photo2 as 'Photo2',materiel.photo3 as 'Photo3' from materiel 
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
where (materiel.date_acquisition between '23/01/2015' and '23/10/2018') and materiel.archiver=0

