using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;

    private PlayerController controller;

    private Health health;
    private List<Stat> stats;


    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Init()
    {
        Instance = this;
        stats = InitStats();
        health = GetComponent<Health>();
        controller = GetComponent<PlayerController>();
    }

    private List<Stat> InitStats()
    {
        List<Stat> statList = new List<Stat>();

        //Index Pos
        statList.Add(new Stat(Stats.Strength, base_value: 1, base_gain: 1)); //0
        statList.Add(new Stat(Stats.Dexterity, base_value: 1, base_gain: 1)); //1
        statList.Add(new Stat(Stats.Resilience, base_value: 1, base_gain: 1)); //2

        return statList;
    }

    public int GetDamage()
    {
        return /*weapon base damage*/ 3 + stats[0].GetValue() ;
    }
}
