using Shops.Services;
using Spectre.Console;

namespace Shops.UI
{
    public class ShopsUIConfirmField : BaseShopsUIInputField<bool>
    {
        private readonly string _helpText;
        private bool _result;
        public ShopsUIConfirmField(IShopUIManager uIManager, string helpText)
            : base(uIManager)
        {
            _helpText = helpText;
        }

        public override void Show()
        {
            _result = AnsiConsole.Console.Confirm(_helpText);
        }

        public override bool GetResult()
        {
            return _result;
        }
    }
}
