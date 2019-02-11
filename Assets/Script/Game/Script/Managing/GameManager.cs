﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using ClientSide;

public class GameManager : GameManagerBase {

    //게임플레이에 관련된 변수.
    private GameObject Planet;
    private int playerNumber;
    public PlayerControlThree PlayerControl;
    public HQControl HQ;
    private GameObject Sheephorde;
    private GameObject sheepPrefab;
    /*public GameObject silversheepprefab;
    public GameObject goldensheepprefab;*/
    private GameObject BackGround;

    private List<SheepControlThree> hordeSheepList;
    private List<SheepControlThree> owneredSheepList;

    //Object 생성 관련된 변수들.
    public float PlanetScale;
    public int initialSheep;
    private int currentSheepNum;
    private int maxSheepNum;
    private float midTime;

    public PlayerControlThree GetPlayer()
    {
        return PlayerControl;
    }

    protected override void Awake()
    {
        base.Awake();
        Application.targetFrameRate = 60;
    }

    protected override void Start()
    {
        base.Start();
        ManagerHandler.Instance.SetManager(this);
    }

    protected override void InitManager()
    {
        base.InitManager();
        //Awake에서 실행하던 함수
        Planet = GameObject.Find("Planet");
        Sheephorde = GameObject.FindGameObjectWithTag("SheepHorde");
        BackGround = GameObject.Find("BackGround");
        sheepPrefab = Resources.Load<GameObject>("Prefab/Sheeps/CartoonSheep");
        GameObject GrassPrefab = Resources.Load<GameObject>("Prefab/BackgroundObject/Grass");
        GameObject FlowerPrefab = Resources.Load<GameObject>("Prefab/BackgroundObject/Flower");
        GameObject GravelPrefab = Resources.Load<GameObject>("Prefab/BackgroundObject/Gravel");

        hordeSheepList = new List<SheepControlThree>();
        owneredSheepList = new List<SheepControlThree>();

        //오브젝트 생성.
        //랜덤 시드를 정해줌. 
        Random.InitState(KingGodClient.Instance.Seed);

        //Start에서 실행하던 함수
        midTime = 0;
        this.playerNumber = KingGodClient.Instance.playerNum;
        InitPlayer();
        SheepSpawn(sheepPrefab, PlanetScale, initialSheep+36, initialSheep);
        ObjectSpawn(GrassPrefab, PlanetScale + 0.5f, 50);
        ObjectSpawn(FlowerPrefab, PlanetScale + 0.5f, 10);
        ObjectSpawn(GravelPrefab,PlanetScale + 0.5f, 10);
    }

    private void InitPlayer()
    {
        Color playerColor = new Color(50 / 255f, 75 / 255f, 200 / 255f, 1);
        Color EnemyColor = new Color(200 / 255f, 50 / 255f, 75 / 255f, 1);
        PlayerControl.SetSymbolColor(playerColor);
        Debug.Log("Search Complete");
    }

    private void SheepSpawn(GameObject sheepprefab, float scale, int maxNumber, int initialNumber)   //양을 임의의 위치에 소환하는 메서드.
    {
        currentSheepNum = initialNumber-1;
        maxSheepNum = maxNumber-1;
        for (int i = 0; i < maxSheepNum; i++)
        {
            Vector3 newposition = Random.onUnitSphere * scale;
            if (Vector3.Distance(newposition, PlayerControl.transform.position) > 4 && Vector3.Distance(newposition, HQ.transform.position) > 4)
            {
                GameObject tempSheep = Instantiate(sheepprefab, newposition, Quaternion.Euler(0, 0, 0), Sheephorde.transform);
                tempSheep.transform.rotation = Quaternion.FromToRotation(tempSheep.transform.up, newposition) * tempSheep.transform.rotation;
                hordeSheepList.Add(tempSheep.GetComponent<SheepControlThree>());
                if (i >= initialNumber)
                {
                    tempSheep.gameObject.SetActive(false);
                }
            }
            else
            {
                i--;
            }
        }
    }

    private void ObjectSpawn(GameObject Objectprefab, float scale, int number)
    {
        for (int i = 0; i < number; i++)
        {
            Vector3 newposition = Random.onUnitSphere * scale;
            if (Vector3.Distance(newposition, PlayerControl.transform.position) > 4 && Vector3.Distance(newposition, HQ.transform.position) > 4)
            {
                GameObject tempObject = Instantiate(Objectprefab, newposition, Quaternion.Euler(0, 0, 0), BackGround.transform);
                tempObject.transform.rotation = Quaternion.FromToRotation(tempObject.transform.up, newposition) * tempObject.transform.rotation;
            }
            else
            {
                i--;
            }
        }
    }

    public void FromHordeSheepToOwneredSheep(SheepControlThree target)
    {
        hordeSheepList.Remove(target);
        owneredSheepList.Add(target);
        DecreaseCurrentSheepNum();
    }

    private void SheepReActive()
    {
        if (currentSheepNum >= maxSheepNum)
            return;
        else
        {
            hordeSheepList[currentSheepNum].gameObject.SetActive(true);
            currentSheepNum += 1;
        }
    }

    public int GetCurrentSheepNum()
    {
        return this.currentSheepNum;
    }

    private void DecreaseCurrentSheepNum()
    {
        this.currentSheepNum -= 1;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Application.runInBackground = true;

        if (ManagerHandler.Instance.GameTime().GetTimePass() - midTime > 5)
        {
            midTime = ManagerHandler.Instance.GameTime().GetTimePass();
            KingGodClient.Instance.GetNetworkMessageSender().SendPlayerEnemyPositionToServer(this.PlayerControl.transform.position, this.playerNumber, ManagerHandler.Instance.GameTime().GetTimePass());
            SheepReActive();
        }
    }

    public SheepControlThree GetSheepFromHordeSheepList(int index)
    {
        return this.hordeSheepList[index];
    }

    public SheepControlThree GetSheepFromOwneredSheepList(int index)
    {
        return this.owneredSheepList[index];
    }

    public int GetHordeSheepListCount()
    {
        return this.hordeSheepList.Count;
    }

    public int GetOwneredSheepListCount()
    {
        return this.owneredSheepList.Count;
    }

    public Transform GetPlanetTransform()
    {
        return this.Planet.transform;
    }

    public Transform GetHQTransform()
    {
        return this.HQ.transform;
    }
}