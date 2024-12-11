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
        targetUnit.dealDamage(5);
    }

    void HeavyStrike(UnitScript targetUnit)
    {
        targetUnit.dealDamage(10);
    }

    void SoulBarrier(UnitScript targetUnit)
    {
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
    }


    void MagicBlast(UnitScript targetUnit)
    {
        targetUnit.dealDamage(5);
    }

    void MagicBarrage(UnitScript targetUnit)
    {
        targetUnit.dealDamage(10);
    }
}
