using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSLovers2
{
    public class AppState
    {
        public string ToastMessage { get; private set; }
        public string ToastType { get; private set; }

        public event Action OnChange;

        public Dictionary<string, string> CssColors = new Dictionary<string, string>() {
            { "orange", "#fec771" },
            { "red","#eb7070"},
            { "yellow","#e6e56c" },
            { "green","#64e291" }
        };

        public void SetToast(string message, string type)
        {
            ToastMessage = message;
            ToastType = type;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
