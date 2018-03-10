﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace ScreenToGif.Util
{
    internal class HotKeyCollection : IDisposable
    {
        internal static readonly HotKeyCollection Default = new HotKeyCollection();

        
        private readonly List<HotKey> _hotKeys = new List<HotKey>();

        internal List<HotKey> HotKeys => _hotKeys;

        /// <summary>
        /// Registers the given keyboard shortcut with a given callback.
        /// </summary>
        /// <param name="modifier">The modifier of the keyboard command.</param>
        /// <param name="key">The key of the keyboard command.</param>
        /// <param name="windowsHandle">A handle to the window that will receive WM_HOTKEY messages generated by the hot key.</param>
        /// <param name="callback">The callback that will be invoked when the keyboard command is pressed.</param>
        /// <exception cref="InvalidOperationException">If the key is already in use.</exception>
        internal void RegisterHotKey(ModifierKeys modifier, Key key, IntPtr windowsHandle, Action callback)
        {
            if (key == Key.None)
                return;

            _hotKeys.Add(new HotKey(modifier, key, windowsHandle, callback));
        }

        /// <summary>
        /// Registers the given keyboard shortcut with a given callback.
        /// </summary>
        /// <param name="modifier">The modifier of the keyboard command.</param>
        /// <param name="key">The key of the keyboard command.</param>
        /// <param name="callback">The callback that will be invoked when the keyboard command is pressed.</param>
        /// <param name="unregisterFirst">Tries to unregister first, before trying to register the hotkey.</param>
        /// <exception cref="InvalidOperationException">If the key is already in use.</exception>
        internal void RegisterHotKey(ModifierKeys modifier, Key key, Action callback, bool unregisterFirst = false)
        {
            if (key == Key.None)
                return;

            _hotKeys.Add(new HotKey(modifier, key, callback, unregisterFirst));
        }

        internal void Remove(ModifierKeys modifier, Key key)
        {
            var hot = Default.HotKeys.FirstOrDefault(f => f.Key == key && f.Modifier == modifier);
            hot?.Dispose();

            if (hot != null)
                Default.HotKeys.Remove(hot);
        }

        public void Dispose()
        {
            foreach (var hotKey in HotKeys)
                hotKey.Dispose();
        }
    }
}