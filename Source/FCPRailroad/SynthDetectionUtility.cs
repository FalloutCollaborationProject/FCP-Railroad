namespace FCP.Railroad;

// Looks up the Institute's hidden synth-replacement hediff by defName rather than
// referencing FCP-Institute's assembly, so this stays a soft, optional integration:
// the liberation incident simply never has anything to find if Institute isn't loaded.
public static class SynthDetectionUtility
{
    private const string HiddenSynthHediffDefName = "FCP_Hediff_SynthComponents_GenIII";

    private static HediffDef cachedHediff;
    private static bool lookedUp;

    private static HediffDef HiddenSynthHediff
    {
        get
        {
            if (!lookedUp)
            {
                cachedHediff = DefDatabase<HediffDef>.GetNamedSilentFail(HiddenSynthHediffDefName);
                lookedUp = true;
            }
            return cachedHediff;
        }
    }

    public static bool IsHiddenSynth(Pawn pawn)
    {
        HediffDef hediff = HiddenSynthHediff;
        return hediff != null && pawn?.health?.hediffSet != null && pawn.health.hediffSet.HasHediff(hediff);
    }

    public static IEnumerable<Pawn> GetHiddenSynthColonists(Map map)
    {
        if (HiddenSynthHediff == null || map == null)
            yield break;

        foreach (Pawn pawn in map.mapPawns.FreeColonists)
        {
            if (IsHiddenSynth(pawn))
                yield return pawn;
        }
    }
}
