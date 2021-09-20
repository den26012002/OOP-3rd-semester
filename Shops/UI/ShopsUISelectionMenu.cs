using System.Collections.Generic;
using Shops.Services;
using Spectre.Console;

namespace Shops.UI
{
    public class ShopsUISelectionMenu<T> : BaseShopsUIMenu<T>
    {
        private List<MenuAction<T>> _preSelectionActions;
        private List<MenuAction<T>> _selectionActions;
        private List<MenuAction<T>> _postSelectionActions;

        public ShopsUISelectionMenu(IShopUIManager manager, string title = "")
            : base(manager)
        {
            Title = title;
            _preSelectionActions = new List<MenuAction<T>>();
            _selectionActions = new List<MenuAction<T>>();
            _postSelectionActions = new List<MenuAction<T>>();
        }

        public string Title { get; private set; }

        public override void Show()
        {
            foreach (MenuAction<T> preAction in _preSelectionActions)
            {
                preAction.Invoke();
            }

            if (_selectionActions.Count == 0)
            {
                AnsiConsole.Prompt(new SelectionPrompt<MenuAction<string>>()
                    .Title("Произошла непредвиденная ошибка")
                    .AddChoices(new MenuAction<string>("Вернуться", () => { Manager.SetActiveElement(ParentElement); }))).Invoke();
                return;
            }

            MenuAction<T> action = AnsiConsole.Prompt(new SelectionPrompt<MenuAction<T>>()
                .Title(Title)
                .AddChoices(_selectionActions.ToArray()));
            action.Invoke();

            foreach (MenuAction<T> postAction in _postSelectionActions)
            {
                postAction.Invoke();
            }
        }

        public void ChangeTitle(string newTitle)
        {
            Title = newTitle;
        }

        public override void AddPreSelectionAction(MenuAction<T> action)
        {
            _preSelectionActions.Add(action);
        }

        public override void AddSelectionAction(MenuAction<T> action)
        {
            _selectionActions.Add(action);
        }

        public override void AddPostSelectionAction(MenuAction<T> action)
        {
            _postSelectionActions.Add(action);
        }

        public override void ClearActions()
        {
            _preSelectionActions.Clear();
            _selectionActions.Clear();
            _postSelectionActions.Clear();
        }
    }
}
