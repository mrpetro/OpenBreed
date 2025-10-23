using OpenBreed.Rendering.Abstractions.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OpenBreed.Editor.UI.Wpf.Extensions
{
    public static class KeyExtensions
    {
        #region Public Methods

        public static Keys ToKeysEnum(this Key key)
        {
            switch (key)
            {
                case Key.None:
                    return Keys.Unknown;

                case Key.Cancel:
                    return Keys.Unknown;

                case Key.Back:
                    return Keys.Backspace;

                case Key.Tab:
                    return Keys.Tab;

                case Key.LineFeed:
                    return Keys.Unknown;

                case Key.Clear:
                    return Keys.Unknown;

                case Key.Enter:
                    return Keys.Enter;

                case Key.Pause:
                    return Keys.Pause;

                case Key.Capital:
                    return Keys.CapsLock;

                case Key.HangulMode:
                    return Keys.Unknown;

                case Key.JunjaMode:
                    return Keys.Unknown;

                case Key.FinalMode:
                    return Keys.Unknown;

                case Key.HanjaMode:
                    return Keys.Unknown;

                case Key.Escape:
                    return Keys.Escape;

                case Key.ImeConvert:
                    return Keys.Unknown;

                case Key.ImeNonConvert:
                    return Keys.Unknown;

                case Key.ImeAccept:
                    return Keys.Unknown;

                case Key.ImeModeChange:
                    return Keys.Unknown;

                case Key.Space:
                    return Keys.Space;

                case Key.PageUp:
                    return Keys.PageUp;

                case Key.Next:
                    return Keys.PageDown;

                case Key.End:
                    return Keys.End;

                case Key.Home:
                    return Keys.Home;

                case Key.Left:
                    return Keys.Left;

                case Key.Up:
                    return Keys.Up;

                case Key.Right:
                    return Keys.Right;

                case Key.Down:
                    return Keys.Down;

                case Key.Select:
                    return Keys.Unknown;

                case Key.Print:
                    return Keys.Unknown;

                case Key.Execute:
                    return Keys.Unknown;

                case Key.PrintScreen:
                    return Keys.PrintScreen;

                case Key.Insert:
                    return Keys.Insert;

                case Key.Delete:
                    return Keys.Delete;

                case Key.Help:
                    return Keys.Unknown;

                case Key.D0:
                    return Keys.D0;

                case Key.D1:
                    return Keys.D1;

                case Key.D2:
                    return Keys.D2;

                case Key.D3:
                    return Keys.D3;

                case Key.D4:
                    return Keys.D4;

                case Key.D5:
                    return Keys.D5;

                case Key.D6:
                    return Keys.D6;

                case Key.D7:
                    return Keys.D7;

                case Key.D8:
                    return Keys.D8;

                case Key.D9:
                    return Keys.D9;

                case Key.A:
                    return Keys.A;

                case Key.B:
                    return Keys.B;

                case Key.C:
                    return Keys.C;

                case Key.D:
                    return Keys.D;

                case Key.E:
                    return Keys.E;

                case Key.F:
                    return Keys.F;

                case Key.G:
                    return Keys.G;

                case Key.H:
                    return Keys.H;

                case Key.I:
                    return Keys.I;

                case Key.J:
                    return Keys.J;

                case Key.K:
                    return Keys.K;

                case Key.L:
                    return Keys.L;

                case Key.M:
                    return Keys.M;

                case Key.N:
                    return Keys.N;

                case Key.O:
                    return Keys.O;

                case Key.P:
                    return Keys.P;

                case Key.Q:
                    return Keys.Q;

                case Key.R:
                    return Keys.R;

                case Key.S:
                    return Keys.S;

                case Key.T:
                    return Keys.T;

                case Key.U:
                    return Keys.U;

                case Key.V:
                    return Keys.V;

                case Key.W:
                    return Keys.W;

                case Key.X:
                    return Keys.X;

                case Key.Y:
                    return Keys.Y;

                case Key.Z:
                    return Keys.Z;

                case Key.LWin:
                    return Keys.LeftSuper;

                case Key.RWin:
                    return Keys.RightSuper;

                case Key.Apps:
                    return Keys.Unknown;

                case Key.Sleep:
                    return Keys.Unknown;

                case Key.NumPad0:
                    return Keys.KeyPad0;

                case Key.NumPad1:
                    return Keys.KeyPad1;

                case Key.NumPad2:
                    return Keys.KeyPad2;

                case Key.NumPad3:
                    return Keys.KeyPad3;

                case Key.NumPad4:
                    return Keys.KeyPad4;

                case Key.NumPad5:
                    return Keys.KeyPad5;

                case Key.NumPad6:
                    return Keys.KeyPad6;

                case Key.NumPad7:
                    return Keys.KeyPad7;

                case Key.NumPad8:
                    return Keys.KeyPad8;

                case Key.NumPad9:
                    return Keys.KeyPad9;

                case Key.Multiply:
                    return Keys.KeyPadMultiply; //???
                case Key.Add:
                    return Keys.KeyPadAdd; //???
                case Key.Separator:
                    return Keys.Unknown; //???
                case Key.Subtract:
                    return Keys.KeyPadSubtract; //???
                case Key.Decimal:
                    return Keys.KeyPadDecimal; //???
                case Key.Divide:
                    return Keys.KeyPadDivide; //???
                case Key.F1:
                    return Keys.F1;

                case Key.F2:
                    return Keys.F2;

                case Key.F3:
                    return Keys.F3;

                case Key.F4:
                    return Keys.F4;

                case Key.F5:
                    return Keys.F5;

                case Key.F6:
                    return Keys.F6;

                case Key.F7:
                    return Keys.F7;

                case Key.F8:
                    return Keys.F8;

                case Key.F9:
                    return Keys.F9;

                case Key.F10:
                    return Keys.F10;

                case Key.F11:
                    return Keys.F11;

                case Key.F12:
                    return Keys.F12;

                case Key.F13:
                    return Keys.F13;

                case Key.F14:
                    return Keys.F14;

                case Key.F15:
                    return Keys.F15;

                case Key.F16:
                    return Keys.F16;

                case Key.F17:
                    return Keys.F17;

                case Key.F18:
                    return Keys.F18;

                case Key.F19:
                    return Keys.F19;

                case Key.F20:
                    return Keys.F20;

                case Key.F21:
                    return Keys.F21;

                case Key.F22:
                    return Keys.F22;

                case Key.F23:
                    return Keys.F23;

                case Key.F24:
                    return Keys.F24;

                case Key.NumLock:
                    return Keys.NumLock;

                case Key.Scroll:
                    return Keys.ScrollLock;

                case Key.LeftShift:
                    return Keys.LeftShift;

                case Key.RightShift:
                    return Keys.RightShift;

                case Key.LeftCtrl:
                    return Keys.LeftControl;

                case Key.RightCtrl:
                    return Keys.RightControl;

                case Key.LeftAlt:
                    return Keys.LeftAlt;

                case Key.RightAlt:
                    return Keys.RightAlt;

                case Key.BrowserBack:
                    return Keys.Unknown;

                case Key.BrowserForward:
                    return Keys.Unknown;

                case Key.BrowserRefresh:
                    return Keys.Unknown;

                case Key.BrowserStop:
                    return Keys.Unknown;

                case Key.BrowserSearch:
                    return Keys.Unknown;

                case Key.BrowserFavorites:
                    return Keys.Unknown;

                case Key.BrowserHome:
                    return Keys.Unknown;

                case Key.VolumeMute:
                    return Keys.Unknown;

                case Key.VolumeDown:
                    return Keys.Unknown;

                case Key.VolumeUp:
                    return Keys.Unknown;

                case Key.MediaNextTrack:
                    return Keys.Unknown;

                case Key.MediaPreviousTrack:
                    return Keys.Unknown;

                case Key.MediaStop:
                    return Keys.Unknown;

                case Key.MediaPlayPause:
                    return Keys.Unknown;

                case Key.LaunchMail:
                    return Keys.Unknown;

                case Key.SelectMedia:
                    return Keys.Unknown;

                case Key.LaunchApplication1:
                    return Keys.Unknown;

                case Key.LaunchApplication2:
                    return Keys.Unknown;

                case Key.Oem1:
                    return Keys.Unknown;

                case Key.OemPlus:
                    return Keys.Unknown;

                case Key.OemComma:
                    return Keys.Unknown;

                case Key.OemMinus:
                    return Keys.Unknown;

                case Key.OemPeriod:
                    return Keys.Unknown;

                case Key.Oem2:
                    return Keys.Unknown;

                case Key.Oem3:
                    return Keys.Unknown;

                case Key.AbntC1:
                    return Keys.Unknown;

                case Key.AbntC2:
                    return Keys.Unknown;

                case Key.Oem4:
                    return Keys.Unknown;

                case Key.Oem5:
                    return Keys.Unknown;

                case Key.Oem6:
                    return Keys.Unknown;

                case Key.Oem7:
                    return Keys.Unknown;

                case Key.Oem8:
                    return Keys.Unknown;

                case Key.Oem102:
                    return Keys.Unknown;

                case Key.ImeProcessed:
                    return Keys.Unknown;

                case Key.System:
                    return Keys.Unknown;

                case Key.DbeAlphanumeric:
                    return Keys.Unknown;

                case Key.DbeKatakana:
                    return Keys.Unknown;

                case Key.DbeHiragana:
                    return Keys.Unknown;

                case Key.DbeSbcsChar:
                    return Keys.Unknown;

                case Key.DbeDbcsChar:
                    return Keys.Unknown;

                case Key.DbeRoman:
                    return Keys.Unknown;

                case Key.Attn:
                    return Keys.Unknown;

                case Key.CrSel:
                    return Keys.Unknown;

                case Key.DbeEnterImeConfigureMode:
                    return Keys.Unknown;

                case Key.DbeFlushString:
                    return Keys.Unknown;

                case Key.DbeCodeInput:
                    return Keys.Unknown;

                case Key.DbeNoCodeInput:
                    return Keys.Unknown;

                case Key.DbeDetermineString:
                    return Keys.Unknown;

                case Key.DbeEnterDialogConversionMode:
                    return Keys.Unknown;

                case Key.OemClear:
                    return Keys.Unknown;

                case Key.DeadCharProcessed:
                    return Keys.Unknown;

                default:
                    return Keys.Unknown;
            }
        }

        #endregion Public Methods
    }
}