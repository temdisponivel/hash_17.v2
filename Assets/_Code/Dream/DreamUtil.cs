using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HASH
{
    public static class DreamUtil
    {
        #region Dream One

        public static void ExecuteDreamOne()
        {
            LoopUtil.RunCoroutine(InternalExecuteDreamOne());
        }

        private static IEnumerator InternalExecuteDreamOne()
        {
            PrepareForDream();

            var dreamOne = DataHolder.DreamReferences.DreamOne;
            var sceneLoad = SceneManager.LoadSceneAsync(dreamOne.Scene, LoadSceneMode.Additive);

            yield return AnnouncementUtil.RunAnnouncement(dreamOne.DreamAnnouncement);
            yield return sceneLoad;

            AnnouncementUtil.HideAnnouncement();

            var dreamOneComponent = GameObject.FindObjectOfType<DreamOneReferences>();
            DebugUtil.Assert(dreamOneComponent == null, "No dream one component on dream one scene!");

            DreamOneController.Run(dreamOneComponent, EndDreamOne);
        }

        private static void EndDreamOne()
        {
            SceneManager.UnloadSceneAsync(DataHolder.DreamReferences.DreamOne.Scene);
            EndDream();
        }

        #endregion

        #region Utility

        private static void PrepareForDream()
        {
            TerminalUtil.HideTerminal();
            WindowUtil.HideAllWindows();
        }

        private static void EndDream()
        {
            TerminalUtil.ShowTerminal();
            WindowUtil.ShowAllWindows();
        }

        #endregion
    }
}