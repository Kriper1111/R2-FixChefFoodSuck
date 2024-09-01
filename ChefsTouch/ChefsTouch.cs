using BepInEx;
using ChefsTouch;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;
using GravitatePickup = On.RoR2.GravitatePickup;

namespace ExamplePlugin; 
// This is an example plugin that can be put in
// BepInEx/plugins/ChefsTouch/ChefsTouch.dll to test out.
// It's a small plugin that adds a relatively simple item to the game,
// and gives you that item whenever you press F2.

// This attribute specifies that we have a dependency on a given BepInEx Plugin,
// We need the R2API ItemAPI dependency because we are using for adding our item to the game.
// You don't need this if you're not using R2API in your plugin,
// it's just to tell BepInEx to initialize R2API before this plugin so it's safe to use R2API.
[BepInDependency(ItemAPI.PluginGUID)]

// This one is because we use a .language file for language tokens
// More info in https://risk-of-thunder.github.io/R2Wiki/Mod-Creation/Assets/Localization/
[BepInDependency(LanguageAPI.PluginGUID)]

// This attribute is required, and lists metadata for your plugin.
[BepInPlugin(PluginGUID, PluginName, PluginVersion)]

// This is the main declaration of our plugin class.
// BepInEx searches for all classes inheriting from BaseUnityPlugin to initialize on startup.
// BaseUnityPlugin itself inherits from MonoBehaviour,
// so you can use this as a reference for what you can declare and use in your plugin class
// More information in the Unity Docs: https://docs.unity3d.com/ScriptReference/MonoBehaviour.html
public class ChefsTouch : BaseUnityPlugin {
    // The Plugin GUID should be a unique ID for this plugin,
    // which is human readable (as it is used in places like the config).
    // If we see this PluginGUID as it is on thunderstore,
    // we will deprecate this mod.
    // Change the PluginAuthor and the PluginName !
    public const string PluginGUID = PluginAuthor + "." + PluginName;
    public const string PluginAuthor = "Kriper1111";
    public const string PluginName = "ChefsTouch";
    public const string PluginVersion = "0.0.1";

    // The Awake() method is run at the very start when the game is initialized.
    public void Awake() {
        On.ChefFoodPickController.Start += ChefFoodPickControllerOnStart;
    }

    private static void ChefFoodPickControllerOnStart(On.ChefFoodPickController.orig_Start orig, ChefFoodPickController self) {
        orig(self);
        var gravity = self.transform.Find("GravitationController");
        gravity.gameObject.SetActive(true);
        gravity.GetComponent<MonoBehaviour>().enabled = true;
        var pickupRigidBody = self.transform.GetComponent<Rigidbody>();
        pickupRigidBody.constraints = RigidbodyConstraints.None;
        pickupRigidBody.WakeUp();
    }

    private void OnDestroy() {
        On.ChefFoodPickController.Start -= ChefFoodPickControllerOnStart;
    }
}