using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AttackData : MonoBehaviour
{
    //jab
    public int jabDam; //damage the attack will do
    public float jabBaseK; //knockback attack will do independent of damage
    public float jabScaleK; //extra knockback attack will do based off of damage
    public float jabAngle; //knockback angle
    public int jabFrameStartup; //how long it takes for the attack to start after you input it in frames
    public int jabFrameDuration; //how long the hitbox remains active for in frames
    public int jabFrameLag; //how long it takes untiil you can input an action after the attack ends in frames

    //forward light
    public int fLightDam;
    public float fLightBaseK;
    public float fLightScaleK;
    public float fLightAngle;
    public int fLightFrameStartup;
    public int fLightFrameDuration;
    public int fLightFrameLag;
   
    //down light
    public int dLightDam;
    public float dLightBaseK;
    public float dLightScaleK;
    public float dLightAngle;
    public int dLightFrameStartup;
    public int dLightFrameDuration;
    public int dLightFrameLag;
   
    //up light
    public int uLightDam;
    public float uLightBaseK;
    public float uLightScaleK;
    public float uLightAngle;
    public int uLightFrameStartup;
    public int uLightFrameDuration;
    public int uLightFrameLag;
   
    //forward strong
    public int fStrongDam;
    public float fStrongBaseK;
    public float fStrongScaleK;
    public float fStrongAngle;
    public int fStrongFrameStartup;
    public int fStrongFrameDuration;
    public int fStrongFrameLag;
   
    //down strong
    public int dStrongDam;
    public float dStrongBaseK;
    public float dStrongScaleK;
    public float dStrongAngle;
    public int dStrongFrameStartup;
    public int dStrongFrameDuration;
    public int dStrongFrameLag;
   
    //up strong
    public int uStrongDam;
    public float uStrongBaseK;
    public float uStrongScaleK;
    public float uStrongAngle;
    public int uStrongFrameStartup;
    public int uStrongFrameDuration;
    public int uStrongFrameLag;
   
    //dash attack
    //not sure if we need this one
    public int dashAttackDam;
    public float dashAttackBaseK;
    public float dashAttackScaleK;
    public float dashAttackAngle;
    public int dashAttackFrameStartup;
    public int dashAttackFrameDuration;
    public int dashAttackFrameLag;
   
    //neutral air
    public int nAirDam;
    public float nAirBaseK;
    public float nAirScaleK;
    public float nAirAngle;
    public int nAirFrameStartup;
    public int nAirFrameDuration;
    public int nAirFrameLag;
   
    //forward air
    public int fAirDam;
    public float fAirBaseK;
    public float fAirScaleK;
    public float fAirAngle;
    public int fAirFrameStartup;
    public int fAirFrameDuration;
    public int fAirFrameLag;
   
    //back air
    public int bAirDam;
    public float bAirBaseK;
    public float bAirScaleK;
    public float bAirAngle;
    public int bAirFrameStartup;
    public int bAirFrameDuration;
    public int bAirFrameLag;
   
    //up air
    public int uAirDam;
    public float uAirBaseK;
    public float uAirScaleK;
    public float uAirAngle;
    public int uAirFrameStartup;
    public int uAirFrameDuration;
    public int uAirFrameLag;

    //down air
    public int dAirDam;
    public float dAirBaseK;
    public float dAirScaleK;
    public float dAirAngle;
    public int dAirFrameStartup;
    public int dAirFrameDuration;
    public int dAirFrameLag;
}