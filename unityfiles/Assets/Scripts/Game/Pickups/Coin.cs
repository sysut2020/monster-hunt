

using UnityEngine;

/// <summary>
/// suggestion for a coin type class
/// </summary>
public class Coin : MonoBehaviour, IPickup {

    private int amount;
    private bool active;

    public Coin(int amount){
        this.amount = amount;
    }

    public void Spawn(Transform pos){
        // somthing like Instantiate(Prefab)
    }

    public void Activate(){
        PlayerInventory.Instance.AddMoney(amount);
    }

    private void Update(){
        if (active){
            // movestuff
        }
    }

}