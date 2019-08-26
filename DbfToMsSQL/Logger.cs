using System;
using System.Windows.Forms;

namespace BdfToMsSQL
{
    public static class Logger
    {
        private static Form _form;
        private static ListBox _listBox;

        public static Form Form
        {
            set
            {
                if (_form == null)
                {
                    _form = value;
                }
            }
        }

        public static ListBox ListBox
        {
            set
            {
                if (_listBox == null)
                {
                    _listBox = value;
                }
            }
        }

        public static void ScrollDown()
        {
            _form.Invoke(new MethodInvoker(() =>
            {
                int visibleItems = _listBox.ClientSize.Height / _listBox.ItemHeight;
                _listBox.TopIndex = Math.Max(_listBox.Items.Count - visibleItems + 1, 0);
            }));
        }

        public static void Clear()
        {
            _form.Invoke(new MethodInvoker(() => { _listBox.Items.Clear(); }));
        }

        public static void WriteMessage(string message)
        {
            _form.Invoke(new MethodInvoker(() => { _listBox.Items.Add(message); }));
            ScrollDown();
        }

        public static void WriteError(Exception exc)
        {
            _form.Invoke(new MethodInvoker(() => { _listBox.Items.Add($"Error: {exc.Message }\r\n {exc.InnerException?.ToString() ?? ""}"); }));
            ScrollDown();
        }
    }
}