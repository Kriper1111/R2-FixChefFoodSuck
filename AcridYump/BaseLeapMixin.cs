using EntityStates.Croco;
using RoR2;
using UnityEngine;

namespace AcridYump; 

public class BaseLeapMixin {
    private readonly BaseLeap _self;
    private bool _isDangerous;

    public BaseLeapMixin(BaseLeap other) {
        _self = other;
        _isDangerous = false;
    }
    
    public void OnMovementHit(ref CharacterMotor.MovementHitInfo info) {
        if (_self.isAuthority && !_isDangerous) {
            // But yea, we just.. Don't explode or clear fall immunity flag
            // if we can guess that this collision _may_ deal damage.
            _isDangerous |= IsFallDamageDangerous(_self.characterBody, info);
            // This makes it not as snappy in multiplayer context
            // But even then, we do risk triggering OnGroundHitServer on the dangerous hit,
            // and get the danger flag cleared the next few frames.
            // I'd put reliability over responsiveness, and this only can happen in guest multiplayer.
            if (_isDangerous) return;
            _self.detonateNextFrame = true;
        }
    }
    
    private static bool IsFallDamageDangerous(CharacterBody body, CharacterMotor.MovementHitInfo info) {
        // Partially gutted from GlobalEventManager.OnHitGroundServer
        // and loosely interpreted from KinematicCharacterMotor.ProbeGround
        // Yes it's _That_ bad
        if (!(info.velocity.y < 0)) return false;
        var velocity = Mathf.Abs(info.velocity.y);
        var canDealDamage = Mathf.Max(velocity - (body.jumpPower + 20), 0.0f) > 0.0f;

        if (!canDealDamage) return false;
        
        var motor = body.characterMotor.Motor;
        var maxMovement = info.velocity.magnitude;
        
        if (info.hitCollider.Raycast(new Ray(body.footPosition, Vector3.down), out var raycastHit, maxMovement)) {
            return (Vector3.Angle(motor._characterUp, raycastHit.normal) <= motor.MaxStableSlopeAngle);
        }

        // we assume it's still a dangerous hit, even if there isn't anything in that direction
        return true;
    }
}