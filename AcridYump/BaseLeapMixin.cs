using EntityStates.Croco;
using RoR2;

namespace AcridYump; 

public class BaseLeapMixin {
    private readonly BaseLeap _self;

    public BaseLeapMixin(BaseLeap other) {
        _self = other;
    }
    
    public void OnMovementHit(ref CharacterMotor.MovementHitInfo info) {
        if (_self.isAuthority) {
            _self.detonateNextFrame = true;
        }
    }
}