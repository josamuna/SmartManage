select affectation_materiel.id as 'N° Aff.',affectation_materiel.date_affectation as 'Date',salle.designation as 'Salle',AC.code_str as 'Année Académique',lieu_affectation.designation as 'Lieu Affectat°',type_lieu_affectation.designation as 'Type lieu Affectat°',
ISNULL(personne.nom,'') + ' ' + ISNULL(personne.postnom,'') + ' ' + ISNULL(personne.prenom,'') as 'Nom',fonction.designation as 'Fonction',categorie_materiel.designation as 'Cat. Mat.',materiel.code_str as 'Code Mat.',materiel.label as 'Label Mat.',materiel.qrcode as 'QrCode' 
from affectation_materiel
inner join materiel on materiel.id=affectation_materiel.id_materiel
inner join categorie_materiel on categorie_materiel.id=materiel.id_categorie_materiel
inner join AC on AC.code_str=affectation_materiel.code_AC
inner join salle on salle.id=affectation_materiel.id_salle
inner join lieu_affectation on lieu_affectation.id=affectation_materiel.id_lieu_affectation
inner join type_lieu_affectation on type_lieu_affectation.id=lieu_affectation.id_type_lieu_affectation
left outer join personne on personne.id=lieu_affectation.id_personne
left outer join fonction on fonction.id=lieu_affectation.id_fonction

where affectation_materiel.code_AC=(select code_str from current_AC) and materiel.archiver=0

UNION

select grade.designation + ' ' + ISNULL(personne.nom,'') + ' ' + ISNULL(personne.postnom,'') + ' ' + ISNULL(personne.prenom,'') as 'Signataire', signataire.signature_specimen as 'Signature' 
from signataire 
inner join AC on AC.code_str=signataire.code_AC
inner join personne on personne.id=signataire.id_personne 
inner join grade on grade.id=personne.id_grade
where signataire.code_AC=(select code_str from current_AC)



select * from personne
select * from grade