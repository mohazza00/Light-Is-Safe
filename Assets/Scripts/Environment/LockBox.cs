using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LockBox : Interactable
{
    [Header("Settings")]
    [SerializeField] private GameObject _lockBoxUI;
    [SerializeField] private GameObject _openedLockBox;
    [SerializeField] private Task LockBoxTask;

    [Header("Password Menu Items")]
    public MenuItem[] DigitItem;
    public int CurrentDigitItem;

    [Header("Texts")]
    public TextMeshProUGUI FirstDigitText;
    public TextMeshProUGUI SecondDigitText;
    public TextMeshProUGUI ThirdDigitText;

    [Header("Images")]
    public Image FirstObjectImage;
    public Image SecondObjectImage;
    public Image ThirdObjectImage;

    private int _firstDigit = 0;
    private int _secondDigit = 0;
    private int _ThirdDigit = 0;

    private int FirstObjectAmount = 0;
    private int SecondObjectAmount = 0;
    private int ThirdObjectAmount = 0;

    private List<LockBoxObjectData> _lockBoxObjects = new List<LockBoxObjectData>();

    public void SetupPassword(List<LockBoxObjectData> objects)
    {
        //Select 3 random objects for the lock box digits
        int safeFailCounter = 0;
        while (_lockBoxObjects.Count < 3)
        {
            safeFailCounter++;
            int rand = UnityEngine.Random.Range(0, objects.Count);

            if (!_lockBoxObjects.Contains(objects[rand]))
            {
                _lockBoxObjects.Add(objects[rand]);
            }
            if (safeFailCounter >= 100)
                break;
        }

        FirstObjectImage.sprite = _lockBoxObjects[0].Icon;
        FirstObjectAmount = _lockBoxObjects[0].Amount;
        FirstObjectImage.SetNativeSize();

        SecondObjectImage.sprite = _lockBoxObjects[1].Icon;
        SecondObjectAmount = _lockBoxObjects[1].Amount;
        SecondObjectImage.SetNativeSize();

        ThirdObjectImage.sprite = _lockBoxObjects[2].Icon;
        ThirdObjectAmount = _lockBoxObjects[2].Amount;
        ThirdObjectImage.SetNativeSize();

    }
    public void OpenLockBox()
    {
        CurrentDigitItem = 0;
        _lockBoxUI.SetActive(true);
    }

    public void CloseLockBox()
    {
        _lockBoxUI.SetActive(false);
    }

    public void MoveCursorLeft()
    {
        CurrentDigitItem--;

        if (CurrentDigitItem < 0)
        {
            CurrentDigitItem = DigitItem.Length - 1;
        }

        foreach (MenuItem item in DigitItem)
        {
            item.DeselectItem();
        }

        DigitItem[CurrentDigitItem].SelectItem();
    }

    public void MoveCursorRight()
    {
        CurrentDigitItem = (CurrentDigitItem + 1) % DigitItem.Length;

        foreach (MenuItem item in DigitItem)
        {
            item.DeselectItem();
        }

        DigitItem[CurrentDigitItem].SelectItem();
    }

    public void MoveCursorUp()
    {
        if(CurrentDigitItem == 0)
        {
            _firstDigit = (_firstDigit + 1) % 10;
            FirstDigitText.text = _firstDigit.ToString();

        }
        else if (CurrentDigitItem == 1)
        {
            _secondDigit = (_secondDigit + 1) % 10;
            SecondDigitText.text = _secondDigit.ToString();

        }
        else if (CurrentDigitItem == 2)
        {
            _ThirdDigit = (_ThirdDigit + 1) % 10;
            ThirdDigitText.text = _ThirdDigit.ToString();
        }
    }

    public void MoveCursorDown()
    {
        if (CurrentDigitItem == 0)
        {
            _firstDigit--;
            if (_firstDigit < 0) _firstDigit = 9;     
            FirstDigitText.text = _firstDigit.ToString();
        }
        else if (CurrentDigitItem == 1)
        {
            _secondDigit--;
            if (_secondDigit < 0) _secondDigit = 9;
            SecondDigitText.text = _secondDigit.ToString();
        }
        else if (CurrentDigitItem == 2)
        {
            _ThirdDigit--;
            if (_ThirdDigit < 0) _ThirdDigit = 9;
            ThirdDigitText.text = _ThirdDigit.ToString();
        }
    }

    public void CheckPassword()
    {
        CloseLockBox();
        if(_firstDigit == FirstObjectAmount &&
            _secondDigit == SecondObjectAmount &&
            _ThirdDigit == ThirdObjectAmount)
        {
            LockBoxTask.ClearTask();
            CanInteract = false;
            _openedLockBox.SetActive(true);
        }
        else
        {
            foreach (MenuItem item in DigitItem)
            {
                item.DeselectItem();
            }
            AudioManager.Instance.PlaySound(SoundName.LOCKED);
        }
    }
}

