# 🎮 Tournament Manager - Plateforme de Gestion de Tournois E-Sport

[![.NET](https://img.shields.io/badge/.NET-9.0-blue)](https://dotnet.microsoft.com/)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-17-blue)](https://www.postgresql.org/)
[![C#](https://img.shields.io/badge/C%23-12.0-purple)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![License](https://img.shields.io/badge/license-MIT-green)](LICENSE)

## 📋 Description

**Tournament Manager** est une plateforme complète de gestion de tournois e-sport développée en C# avec PostgreSQL. Elle permet de gérer les équipes, les joueurs, les matchs, les streams, et le système de points/classement pour des jeux comme League of Legends, Counter-Strike, Valorant, etc.

Ce projet est un **projet final intégrateur** combinant :
- 📚 **Base de Données** : Conception MCD/MLD/MPD, requêtes SQL avancées
- 💻 **Programmation C#** : Architecture en couches, ADO.NET, services métier

---

## 🎯 Fonctionnalités

### ✅ Gestion des Entités
- **Tournois** : Création, suivi, gestion du statut (Ouvert/En cours/Terminé)
- **Équipes** : Enregistrement avec nom, tag, pays, date de création
- **Joueurs** : Profils détaillés (pseudo, rôle, statut titulaire/remplaçant)
- **Matchs** : Planification par phase (Poules, Quarts, Demi-finales, Finale)
- **Inscriptions** : Gestion des équipes participantes avec validation
- **Classements** : Calcul automatique des points, victoires, défaites
- **Streams** : Suivi des diffusions (Twitch, YouTube) avec nombre de viewers

### 🔧 Fonctionnalités Techniques
- Architecture en 3 couches (Models, DataAccess, Services)
- Connexion sécurisée avec fichier de configuration
- Requêtes SQL paramétrées (protection contre les injections SQL)
- Interface console interactive

---

## 🏗️ Architecture du Projet
```
TournamentManager/
├── Models/                    # Entités métier (POCO classes)
│   ├── Tournoi.cs
│   ├── Equipe.cs
│   ├── Joueur.cs
│   ├── Match.cs
│   ├── Participant.cs
│   ├── Inscription.cs
│   ├── Classement.cs
│   ├── Stream.cs
│   └── Jeu.cs
├── DataAccess/                # Couche d'accès aux données (Repositories)
│   ├── DatabaseContext.cs
│   ├── TournoiRepository.cs
│   ├── EquipeRepository.cs
│   ├── JoueurRepository.cs
│   └── InscriptionRepository.cs
├── Services/                  # Logique métier
│   └── TournoiService.cs
├── UI/                        # Interface utilisateur console
│   └── (à venir)
├── appsettings.json          # Configuration (mot de passe BDD)
├── appsettings.example.json  # Template de configuration
└── Program.cs                # Point d'entrée de l'application
```

---

## 🗄️ Modèle de Données

### Entités Principales

**TOURNOI**
- id, nom, id_jeu, date_debut, date_fin
- nb_equipes_max, format, prize_pool, statut

**EQUIPE**
- id, nom, tag, date_creation, pays

**JOUEUR**
- id, pseudo, nom_reel, email, date_naissance
- role, est_titulaire, id_equipe

**MATCH**
- id, id_tournoi, date_heure, phase
- format (BO1/BO3/BO5), statut, map

**PARTICIPANT** (relation Match ↔ Equipe)
- id_equipe, id_match, numero_equipe
- score, est_vainqueur

**INSCRIPTION** (relation Equipe ↔ Tournoi)
- id_equipe, id_tournoi, date_inscription
- seed, est_valide

**CLASSEMENT**
- id_equipe, id_tournoi, points
- victoires, defaites, matchs_nuls, position

---

## 🚀 Installation et Configuration

### Prérequis

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL 17](https://www.postgresql.org/download/)
- [pgAdmin 4](https://www.pgadmin.org/) (optionnel, pour l'interface graphique)

### Étape 1 : Cloner le projet
```bash
git clone https://github.com/PatrickRasheed/project-esport.git
cd project-esport/TournamentManager
```

### Étape 2 : Créer la base de données

1. Ouvre **pgAdmin 4** ou **psql**
2. Crée la base de données :
```sql
CREATE DATABASE tournois_esport;
```

3. Exécute les scripts SQL de création des tables (voir `schema.sql`)

### Étape 3 : Configurer la connexion

1. Copie le fichier de configuration exemple :
```bash
cp appsettings.example.json appsettings.json
```

2. Édite `appsettings.json` et remplace le mot de passe :
```json
{
  "ConnectionStrings": {
    "PostgreSQL": "Host=localhost;Port=5432;Database=tournois_esport;Username=postgres;Password=TON_MOT_DE_PASSE"
  }
}
```

### Étape 4 : Installer les dépendances
```bash
dotnet restore
```

### Étape 5 : Compiler et exécuter
```bash
dotnet build
dotnet run
```

Si tout fonctionne, tu devrais voir :
```
=== Tournament Manager ===
✅ Connexion réussie à la base de données !
```

---

## 📦 Dépendances NuGet

- `Npgsql` (PostgreSQL driver pour .NET)
- `Microsoft.Extensions.Configuration`
- `Microsoft.Extensions.Configuration.Json`

Installation :
```bash
dotnet add package Npgsql
dotnet add package Microsoft.Extensions.Configuration
dotnet add package Microsoft.Extensions.Configuration.Json
```

---

## 🧪 Tests (à venir)

- Tests unitaires des repositories
- Tests d'intégration des services
- Analyse de couverture de code

---

## 📊 Roadmap

### Phase 1 - Fondations ✅
- [x] Conception MCD/MLD/MPD
- [x] Création des tables PostgreSQL
- [x] Classes Models C#
- [x] Repositories (DataAccess)
- [x] Services métier de base
- [x] Configuration sécurisée

### Phase 2 - En cours 🔄
- [ ] Interface console interactive complète
- [ ] CRUD complet pour toutes les entités
- [ ] Gestion des matchs et des phases
- [ ] Calcul automatique des classements

### Phase 3 - À venir 📅
- [ ] Tests unitaires et d'intégration
- [ ] Requêtes SQL avancées et statistiques
- [ ] Export de données (CSV, PDF)
- [ ] Interface graphique (WPF/Avalonia)

---

## 👥 Contribution

Projet académique développé par **Fréjus Adedemi** et **Patrick** dans le cadre d'un projet intégrateur BDD + C#.

Développeur 1 : [FrejusAdedemi](https://github.com/FrejusAdedemi)
Développeur 2 : [PatrickRasheed](https://github.com/PatrickRasheed)


---

## 🙏 Remerciements

- Professeur de Base de Données
- Professeur de Programmation C#
- PostgreSQL Community
- Microsoft .NET Team

---

**Dernière mise à jour** : Janvier 2025