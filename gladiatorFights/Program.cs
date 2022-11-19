using System.Runtime.CompilerServices;

class program
{
    static void Main(string[] args)
    {
        Barbarian barbarian = new Barbarian();
        Knight knight = new Knight();

        while (barbarian.IsAlive == true && knight.IsAlive == true)
        {
            barbarian.Attack(knight);

            knight.Attack(barbarian);
            knight.TryToActivateAbility(knight);

            knight.ShowInfo();
        }
    }
}

class Arena
{
    private Fighter[] _fighters =
    {
        new Barbarian(),
        new Knight(),
        new Glutton(),
        new Warrior(),
        new Assassin();
    }
}

class Fight
{
    private Fighter _fighterLeft;
    private Fighter _fighterRight;

}

abstract class Fighter
{
    protected Ability Ability;

    public string Name { get; protected set; }

    public int Armor { get; protected set; }
    public int BaseArmor { get; protected set; }

    public int Damage { get; protected set; }
    public int BaseDamage { get; protected set; }

    public int MaxHealth { get; protected set; }
    public int Health { get; protected set; }
    public bool IsAlive { get; protected set; }

    public void Attack(Fighter fighter)
    {
        Console.WriteLine($"{Name}: Атакую!");

        fighter.TakeDamage(Damage);
    }

    public void TakeDamage(int damage)
    {
        int currentDamage;

        currentDamage = damage * (100 - Armor) / 100;
        Health -= currentDamage;

        IsAlive = Health > 0;

        if (IsAlive)
        {
            Console.WriteLine($"{Name} получает урон в размере {currentDamage} единиц.");
        }
        else
        {
            Health = 0;

            Console.WriteLine($"{Name} убит!");

            IsAlive = false;
        }
    }

    public void TryToActivateAbility(Fighter fighter)
    {
        Ability.OnTurn(fighter);
    }

    public void ChangeMaxHealth(int maxHealth)
    {
        MaxHealth = maxHealth;
    }

    public void ChangeHealth(int health)
    {
        Health += health;

        if (Health > 0)
            Health = MaxHealth;
    }

    public void ChangeDamage(int damage)
    {
        Damage = damage;
    }

    public void ChangeArmor(int armor)
    {
        Armor = armor;
    }

    public void ShowInfo()
    {
        Console.WriteLine($"||Класс бойца: {Name} | Запас здоровья: {MaxHealth} | Броня: {Armor} | Урон: {Damage} | Способность: {Ability.Name}||");
    }
}

class Barbarian : Fighter
{

    public Barbarian()
    {
        Ability = new StoneSkin();
        Name = "Варвар";
        MaxHealth = 150;
        Health = MaxHealth;
        BaseArmor = 3;
        Armor = BaseArmor;
        BaseDamage = 33;
        Damage = BaseDamage;
        IsAlive = true;
    }
}

class Knight : Fighter
{
    public Knight()
    {
        Ability = new Healing();
        Name = "Рыцарь";
        MaxHealth = 250;
        Health = MaxHealth;
        BaseArmor = 10;
        Armor = BaseArmor;
        BaseDamage = 10;
        Damage = BaseDamage;
        IsAlive = true;
    }
}

class Glutton : Fighter
{

    public Glutton()
    {
        Ability = new FatMan();
        Name = "Обжора";
        MaxHealth = 150;
        Health = MaxHealth;
        BaseArmor = 2;
        Armor = BaseArmor;
        BaseDamage = 30;
        Damage = BaseDamage;
        IsAlive = true;
    }
}

class Warrior : Fighter
{
    public Warrior()
    {
        Ability = new Berserk();
        Name = "Воин";
        MaxHealth = 200;
        Health = MaxHealth;
        BaseArmor = 8;
        Armor = BaseArmor;
        BaseDamage = 25;
        Damage = BaseDamage;
        IsAlive = true;
    }
}

class Assassin : Fighter
{
    public Assassin()
    {
        Ability = new DoubleDamage();
        Name = "Убийца";
        MaxHealth = 130;
        Health = MaxHealth;
        BaseArmor = 7;
        Armor = BaseArmor;
        BaseDamage = 35;
        Damage = BaseDamage;
        IsAlive = true;
    }
}

abstract class Ability
{
    protected int ChanceOfTriggering;
    protected Random Random = new Random();

    public string Name { get; protected set; }

    public virtual void OnTurn(Fighter fighter)
    {
        if (ShouldTrigger(fighter))
            Trigger(fighter);
    }

    protected int GetRandomNumber()
    {
        int minimumRandomNumber = 0;
        int maximumRandomNumber = 100;

        return Random.Next(minimumRandomNumber, maximumRandomNumber);
    }

    protected virtual bool ShouldTrigger(Fighter fighter)
    {
        int resultNumber = GetRandomNumber();

        return resultNumber <= ChanceOfTriggering;
    }

    protected abstract void Trigger(Fighter fighter);
}

class Healing : Ability
{
    private int _minimumHealingPoint = 15;
    private int _maximumHealingPoint = 26;

    public Healing()
    {
        Name = "Исцеление";
        ChanceOfTriggering = 10;
    }

    protected override void Trigger(Fighter fighter)
    {
        fighter.ChangeHealth(Random.Next(_minimumHealingPoint, _maximumHealingPoint));
    }
}

class StoneSkin : Ability
{
    private int _armorBoost = 10;
    private int _amountOfMoves = 3;
    private int _moveCounter = 0;
    private bool _isActive = false;

    public StoneSkin()
    {
        Name = "Каменная кожа";
        ChanceOfTriggering = 15;
    }

    public override void OnTurn(Fighter fighter)
    {
        if (_moveCounter == _amountOfMoves)
            OffTrigger(fighter);

        if (_isActive)
        {
            _moveCounter++;
        }
        else
        {
            base.OnTurn(fighter);
        }
    }

    protected override void Trigger(Fighter fighter)
    {
        fighter.ChangeArmor(fighter.Armor + _armorBoost);
    }

    private void OffTrigger(Fighter fighter)
    {
        _isActive = false;
        _moveCounter = 0;

        fighter.ChangeArmor(fighter.BaseArmor);
    }
}

class FatMan : Ability
{
    private int _thresholdHealthForTriggering = 30;
    private int _boostMaxHealth = 500;
    private int _healingHealth;
    private int _damage = 5;
    private int _armor = 1;

    public FatMan()
    {
        Name = "Толстяк";
        _healingHealth = _boostMaxHealth;
    }

    protected override bool ShouldTrigger(Fighter fighter)
    {
        return fighter.Health <= _thresholdHealthForTriggering;
    }

    protected override void Trigger(Fighter fighter)
    {
        fighter.ChangeMaxHealth(_boostMaxHealth);
        fighter.ChangeHealth(_healingHealth);
        fighter.ChangeDamage(_damage);
        fighter.ChangeArmor(_armor);
    }
}

class Berserk : Ability
{
    private int _armorBoost = 10;
    private int _damageBoost = 10;
    private int _thresholdHealthForTriggering = 20;

    public Berserk()
    {
        Name = "Берсерк";
    }

    protected override bool ShouldTrigger(Fighter fighter)
    {
        return fighter.Health <= _thresholdHealthForTriggering;
    }

    protected override void Trigger(Fighter fighter)
    {
        fighter.ChangeArmor(_armorBoost + fighter.Armor);
        fighter.ChangeDamage(_damageBoost + fighter.Damage);
    }
}

class DoubleDamage : Ability
{
    private int _damageMultiplier = 2;
    private bool _isActive = false;

    public DoubleDamage()
    {
        Name = "Двойной урон";
        ChanceOfTriggering = 25;
    }

    public override void OnTurn(Fighter fighter)
    {
        if (_isActive)
        {
            OffTrigger(fighter);
        }
        else
        {
            base.OnTurn(fighter);
        }
    }

    protected override void Trigger(Fighter fighter)
    {
        _isActive = true;

        fighter.ChangeDamage(fighter.Damage * _damageMultiplier);
    }

    private void OffTrigger(Fighter fighter)
    {
        _isActive = false;

        fighter.ChangeDamage(fighter.BaseDamage);
    }
}