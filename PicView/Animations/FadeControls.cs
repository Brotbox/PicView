﻿using PicView.UILogic.Loading;
using PicView.UILogic.TransformImage;
using System;
using System.Timers;
using static PicView.UILogic.UC;

namespace PicView.UILogic.Animations
{
    internal static class FadeControls
    {
        /// <summary>
        /// Timer used to hide interface and/or scrollbar
        /// </summary>
        internal static Timer ActivityTimer { get; set; }

        /// <summary>
        /// Hides/shows interface elements with a fade animation
        /// </summary>
        /// <param name="show"></param>
        internal static async void FadeControlsAsync(bool show, double time = .5)
        {
            if (Properties.Settings.Default.ShowInterface
                || Properties.Settings.Default.Fullscreen
                || GetClickArrowRight == null
                || GetClickArrowLeft == null
                || Getx2 == null
                || GetGalleryShortcut == null
                || Scroll.IsAutoScrolling)
            {
                return;
            }

            if (GetCropppingTool != null)
            {
                if (GetCropppingTool.IsVisible)
                {
                    return;
                }
            }

            await LoadWindows.GetMainWindow.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Input, (Action)(() =>
            {
                if (Properties.Settings.Default.ScrollEnabled && LoadWindows.GetMainWindow.Scroller.ScrollableHeight > 0)
                {
                    ScrollbarFade(show);
                }

                // Don't run, if already being animated || prevent lag
                if (Getx2.Opacity > 0 && Getx2.Opacity < 1)
                {
                    return;
                }

                if (GetClickArrowRight.IsMouseOver || GetClickArrowLeft.IsMouseOver || GetMinus.IsMouseOver
                    || GetRestorebutton.IsMouseOver || Getx2.IsMouseOver)
                {
                    return;
                }

                TimeSpan timespan = TimeSpan.FromSeconds(time);

                if (!show)
                {
                    AnimationHelper.Fade(GetClickArrowLeft, 0, timespan);
                    AnimationHelper.Fade(GetClickArrowRight, 0, timespan);
                    AnimationHelper.Fade(GetGalleryShortcut, 0, timespan);
                    AnimationHelper.Fade(Getx2, 0, timespan);
                    AnimationHelper.Fade(GetMinus, 0, timespan);
                    AnimationHelper.Fade(GetRestorebutton, 0, timespan);
                    return;
                }
                else if (Scroll.IsAutoScrolling)
                {
                    GetClickArrowLeft.Opacity =
                    GetClickArrowRight.Opacity =
                    GetGalleryShortcut.Opacity =
                    Getx2.Opacity =
                    GetMinus.Opacity = 0;
                    return;
                }

                AnimationHelper.Fade(GetClickArrowLeft, 1, timespan);
                AnimationHelper.Fade(GetClickArrowRight, 1, timespan);
                AnimationHelper.Fade(GetGalleryShortcut, 1, timespan);
                AnimationHelper.Fade(Getx2, 1, timespan);
                AnimationHelper.Fade(GetMinus, 1, timespan);
                AnimationHelper.Fade(GetRestorebutton, 1, timespan);
            }));
        }

        /// <summary>
        /// Find scrollbar and start fade animation
        /// </summary>
        /// <param name="show"></param>
        internal static void ScrollbarFade(bool show)
        {
            var s = LoadWindows.GetMainWindow.Scroller.Template.FindName("PART_VerticalScrollBar", LoadWindows.GetMainWindow.Scroller) as System.Windows.Controls.Primitives.ScrollBar;

            if (show)
            {
                AnimationHelper.Fade(s, 1, TimeSpan.FromSeconds(.7));
            }
            else
            {
                AnimationHelper.Fade(s, 0, TimeSpan.FromSeconds(1));
            }
        }
    }
}