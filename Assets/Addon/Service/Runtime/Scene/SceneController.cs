using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Services
{
    public class SceneController : Service
    {
        [Other]
        private EventSystem eventSystem;

        private SceneControllerCore core;

        /// <summary>
        /// �Ƿ��첽���س���
        /// </summary>
        [SerializeField]
        private bool async = true;
        /// <summary>
        /// �Ƿ���Ҫȷ��
        /// </summary>
        [SerializeField]
        private bool needConfirm = true;

        public static bool InGame => SceneControllerUtility.SceneIndex == 2;

        /// <summary>
        /// ��ʼ�첽���س���ʱ�������첽��������������ļ��ؽ��ȣ�ʹ��EventSystem�е��¼�����
        /// </summary>
        public event UnityAction<AsyncOperation> AsyncLoadScene
        {
            add => core.AsyncLoadScene += value;
            remove => core.AsyncLoadScene -= value;
        }

        protected override void Awake()
        {
            base.Awake();
            core = new SceneControllerCore(this, BeforeLoadScene, AfterLoadScene);
        }

        public void LoadScene(string name, LoadSceneMode mode = LoadSceneMode.Single)
            => core.LoadScene(SceneControllerUtility.ToSceneIndex(name), mode, async, needConfirm);

        public void LoadScene(int index, LoadSceneMode mode = LoadSceneMode.Single)
            => core.LoadScene(index, mode, async, needConfirm);
        public void LoadNextScene(LoadSceneMode mode = LoadSceneMode.Single)
            => LoadScene(SceneControllerUtility.SceneIndex + 1, mode);

        public void Quit()
            => core.Quit();

        private void BeforeLoadScene(int index)
        {
            eventSystem.Invoke(EEvent.BeforeLoadScene, index);
        }

        private void AfterLoadScene(int index)
        {
            eventSystem.Invoke(EEvent.AfterLoadScene, index);
        }
    }
}