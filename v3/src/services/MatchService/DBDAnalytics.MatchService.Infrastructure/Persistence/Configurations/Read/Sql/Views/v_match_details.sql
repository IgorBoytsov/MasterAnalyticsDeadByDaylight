CREATE OR REPLACE VIEW public.v_match_details AS

WITH "SurvivorFullData" AS (
    SELECT
        sp."MatchId",
        jsonb_build_object(
            'SurvivorId', sp."SurvivorId",
            'PlayerAssociationId', sp."PlayerAssociationId",
            'PlatformId', sp."PlatformId",
            'Experience', sp."Experience",
            'NumberBloodDrops', sp."NumberBloodDrops",
            'IsAnonymousMode', sp."IsAnonymousMode",
            'Score', sp."Score",
            'Prestige', sp."Prestige",
            'TypeDeathId', sp."TypeDeathId",
            'IsBot', sp."IsBot",
            'PerkIds', COALESCE((SELECT array_agg(p."PerkId") FROM public."SurvivorPerks" p WHERE p."SurvivorPerformanceId" = sp."Id"), '{}'::uuid[]),
            'Item', (
                SELECT jsonb_build_object(
                    'ItemId', si."ItemId",
                    'AddonIds', COALESCE((SELECT array_agg(sia."AddonId") FROM public."SurvivorItemAddons" sia WHERE sia."SurvivorItemId" = si."Id"), '{}'::uuid[])
                )
                FROM public."SurvivorItems" si
                WHERE si."SurvivorPerformanceId" = sp."Id"
            )
        ) AS "SurvivorJson"
    FROM
        public."SurvivorPerformances" sp
),

"AggregatedSurvivors" AS (
    SELECT
        sfd."MatchId",
        jsonb_agg(sfd."SurvivorJson") AS "Survivors"
    FROM
        "SurvivorFullData" sfd
    GROUP BY
        sfd."MatchId"
)

SELECT
    m."Id" AS "MatchId",
    m."StartedAt",
    m."Duration",
    m."MapId",
    m."GameModeId",
    m."GameEventId",
    m."PatchId",

    jsonb_build_object(
        'KillerId', kp."KillerId",
        'PlayerAssociationId', kp."PlayerAssociationId",
        'PlatformId', kp."PlatformId",
        'Experience', kp."Experience",
        'NumberBloodDrops', kp."NumberBloodDrops",
        'IsAnonymousMode', kp."IsAnonymousMode",
        'Score', kp."Score",
        'Prestige', kp."Prestige",
        'IsBot', kp."IsBot",
        'PerkIds', COALESCE((SELECT array_agg(p."PerkId") FROM public."KillerPerks" p WHERE p."KillerPerformanceId" = kp."Id"), '{}'::uuid[]),
        'AddonIds', COALESCE((SELECT array_agg(a."AddonId") FROM public."KillerAddons" a WHERE a."KillerPerformanceId" = kp."Id"), '{}'::uuid[])
    ) AS "KillerDetails",

    COALESCE(ags."Survivors", '[]'::jsonb) AS "SurvivorDetails"
FROM
    public."Matches" m
LEFT JOIN
    public."KillerPerformances" kp ON m."Id" = kp."MatchId"
LEFT JOIN
    "AggregatedSurvivors" ags ON m."Id" = ags."MatchId";