using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEV_Test_Enemy_Manager : MonoBehaviour // By Samuel White
{
    //=====================================
    // This script is built for the designers to experiment with spawning enemies with their own assigned scriptable object data.
    //=====================================

    public enum Enemy
    {
        Imp, Succubus, Chef, Oni, Limb
    }
    public Enemy selectedEnemy;

    [SerializeField] Enemy_Character_Data selectedEnemydata;

    [SerializeField] New_Level_Manager.Wave.EnemySpawn.EnemyInfo.SpawnType spawnType;

    [Serializable]
    public struct EnemyData
    {
        public string name;
        public SO_Standard_Enemy_Movement movementData;
        public SO_Standard_Enemy_Attack attackData;
        public SO_Proj_Eni_Bas projectileData; 
    }
    [SerializeField] EnemyData[] enemyDatas;

    private void Awake()
    {
        enemyDatas.Initialize(); // Unsure what this does... Hoping it'll restrict data from being added.
    }

    public void SelectEnemy(int enemy)
    {
        selectedEnemydata = New_Enemy_Pool_System.instance.GetEnemy(enemy);
        if (selectedEnemydata == null) return;

        selectedEnemydata.spawnType = spawnType;
        selectedEnemydata.targetPosition = new(0, 0, 10);
        switch (enemy)
        {
            case 0: selectedEnemy = Enemy.Imp; break;
            case 1: selectedEnemy = Enemy.Succubus; break;
            case 2: selectedEnemy = Enemy.Chef; break;
            case 3: selectedEnemy = Enemy.Oni; break;
            case 4: selectedEnemy = Enemy.Limb; break;
        }
        selectedEnemydata.movementData = enemyDatas[enemy].movementData;
        selectedEnemydata.attackData = enemyDatas[enemy].attackData;
        selectedEnemydata.projectileData = enemyDatas[enemy].projectileData;

        if (selectedEnemydata != null)
        {
            selectedEnemydata.gameObject.SetActive(false);
            New_Enemy_Pool_System.instance.ReturnEnemy(selectedEnemydata);
        }
    }

    public void SelectEnterType(int type)
    {
        switch (type)
        {
            case 0: spawnType = New_Level_Manager.Wave.EnemySpawn.EnemyInfo.SpawnType.Portal; break;
            case 1: spawnType = New_Level_Manager.Wave.EnemySpawn.EnemyInfo.SpawnType.Front; break;
            case 2: spawnType = New_Level_Manager.Wave.EnemySpawn.EnemyInfo.SpawnType.Behind; break;
        }
    }

    public void SummonEnemy()
    {
        if (selectedEnemydata == null) return;

        selectedEnemydata.gameObject.SetActive(true);
        selectedEnemydata.spawnType = spawnType;
        selectedEnemydata.StartEnterance();
    }
    public void UnSummonEnemy()
    {
        if (selectedEnemydata == null) return;

        selectedEnemydata.transform.position = new();
        selectedEnemydata.gameObject.SetActive(false);
    }

    public void EnemyShoot()
    {
        if (selectedEnemydata == null) return;

        selectedEnemydata.ChangeState(selectedEnemydata.ShootState);
    }

    public void EnemyStopShoot()
    {
        if (selectedEnemydata == null) return;
        selectedEnemydata.ChangeState(selectedEnemydata.IdleState);
    }
}
