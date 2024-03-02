using System;
using System.Numerics;
using System.Threading.Channels;

namespace Magician
{

    class Player
    {
        private static int maxHelth = new Random().Next(50, 450);
        private int currentHelth = maxHelth;

        public void TakeDamage(int damage)
        {
            currentHelth -= damage;

            CheckDiePlayer();
        }

        public void CheckDiePlayer()
        {
            if (currentHelth <= 0)
            {
                Console.WriteLine("Герой пал от рук босса");
                Program.notIsDead = false;
            }
        }

        public void GetHelth(int helth)
        {
            currentHelth += helth;
        }

        public void SummonConstruct()
        {
            if (Program.disguiseSelf)
            {
                Console.WriteLine("Вы находитесь в масировке");
            }
            else
            {
                if (!(Program.construct))
                {
                    Program.countConstruct = 5;
                    Program.construct = true;
                    Console.WriteLine($"Вы призвали голема \n");
                }
                else
                {
                    Console.WriteLine("Вы не можете призвать больше чем 1 голема");
                }
            }
        }

        public void UsingWordlifeSpell()
        {
            if (!(Program.disguiseSelf))
            {
                int helth = new Random().Next(0, 100);
                GetHelth(helth);
                Console.WriteLine($"Вы востановили {helth} здоровья");
            }
            else
            {
                Console.WriteLine("Вы находитесь в масировке");
            }
        }

        public void ContractWithUnderworld()
        {
            if (!(Program.disguiseSelf))
            {
                if (currentHelth <= 100)
                {
                    int damage = new Random().Next(0, 100);
                    TakeDamage(damage);
                    Program.contractBonus = 100;
                    Console.WriteLine($"На поле битвы сложись плачевная ситуация и вы решили обратится к нижнему миру. " +
                                      $"Земля раздвигается и перед вами предстаёт красный высший демон. ");
                }
                else
                {
                    Console.WriteLine("Вы слишком здоровы");
                }
            }
            else
            {
                Console.WriteLine("Вы находитесь в масировке");
            }

        }

        public void ShowCurrentHelth()
        {
            Console.WriteLine(currentHelth);
        }

        public void DisguiseSelf()
        {
            if (!(Program.disguiseSelf))
            {
                Program.disguiseSelf = true;
                Program.countDisguiseSelf = 3;
            }
            else
            {
                Console.WriteLine("Вы уже замаскировались");
            }
        }
        public void GetOutOfDisguise()
        {
            Program.disguiseSelf = false;
            Program.countDisguiseSelf = 0;
        }

        public void CapturingACreature(Boss boss)
        {
            if (!(Program.disguiseSelf))
            {
                int chance = new Random().Next(0, 10);
                if (boss.TransmissionCurrentHelth() <= 30 && chance < 3)
                {
                    Console.WriteLine("Вы успешно захватили Босса");
                    Program.notIsDead = false;
                }
                else
                {
                    Console.WriteLine("Босс в ярости его удары стали ещё сильнее");
                    boss.СhangeIncreasedDamage(50);
                }
            }
            else
            {
                Console.WriteLine("Вы находитесь в масировке");

            }
        }
    }
    class Boss
    {
        private static int maxHelth = new Random().Next(150, 1000);
        private int currentHelth = maxHelth;
        private int increasedDamage = 25;

        public void ShowCurrentHelth()
        {
            Console.WriteLine(currentHelth);
        }
        public int TransmissionCurrentHelth()
        {
            return currentHelth;
        }
        public void СhangeIncreasedDamage(int plus)
        {
            increasedDamage += plus;
        }

        public void Attack(Player player)
        {
            if (!(Program.disguiseSelf))
            {
                int damage = new Random().Next(5, increasedDamage);
                Console.WriteLine($"Босс нанес {damage} урона");
                player.TakeDamage(damage);
                increasedDamage += 20;
                int chance = new Random().Next(0, 10);
                if (chance == 0)
                {
                    Console.WriteLine("Удар был настолько сильным, что вы ошемлены(вы пропускаете свой ход) ");
                    Program.notdaze = false;
                }
            }
            else
            {
                Console.WriteLine("Босс оглядывается по сторонам, но не находит вас");
            }

        }

        public void TakeDamage(int damage)
        {
            currentHelth -= damage;

            CheckDieBoss();
        }
        public void CheckDieBoss()
        {
            if (currentHelth <= 0)
            {
                Console.WriteLine("Босс был уничтожен");
                Program.notIsDead = false;
            }
        }
    }

    class Program
    {
        static bool firstRound = true;
        public static bool notIsDead = true;
        public static bool construct = false;
        public static int damageConstruct = 0;
        public static int countConstruct = 0;
        public static int contractBonus = 0;
        public static bool disguiseSelf = false;
        public static int countDisguiseSelf = 0;
        public static bool notdaze = true;
        static string input;

        static bool DefinitionOfTheFirstInUse()
        {
            int choice = new Random().Next(0, 2);
            if (choice == 0 && firstRound)
            {
                firstRound = false;
                return false;
            }
            firstRound = false;
            return true;
        }

        static void ShowHealth(Player player, Boss boss)
        {
            Console.WriteLine("Здоровье Босса: ");
            boss.ShowCurrentHelth();
            Console.WriteLine("Здоровье Игрока: ");
            player.ShowCurrentHelth();
            Console.WriteLine();
        }

        static void EffectsManager(Player player, Boss boss)
        {
            if (construct || disguiseSelf)
            {
                Console.WriteLine("Эффекты:");
            }
            if (countConstruct > 0)
            {
                damageConstruct = new Random().Next(0, 100);
                countConstruct--;
                if (countConstruct == 1)
                {
                    Console.WriteLine($"Голем нанес {damageConstruct} урона и пропадет в следуюшем раунде");
                }
                else if (countConstruct == 5)
                {
                    Console.WriteLine($"Голем нанес {damageConstruct} урона и продержится ещё {countConstruct} раундов");
                }
                else
                {
                    Console.WriteLine($"Голем нанес {damageConstruct} урона и продержится ещё {countConstruct} раунда");
                }
                boss.TakeDamage(damageConstruct + contractBonus);
                if (!(notIsDead))
                {
                    return;
                }

            }
            else if (construct)
            {
                construct = false;
            }
            if (countDisguiseSelf > 0)
            {
                countDisguiseSelf--;
                if (countDisguiseSelf == 1)
                {
                    Console.WriteLine($"Вы находитесь в масировки Босс вас не тронет, вы раскроете себя в следуюшем раунде");
                }
                else
                {
                    Console.WriteLine($"Вы находитесь в масировки Босс вас не тронет, это продлится ещё {countDisguiseSelf} раунда");
                }

            }
            else if (disguiseSelf)
            {
                disguiseSelf = false;
                Console.WriteLine("Вы больше не в маскировке");
            }
            if (!(notdaze))
            {
                notdaze = true;
            }
        }

        static void SpellOutputAndPlayerInputData()
        {
            Console.WriteLine("Ваш ход");
            Console.WriteLine("В вашем арсинале следующие закленания: \n"
                             + "* ксп (Контракт с преисподнеей-Вы подписываете контрак и теперь ваша мощь увеличилась(ваш урон увеличился на 100 единиц, но ваше здоровье снизилось в диапозоне от 0 до 100 единиц)) \n"
                             + "* сж (Слово жизнь-востоновите от 0 до 100 здоровья) \n"
                             + "* пг (Призыв голема-Призовите голема который в течений 5 ходов, будет сражатся на вашей стороне) \n"
                             + "* мас (Маскировка-Замаскируйте себя листвой и Босс вас не тронет на 3 хода(вы не сможите использовать закленания)) \n"
                             + "* немас (Снять с себя маскировку-Снемите с себя листву и получите возможность использовать закленания) \n"
                             + "* зс (Захват существа-Когда у Босс мало здоровья мы можите с шансом 30% захватить его в свои оковы и подчинить своей воле)");
            Console.WriteLine("Введите заклинание(его сокращение): ");
            input = Console.ReadLine().ToLower();
        }

        static void SpellSelections(Player player, Boss boss)
        {
            switch (input)
            {
                case "ксп": player.ContractWithUnderworld(); break;
                case "сж": player.UsingWordlifeSpell(); break;
                case "пг": player.SummonConstruct(); break;
                case "мас": player.DisguiseSelf(); break;
                case "немас": player.GetOutOfDisguise(); break;
                case "зс": player.CapturingACreature(boss); break;
                default: Console.WriteLine("Вы что-то бормочите под свой нос и ваши попытки сотворить какое-нибудь заклинание оказались четными"); break;
            }
        }

        static void Main()
        {
            Player player = new Player();
            Boss boss = new Boss();

            int count = 1;
            while (notIsDead)
            {
                Console.WriteLine($"Раунд {count}");
                if (DefinitionOfTheFirstInUse())
                {
                    ShowHealth(player, boss);
                    Console.WriteLine("Ход Босса");
                    boss.Attack(player);
                    if (!(notIsDead))
                    {
                        return;
                    }
                }
                ShowHealth(player, boss);
                if (notdaze)
                {
                    SpellOutputAndPlayerInputData();

                    SpellSelections(player, boss);
                    if (!(notIsDead))
                    {
                        return;
                    }
                }
                EffectsManager(player, boss);
                count++;
            }
        }
    }
}