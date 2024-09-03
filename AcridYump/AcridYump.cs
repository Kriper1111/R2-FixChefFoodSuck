using System.Collections.Generic;
using BepInEx;
using On.EntityStates.Croco;

namespace AcridYump;


[BepInPlugin(PluginGUID, PluginName, PluginVersion)]
public class AcridYump : BaseUnityPlugin {
    public const string PluginGUID = PluginAuthor + "." + PluginName;
    public const string PluginAuthor = "Kriper1111";
    public const string PluginName = "AcridYump";
    public const string PluginVersion = "1.0.0";
    
    private readonly Dictionary<int, BaseLeapMixin> _mixins = new();

    public void Awake() {
        On.EntityStates.Croco.BaseLeap.OnEnter += BaseLeapOnOnEnter;
        On.EntityStates.Croco.BaseLeap.OnExit += BaseLeapOnOnExit;
    }

    private void BaseLeapOnOnEnter(BaseLeap.orig_OnEnter orig, EntityStates.Croco.BaseLeap self) {
        orig(self);
        
        var instanceID = self.gameObject.GetInstanceID();
        if (self.isAuthority && !_mixins.ContainsKey(instanceID)) {
            var mixin = new BaseLeapMixin(self);
            self.characterMotor.onMovementHit += mixin.OnMovementHit;
            _mixins[instanceID] = mixin;
        }
    }

    private void BaseLeapOnOnExit(BaseLeap.orig_OnExit orig, EntityStates.Croco.BaseLeap self) {
        var instanceID = self.gameObject.GetInstanceID();
        if (self.isAuthority && _mixins.ContainsKey(instanceID)) {
            var mixin = _mixins[instanceID];
            self.characterMotor.onMovementHit -= mixin.OnMovementHit;
            _mixins.Remove(instanceID);
        }

        orig(self);
    }
    
    public void OnDestroy() {
        On.EntityStates.Croco.BaseLeap.OnEnter -= BaseLeapOnOnEnter;
        On.EntityStates.Croco.BaseLeap.OnExit -= BaseLeapOnOnExit;
    }
}