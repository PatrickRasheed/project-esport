-- ================================================
-- REQUÊTES SQL AVANCÉES - TOURNAMENT MANAGER
-- ================================================
-- Ce fichier contient des requêtes SQL avancées démontrant
-- la maîtrise des JOINs, agrégations, GROUP BY, sous-requêtes, etc.
-- ================================================

-- ================================================
-- 1. STATISTIQUES GLOBALES DU SYSTÈME
-- ================================================

-- Résumé complet de la plateforme
SELECT 
    'Nombre de jeux' as statistique,
    COUNT(*) as valeur
FROM jeux
UNION ALL
SELECT 'Nombre d''équipes', COUNT(*) FROM equipe
UNION ALL
SELECT 'Nombre de joueurs', COUNT(*) FROM joueur
UNION ALL
SELECT 'Nombre de tournois', COUNT(*) FROM tournoi
UNION ALL
SELECT 'Nombre de matchs', COUNT(*) FROM match_
UNION ALL
SELECT 'Prize pool total (€)', SUM(prize_pool)::integer FROM tournoi;


-- ================================================
-- 2. CLASSEMENT COMPLET AVEC NOMS D'ÉQUIPES
-- ================================================

-- Afficher le classement avec tous les détails des équipes
SELECT 
    c.position_actuelle as "Position",
    e.nom_equipe as "Équipe",
    e.tag_equipe as "Tag",
    e.pays as "Pays",
    t.nom_tournoi as "Tournoi",
    c.points as "Points",
    c.victoires as "V",
    c.defaites as "D",
    c.matchs_nuls as "N",
    ROUND(c.victoires::numeric / NULLIF(c.victoires + c.defaites + c.matchs_nuls, 0) * 100, 2) as "Win Rate %"
FROM classement c
JOIN equipe e ON c.id_equipe = e.id_equipe
JOIN tournoi t ON c.id_tournoi = t.id_tournoi
ORDER BY t.nom_tournoi, c.position_actuelle;


-- ================================================
-- 3. STATISTIQUES PAR JEU
-- ================================================

-- Prize pool total et nombre de tournois par jeu
SELECT 
    j.nom_jeu as "Jeu",
    j.genre as "Genre",
    COUNT(t.id_tournoi) as "Nb Tournois",
    COALESCE(SUM(t.prize_pool), 0) as "Prize Pool Total (€)",
    COALESCE(AVG(t.prize_pool), 0)::integer as "Prize Pool Moyen (€)",
    COUNT(DISTINCT i.id_equipe) as "Équipes Participantes"
FROM jeux j
LEFT JOIN tournoi t ON j.id_jeux = t.id_jeux
LEFT JOIN inscription i ON t.id_tournoi = i.id_tournoi
GROUP BY j.id_jeux, j.nom_jeu, j.genre
ORDER BY SUM(t.prize_pool) DESC NULLS LAST;


-- ================================================
-- 4. ÉQUIPES AVEC EFFECTIF COMPLET
-- ================================================

-- Afficher toutes les équipes avec le nombre de joueurs
SELECT 
    e.id_equipe,
    e.nom_equipe as "Équipe",
    e.tag_equipe as "Tag",
    e.pays as "Pays",
    COUNT(j.id_joueur) as "Nb Joueurs",
    COUNT(CASE WHEN j.est_titulaire = true THEN 1 END) as "Titulaires",
    COUNT(CASE WHEN j.est_titulaire = false THEN 1 END) as "Remplaçants",
    CASE 
        WHEN COUNT(j.id_joueur) = 0 THEN '❌ Équipe vide'
        WHEN COUNT(j.id_joueur) < 5 THEN '⚠️ Effectif incomplet'
        WHEN COUNT(j.id_joueur) = 5 THEN '✅ Effectif complet'
        ELSE '✅ Effectif complet + remplaçants'
    END as "Statut"
FROM equipe e
LEFT JOIN joueur j ON e.id_equipe = j.id_equipe
GROUP BY e.id_equipe, e.nom_equipe, e.tag_equipe, e.pays
ORDER BY COUNT(j.id_joueur) DESC, e.nom_equipe;


-- ================================================
-- 5. TOP 10 JOUEURS LES PLUS JEUNES
-- ================================================

-- Classement des joueurs par âge
SELECT 
    j.pseudo as "Pseudo",
    j.prenom || ' ' || j.nom_reel as "Nom Complet",
    e.nom_equipe as "Équipe",
    j.role_jeu as "Rôle",
    j.date_naissance as "Date Naissance",
    EXTRACT(YEAR FROM AGE(j.date_naissance)) as "Âge",
    CASE WHEN j.est_titulaire THEN '⭐ Titulaire' ELSE '🔄 Remplaçant' END as "Statut"
FROM joueur j
JOIN equipe e ON j.id_equipe = e.id_equipe
ORDER BY j.date_naissance DESC
LIMIT 10;


-- ================================================
-- 6. TOURNOIS AVEC INSCRIPTIONS ET MATCHS
-- ================================================

-- Vue détaillée de chaque tournoi
SELECT 
    t.nom_tournoi as "Tournoi",
    j.nom_jeu as "Jeu",
    t.statut as "Statut",
    t.date_debut as "Date Début",
    t.date_fin as "Date Fin",
    t.prize_pool as "Prize Pool (€)",
    COUNT(DISTINCT i.id_equipe) as "Équipes Inscrites",
    t.nombre_equipes_max as "Places Disponibles",
    COUNT(DISTINCT m.id_match) as "Matchs",
    COUNT(CASE WHEN m.statut = 'Terminé' THEN 1 END) as "Matchs Terminés",
    COUNT(CASE WHEN m.statut = 'En cours' THEN 1 END) as "Matchs En Cours",
    COUNT(CASE WHEN m.statut = 'Planifié' THEN 1 END) as "Matchs Planifiés"
FROM tournoi t
JOIN jeux j ON t.id_jeux = j.id_jeux
LEFT JOIN inscription i ON t.id_tournoi = i.id_tournoi AND i.est_valide = true
LEFT JOIN match_ m ON t.id_tournoi = m.id_tournoi
GROUP BY t.id_tournoi, t.nom_tournoi, j.nom_jeu, t.statut, t.date_debut, t.date_fin, t.prize_pool, t.nombre_equipes_max
ORDER BY t.date_debut DESC;


-- ================================================
-- 7. RÉSULTATS DES MATCHS AVEC ÉQUIPES
-- ================================================

-- Historique complet des matchs avec scores
SELECT 
    m.id_match as "ID",
    t.nom_tournoi as "Tournoi",
    m.phase_tournoi as "Phase",
    m.date_match as "Date",
    m.format_match as "Format",
    e1.nom_equipe as "Équipe 1",
    p1.score as "Score 1",
    e2.nom_equipe as "Équipe 2",
    p2.score as "Score 2",
    CASE 
        WHEN p1.est_vainqueur THEN e1.nom_equipe || ' 🏆'
        WHEN p2.est_vainqueur THEN e2.nom_equipe || ' 🏆'
        ELSE 'Match nul'
    END as "Vainqueur",
    m.statut as "Statut"
FROM match_ m
JOIN tournoi t ON m.id_tournoi = t.id_tournoi
LEFT JOIN participant p1 ON m.id_match = p1.id_match AND p1.numero_equipe = 1
LEFT JOIN participant p2 ON m.id_match = p2.id_match AND p2.numero_equipe = 2
LEFT JOIN equipe e1 ON p1.id_equipe = e1.id_equipe
LEFT JOIN equipe e2 ON p2.id_equipe = e2.id_equipe
ORDER BY m.date_match DESC;


-- ================================================
-- 8. ÉQUIPES SANS INSCRIPTION À UN TOURNOI
-- ================================================

-- Trouver les équipes qui ne sont inscrites à aucun tournoi
SELECT 
    e.id_equipe,
    e.nom_equipe as "Équipe",
    e.tag_equipe as "Tag",
    e.pays as "Pays",
    COUNT(j.id_joueur) as "Nb Joueurs",
    '❌ Aucune inscription' as "Statut"
FROM equipe e
LEFT JOIN inscription i ON e.id_equipe = i.id_equipe
LEFT JOIN joueur j ON e.id_equipe = j.id_equipe
WHERE i.id_inscription IS NULL
GROUP BY e.id_equipe, e.nom_equipe, e.tag_equipe, e.pays
ORDER BY COUNT(j.id_joueur) DESC;


-- ================================================
-- 9. MATCHS À VENIR DANS LES 30 PROCHAINS JOURS
-- ================================================

-- Calendrier des prochains matchs
SELECT 
    m.id_match as "ID",
    t.nom_tournoi as "Tournoi",
    j.nom_jeu as "Jeu",
    m.phase_tournoi as "Phase",
    m.date_match as "Date & Heure",
    m.format_match as "Format",
    EXTRACT(DAY FROM (m.date_match - CURRENT_TIMESTAMP)) as "Jours Restants",
    m.statut as "Statut"
FROM match_ m
JOIN tournoi t ON m.id_tournoi = t.id_tournoi
JOIN jeux j ON t.id_jeux = j.id_jeux
WHERE m.date_match BETWEEN CURRENT_TIMESTAMP AND CURRENT_TIMESTAMP + INTERVAL '30 days'
    AND m.statut IN ('Planifié', 'En cours')
ORDER BY m.date_match ASC;


-- ================================================
-- 10. ANALYSE DE POPULARITÉ DES STREAMS
-- ================================================

-- Statistiques sur les diffusions de matchs
SELECT 
    t.nom_tournoi as "Tournoi",
    COUNT(DISTINCT s.id_stream) as "Nb Streams",
    COUNT(DISTINCT s.plateform) as "Nb Plateformes",
    SUM(s.nb_viewers_pic) as "Total Viewers Peak",
    AVG(s.nb_viewers_pic)::integer as "Moyenne Viewers",
    MAX(s.nb_viewers_pic) as "Max Viewers",
    STRING_AGG(DISTINCT s.plateform, ', ') as "Plateformes Utilisées"
FROM stream s
JOIN match_ m ON s.id_match = m.id_match
JOIN tournoi t ON m.id_tournoi = t.id_tournoi
GROUP BY t.id_tournoi, t.nom_tournoi
ORDER BY SUM(s.nb_viewers_pic) DESC;


-- ================================================
-- VUES UTILES POUR L'APPLICATION
-- ================================================

-- Vue : Classement complet enrichi
CREATE OR REPLACE VIEW v_classement_complet AS
SELECT 
    c.id_classement,
    c.position_actuelle,
    e.nom_equipe,
    e.tag_equipe,
    e.pays,
    t.nom_tournoi,
    c.points,
    c.victoires,
    c.defaites,
    c.matchs_nuls,
    c.victoires + c.defaites + c.matchs_nuls as matchs_joues,
    CASE 
        WHEN c.victoires + c.defaites + c.matchs_nuls > 0 
        THEN ROUND(c.victoires::numeric / (c.victoires + c.defaites + c.matchs_nuls) * 100, 2)
        ELSE 0 
    END as win_rate
FROM classement c
JOIN equipe e ON c.id_equipe = e.id_equipe
JOIN tournoi t ON c.id_tournoi = t.id_tournoi;

-- Utilisation : SELECT * FROM v_classement_complet ORDER BY position_actuelle;


-- Vue : Effectifs des équipes
CREATE OR REPLACE VIEW v_effectifs_equipes AS
SELECT 
    e.id_equipe,
    e.nom_equipe,
    e.tag_equipe,
    e.pays,
    COUNT(j.id_joueur) as nb_joueurs_total,
    COUNT(CASE WHEN j.est_titulaire THEN 1 END) as nb_titulaires,
    COUNT(CASE WHEN NOT j.est_titulaire THEN 1 END) as nb_remplacants
FROM equipe e
LEFT JOIN joueur j ON e.id_equipe = j.id_equipe
GROUP BY e.id_equipe, e.nom_equipe, e.tag_equipe, e.pays;

-- Utilisation : SELECT * FROM v_effectifs_equipes WHERE nb_joueurs_total < 5;


-- ================================================
-- FIN DES REQUÊTES AVANCÉES
-- ================================================