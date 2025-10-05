namespace QRK3y.UI.Components
{
    public class MenuItem
    {
        public string DisplayText { get; set; } = string.Empty;
        public string ActionKey { get; set; } = string.Empty;

        public MenuItem(string displayText, string actionKey)
        {
            DisplayText = displayText;
            ActionKey = actionKey;
        }
    }
}