using System.Collections.Generic;
using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Game{
    public class Dice : BaseScreen<MenuScreen> {
        
        private Button _dice1;
        private Button _dice2;
        private Button _roll;
        private Label _yourolled;
        private Label _rolltotal;
        private int _i;
        private int _k;
        private Background _background;
        public Texture2D[] spriteList;


        public override void Initialise(){
            _roll = Root.Q<Button>("roll");
            _dice1 = Root.Q<Button>("dice1");
            _dice2 = Root.Q<Button>("dice2");
            _rolltotal = Root.Q<Label>("rolltotal");
            _yourolled = Root.Q<Label>("yourolled");
            
            _rolltotal.style.display = DisplayStyle.None;
            _yourolled.style.display = DisplayStyle.None;

            _i = 0;
            _k = 0;
            _roll.RegisterCallback<ClickEvent>(OnRollClicked);
            spriteList = Resources.LoadAll<Texture2D>("Images/Icons/Dice");
        }

        protected override void CleanUp(){
            _roll.UnregisterCallback<ClickEvent>(OnRollClicked);
        }
        
        public void OnRollClicked(ClickEvent e){
            Debug.Log("roll dice");
            _roll.style.display = DisplayStyle.None;
            StartCoroutine(Sleep(0.5f));
            _rolltotal.style.display = DisplayStyle.Flex;
            _yourolled.style.display = DisplayStyle.Flex;
        }

        public IEnumerator Sleep(float waitTime)
        {
            int j = 0;
            while (j < 10)
            {
                _i = Random.Range(1,6);
                _k = Random.Range(1,6);
                _dice1.style.backgroundImage = spriteList[_i];
                _dice2.style.backgroundImage = spriteList[_k];
                j = j + 1;
                yield return new WaitForSeconds(waitTime);
            }

            _rolltotal.text = (_i + _k + 2).ToString();
        }
    }
}
