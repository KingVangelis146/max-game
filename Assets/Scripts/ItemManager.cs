using UnityEngine;

public class ItemController : MonoBehaviour
{
    [SerializeField] public enum type
    {
        None,
        Heal,
        MaxHeal,
        Mana,
        MaxMana
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    static public void Activate(type item, GameObject player)
    {
         Playercontroller play = player.GetComponent<Playercontroller>();
        switch (item)
        {
            case type.None:
                break;
            case type.Heal:
                play.EditHealth(2, true);
                break;
            case type.MaxHeal:
                play.EditHealth(play.maxHealth, true);
                break;
        }
    }
}
