using System.Reflection;
using HarmonyLib;
using Verse;

namespace MultiBedRoyaltyPatch
{
    [StaticConstructorOnStartup]
    internal static class MultiBedRoyaltyPatch
    {
        static MultiBedRoyaltyPatch()
        {
            Log.Message("Multi-Bed Royalty Patch started.");
            new Harmony("Multi-Bed Royalty Patch").PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}