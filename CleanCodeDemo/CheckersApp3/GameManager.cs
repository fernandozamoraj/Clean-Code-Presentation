namespace CheckersApp3
{
    public class GameManager
    {
        private CheckerGame _game;
        private static GameManager _gameManager;

        public static GameManager CreateTheGame(CheckerGame game)
        {
            if(_gameManager == null)
            {
                _gameManager = new GameManager(game);
            }

            return _gameManager;
        }

        public static GameManager TheGameManager
        {
            get { return _gameManager; }
        }

        private GameManager(CheckerGame game)
        {
            _game = game;
        }

        public void Draw()
        {
            _game.Draw();
        }
    }
}