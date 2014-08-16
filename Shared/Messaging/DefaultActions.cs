﻿using System;
using System.Windows;

#if WP7 || WP8
using MyToolkit.Environment;
using MyToolkit.Paging;
#endif

#if WINRT
using MyToolkit.Paging;
using System.Threading.Tasks;
using MyToolkit.Resources;
using Windows.UI.Popups;
#endif

namespace MyToolkit.Messaging
{
    /// <summary>
    /// Provides default actions for some message lcasses to use with the <see cref="Messenger"/>. 
    /// </summary>
    public static class DefaultActions
    {
#if WINRT

        public static Action<GoBackMessage> GetGoBackMessageAction(MtFrame frame)
        {
            return m => frame.GoBackAsync();
        }

        public static Action<NavigateMessage> GetNavigateMessageAction(MtFrame frame)
        {
            return m => frame.NavigateAsync(m.Page, m.Parameter);
        }

        public static Action<TextMessage> GetTextMessageAction()
        {
            return message => GetTextMessageImplementation(message);
        }

        private static async Task GetTextMessageImplementation(TextMessage message)
        {
            if (message.Button == MessageButton.OK)
            {
                var msg = new MessageDialog(message.Text, message.Title);
                await msg.ShowAsync();
            }
            else
            {
                var msg = new MessageDialog(message.Text, message.Title);
                
                if (message.Button == MessageButton.OKCancel)
                {
                    msg.Commands.Add(new UICommand(Strings.ButtonOk));
                    msg.Commands.Add(new UICommand(Strings.ButtonCancel));
                }
                else if (message.Button == MessageButton.YesNoCancel || message.Button == MessageButton.YesNo)
                {
                    msg.Commands.Add(new UICommand(Strings.ButtonYes));
                    msg.Commands.Add(new UICommand(Strings.ButtonNo));
                    
                    if (message.Button == MessageButton.YesNoCancel)
                        msg.Commands.Add(new UICommand(Strings.ButtonCancel));
                }

                msg.DefaultCommandIndex = 0;
                msg.CancelCommandIndex = 1;

                var cmd = await msg.ShowAsync();

                var index = msg.Commands.IndexOf(cmd); 
                if (message.Button == MessageButton.OKCancel)
                    message.CallSuccessCallback(index == 0 ? MessageResult.OK : MessageResult.Cancel);
                else if (message.Button == MessageButton.YesNoCancel)
                    message.CallSuccessCallback(index == 0 ? MessageResult.Yes : 
                        (index == 1 ? MessageResult.No : MessageResult.Cancel));
                else if (message.Button == MessageButton.YesNo)
                    message.CallSuccessCallback(index == 0 ? MessageResult.Yes : MessageResult.No);
            }
        }

#else

        public static Action<TextMessage> GetTextMessageAction()
        {
            return message =>
            {
                var result = MessageBoxResult.OK;
                if (message.Button == MessageButton.OK)
                    result = MessageBox.Show(message.Text, message.Title, MessageBoxButton.OK);
                else if (message.Button == MessageButton.OKCancel)
                    result = MessageBox.Show(message.Text, message.Title, MessageBoxButton.OKCancel);
#if !WP8 && !WP7 && !SL5
                else if (message.Button == MessageButton.YesNo)
                    result = MessageBox.Show(message.Text, message.Title, MessageBoxButton.YesNo);
                else if (message.Button == MessageButton.YesNoCancel)
                    result = MessageBox.Show(message.Text, message.Title, MessageBoxButton.YesNoCancel);
#else
                else
                    throw new Exception(string.Format("The MessageButton type '{0}' is not available on this platform. ", message.Button));
#endif

                if (result == MessageBoxResult.Yes)
                    message.CallSuccessCallback(MessageResult.Yes);
                else if (result == MessageBoxResult.No)
                    message.CallSuccessCallback(MessageResult.No);
                else if (result == MessageBoxResult.OK)
                    message.CallSuccessCallback(MessageResult.OK);
                else if (result == MessageBoxResult.Cancel)
                    message.CallSuccessCallback(MessageResult.Cancel);
            };
        }

#endif

#if WP7 || WP8

        public static Action<GoBackMessage> GetGoBackMessageAction()
        {
            return message =>
            {
                var page = PhoneApplication.CurrentPage; 
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    try
                    {
                        page.NavigationService.GoBack();
                        if (message.Completed != null)
                            message.Completed(true);
                    }
                    catch
                    {
                        if (message.Completed != null)
                            message.Completed(false);
                    }
                });
            };
        }

#endif
    }
}
