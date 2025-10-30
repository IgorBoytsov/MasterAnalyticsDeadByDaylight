CREATE OR REPLACE VIEW public.v_match_list_items AS

WITH "AllPerformances" AS (

    SELECT 
        'Killer'::text AS "Role",
        "MatchId",
        "PlayerAssociationId",
        "KillerId" AS "PlayerCharacterId", 
        NULL::int4 AS "TypeDeathId"
    FROM
        public."KillerPerformances"

    UNION ALL

    SELECT 
        'Survivor'::text AS "Role",
        "MatchId",
        "PlayerAssociationId",
        "SurvivorId" AS "PlayerCharacterId", 
        "TypeDeathId"
    FROM
        public."SurvivorPerformances"
)

SELECT
    m."Id" AS "MatchId",
    m."UserId",
    p."PlayerAssociationId",
    p."PlayerCharacterId", 
    p."Role" AS "PlayerRole",
    p."TypeDeathId",
    m."StartedAt",
    m."Duration",
    m."MapId",
    m."CountKills",
    m."CountHooks",
    m."CountRecentGenerators"
FROM
    public."Matches" m
JOIN
     "AllPerformances" p ON m."Id" = p."MatchId";