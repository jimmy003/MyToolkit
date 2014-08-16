using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MyToolkit.Paging.Handlers
{
    /// <summary>Registers for the hardware back key button on Windows Phone and calls the registered methods when the event occurs. </summary>
    public class BackKeyPressedHandler
    {
        private static readonly Type HardwareButtonsType;
        private static readonly EventInfo BackPressedEvent;

        private readonly List<Tuple<MtPage, Func<object, bool>>> _handlers;
        private bool _isRegistered = false;
        private Delegate _backPressedDelegate;

        static BackKeyPressedHandler()
        {
            BackPressedEvent = HardwareButtonsType.GetRuntimeEvent("BackPressed");
            HardwareButtonsType = Type.GetType(
                "Windows.Phone.UI.Input.HardwareButtons, " +
                "Windows, Version=255.255.255.255, Culture=neutral, " +
                "PublicKeyToken=null, ContentType=WindowsRuntime");
        }

        public BackKeyPressedHandler()
        {
            _handlers = new List<Tuple<MtPage, Func<object, bool>>>();
        }

        /// <summary>Adds a back key handler for a given page. </summary>
        /// <param name="page">The page. </param>
        /// <param name="handler">The handler. </param>
        public void Add(MtPage page, Func<object, bool> handler)
        {
            if (!_isRegistered)
            {
                Action<object, object> callback = OnBackKeyPressed;

                var callbackMethodInfo = callback.GetMethodInfo();
                _backPressedDelegate = callbackMethodInfo.CreateDelegate(BackPressedEvent.EventHandlerType, this);

                BackPressedEvent.AddMethod.Invoke(null, new object[] { _backPressedDelegate });
                _isRegistered = true;
            }

            _handlers.Insert(0, new Tuple<MtPage, Func<object, bool>>(page, handler));
        }

        /// <summary>Removes a back key pressed handler for a given page. </summary>
        /// <param name="page">The page. </param>
        public void Remove(MtPage page)
        {
            _handlers.Remove(_handlers.Single(h => h.Item1 == page));

            if (_handlers.Count == 0)
                BackPressedEvent.RemoveMethod.Invoke(null, new object[] { _backPressedDelegate });
        }

        private void OnBackKeyPressed(object sender, dynamic args)
        {
            if (args.Handled)
                return;

            foreach (var item in _handlers)
            {
                args.Handled = item.Item2(sender);
                if (args.Handled)
                    return;
            }
        }
    }
}