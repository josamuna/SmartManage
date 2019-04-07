--exec sp_materiel_criteria '12'
--compte

CREATE PROCEDURE sp_compte_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM compte  WHERE 1=1
	OR numero LIKE @criteria
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
	ORDER BY numero ASC
END
GO

--fo
CREATE PROCEDURE sp_fo_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM fo  WHERE 1=1
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
	ORDER BY valeur ASC
END
GO

--groupe
CREATE PROCEDURE sp_groupe_criteria @criteria varchar(30)
AS 
BEGIN
	SELECT *  FROM groupe  WHERE 1=1
	OR designation LIKE @criteria
	ORDER BY designation ASC
END
GO

--serial
CREATE PROCEDURE sp_serial_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM serial  WHERE 1=1
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
END
GO

--marque
CREATE PROCEDURE sp_marque_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM marque  WHERE 1=1
	OR designation LIKE @criteria
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
	ORDER BY designation ASC
END
GO

--modele
CREATE PROCEDURE sp_modele_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM modele  WHERE 1=1
	OR designation LIKE @criteria
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
	ORDER BY designation ASC
END
GO

--default_ip
CREATE PROCEDURE sp_default_ip_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM default_ip  WHERE 1=1
	OR designation LIKE @criteria
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
	ORDER BY designation ASC
END
GO

--couleur
CREATE PROCEDURE sp_couleur_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM couleur  WHERE 1=1
	OR designation LIKE @criteria
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
	ORDER BY designation ASC
END
GO

--default_pwd
CREATE PROCEDURE sp_default_pwd_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM default_pwd  WHERE 1=1
	OR designation LIKE @criteria
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
	ORDER BY designation ASC
END
GO

--portee
CREATE PROCEDURE sp_portee_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM portee  WHERE 1=1
	OR valeur LIKE @criteria
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
	ORDER BY valeur ASC
END
GO

--poids
CREATE PROCEDURE sp_poids_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM poids  WHERE 1=1
	OR valeur LIKE @criteria
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
	ORDER BY valeur ASC
END
GO

--console
CREATE PROCEDURE sp_console_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM console  WHERE 1=1
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
	ORDER BY valeur ASC
END
GO

--type_ordinateur
CREATE PROCEDURE sp_type_ordinateur_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM type_ordinateur  WHERE 1=1
	OR designation LIKE @criteria
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
	ORDER BY designation ASC
END
GO

--auxiliaire
CREATE PROCEDURE sp_auxiliaire_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM auxiliaire  WHERE 1=1
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
	ORDER BY valeur ASC
END
GO

--telephone
CREATE PROCEDURE sp_telephone_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM telephone  WHERE 1=1
	OR code LIKE @criteria
	OR numero LIKE @criteria
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
	ORDER BY numero ASC
END
GO

--telephone°personne
CREATE PROCEDURE sp_telephone_personne_criteria @criteria varchar(50),@id int
AS 
BEGIN
	SELECT *  FROM telephone INNER JOIN personne 
	ON personne.id=telephone.id_personne WHERE personne.id=@id  
	OR telephone.code LIKE @criteria
	OR telephone.numero LIKE @criteria
	OR telephone.user_created LIKE @criteria
	OR telephone.user_modified LIKE @criteria
	ORDER BY numero ASC
END
GO

--type_imprimante
CREATE PROCEDURE sp_type_imprimante_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM type_imprimante  WHERE 1=1
	OR designation LIKE @criteria
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
	ORDER BY designation ASC
END
GO

--frequence
CREATE PROCEDURE sp_frequence_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM frequence  WHERE 1=1
	OR designation LIKE @criteria
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
	ORDER BY designation ASC
END
GO

--antenne
CREATE PROCEDURE sp_antenne_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM antenne  WHERE 1=1
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
	ORDER BY valeur ASC
END
GO

--type_amplificateur
CREATE PROCEDURE type_amplificateur_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM type_amplificateur  WHERE 1=1
	OR designation LIKE @criteria
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
	ORDER BY designation ASC
END
GO

--email
CREATE PROCEDURE sp_email_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM email  WHERE 1=1
	OR designation LIKE @criteria
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
	ORDER BY designation ASC
END
GO

--email_personne
CREATE PROCEDURE sp_email_personne_criteria @criteria varchar(50),@id int
AS 
BEGIN
	SELECT *  FROM email INNER JOIN personne 
	ON personne.id=email.id_personne WHERE personne.id=@id  
	OR email.designation LIKE @criteria
	OR email.user_created LIKE @criteria
	OR email.user_modified LIKE @criteria
	ORDER BY designation ASC
END
GO

--type_routeur_AP
CREATE PROCEDURE sp_type_routeur_AP_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM type_routeur_AP  WHERE 1=1
	OR designation LIKE @criteria
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
	ORDER BY designation ASC
END
GO

--adresse
CREATE PROCEDURE sp_adresse_AP_criteria @criteria varchar(300)
AS 
BEGIN
	SELECT *  FROM adresse  WHERE 1=1
	OR designation LIKE @criteria
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
	ORDER BY designation ASC
END
GO

--adresse_personne
CREATE PROCEDURE sp_adresse_personne_criteria @criteria varchar(50),@id int
AS 
BEGIN
	SELECT *  FROM adresse INNER JOIN personne 
	ON personne.id=adresse.id_personne WHERE personne.id=@id  
	OR adresse.designation LIKE @criteria
	OR adresse.user_created LIKE @criteria
	OR adresse.user_modified LIKE @criteria
	ORDER BY designation ASC
END
GO

--type_switch
CREATE PROCEDURE sp_type_switch_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM type_switch  WHERE 1=1
	OR designation LIKE @criteria
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
	ORDER BY designation ASC
END
GO

--type_clavier
CREATE PROCEDURE sp_type_clavier_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM type_clavier  WHERE 1=1
	OR designation LIKE @criteria
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
	ORDER BY designation ASC
END
GO

--etat_materiel
CREATE PROCEDURE sp_etat_materiel_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM etat_materiel  WHERE 1=1
	OR designation LIKE @criteria
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
	ORDER BY designation ASC
END
GO

--type_OS
CREATE PROCEDURE sp_type_OS_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM type_OS  WHERE 1=1
	OR designation LIKE @criteria
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
	ORDER BY designation ASC
END
GO

--architecture_OS
CREATE PROCEDURE sp_architecture_OS_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM type_OS  WHERE 1=1
	OR designation LIKE @criteria
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
	ORDER BY designation ASC
END
GO

--OS_architecture_OS
CREATE PROCEDURE sp_OS_architecture_OS_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT type_OS.designation + ' ' + OS.designation + ' ' + architecture_OS.designation AS designation,OS.*  FROM OS 
    INNER JOIN architecture_OS ON architecture_OS.id=OS.id_architecture_OS 
    INNER JOIN type_OS ON type_OS.id=OS.id_type_OS WHERE
	OS.designation LIKE @criteria
	OR OS.user_created LIKE @criteria
	OR OS.user_modified LIKE @criteria
	ORDER BY OS.designation ASC
END
GO

--version_ios
CREATE PROCEDURE sp_version_ios_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM version_ios  WHERE 1=1
	OR designation LIKE @criteria
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
	ORDER BY designation ASC
END
GO

--netette
CREATE PROCEDURE sp_netette_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM netette  WHERE 1=1
	OR designation LIKE @criteria
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
	ORDER BY designation ASC
END
GO

--garantie
CREATE PROCEDURE sp_garantie_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM garantie  WHERE 1=1
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
	ORDER BY valeur ASC
END
GO

--ram
CREATE PROCEDURE sp_ram_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM ram  WHERE 1=1
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
	ORDER BY valeur ASC
END
GO

--processeur
CREATE PROCEDURE sp_processeur_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM processeur  WHERE 1=1
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
	ORDER BY valeur ASC
END
GO

--nombre_coeur_processeur
CREATE PROCEDURE sp_nombre_coeur_processeur_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM nombre_coeur_processeur  WHERE 1=1
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
	ORDER BY valeur ASC
END
GO

--type_hdd
CREATE PROCEDURE sp_type_hdd_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM type_hdd  WHERE 1=1
	OR designation LIKE @criteria
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
	ORDER BY designation ASC
END
GO

--nombre_hdd
CREATE PROCEDURE sp_nombre_hdd_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM nombre_hdd  WHERE 1=1
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
	ORDER BY valeur ASC
END
GO

--capacite_hdd
CREATE PROCEDURE sp_capacite_hdd_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM capacite_hdd  WHERE 1=1
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
	ORDER BY valeur ASC
END
GO

--taille_ecran
CREATE PROCEDURE sp_taille_ecran_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM taille_ecran  WHERE 1=1
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
	ORDER BY valeur ASC
END
GO

--materiel
CREATE PROCEDURE sp_materiel_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM materiel  WHERE 1=1
	OR code_str LIKE @criteria
	OR label LIKE @criteria
	OR mac_adresse1 LIKE @criteria
	OR mac_adresse2 LIKE @criteria
	OR commentaire LIKE @criteria
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
	ORDER BY code_str ASC
END
GO

--grade
CREATE PROCEDURE sp_grade_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM grade  WHERE 1=1
	OR designation LIKE @criteria
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
	ORDER BY designation ASC
END
GO

--usb2
CREATE PROCEDURE sp_usb2_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM usb2  WHERE 1=1
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
	ORDER BY valeur ASC
END
GO

--personne
CREATE PROCEDURE sp_personne_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM personne  WHERE 1=1
	OR nom LIKE @criteria
	OR postnom LIKE @criteria
	OR prenom LIKE @criteria
	OR sexe LIKE @criteria
	OR etatcivil LIKE @criteria
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
	ORDER BY nom ASC
END
GO

--usb3
CREATE PROCEDURE sp_usb3_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM usb3  WHERE 1=1
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
	ORDER BY valeur ASC
END
GO

--type_lieu_affectation
CREATE PROCEDURE sp_type_lieu_affectationcriteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM type_lieu_affectation  WHERE 1=1
	OR designation LIKE @criteria
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
	ORDER BY designation ASC
END
GO

--hdmi
CREATE PROCEDURE sp_hdmi_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM hdmi  WHERE 1=1
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
	ORDER BY valeur ASC
END
GO

--AC
CREATE PROCEDURE sp_AC_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM AC  WHERE 1=1
	OR code_str LIKE @criteria
	OR designation LIKE @criteria
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
	ORDER BY code_str ASC
END
GO

--vga
CREATE PROCEDURE sp_vga_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM vga  WHERE 1=1
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
	ORDER BY valeur ASC
END
GO

--optio
CREATE PROCEDURE sp_optio_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM optio  WHERE 1=1
	OR designation LIKE @criteria
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
	ORDER BY designation ASC
END
GO

--tension_batterie
CREATE PROCEDURE sp_tension_batterie_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM tension_batterie  WHERE 1=1
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
	ORDER BY valeur ASC
END
GO

--promotion
CREATE PROCEDURE sp_promotion_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM promotion  WHERE 1=1
	OR designation LIKE @criteria
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
	ORDER BY designation ASC
END
GO

--tension_adaptateur
CREATE PROCEDURE sp_tension_adaptateur_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM tension_adaptateur  WHERE 1=1
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
	ORDER BY valeur ASC
END
GO

--section
CREATE PROCEDURE sp_section_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM section  WHERE 1=1
	OR designation1 LIKE @criteria
	OR designation2 LIKE @criteria
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
	ORDER BY designation1 ASC
END
GO

--puissance_adaptateur
CREATE PROCEDURE sp_puissance_adaptateur_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM puissance_adaptateur  WHERE 1=1
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
	ORDER BY valeur ASC
END
GO

--retrait_materiel
CREATE PROCEDURE sp_retrait_materiel_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM retrait_materiel  WHERE 1=1 
	OR code_AC LIKE @criteria
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
	ORDER BY code_AC,id ASC
END
GO

--intensite_adaptateur
CREATE PROCEDURE sp_intensite_adaptateur_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM intensite_adaptateur  WHERE 1=1
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
	ORDER BY valeur ASC
END
GO

--puissance
CREATE PROCEDURE sp_puissance_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM puissance  WHERE 1=1
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
	ORDER BY valeur ASC
END
GO

--detail_retrait_materiel
CREATE PROCEDURE sp_detail_retrait_materiel_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM detail_retrait_materiel  WHERE 1=1
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
END
GO

--intensite
CREATE PROCEDURE sp_intensite_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM intensite  WHERE 1=1
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
	ORDER BY valeur ASC
END
GO

--page_par_minute
CREATE PROCEDURE sp_page_par_minute_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM page_par_minute  WHERE 1=1
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
	ORDER BY valeur ASC
END
GO

--signataire
CREATE PROCEDURE sp_signataire_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM signataire  WHERE 1=1
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
END
GO

--tension_alimentation
CREATE PROCEDURE sp_tension_alimentation_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM tension_alimentation  WHERE 1=1
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
	ORDER BY valeur ASC
END
GO

--salle
CREATE PROCEDURE sp_salle_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM salle  WHERE 1=1
	OR designation LIKE @criteria
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
	ORDER BY designation ASC
END
GO

--usb
CREATE PROCEDURE sp_usb_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM usb  WHERE 1=1
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
	ORDER BY valeur ASC
END
GO

--fonction
CREATE PROCEDURE sp_fonction_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM fonction  WHERE 1=1
	OR designation LIKE @criteria
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
	ORDER BY designation ASC
END
GO

--memoire
CREATE PROCEDURE sp_memoire_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM memoire  WHERE 1=1
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
	ORDER BY valeur ASC
END
GO

--lieu_affectation
CREATE PROCEDURE sp_lieu_affectation_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM lieu_affectation  WHERE 1=1
	OR code_AC LIKE @criteria
	OR designation LIKE @criteria
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
	ORDER BY designation ASC
END
GO

--sorties_audio
CREATE PROCEDURE sp_sorties_audio_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM sorties_audio  WHERE 1=1
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
	ORDER BY valeur ASC
END
GO

--microphone
CREATE PROCEDURE sp_microphone_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM microphone  WHERE 1=1
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
	ORDER BY valeur ASC
END
GO

--affectation_materiel
CREATE PROCEDURE sp_affectation_materiel_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM affectation_materiel  WHERE 1=1
	OR code_AC LIKE @criteria
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
END
GO

--gain
CREATE PROCEDURE sp_gain_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM gain  WHERE 1=1
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
	ORDER BY valeur ASC
END
GO

--gbe
CREATE PROCEDURE sp_gbe_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM gbe  WHERE 1=1
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
	ORDER BY valeur ASC
END
GO

--utilisateur
CREATE PROCEDURE sp_utilisateur_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM utilisateur  WHERE 1=1
	OR nomuser LIKE @criteria
	OR motpass LIKE @criteria
	OR schema_user LIKE @criteria
	OR droits LIKE @criteria
END
GO

--categorie_materiel
CREATE PROCEDURE sp_categorie_materiel_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM categorie_materiel  WHERE 1=1
	OR designation LIKE @criteria
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
	ORDER BY designation ASC
END
GO

--fe
CREATE PROCEDURE sp_fe_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT *  FROM fe  WHERE 1=1
	OR user_created LIKE @criteria
	OR user_modified LIKE @criteria
	ORDER BY valeur ASC
END
GO

--QrCode avec critere
CREATE PROCEDURE sp_qrCode_criteria @criteria varchar(50)
AS 
BEGIN
	SELECT qrcode,code_str,label FROM materiel WHERE
	code_str=@criteria 
	ORDER BY id ASC
END
GO

--QrCode
CREATE PROCEDURE sp_qrCode
AS 
BEGIN
	SELECT qrcode,code_str,label FROM materiel ORDER BY id ASC
END
GO