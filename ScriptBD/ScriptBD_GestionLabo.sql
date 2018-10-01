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

create table materiel
(
	id int,
	code_str varchar(10) not null,--Auto generate id+id_categorie_materiel
	id_categorie_materiel int not null,
	id_compte int not null,
	qrcode text not null,
	date_acquisition smalldatetime,
	guarantie int,
	id_marque int not null,
	id_modele int not null,
	id_couleur int not null,
	id_poids int not null,
	id_etat_materiel int not null,
	photo1 text,
	photo2 text,
	photo3 text,
	label varchar(20),
	mac_adresse1 varchar(20),
	mac_adresse2 varchar(20),
	commentaire varchar(400),
	user_created varchar(50),
	date_created smalldatetime,
	user_modified varchar(50),
	date_modified smalldatetime,
	
	--Partie non commune
	--Ordinateur
	id_type_ordinateur int,
	id_type_clavier int,
	id_OS int,																																							
	ram float,
	processeur float,
	nombre_coeur_processeur int,
	nombre_hdd int,
	capacite_hdd int,
	indice_performance float,
	pouce int,
	nombre_usb2 int,
	nombre_usb3 int,
	nombre_hdmi int,
	nombre_vga int,
	tension_batterie float,
	tension_adaptateur float,
	puissance_adaptateur float,
	intensite_adaptateur float,
	numero_cle int,
	--Imprimante
	id_type_imprimante int,
	puissance float,
	intensite float,
	nombre_page_par_minute float,
	--Amplificateur
	id_type_amplificateur int,
	tension_alimentation int,
	nombre_usb int,
	nombre_memoire int,
	nombre_sorties_audio int,
	nombre_microphone int,
	gain float,
	--RouteurAP
	id_type_routeur_AP int,
	id_version_ios int,
	nombre_gbe int,
	nombre_fe int,
	nombre_fo int,
	nombre_serial int,
	capable_usb bit,
	motpasse_defaut varchar(20),
	default_IP varchar(50),
	nombre_console int,
	nombre_auxiliaire int,
	--AP
	id_type_AP int,
	--tension_alimentation
	id_type_switch int,
	--Emetteur
	frequence float,
	alimentation varchar(20),
	nombre_antenne int,
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
	constraint fk_materiel_id_type_routeur_AP foreign key(id_type_routeur_AP) references type_routeur_AP(id),
	constraint fk_materiel_version_ios foreign key(id_version_ios) references version_ios(id),
	constraint fk_materiel_type_amplificateur foreign key(id_type_amplificateur) references type_amplificateur(id),
	constraint fk_materiel_type_imprimante foreign key(id_type_imprimante) references type_imprimante(id),
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
(7,'Routeur_Access Point','sa',GETDATE(),null,null),(8,'Access Point','sa',GETDATE(),null,null)

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
(1,1,1,'Windows 7 SP1 Professionnel','sa',GETDATE(),null,null),(2,1,2,'Windows 7 SP1 Professionnel','sa',GETDATE(),null,null),
(3,1,1,'Windows 7 SP2 Professionnel','sa',GETDATE(),null,null),(4,1,2,'Windows 7 SP2 Professionnel','sa',GETDATE(),null,null),
(5,1,1,'Windows 7 SP1 Intégral','sa',GETDATE(),null,null),(6,1,2,'Windows 7 SP1 Intégral','sa',GETDATE(),null,null),
(7,1,1,'Windows 7 SP2 Intégral','sa',GETDATE(),null,null),(8,1,2,'Windows 7 SP2 Intégral','sa',GETDATE(),null,null),
(9,1,1,'Windows 8 Professionnel','sa',GETDATE(),null,null),(10,1,2,'Windows 8 Professionnel','sa',GETDATE(),null,null),
(11,1,1,'Windows 8.1 Professionnel','sa',GETDATE(),null,null),(12,1,2,'Windows 8.1 Professionnel','sa',GETDATE(),null,null),
(13,1,2,'Windows 10 Professionnel','sa',GETDATE(),null,null),(14,1,2,'Windows Server 2008 R2 Entreprise','sa',GETDATE(),null,null),
(15,1,2,'Windows Server 2008','sa',GETDATE(),null,null),(16,2,2,'Ubuntu 14.04','sa',GETDATE(),null,null),
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
