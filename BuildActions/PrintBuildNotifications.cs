using System;
using System.Linq;
using SuperUnityBuild.BuildTool;
using UnityEngine;

namespace YogurtTheHorse.Unity.BuildActions
{
    public class PrintBuildNotifications : BuildAction, IPostBuildAction
    {
        public override void Execute()
        {
            var notifications = BuildNotificationList.instance.notifications
                .Concat(BuildNotificationList.instance.warnings)
                .Concat(BuildNotificationList.instance.errors);

            foreach (var notification in notifications)
            {
                Debug.Log($"[{notification.cat}] {notification.title}: {notification.details}");
            }
        }
    }
}