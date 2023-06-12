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
        /// 是否异步加载场景
        /// </summary>
        [SerializeField]
        private bool async = true;
        /// <summary>
        /// 是否需要确认
        /// </summary>
        [SerializeField]
        private bool needConfirm = true;

        public static bool InGame => SceneControllerUtility.SceneIndex == 2;

        /// <summary>
        /// 开始异步加载场景时，发送异步操作，如果不关心加载进度，使用EventSystem中的事件即可
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