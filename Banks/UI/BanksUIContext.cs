using Banks.Entities;

namespace Banks.UI
{
    public class BanksUIContext
    {
        private CentralBank _centralBank;
        private BaseBanksUIState _state;

        public BanksUIContext(CentralBank centralBank)
        {
            _centralBank = centralBank;
            _state = new BanksUIMainMenuState(centralBank);
            TransitionTo(_state);
        }

        public void TransitionTo(BaseBanksUIState state)
        {
            _state = state;
            _state.SetContext(this);
        }

        public void Show()
        {
            while (true)
            {
                _state.Show();
            }
        }
    }
}
