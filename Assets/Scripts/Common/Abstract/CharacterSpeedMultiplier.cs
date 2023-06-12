using System.Collections.Generic;

public class CharacterSpeedMultiplier
{
    public float multiplier;

    protected List<SpeedFactor> speedFactors;

    public CharacterSpeedMultiplier()
    {
        multiplier = 1f;
    }

    public virtual void ReCalculateMultiplier()
    {
        multiplier = 1f;
        foreach (var factor in speedFactors)
        {
            if (factor.Effective)
            {
                multiplier *= 1 + factor.multiplier;
            }
        }
    }

    public virtual void Reset()
    {
        foreach (var factor in speedFactors)
        {
            factor.Effective = false;
        }
    }
}
