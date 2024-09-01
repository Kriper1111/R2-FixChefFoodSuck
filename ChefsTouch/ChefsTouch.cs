using BepInEx;
using R2API;
using UnityEngine;

namespace ChefsTouch; 
// This is borrowed from the example plugin boilerplate
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
    public const string PluginVersion = "1.0.1";

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