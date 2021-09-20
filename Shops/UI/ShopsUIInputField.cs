using Shops.Services;
using Spectre.Console;

namespace Shops.UI
{
    public class ShopsUIInputField<T> : BaseShopsUIInputField<T>
    {
        private string _helpText;
        private T result;
        public ShopsUIInputField(IShopUIManager manager, string helpText = "")
            : base(manager)
        {
            _helpText = helpText;
        }

        public override void Show()
        {
            result = AnsiConsole.Prompt(new TextPrompt<T>(_helpText)
                .ValidationErrorMessage("[red]Ошибка ввода[/]"));
        }

        public override T GetResult()
        {
            return result;
        }
    }
}
