/// <summary>
/// Object which need notifications when their health controller has reported
/// damage can implement this interface.
/// </summary>
public interface IDamageNotifyable {

    /// <summary>
    /// Notifier call when the health controller has taken damage
    /// </summary>
    void Damaged();

}