using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Flux.UI.Popup
{
    public class UICharacterStatusPopup : UIPopup
    {
        [SerializeField] private TextMeshProUGUI _statusSTRText;
        [SerializeField] private TextMeshProUGUI _statusDEXText;
        [SerializeField] private TextMeshProUGUI _statusHPText;
        [SerializeField] private TextMeshProUGUI _statusMPText;
        
        public void ShowLayout(int id)
        {
            Debug.Log($"{this.GetType().Name} : ShowLayout");
            
            // Todo : Do Something
            // 더미 데이터(이후 Table이나 ScriptableObject을 이용하여 바인딩)
            switch (id)
            {
                case 1:
                    _statusSTRText.text = "90";
                    _statusDEXText.text = "120";
                    _statusHPText.text = "500";
                    _statusMPText.text = "100";
                    break;
                case 2:
                    _statusSTRText.text = "100";
                    _statusDEXText.text = "70";
                    _statusHPText.text = "1000";
                    _statusMPText.text = "50";
                    break;
                default:
                    _statusSTRText.text = "80";
                    _statusDEXText.text = "150";
                    _statusHPText.text = "400";
                    _statusMPText.text = "120";
                    break;
            }
        }

        public void OnTouchClose()
        {
            // 백버튼이나 팝업을 닫는 로직 수행...
            UINavigator.Instance.OnProcessBackButton();
            //UINavigator.Instance.ClosePopup();
        }

        #region override

        public override void OnRefresh()
        {
            Debug.Log($"{this.GetType().Name} : OnRefresh");

            // Todo : Do Something
        }
        public override void OnClose()
        {
            Debug.Log($"{this.GetType().Name} : OnClose");
            
            // Todo : Do Something
        }
        #endregion    
    }
}