using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.UI;
using System;
using RandomOrg.CoreApi;

public class DiceRoller : MonoBehaviour
{
    //looping dice animation with fade in start
    [SerializeField] Animator DiceAnimation;
    //dice sprite that shows the result of the roll
    [SerializeField] Image ResultDice;
    //list of dice sprites arranged relative to the dice number roll 1-6
    [SerializeField] List<Sprite> DiceSprites;
    //callback on finished dice rolling (calculated result)
    [SerializeField] Action<int> CallOnDiceRolled;

    public void Init()
    {
        CallOnDiceRolled += GameManager.Instance.MoveChip;
        ResultDice.color = Vector4.zero;
        DiceAnimation.gameObject.SetActive(false);
    }

    //fetch random number using random.org api and broadcast it using the callback
    public async void RollDice()
    {
        ResultDice.color = Vector4.zero;
        DiceAnimation.gameObject.SetActive(true);
        int diceResult = RandomGeneratorInstance.Instance.GetRandomNumber(1,6);
        await Task.Delay(1500);
        DiceAnimation.gameObject.SetActive(false);

        ResultDice.sprite = DiceSprites[diceResult-1];
        ResultDice.color = Vector4.one;

        CallOnDiceRolled?.Invoke(diceResult);
    }

}
