/// <summary>
/// Objects that can receive damage from obstacles in the world
/// </summary>
public interface IObstacleDamagable {

    /// <summary>
    /// Applies the provided obstacle damage to the damageable type
    /// </summary>
    /// <param name="damage">amount of damage</param>
    void ApplyObstacleDamage(float damage);

}