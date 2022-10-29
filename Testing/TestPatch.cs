// Path aiming for is "~/BloodParticle.cs"
using HarmonyLib;
//using BloodParticle;

public class MyPatcher {
    // make sure DoPatching() is called at start either by
    // the mod loader or by your injector

    public static void DoPatching() {
        var harmony = new Harmony("com.example.patch");
        harmony.PatchAll();
    }
}

[HarmonyPatch(typeof(TestingClass))]
[HarmonyPatch("DoSomething")] // if possible use nameof() here
class Patch01 {
    static AccessTools.FieldRef<TestingClass, string> messageRef = 
        AccessTools.FieldRefAccess<TestingClass, string>("message");

    static void Prefix(TestingClass __instance){
        messageRef(__instance) = "GOODBYE!";
    }

    static void Postfix(){
        
    }
}