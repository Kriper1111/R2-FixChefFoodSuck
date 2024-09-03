using EntityStates.Croco;
using RoR2;
using UnityEngine;

namespace AcridYump; 

public class BaseLeapMixin {
    private readonly BaseLeap _self;
    public bool DetonateNextNextFrame;

    public BaseLeapMixin(BaseLeap other) {
        _self = other;
    }
    
    public void OnMovementHit(ref CharacterMotor.MovementHitInfo info) {
        if (_self.isAuthority) {
            var isDangerous = IsFallDamageDangerous(_self.characterBody, info);
            if (isDangerous) return;
            _self.detonateNextFrame = true;
        }
    }
    
    private static bool IsFallDamageDangerous(CharacterBody body, CharacterMotor.MovementHitInfo info) {
        if (!(info.velocity.y < 0)) return false;
        var velocity = Mathf.Abs(info.velocity.y);
        return Mathf.Max(velocity - (body.jumpPower + 20), 0.0f) > 0.0f;
    }
}