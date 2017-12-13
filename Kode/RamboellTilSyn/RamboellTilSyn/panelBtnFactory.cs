﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;

using Foundation;
using UIKit;

namespace Ramboell.iOS
{
    public class PanelBtnFactory
    {
        public enum BtnType
        {
            PrePage,
            NxtPage,
            AddCircle,
            AddCheckMark,
            ShowList,
            AddMinus
        }
        public static UIButton GetButtonForType(BtnType param)
        {
           var btn = UIButton.FromType(UIButtonType.Plain);
            btn.BackgroundColor = UIColor.LightTextColor;

            switch (param)
            {
                case BtnType.PrePage:
                    btn.SetImage(UIImage.FromBundle("back"),UIControlState.Normal);
                    break;
                case BtnType.NxtPage:
                    btn.SetImage(UIImage.FromBundle("forward"), UIControlState.Normal);
                    break;
                case BtnType.AddCircle:
                    btn.SetImage(UIImage.FromBundle("circle"), UIControlState.Normal);
                    break;
                case BtnType.AddCheckMark:
                    btn.SetImage(UIImage.FromBundle("checkmark"), UIControlState.Normal);
                    break;
                case BtnType.ShowList:
                    btn.SetImage(UIImage.FromBundle("list"), UIControlState.Normal);
                    break;
                case BtnType.AddMinus:
                    btn.SetImage(UIImage.FromBundle("minus"), UIControlState.Normal);
                    break;
            }
            return btn; 
        }
    }
}