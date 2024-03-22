using System.Reflection;
using HarmonyLib;
using Verse;

namespace MultiBedRoyaltyPatch;

[StaticConstructorOnStartup]
internal static class MultiBedRoyaltyPatch
{
    static MultiBedRoyaltyPatch()
    {
        new Harmony("Multi-Bed Royalty Patch").PatchAll(Assembly.GetExecutingAssembly());
    }
}