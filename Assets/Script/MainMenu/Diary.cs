﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Category { records , humans, irregulars, raChips }

public class Diary : MonoBehaviour {
    public Dictionary<Category, List<int>> Diaries { get; private set; }
    private DiaryTab record;
    private DiaryTab human;
    private DiaryTab irregular;
    private DiaryTab raChip;
    private Color activeColor = new Color(0.9f, 0.9f, 0.9f, 255);
    private Color deActiveColor = new Color(0.3f,0.3f,0.3f,255);
    private Transform content;
    private Text unlockRate;
    private MainMenu.voidFunc mainmenufunc;

    public void Awake()
    {
        record = transform.Find("Record").GetComponent<DiaryTab>();
        human = transform.Find("Human").GetComponent<DiaryTab>();
        irregular = transform.Find("Irregular").GetComponent<DiaryTab>();
        raChip = transform.Find("R.A.Chip").GetComponent<DiaryTab>();
        content= transform.Find("List").Find("Scroll View").Find("Viewport").Find("Content");
        unlockRate = transform.Find("PercentText").GetComponent<Text>();
        ParseDiary();
    }

    public void On(MainMenu.voidFunc voidFunc)
    {
        mainmenufunc = voidFunc;
        gameObject.SetActive(true);
        SetUnlcokRate();
        CheckNew();
        SetTabColor(Category.records);
        DisplayList(Category.records);
    }

    public void Off()
    {
        mainmenufunc();
        gameObject.SetActive(false);
    }

    public void OnRecordButtonDown()
    {
        SetTabColor(Category.records);
        DisplayList(Category.records);
    }

    public void OnHumanButtonDown()
    {
        SetTabColor(Category.humans);
        DisplayList(Category.humans);
    }

    public void OnIrregularButtonDown()
    {
        SetTabColor(Category.irregulars);
        DisplayList(Category.irregulars);
    }

    public void OnChipButtonDown()
    {
        SetTabColor(Category.raChips);
        DisplayList(Category.raChips);
    }

    public void ParseDiary()
    {
        Diaries = new Dictionary<Category, List<int>>();
        Diaries.Add(Category.records, new List<int>());
        Diaries.Add(Category.humans, new List<int>());
        Diaries.Add(Category.irregulars, new List<int>());
        Diaries.Add(Category.raChips, new List<int>());
        for (int i = 1; i <= Database.diaryDatas.Count; i++)
        {
            switch (Database.diaryDatas[i].category)
            {
                case Category.irregulars: Diaries[Category.irregulars].Add(i);  break;
                case Category.raChips: Diaries[Category.raChips].Add(i); break;
                case Category.records: Diaries[Category.records].Add(i); break;
                case Category.humans: Diaries[Category.humans].Add(i); break;
            }
        }
    }

    public void DisplayList(Category category)
    {
        Transform[] childList = content.GetComponentsInChildren<Transform>(true);
        if (childList != null)
        {
            for (int i = 0; i < childList.Length; i++)
            {
                if (childList[i] != content)
                    Destroy(childList[i].gameObject);
            }
        }

        GameObject entityPrefab = Resources.Load<GameObject>("DiaryEntity");
        GameObject entity;

        for (int i = 0; i < Diaries[category].Count; i++)
        {
            entity = Instantiate(entityPrefab, content);
            if (SaveData.diaryUnlockData[(Diaries[category])[i]][0] == true)
            {
                entity.GetComponent<DiaryEntity>().SetEntity(Database.diaryDatas[(Diaries[category])[i]]);
            }
            else
                entity.GetComponent<DiaryEntity>().SetEntity(null);
        }
    }

    public void CheckNew()
    {
        record.CheckNew();
        human.CheckNew();
        raChip.CheckNew();
        irregular.CheckNew();
    }

    public void SetTabColor(Category category)
    {
        record.SetTabColor(deActiveColor);
        human.SetTabColor(deActiveColor);
        irregular.SetTabColor(deActiveColor);
        raChip.SetTabColor(deActiveColor);
        switch (category)
        {
            case Category.irregulars: irregular.SetTabColor(activeColor); break;
            case Category.raChips: raChip.SetTabColor(activeColor); break;
            case Category.records: record.SetTabColor(activeColor); break;
            case Category.humans: human.SetTabColor(activeColor); break;
        }
    }

    public void SetUnlcokRate()
    {
        int trueCount = 0;
        for (int idx = 1; idx < SaveData.diaryUnlockData.Count; idx++)
        {
            if (SaveData.diaryUnlockData[idx][0] == true) trueCount++;
        }
        unlockRate.text = (float)trueCount*100 / (float)(SaveData.diaryUnlockData.Count - 1) + "%";
    }
}
