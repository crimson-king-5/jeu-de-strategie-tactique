using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resource_Manager : MonoBehaviour
{
    public Text FoodT;
    public Text StoneT;
    public Text MoonT;
    public Text FermeT;
    public Text MineT;
    public Text HouseT;
    public Text CastleT;


    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindGameObjectsWithTag("Resource").Length > 1)
        {
            Destroy(gameObject);
        }
    }

    public Resources1 Player1 = new Resources1();
    public Resources1 Player2 = new Resources1();
    public struct Resources1
    {
        public bool LockTrade { get; private set; }

        public int Food { get; private set; }
        public int Stone { get; private set; }
        public int Moon { get; private set; }

        public int Ferme { get; private set; }
        public int Mine { get; private set; }
        public int House { get; private set; }
        public int Castle { get; private set; }


        public int LockingTrade()
        {
            if (!LockTrade)
            {
                LockTrade = true;
                return 1;
            }
            return -1;
        }
        public int UnlockingTrade()
        {
            if (LockTrade)
            {
                LockTrade = false;
                return 1;
            }
            return -1;
        }

        public int FoodAdd(int i)
        {
            if (!LockTrade)
            {
                this.Food += i;
                return 1;
            }
            return -1;
        }
        public int StoneAdd(int i)
        {
            if (!LockTrade)
            {
                this.Stone += i;
                return 1;
            }
            return -1;
        }
        public int MoonAdd(int i)
        {
            if (!LockTrade)
            {
                this.Moon += i;
                return 1;
            }
            return -1;
        }

        public int FermeAdd()
        {
            if (!LockTrade)
            {
                this.Ferme++;
                return 1;
            }
            return -1;
        }
        public int CastleAdd()
        {
            if (!LockTrade)
            {
                this.Castle++;
                return 1;
            }
            return -1;
        }
        public int MineAdd()
        {
            if (!LockTrade)
            {
                this.Mine++;
                return 1;
            }
            return -1;
        }
        public int HouseAdd()
        {
            if (!LockTrade)
            {
                this.House++;
                return 1;
            }
            return -1;
        }

        public int FoodRemove(int i)
        {
            if (!LockTrade)
            {
                this.Food -= i;
                return 1;
            }
            return -1;
        }
        public int StoneRemove(int i)
        {
            if (!LockTrade)
            {
                this.Stone -= i;
                return 1;
            }
            return -1;
        }
        public int MoonRemove(int i)
        {
            if (!LockTrade)
            {
                this.Moon -= i;
                return 1;
            }
            return -1;
        }

    }


    // Update is called once per frame
    void Update()
    {
        FoodT.text = "Food : " + Player1.Food.ToString();
        StoneT.text = " Stone : " + Player1.Stone.ToString();
        MoonT.text = "Moon : " + Player1.Moon.ToString();
        FermeT.text = "Ferme : " + Player1.Ferme.ToString();
        MineT.text = "Mine : " + Player1.Mine.ToString();
        HouseT.text = "House : " + Player1.House.ToString();
        CastleT.text = "Castle : " + Player1.Castle.ToString();
    }

}
