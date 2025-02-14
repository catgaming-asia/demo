using CoreGame;

namespace Game.Command
{
    public class StartGameCommand
    {
        GameManager _gameManager => Locator<GameManager>.Instance;
       
        public void Execute()
        {
        }
    }
}