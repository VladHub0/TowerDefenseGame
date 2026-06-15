namespace CustomEventBus.Signals
{
    public class EnemyReachedCastleSignal
    {
        public int DamageAmount { get; }

        public EnemyReachedCastleSignal(int damageAmount)
        {
            DamageAmount = damageAmount;
        }
    }
}