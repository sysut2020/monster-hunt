/// <summary>
/// Objects that can revice damage from obstacles in the world
/// </summary>
public interface IObstacleDamagable {

    /// <summary>
    /// Applies the provided obstacle damage to the damageable type
    /// </summary>
    /// <param name="damage">amont of damage</param>
    void ApplyObstacleDamage (float damage);

}