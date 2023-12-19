using System;

public static class DamageCalculator
{
    // This is a static method that calculates the damage.
    public static int CalculateDamage(int damage, int precisionMod, int wpmMod)
    {
        // Perform the calculation
        float result = (damage * precisionMod * wpmMod) / (100f * 70.00f);

        // Convert the result to an integer and return it
        return Convert.ToInt32(result);
    }
}