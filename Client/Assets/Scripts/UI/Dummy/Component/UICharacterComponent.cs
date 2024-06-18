using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Flux.UI.Component
{
    public class UICharacterComponent : UIComponent
    {
        [SerializeField] private TextMeshProUGUI _characterNameText;
        [SerializeField] private GameObject[] _characterPortraits;

        private int _characterId = 0;
        
        public System.Action<int> onEventSelectCharacter = null;
        
        
        public void ShowLayout(int id)
        {
            _characterId = id;

            SetCharacter(id);
        }

        private void SetCharacter(int id)
        {
            // 더미 캐릭터 세팅
            for(int i = 0; i < _characterPortraits.Length; i++)
                _characterPortraits[i].SetActive(i + 1 == id);
 
            switch (id)
            {
                case 1:
                    _characterNameText.text = "Ashe"; 
                    break;
                case 2:
                    _characterNameText.text = "Murdok";
                    break;
                default:
                    _characterNameText.text = "Acon";
                    break;
            }
        }

        public void OnTouchSelect()
        {
            onEventSelectCharacter?.Invoke(_characterId);
        }
    }
}
