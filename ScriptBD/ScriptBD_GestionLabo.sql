--Verification de l'existance de la BD et dans le cas contraire on la cree
if exists(select name from sys.databases where name='gestion_labo_DB') 
	begin
		use master
		drop database gestion_labo_DB
		create database gestion_labo_DB
		use gestion_labo_DB;
	end
else
	begin
		create database gestion_labo_DB
		use gestion_labo_DB
	end
 
--Debut creation des tables
create table categorie_materiel
(
	id int,
	designation varchar(50) not null,
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime
	constraint pk_categorie_materiel primary key(id),
	constraint uk_categorie_materiel unique(designation)
)
go

create table compte
(
	id int,
	numero varchar(10) not null,
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime
	constraint pk_compte primary key(id),
	constraint uk_numero unique(numero)
)
go

create table marque
(
	id int,
	designation varchar(50) not null,
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime
	constraint pk_marque primary key(id),
	constraint uk_marque unique(designation)
)
go

create table modele
(
	id int,
	designation varchar(50) not null,
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime
	constraint pk_modele primary key(id),
	constraint uk_modele unique(designation)
)
go

create table couleur
(
	id int,
	designation varchar(50) not null,
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime
	constraint pk_couleur primary key(id),
	constraint uk_couleur unique(designation)
)
go

create table poids
(
	id int,
	valeur float not null,
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime
	constraint pk_poids primary key(id),
	constraint uk_poids unique(valeur)
)
go

create table type_ordinateur
(
	id int,
	designation varchar(50) not null,
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime
	constraint pk_type_ordinateur primary key(id),
	constraint uk_type_ordinateur unique(designation)
)
go

create table type_imprimante
(
	id int,
	designation varchar(50) not null,
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime
	constraint pk_type_imprimante primary key(id),
	constraint uk_type_imprimante unique(designation)
)
go

create table type_amplificateur
(
	id int,
	designation varchar(50) not null,
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime
	constraint pk_type_amplificateur primary key(id),
	constraint uk_type_amplificateur unique(designation)
)
go

create table type_routeur_AP
(
	id int,
	designation varchar(50) not null,
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime
	constraint pk_type_routeur_AP primary key(id),
	constraint uk_type_routeur_AP unique(designation)
)
go

create table type_AP
(
	id int,
	designation varchar(50) not null,
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime
	constraint pk_type_AP primary key(id),
	constraint uk_type_AP unique(designation)
)
go

create table portee
(
	id int,
	valeur float not null,
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime
	constraint pk_portee primary key(id),
	constraint uk_portee unique(valeur)
)
go

create table type_switch
(
	id int,
	designation varchar(50) not null,
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime
	constraint pk_type_switch primary key(id),
	constraint uk_type_switch unique(designation)
)
go

create table type_clavier
(
	id int,
	designation varchar(10) not null,
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime
	constraint pk_type_clavier primary key(id),
	constraint uk_type_clavier unique(designation)
)
go

create table etat_materiel
(
	id int,
	designation varchar(50) not null,
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime
	constraint pk_etat_materiel primary key(id),
	constraint uk_etat_materiel unique(designation)
)
go

create table type_OS
(
	id int,
	designation varchar(20) not null,
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime
	constraint pk_type_OS primary key(id),
	constraint uk_type_OS unique(designation)
)
go

create table architecture_OS
(
	id int,
	designation varchar(10) not null,
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime
	constraint pk_architecture_OS primary key(id),
	constraint uk_architecture_OS unique(designation)
)
go

create table OS
(
	id int,
	id_type_OS int not null,
	id_architecture_OS int not null,
	designation varchar(50) not null,
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime
	constraint pk_OS primary key(id),
	constraint fk_OS_typeOS foreign key(id_type_OS) references type_OS(id),
	constraint fk_OS_architecture_OS foreign key(id_architecture_OS) references architecture_OS(id)
)
go

create table version_ios
(
	id int,
	designation varchar(10) not null,
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime
	constraint pk_version_ios primary key(id),
	constraint uk_version_ios unique(designation)
)
go

create table netette
(
	id int,
	designation varchar(20) not null,
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime
	constraint pk_netette primary key(id),
	constraint uk_netette unique(designation)
)
go

create table garantie
(
	id int,
	valeur int not null,
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime
	constraint pk_guarantie primary key(id),
	constraint uk_guarantie unique(valeur)
)
go

create table ram
(
	id int,
	valeur float not null,
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime
	constraint pk_ram primary key(id),
	constraint uk_ram unique(valeur)
)
go

create table processeur
(
	id int,
	valeur float not null,
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime
	constraint pk_processeur primary key(id),
	constraint uk_processeur unique(valeur)
)
go

create table nombre_coeur_processeur
(
	id int,
	valeur int not null,
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime
	constraint pk_nombre_coeur_processeur primary key(id),
	constraint uk_nombre_coeur_processeur unique(valeur)
)
go

create table type_hdd
(
	id int,
	designation varchar(50) not null,
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime
	constraint pk_nombre_type_hdd primary key(id),
	constraint uk_nombre_type_hdd unique(designation)
)
go

create table nombre_hdd
(
	id int,
	valeur int not null,
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime
	constraint pk_nombre_hdd primary key(id),
	constraint uk_nombre_hdd unique(valeur)
)
go

create table capacite_hdd
(
	id int,
	valeur int not null,
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime
	constraint pk_capacite_hdd primary key(id),
	constraint uk_capacite_hdd unique(valeur)
)
go

create table taille_ecran
(
	id int,
	valeur int not null,
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime
	constraint pk_taille_ecran primary key(id),
	constraint uk_taille_ecran unique(valeur)
)
go

create table usb2
(
	id int,
	valeur int not null,
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime
	constraint pk_usb2 primary key(id),
	constraint uk_usb2 unique(valeur)
)
go

create table usb3
(
	id int,
	valeur int not null,
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime
	constraint pk_usb3 primary key(id),
	constraint uk_usb3 unique(valeur)
)
go

create table hdmi
(
	id int,
	valeur int not null,
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime
	constraint pk_hdmi primary key(id),
	constraint uk_hdmi unique(valeur)
)
go

create table vga
(
	id int,
	valeur int not null,
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime
	constraint pk_vga primary key(id),
	constraint uk_vga unique(valeur)
)
go

create table tension_batterie
(
	id int,
	valeur float not null,
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime
	constraint pk_tension_batterie primary key(id),
	constraint uk_tension_batterie unique(valeur)
)
go

create table tension_adaptateur
(
	id int,
	valeur float not null,
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime
	constraint pk_tension_adaptateur primary key(id),
	constraint uk_tension_adaptateur unique(valeur)
)
go

create table puissance_adaptateur
(
	id int,
	valeur float not null,
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime
	constraint pk_puissance_adaptateur primary key(id),
	constraint uk_puissance_adaptateur unique(valeur)
)
go

create table intensite_adaptateur
(
	id int,
	valeur float not null,
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime
	constraint pk_intensite_adaptateur primary key(id),
	constraint uk_intensite_adaptateur unique(valeur)
)
go

create table puissance
(
	id int,
	valeur float not null,
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime
	constraint pk_puissance primary key(id),
	constraint uk_puissance unique(valeur)
)
go

create table intensite
(
	id int,
	valeur float not null,
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime
	constraint pk_intensite primary key(id),
	constraint uk_intensite unique(valeur)
)
go

create table page_par_minute
(
	id int,
	valeur float not null,
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime
	constraint pk_page_par_minute primary key(id),
	constraint uk_page_par_minute unique(valeur)
)
go

create table tension_alimentation
(
	id int,
	valeur int not null,
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime
	constraint pk_tension_alimentation primary key(id),
	constraint uk_tension_alimentation unique(valeur)
)
go

--Nombre usb
create table usb
(
	id int,
	valeur int not null,
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime
	constraint pk_usb primary key(id),
	constraint uk_usb unique(valeur)
)
go

--Nombre memoire
create table memoire
(
	id int,
	valeur int not null,
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime
	constraint pk_memoire primary key(id),
	constraint uk_memoire unique(valeur)
)
go

--Nombre sorties_audio
create table sorties_audio
(
	id int,
	valeur int not null,
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime
	constraint pk_sorties_audio primary key(id),
	constraint uk_sorties_audio unique(valeur)
)
go

--Nombre microphone
create table microphone
(
	id int,
	valeur int not null,
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime
	constraint pk_microphone primary key(id),
	constraint uk_microphone unique(valeur)
)
go

--gain de l'appareil
create table gain
(
	id int,
	valeur int not null,
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime
	constraint pk_gain primary key(id),
	constraint uk_gain unique(valeur)
)
go

--Nombre port gigabit eth
create table gbe
(
	id int,
	valeur int not null,
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime
	constraint pk_gbe primary key(id),
	constraint uk_gbe unique(valeur)
)
go

--Nombre port fast eth
create table fe
(
	id int,
	valeur int not null,
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime
	constraint pk_fe primary key(id),
	constraint uk_fe unique(valeur)
)
go

--Nombre port pour Fibre Optique
create table fo
(
	id int,
	valeur int not null,
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime
	constraint pk_fo primary key(id),
	constraint uk_fo unique(valeur)
)
go

--Nombre port Serie
create table serial
(
	id int,
	valeur int not null,
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime
	constraint pk_serial primary key(id),
	constraint uk_serial unique(valeur)
)
go

--Adresse IP par defaut
create table default_ip
(
	id int,
	designation varchar(50) not null,
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime
	constraint pk_default_ip primary key(id),
	constraint uk_default_ip unique(designation)
)
go

--Mot de passe 
create table default_pwd
(
	id int,
	designation varchar(20) not null,
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime,
	constraint pk_default_pwd primary key(id),
	constraint uk_default_pwd unique(designation)
)
go

--Nombre port Console
create table console
(
	id int,
	valeur int not null,
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime
	constraint pk_console primary key(id),
	constraint uk_console unique(valeur)
)
go

--Nombre port Auxiliaires
create table auxiliaire
(
	id int,
	valeur int not null,
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime
	constraint pk_auxiliaire primary key(id),
	constraint uk_auxiliaire unique(valeur)
)
go

--Fréquence de fonctionnement
create table frequence
(
	id int,
	designation varchar(20) not null,
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime
	constraint pk_frequence primary key(id),
	constraint uk_frequence unique(designation)
)
go

--Nombre d'antennes
create table antenne
(
	id int,
	valeur int not null,
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime
	constraint pk_antenne primary key(id),
	constraint uk_antenne unique(valeur)
)
go

create table materiel
(
	id int,
	code_str varchar(10) not null,--Auto generate id+id_categorie_materiel
	id_categorie_materiel int not null,
	id_compte int not null,
	qrcode image not null,
	date_acquisition smalldatetime,
	id_garantie int,
	id_marque int not null,
	id_modele int not null,
	id_couleur int not null,
	id_poids int not null,
	id_etat_materiel int not null,
	photo1 image,
	photo2 image,
	photo3 image,
	label varchar(20),
	mac_adresse1 varchar(20),
	mac_adresse2 varchar(20),
	commentaire varchar(400),
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime,
	archiver bit default 0,
	
	--Partie non commune
	--Ordinateur
	id_type_ordinateur int,
	id_type_clavier int,
	id_OS int,																																							
	id_ram int,
	id_processeur int,
	id_nombre_coeur_processeur int,
	id_type_hdd int,
	id_nombre_hdd int,
	id_capacite_hdd int,
	id_taille_ecran int,
	id_usb2 int,
	id_usb3 int,
	id_hdmi int,
	id_vga int,
	id_tension_batterie int,
	id_tension_adaptateur int,
	id_puissance_adaptateur int,
	id_intensite_adaptateur int,
	numero_cle int,
	--Imprimante
	id_type_imprimante int,
	id_puissance int,
	id_intensite int,
	id_page_par_minute int,
	--Amplificateur
	id_type_amplificateur int,
	id_tension_alimentation int,
	id_usb int,
	id_memoire int,
	id_sorties_audio int,
	id_microphone int,
	id_gain int,
	--RouteurAP
	id_type_routeur_AP int,
	id_version_ios int,
	id_gbe int,
	id_fe int,
	id_fo int,
	id_serial int,
	capable_usb bit,
	id_default_pwd int,
	id_default_ip int,
	id_console int,
	id_auxiliaire int,
	--AP
	id_type_AP int, --utiliser id_tension_alimentation pour Amplificateur  
	id_portee int,
	--Switch
	id_type_switch int,--, utiliser id_tension_alimentation pour Amplificateur
	--Emetteur
	id_frequence int,
	--alimentation varchar(20), utiliser id_tension_alimentation pour Amplificateur
	id_antenne int,
	--Retroprojecteurs
	id_netette int,
	compatible_wifi bit,
	constraint pk_materiel primary key(id),
	constraint fk_materiel_categorie_materiel foreign key(id_categorie_materiel) references categorie_materiel(id),
	constraint fk_materiel_compte foreign key(id_compte) references compte(id),
	constraint fk_materiel_OS foreign key(id_OS) references OS(id),
	constraint fk_materiel_marque foreign key(id_marque) references marque(id),
	constraint fk_materiel_modele foreign key(id_modele) references modele(id),
	constraint fk_materiel_couleur foreign key(id_couleur) references couleur(id),
	constraint fk_materiel_poids foreign key(id_poids) references poids(id),
	constraint fk_materiel_etat_materiel foreign key(id_etat_materiel) references etat_materiel(id),
	constraint fk_materiel_type_ordinateur foreign key(id_type_ordinateur) references type_ordinateur(id),
	constraint fk_materiel_type_clavier foreign key(id_type_clavier) references type_clavier(id),
	constraint fk_materiel_netette foreign key(id_netette) references netette(id),
	constraint fk_materiel_type_switch foreign key(id_type_switch) references type_switch(id),
	constraint fk_materiel_typeAP foreign key(id_type_AP) references type_AP(id),
	constraint fk_materiel_portee foreign key(id_portee) references portee(id),
	constraint fk_materiel_id_type_routeur_AP foreign key(id_type_routeur_AP) references type_routeur_AP(id),
	constraint fk_materiel_version_ios foreign key(id_version_ios) references version_ios(id),
	constraint fk_materiel_type_amplificateur foreign key(id_type_amplificateur) references type_amplificateur(id),
	constraint fk_materiel_type_imprimante foreign key(id_type_imprimante) references type_imprimante(id),
	constraint fk_materiel_garantie foreign key(id_garantie) references garantie(id),
	constraint fk_materiel_ram foreign key(id_ram) references ram(id),
	constraint fk_materiel_processeur foreign key(id_processeur) references processeur(id),
	constraint fk_materiel_nombre_coeur_processeur foreign key(id_nombre_coeur_processeur) references nombre_coeur_processeur(id),
	constraint fk_materiel_nombre_type_hdd foreign key(id_type_hdd) references type_hdd(id),
	constraint fk_materiel_nombre_hdd foreign key(id_nombre_hdd) references nombre_hdd(id),
	constraint fk_materiel_capacite_hdd foreign key(id_capacite_hdd) references capacite_hdd(id),
	constraint fk_materiel_taille_ecran foreign key(id_taille_ecran) references taille_ecran(id),
	constraint fk_materiel_usb2 foreign key(id_usb2) references usb2(id),
	constraint fk_materiel_usb3 foreign key(id_usb3) references usb3(id),
	constraint fk_materiel_hdmi foreign key(id_hdmi) references hdmi(id),
	constraint fk_materiel_vga foreign key(id_vga) references vga(id),
	constraint fk_materiel_tension_batterie foreign key(id_tension_batterie) references tension_batterie(id),
	constraint fk_materiel_tension_adaptateur foreign key(id_tension_adaptateur) references tension_adaptateur(id),
	constraint fk_materiel_puissance_adaptateur foreign key(id_puissance_adaptateur) references puissance_adaptateur(id),
	constraint fk_materiel_intensite_adaptateur foreign key(id_intensite_adaptateur) references intensite_adaptateur(id),
	constraint fk_materiel_puissance foreign key(id_puissance) references puissance(id),
	constraint fk_materiel_intensite foreign key(id_intensite) references intensite(id),
	constraint fk_materiel_page_par_minute foreign key(id_page_par_minute) references page_par_minute(id),
	constraint fk_materiel_tension_alimentation foreign key(id_tension_alimentation) references tension_alimentation(id),
	constraint fk_materiel_usb foreign key(id_usb) references usb(id),
	constraint fk_materiel_memoire foreign key(id_memoire) references memoire(id),
	constraint fk_materiel_sorties_audio foreign key(id_sorties_audio) references sorties_audio(id),
	constraint fk_materiel_microphone foreign key(id_microphone) references microphone(id),
	constraint fk_materiel_gain foreign key(id_gain) references gain(id),
	constraint fk_materiel_gbe foreign key(id_gbe) references gbe(id),
	constraint fk_materiel_fe foreign key(id_fe) references fe(id),
	constraint fk_materiel_fo foreign key(id_fo) references fo(id),
	constraint fk_materiel_serial foreign key(id_serial) references serial(id),
	constraint fk_materiel_default_ip foreign key(id_default_ip) references default_ip(id),
	constraint fk_materiel_default_pwd foreign key(id_default_pwd) references default_pwd(id),
	constraint fk_materiel_console foreign key(id_console) references console(id),
	constraint fk_materiel_auxiliaire foreign key(id_auxiliaire) references auxiliaire(id),
	constraint fk_materiel_frequence foreign key(id_frequence) references frequence(id),
	constraint fk_materiel_antenne foreign key(id_antenne) references antenne(id),
	constraint uk_materiel unique(code_str)
)
go

create table grade
(
	id int,
	designation varchar(50) not null,
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime
	constraint pk_grade primary key(id),
	constraint uk_grade unique(designation)
)
go

create table personne
(
	id int,
	nom varchar(50) not null,
	postnom varchar(30),
	prenom varchar(30),
	id_grade int not null,
	isenseignant bit,
	isagent bit,
	isetudiant bit,	
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime
	constraint pk_personne primary key(id),
	constraint fk_personne_grade foreign key(id_grade) references grade(id),
	constraint uk_nom unique(nom,postnom,prenom)
)
go

create table type_lieu_affectation
(
	id int,
	designation varchar(50) not null,
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime
	constraint pk_type_lieu_affectation primary key(id),
	constraint uk_type_lieu_affectation unique(designation)
)
go

create table AC
(
	id int,
	code_str varchar(50) not null,
	designation varchar(50) not null,
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime
	constraint pk_AC primary key(id),
	constraint uk_AC unique(code_str)
)
go

create table optio
(
	id int,
	designation varchar(50) not null,
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime
	constraint pk_optio primary key(id),
	constraint uk_optio unique(designation)
)
go

create table promotion
(
	id int,
	designation varchar(50) not null,
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime
	constraint pk_promotion primary key(id),
	constraint uk_promotion unique(designation)
)
go

create table section
(
	id int,
	designation1 varchar(5) not null,
	designation2 varchar(30) not null,
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime
	constraint pk_section primary key(id),
	constraint uk_section unique(designation1,designation2)
)
go

create table retrait_materiel
(
	id int,
	id_personne int not null,
	code_AC varchar(50) not null,
	id_optio int,
	id_promotion int,
	id_section int,
	date_retrait datetime not null,
	retirer bit not null,
	deposer bit,	
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime
	constraint pk_retrait_materiel primary key(id),
	constraint fk_retrait_materiel_personne foreign key(id_personne) references personne(id),
	constraint fk_retrait_materiel_AC foreign key(code_AC) references AC(code_str),
	constraint fk_retrait_materiel_optio foreign key(id_optio) references optio(id),
	constraint fk_retrait_materiel_promotion foreign key(id_promotion) references promotion(id),
	constraint fk_retrait_materiel_section foreign key(id_section) references section(id)
)
go

create table detail_retrait_materiel
(
	id int,
	id_retrait_materiel int not null,
	id_materiel int not null,
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime
	constraint pk_detail_retrait_materiel primary key(id),
	constraint fk_detail_retrait_materiel_retrait_materiel foreign key(id_retrait_materiel) references retrait_materiel(id),
	constraint fk_detail_retrait_materiel_materiel foreign key(id_materiel) references materiel(id)
)
go

create table signataire
(
	id int,
	id_personne int not null,
	code_AC varchar(50) not null,
	signature_specimen text not null,
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime
	constraint pk_signataire primary key(id),
	constraint fk_signataire_personne foreign key(id_personne) references personne(id),
	constraint fk_signataire_AC foreign key(code_AC) references AC(code_str)
)
go

create table salle
(
	id int,
	designation varchar(50) not null,
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime
	constraint pk_salle primary key(id),
	constraint uk_salle unique(designation)
)
go

create table fonction
(
	id int,
	designation varchar(50) not null,
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime
	constraint pk_fonction primary key(id)
)
go

create table lieu_affectation
(
	id int,	
	code_AC varchar(50) not null,
	id_type_lieu_affectation int not null,
	id_personne int,
	id_fonction int,
	designation varchar(50),
	date_affectation smalldatetime not null,
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime
	constraint pk_lieu_affectation primary key(id),
	constraint fk_lieu_affectation_AC foreign key(code_AC) references AC(code_str),
	constraint fk_lieu_affectation_type_lieu_affectation foreign key(id_type_lieu_affectation) references type_lieu_affectation(id),
	constraint fk_lieu_affectation_personne foreign key(id_personne) references personne(id),
	constraint fk_lieu_affectation_fonction foreign key(id_fonction) references fonction(id),
	constraint uk_lieu_affectation unique(code_AC,id_type_lieu_affectation,id_fonction)
)
go

create table affectation_materiel
(
	id int,	
	code_AC varchar(50) not null,
	id_lieu_affectation int not null,
	id_materiel int not null,
	id_salle int,
	date_affectation smalldatetime not null,
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime
	constraint pk_affectation_materiel primary key(id),
	constraint fk_affectation_materiel_AC foreign key(code_AC) references AC(code_str),
	constraint fk_affectation_materiel_lieu_affectation foreign key(id_lieu_affectation) references lieu_affectation(id),
	constraint fk_affectation_materiel_materiel foreign key(id_materiel) references materiel(id),
	constraint fk_affectation_materiel_salle foreign key(id_salle) references salle(id),
	constraint uk_affectation_materiel unique(code_AC,id_lieu_affectation,id_salle)
)
go

create table utilisateur
(
	id int,
	id_personne int not null,
	nomuser varchar(30) not null,
	motpass varchar(1000) not null,
	schema_user varchar(20) not null,
	droits varchar(300) default 'Aucun',
	activation bit,
	constraint pk_utilisateur primary key(id),
	constraint fk_utilisateur_id_personne foreign key(id_personne) references personne(id),
	constraint uk_utilisateur unique(nomuser)
)
go

create table groupe
(
	id int,
	designation varchar(30) not null,
	niveau int,
	constraint pk_groupe primary key(id)
)
go

--Insertion des data de base
insert into groupe(id,designation,niveau) values
(1,'Administrateur',0),(2,'Admin',1),(3,'User',2)

insert into compte(id,numero,user_created,date_created,user_modified,date_modified) values
(1,'2220','sa',GETDATE(),null,null),(2,'227','sa',GETDATE(),null,null)

insert into categorie_materiel(id,designation,user_created,date_created,user_modified,date_modified) values
(1,'Ordinateur','sa',GETDATE(),null,null),(2,'Switch','sa',GETDATE(),null,null),
(3,'Imprimante','sa',GETDATE(),null,null),(4,'Emetteur','sa',GETDATE(),null,null),
(5,'Amplificateur','sa',GETDATE(),null,null),(6,'Retroprojecteur','sa',GETDATE(),null,null),
(7,'Routeur_Access Point','sa',GETDATE(),null,null),(8,'Access Point','sa',GETDATE(),null,null),
(9,'Autre','sa',GETDATE(),null,null)

insert into type_amplificateur(id,designation,user_created,date_created,user_modified,date_modified) values
(1,'Avec baffle','sa',GETDATE(),null,null),(2,'Sans baffle','sa',GETDATE(),null,null)

insert into type_AP(id,designation,user_created,date_created,user_modified,date_modified) values
(1,'Avec POE','sa',GETDATE(),null,null),(2,'Sans POE','sa',GETDATE(),null,null)

insert into type_routeur_AP(id,designation,user_created,date_created,user_modified,date_modified) values
(1,'Manageable','sa',GETDATE(),null,null),(2,'Non manageable','sa',GETDATE(),null,null)

insert into type_switch(id,designation,user_created,date_created,user_modified,date_modified) values
(1,'Manageable','sa',GETDATE(),null,null),(2,'Non manageable','sa',GETDATE(),null,null)

insert into version_ios(id,designation,user_created,date_created,user_modified,date_modified) values
(1,'12.02','sa',GETDATE(),null,null),(2,'15.01','sa',GETDATE(),null,null)

insert into type_clavier(id,designation,user_created,date_created,user_modified,date_modified) values
(1,'Azerty','sa',GETDATE(),null,null),(2,'Qwerty','sa',GETDATE(),null,null)

insert into type_imprimante(id,designation,user_created,date_created,user_modified,date_modified) values
(1,'Réseau','sa',GETDATE(),null,null),(2,'Wifi','sa',GETDATE(),null,null),
(3,'Non réseau','sa',GETDATE(),null,null)

insert into type_ordinateur(id,designation,user_created,date_created,user_modified,date_modified) values
(1,'Serveur','sa',GETDATE(),null,null),(2,'Laptop','sa',GETDATE(),null,null),
(3,'Desktop','sa',GETDATE(),null,null),(4,'Mini serveur','sa',GETDATE(),null,null)

insert into type_OS(id,designation,user_created,date_created,user_modified,date_modified) values
(1,'Windows','sa',GETDATE(),null,null),(2,'Linux','sa',GETDATE(),null,null),(3,'MAC','sa',GETDATE(),null,null)

insert into architecture_OS(id,designation,user_created,date_created,user_modified,date_modified) values
(1,'32Bits','sa',GETDATE(),null,null),(2,'64Bits','sa',GETDATE(),null,null)

insert into OS(id,id_type_OS,id_architecture_OS,designation,user_created,date_created,user_modified,date_modified) values
(1,1,1,'7 SP1 Professionnel','sa',GETDATE(),null,null),(2,1,2,'7 SP1 Professionnel','sa',GETDATE(),null,null),
(3,1,1,'7 SP2 Professionnel','sa',GETDATE(),null,null),(4,1,2,'7 SP2 Professionnel','sa',GETDATE(),null,null),
(5,1,1,'7 SP1 Intégral','sa',GETDATE(),null,null),(6,1,2,'7 SP1 Intégral','sa',GETDATE(),null,null),
(7,1,1,'7 SP2 Intégral','sa',GETDATE(),null,null),(8,1,2,'7 SP2 Intégral','sa',GETDATE(),null,null),
(9,1,1,'8 Professionnel','sa',GETDATE(),null,null),(10,1,2,'8 Professionnel','sa',GETDATE(),null,null),
(11,1,1,'8.1 Professionnel','sa',GETDATE(),null,null),(12,1,2,'8.1 Professionnel','sa',GETDATE(),null,null),
(13,1,2,'10 Professionnel','sa',GETDATE(),null,null),(14,1,2,'Server 2008 R2 Entreprise','sa',GETDATE(),null,null),
(15,1,2,'Server 2008','sa',GETDATE(),null,null),(16,2,2,'Ubuntu 14.04','sa',GETDATE(),null,null),
(17,2,2,'Ubuntu 16.04','sa',GETDATE(),null,null)

insert into netette(id,designation,user_created,date_created,user_modified,date_modified) values
(1,'Excellente','sa',GETDATE(),null,null),(2,'Bonne','sa',GETDATE(),null,null),
(3,'Moyenne','sa',GETDATE(),null,null),(4,'Mauvaise','sa',GETDATE(),null,null)

insert into couleur(id,designation,user_created,date_created,user_modified,date_modified) values
(1,'Noire','sa',GETDATE(),null,null),(2,'Noir','sa',GETDATE(),null,null),
(3,'Blanche','sa',GETDATE(),null,null),(4,'Blanc','sa',GETDATE(),null,null),
(5,'Gris-Noire','sa',GETDATE(),null,null),(6,'Gris-Noir','sa',GETDATE(),null,null),
(7,'Vert-Noire','sa',GETDATE(),null,null),(8,'Vert-Noir','sa',GETDATE(),null,null),
(9,'Grise','sa',GETDATE(),null,null),(10,'Gris','sa',GETDATE(),null,null),
(11,'Beige','sa',GETDATE(),null,null)

insert into etat_materiel(id,designation,user_created,date_created,user_modified,date_modified) values
(1,'Bon','sa',GETDATE(),null,null),(2,'En panne','sa',GETDATE(),null,null),
(3,'Hors service','sa',GETDATE(),null,null),(4,'Obsolète','sa',GETDATE(),null,null),
(5,'Thereza Mira','sa',GETDATE(),null,null)

insert into marque(id,designation,user_created,date_created,user_modified,date_modified) values
(1,'HP','sa',GETDATE(),null,null),(2,'Toshiba satellite Pro','sa',GETDATE(),null,null),
(3,'HP Pavillon','sa',GETDATE(),null,null),(4,'HPCompaq','sa',GETDATE(),null,null),
(5,'Dell','sa',GETDATE(),null,null),(6,'Acer','sa',GETDATE(),null,null),
(7,'Sony','sa',GETDATE(),null,null),(8,'Elonex Prosentia','sa',GETDATE(),null,null),
(9,'Epson','sa',GETDATE(),null,null),(10,'Benq','sa',GETDATE(),null,null),
(11,'Max','sa',GETDATE(),null,null),(12,'PIACING','sa',GETDATE(),null,null),
(13,'HP Deskjet','sa',GETDATE(),null,null),(14,'D-Link','sa',GETDATE(),null,null),
(15,'Cisco','sa',GETDATE(),null,null),(16,'Shure','sa',GETDATE(),null,null),
(17,'HP Notebook','sa',GETDATE(),null,null),(18,'Canon','sa',GETDATE(),null,null),
(19,'Asus','sa',GETDATE(),null,null),(20,'Scenic Fujitsu Siemens','sa',GETDATE(),null,null),
(21,'Compaq','sa',GETDATE(),null,null),(22,'Lenovo','sa',GETDATE(),null,null),
(23,'IBM','sa',GETDATE(),null,null),(24,'Siemens','sa',GETDATE(),null,null)

insert into modele(id,designation,user_created,date_created,user_modified,date_modified) values
(1,'620','sa',GETDATE(),null,null),(2,'650','sa',GETDATE(),null,null),
(3,'630','sa',GETDATE(),null,null),(4,'EH-TW550 H499B','sa',GETDATE(),null,null),
(5,'EH-TW490 H558B','sa',GETDATE(),null,null),(6,'2050A','sa',GETDATE(),null,null),
(7,'510','sa',GETDATE(),null,null),(8,'2000','sa',GETDATE(),null,null),
(9,'EB-S31','sa',GETDATE(),null,null),(10,'X751SA','sa',GETDATE(),null,null),
(11,'VPL-DX100','sa',GETDATE(),null,null),(12,'V-3100','sa',GETDATE(),null,null),
(13,'VPL-DX220','sa',GETDATE(),null,null),(14,'H719B','sa',GETDATE(),null,null),
(15,'DES-1016A','sa',GETDATE(),null,null),(16,'J1812','sa',GETDATE(),null,null),
(17,'AV-6080B','sa',GETDATE(),null,null),(18,'WG-2186','sa',GETDATE(),null,null),
(19,'DH-744','sa',GETDATE(),null,null),(20,'DH-755','sa',GETDATE(),null,null),
(21,'EP4227','sa',GETDATE(),null,null),(22,'1211S','sa',GETDATE(),null,null)

insert into garantie(id,valeur,user_created,date_created,user_modified,date_modified) values
(1,1,'sa',GETDATE(),null,null),(2,2,'sa',GETDATE(),null,null),
(3,3,'sa',GETDATE(),null,null),(4,5,'sa',GETDATE(),null,null)

insert into ram(id,valeur,user_created,date_created,user_modified,date_modified) values
(1,1,'sa',GETDATE(),null,null),(2,2,'sa',GETDATE(),null,null),
(3,3,'sa',GETDATE(),null,null),(4,4,'sa',GETDATE(),null,null)

insert into processeur(id,valeur,user_created,date_created,user_modified,date_modified) values
(1,1,'sa',GETDATE(),null,null),(2,1.6,'sa',GETDATE(),null,null),
(3,2,'sa',GETDATE(),null,null),(4,2.2,'sa',GETDATE(),null,null)

insert into nombre_coeur_processeur(id,valeur,user_created,date_created,user_modified,date_modified) values
(1,1,'sa',GETDATE(),null,null),(2,2,'sa',GETDATE(),null,null),
(3,3,'sa',GETDATE(),null,null),(4,4,'sa',GETDATE(),null,null)

insert into type_hdd(id,designation,user_created,date_created,user_modified,date_modified) values
(1,'IDE','sa',GETDATE(),null,null),(2,'SATA','sa',GETDATE(),null,null),
(3,'SSD','sa',GETDATE(),null,null)

insert into capacite_hdd(id,valeur,user_created,date_created,user_modified,date_modified) values
(1,78,'sa',GETDATE(),null,null),(2,74,'sa',GETDATE(),null,null),
(3,233,'sa',GETDATE(),null,null),(4,300,'sa',GETDATE(),null,null),
(5,450,'sa',GETDATE(),null,null),(6,465,'sa',GETDATE(),null,null),
(7,500,'sa',GETDATE(),null,null)

insert into nombre_hdd(id,valeur,user_created,date_created,user_modified,date_modified) values
(1,1,'sa',GETDATE(),null,null),(2,2,'sa',GETDATE(),null,null)

insert into taille_ecran(id,valeur,user_created,date_created,user_modified,date_modified) values
(1,14,'sa',GETDATE(),null,null),(2,15,'sa',GETDATE(),null,null),
(3,17,'sa',GETDATE(),null,null),(4,24,'sa',GETDATE(),null,null)

insert into usb2(id,valeur,user_created,date_created,user_modified,date_modified) values
(1,1,'sa',GETDATE(),null,null),(2,2,'sa',GETDATE(),null,null),
(3,3,'sa',GETDATE(),null,null),(4,4,'sa',GETDATE(),null,null)

insert into usb3(id,valeur,user_created,date_created,user_modified,date_modified) values
(1,1,'sa',GETDATE(),null,null),(2,2,'sa',GETDATE(),null,null),
(3,3,'sa',GETDATE(),null,null),(4,4,'sa',GETDATE(),null,null)

insert into hdmi(id,valeur,user_created,date_created,user_modified,date_modified) values
(1,1,'sa',GETDATE(),null,null),(2,2,'sa',GETDATE(),null,null),
(3,3,'sa',GETDATE(),null,null)

insert into vga(id,valeur,user_created,date_created,user_modified,date_modified) values
(1,1,'sa',GETDATE(),null,null),(2,2,'sa',GETDATE(),null,null),
(3,3,'sa',GETDATE(),null,null)

insert into tension_batterie(id,valeur,user_created,date_created,user_modified,date_modified) values
(1,10.8,'sa',GETDATE(),null,null),(2,12,'sa',GETDATE(),null,null),
(3,14,'sa',GETDATE(),null,null)

insert into tension_adaptateur(id,valeur,user_created,date_created,user_modified,date_modified) values
(1,15,'sa',GETDATE(),null,null),(2,18.5,'sa',GETDATE(),null,null),
(3,19,'sa',GETDATE(),null,null),(4,19.5,'sa',GETDATE(),null,null)

insert into puissance_adaptateur(id,valeur,user_created,date_created,user_modified,date_modified) values
(1,10.8,'sa',GETDATE(),null,null),(2,12,'sa',GETDATE(),null,null),
(3,14,'sa',GETDATE(),null,null)

insert into intensite_adaptateur(id,valeur,user_created,date_created,user_modified,date_modified) values
(1,3.33,'sa',GETDATE(),null,null),(2,3.5,'sa',GETDATE(),null,null),
(3,5,'sa',GETDATE(),null,null)

insert into usb(id,valeur,user_created,date_created,user_modified,date_modified) values
(1,1,'sa',GETDATE(),null,null),(2,2,'sa',GETDATE(),null,null)

insert into memoire(id,valeur,user_created,date_created,user_modified,date_modified) values
(1,1,'sa',GETDATE(),null,null),(2,2,'sa',GETDATE(),null,null)

insert into sorties_audio(id,valeur,user_created,date_created,user_modified,date_modified) values
(1,1,'sa',GETDATE(),null,null),(2,2,'sa',GETDATE(),null,null),
(3,3,'sa',GETDATE(),null,null)

insert into microphone(id,valeur,user_created,date_created,user_modified,date_modified) values
(1,1,'sa',GETDATE(),null,null),(2,2,'sa',GETDATE(),null,null)

insert into gain(id,valeur,user_created,date_created,user_modified,date_modified) values
(1,10,'sa',GETDATE(),null,null),(2,20,'sa',GETDATE(),null,null),
(3,30,'sa',GETDATE(),null,null)

insert into gbe(id,valeur,user_created,date_created,user_modified,date_modified) values
(1,1,'sa',GETDATE(),null,null),(2,2,'sa',GETDATE(),null,null),
(3,3,'sa',GETDATE(),null,null)

insert into fe(id,valeur,user_created,date_created,user_modified,date_modified) values
(1,1,'sa',GETDATE(),null,null),(2,2,'sa',GETDATE(),null,null),
(3,3,'sa',GETDATE(),null,null)

insert into fo(id,valeur,user_created,date_created,user_modified,date_modified) values
(1,1,'sa',GETDATE(),null,null),(2,2,'sa',GETDATE(),null,null),
(3,3,'sa',GETDATE(),null,null)

insert into serial(id,valeur,user_created,date_created,user_modified,date_modified) values
(1,1,'sa',GETDATE(),null,null),(2,2,'sa',GETDATE(),null,null),
(3,3,'sa',GETDATE(),null,null)

insert into console(id,valeur,user_created,date_created,user_modified,date_modified) values
(1,1,'sa',GETDATE(),null,null),(2,2,'sa',GETDATE(),null,null)

insert into auxiliaire(id,valeur,user_created,date_created,user_modified,date_modified) values
(1,1,'sa',GETDATE(),null,null),(2,2,'sa',GETDATE(),null,null)

insert into default_ip(id,designation,user_created,date_created,user_modified,date_modified) values
(1,'192.168.1.1','sa',GETDATE(),null,null),(2,'192.168.20.1','sa',GETDATE(),null,null),
(3,'192.168.0.1','sa',GETDATE(),null,null)

insert into default_pwd(id,designation,user_created,date_created,user_modified,date_modified) values
(1,'admin','sa',GETDATE(),null,null),(2,'cisco','sa',GETDATE(),null,null)

insert into frequence(id,designation,user_created,date_created,user_modified,date_modified) values
(1,'255,1-227,1','sa',GETDATE(),null,null),(2,'256,9-228,9','sa',GETDATE(),null,null),
(3,'263,4-239,4','sa',GETDATE(),null,null),(4,'247,1-218,2','sa',GETDATE(),null,null),
(5,'252,1-214,1','sa',GETDATE(),null,null),(6,'254,6-230,7','sa',GETDATE(),null,null)

insert into antenne(id,valeur,user_created,date_created,user_modified,date_modified) values
(1,0,'sa',GETDATE(),null,null),(2,1,'sa',GETDATE(),null,null),
(3,2,'sa',GETDATE(),null,null)

insert into AC(id,code_str,designation,user_created,date_created,user_modified,date_modified) values
(1,'2014-2015','2014-2015','sa',GETDATE(),null,null),(2,'2015-2016','2015-2016','sa',GETDATE(),null,null),
(3,'2016-2017','2016-2017','sa',GETDATE(),null,null),(4,'2017-2018','2017-2018','sa',GETDATE(),null,null),
(5,'2018-2019','2018-2019','sa',GETDATE(),null,null)

insert into poids(id,valeur,user_created,date_created,user_modified,date_modified) values
(1,2,'sa',GETDATE(),null,null),(2,3.5,'sa',GETDATE(),null,null),
(3,2.5,'sa',GETDATE(),null,null),(4,6,'sa',GETDATE(),null,null),
(5,3,'sa',GETDATE(),null,null),(6,4,'sa',GETDATE(),null,null),
(7,2.32,'sa',GETDATE(),null,null),(8,1,'sa',GETDATE(),null,null)

insert into salle(id,designation,user_created,date_created,user_modified,date_modified) values
(1,'BUGANDWA','sa',GETDATE(),null,null),(2,'KATULANYA','sa',GETDATE(),null,null),
(3,'ISSU','sa',GETDATE(),null,null),(4,'Alain Odon','sa',GETDATE(),null,null),
(5,'A coté du Goupe','sa',GETDATE(),null,null),(6,'A coté Bureau DG','sa',GETDATE(),null,null),
(7,'Laboratoir Informatique','sa',GETDATE(),null,null)

insert into grade(id,designation,user_created,date_created,user_modified,date_modified) values
(1,'PO','sa',GETDATE(),null,null),(2,'PA','sa',GETDATE(),null,null),
(3,'P','sa',GETDATE(),null,null),(4,'Prof. Dr.','sa',GETDATE(),null,null),
(5,'DEA','sa',GETDATE(),null,null),(6,'CT','sa',GETDATE(),null,null),
(7,'Ass.','sa',GETDATE(),null,null),(8,'Ass2.','sa',GETDATE(),null,null),
(9,'Dr.','sa',GETDATE(),null,null),(10,'CPP','sa',GETDATE(),null,null),
(11,'Mr.','sa',GETDATE(),null,null),(12,'Mme','sa',GETDATE(),null,null),
(13,'Mlle','sa',GETDATE(),null,null),(14,'Ir','sa',GETDATE(),null,null),
(15,'PP','sa',GETDATE(),null,null),(16,'Doctorant','sa',GETDATE(),null,null)

insert into type_lieu_affectation(id,designation,user_created,date_created,user_modified,date_modified) values
(1,'Bureau','sa',GETDATE(),null,null),(2,'Auditoire','sa',GETDATE(),null,null),
(3,'Enseignant','sa',GETDATE(),null,null),(4,'Cave','sa',GETDATE(),null,null)

insert into fonction(id,designation,user_created,date_created,user_modified,date_modified) values
(1,'DSIG','sa',GETDATE(),null,null),(2,'Resp. Labo. Info','sa',GETDATE(),null,null),
(3,'Chef de Section','sa',GETDATE(),null,null),(4,'Appariteur Central','sa',GETDATE(),null,null),
(5,'Comptable','sa',GETDATE(),null,null),(6,'Trésorier','sa',GETDATE(),null,null),
(7,'Bibliothécaire','sa',GETDATE(),null,null),(8,'Resp. Salle de lecture','sa',GETDATE(),null,null),
(9,'Chef de Département IG','sa',GETDATE(),null,null),(10,'Chef de Département GIMFs et GD','sa',GETDATE(),null,null),
(11,'Chef de Département SP et GIS','sa',GETDATE(),null,null),(12,'Maître de stage','sa',GETDATE(),null,null),
(13,'Chef gardien Jour','sa',GETDATE(),null,null),(14,'Chef gardien Nuit','sa',GETDATE(),null,null),
(15,'Logisticien','sa',GETDATE(),null,null),(16,'Resp. Cyber café','sa',GETDATE(),null,null),
(17,'Secrétaire de section','sa',GETDATE(),null,null),(18,'SG Adm et Fin','sa',GETDATE(),null,null),
(19,'SG Académique','sa',GETDATE(),null,null),(20,'Directeur Général','sa',GETDATE(),null,null),
(21,'Adj. Section chargé de l''enseignement','sa',GETDATE(),null,null),(22,'Webmaster','sa',GETDATE(),null,null),
(23,'Administrateur réseau','sa',GETDATE(),null,null)

insert into personne(id,nom,postnom,prenom,id_grade,isenseignant,isagent,isetudiant,user_created,date_created,user_modified,date_modified) values
(1,'Zihindula','Biguru','Lucien',4,1,1,0,'sa',GETDATE(),null,null),(2,'Kasuku','Kalaba','Eric',16,1,1,0,'sa',GETDATE(),null,null),
(3,'Amani','Haguma','Joseph',6,1,1,0,'sa',GETDATE(),null,null),(4,'Isamuna','Nkembo','Josué',8,1,1,0,'sa',GETDATE(),null,null),
(5,'Fataki','Simba','Didier',6,1,1,0,'sa',GETDATE(),null,null),(6,'Kakule','Muhindo','Omer',5,1,1,0,'sa',GETDATE(),null,null)

insert into lieu_affectation(id,code_AC,id_type_lieu_affectation,id_personne,id_fonction,designation,date_affectation,user_created,date_created,user_modified,date_modified) values
(1,'2017-2018',1,3,1,'Direction du Système d''Information',GETDATE(),'sa',GETDATE(),null,null),
(2,'2017-2018',1,4,2,'Laboratoire Informatique',GETDATE(),'sa',GETDATE(),null,null),
(3,'2017-2018',1,4,23,'Administration réseau',GETDATE(),'sa',GETDATE(),null,null),
(4,'2017-2018',1,5,3,'Section',GETDATE(),'sa',GETDATE(),null,null),
(5,'2017-2018',1,6,11,'Département SP et GIS',GETDATE(),'sa',GETDATE(),null,null),
(6,'2017-2018',1,1,20,'Direction Générale',GETDATE(),'sa',GETDATE(),null,null),
(7,'2017-2018',1,2,18,'Secrétariat Général Adm et Fin',GETDATE(),'sa',GETDATE(),null,null)

--insert into signataire(id,id_personne,code_AC,signature_specimen,user_created,date_created,user_modified,date_modified) values
--(1,4,'2014-2015',null,'sa',GETDATE(),null,null),(2,4,'2015-2016',null,'sa',GETDATE(),null,null),
--(3,4,'2016-2017',null,'sa',GETDATE(),null,null),(4,4,'2017-2018',null,'sa',GETDATE(),null,null)

--Differentes categories des materiel qui ne devra pas etre change
--Ordinateur
SELECT *  FROM materiel INNER JOIN categorie_materiel ON categorie_materiel.id=materiel.id_categorie_materiel WHERE categorie_materiel.id=1 ORDER BY materiel.id ASC
--Switch
SELECT *  FROM materiel INNER JOIN categorie_materiel ON categorie_materiel.id=materiel.id_categorie_materiel WHERE categorie_materiel.id=2 ORDER BY materiel.id ASC
--Imprimante
SELECT *  FROM materiel INNER JOIN categorie_materiel ON categorie_materiel.id=materiel.id_categorie_materiel WHERE categorie_materiel.id=3 ORDER BY materiel.id ASC
--Emetteur
SELECT *  FROM materiel INNER JOIN categorie_materiel ON categorie_materiel.id=materiel.id_categorie_materiel WHERE categorie_materiel.id=4 ORDER BY materiel.id ASC
--Amplificateur
SELECT *  FROM materiel INNER JOIN categorie_materiel ON categorie_materiel.id=materiel.id_categorie_materiel WHERE categorie_materiel.id=5 ORDER BY materiel.id ASC
--Retroprojecteur
SELECT *  FROM materiel INNER JOIN categorie_materiel ON categorie_materiel.id=materiel.id_categorie_materiel WHERE categorie_materiel.id=6 ORDER BY materiel.id ASC
--Routeur_Access Point
SELECT *  FROM materiel INNER JOIN categorie_materiel ON categorie_materiel.id=materiel.id_categorie_materiel WHERE categorie_materiel.id=7 ORDER BY materiel.id ASC
--Access Point
SELECT *  FROM materiel INNER JOIN categorie_materiel ON categorie_materiel.id=materiel.id_categorie_materiel WHERE categorie_materiel.id=8 ORDER BY materiel.id ASC
