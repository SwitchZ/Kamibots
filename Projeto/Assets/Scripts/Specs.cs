public enum KamibotType 
{
    Mantial,
    Sliceknight,
    Metaman,
    Aquasniper,
    Kinderbot
}



public class Specs : Attributes
{
    public static Specs FromType(KamibotType type)
    {
        Specs factorySpecs = new Specs(); //auxiliar variable for switch-case below

        switch (type) //define general bonuses for the player, based on their type
        {
            case KamibotType.Mantial: 
                factorySpecs = new Specs()
                {
                    alterAttackRange = 2,
                    alterDamageReduction = 0,
                    alterDamageBase = 4,
                    alterMovementRange = 4
                };
                break;

            case KamibotType.Sliceknight: 
                factorySpecs = new Specs()
                {
                    alterAttackRange = 1,
                    alterDamageReduction = 0,
                    alterDamageBase = 6,
                    alterMovementRange = 3
                };
                break;

            case KamibotType.Metaman: 
                factorySpecs = new Specs()
                {
                    /*
                    alterAttackRange = 5,
                    alterDamageReduction = 0,
                    alterDamageBase = 2,
                    alterMovementRange = 5
                    */

                    alterAttackRange = 0,
                    alterDamageReduction = 0,
                    alterDamageBase = 0,
                    alterMovementRange = 5
                };
                break;

            case KamibotType.Aquasniper: 
                factorySpecs = new Specs()
                {
                    alterAttackRange = 5,
                    alterDamageReduction = 0,
                    alterDamageBase = 3,
                    alterMovementRange = 2
                };
                break;

            case KamibotType.Kinderbot: 
                factorySpecs = new Specs()
                {
                    alterAttackRange = 1,
                    alterDamageReduction = 0,
                    alterDamageBase = 3,
                    alterMovementRange = 6
                };
                break;

        }
        return factorySpecs;
    }
}