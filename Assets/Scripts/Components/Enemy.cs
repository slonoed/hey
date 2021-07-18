using Leopotam.Ecs;

namespace YANTH {
    public enum EnemyState {
        Invalid = 0,
        Roam,
        Fight,
        Wait,
        Death,
    }

    struct Enemy : IEcsAutoReset<Enemy> {
        public string name;
        public float speed;
        public EnemyState state;
        public float lastHitTime;
        public int attack;
        public float attackDelay;

        public void AutoReset(ref Enemy e) {
            e.state = EnemyState.Wait;
        }
    }
}