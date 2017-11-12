using System;
using System.Collections;
using HASH;
using UnityEngine;

namespace HASH
{
    public static class AnnouncementUtil
    {
        public static Coroutine RunAnnouncement(Announcement announcement)
        {
            DataHolder.GUIReferences.AnnouncementPanel.gameObject.SetActive(true);

            return LoopUtil.RunCoroutine(InternalAnnouncementRunner(announcement));
        }

        public static void HideAnnouncement()
        {
            DataHolder.GUIReferences.AnnouncementPanel.gameObject.SetActive(false);
            DataHolder.GUIReferences.AnnoucementLabel.text = string.Empty;
        }

        private static IEnumerator InternalAnnouncementRunner(Announcement announcement)
        {
            for (int i = 0; i < announcement.Nodes.Length; i++)
            {
                var node = announcement.Nodes[i];
                DataHolder.GUIReferences.AnnoucementLabel.text = node.Text;
                yield return new WaitForSecondsRealtime(node.TimeOnScreen);
            }
        }
    }
}