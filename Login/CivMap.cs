using ConsoleGameLib.CoreTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login
{
    public class CivMap
    {
        public Civilization Player = new Civilization();
        public List<Civilization> NPCs = new List<Civilization>();
        public bool ShouldQuit = false;
        Random rand = new Random();
        public int Turn = 0;
        public void Update()
        {
            Turn++;
            
            
            Player.Update();
            Player.Draw();
            foreach(Civilization npc in NPCs)
            {
                Console.SetCursorPosition(0, 12 + NPCs.IndexOf(npc));
                Console.Write($"{npc.Name}\t| Money: ${npc.Money}\tSoldiers: {npc.Soldiers}\tCitizen Happiness: {Math.Round(npc.JobApproval * 100)}%\tDiplomatic Status: {npc.WarStatus.ToString()}\tMoney Owed: ${npc.MoneyOwed}");

            }
            Console.SetCursorPosition(0,20);
            Console.Write($"Turn: {Turn}");

            foreach(Civilization npc in NPCs)
            {
                if(npc.MoneyOwed > 0)
                {
                    npc.MoneyOwed -= 60;
                    npc.Money += 60;
                    Player.Money -= 60;
                }
            }
            

            while (!Console.KeyAvailable)
            {

            }
            ConsoleKeyInfo pressedKey = Console.ReadKey(true);
            if (pressedKey.KeyChar == 'b' && Player.Money >= 125)
            {
                Player.Constructions.Add(new Barracks());
                Player.Money -= 125;
            }
            else if(pressedKey.KeyChar == 'f' && Player.Money >= 25)
            {
                Player.Constructions.Add(new Farm());
                Player.Money -= 25;
            }
            else if((byte)pressedKey.KeyChar >= 49 && (byte)pressedKey.KeyChar <= 57 && (byte)pressedKey.KeyChar - 49 < NPCs.Count)
            {
                if(NPCs[(byte)pressedKey.KeyChar - 49].WarStatus != CivStatus.War)
                {
                    
                    Console.SetCursorPosition(0, 9);
                    Console.Write($"{Player.Name} declared war on {NPCs[(byte)pressedKey.KeyChar - 49].Name}!");
                }

                Player.WarStatus = CivStatus.War;
                NPCs[(byte)pressedKey.KeyChar - 49].WarStatus = CivStatus.War;

                Battle(Player,NPCs[(byte)pressedKey.KeyChar - 49]);
            }
            else if (pressedKey.Modifiers == ConsoleModifiers.Shift && (byte)pressedKey.Key >= 49 && (byte)pressedKey.Key <= 57 && (byte)pressedKey.Key - 49 < NPCs.Count)
            {
              
                if (NPCs[(byte)pressedKey.Key - 49].WarStatus != CivStatus.War && NPCs[(byte)pressedKey.Key - 49].MoneyOwed <= 3000)
                {
                    Player.Money += 1000;
                    NPCs[(byte)pressedKey.Key - 49].MoneyOwed += 1200;
                    NPCs[(byte)pressedKey.Key - 49].Money -= 1000;
                    Console.SetCursorPosition(0, 9);
                    Console.Write($"{Player.Name} borrowed $1000 from {NPCs[(byte)pressedKey.Key - 49].Name} and will return $1200 to them over 20 turns, beginning next turn.");
                }
                System.Threading.Thread.Sleep(3000);
            }
            else if(pressedKey.Key == ConsoleKey.Escape)
            {
                ShouldQuit = true;
            }
            
            foreach(Civilization npc in NPCs)
            {
                npc.Update();
                int selection = rand.Next(0,5);
                if (selection >= 0 && selection <= 1)
                {
                    if (npc.Money >= 25)
                    {
                        npc.Money -= 25;
                        npc.Constructions.Add(new Farm());
                    }
                }
                else if (selection >= 2 && selection <= 3)
                {
                    if (npc.Money >= 125)
                    {
                        npc.Money -= 125;
                        npc.Constructions.Add(new Barracks());
                    }
                }
                else if (selection == 4)
                {
                    if (npc.Soldiers > Player.Soldiers && Player.Money > npc.Money && rand.Next(0, 6) == 0)
                    {
                        if (npc.WarStatus != CivStatus.War)
                        {
                            Console.SetCursorPosition(0, 9);
                            Console.Write($"{npc.Name} declared war on {Player.Name}!");
                        }

                        npc.WarStatus = CivStatus.War;
                        Player.WarStatus = CivStatus.War;

                            Battle(Player, npc);
                        
                        
                        
                    }
                }

                }
        }

        public CivMap(int npcs)
        {
            Player.IsPlayer = true;
            Player.Name = "Player";
            for(int i = 0; i < npcs; i++)
            {
                NPCs.Add(new Civilization());
                NPCs[i].Name = $"NPC #{i+1}";
            }
        }

        public void Battle(Civilization a, Civilization b)
        {

            
            int aLost = 0;
            int bLost = 0;

            
            int min = a.Soldiers >= 1 && b.Soldiers >= 1 ? 1 : 0;
            int max = a.Soldiers > b.Soldiers ? b.Soldiers : a.Soldiers;
            int fights = rand.Next(min, max + 1);
            for(int i = 0; i < fights; i++)
            {
                int aAttack = rand.Next(1, 7);
                int bAttack = rand.Next(1, 7);
                if (aAttack > bAttack)
                {
                    b.Soldiers--;
                    bLost++;
                }
                else if (aAttack == bAttack)
                {

                }
                else
                {
                    aLost++;
                    a.Soldiers--;
                }
            }

            if(a.Soldiers < 0)
            {
                a.Soldiers = 0;
            }
            if(b.Soldiers < 0)
            {
                b.Soldiers = 0;
            }

            int greaterCost = rand.Next(500, 1501);
            int lesserCost = rand.Next(-500, 801);

            int aCost = a.Soldiers > b.Soldiers ? lesserCost : greaterCost;
            int bCost = b.Soldiers > a.Soldiers ? lesserCost : greaterCost;

            a.Money -= aCost;
            b.Money -= bCost;

            Console.SetCursorPosition(0, 10);
            Console.Write($"A battle occurred between {a.Name} and {b.Name}. {a.Name} {(aCost >= 0 ? "lost" : "gained")} ${Math.Abs(aCost)} and lost {aLost} soldiers. {b.Name} {(bCost >= 0 ? "lost" : "gained")} ${Math.Abs(bCost)} and lost {bLost} soldiers.");
            System.Threading.Thread.Sleep(3000);
            while (Console.KeyAvailable)
            {
                Console.ReadKey(true);
            }
        }
    }
}
