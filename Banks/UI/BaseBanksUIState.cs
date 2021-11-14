namespace Banks.UI
{
    public abstract class BaseBanksUIState
    {
        protected BaseBanksUIState()
        {
        }

        protected BanksUIContext Context { get; private set; }

        public void SetContext(BanksUIContext context)
        {
            Context = context;
        }

        public abstract void Show();
    }
}
