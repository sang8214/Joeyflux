using UnityEngine;

namespace Flux.UI
{
    public class UIView : MonoBehaviour
    {
        public System.Action onEventClose = null;
        private bool _initialized = false;

        private void Awake()
        {
            OnCreate();
        }

        // 첫 UI 오브젝트 생성시 호출됨
        void OnCreate()
        {
            if (_initialized)
                return;

            _initialized = true;
        }

        // 활성화 되기 전 전처리
        public virtual void OnPreEnable()
        {
        }
        // Navigator에 의해 갱신 필요시 호출됨
        public virtual void OnRefresh()
        {
        }
        // 창 비활성화 시 종료 이벤트
        public virtual void OnClose()
        {
            onEventClose?.Invoke();
        }
        // 백버튼(뒤로가기 처리)
        public virtual void OnBackButton()
        {
            UINavigator.Instance.OnProcessBackButton();
        }
        // 홈버튼
        public virtual void OnHomeButton()
        {
            UINavigator.Instance.OnProcessHomeButton();
        }
    }
}
