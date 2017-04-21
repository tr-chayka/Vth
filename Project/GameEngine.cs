using System;
using System.Collections.Generic;

namespace Project
{
    // result
    // 0 - movement
    // 10 - draw
    // 20 - user win
    // 30 - bot win
    
    class Game
    {
        const int Player = 1;
        const int Bot = 2;

        Random random = new Random();

        int[] Field = new int[9];
        int result = 0;
        int last_mov = -1;

        int rand()
        {
            return random.Next() % 9;
        }

        public int Result
        {
            get
            {
                return result;
            }
        }

        public int LastMov
        {
            get
            {
                return last_mov;
            }
        }

        public Game()
        {
        }

        bool Draw()
        {
            for (int i = 0; i < 9; i++)
                if (Field[i] == 0)
                    return false;

            return true;
        }

        bool Win(int player)
        {
            return
                (Field[0] == player && Field[1] == player && Field[2] == player) ||
                (Field[3] == player && Field[4] == player && Field[5] == player) ||
                (Field[6] == player && Field[7] == player && Field[8] == player) ||

                (Field[0] == player && Field[3] == player && Field[6] == player) ||
                (Field[1] == player && Field[4] == player && Field[7] == player) ||
                (Field[2] == player && Field[5] == player && Field[8] == player) ||

                (Field[0] == player && Field[4] == player && Field[8] == player) ||
                (Field[2] == player && Field[4] == player && Field[6] == player);
        }

        public void Movemet(int pos)
        {
            last_mov = -1;

            if (pos >= 0)
                Field[pos] = Player;

            if (Win(Player))
            {
                result = 20;
                return;
            }

            if (Draw())
            {
                result = 10;
                return;
            }

            // try to win
            for (int i = 0; i < 9; i++)
                if (Field[i] == 0)
                {
                    Field[i] = Bot;

                    if (Win(Bot))
                    {
                        last_mov = i;
                        result = 30;
                        return;
                    }

                    Field[i] = 0;
                }

            // ...
            for (int i = 0; i < 9; i++)
                if (Field[i] == 0)
                {
                    Field[i] = Player;

                    if (Win(Player))
                    {
                        Field[i] = Bot;

                        last_mov = i;
                        result = Draw() ? 10 : 0;
                        return;
                    }

                    Field[i] = 0;
                }

            // random ...
            int free_count = 0;
            for (int i = 0; i < 9; i++)
                if (Field[i] == 0)
                    free_count++;

            int n = rand() % free_count;

            for ( int i = 0; i < 9; i++)
                if (Field[i] == 0)
                {
                    if (n > 0)
                        n--;
                    else
                    {
                        Field[i] = Bot;

                        last_mov = i;
                        result = Draw() ? 10 : 0;
                        return;
                    }
                }
        }
    }

    class GameObject
    {
        public Game Game;
        public GameLog Log;
    }

    public class GameEngine
    {
        DatabaseModel Database = new DatabaseModel();
        List<GameObject> Games = new List<GameObject>();

        public int NewGame()
        {
            GameObject NewGame = new GameObject { Game = new Game(), Log = new GameLog() };

            NewGame.Log.BeginTime = DateTime.Now.ToString();
            Games.Add(NewGame);

            return Games.Count - 1;
        }

        public int Step(int index, int position)
        {
            if (index < 0 || index > Games.Count)
                return 1000;

            GameObject CurrentGame = Games[index];

            if (position >= 0)
                CurrentGame.Log.Log.Add(new Movement { User = true, Position = position });
            CurrentGame.Game.Movemet(position);

            if (CurrentGame.Game.LastMov >= 0)
                CurrentGame.Log.Log.Add(new Movement { User = false, Position = CurrentGame.Game.LastMov });

            int result = CurrentGame.Game.Result;

            if (result == 10 || result == 20 || result == 30)
            {
                CurrentGame.Log.EndTime = DateTime.Now.ToString();
                CurrentGame.Log.Result = result;

                Database.GameLog.Add(CurrentGame.Log);
                Database.SaveChanges();

                Games.Remove(CurrentGame);
                return CurrentGame.Game.LastMov + result + 1;
            }

            return CurrentGame.Game.LastMov;
        }
    }
}