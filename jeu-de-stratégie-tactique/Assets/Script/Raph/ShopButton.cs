using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopButton : MonoBehaviour
{
    private Resource_Manager R;

    //Modifier le type de boutton permet de modifier le type de batiment acheter
    public enum Upgrade { Ferme, Mine, House, Castle }
    public Upgrade type;

    //Coût des batiments
    public struct Ferme
    {
        public static int Food { get => 0; }
        public static int Stone { get => 3; }
        public static int Moon { get => 0; }
    }
    public struct Mine
    {
        public static int Food { get => 3; }
        public static int Stone { get => 4; }
        public static int Moon { get => 0; }
    }
    public struct House
    {
        public static int Food { get => 3; }
        public static int Stone { get => 2; }
        public static int Moon { get => 0; }
    }
    public struct Castle
    {
        public static int Food { get => 8; }
        public static int Stone { get => 5; }
        public static int Moon { get => 0; }
    }

    //La fonction d'échange
    public void Trading()
    {
        switch (type)
        {
            case Upgrade.Ferme:
                if (R.Player1.Food >= Ferme.Food && R.Player1.Stone >= Ferme.Stone && R.Player1.Moon >= Ferme.Moon)
                {
                    R.Player1.UnlockingTrade();
                    if (Ferme.Food != 0)
                    {
                        Debug.Log("food--");
                        R.Player1.FoodRemove(Ferme.Food);
                    }
                    if (Ferme.Stone != 0)
                    {
                        R.Player1.StoneRemove(Ferme.Stone);
                        Debug.Log("stone--");
                    }
                    if (Ferme.Moon != 0)
                    {
                        Debug.Log("moon--");
                        R.Player1.MoonRemove(Ferme.Moon);
                    }
                    R.Player1.FermeAdd();
                    Debug.Log("ferme++");
                    Debug.Log(R.Player1.Food);
                    Debug.Log(R.Player1.Stone);
                    Debug.Log(R.Player1.Moon);
                    R.Player1.LockingTrade();

                    Debug.Log("upgrade");
                }
                break;

            case Upgrade.Mine:
                if (R.Player1.Food >= Mine.Food && R.Player1.Stone >= Mine.Stone && R.Player1.Moon >= Mine.Moon)
                {
                    R.Player1.UnlockingTrade();
                    if (Mine.Food != 0)
                    {
                        Debug.Log("food--");
                        R.Player1.FoodRemove(Mine.Food);
                    }
                    if (Mine.Stone != 0)
                    {
                        R.Player1.StoneRemove(Mine.Stone);
                        Debug.Log("stone--");
                    }
                    if (Mine.Moon != 0)
                    {
                        Debug.Log("moon--");
                        R.Player1.MoonRemove(Mine.Moon);
                    }
                    R.Player1.MineAdd();
                    Debug.Log("ferme++");
                    Debug.Log(R.Player1.Food);
                    Debug.Log(R.Player1.Stone);
                    Debug.Log(R.Player1.Moon);
                    R.Player1.LockingTrade();

                    Debug.Log("upgrade");
                }
                break;

            case Upgrade.House:
                if (R.Player1.Food >= House.Food && R.Player1.Stone >= House.Stone && R.Player1.Moon >= House.Moon)
                {
                    R.Player1.UnlockingTrade();
                    if (House.Food != 0)
                    {
                        Debug.Log("food--");
                        R.Player1.FoodRemove(House.Food);
                    }
                    if (House.Stone != 0)
                    {
                        R.Player1.StoneRemove(House.Stone);
                        Debug.Log("stone--");
                    }
                    if (House.Moon != 0)
                    {
                        Debug.Log("moon--");
                        R.Player1.MoonRemove(House.Moon);
                    }
                    R.Player1.HouseAdd();
                    Debug.Log("ferme++");
                    Debug.Log(R.Player1.Food);
                    Debug.Log(R.Player1.Stone);
                    Debug.Log(R.Player1.Moon);
                    R.Player1.LockingTrade();

                    Debug.Log("upgrade");
                }
                break;

            case Upgrade.Castle:
                if (R.Player1.Food >= Castle.Food && R.Player1.Stone >= Castle.Stone && R.Player1.Moon >= Castle.Moon)
                {
                    R.Player1.UnlockingTrade();
                    if (Castle.Food != 0)
                    {
                        Debug.Log("food--");
                        R.Player1.FoodRemove(Castle.Food);
                    }
                    if (Castle.Stone != 0)
                    {
                        R.Player1.StoneRemove(Castle.Stone);
                        Debug.Log("stone--");
                    }
                    if (Castle.Moon != 0)
                    {
                        Debug.Log("moon--");
                        R.Player1.MoonRemove(Castle.Moon);
                    }
                    R.Player1.CastleAdd();
                    Debug.Log("ferme++");
                    Debug.Log(R.Player1.Food);
                    Debug.Log(R.Player1.Stone);
                    Debug.Log(R.Player1.Moon);
                    R.Player1.LockingTrade();

                    Debug.Log("upgrade");
                }
                break;

            default:

                break;
        }
    }

    //Les fonctions d'ajout de ressource par bouton (Debug Only)
    public void FoodAdd()
    {
        R.Player1.UnlockingTrade();
        R.Player1.FoodAdd(1);
        R.Player1.LockingTrade();
    }
    public void StoneAdd()
    {
        R.Player1.UnlockingTrade();
        R.Player1.StoneAdd(1);
        R.Player1.LockingTrade();
    }
    public void MoonAdd()
    {
        R.Player1.UnlockingTrade();
        R.Player1.MoonAdd(1);
        R.Player1.LockingTrade();
    }

    // Start is called before the first frame update
    void Start()
    {
        //recupere le resource manager
        R = GameObject.FindGameObjectWithTag("Resource").GetComponent<Resource_Manager>();
    }
}
