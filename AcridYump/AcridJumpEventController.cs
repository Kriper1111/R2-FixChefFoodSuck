using EntityStates.Croco;
using RoR2;
using UnityEngine;

namespace AcridYump; 

public class AcridJumpEventController : MonoBehaviour {
    public BaseLeap leapAbility;
    private bool _hasAttachedCallback = false;
    
    private void OnEnable() {
        if (leapAbility == null) {
            Debug.LogWarning($"AcridYump: Enabling on {this.gameObject} - leapAbility was not set??");
            return;
        }

        if (leapAbility.isAuthority) {
            leapAbility.characterMotor.onMovementHit += OnMovementHit;
            _hasAttachedCallback = true;
        }
    }

    private void OnMovementHit(ref CharacterMotor.MovementHitInfo hitInfo) {
        leapAbility.detonateNextFrame = true;
    }

    private void OnDisable() {
        if (leapAbility == null || !_hasAttachedCallback) {
            return; 
        }

        if (leapAbility.isAuthority) {
            leapAbility.characterMotor.onMovementHit -= OnMovementHit;
            _hasAttachedCallback = false;
        }
    }
}