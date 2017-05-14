using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Login
{
    public class Civilization
    {
        public CivStatus WarStatus = CivStatus.Peace;
        public TradeStatus TradingStatus = TradeStatus.Open;

        public int Money = 100;
        public float JobApproval = 1f;

        public float TopPersonalWealth= 1f;

        public int MoneyOwed = 0;


        public string Name;

        Random rand = new Random();

        public bool IsPlayer = false;


        private int oldSoldiers;

        public List<IConstruction> Constructions = new List<IConstruction>();

        public int MaximumSoldiers
        {
            get
            {
                int maxSoldiers = 0;
                foreach (IConstruction construction in Constructions)
                {
                    if (construction.GetType() == typeof(Barracks))
                    {
                        maxSoldiers += 100 * (construction.Level + 1);
                    }
                }
                return maxSoldiers;
            }
        }

        private int soldiers;

        public int Soldiers
        {
            get { return soldiers; }
            set
            {
                if (value <= MaximumSoldiers)
                {
                    soldiers = value;
                }
                else
                {
                    soldiers = MaximumSoldiers;
                    //throw new Exception("The number of soldiers must be less than or equal to the maximum number of soldiers.");
                }
            }
        }

        public void Update()
        {

            foreach (IConstruction construction in Constructions)
            {
                if (construction.GetType() == typeof(Farm))
                {
                    Money += 2 * (construction.Level + 1);
                }
                else if (construction.GetType() == typeof(Barracks))
                {
                    Money -= 2;
                    if (Soldiers < MaximumSoldiers)
                    {
                        if (rand.Next(0, 5) == 0)
                        {
                            Soldiers++;
                        }
                    }
                }
            }

            if (rand.Next(1, 251) == 1)
            {
                Disaster disaster = (Disaster)rand.Next(1,4);

                Console.SetCursorPosition(0, 10);
                int cost = rand.Next(300, 1001);
                int dT = rand.Next(20, 401);
                int destroyedBuildings = rand.Next(0, Constructions.Count/2 + 1);

                for(int i = 0; i < destroyedBuildings; i++)
                {
                    Constructions.Remove(Constructions[rand.Next(0,Constructions.Count)]);
                }

                Money -= cost;
                soldiers = Math.Max(soldiers - dT, 0);


                if (IsPlayer)
                {
                    Console.Write($"{disaster.ToString()}! ${cost} in damages! {dT} killed! {destroyedBuildings} buildings destroyed!");

                    Thread.Sleep(3000);
                    while (Console.KeyAvailable)
                    {
                        Console.ReadKey(true);
                    }
                }
            }

            if(Soldiers != oldSoldiers)
            {
                TopPersonalWealth = 1f;
            }

            if (Soldiers != 0 && MaximumSoldiers != 0)
            {
                if (Money / Soldiers > TopPersonalWealth)
                {
                    TopPersonalWealth = Money / Soldiers;
                }

                JobApproval = Mean((float)Money / (float)Soldiers / TopPersonalWealth, (float)Money / (float)Soldiers / TopPersonalWealth, (float)Soldiers / (float)MaximumSoldiers);
                JobApproval = Math.Max(JobApproval, 0f);
                JobApproval = Math.Min(JobApproval, 1f);
            }



            if (rand.Next(0, (int)((1f - JobApproval) * 100 + 1)) >= 85 && Soldiers > 5)
            {
                int destroyedBuildings = rand.Next(0, Constructions.Count / 2 + 1);

                for (int i = 0; i < destroyedBuildings; i++)
                {
                    Constructions.Remove(Constructions[rand.Next(0, Constructions.Count)]);
                }
                if (IsPlayer)
                {
                    Console.SetCursorPosition(0, 10);
                    Console.WriteLine($"Revolt! {destroyedBuildings} buildings destroyed!");
                    Thread.Sleep(3000);
                    while (Console.KeyAvailable)
                    {
                        Console.ReadKey(true);
                    }
                }
            }
            




            oldSoldiers = Soldiers;
        }

        public float Mean(params float[] val)
        {
            float sum = 0f;
            foreach(float num in val)
            {
                sum += num;
            }
            return sum / val.Length;
        }

        public void Draw()
        {
            Console.Clear();

            Console.SetCursorPosition(0, 0);
            Console.Write($"Money: ${Money}\tSoldiers: {Soldiers}\tCitizen Happiness: {JobApproval * 100}%\tDiplomatic Status: {WarStatus.ToString()}");
            Console.SetCursorPosition(0, 2);
            Console.Write("Farms:    ");
            foreach (IConstruction constr in Constructions)
            {
                if (constr.GetType() == typeof(Farm))
                {
                    Console.Write('@');
                }
            }
            Console.WriteLine("");
            
            
            Console.Write("Barracks: ");
            foreach (IConstruction constr in Constructions)
            {
                if (constr.GetType() == typeof(Barracks))
                {
                    Console.Write('@');
                }
            }
            Console.WriteLine("");



        }


    }
}
