using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TEAM2;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager _Instance;

    public static UIManager Instance
    {
        get { return _Instance; }
    }

    private void Awake()
    {
        if (_Instance == null)
        {
            _Instance = this;
        }
    }

    public GameObject orderUI;

    [System.Serializable]
    struct Orders
    {
        public GameObject Walk;
        public GameObject Attack;
        public GameObject Defend;
        public GameObject Build;
    }

    [System.Serializable]
    struct OrdersImgs
    {
        public GameObject Walk;
        public GameObject Attack;
        public GameObject Defend;
        public GameObject Build;
    }

    [SerializeField] private Orders orders;
    [SerializeField] private OrdersImgs orderImgs;
    [SerializeField] private GameObject orderUIList;
    [SerializeField] private Color p1UIColor;
    [SerializeField] private Color p2UIColor;

    void Start()
    {
        ChangeOrderPlayer();
        orders.Walk.GetComponent<Button>().onClick.AddListener(delegate { MoveOrder.SetMove(); });
        orders.Attack.GetComponent<Button>().onClick.AddListener(delegate { AttackOrder.SetAttack(); });
        orders.Defend.GetComponent<Button>().onClick.AddListener(delegate { DefendOrder.SetDefend(); });
        orders.Build.GetComponent<Button>().onClick.AddListener(delegate { BuildOrder.SetBuild(); });
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) 
        {
            GameManager.Instance.PlayerManager.CurrentPlayer.MakeUnitsEnd(); 
            ChangeOrderPlayer(); 
        }
    }

    public void ChangeOrderPlayer()
    {
        Color col = Color.white;
        if (GameManager.Instance.PlayerManager.CurrentPlayer == GameManager.Instance.P1) col = p1UIColor;
        else if (GameManager.Instance.PlayerManager.CurrentPlayer == GameManager.Instance.P2) col = p2UIColor;
        for (int i = 0; i < orderUI.transform.childCount; i++)
        {
            orderUI.transform.GetChild(i).GetComponent<Image>().color = col;
        }
    }

    public void CreateOrder(OrderType type)
    {
        Order order;
        switch (type)
        {
            case OrderType.WALK:
                order = new MoveOrder();
                AddOrderUI(orderImgs.Walk);
                break;

            case OrderType.ATTACK:
                order = new AttackOrder();
                AddOrderUI(orderImgs.Attack);
                break;

            case OrderType.DEFEND:
                order = new DefendOrder();
                AddOrderUI(orderImgs.Defend);
                break;

            case OrderType.BUILD:
                order = new BuildOrder();
                AddOrderUI(orderImgs.Build);
                break;

            default:
                order = new AttackOrder();
                AddOrderUI(orderImgs.Attack);
                break;

        }
        GameManager.Instance.PlayerManager.CurrentPlayer.AddOrderToList(order);
        TileSelector.instance.ResetTile();
    }

    void AddOrderUI(GameObject img)
    {
        GameObject go = Instantiate(img);
        go.transform.parent = orderUIList.transform;
        go.GetComponent<RectTransform>().localScale = Vector3.one;
    }
    
}