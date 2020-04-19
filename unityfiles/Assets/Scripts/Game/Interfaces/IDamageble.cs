/// <summary>
/// Interface for any object that can receive damage
/// </summary>
public interface IDamageable {

    /// <summary>
    /// Applies damage to the damageble by given value
    /// </summary>
    /// <param name="dmg">damage value</param>
    void ApplyDamage(float dmg);

}