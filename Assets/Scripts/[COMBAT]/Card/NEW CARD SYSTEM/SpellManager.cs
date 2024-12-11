using UnityEngine;

public class SpellManager : MonoBehaviour
{
    // Define spell types as an enum
    public enum SpellType
    {
        Strike,
        HeavyStrike,
        SoulBarrier,
        MagicBlast,
        MagicBarrage
    }

    // Reference to the UnitScript (assign this in the Inspector)
    public UnitScript unitScript;

    // Call the appropriate spell effect based on the SpellType
    public void CastSpell(SpellType spellType, UnitScript targetUnit)
    {
        if (targetUnit != null)
        {
            switch (spellType)
            {
                case SpellType.Strike:
                    Strike(targetUnit);
                    break;
                case SpellType.HeavyStrike:
                    HeavyStrike(targetUnit);
                    break;
                case SpellType.SoulBarrier:
                    SoulBarrier(targetUnit);
                    break;
                case SpellType.MagicBlast:
                    MagicBlast(targetUnit);
                    break;
                case SpellType.MagicBarrage:
                    MagicBarrage(targetUnit);
                    break;
                default:
                    Debug.LogError("Unknown spell type: " + spellType);
                    break;
            }
        }
    }

    // Define the spell effects for each spell type, now accepting the target unit
    void Strike(UnitScript targetUnit)
    {
        unitScript.SetAttacking(true);  // Trigger attacking animation for the caster
        targetUnit.dealDamage(5);
        unitScript.SetAttacking(false); // Reset animation after casting
    }

    void HeavyStrike(UnitScript targetUnit)
    {
        unitScript.SetAttacking(true);  // Trigger attacking animation for the caster
        targetUnit.dealDamage(10);
        unitScript.SetAttacking(false); // Reset animation after casting
    }

    void SoulBarrier(UnitScript targetUnit)
    {
        unitScript.SetAttacking(true);  // Trigger attacking animation for the caster
                                        // Check if the target unit belongs to team 0
        if (targetUnit.teamNum == 0)
        {
            targetUnit.currentHealthPoints += 5;

            // Prevent health from exceeding max health
            if (targetUnit.currentHealthPoints > targetUnit.maxHealthPoints)
            {
                targetUnit.currentHealthPoints = targetUnit.maxHealthPoints;
            }

            targetUnit.updateHealthUI();
        }
        unitScript.SetAttacking(false); // Reset animation after casting
    }

    void MagicBlast(UnitScript targetUnit)
    {
        unitScript.SetAttacking(true);  // Trigger attacking animation for the caster
        targetUnit.dealDamage(5);
        unitScript.SetAttacking(false); // Reset animation after casting
    }

    void MagicBarrage(UnitScript targetUnit)
    {
        unitScript.SetAttacking(true);  // Trigger attacking animation for the caster
        targetUnit.dealDamage(10);
        unitScript.SetAttacking(false); // Reset animation after casting
    }

}
