using BepInEx;
using On.EntityStates.Croco;

namespace AcridYump;


[BepInPlugin(PluginGUID, PluginName, PluginVersion)]
public class AcridYump : BaseUnityPlugin {
    public const string PluginGUID = PluginAuthor + "." + PluginName;
    public const string PluginAuthor = "Kriper1111";
    public const string PluginName = "AcridYump";
    public const string PluginVersion = "1.0.0";
    
    public void Awake() {
        On.EntityStates.Croco.BaseLeap.OnEnter += BaseLeapOnOnEnter;
        On.EntityStates.Croco.BaseLeap.OnExit += BaseLeapOnOnExit;
    }

    private void BaseLeapOnOnEnter(BaseLeap.orig_OnEnter orig, EntityStates.Croco.BaseLeap self) {
        orig(self);
        
        var acridJumpController = self.gameObject.GetComponent<AcridJumpEventController>();
        if (acridJumpController == null) {
            acridJumpController = self.gameObject.AddComponent<AcridJumpEventController>();
            acridJumpController.enabled = false;
        }

        acridJumpController.leapAbility = self;
        acridJumpController.enabled = true;
    }

    private void BaseLeapOnOnExit(BaseLeap.orig_OnExit orig, EntityStates.Croco.BaseLeap self) {
        var acridJumpController = self.gameObject.GetComponent<AcridJumpEventController>();
        acridJumpController.enabled = false;

        orig(self);
    }
    
    public void OnDestroy() {
        On.EntityStates.Croco.BaseLeap.OnEnter -= BaseLeapOnOnEnter;
        On.EntityStates.Croco.BaseLeap.OnExit -= BaseLeapOnOnExit;
    }
}