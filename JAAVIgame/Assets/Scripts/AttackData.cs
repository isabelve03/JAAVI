using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackData : MonoBehaviour
{
    //jab
    public Vector3 jabHitbox; //sets the position of the hitbox relative to the player character
    public float jabRange; //sets range of attack
    public int jabDam; //damage the attack will do
    public Vector2 jabBaseK; //knockback attack will do independent of damage with angle 
    public float jabScaleK; //extra knockback attack will do based off of current damage
    public int jabFrameStartup; //how long it takes for the attack to start after you input it in frames
    public int jabFrameDuration; //how long the hitbox remains active for in frames
    public int jabFrameLag; //how long it takes until you can input an action after the attack ends in frames

    //forward light
    public Vector3 fLightHitbox;
    public float fLightRange;
    public int fLightDam;
    public Vector2 fLightBaseK;
    public float fLightScaleK;
    public int fLightFrameStartup;
    public int fLightFrameDuration;
    public int fLightFrameLag;
   
    //down light
    public Vector3 dLightHitbox;
    public float dLightRange;
    public int dLightDam;
    public Vector2 dLightBaseK;
    public float dLightScaleK;
    public int dLightFrameStartup;
    public int dLightFrameDuration;
    public int dLightFrameLag;
   
    //up light
    public Vector3 uLightHitbox;
    public float uLightRange;
    public int uLightDam;
    public Vector2 uLightBaseK;
    public float uLightScaleK;
    public int uLightFrameStartup;
    public int uLightFrameDuration;
    public int uLightFrameLag;
   
    //forward strong
    public Vector3 fStrongHitbox;
    public float fStrongRange;
    public int fStrongDam;
    public Vector2 fStrongBaseK;
    public float fStrongScaleK;
    public int fStrongFrameStartup;
    public int fStrongFrameDuration;
    public int fStrongFrameLag;
   
    //down strong
    public Vector3 dStrongHitbox;
    public float dStrongRange;
    public int dStrongDam;
    public Vector2 dStrongBaseK;
    public float dStrongScaleK;
    public int dStrongFrameStartup;
    public int dStrongFrameDuration;
    public int dStrongFrameLag;
   
    //up strong
    public Vector3 uStrongHitbox;
    public float uStrongRange;
    public int uStrongDam;
    public Vector2 uStrongBaseK;
    public float uStrongScaleK;
    public int uStrongFrameStartup;
    public int uStrongFrameDuration;
    public int uStrongFrameLag;
   
    //dash attack
    //not sure if we need this one
    public Vector3 dashAttackHitbox;
    public float dashAttackRange;
    public int dashAttackDam;
    public Vector2 dashAttackBaseK;
    public float dashAttackScaleK;
    public int dashAttackFrameStartup;
    public int dashAttackFrameDuration;
    public int dashAttackFrameLag;
   
    //neutral air
    public Vector3 nAirHitbox;
    public float nAirRange;
    public int nAirDam;
    public Vector2 nAirBaseK;
    public float nAirScaleK;
    public int nAirFrameStartup;
    public int nAirFrameDuration;
    public int nAirFrameLag;
   
    //forward air
    public Vector3 fAirHitbox;
    public float fAirRange;
    public int fAirDam;
    public Vector2 fAirBaseK;
    public float fAirScaleK;
    public int fAirFrameStartup;
    public int fAirFrameDuration;
    public int fAirFrameLag;
   
    //back air
    public Vector3 bAirHitbox;
    public float bAirRange;
    public int bAirDam;
    public Vector2 bAirBaseK;
    public float bAirScaleK;
    public int bAirFrameStartup;
    public int bAirFrameDuration;
    public int bAirFrameLag;
   
    //up air
    public Vector3 uAirHitbox;
    public float uAirRange;
    public int uAirDam;
    public Vector2 uAirBaseK;
    public float uAirScaleK;
    public int uAirFrameStartup;
    public int uAirFrameDuration;
    public int uAirFrameLag;

    //down air
    public Vector3 dAirHitbox;
    public float dAirRange;
    public int dAirDam;
    public Vector2 dAirBaseK;
    public float dAirScaleK;
    public int dAirFrameStartup;
    public int dAirFrameDuration;
    public int dAirFrameLag;
}