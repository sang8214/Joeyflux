# UI System
게임 내 UI요소 관리 및 트랜지션 구현 

## 🚩프로젝트 목적 및 내용
1. 코드 샘플용 포트폴리오
2. Window/Popup/Component 요소로 UI를 구분하고, 해당 단위의 템플릿을 시스템에 쉽게 연동할 수 있도록 구현
3. 게임 완성본이 아닌, 게임내 존재하는 UI요소들을 어떻게 관리하는지에 초점을 맞춤
4. 향후 프로토타입을 기준으로 게임 특성에 맞는 요소로 커스터 마이징 가능하도록 설계

## 🍳프로젝트 개발 환경
1. Engine : Unity 2023.2
2. IDE : JetBrain Rider 
3. Package Assets : UGUI, Addressable Assets, URP(Optional), TextMeshPro(Optional)
4. UI Resource : AssetStore Free Asset

## 😎프로젝트 담당자
1. 오상현(JoeyFlux) : 클라이언트 프로그래머

## UI요소 및 규약
1. Window(UIWindow) : UI 메인 레이아웃으로 동일한 여러개의 Window는 노출 불가
2. Popup(UIPopup) : Window 레이어 보다 위에 상주하며, 동일한 여러개의 Popup은 Stack형식으로 노출 가능
3. Component(UIComponent) : Window/Popup 내부의 구성요소가 될 수 있으며, 동일한 여러개의 Component가 어떠한 형태로든 노출 가능

## 관리 클래스 정의
1. UINavigator
   - 순차적인 노출/숨김(or 반환)을 위한 Window/Popup용 Stack 관리
   - Back/Home 버튼에 대한 이벤트를 처리
   - UIManager에 해당 요소에 대한 노출/숨김을 처리하도록 요청, 이후 Stack 조작
2. UIMananger
   - Window/Popup/Component 로드 및 반환 처리
   - Window로드의 경우, BundleManager로 Addressable Asset System을 이용하여 로드
   - Popup/Component로드의 경우, ObjectPoolManager에서 해당요소 존재하는지 검사 후 없을 경우, BundleManager로 Addressable Asset System을 이용하여 로드
   - Popup/Component반환의 경우, ObjectPoolManager로 반환
3. BundleManager
   - Addressable Asset System을 통한 Load/UnLoad를 진행
   - Sync Load : UI및 Effect 로드 방식, 바로 로드되는 방식으로 이 프로젝트에서 사용되고 있음
   - Async Load : 3D 모델이나, Scene 트랜지션간 리소스 로딩 용도로 사용, 이 프로젝트에서는 사용되고 있지 않지만, 구현은 해둠
     > Async 방식이기 때문에 Load Complete에 대한 콜백을 캐싱하며, BundleLoadScheduler를 통해 로딩 대기 및 중복 방지 관리
4. ObjectPoolManager
   - Popup/Component의 경우, 동일한 Instance가 여러개 생성될 수 있으므로, 잦은 리소스 로딩이 아닌 재활용을 위해 사용
   - 사용되지 않는 Pool Object가 있을 경우 반환, 없을 경우 캐싱된 오브젝트를 복제하여 반환, 캐싱된 오브젝트가 없을 경우, BundleManager를 통해 해당 리소스 로딩 후 오브젝트 캐싱

## 더미 클래스 정의
1. UI System 시뮬레이션을 위한 더미용 Window/Popup/Component
1. Window
   - UILobbyTopWindow : 상단에 떠 있는 Window 이름, Back/Home 버튼, 옵션 버튼을 관리하며, 현재 Window상태에 따라 노출여부 판단
   - UILobbyWindow : LobbyScene 첫 실행시 바로 노출되는 창, InventoryWindow로 이동할 수 있는 버튼 존재
   - UIInventoryWindow : 인벤토리 더미 창, CharacterWindow로 이동할 수 있는 버튼 존재
   - UICharacterWindow : 3개의 UICharacterComponent가 존재하며, Component 선택시 UICharacterPopup 노출
2. Popup
   - UICharacterPopup : 캐릭터에 대한 정보를 노출, Status버튼 누를 시 UICharacterStatusPopup 노출, X버튼 존재
   - UICharacterStatusPopup : 캐릭터의 스테이터스 정보를 노출, X버튼 존재
3. Component
   - UICharacterComponent : UICharacterWindow와 UICharacterPopup에서 사용되고 있는 Component
     
## 플로우 다이어 그램


