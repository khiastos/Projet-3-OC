# Projet 3 : Testez l'implémentation d'une nouvelle fonctionnalité .NET

Ce projet est une **correction d'un site web développé en .NET**, dans le cadre de ma formation "Développeur d'application back-end .NET".  
Il est basé sur un projet existant mais modifié dans une branche dédiée (dev) à ma version contenant mon travail final qui était d'effectuer des tests unitaires et d'intégrations après l'ajout d'une nouvelle fonctionnalité.

---
## Les ajouts dans le code

- Ajout des data annotations pour valider les champs de validation lors de la création des produits du côté admin
- Ajout des messages d'erreurs lors de la validation de la commande
- Réalisation des tests unitaires pour tester les entrées utilisateurs via la nouvelle fonctionnalité
- Réalisation des tests d'intégrations pour tester la nouvelle fonctionnalité (ajout, suppression, mise à jour des produits dans la base de donnée)
- Correction de certains bug (absence du symbole monétaire à côté des prix, absence de la langue espagnol, suppression des articles lorsque le stock est de 0 ou moins,...)

---
## Outils et technologies utilisés

- **Visual Studio 2022**
- **C# / ASP.NET Core**
- **SQL Server Management Studio**
